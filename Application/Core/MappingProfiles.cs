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
            CreateMap<Set, Set>();
            CreateMap<Flashcard, Flashcard>();
            CreateMap<Set, SetDto>()
                .ForMember(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser.UserName));
            CreateMap<Flashcard, FlashcardDto>();
        }
    }
}