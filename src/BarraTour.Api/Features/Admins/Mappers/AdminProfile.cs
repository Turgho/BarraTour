using AutoMapper;
using BarraTour.Api.Features.Admins.DTOs;
using BarraTour.Api.Features.Admins.Models;

namespace BarraTour.Api.Features.Admins.Mappers;


public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<CreateAdminRequestDto, Admin>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<UpdateAdminRequestDto, Admin>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Admin, AdminResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
    }
}
