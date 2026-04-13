using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Interfaces;
using TRFSAE.MemberPortal.API.Models;
using TRFSAE.MemberPortal.API.Enums;
using System.Text.Json;
using Supabase;
using Supabase.Gotrue;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.CodeDom;

namespace TRFSAE.MemberPortal.API.Services;

public class UserService : IUserService
{
    private readonly Supabase.Client _supabaseClient;

    public UserService(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync()
    {
        var response = await _supabaseClient
            .From<UserModel>()
            .Order(u => u.CreatedAt, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Select("id,name,email,gradDate,subsystem")
            .Get();

        var userSummaries = response.Models.Select(u => new UserSummaryDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            GradYear = u.GradYear,
            Subsystem = u.Subsystem
        });

        return userSummaries;
    }

    public async Task<UserDetailDto> GetUserAsync(Guid id)
    {
        var response = await _supabaseClient
          .From<UserModel>()
          .Select("*")
          .Where(x => x.Id == id)
          .Single();

        if (response == null)
        {
            Console.write("User not found");
            return null;
        }

        var userDetail = new UserDetailDto
        {
            Id = response.Id,
            Name = response.Name,
            Email = response.Email,
            GradYear = response.GradYear,
            Subsystem = response.Subsystem
        };

        return userDetail;
    }

     public async Task<CurrentUserDto?> GetCurrentUserAsync(Guid id)
    {
        var response = await _supabaseClient
            .From<UserModel>()
            .Select("id,name,email,system,subsystem,shirtSize,hazingStatus,feeStatus,gradYear,createdAt")
            .Where(x => x.Id == id)
            .Get();

        if (response == null)
        {
            Console.Write("User not found or not signed in."); 
            return null;
        }

        var user = response.Models.FirstOrDefault();

        if (user == null)
        {
            return null;
        }

        var currentUserDetail = new CurrentUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            GradYear = user.GradYear,
            System = user.System,
            Subsystem = user.Subsystem,
            ShirtSize = user.ShirtSize,
            HazingStatus = user.HazingStatus,
            FeeStatus = user.FeeStatus
        };

        return currentUserDetail;
    }

    public async Task<bool> CreateUserAsync(CreateUserDto createDto)
    {
        var userId = Guid.NewGuid();

        var newUser = new UserModel
        {
            Id = userId,
            Name = createDto.Name,
            Email = createDto.Email,
            Role = createDto.Role,
            StudentId = createDto.StudentId,
            HazingStatus = createDto.HazingStatus,
            FeeStatus = createDto.FeeStatus,
            GradYear = createDto.GradYear,
            ShirtSize = createDto.ShirtSize,
            Subsystem = createDto.Subsystem,
            CreatedAt = DateTime.UtcNow,
        };

        try
        {
            var response = await _supabaseClient
                .From<UserModel>()
                .Insert(newUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
            return false;
        }

        return true;
    }


    public async Task<bool> UpdateUserAsync(Guid id, UserUpdateDto updateDto)
    {
        try
        {
            var response = await _supabaseClient
                .From<UserModel>()
                .Where(u => u.Id == id)
                .Get();

            var model = response.Models.FirstOrDefault();

            if (model == null)
            {
                Console.WriteLine($"User with ID {id} not found");
                return false;
            }
            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                model.Name = updateDto.Name;
            }
            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                model.Email = updateDto.Email;
            }
            if (updateDto.Role.HasValue)
            {
                model.Role = updateDto.Role.Value;
            }
            if (updateDto.StudentId.HasValue)
            {
                model.StudentId = updateDto.StudentId.Value;
            }
            if (updateDto.HazingStatus.HasValue)
            {
                model.HazingStatus= updateDto.HazingStatus.Value;
            }
            if (updateDto.FeeStatus.HasValue)
            {
                model.FeeStatus = updateDto.FeeStatus.Value;
            }
            if (updateDto.GradYear.HasValue)
            {
                model.GradYear = updateDto.GradYear.Value;
            }
            if (updateDto.ShirtSize.HasValue)
            {
                model.ShirtSize = updateDto.ShirtSize.Value;
            }
            if (updateDto.Subsystem.HasValue)
            {
                model.Subsystem = updateDto.Subsystem.Value;
            }
            model.UpdatedAt = DateTime.UtcNow;

            var updateResponse = await _supabaseClient
                .From<UserModel>()
                .Where(u => u.Id == id)
                .Update(model);

            var updatedUser = updateResponse.Models.FirstOrDefault();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error attempting update: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        try
        {
            await _supabaseClient
              .From<UserModel>()
              .Where(x => x.Id == id)
              .Delete();

            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }
}
