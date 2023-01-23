using System.Text.Json.Serialization;
using WebApi.FreelanceQueries;

namespace WebApi.Models;

public class ProjectSearch
{
    public int Id { get; set; }
    [JsonIgnore]
    public AppUser? User { get; set; }
    [JsonIgnore]
    public string? UserId { get; set; }
    public ActiveProjectsInput Input { get; set; } = null!;

}