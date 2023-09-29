using AuctionService.Controllers;
using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AuctionService.UnitTests;

public class AuctionControllerTests
{
    private readonly Mock<IAuctionRepository> _auctionRepo;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Fixture _fixture; // from the AutoFixture package
    private readonly AuctionsController _controller;
    private readonly IMapper _mapper;


    // XUnit reinitializes anything we have in the constructor for each test
    public AuctionControllerTests()
    {
        _fixture = new Fixture();
        _auctionRepo = new Mock<IAuctionRepository>();
        _publishEndpoint = new Mock<IPublishEndpoint>();

        var mockMapper = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        _mapper = new Mapper(mockMapper);
        _controller = new AuctionsController(_auctionRepo.Object, _mapper, _publishEndpoint.Object);
    }
}
