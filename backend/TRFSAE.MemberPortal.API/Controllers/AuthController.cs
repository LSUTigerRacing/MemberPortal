using Microsoft.AspNetCore.Mvc;
using Supabase;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Models;

namespace TRFSAE.MemberPortal.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly Client _supabaseClient;

    public AuthController(IAuthService authService, Client supabaseClient)
    {
        _authService = authService;
        _supabaseClient = supabaseClient;
    }

    [HttpGet("google")]
    public async Task<IActionResult> GoogleLogin()
    {
        var callbackUrl = "http://localhost:5096/api/auth/callback";
        var verifierBytes = new byte[32];
        System.Security.Cryptography.RandomNumberGenerator.Fill(verifierBytes);
        var pkceVerifier = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(verifierBytes);

        var challengeBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(pkceVerifier));
        var pkceChallenge = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(challengeBytes);

        var options = new Supabase.Gotrue.SignInOptions
        {
            RedirectTo = callbackUrl,
            QueryParams = new Dictionary<string, string>
            {
                { "code_challenge", pkceChallenge },
                { "code_challenge_method", "s256" }
            }
        };

        var providerState = await _supabaseClient.Auth.SignIn(Supabase.Gotrue.Constants.Provider.Google, options);

        Response.Cookies.Append("pkce_verifier", pkceVerifier, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps, // Set to true in production
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(5)
        });

        return Redirect(providerState.Uri.ToString());
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code)) return BadRequest("Missing authorization code.");

        if (!Request.Cookies.TryGetValue("pkce_verifier", out var pkceVerifier))
        {
            return BadRequest("PKCE verifier missing or expired. Please try logging in again.");
        }

        try
        {
            var session = await _supabaseClient.Auth.ExchangeCodeForSession(pkceVerifier, code);

            Response.Cookies.Append("access_token", session.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddSeconds(session.ExpiresIn)
            });

            Response.Cookies.Delete("pkce_verifier");

            await _authService.SyncUserToDatabase(session.AccessToken);

            return Redirect("http://localhost:3000");        
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Authentication failed: " + ex.Message);
        }
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        if (!Request.Cookies.TryGetValue("access_token", out var token))
        {
            return Unauthorized();
        }

        if (!_authService.ValidateSupabaseToken(token)) return Unauthorized();

        var user = await _authService.GetUserFromToken(token);
        if (user == null) return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _supabaseClient.Auth.SignOut();
        }
        catch
        {
            //ensures that cookie is deleted even when waiting for supabase
        }

        Response.Cookies.Delete("access_token");
        return Ok(new { message = "Successfully logged out." });
    }

// AI-Given test url- delete later
    [HttpGet("test-auth")]
    public IActionResult TestAuth()
    {
        // A simple endpoint to visually verify the cookie is working 
        // without needing the Svelte frontend running.
        if (Request.Cookies.TryGetValue("access_token", out var token))
        {
            return Ok(new 
            { 
                status = "Authenticated ✅", 
                message = "The HttpOnly cookie was successfully set and attached to this request!",
                // Print just the start of the token to prove it's there without exposing the whole thing
                tokenPrefix = token.Substring(0, 15) + "..." 
            });
        }

        return Unauthorized(new 
        { 
            status = "Unauthenticated ❌", 
            message = "No access_token cookie found. You are not logged in." 
        });
    }
}