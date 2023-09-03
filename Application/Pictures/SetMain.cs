using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Pictures
{
    // Sets the selected picture to the users main picture
    public class SetMain
    {
        // Command request comes in with id of picture to set to main
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        // Handler for handling the command request
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting the db context and user accessor via constructor
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            // Handle method for handling the incoming command request
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve user using the user accessor, includes their pictures. If user not found return null
                var user = await _context.Users.Include(p => p.Pictures)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                // Find the picture in the users pictures that needs to be set to main. If picture not found return null
                var picture = user.Pictures.FirstOrDefault(x => x.Id == request.Id);
                if (picture == null) return null;

                // Find the current main picture in the users pictures. Set the isMain property on the picture to false if found
                var currentMain = user.Pictures.FirstOrDefault(x => x.IsMain);
                if (currentMain != null) currentMain.IsMain = false;

                // Set the request picture to main
                picture.IsMain = true;

                // Save changes, if the changes save successfully, then this boolean will return true
                var success = await _context.SaveChangesAsync() > 0;

                // Return success with the picture if result is true
                if (success) return Result<Unit>.Success(Unit.Value);

                // Return failure with error message
                return Result<Unit>.Failure("Problem setting picture");
            }
        }
    }
}