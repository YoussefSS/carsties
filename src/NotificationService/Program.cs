using MassTransit;
using NotificationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            // guest is a default value that will be used if RabbitMq:Username doesn't return anything
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        config.ConfigureEndpoints(context);
    });
});
builder.Services.AddSignalR();

var app = builder.Build();

// string is the route
app.MapHub<NotificationHub>("/notifications");

app.Run();
