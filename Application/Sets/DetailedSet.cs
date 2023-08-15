using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for getting a single set
    public class DetailedSet
    {
        // Creating a query that extends IRequest with type of Flashcard and contains a property Id
        public class Query : IRequest<Set>
        {
            public int Id { get; set; }
        }

        // Handler class that uses the query of type Flashcard to process the request to Mediator
        public class Handler : IRequestHandler<Query, Set>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created query to obtain the requested flashcard
            public async Task<Set> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Sets.FindAsync(request.Id);
            }
        }
    }
}