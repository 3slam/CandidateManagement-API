using Azure;
using CandidateManagement.Common.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace CandidateManagement_API.Base;

public class ApplicationBaseController : ControllerBase
{
    public ActionResult HandleResult<T>(Result<T> result)
    {
        switch (result.IsSuccess)
        {
            case true:
                return new OkObjectResult(result.Data);
            case false:
                return new BadRequestObjectResult(result.Error.Description);
        }
    }
}
