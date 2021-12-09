using CustomAuthSystem.Application.Commands;
using CustomAuthSystem.DataTransfer.Responses;
using CustomAuthSystem.Domain.Interfaces;
using MediatR;

namespace CustomAuthSystem.Application.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterHandler(IAuthenticationService authenticationService) =>
        _authenticationService = authenticationService;

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken) =>
        new RegisterResponse
        {
            UserId = await _authenticationService.RegisterAsync(request.Email, request.Password)
        };
}