using Domain;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for getting a single flashcard
    public class DetailedFlashcard
    {
        // Creating a query that extends IRequest with type of Flashcard and contains a property Id
        public class Query : IRequest<Flashcard>
        {
            public int Id { get; set; }
        }

        // Handler class that uses the query of type Flashcard to process the request to Mediator
        public class Handler : IRequestHandler<Query, Flashcard>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Flashcard> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Flashcards.FindAsync(request.Id);
            }
        }
    }
}