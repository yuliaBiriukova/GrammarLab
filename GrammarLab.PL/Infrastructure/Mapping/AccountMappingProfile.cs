using AutoMapper;
using GrammarLab.BLL.Entities;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<RegisterUserViewModel, RegisterUserDto>();
        CreateMap<RegisterUserDto, User>();

        CreateMap<LoginViewModel, LoginDto>();

        CreateMap<User, UserDto>();
        CreateMap<UserDto, UserViewModel>();
    }
}