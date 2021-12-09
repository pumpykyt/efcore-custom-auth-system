﻿namespace CustomAuthSystem.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(string email, string password);
    Task<string> LoginAsync(string email, string password);
}