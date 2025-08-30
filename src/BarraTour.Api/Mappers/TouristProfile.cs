using AutoMapper;
using BarraTour.Api.DTOs.Tourists;
using BarraTour.Api.Models;

namespace BarraTour.Api.Mappers;

public class TouristProfile : Profile
{
    public TouristProfile()
    {
        // Mapeia Tourist -> ReadTouristDto
        CreateMap<Tourist, ReadTouristDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.User.Status))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.User.CreatedAt));
    }
}