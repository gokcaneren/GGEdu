using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace GGEdu.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

                var errorMessages = string.Join('-', errors);

                context.Result = new BadRequestObjectResult(ApiResponse<bool>.ErrorResponse(HttpStatusCode.BadRequest,
                    errorMessages, false));
            }
        }
    }
}
