using AutoMapper;
using BarraTour.Application.DTOs.User;
using BarraTour.Domain.Entities;
using BarraTour.Domain.ValueObjects.User;

namespace BarraTour.Application.Mapping;

public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Mapeamento de CreateUserDto para User
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                    new UserName(src.FirstName, src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => 
                    new Email(src.Email)))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => 
                    PasswordHash.Create(src.Password)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => 
                    PhoneNumber.Parse(src.PhoneNumber)))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailVerified, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // Mapeamento de User para UserResponseDto
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.ToString()));

            // Mapeamento de User para UserProfileDto
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.ToString()));

            // Mapeamento de UpdateUserDto para User (para atualizações parciais)
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => 
                    new UserName(
                        src.FirstName ?? dest.Name.FirstName, 
                        src.LastName ?? dest.Name.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom((src, dest) => 
                    src.Email != null ? new Email(src.Email) : dest.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom((src, dest) => 
                    src.PhoneNumber != null ? PhoneNumber.Parse(src.PhoneNumber) : dest.PhoneNumber))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }