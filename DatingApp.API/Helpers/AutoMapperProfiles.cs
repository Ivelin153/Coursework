using System.Linq;
using AutoMapper;
using AprioriApp.API.DTOs;
using AprioriApp.API.Model;

namespace AprioriApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<UserForUpdateDto, User>();            
            CreateMap<FileForCreationDto, File>();
        }
    }
}