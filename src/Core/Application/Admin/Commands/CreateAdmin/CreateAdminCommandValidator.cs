namespace Application.Admin.Commands.CreateAdmin
{
    using FluentValidation;
    using global::Common;

    public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
    {
        public CreateAdminCommandValidator()
        {
            RuleFor(u => u.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            RuleFor(u => u.Role).NotEmpty();
        }
    }
}