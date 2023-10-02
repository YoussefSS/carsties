using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AuctionService.IntegrationTests;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        // Will start a running instance of our test database as a docker container
        await _postgreSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // DbContext: We must manually remove it before adding our test container

            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

            // Removing the already existing production database
            if (descriptor != null) services.Remove(descriptor);

            // Replacing the production database with the test container
            services.AddDbContext<AuctionDbContext>(options =>
            {
                options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            });

            // MassTransit: This will automatically remove the config we have in Program.cs and replace it
            services.AddMassTransitTestHarness();

            // Migrating: Setting up the test database with our schema
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AuctionDbContext>();

            db.Database.Migrate();
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}
