using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuthSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ISender Mediator;

    public ApiControllerBase(ISender mediator) => Mediator = mediator ?? throw new ArgumentNullException();
}