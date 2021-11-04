using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Google.Cloud.Storage.V1;

namespace CloudRun_Bug
{
   public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            logger.LogInformation("Service is starting...");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapGet("/", async context =>
                {
                    //handle HTTP POST of Cloud Event
                    logger.LogInformation("Handling HTTP GET");
                    
                    //list project Cloud Storage buckets to invoke auth
                    string projectId = "*projectID*"; //set unique projetID here
                    var storage = StorageClient.Create();
                    var buckets = storage.ListBuckets(projectId);
                    logger.LogInformation("Buckets:");
                    foreach (var bucket in buckets)
                    {
                        logger.LogInformation(bucket.Name);
                    }

                    await context.Response.WriteAsync(buckets.ToString());
                });
            });
        }
    }
}
