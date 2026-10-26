namespace gateway.Contracts.Voice
{
    /// <summary>
    /// Контракт для предоставления ответа на запрос из gateway.
    /// </summary>
    /// <param name="Message">Сообщение, возвращенное из сервиса voice.</param>
    public sealed record PingResponse(string Message);
}
