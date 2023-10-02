using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebMotions.Fake.Authentication.JwtBearer;

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
            services.RemoveDbContext();

            // Replacing the production database with the test container
            services.AddDbContext<AuctionDbContext>(options =>
            {
                options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            });

            // MassTransit: This will automatically remove the config we have in Program.cs and replace it
            services.AddMassTransitTestHarness();

            // Migrating: Setting up the test database with our schema
            services.EnsureCreated();

            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme)
                .AddFakeJwtBearer(opt =>
                {
                    opt.BearerValueType = FakeJwtBearerBearerValueType.Jwt;
                });
        });
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}
