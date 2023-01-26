namespace WebApi.DTOs.ProjectSearch;

public class SearchInputDTO
{
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public List<JobDto> Jobs { get; set; } = new List<JobDto>();
}
public class JobDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}