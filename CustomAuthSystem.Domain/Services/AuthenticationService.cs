using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using CustomAuthSystem.Data;
using CustomAuthSystem.Data.Entities;
using CustomAuthSystem.Domain.Configs;
using CustomAuthSystem.Domain.Exceptions;
using CustomAuthSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CustomAuthSystem.Domain.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext _context;
    private readonly JwtConfig _jwtConfig;

    public AuthenticationService(DataContext context, IOptions<JwtConfig> options)
    {
        _context = context;
        _jwtConfig = options.Value;
    }
    
    public async Task<string> RegisterAsync(string email, string password)
    {
        var role = await _context.Roles.SingleAsync(t => t.Value == "user");
        var user = await _context.Users.SingleOrDefaultAsync(t => t.Email == email);
        if (user is not null) throw new HttpStatusException(HttpStatusCode.Conflict, "Already registered");
        var newUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Created = DateTime.UtcNow,
            RoleId = role.Id
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser.Id;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _context.Users.AsNoTracking()
                                       .Include(t => t.Role)
                                       .SingleOrDefaultAsync(t => t.Email == email);
        if (user is null) throw new HttpStatusException(HttpStatusCode.NotFound, "Not registered");
        var verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!verified) throw new HttpStatusException(HttpStatusCode.BadRequest, "Bad credentials");
        
        return GenerateJwt(user.Id, user.Role.Value);
    }

    private string GenerateJwt(string userId, string role)
    {
        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("id", userId),
                new("role", role)
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}