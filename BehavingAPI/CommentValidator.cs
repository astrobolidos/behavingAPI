using FluentValidation;

namespace BehavingAPI
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator() 
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .NotNull();

            RuleFor(c => c.Text)
                .MinimumLength(14)
                .MaximumLength(25)
                .NotEmpty();
        }
    }
}
