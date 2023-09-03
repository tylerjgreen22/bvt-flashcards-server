using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.AspNetCore.Http;

namespace Application.Pictures
{
    // Add a picture to Cloudinary and the DB
    public class Add
    {
        // Command request comes in with a file (picture)
        public class Command : IRequest<Result<Picture>>
        {
            public IFormFile File { get; set; }
        }

        // Handler to handle the command request
        public class Handler : IRequestHandler<Command, Result<Picture>>
        {
            // Injecting the db context, picture accessor and user accessor via constructor
            private readonly DataContext _context;
            private readonly IPictureAccessor _pictureAccessor;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IPictureAccessor pictureAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _pictureAccessor = pictureAccessor;
                _context = context;
            }

            // Handle method that handles the command request
            public async Task<Result<Picture>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve user using the user accessor, includes their pictures. If user not found return null
                var user = await _context.Users.Include(p => p.Pictures).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                // Upload picture to Cloudinary using the picture accessor
                var pictureUploadResult = await _pictureAccessor.AddPicture(request.File);

                // Create a new picture entity using the results returned from the Cloudinary picture upload
                var picture = new Picture
                {
                    Url = pictureUploadResult.Url,
                    Id = pictureUploadResult.PublicId
                };

                // If the user has no main picture, set this picture to their main picture
                if (!user.Pictures.Any(x => x.IsMain)) picture.IsMain = true;

                // Add the picture to the users pictures collection
                user.Pictures.Add(picture);

                // Save changes, if the changes save successfully, then this boolean will return true
                var success = await _context.SaveChangesAsync() > 0;

                // Return success with the picture if result is true
                if (success) return Result<Picture>.Success(picture);

                // Return failure with error message
                return Result<Picture>.Failure("Problem adding Picture");
            }
        }
    }
}