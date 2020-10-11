using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using homework_1;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace CalculatorASP
{
    public class Startup
    {
        private static readonly string[] SupportedOperators = new[] {"+", "-", "*", "/"};
        
        private static void Calculate(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                //var parameters = 
                //var val1 = context
                var parameters = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
                int val1, val2;
                var operation = parameters.Get("operation"); ;
                if (SupportedOperators.Contains(operation) &&
                    Int32.TryParse(parameters.Get("val1"), out val1) &&
                    Int32.TryParse(parameters.Get("val2"), out val2))
                {
                    var result = Calculator.Calculate(val1, operation, val2).ToString();
                    await context.Response.WriteAsync(result);
                }
                else
                {
                    await context.Response.WriteAsync("Wrong query.");
                }
            });
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.Map("/calculate", Calculate);

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from non-Map delegate.");
            });
        }    
    }
}