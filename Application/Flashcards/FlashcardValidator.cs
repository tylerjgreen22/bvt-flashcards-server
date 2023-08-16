using Domain;
using FluentValidation;

namespace Application.Flashcards
{
    // Validator that defines the rules that sets must pass to be considered valid
    public class FlashcardValidator : AbstractValidator<Flashcard>
    {
        public FlashcardValidator()
        {
            RuleFor(x => x.Term).NotEmpty();
            RuleFor(x => x.Definition).NotEmpty();
            RuleFor(x => x.SetId).NotEmpty();
        }
    }
}