namespace Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Application;
    using Application.Common.Models;
    using Application.Users.Commands;
    using Application.Users.Commands.ConfirmEmail;
    using Application.Users.Commands.CreateUser;
    using Application.Users.Commands.Jwt.Refresh;
    using Application.Users.Commands.LoginUser;
    using Application.Users.Commands.Logout;
    using global::Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    public class IdentityController : BaseController
    {
        private readonly IDateTime dateTime;

        public IdentityController(IDateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(Register))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulRegisterRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnRegisterDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Register(CreateUserCommand model)
        {
            await Mediator.Send(model);
            return Ok();
        }

        /// <summary>
        /// Verifies user email
        /// </summary>
        [HttpPost]
        [Route(nameof(Confirm))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulEmailConfirmationMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnConfirmEmailMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Confirm(ConfirmEmailCommand model)
        {
            await Mediator.Send(model);
            return Ok();
        }

        /// <summary>
        /// Verifies user credentials and generates JWT and Refresh token
        /// </summary>
        [HttpPost]
        [Route(nameof(Login))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulLoginRequestDescriptionMessage,
            typeof(Response<AuthSuccessResponse>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnLoginDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Login(LoginUserCommand model)
        {
            var result = await Mediator.Send(model);
            SetCookies(result.Data.Token, result.Data.RefreshToken.ToString());
            return Ok(result);
        }

        /// <summary>
        /// Verifies the provided token and generates new token and refresh token
        /// </summary>
        [HttpPost]
        [Route(nameof(Refresh))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulTokenRefreshRequestDescriptionMessage,
            typeof(Response<AuthSuccessResponse>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.IdentityConstants.BadRequestOnTokenRefreshDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Refresh(JwtRefreshTokenCommand model)
        {
            var refreshToken = Request.Cookies[ApiConstants.RefreshToken];
            var jwtToken = Request.Cookies[ApiConstants.JwtToken];

            if (refreshToken == null || jwtToken == null) return Unauthorized();

            model.RefreshToken = Guid.Parse(refreshToken);
            model.Token = jwtToken;
            var result = await Mediator.Send(model);
            SetCookies(result.Data.Token, result.Data.RefreshToken.ToString());
            return Ok(result);
        }

        /// <summary>
        /// Invalidates jwt tokens and removes cookies - logout user
        /// </summary>
        [HttpPost]
        [Route(nameof(Logout))]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.IdentityConstants.SuccessfulLogOut)]
        public async Task<IActionResult> Logout()
        {
            Request.Cookies.TryGetValue(ApiConstants.RefreshToken, out var refreshToken);
            Response.Cookies.Delete(ApiConstants.JwtToken);
            Response.Cookies.Delete(ApiConstants.RefreshToken);

            await Mediator.Send(new LogoutUserCommand {RefreshToken = refreshToken});
            return Ok();
        }

        private void SetCookies(string jwtToken, string refreshToken)
        {
            SetJwtTokenCookie(jwtToken);
            SetRefreshTokenCookie(refreshToken);
        }

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = dateTime.UtcNow.AddMonths(AppConstants.RefreshTokenExpirationTimeInMonths)
            };

            Response.Cookies.Append(ApiConstants.RefreshToken, token, cookieOptions);
        }

        private void SetJwtTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                // It doesn't mater what time we will set since we check the expiration time later :)
                Expires = DateTimeOffset.MaxValue
            };

            Response.Cookies.Append(ApiConstants.JwtToken, token, cookieOptions);
        }
    }
}