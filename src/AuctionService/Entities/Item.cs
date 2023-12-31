﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;

[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }

    // Nav properties
    // Properties that define the relationship with the Auction entity/table
    public Auction Auction { get; set; }
    // Convention based foreign-key
    public Guid AuctionId { get; set; }
}
