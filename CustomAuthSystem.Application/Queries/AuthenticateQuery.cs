using CustomAuthSystem.DataTransfer.Responses;
using MediatR;

namespace CustomAuthSystem.Application.Queries;

public class AuthenticateQuery: IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public AuthenticateQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }
}