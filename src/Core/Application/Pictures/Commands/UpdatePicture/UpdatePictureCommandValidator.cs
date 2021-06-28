namespace Application.Pictures.Commands.UpdatePicture
{
    using FluentValidation;
    using Items.Commands.UpdateItem;

    public class UpdatePictureCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdatePictureCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
        }
    }
}