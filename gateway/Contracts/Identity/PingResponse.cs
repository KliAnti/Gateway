namespace gateway.Contracts.Identity
{
    /// <summary>
    /// Контракт для предоставления ответа на запрос из gateway.
    /// </summary>
    /// <param name="Message">Сообщение, возвращенное из сервиса identity.</param>
    public sealed record PingResponse(string Message);
}
