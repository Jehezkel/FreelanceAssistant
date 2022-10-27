namespace WebApi.FreelanceQueries;

public record ProjectsFilter
{
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public IReadOnlyList<int> Jobs { get; init; } = new List<int>();
}