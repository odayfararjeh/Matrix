using Dapper;
using DTO.IdentityDtos;
using Infrastructure.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Repository.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<UserDto> GetUserAsync(string userName)
        {
            try
            {
                string commandTxt = @"
                            Select 
                                * 
                            FROM 
                                dbo.[User] 
                            WHERE 
                                UserName = @UserName
                        ";

                var commandParam = new
                {
                    UserName = userName
                };

                UserDto result;
                using (var _dbConn = new SqlConnection(_connectionString))
                {
                    result = await _dbConn.QueryFirstOrDefaultAsync<UserDto>(commandTxt, commandParam);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUserLastLoginDateAsync(UserDto model)
        {
            try
            {
                string commandTxt = @"
                            UPDATE 
                                dbo.[User] 
                            SET 
                                LastLoginDate = @LastLoginDate
                            WHERE 
                                ID = @UserId
                        ";

                var commandParam = new
                {
                    UserId = model.ID,
                    LastLoginDate = model.LastLoginDate
                };

                int result;
                using (var _dbConn = new SqlConnection(_connectionString))
                {
                    result = await _dbConn.ExecuteAsync(commandTxt, commandParam);
                }

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUserPasswordAsync(UserDto model)
        {
            try
            {
                string commandTxt = @"
                            UPDATE 
                                dbo.[User] 
                            SET 
                                Password = @Password
                            WHERE 
                                ID = @UserId
                        ";

                var commandParam = new
                {
                    UserId = model.ID,
                    Password = model.Password
                };

                int result;
                using (var _dbConn = new SqlConnection(_connectionString))
                {
                    result = await _dbConn.ExecuteAsync(commandTxt, commandParam);
                }

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
