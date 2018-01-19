using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using CognitiveExample.Web.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using CognitiveExample.Web.Services.Abstractions;
using CognitiveExample.Web.Services;
using Microsoft.AspNetCore.Routing;
using CognitiveExample.Web.Models.CognitiveEntities;

namespace CognitiveExample.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TwitterApiCollection>(Configuration.GetSection("TwitterApiCollection"));
            services.Configure<TextAnalyticsApiConfig>(Configuration.GetSection("CognitiveServicesConfiguration:TextAnalyticsApiConfig"));

            services.AddMvc();

            var textApiSettings = new TextAnalyticsApiConfig();
            Configuration.GetSection("CognitiveServicesConfiguration:TextAnalyticsApiConfig").Bind(textApiSettings);

            services.AddSingleton(GetTextAnalyticsApi(textApiSettings));

            services.AddTransient<IAnalysisResults, AnalysisResults>();
            services.AddTransient<IList<Feelings>, List<Feelings>>();
            services.AddTransient<IList<MultiLanguageInput>, List<MultiLanguageInput>>();
            services.AddTransient<IDictionary<string, string>, TweetDictionary>();
            services.AddTransient<ITextAnalysis, TextAnalysisService>();
            
            services.AddSingleton<ITwitterService, TwitterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env,
                              ILogger<Startup> logger)
        {
            logger.LogInformation("Configuration started");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseNodeModules(env.ContentRootPath);

            app.UseMvc(ConfigureRoutes);
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default",
                "{controller=Home}/{action=Index}/{id?}");
        }

        private TextAnalyticsAPI GetTextAnalyticsApi(TextAnalyticsApiConfig configuration) => new TextAnalyticsAPI()
        {
            AzureRegion = configuration.AzureRegion,
            SubscriptionKey = configuration.SubscriptionKey
        };
    }
}
