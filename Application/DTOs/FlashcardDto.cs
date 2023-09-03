namespace Application.DTOs
{
    // Flashcard DTO that removes the set object from the outgoing flashcards. The flashcards always go out with the set anyways, 
    // so the set information is redundent here
    public class FlashcardDto
    {
        public string Id { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
        public string PictureUrl { get; set; }
        public string SetId { get; set; }
    }
}