using Chronos.Asp.Json;
using Chronos.Asp.Middleware;
using Chronos.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Chronos.Asp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanSecondsJsonConverter());
                });

            builder.Services.AddEndpointsApiExplorer();

            var appDataDirectory = ResolveAppDataDirectory();
            builder.Services.AddChronosCore(appDataDirectory);

            var app = builder.Build();
            app.Services.GetRequiredService<ChronosCore>().Initialize();

            app.UseDefaultFiles();
            app.UseStaticFiles();

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
