namespace Application.Users.Commands.Jwt
{
    using FluentValidation;

    public class GenerateJwtTokenCommandValidator : AbstractValidator<GenerateJwtTokenCommand>
    {
        public GenerateJwtTokenCommandValidator()
        {
            RuleFor(p => p.UserId).NotEmpty();
            RuleFor(p => p.Username).NotEmpty();
        }
    }
}