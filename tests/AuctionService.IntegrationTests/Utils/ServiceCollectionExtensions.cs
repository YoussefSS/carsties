using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

public static class ServiceCollectionExtensions
{
    public static void RemoveDbContext(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

        // Removing the already existing production database
        if (descriptor != null) services.Remove(descriptor);
    }

    public static void EnsureCreated(this IServiceCollection services)
    {
        // Migrating: Setting up the test database with our schema
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<AuctionDbContext>();

        db.Database.Migrate();

        // Seeding test data
        DbHelper.InitDbForTests(db);
    }
}
