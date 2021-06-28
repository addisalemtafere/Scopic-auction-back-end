namespace Application.Users.Commands.Jwt
{
    using MediatR;

    public class GenerateJwtTokenCommand : IRequest<AuthSuccessResponse>
    {
        public GenerateJwtTokenCommand(string userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        public string UserId { get; }

        public string Username { get; }
    }
}