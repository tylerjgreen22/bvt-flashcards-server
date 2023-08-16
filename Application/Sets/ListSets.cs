using Application.Core;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a list of sets. Method returns are wrapped in a result object that faciliates error handling
    public class ListSets
    {
        // Creating a query that extends IRequest with a type of a list of flashcards
        public class Query : IRequest<Result<List<Set>>> { }

        // Handler class that uses the query of type Flashcard list to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<List<Set>>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Result<List<Set>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Set>>.Success(await _context.Sets.ToListAsync());
            }
        }
    }
}