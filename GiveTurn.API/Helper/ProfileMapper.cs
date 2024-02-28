using AutoMapper;
using GiveTurn.API.Entities;
using GiveTurn.Models.Dto;

namespace GiveTurn.API.Helper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<Turn, TurnDto>();
            CreateMap<TurnDto, Turn>();
        }
    }
}
