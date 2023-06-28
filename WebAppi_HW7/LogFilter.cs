using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebAppi_Diplom
{
    public class LogFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Second step (LogFilter) {context.HttpContext.Request.Path}");
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {           
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"First step (LogFilter) {context.HttpContext.Request.Path}");
        }
    }
}
