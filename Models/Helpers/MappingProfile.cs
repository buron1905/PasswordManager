using AutoMapper;
using Models;
using Models.DTOs;

namespace Models.Helpers
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
