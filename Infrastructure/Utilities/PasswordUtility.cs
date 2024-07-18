using DTO.IdentityDtos;
using Infrastructure.Interfaces.Utilities;
using Infrastructure.Models;
using System;

namespace Infrastructure.Utilities
{
    public class PasswordUtility : IPasswordUtility
    {
        public void GenerateHashedPassword(ref UserDto userDto, bool isNewUser)
        {
            try
            {
                if (isNewUser)
                {
                    userDto.SaltKey = BCrypt.Net.BCrypt.GenerateSalt();
                }

                userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password, userDto.SaltKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
