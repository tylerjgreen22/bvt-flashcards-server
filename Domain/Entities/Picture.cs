namespace Domain.Entities
{
    // Picture entity, used to store information from Cloudinary
    public class Picture
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}