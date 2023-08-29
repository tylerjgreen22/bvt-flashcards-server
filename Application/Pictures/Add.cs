using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.AspNetCore.Http;

namespace Application.Pictures
{
    public class Add
    {
        public class Command : IRequest<Result<Picture>>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Picture>>
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

            public async Task<Result<Picture>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(p => p.Pictures).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                var pictureUploadResult = await _pictureAccessor.AddPicture(request.File);

                var picture = new Picture
                {
                    Url = pictureUploadResult.Url,
                    Id = pictureUploadResult.PublicId
                };

                if (!user.Pictures.Any(x => x.IsMain)) picture.IsMain = true;

                user.Pictures.Add(picture);

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Result<Picture>.Success(picture);

                return Result<Picture>.Failure("Problem adding Picture");
            }
        }
    }
}