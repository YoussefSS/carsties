using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    // No need to dependency inject mongo as it can be used as a static class
    // https://mongodb-entities.com/wiki/Get-Started.html

    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm, int pageNumber = 1, int pageSize = 4)
    {
        var query = DB.PagedSearch<Item>();

        query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrEmpty(searchTerm))
        {
            // Search.Full means a full text search
            // SortByTextScore is useful with a large database
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(new // anonymous object
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
