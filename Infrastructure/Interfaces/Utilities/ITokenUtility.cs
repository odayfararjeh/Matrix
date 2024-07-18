using DTO.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Utilities
{
    public interface ITokenUtility
    {
        Task<string> GenerateAccessTokenAsync(UserDto user);
        int GetUserIdFromAccessToken(string accessToken);
        bool IsTokenExpired(string token);
        Task<bool> DeleteUserAccessTokenAsync(int userId);
    }
}
