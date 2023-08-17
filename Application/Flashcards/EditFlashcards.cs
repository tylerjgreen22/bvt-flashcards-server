using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Flashcards
{
    // Mediator class for editing a flashcard. Method returns are wrapped in a result object that faciliates error handling
    public class EditFlashcards
    {
        // Creating a command that extends IRequest, and contains a property Flashcard
        public class Command : IRequest<Result<Unit>>
        {
            public Flashcard[] Flashcards { get; set; }
            public Guid SetId { get; set; }
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

            // Handle method that uses the created command and auto mapper to replace incoming properties on flashcard obtained from DB. Also adds and deletes new flashcards
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingFlashcards = await _context.Flashcards.Where(flashcard => flashcard.SetId == request.SetId).ToListAsync();
                if (!existingFlashcards.Any()) return null;
                var flashcardIds = request.Flashcards.Select(flashcard => flashcard.Id).ToList();

                foreach (var flashcard in request.Flashcards)
                {
                    if (flashcard.Term == "") return Result<Unit>.Failure($"Term cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.Definition == "") return Result<Unit>.Failure($"Definition cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.SetId == Guid.Empty) return Result<Unit>.Failure($"SetId cannot be empty on flashcard: {flashcard.Id}");

                    var existingFlashcard = existingFlashcards.FirstOrDefault(f => f.Id == flashcard.Id);

                    if (existingFlashcard != null)
                    {
                        _mapper.Map(flashcard, existingFlashcard);
                    }
                    else
                    {
                        _context.Flashcards.Add(flashcard);
                    }
                }

                var deleteFlashcards = existingFlashcards.Where(f => !flashcardIds.Contains(f.Id)).ToList();
                _context.Flashcards.RemoveRange(deleteFlashcards);

                _context.SaveChanges();

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}