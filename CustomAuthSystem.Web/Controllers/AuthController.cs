using CustomAuthSystem.Application.Commands;
using CustomAuthSystem.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthSystem.Web.Controllers;

public sealed class AuthController : ApiControllerBase
{
    public AuthController(ISender mediator) : base(mediator) { }

    [HttpGet("login")]
    public async Task<IActionResult> AuthenticateAsync(string email, string password) =>
        Ok(await Mediator.Send(new AuthenticateQuery(email, password)));

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(string email, string password) =>
        Ok(await Mediator.Send(new RegisterCommand(email, password)));
}