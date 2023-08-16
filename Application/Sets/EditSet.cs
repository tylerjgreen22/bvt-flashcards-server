using Application.Core;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for editing a set. Method returns are wrapped in a result object that faciliates error handling
    public class EditSet
    {
        // Creating a command that extends IRequest, and contains a property Set
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

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting data context to access DB and Auto Mapper to map incoming set to set in DB
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handle method that uses the created command and auto mapper to replace incoming properties on set obtained from DB
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var set = await _context.Sets.FindAsync(request.Set.Id);
                if (set == null) return null;
                _mapper.Map(request.Set, set);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to update the set");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}