using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<AddUserViewModel, AddUserDto>();
        CreateMap<AddUserDto, User>();

        CreateMap<User, UserDto>();
        CreateMap<UserDto, UserViewModel>();

        CreateMap<UpdateUserViewModel, UpdateUserDto>();
    }
}