using Application.DTOs;
using AutoMapper;
using Domain;
using Domain.Entities;

namespace Application.Core
{
    // Mapping profiles for AutoMapper to map objects to other objects
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Set, Set>()
                .ForMember(dest => dest.Flashcards, opt => opt.Ignore());
            CreateMap<Flashcard, Flashcard>();
            CreateMap<Set, SetDto>()
                .ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.CardCount, opt => opt.MapFrom(src => src.Flashcards.Count))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.AppUser.Pictures.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Set, SetWithFlashcardsDto>()
                .ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Flashcards, opt => opt.MapFrom(src => src.Flashcards));
            CreateMap<Flashcard, FlashcardDto>();
        }
    }
}