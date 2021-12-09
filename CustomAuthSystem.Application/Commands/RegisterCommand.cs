using CustomAuthSystem.DataTransfer.Responses;
using MediatR;

namespace CustomAuthSystem.Application.Commands;

public class RegisterCommand : IRequest<RegisterResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public RegisterCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}