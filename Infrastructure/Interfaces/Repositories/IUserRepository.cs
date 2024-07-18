using DTO.IdentityDtos;

namespace Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserAsync(string userName);
        Task<bool> UpdateUserLastLoginDateAsync(UserDto model);
        Task<bool> UpdateUserPasswordAsync(UserDto model);
    }
}
