namespace webapi.Models;
public class ProjectsResponse
{
    public string status { get; set; }
    public ProjectsResult? result { get; set; }
    public ProjectsResponse(string status)
    {
        this.status = status;
    }
}
public class ProjectsResult
{
    public IEnumerable<Project> projects { get; set; } = new List<Project>();
}
public class Project
{
    public int id { get; set; }
    public string? title { get; set; }
    public string? preview_description { get; set; }
    public string? type { get; set; }
    public Budget budget { get; set; } = new Budget();
    public Currency currency { get; set; } = new Currency();
    public BidStats bid_stats { get; set; } = new BidStats();

}
public class Budget
{
    public decimal? minimum { get; set; }
    public decimal? maximum { get; set; }
}
public class Currency
{
    public string? code { get; set; }
    public string? sign { get; set; }
    public string? name { get; set; }
    public decimal exchange_rate { get; set; }
    public string? country { get; set; }
}
public class BidStats
{
    public int? bid_count { get; set; }
    public decimal? bid_avg { get; set; }
}