using AutoMapper;
using PlatformService.DTO;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles; 

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        // Source (database model) --> Target (dto)
        CreateMap<Platform, PlatformReadDto>();
        // Source (dto) --> Target (database model)
        CreateMap<PlatformCreateDto, Platform>();
        // Source (dto) --> Target (dto)
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
        CreateMap<Platform, GrpcPlatformModel>()
        .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
    }
}