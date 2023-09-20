using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    // calls the base class with options - this is where our connection string is passed
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    // Creating the Auctions set explicitly, but not Items as Item is related to the Auction, 
    // so EF knows to create it implicitly because they're related
    public DbSet<Auction> Auctions { get; set; }
}
