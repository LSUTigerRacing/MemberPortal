using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Supabase;
using Scalar.AspNetCore;
using TRFSAE.MemberPortal.API.Authorization;
using TRFSAE.MemberPortal.API.Enums;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Services;
using dotenv.net;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<ITaskService, TaskService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IPermissionService, PermissionService>();
    // builder.Services.AddScoped<IGoogleSheetsService, GoogleSheetsService>();

    builder.Services.AddSingleton<IAuthorizationHandler, MinimumRoleHandler>();
}

// register Supabase client as scoped for reuse across project
builder.Services.AddScoped(provider =>
{
    var options = new SupabaseOptions
    {
        AutoConnectRealtime = true,
        AutoRefreshToken = true,
    };

    var url = builder.Configuration["SupabaseUrl"] ?? throw new InvalidOperationException("Supabase URL is not configured.");
    var key = builder.Configuration["SupabaseKey"] ?? throw new InvalidOperationException("Supabase Key is not configured.");

    var client = new Client(url, key, options);

    // Synchronously initialize the client
    client.InitializeAsync().GetAwaiter().GetResult();

    return client;
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var supabaseUrl = builder.Configuration["SupabaseUrl"];
        var jwtSecret = builder.Configuration["SupabaseJwtSecret"];

        // JwtSecurityTokenHandler remaps short JWT claim names (e.g. "sub")
        // to long WS-Federation URIs by default. OnTokenValidated below and
        // PermissionService both look up FindFirst("sub") verbatim, so
        // without this, that lookup always misses and no caller - not even
        // SuperAdmin - ever gets a role/isFinance claim attached. Confirmed
        // via live testing: every AA policy denied every role identically.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{supabaseUrl}/auth/v1",

            ValidateAudience = false,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtSecret!)
            ),

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            // The frontend never sends an Authorization header — AuthController's
            // /api/auth/callback stores the Supabase access token in an httpOnly
            // "access_token" cookie, and API.ts calls with withCredentials: true.
            // Pull the token from that cookie so [Authorize] has something to check.
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("access_token", out var token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },

            // The Supabase JWT only proves identity — it knows nothing about our
            // app-level `role`/`isFinance` columns. Look the user up once per
            // request and attach them as claims so policy handlers and
            // IPermissionService can read them without a DB round-trip each time.
            OnTokenValidated = async context =>
            {
                var userIdClaim = context.Principal?.FindFirst("sub")?.Value;
                if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return;
                }

                var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                var user = await userService.GetUserAsync(userId);

                if (user != null && context.Principal?.Identity is ClaimsIdentity identity)
                {
                    identity.AddClaim(new Claim("role", user.Role.ToString()));
                    identity.AddClaim(new Claim("isFinance", user.IsFinance.ToString()));
                }
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MemberAA", policy => policy.Requirements.Add(new MinimumRoleRequirement(Role.Member)));
    options.AddPolicy("SubsystemLeadAA", policy => policy.Requirements.Add(new MinimumRoleRequirement(Role.SubsystemLead)));
    options.AddPolicy("AdminAA", policy => policy.Requirements.Add(new MinimumRoleRequirement(Role.Admin)));
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// CORS stuff
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSvelteApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowSvelteApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// using (var scope = app.Services.CreateScope())
// {
//     var googleSheetsService = scope.ServiceProvider.GetRequiredService<IGoogleSheetsService>();

//     // Initialize Google Sheets API once
//     await googleSheetsService.ListenToSupabaseChangesAsync();
// }

app.Run();
