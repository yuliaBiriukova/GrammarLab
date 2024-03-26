using AutoMapper;
using GrammarLab.BLL.Models;
using GrammarLab.PL.ViewModels;

namespace GrammarLab.PL.Infrastructure.Mapping;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<LoginViewModel, LoginDto>();
    }
}