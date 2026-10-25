using Serilog;
using Serilog.Enrichers.Span;
using gateway.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .Enrich.FromLogContext()
        .Enrich.WithSpan()
        .WriteTo.Console(
            outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}"));

builder.Services.AddGrpc(options => options.Interceptors.Add<TraceIdInterceptor>());

builder.Services.AddGrpcHealthChecks();
builder.Services.AddHealthChecks().AddCheck("gateway", () => HealthCheckResult.Healthy());

var app = builder.Build();

app.MapGet("/", () => "Hello world");

app.MapGrpcHealthChecksService();

app.Run();
