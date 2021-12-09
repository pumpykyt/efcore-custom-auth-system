using CustomAuthSystem.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CustomAuthSystem.Application.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder application)
    {
        return application.UseMiddleware<ErrorHandlerMiddleware>();
    }
}