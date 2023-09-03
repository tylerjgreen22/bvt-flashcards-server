using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Pictures
{
    // Delete a picture from Cloudinary and the DB
    public class Delete
    {
        // Command comes in with the id of the picture
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        // Handler for handling the command request
        public class Handler : IRequestHandler<Command, Result<Unit>>
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

            // Handle method for handling the command request
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve user using the user accessor, includes their pictures. If user not found return null
                var user = await _context.Users.Include(p => p.Pictures).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                // Find the picture that needs to be deleted. If picture not found returns null
                var picture = user.Pictures.FirstOrDefault(x => x.Id == request.Id);
                if (picture == null) return null;

                // If the picture is the users main picture, return a failure
                if (picture.IsMain) return Result<Unit>.Failure("You cannot delete your main picture");

                // Use the picture accessor to delete the picture from Cloudinary. Return failure if it does not delete properly
                var result = await _pictureAccessor.DeletePicture(picture.Id);
                if (result == null) return Result<Unit>.Failure("Problem deleting picture from Cloudinary");

                // Remove the picture from the users pictures collection
                user.Pictures.Remove(picture);

                // Save changes, if the changes save successfully, then this boolean will return true
                var success = await _context.SaveChangesAsync() > 0;

                // Return success with the picture if result is true
                if (success) return Result<Unit>.Success(Unit.Value);

                // Return failure with error message
                return Result<Unit>.Failure("Problem deleting picture from API");
            }
        }
    }
}