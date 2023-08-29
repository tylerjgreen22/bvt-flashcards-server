using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Mediator class for creating sets. Method returns are wrapped in a result object that faciliates error handling
    public class CreateSet
    {
        // Class to create the command, extends IRequest and has a property for the sets being created
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

        // Handler class that uses the created command to handle the request to the Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting the data context via dependency injection to allow interaction with database
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            // Handle method that adds the set to the database
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                request.Set.AppUserId = user.Id;

                _context.Sets.Add(request.Set);
                foreach (var flashcard in request.Set.Flashcards)
                {
                    if (flashcard.Term == "") return Result<Unit>.Failure($"Term cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.Definition == "") return Result<Unit>.Failure($"Definition cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.SetId == "") return Result<Unit>.Failure($"SetId cannot be empty on flashcard: {flashcard.Id}");

                    _context.Flashcards.Add(flashcard);
                }

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to create set");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}