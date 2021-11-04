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
                    
                    string projectId = "YOUR-PROJECT-ID"; //set unique projetID here
                    string bucketName = "sample-code-test-bucket";

                    //create new bucket
                    var storage = StorageClient.Create();
                    var buckets = storage.ListBuckets(projectId);
                    var newbucket = storage.CreateBucket(projectId, bucketName);
                    logger.LogInformation("New Bucket: " + bucketName + "created");

                    //list all buckets
                    logger.LogInformation("Buckets:");
                    foreach (var bucket in buckets)
                    {
                        logger.LogInformation(bucket.Name);
                    }

                    //delete new bucket
                    storage.DeleteBucket(bucketName);
                    logger.LogInformation("New Bucket: " + bucketName + " deleted");

                    await context.Response.WriteAsync("Bucket create/delete test complete");
                });
            });
        }
    }
}
