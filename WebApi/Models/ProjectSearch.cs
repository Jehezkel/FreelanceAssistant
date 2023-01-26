using System.Text.Json.Serialization;
using WebApi.DTOs.ProjectSearch;
using WebApi.FreelanceQueries;

namespace WebApi.Models;

public class ProjectSearch
{
    public int Id { get; set; }
    [JsonIgnore]
    public AppUser? User { get; set; }
    [JsonIgnore]
    public string? UserId { get; set; }
    public SearchInputDTO Input { get; set; } = null!;
    public string UserName { get { return this.User?.UserName ?? "missing"; } }

}