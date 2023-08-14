namespace Domain
{
    // The flashcard entity, the core entity of the project
    public class Flashcard
    {
        public Guid Id { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
        public string PictureUrl { get; set; }
    }
}