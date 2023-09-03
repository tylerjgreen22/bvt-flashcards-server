using Domain.Entities.Identity;

namespace Domain.Entities
{
    // The set entity linked to the app user by the app user id and contains a collection of flashcards
    public class Set : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; }
    }
}