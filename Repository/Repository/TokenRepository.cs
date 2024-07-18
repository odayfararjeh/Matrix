using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.IdentityDtos;
using Infrastructure.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;

namespace Repository.Repository
{
    public class TokenRepository : BaseRepository, ITokenRepository
    {
        public TokenRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<string> GetUserAccessTokenAsync(int userId)
        {
            try
            {
                string commandTxt = @"
                            SELECT 
                                AccessToken 
                            FROM 
                                dbo.[User]  
							WHERE
								ID = @UserID
                            AND
                                IsDeleted = CAST(0 AS BIT)
                        ";

                var commandParam = new
                {
                    UserID = userId
                };

                string result;
                using (var _dbConn = new SqlConnection(_connectionString))
                {
                    result = await _dbConn.QueryFirstOrDefaultAsync<string>(commandTxt, commandParam);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddUserAccessTokenAsync(AccessTokenDto model)
        {
            try
            {
                string commandTxt = @"
                            UPDATE 
                                dbo.[User]  
                            SET
                                AccessToken = @AccessToken
							WHERE
								ID = @UserID
                        ";

                var commandParam = new
                {
                    UserID = model.UserID,
                    AccessToken = model.AccessToken,
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

        public async Task<bool> DeleteUserAccessTokenAsync(AccessTokenDto model)
        {
            try
            {
                string commandTxt = @"
                            UPDATE 
                                dbo.[User]  
                            SET
                                AccessToken = NULL
							WHERE
								ID = @UserID
                        ";

                var commandParam = new
                {
                    UserID = model.UserID
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
