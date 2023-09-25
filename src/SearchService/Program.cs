using System.Net;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x =>
{
    // Any other consumers added from the same namespace as this class will be registered to MassTransit
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    // Kebab case example: search-auction-created
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            // guest is a default value that will be used if RabbitMq:Username doesn't return anything
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        // e is endpoint formatter
        config.ReceiveEndpoint("search-auction-created", e =>
        {
            // First param is number of retrys to attempt, second is the interval between each retry
            e.UseMessageRetry(r => r.Interval(5, 5));

            // apply the retry policy for the AuctionCreatedConsumer only
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });

        config.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError() // Transient failure means a failure that will hopefully not be a failure in the future
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound) // Just an example, we make no use of this. Not really a transient error because if it's not found once, it's not gonna be found later
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3)); // Retry every 3 seconds forever untill we get a success