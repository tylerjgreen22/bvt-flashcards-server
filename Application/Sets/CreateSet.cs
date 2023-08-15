using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for creating sets
    public class CreateSet
    {
        // Class to create the command, extends IRequest and has a property for the sets being created
        public class Command : IRequest
        {
            public Set Set { get; set; }
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

            // Handle method that adds the set to the database
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Sets.Add(request.Set);

                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}