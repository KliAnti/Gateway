using gateway.Contracts.Voice;
using VoiceClient = voice.Grpc.Voice.VoiceClient;
using VoicePingRequest = voice.Grpc.PingRequest;

namespace gateway.Integrations
{
    /// <summary>
    /// Ходит в сервис voice по gRPC и переводит его ответы в формат, понятный внешнему миру.
    /// </summary>
    public sealed class VoiceIntegration
    {
        private readonly VoiceClient _grpcClient;

        /// <summary>
        /// Принимает уже готового gRPC-клиента voice — его собирает и передаёт DI-контейнер.
        /// </summary>
        /// <param name="grpcClient">gRPC-клиент, через который идут вызовы к сервису voice.</param>
        public VoiceIntegration(VoiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        /// <summary>
        /// Отправляет voice тестовое сообщение и возвращает то, что он ответил.
        /// </summary>
        /// <param name="message">Текст, который нужно отправить в voice.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Ответ voice в собсвтенном формате gateway.</returns>
        public async Task<PingResponse> PingAsync(string message, CancellationToken cancellationToken)
        {
            var grpcResponse = await _grpcClient.PingAsync(
                new VoicePingRequest
                {
                    Message = message
                },
                cancellationToken: cancellationToken);

            return new PingResponse(grpcResponse.Message);
        }
    }
}
