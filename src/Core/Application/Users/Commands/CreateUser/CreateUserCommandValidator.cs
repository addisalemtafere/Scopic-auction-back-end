namespace Application.Users.Commands.CreateUser
{
    using FluentValidation;
    using global::Common;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            RuleFor(p => p.FullName).NotEmpty().MaximumLength(ModelConstants.User.FullNameMaxLength);
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}