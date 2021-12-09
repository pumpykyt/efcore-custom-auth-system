using System.Net;
using CustomAuthSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace CustomAuthSystem.Application.Middlewares;

internal class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (HttpStatusException httpException)
        {
            context.Response.StatusCode = (int)httpException.Status;
            var responseFeature = context.Features.Get<IHttpResponseFeature>();
            responseFeature.ReasonPhrase = httpException.Message;
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var responseFeature = context.Features.Get<IHttpResponseFeature>();
            responseFeature.ReasonPhrase = exception.Message;
        }
    }
}