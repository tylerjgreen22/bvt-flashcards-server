using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a list of sets
    public class ListSets
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<List<Set>> { }

        // Handler class that uses the query of type Flashcard list to process the request to Mediator
        public class Handler : IRequestHandler<Query, List<Set>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<List<Set>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Sets.ToListAsync();
            }
        }
    }
}