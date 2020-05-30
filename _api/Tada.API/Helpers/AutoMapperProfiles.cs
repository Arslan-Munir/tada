using System.Linq;
using AutoMapper;
using Tada.API.Dtos;
using Tada.API.Models;

namespace Tada.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => 
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => 
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<Photo, PhotoToReturnDto>();
            CreateMap<PhotoToSaveDto, Photo>();
            CreateMap<UserToUpdateDto, User>();
            CreateMap<UserToRegisterDto, User>();
            CreateMap<MessageToCreateDto, Message>();
            CreateMap<Message, MessageToCreateDto>();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => 
                    opt.MapFrom(m => m.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.ReceiverPhotoUrl, opt => 
                    opt.MapFrom(m => m.Receiver.Photos.FirstOrDefault(p => p.IsMain).Url));


        }
    }
}