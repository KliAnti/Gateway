using gateway.Integrations;

namespace gateway.Endpoints
{
    /// <summary>
    /// Собирает все HTTP-эндпоинты, которые обращаются к сервису voice.
    /// </summary>
    public static class VoiceEndpoints
    {
        /// <summary>
        /// Регистрирует эндпоинты voice в приложении - чтобы их можно было вызвать по HTTP.
        /// </summary>
        /// <param name="app">Объект, через который приложение добавляет себе маршруты.</param>
        public static void MapVoiceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/voice");

            group.MapGet("/ping", async (
                VoiceIntegration voiceIntegration,
                CancellationToken cancellationToken,
                string message = "Ping") =>
            {
                var response = await voiceIntegration.PingAsync(message, cancellationToken);
                return Results.Ok(response);
            });
        }
    }
}
