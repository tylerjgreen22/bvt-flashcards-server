using Domain.Entities;
using FluentValidation;

namespace Application.Sets
{
    // Validator that defines the rules that sets must pass to be considered valid
    public class SetValidator : AbstractValidator<Set>
    {
        public SetValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.AppUserId).NotEmpty();
        }
    }
}