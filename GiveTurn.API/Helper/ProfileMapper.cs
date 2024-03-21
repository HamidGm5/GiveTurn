using AutoMapper;
using GiveTurn.API.Entities;
using GiveTurn.Model.Dtos;

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
            CreateMap<TurnDto, AddTurnDto>();
            CreateMap<AddTurnDto, TurnDto>();
            CreateMap<AddTurnDto, Turn>();
            CreateMap<Turn, AddTurnDto>();
        }
    }
}
