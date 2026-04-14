using TRFSAE.MemberPortal.API.DTOs;
using TRFSAE.MemberPortal.API.Enums;

namespace TRFSAE.MemberPortal.API.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync();
    Task<UserDetailDto> GetUserAsync(Guid id);
    Task<bool> CreateUserAsync(CreateUserDto createDto);
    Task<bool> UpdateUserAsync(Guid userID, UserUpdateDto updateDto);
    Task<bool> DeleteUserAsync(Guid id);
}
