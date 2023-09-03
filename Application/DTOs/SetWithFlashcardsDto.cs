namespace Application.DTOs
{
    // Set DTO with flashcards, contains app user username, profile picture, and a collection of the flashcards belonging to that set
    public class SetWithFlashcardsDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AppUser { get; set; }
        public ICollection<FlashcardDto> Flashcards { get; set; }
    }
}