using System.Text.Json;
using MongoDB.Entities;

namespace SearchService;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        // First param is the name of the db
        await DB.InitAsync("SearchDb", MongoDB.Driver.MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        // Creating an index for the properties that we will want to search for
        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        // We use 'using' as we can't inject the service here
        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine(items.Count + " returned from the auction service");

        if (items.Count > 0) await DB.SaveAsync(items);
    }
}
