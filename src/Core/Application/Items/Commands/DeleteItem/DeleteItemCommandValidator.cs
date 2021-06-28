namespace Application.Items.Commands.DeleteItem
{
    using FluentValidation;

    public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
        }
    }
}