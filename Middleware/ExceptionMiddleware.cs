namespace GerenciadorDeTarefas.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Ocorreu uma exceção não tratada durante a requisição.");

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();

            var (statusCode, message) = MapException(exception);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                statusCode,
                message
            });
        }
    }

    private static (int StatusCode, string Message) MapException(
        Exception exception)
    {
        if (exception is InvalidOperationException)
        {
            var message = exception.Message.Trim();

            string[] notFoundMessages =
            [
                "Usuário não encontrado.",
                "Tarefa não encontrada.",
                "Tarefa não pertence ao usuário."
            ];

            string[] conflictMessages =
            [
                "E-mail já cadastrado.",
                "Tarefa já cadastrada para este usuário.",
                "A data de vencimento deve ser posterior à data atual."
            ];

            if (notFoundMessages.Contains(
                    message,
                    StringComparer.OrdinalIgnoreCase))
            {
                return (
                    StatusCodes.Status404NotFound,
                    message);
            }

            if (conflictMessages.Contains(
                    message,
                    StringComparer.OrdinalIgnoreCase))
            {
                return (
                    StatusCodes.Status409Conflict,
                    message);
            }
        }

        return (
            StatusCodes.Status500InternalServerError,
            "Ocorreu um erro interno no servidor.");
    }
}
