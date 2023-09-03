namespace Application.DTOs
{
    // Set DTO that contains the app users username, their profile picture and the total count of cards in that given set
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