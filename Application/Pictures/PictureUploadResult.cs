namespace Application.Pictures
{
    // Represents the informain sent by Cloudinary on a successful picture upload
    public class PictureUploadResult
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
    }
}