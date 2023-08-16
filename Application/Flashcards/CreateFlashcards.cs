using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for creating flashcards. Method returns are wrapped in a result object that faciliates error handling
    public class CreateFlashcards
    {
        // Class to create the command, extends IRequest and has a property for the flashcard being created
        public class Command : IRequest<Result<Unit>>
        {
            public Flashcard[] Flashcards { get; set; }
        }

        // Handler class that uses the created command to handle the request to the Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting the data context via dependency injection to allow interaction with database
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that adds the flashcards to the database
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach (var flashcard in request.Flashcards)
                {
                    if (flashcard.Term == "") return Result<Unit>.Failure($"Term cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.Definition == "") return Result<Unit>.Failure($"Definition cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.SetId < 0) return Result<Unit>.Failure($"SetId cannot be empty on flashcard: {flashcard.Id}");

                    _context.Flashcards.Add(flashcard);
                }

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to create flashcard");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}