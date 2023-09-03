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
            // Map incoming set to set in DB, ignore flashcards
            CreateMap<Set, Set>()
                .ForMember(dest => dest.Flashcards, opt => opt.Ignore());

            // Map incoming flashcard to flashcard in DB
            CreateMap<Flashcard, Flashcard>();

            // Map set to an outgoing set DTO containing username, total cards and user profile picture
            CreateMap<Set, SetDto>()
                .ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.CardCount, opt => opt.MapFrom(src => src.Flashcards.Count))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.AppUser.Pictures.FirstOrDefault(x => x.IsMain).Url));

            // Map set to an outgoing set DTO containing username, user profile picture and flashcards in the set
            CreateMap<Set, SetWithFlashcardsDto>()
                .ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Flashcards, opt => opt.MapFrom(src => src.Flashcards));

            // Map flashcard to an outgoing flashcard DTO, removes the set but keeps the set id
            CreateMap<Flashcard, FlashcardDto>();
        }
    }
}