using DTO.IdentityDtos;
using Infrastructure.Models;

namespace Infrastructure.Interfaces.Utilities
{
    public interface IPasswordUtility
    {
        void GenerateHashedPassword(ref UserDto userDto, bool isNewUser);
        bool ValidatePassword(string password, string hashedPassword);
    }
}
