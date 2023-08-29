using Application.Pictures;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPictureAccessor
    {
        Task<PictureUploadResult> AddPicture(IFormFile file);
        Task<string> DeletePicture(string publicId);
    }
}