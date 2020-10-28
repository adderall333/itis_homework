using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using homework_1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CalculatorASP
{
    public class CalculatorMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string[] SupportedOperators = new[] {"+", "-", "*", "/"};

        public CalculatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context, ICalculator calculator)
        {
            var parameters = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
            double val1, val2;
            var operation = parameters.Get("operation"); ;
            if (SupportedOperators.Contains(operation) &&
                Double.TryParse(parameters.Get("val1"), out val1) &&
                Double.TryParse(parameters.Get("val2"), out val2))
            {
                var result = calculator.Calculate(val1, operation, val2).ToString();
                await context.Response.WriteAsync(result);
            }
            else
            {
                await context.Response.WriteAsync("Wrong query.");
            }
            await _next.Invoke(context);
        }
    }
}