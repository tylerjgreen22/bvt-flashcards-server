using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Sets
{
    // Mediator class for editing a set
    public class EditSet
    {
        // Creating a command that extends IRequest, and contains a property Set
        public class Command : IRequest
        {
            public Set Set { get; set; }
        }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command>
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
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var set = await _context.Sets.FindAsync(request.Set.Id);
                _mapper.Map(request.Set, set);

                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}