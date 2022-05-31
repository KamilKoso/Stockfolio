﻿using Microsoft.AspNetCore.Mvc;
using Stockfolio.Shared.Infrastructure.Api;

namespace Stockfolio.Modules.Users.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesDefaultContentType]
internal abstract class BaseController : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is not null)
            return Ok(model);

        return NotFound();
    }
}