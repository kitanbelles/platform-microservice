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
    }
}