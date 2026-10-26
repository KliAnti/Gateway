using gateway.Endpoints;
using gateway.Integrations;
using Serilog;
using Serilog.Enrichers.Span;
using IdentityClient = identity.Grpc.Identity.IdentityClient;
using VoiceClient = voice.Grpc.Voice.VoiceClient;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .Enrich.FromLogContext()
        .Enrich.WithSpan()
        .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}"));

builder.Services.AddGrpcClient<IdentityClient>(options =>
{
    var address = builder.Configuration["Grpc:IdentityAddress"]
        ?? throw new InvalidOperationException("Configuration value 'Grpc:IdentityAddress' is required.");
    options.Address = new Uri(address);
});

builder.Services.AddGrpcClient<VoiceClient>(options =>
{
    var address = builder.Configuration["Grpc:VoiceAddress"]
        ?? throw new InvalidOperationException("Configuration value 'Grpc:VoiceAddress' is required.");
    options.Address = new Uri(address);
});

builder.Services.AddScoped<IdentityIntegration>();
builder.Services.AddScoped<VoiceIntegration>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health");

app.MapIdentityEndpoints();
app.MapVoiceEndpoints();

app.Run();
