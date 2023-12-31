﻿using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

// Consumers should end with "Consumer" due to MassTransits convention based system.
public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        // context.Message is of type AuctionCreated
        Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        // Just an example to generate an exception and see how it's handled in the error queue
        if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");

        // throws an exception if the save is not successful
        await item.SaveAsync();
    }
}
