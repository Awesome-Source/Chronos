using Apollo.Sqlite;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace Chronos.Asp.Middleware
{
    public static class ExceptionHandling
    {
        private const int SqliteConstraintErrorCode = 19;

        public static void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = feature?.Error;

                var (status, title) = Classify(exception);

                context.Response.StatusCode = status;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                };

                await context.Response.WriteAsJsonAsync(problem);
            });
        }

        private static (int Status, string Title) Classify(Exception? exception)
        {
            return exception switch
            {
                ForeignKeyCheckFailedException => (StatusCodes.Status409Conflict,
                    "Cannot delete: this item is still referenced by other records."),
                SqliteException sqliteException when sqliteException.SqliteErrorCode == SqliteConstraintErrorCode =>
                    (StatusCodes.Status409Conflict, "Cannot delete: this item is still referenced by other records."),
                ArgumentException => (StatusCodes.Status400BadRequest, exception!.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred."),
            };
        }
    }
}
