using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for getting a list of flashcards. Method returns are wrapped in a result object that faciliates error handling
    public class ListFlashcards
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<Result<List<Flashcard>>> { }

        // Handler class that uses the query of type Flashcard list to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<List<Flashcard>>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Result<List<Flashcard>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Flashcard>>.Success(await _context.Flashcards.ToListAsync());
            }
        }
    }
}