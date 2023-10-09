using AutoMapper;
using Contracts;
using MongoDB.Driver.Core.Operations;

namespace BiddingService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Bid, BidDto>();
        CreateMap<Bid, BidPlaced>();
    }
}
