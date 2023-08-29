using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Pictures
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IPictureAccessor _pictureAccessor;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IPictureAccessor pictureAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _pictureAccessor = pictureAccessor;
                _context = context;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(p => p.Pictures).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                var photo = user.Pictures.FirstOrDefault(x => x.Id == request.Id);

                if (photo == null) return null;

                if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main picture");

                var result = await _pictureAccessor.DeletePicture(photo.Id);

                if (result == null) return Result<Unit>.Failure("Problem deleting picture from Cloudinary");

                user.Pictures.Remove(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Problem deleting picture from API");
            }
        }
    }
}