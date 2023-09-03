using Application.Core;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Delete a set from DB
    public class DeleteSet
    {
        // Command request comes in with the id of the set to delete
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        // Handler that handles the command request
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting data context to access DB
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that handles the command request
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Find the set that needs to be deleted. If not found return null. Remove the set
                var set = await _context.Sets.FindAsync(request.Id);
                if (set == null) return null;
                _context.Remove(set);

                // Save changes, if the changes save successfully, then this boolean will return true
                var success = await _context.SaveChangesAsync() > 0;

                // Return success if result is true
                if (success) return Result<Unit>.Success(Unit.Value);

                // Return failure with error message
                return Result<Unit>.Failure("Failed to delete the set");
            }
        }
    }


}