using Application.Interfaces;
using Application.Pictures;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Pictures
{
    // Picture accessor that is used to interact with Cloudinary API
    public class PictureAccessor : IPictureAccessor
    {
        // Creating an account using the Cloudinary settings from config and creating a Cloudinary object using that account
        private readonly Cloudinary _cloudinary;
        public PictureAccessor(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        // Add a picture to Cloudinary using provided file (picture)
        public async Task<PictureUploadResult> AddPicture(IFormFile file)
        {
            // If the file has content, add it to Cloudinary, otherwise return null
            if (file.Length > 0)
            {
                // Opens a readstream, using keyword desposes of stream after use
                await using var stream = file.OpenReadStream();

                // Creating upload params to specify what is uploaded to Cloudinary
                // The file to be uploaded is provided, and the transformation property sets the picture to be square with a crop of fill
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                // Upload the file to Cloudinary using the upload params
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // If the upload result contains an error, throw an exception with the provided error message
                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                // Create a picture upload result using the information returned by Cloudinary on a successful picture upload
                return new PictureUploadResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString()
                };
            }

            return null;
        }

        // Delete a picture from Cloudinary based on the provided picture id
        public async Task<string> DeletePicture(string publicId)
        {
            // Create delete params using the provided id
            var deleteParams = new DeletionParams(publicId);

            // Delete the picture from Cloudinary using the delete params
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // IF the delete is successful, return the result from Cloudinary, otherwise return null
            return result.Result == "ok" ? result.Result : null;
        }
    }
}