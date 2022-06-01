using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Http.Polly;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Contrib.WaitAndRetry;
using Polly.Contrib.Simmy;
using Polly.Registry;
using Polly.Contrib.Simmy.Behavior;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;
using WebApp.Chaos;
using WebApp.Chaos.Extensions;



using System.Data;
using System.Net;
using System.Reflection;
using WebApp.Extensions;
//using Duber.Infrastructure.Chaos;

namespace WebApp
{
    public class Startup
    {
        const string ResiliencePolicy = "ResiliencePolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var retryPolicy = HttpPolicyExtensions
            //    .HandleTransientHttpError() // HttpRequestException, 5XX and 408
            //    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
            services.AddRazorPages();
            //services.AddHttpClient();

            //services.AddPolicyRegistry(new PolicyRegistry
            //{
            //    { ResiliencePolicy, GetResiliencePolicy() }
            //});
            //services.AddHttpClient<ResilientHttpClient>("HttpClient")
            //    .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
            //    .AddPolicyHandler(GetResiliencePolicy());
            {
                //var policyRegistry = services.AddPolicyRegistry();
                //policyRegistry["ResiliencePolicy"] = GetHttpResiliencePolicy();

                //services.AddHttpClient<ResilientHttpClient>()
                //    .AddPolicyHandler(retryPolicy);
                //    //.AddPolicyHandlerFromRegistry("ResiliencePolicy");


                //services.AddSingleton<ChaosApiHttpClient>();
                //services.AddChaosApiHttpClient(Configuration);
                //var cSettings = Configuration.GetSection("ChaosSettings");
                //services.Configure<AppChaosSettings>(Configuration.GetSection("ChaosSettings"));
            }
            
            services.AddHttpClient<IHttpClientService,HttpClientService>(Constants._faultTolerantHttpClientName)
                .AddPolicyHandlerApi(Configuration);
           

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        private IsPolicy GetHttpResiliencePolicy()
        {
            var retryCount = 5;
            var exceptionsAllowedBeforeBreaking = 5;
            //if (!string.IsNullOrEmpty(configuration["HttpClientRetryCount"]))
            //{
            //    retryCount = int.Parse(configuration["HttpClientRetryCount"]);
            //}

            //var exceptionsAllowedBeforeBreaking = 4;
            //if (!string.IsNullOrEmpty(configuration["HttpClientExceptionsAllowedBeforeBreaking"]))
            //{
            //    exceptionsAllowedBeforeBreaking = int.Parse(configuration["HttpClientExceptionsAllowedBeforeBreaking"]);
            //}

            // Define a couple of policies which will form our resilience strategy.
            var policies = HttpPolicyExtensions.HandleTransientHttpError()
                .RetryAsync(retryCount)
                .WrapAsync(HttpPolicyExtensions.HandleTransientHttpError()
                    .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, TimeSpan.FromSeconds(5)));

            return policies;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //var httpPolicyRegistry = app.ApplicationServices.GetRequiredService<IPolicyRegistry<string>>();
                //httpPolicyRegistry?.AddHttpChaosInjectors();
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //if (env.IsDevelopment())
            //{
            //    // Wrap every policy in the policy registry in Simmy chaos injectors.
            //    var httpPolicyRegistry = app.ApplicationServices.GetRequiredService<IPolicyRegistry<string>>();
            //    httpPolicyRegistry?.AddHttpChaosInjectors();
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }


        
       
    }
    
}
