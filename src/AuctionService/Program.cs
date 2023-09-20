using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    // These are the DbContextOptions passed in to the AuctionDbContext constructor
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers(); // directs HTTP requests to the correct endpoint

try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
