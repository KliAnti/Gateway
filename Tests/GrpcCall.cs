using Grpc.Core;
using Grpc.Core.Testing;

namespace gateway.Tests
{
    /// <summary>
    /// Собирает ответы gRPC-вызовов для настройки моков.
    /// </summary>
    public static class GrpcCall
    {
        /// <summary>
        /// Собирает ответ сервиса для успешных (happy) тестов.
        /// </summary>
        /// <param name="response">Тело ответа, возвращаемое моком.</param>
        public static AsyncUnaryCall<TResponse> Ok<TResponse>(TResponse response)
            => TestCalls.AsyncUnaryCall(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { });

        /// <summary>
        /// Собирает ответ сервиса для упавших тестов.
        /// </summary>
        /// <param name="response">gRPC-статус упавшего вызова.</param>
        public static AsyncUnaryCall<TResponse> Error<TResponse>(StatusCode statusCode)
        {
            var status = new Status(statusCode, statusCode.ToString());

            return TestCalls.AsyncUnaryCall<TResponse>(
                Task.FromException<TResponse>(new RpcException(status)),
                Task.FromResult(new Metadata()),
                () => status,
                () => new Metadata(),
                () => { });
        }
    }
}
