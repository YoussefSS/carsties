using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace NotificationService;

public class BidPlacedConsumer : IConsumer<BidPlacedConsumer>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<BidPlacedConsumer> context)
    {
        Console.WriteLine("--> Bid placed message received");

        // We'll send a notification to every connected client (no authentication)
        await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}
