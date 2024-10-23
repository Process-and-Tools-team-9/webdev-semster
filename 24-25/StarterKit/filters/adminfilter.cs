using Microsoft.AspNetCore.Mvc.Filters;

public class AdminFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
    {
        var context = actionContext.HttpContext;
        if (!context.Request.Headers.ContainsKey("AdminKey")) {
            Console.WriteLine($"{context.Request.Path} was requested but there is no correct header");
            context.Response.StatusCode = 401;
            return;
        }
        if (context.Request.Headers["AdminKey"] != "Password123") {
            Console.WriteLine($"{context.Request.Path} was requested but the AdminKey is {context.Request.Headers["AdminKey"]} instead of 'AdminKey'");
            context.Response.StatusCode = 401;
            return;
        }
        await next();
        // Do something after the action executes.
    }
}