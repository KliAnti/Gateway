using FluentAssertions;
using Grpc.Core;
using Moq;
using System.Net;
using VoicePingRequest = voice.Grpc.PingRequest;
using VoicePongResponse = voice.Grpc.PongResponse;
using PingResponse = gateway.Contracts.Voice.PingResponse;

namespace gateway.Tests.Tests
{
    /// <summary>
    /// Проверяет эндпоинт /api/voice/ping (запрос - ответ).
    /// </summary>
    [Collection("Gateway")]
    public class Voice
    {
        private readonly ApiClient _client;
        private readonly GatewayFixture _fixture;

        /// <summary>
        /// Получает общую фикстуру и сбрасывает моки.
        /// </summary>
        /// <param name="fixture">Фикстура с поднятым gateway и моками клиентов (микросервисов).</param>
        public Voice(GatewayFixture fixture)
        {
            _fixture = fixture;
            _fixture.Reset();
            _client = new ApiClient(fixture.CreateClient());
        }

        [Fact]
        public async Task Ping_WhenMessageIsNotProvided_ReturnDefaultMessage()
        {
            // Arrange
            VoicePingRequest? request = null;
            _fixture.VoiceClient
                .Setup(c => c.PingAsync(It.IsAny<VoicePingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Callback<VoicePingRequest, Metadata, DateTime?, CancellationToken>((req, _, _, _) => request = req)
                .Returns(GrpcCall.Ok(new VoicePongResponse { Message = "pong" }));

            // Act
            var response = await _client.PingVoice();
            var body = await _client.ReadAsAsync<PingResponse>(response);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body!.Message.Should().Be("pong");
            request!.Message.Should().Be("Ping");
        }

        [Fact]
        public async Task Ping_WhenMessageIsProvided_ReturnOk()
        {
            // Arrange
            VoicePingRequest? request = null;
            _fixture.VoiceClient
                .Setup(c => c.PingAsync(It.IsAny<VoicePingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Callback<VoicePingRequest, Metadata, DateTime?, CancellationToken>((req, _, _, _) => request = req)
                .Returns(GrpcCall.Ok(new VoicePongResponse { Message = "custom pong" }));

            // Act
            var response = await _client.PingVoice("hello");
            var body = await _client.ReadAsAsync<PingResponse>(response);

            // Assert
            body!.Message.Should().Be("custom pong");
            request!.Message.Should().Be("hello");
        }

        [Fact]
        public async Task Ping_WhenIdentityServiceIsUnavailable_ReturnsInternalServerError()
        {
            // Arrange
            _fixture.VoiceClient
                .Setup(c => c.PingAsync(It.IsAny<VoicePingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns(GrpcCall.Error<VoicePongResponse>(StatusCode.Unavailable));

            // Act
            var response = await _client.PingVoice();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
