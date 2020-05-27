using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BehavingAPI.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {


        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(kv => kv.Key, kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                context.Result = new UnprocessableEntityObjectResult(errors);
                
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
