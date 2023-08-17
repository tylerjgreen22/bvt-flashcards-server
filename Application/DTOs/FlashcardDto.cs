namespace Application.DTOs
{
    public class FlashcardDto
    {
        public string Id { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
        public string PictureUrl { get; set; }
        public Guid SetId { get; set; }
    }
}