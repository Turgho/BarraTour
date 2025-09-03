using AutoMapper;
using BarraTour.Api.Features.Companies.DTOs;
using BarraTour.Api.Features.Companies.Models;

namespace BarraTour.Api.Features.Companies.Mappers;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyRequestDto, Company>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<UpdateCompanyRequestDto, Company>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Company, CompanyResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
    }
}