using gateway.Integrations;

namespace gateway.Endpoints
{
    /// <summary>
    /// Собирает все HTTP-эндпоинты, которые обращаются к сервису identity.
    /// </summary>
    public static class IdentityEndpoints
    {
        /// <summary>
        /// Регистрирует эндпоинты identity в приложении - чтобы их можно было вызвать по HTTP.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/identity");

            group.MapGet("/ping", async (
                IdentityIntegration identityIntegration,
                CancellationToken cancellationToken,
                string message = "Ping") =>
            {
                var response = await identityIntegration.PingAsync(message, cancellationToken);
                return Results.Ok(response);
            });
        }
    }
}
