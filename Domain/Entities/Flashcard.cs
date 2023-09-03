using Domain.Entities;

namespace Domain
{
    // The flashcard entity, linked to the set entity via the set id
    public class Flashcard : BaseEntity
    {
        public string Term { get; set; }
        public string Definition { get; set; }
        public string PictureUrl { get; set; }
        public Set Set { get; set; }
        public string SetId { get; set; }
    }
}