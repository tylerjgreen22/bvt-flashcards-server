using Domain;
using Domain.Entities;
using FluentValidation;

namespace Application.Sets
{
    // Validator that defines the rules that flashcards must pass to be considered valid
    public class FlashcardValidator : AbstractValidator<Flashcard>
    {
        public FlashcardValidator()
        {
            // RuleFor(x => x.Id).NotEmpty().WithMessage("Flashcard Id is required");
            RuleFor(x => x.Term).NotEmpty().WithMessage("Term is required");
            RuleFor(x => x.Definition).NotEmpty().WithMessage("Definition is required");
            // RuleFor(x => x.SetId).NotEmpty().WithMessage("Set Id is required");
        }
    }

    // Validator that defines the rules that sets must pass to be considered valid
    public class SetValidator : AbstractValidator<Set>
    {
        public SetValidator()
        {
            // RuleFor(x => x.Id).NotEmpty().WithMessage("Set Id is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleForEach(x => x.Flashcards).SetValidator(new FlashcardValidator());
        }
    }
}