using Application.Core;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sets
{
    // Edit a set in the DB
    public class EditSet
    {
        // Command comes in with the set information to replace the set in the DB with
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
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

        // Handler that handles the command request
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting db context via constructor
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handle method that handles the command request
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Find the set in the DB to edit that matches the incoming set id. If not found return null
                var set = await _context.Sets.Include(x => x.Flashcards).FirstOrDefaultAsync(x => x.Id == request.Id);
                if (set == null) return null;

                // Set the properties of the set in the db to the properties from the incoming set
                set.Title = request.Set.Title;
                set.Description = request.Set.Description;

                // Retrieve the exisiting flashcards from the set from the db
                var existingFlashcards = set.Flashcards;

                // Get the ids of the flashcards from the incoming set
                var flashcardIds = request.Set.Flashcards.Select(flashcard => flashcard.Id).ToList();

                foreach (var flashcard in request.Set.Flashcards)
                {
                    // Find the flashcard from the DB that matches the flashcard from the incoming set
                    var existingFlashcard = existingFlashcards.FirstOrDefault(f => f.Id == flashcard.Id);

                    // If the flashcard in the DB is found, map the incoming flashcard to the DB flashcard
                    // Otherwise, if the flashcard in the db is not found, add the new flashcard to the DB
                    if (existingFlashcard != null)
                    {

                        existingFlashcard.Term = flashcard.Term;
                        existingFlashcard.Definition = flashcard.Definition;
                        existingFlashcard.PictureUrl = flashcard.PictureUrl;
                    }
                    else
                    {
                        flashcard.Id = Guid.NewGuid().ToString();
                        flashcardIds.Add(flashcard.Id);
                        flashcard.SetId = request.Id;
                        _context.Flashcards.Add(flashcard);
                    }
                }

                // Find the exisiting flashcards that are not present in the incoming flashcards and delete them
                var deleteFlashcards = existingFlashcards.Where(f => !flashcardIds.Contains(f.Id)).ToList();
                if (deleteFlashcards.Count > 0)
                {
                    _context.Flashcards.RemoveRange(deleteFlashcards);
                }


                // Save changes, if the changes save successfully, then this boolean will return true
                var success = await _context.SaveChangesAsync() > 0;

                // Return success if result is true
                if (success) return Result<Unit>.Success(Unit.Value);

                // Return failure with error message
                return Result<Unit>.Failure("Failed to edit the set");
            }
        }
    }
}