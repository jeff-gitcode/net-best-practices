using Microsoft.AspNetCore.Mvc.Infrastructure;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
      : base(message)
    {
    }
}

public class ErrorResponseHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorResponseHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = factory.CreateProblemDetails(context, detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);

            await Results
             .Problem(problemDetails)
             .ExecuteAsync(context);
        }
    }
}