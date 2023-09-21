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

        // Check the count
        var count = await DB.CountAsync<Item>();
        if (count == 0)
        {
            Console.WriteLine("No data - will attempt to seed");

            // Temporary solution of getting the data from our auction service
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }
}
