namespace Domain.Entities
{
    // The set entity, used for grouping flashcards and associating flashcards to a user
    public class Set : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}