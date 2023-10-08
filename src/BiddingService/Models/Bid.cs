using MongoDB.Entities;

namespace BiddingService;

public class Bid : Entity
{
    public string AuctionId { get; set; }
    public string Bidder { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow; // Matches BidTime in the BidPlaced contract
    public int Amount { get; set; }
    public BidStatus BidStatus { get; set; }
}
