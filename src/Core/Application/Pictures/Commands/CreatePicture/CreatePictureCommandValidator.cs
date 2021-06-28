namespace Application.Pictures.Commands.CreatePicture
{
    using FluentValidation;

    public class CreatePictureCommandValidator : AbstractValidator<CreatePictureCommand>
    {
        public CreatePictureCommandValidator()
        {
            RuleFor(p => p.ItemId).NotEmpty();
        }
    }
}