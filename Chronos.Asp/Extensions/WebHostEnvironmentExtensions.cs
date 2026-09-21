namespace Chronos.Asp.Extensions
{
    public static class WebHostEnvironmentExtensions
    {
        public const string TestingEnvironmentName = "Testing";

        public static bool IsTestEnvironment(this IWebHostEnvironment environment)
        {
            return environment.IsEnvironment(TestingEnvironmentName);
        }
    }
}
