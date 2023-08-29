using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        // public class CommandValidator : AbstractValidator<Command>
        // {
        //     public CommandValidator()
        //     {
        //         RuleFor(x => x.Set).SetValidator(new SetValidator());
        //     }
        // }

        // Handler class that handles the request to Mediator
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            // Injecting data context to access DB and Auto Mapper to map incoming set to set in DB
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _mapper = mapper;
                _userAccessor = userAccessor;
                _context = context;
            }

            // Handle method that uses the created command and auto mapper to replace incoming properties on set obtained from DB
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                var set = await _context.Sets.FindAsync(request.Set.Id);
                if (set == null) return null;
                _mapper.Map(request.Set, set);
                set.AppUserId = user.Id;
                await _context.SaveChangesAsync();

                if (set.AppUserId == null)
                {
                    set.AppUserId = user.Id;
                    await _context.SaveChangesAsync();
                }

                _context.Entry(set).State = EntityState.Detached;

                var existingFlashcards = await _context.Flashcards.Where(flashcard => flashcard.SetId == request.Set.Id).ToListAsync();
                var flashcardIds = request.Set.Flashcards.Select(flashcard => flashcard.Id).ToList();

                foreach (var flashcard in request.Set.Flashcards)
                {
                    if (flashcard.Term == "") return Result<Unit>.Failure($"Term cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.Definition == "") return Result<Unit>.Failure($"Definition cannot be empty on flashcard: {flashcard.Id}");
                    if (flashcard.SetId == "") return Result<Unit>.Failure($"SetId cannot be empty on flashcard: {flashcard.Id}");

                    var existingFlashcard = existingFlashcards.FirstOrDefault(f => f.Id == flashcard.Id);

                    if (existingFlashcard != null)
                    {
                        _mapper.Map(flashcard, existingFlashcard);
                        _context.Update(existingFlashcard);
                    }
                    else
                    {
                        _context.Flashcards.Add(flashcard);
                    }
                }

                var deleteFlashcards = existingFlashcards.Where(f => !flashcardIds.Contains(f.Id)).ToList();
                if (deleteFlashcards.Count > 0)
                {
                    _context.Flashcards.RemoveRange(deleteFlashcards);
                }


                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to update the set");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}