using FluentValidation;

namespace SistemaHostal.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Error de validacion en {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errores = ex.Errors.Select(e => e.ErrorMessage);
            await context.Response.WriteAsJsonAsync(new { message = "Error de validación.", errores });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new { message = "Ocurrió un error inesperado en el servidor." });
        }
    }
}