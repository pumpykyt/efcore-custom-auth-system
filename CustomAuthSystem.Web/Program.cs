using CustomAuthSystem.Application.Extensions;
using CustomAuthSystem.Application.Handlers;
using CustomAuthSystem.Application.Profiles;
using CustomAuthSystem.Data;
using CustomAuthSystem.Domain.Configs;
using CustomAuthSystem.Domain.Interfaces;
using CustomAuthSystem.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(AuthenticateHandler));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<DataContext>(a =>
    a.UseNpgsql(builder.Configuration.GetSection("ConnectionString").Value,
        b => b.MigrationsAssembly("CustomAuthSystem.Web")
    )
);
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters();
        }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseErrorHandlerMiddleware();
app.UseAuthorization();
app.MapControllers();
app.Run();