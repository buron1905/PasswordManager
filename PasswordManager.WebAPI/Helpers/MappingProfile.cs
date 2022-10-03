using AutoMapper;
using PasswordManager.WebAPI.Models.Identity;
using PasswordManager.WebAPI.Models.Passwords;

namespace PasswordManager.WebAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Password, PasswordDTO>();
        }
    }
}
