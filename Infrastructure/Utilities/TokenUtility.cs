using Infrastructure.Interfaces.Utilities;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Constants;
using Common.Enums;
using DTO.IdentityDtos;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Utilities
{
    public class TokenUtility : ITokenUtility
    {
        private readonly JWTSettings _jwtsettings;
        private readonly ITokenRepository _tokenRepository;

        public TokenUtility(IOptions<JWTSettings> jwtsettings,
                            ITokenRepository tokenRepository)
        {
            _jwtsettings = jwtsettings.Value;
            _tokenRepository = tokenRepository;
        }

        public async Task<string> GenerateAccessTokenAsync(UserDto user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, Convert.ToString(user.UserName)),
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.ID)),
                        new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToString()),
                        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now).AddDays(1).ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                    Claims = new Dictionary<string, object>(),
                    AdditionalHeaderClaims = new Dictionary<string, object>()
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string accessToken = tokenHandler.WriteToken(token);

                bool result = await AddAccessTokenAsync(accessToken, user.ID);

                if (result)
                {
                    return accessToken;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetUserAccessTokenAsync(int userId)
        {
            try
            {
                string accessToken = await _tokenRepository.GetUserAccessTokenAsync(userId);

                return accessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUserAccessTokenAsync(int userId)
        {
            try
            {
                var tokenModel = new AccessTokenDto
                {
                    UserID = userId
                };

                bool result = await _tokenRepository.DeleteUserAccessTokenAsync(tokenModel);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> AddAccessTokenAsync(string accessToken, int userId)
        {
            try
            {
                var tokenModel = new AccessTokenDto
                {
                    UserID = userId,
                    AccessToken = accessToken
                };

                bool result = await _tokenRepository.AddUserAccessTokenAsync(tokenModel);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetUserRoleFromUserType(UserTypeEnum userEnum)
        {
            switch (userEnum)
            {
                case UserTypeEnum.Admin:
                    return Roles.Admin;
                default:
                    return string.Empty;
            }
        }

        public int GetUserIdFromAccessToken(string accessToken)
        {
            try
            {
                int userId = 0;
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken;
                ClaimsPrincipal principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    userId = Convert.ToInt32(principle.FindFirst(ClaimTypes.Name)?.Value);
                }

                return userId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                var jwtToken = new JwtSecurityToken(token);

                return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
