using Chronos.Asp.Extensions;
using Chronos.Asp.Json;
using Chronos.Asp.Middleware;
using Chronos.Core;

namespace Chronos.Asp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = BuildApp(args);
            var chronosCore = app.Services.GetRequiredService<ChronosCore>();
            chronosCore.Initialize();

            RegisterTrackingStopOnShutdown(app, chronosCore);

            app.UseDefaultFiles();
            app.UseStaticFiles(CreateStaticFileOptionsForNoCacheInDevEnvironment(app));
            app.UseExceptionHandler(ExceptionHandling.Configure);
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static WebApplication BuildApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanSecondsJsonConverter());
                });

            builder.Services.AddEndpointsApiExplorer();

            if (builder.Environment.IsTestEnvironment())
            {
                builder.Services.AddInMemoryChronosCore();
            }
            else
            {
                builder.Services.AddChronosCore(ResolveAppDataDirectory());
            }

            var app = builder.Build();
            return app;
        }

        private static StaticFileOptions CreateStaticFileOptionsForNoCacheInDevEnvironment(WebApplication app)
        {
            return new StaticFileOptions
            {
                OnPrepareResponseAsync = context =>
                {
                    if (app.Environment.IsDevelopment())
                    {
                        context.Context.Response.Headers["Cache-Control"] = "No-Store";
                        return Task.CompletedTask;
                    }

                    var maxAgeInSeconds = (int)TimeSpan.FromMinutes(5).TotalSeconds;
                    context.Context.Response.Headers["Cache-Control"] = "public,max-age=" + maxAgeInSeconds;

                    return Task.CompletedTask;
                }
            };
        }

        private static void RegisterTrackingStopOnShutdown(WebApplication app, ChronosCore chronosCore)
        {
            if (app.Environment.IsTestEnvironment())
            {
                return;
            }

            app.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.Register(() =>
            {
                chronosCore.TrackingService.StopTracking(TimeOnly.FromDateTime(DateTime.Now));
            });
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
