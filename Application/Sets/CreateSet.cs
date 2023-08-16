using Application.Core;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for creating sets. Method returns are wrapped in a result object that faciliates error handling
    public class CreateSet
    {
        // Class to create the command, extends IRequest and has a property for the sets being created
        public class Command : IRequest<Result<Unit>>
        {
            public Set Set { get; set; }
        }

        // Creating a command validator that will validate the incoming set against the validations rules set in the SetValidator
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Set).SetValidator(new SetValidator());
            }
        }

        // Handler class that uses the created command to handle the request to the Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting the data context via dependency injection to allow interaction with database
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that adds the set to the database
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Sets.Add(request.Set);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to create set");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}