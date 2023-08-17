using Application.Core;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a single set. Method returns are wrapped in a result object that faciliates error handling
    public class DetailedSet
    {
        // Creating a query that extends IRequest with type of Flashcard and contains a property Id
        public class Query : IRequest<Result<Set>>
        {
            public Guid Id { get; set; }
        }

        // Handler class that uses the query of type Flashcard to process the request to Mediator
        public class Handler : IRequestHandler<Query, Result<Set>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Result<Set>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<Set>.Success(await _context.Sets.FindAsync(request.Id));
            }
        }
    }
}