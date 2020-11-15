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
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<CalculatorMiddleware>();
            app.Run(async context =>
            {
                await context.Response.WriteAsync("");
            });
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICalculator, Calculator>();
        }
    }
}