using Domain;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for creating flashcards
    public class CreateFlashcard
    {
        // Class to create the command, extends IRequest and has a property for the flashcard being created
        public class Command : IRequest
        {
            public Flashcard Flashcard { get; set; }
        }

        // Handler class that uses the created command to handle the request to the Mediator
        public class Handler : IRequestHandler<Command>
        {
            // Injecting the data context via dependency injection to allow interaction with database
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that adds the flashcard to the database
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Flashcards.Add(request.Flashcard);

                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}