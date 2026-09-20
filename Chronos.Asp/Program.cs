using Chronos.Asp.Json;
using Chronos.Asp.Middleware;
using Chronos.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Chronos.Asp
{
    public class Program
    {
        /// <summary>
        /// Environment name under which the in-memory database is used instead of the app data directory.
        /// </summary>
        public const string TestingEnvironmentName = "Testing";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanSecondsJsonConverter());
                });

            builder.Services.AddEndpointsApiExplorer();

            if (builder.Environment.IsEnvironment(TestingEnvironmentName))
            {
                builder.Services.AddInMemoryChronosCore();
            }
            else
            {
                builder.Services.AddChronosCore(ResolveAppDataDirectory());
            }

            var app = builder.Build();
            app.Services.GetRequiredService<ChronosCore>().Initialize();

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponseAsync = context =>
                {
                    if(app.Environment.IsDevelopment())
                    {
                        context.Context.Response.Headers["Cache-Control"] = "No-Store";
                        return Task.CompletedTask;
                    }

                    var maxAgeInSeconds = (int) TimeSpan.FromMinutes(5).TotalSeconds;
                    context.Context.Response.Headers["Cache-Control"] = "public,max-age=" + maxAgeInSeconds;

                    return Task.CompletedTask;
                }
            });

            app.UseExceptionHandler(ExceptionHandling.Configure);

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static string ResolveAppDataDirectory()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directoryPath = Path.Combine(appData, "Chronos");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }
    }
}
