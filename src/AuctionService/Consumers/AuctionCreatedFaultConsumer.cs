using Contracts;
using MassTransit;

namespace AuctionService;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> Consuming faulty creation");

        var exception = context.Message.Exceptions.First();

        // We know that this is our "Foo" error that we throw on purpose
        if (exception.ExceptionType == "System.ArgumentException")
        {
            // Change the model to something we know will be accepted
            context.Message.Message.Model = "FooBar";

            // Republish the message
            await context.Publish(context.Message.Message);
        }
        else
        {
            Console.WriteLine("Not an argument exception - update error dashboard somewhere");
        }
    }
}
