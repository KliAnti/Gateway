using gateway.Contracts.Identity;
using IdentityClient = identity.Grpc.Identity.IdentityClient;
using IdentityPingRequest = identity.Grpc.PingRequest;

namespace gateway.Integrations
{
    /// <summary>
    /// Ходит в сервис identity по gRPC и переводит его ответы в формат, понятный внешнему миру.
    /// </summary>
    public sealed class IdentityIntegration
    {
        private readonly IdentityClient _grpcClient;

        /// <summary>
        /// Принимает уже готового gRPC-клиента identity — его собирает и передаёт DI-контейнер.
        /// </summary>
        /// <param name="grpcClient">gRPC-клиент, через который идут вызовы к сервису identity.</param>
        public IdentityIntegration(IdentityClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        /// <summary>
        /// Отправляет identity тестовое сообщение и возвращает то, что он ответил.
        /// </summary>
        /// <param name="message">Текст, который нужно отправить в identity.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Ответ identity в собсвтенном формате gateway.</returns>
        public async Task<PingResponse> PingAsync(string message, CancellationToken cancellationToken)
        {
            var grpcResponse = await _grpcClient.PingAsync(
                new IdentityPingRequest 
                { 
                    Message = message 
                },
                cancellationToken: cancellationToken);

            return new PingResponse(grpcResponse.Message);
        }
    }
}
