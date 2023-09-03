using Application.Pictures;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    // Interface for the picture accessor, defines the methods that the picture accessor must implement.
    // Interface is located here so that it is injectable into application layer without the Cloudinary dependencies being located here
    public interface IPictureAccessor
    {
        Task<PictureUploadResult> AddPicture(IFormFile file);
        Task<string> DeletePicture(string publicId);
    }
}