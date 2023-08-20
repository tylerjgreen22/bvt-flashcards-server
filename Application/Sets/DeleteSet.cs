using Application.Core;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for deleting a set. Method returns are wrapped in a result object that faciliates error handling
    public class DeleteSet
    {
        // Creating a command that extends IRequest, and contains a property Id
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that uses the created command to perform delete operation
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var set = await _context.Sets.FindAsync(request.Id);
                if (set == null) return null;
                _context.Remove(set);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the set");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }


}