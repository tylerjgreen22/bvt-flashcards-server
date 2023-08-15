using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for deleting a set
    public class DeleteSet
    {
        // Creating a command that extends IRequest, and contains a property Id
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created command to perform delete operation
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var set = await _context.Sets.FindAsync(request.Id);
                _context.Remove(set);

                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }


}