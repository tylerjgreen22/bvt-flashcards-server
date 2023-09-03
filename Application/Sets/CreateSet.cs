using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Add a set to the DB
    public class CreateSet
    {
        // Command request comes in with the set to be added
        public class Command : IRequest<Result<Unit>>
        {
            public Set Set { get; set; }
        }

        // Creating a command validator that will validate the incoming set against the validations rules set in the SetValidator
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Set).SetValidator(new SetValidator());
            }
        }

        // Handler that handles the command request
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

            // Handle method that handles the command request
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retreive the user making the request using the user accessor. If user not found return null
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                var setId = Guid.NewGuid().ToString();

                // Set the app user id on the set to the found user id and create a set id
                request.Set.Id = setId;
                request.Set.AppUserId = user.Id;

                // Set the flashcards in the request set to have a guid id and to have the set id that links them to the created set
                foreach (var flashcard in request.Set.Flashcards)
                {
                    flashcard.Id = Guid.NewGuid().ToString();
                    flashcard.SetId = setId;
                }

                // Add the set to the db
                _context.Sets.Add(request.Set);

                // Save changes, if the changes save successfully, then this boolean will return true
                var success = await _context.SaveChangesAsync() > 0;

                // Return success if result is true
                if (success) return Result<Unit>.Success(Unit.Value);

                // Return failure with error message
                return Result<Unit>.Failure("Failed to create set");
            }
        }
    }
}