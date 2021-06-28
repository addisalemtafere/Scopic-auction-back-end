namespace Application.Admin.Commands.DeleteAdmin
{
    using FluentValidation;
    using global::Common;

    public class DeleteAdminCommandValidator : AbstractValidator<DeleteAdminCommand>
    {
        public DeleteAdminCommandValidator()
        {
            RuleFor(u => u.Email).NotEmpty().Matches(ModelConstants.User.EmailRegex);
            RuleFor(u => u.Role).NotEmpty();
        }
    }
}