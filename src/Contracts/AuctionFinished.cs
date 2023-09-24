namespace Contracts;

public class AuctionFinished
{
    public bool ItemSold { get; set; } // Auction has met reseve price and was sold
    public string AuctionId { get; set; }
    public string Winner { get; set; }
    public string Seller { get; set; }
    public int? Amount { get; set; }
}
