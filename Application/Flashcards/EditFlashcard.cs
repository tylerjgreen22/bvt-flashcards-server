using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for editing a flashcard
    public class EditFlashcard
    {
        // Creating a command that extends IRequest, and contains a property Flashcard
        public class Command : IRequest
        {
            public Flashcard Flashcard { get; set; }
        }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command>
        {
            // Injecting data context to access DB and Auto Mapper to map incoming flashcard to flashcard in DB
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handle method that uses the created command and auto mapper to replace incoming properties on flashcard obtained from DB
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var flashcard = await _context.Flashcards.FindAsync(request.Flashcard.Id);
                _mapper.Map(request.Flashcard, flashcard);

                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}