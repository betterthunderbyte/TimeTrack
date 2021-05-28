using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Core;

namespace TimeTrack.Web.Api.Common
{
    public static class UseCaseConverter
    {
        public static ActionResult<T> ToSingleAction<T>(this UseCaseResult<T> useCaseResult) where T : class
        {
            switch (useCaseResult.ResultType)
            {
                case UseCaseResultType.Ok:
                    return new OkObjectResult(useCaseResult.Values.First());
                case UseCaseResultType.NoContent:
                    return new NoContentResult();
                case UseCaseResultType.Accepted:
                    return new AcceptedResult();
                case UseCaseResultType.Conflict:
                    return new ConflictObjectResult(useCaseResult.MessageOutput);
                case UseCaseResultType.BadRequest:
                    return new BadRequestObjectResult(useCaseResult.MessageOutput);
                default:
                    return new NotFoundObjectResult(useCaseResult.MessageOutput);
            }
        }
        
        public static ActionResult<IEnumerable<T>> ToMultiAction<T>(this UseCaseResult<T> useCaseResult) where T : class
        {
            switch (useCaseResult.ResultType)
            {
                case UseCaseResultType.Ok:
                    return new OkObjectResult(useCaseResult.Values);
                case UseCaseResultType.NoContent:
                    return new NoContentResult();
                case UseCaseResultType.Accepted:
                    return new AcceptedResult();
                case UseCaseResultType.Conflict:
                    return new ConflictObjectResult(useCaseResult.MessageOutput);
                case UseCaseResultType.BadRequest:
                    return new BadRequestObjectResult(useCaseResult.MessageOutput);
                default:
                    return new NotFoundObjectResult(useCaseResult.MessageOutput);
            }
        }
        
    }
}