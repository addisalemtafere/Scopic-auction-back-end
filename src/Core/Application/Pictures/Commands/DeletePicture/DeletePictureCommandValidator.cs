namespace Application.Pictures.Commands.DeletePicture
{
    using FluentValidation;

    public class DeletePictureCommandValidator : AbstractValidator<DeletePictureCommand>
    {
        public DeletePictureCommandValidator()
        {
            RuleFor(p => p.PictureId).NotEmpty();
            RuleFor(p => p.ItemId).NotEmpty();
        }
    }
}