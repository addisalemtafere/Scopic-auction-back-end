namespace Application.Users.Commands.ConfirmEmail
{
    using FluentValidation;
    using global::Common;

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(p => p.Code).NotEmpty();
            RuleFor(p => p.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
        }
    }
}