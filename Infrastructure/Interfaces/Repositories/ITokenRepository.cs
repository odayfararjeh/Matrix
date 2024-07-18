using DTO.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task<string> GetUserAccessTokenAsync(int userId);
        Task<bool> AddUserAccessTokenAsync(AccessTokenDto model);
        Task<bool> DeleteUserAccessTokenAsync(AccessTokenDto model);
    }
}
