namespace Application.Users.Commands.LoginUser
{
    using FluentValidation;
    using global::Common;

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}