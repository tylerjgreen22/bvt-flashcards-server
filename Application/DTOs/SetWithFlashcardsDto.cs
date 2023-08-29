using Domain;

namespace Application.DTOs
{
    public class SetWithFlashcardsDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AppUser { get; set; }
        public ICollection<FlashcardDto> Flashcards { get; set; }
    }
}