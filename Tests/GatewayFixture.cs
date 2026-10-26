using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using IdentityClient = identity.Grpc.Identity.IdentityClient;
using VoiceClient = voice.Grpc.Voice.VoiceClient;

namespace gateway.Tests
{
    /// <summary>
    /// Поднятие gateway в памяти, без реального порта и сети.
    /// Подмена настоящих микросервисов (клиентов) на их моки.
    /// Фикстура создается единажды на все Gateway тесты.
    /// </summary>
    public class GatewayFixture : WebApplicationFactory<Program>
    {
        public Mock<IdentityClient> IdentityClient { get; } = new();
        public Mock<VoiceClient> VoiceClient { get; } = new();

        /// <summary>
        /// Подменяет в DI настоящие клиенты на моки перед запуском тестового хоста.
        /// </summary>
        /// <param name="builder">Конфигурация хоста, которую собирает WebApplicationFactory.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IdentityClient>();
                services.AddSingleton(IdentityClient.Object);

                services.RemoveAll<VoiceClient>();
                services.AddSingleton(VoiceClient.Object);
            });
        }

        /// <summary>
        /// Сбрасывает настройки и историю вызовов обоих моков.
        /// Под каждый тест своя настройка.
        /// </summary>
        public void Reset()
        {
            IdentityClient.Reset();
            VoiceClient.Reset();
        }
    }

    /// <summary>
    /// Сообщает xUnit, что все тестовые классы с атрибутом [Collection("Gateway")]
    /// должны получать один общий GatewayFixture.
    /// </summary>
    [CollectionDefinition("Gateway")]
    public class GatewayCollection : ICollectionFixture<GatewayFixture> { }
}
