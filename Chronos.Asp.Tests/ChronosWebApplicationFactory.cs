using Chronos.Asp.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Chronos.Asp.Tests
{
    /// <summary>
    /// Boots the real Chronos.Asp pipeline, but backed by the in-memory database instead of the app data directory.
    /// </summary>
    public class ChronosWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(WebHostEnvironmentExtensions.TestingEnvironmentName);
        }
    }
}
