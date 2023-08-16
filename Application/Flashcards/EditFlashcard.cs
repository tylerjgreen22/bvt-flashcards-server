using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for editing a flashcard. Method returns are wrapped in a result object that faciliates error handling
    public class EditFlashcard
    {
        // Creating a command that extends IRequest, and contains a property Flashcard
        public class Command : IRequest<Result<Unit>>
        {
            public Flashcard Flashcard { get; set; }
        }

        // Creating a command validator that will validate the incoming set against the validations rules set in the SetValidator
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Flashcard).SetValidator(new FlashcardValidator());
            }
        }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
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
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var flashcard = await _context.Flashcards.FindAsync(request.Flashcard.Id);
                if (flashcard == null) return null;
                _mapper.Map(request.Flashcard, flashcard);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to update flashcard");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}