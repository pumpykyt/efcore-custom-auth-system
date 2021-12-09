using CustomAuthSystem.Application.Queries;
using CustomAuthSystem.DataTransfer.Responses;
using CustomAuthSystem.Domain.Interfaces;
using MediatR;

namespace CustomAuthSystem.Application.Handlers;

public class AuthenticateHandler : IRequestHandler<AuthenticateQuery, AuthResponse>
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticateHandler(IAuthenticationService authenticationService) =>
        _authenticationService = authenticationService;

    public async Task<AuthResponse> Handle(AuthenticateQuery request, CancellationToken cancellationToken) =>
        new AuthResponse
        {
            Token = await _authenticationService.LoginAsync(request.Email, request.Password)
        };
}