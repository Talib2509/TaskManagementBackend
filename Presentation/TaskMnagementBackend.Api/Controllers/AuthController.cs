using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.Auth.Commands.ConfirmEmail;
using TaskMnagementBackend.Aplication.Features.Auth.Commands.ForgotPassword;
using TaskMnagementBackend.Aplication.Features.Auth.Commands.Login;
using TaskMnagementBackend.Aplication.Features.Auth.Commands.RefreshTokenLogin;
using TaskMnagementBackend.Aplication.Features.Auth.Commands.Register;
using TaskMnagementBackend.Aplication.Features.Auth.Commands.ResetPassword;

namespace TaskMnagementBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return Unauthorized(response);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token-login")]
        public async Task<IActionResult> RefreshTokenLogin([FromForm] RefreshTokenLoginRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return Unauthorized(response);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordRequest request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
