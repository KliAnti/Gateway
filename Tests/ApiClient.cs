using System.Net.Http.Json;

namespace gateway.Tests
{
    /// <summary>
    /// Обертка вокруг HttpClient.
    /// Вызывает надстроенные собственные методы и не собирает url вручную.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Принимает и собирает готовый HttpClient.
        /// Далее передает его в GatewayFixture.
        /// </summary>
        /// <param name="httpClient">HTTP-клиент, настроенный на тестовый хост gateway.</param>
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Отправляет GET-запрос по указанному пути.
        /// </summary>
        /// <param name="url">Путь эндпоинта.</param>
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _httpClient.GetAsync(url);
        }

        /// <summary>
        /// Десериализует тело HTTP-ответа в указанный тип.
        /// </summary>
        /// <param name="response">Тело http ответа.</param>
        public Task<TResponse?> ReadAsAsync<TResponse>(HttpResponseMessage response)
        {
            return response.Content.ReadFromJsonAsync<TResponse>();
        }

        /// <summary>
        /// Дергает /api/identity/ping.
        /// </summary>
        /// <param name="message">Текст, отправляемый в identity.</param>
        public Task<HttpResponseMessage> PingIdentity(string? message = null)
        {
            return GetAsync(WithMessage(Routes.IdentityPing, message));
        }

        /// <summary>
        /// Дергает /api/voice/ping.
        /// </summary>
        /// <param name="message">Текст, отправляемый в voice.</param>
        public Task<HttpResponseMessage> PingVoice(string? message = null)
        {
            return GetAsync(WithMessage(Routes.VoicePing, message));
        }

        /// <summary>
        /// Добавляет к пути query-параметр message.
        /// </summary>
        /// <param name="route">Путь эндпоинта.</param>
        /// <param name="message">Значение query-параметра message (может быть null).</param>
        private static string WithMessage(string route, string? message)
        {
            return message is null ? route : $"{route}?message={Uri.EscapeDataString(message)}";
        }
    }
}
