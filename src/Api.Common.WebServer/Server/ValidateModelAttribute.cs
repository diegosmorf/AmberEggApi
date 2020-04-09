using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Api.Common.WebServer.Server
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var list = (from modelState in context.ModelState.Values
                            from error in modelState.Errors
                            select error.ErrorMessage).ToList();

                context.Result = new BadRequestObjectResult(list);
            }

            base.OnActionExecuting(context);
        }
    }
}