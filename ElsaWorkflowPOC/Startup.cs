using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Elsa.Activities.Email.Extensions;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Timers.Extensions;
using Elsa.Dashboard.Extensions;
using Elsa.Persistence.MongoDb.Extensions;
using Elsa.Activities.Console.Extensions;

namespace ElsaWorkflowPOC
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                // Add services used for the workflows runtime.
                .AddElsa(elsa => elsa.AddMongoDbStores(Configuration, "Elsa", "MongoDb"))
                .AddHttpActivities(options => options.Bind(Configuration.GetSection("Elsa:Http"))) // built in activity
                .AddEmailActivities(options => options.Bind(Configuration.GetSection("Elsa:Smtp"))) // built in activity
                .AddTimerActivities(options => options.Bind(Configuration.GetSection("Elsa:Timers"))) // built in activity
                .AddConsoleActivities()
                .AddWorkflow<NumberGenerationWorkflow>() // workflow with custom activities
                                                         // Add services used for the workflows dashboard.
                .AddElsaDashboard();

            services.AddActivity<GenerateARandomNumberActivity>();
            services.AddActivity<ToExecuteInCaseNumberIsEvenActivity>();
            services.AddActivity<ToExecuteInCaseNumberIsOddActivity>();
            services.AddActivity<ToExecuteInCaseOfFailureActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseHttpActivities();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseWelcomePage();
        }
    }
}
