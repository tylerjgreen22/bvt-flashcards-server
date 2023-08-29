namespace Application.DTOs
{
    public class SetDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AppUser { get; set; }
        public string UserImage { get; set; }
        public int CardCount { get; set; } = 0;
    }
}