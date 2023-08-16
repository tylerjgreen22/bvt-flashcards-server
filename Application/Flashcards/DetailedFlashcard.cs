using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for getting a single flashcard. Method returns are wrapped in a result object that faciliates error handling
    public class DetailedFlashcard
    {
        // Creating a query that extends IRequest with type of Flashcard and contains a property Id
        public class Query : IRequest<Result<Flashcard>>
        {
            public int Id { get; set; }
        }

        // Handler class that uses the query of type Flashcard to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<Flashcard>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Result<Flashcard>> Handle(Query request, CancellationToken cancellationToken)
            {
                var flashcard = await _context.Flashcards.FindAsync(request.Id);

                return Result<Flashcard>.Success(flashcard);
            }
        }
    }
}