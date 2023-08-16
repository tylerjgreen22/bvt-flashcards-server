using Application.Core;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for deleting a flashcard. Method returns are wrapped in a result object that faciliates error handling
    public class DeleteFlashcard
    {
        // Creating a command that extends IRequest, and contains a property Id
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created command to perform delete operation
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var flashcard = await _context.Flashcards.FindAsync(request.Id);
                if (flashcard == null) return null;
                _context.Remove(flashcard);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the flashcard");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}