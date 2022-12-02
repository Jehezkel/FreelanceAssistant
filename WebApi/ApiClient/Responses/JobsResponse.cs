using System.Collections.Generic;

namespace WebApi.ApiClient.Responses;

public record JobResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public CategoryResponse Category { get; init; } = null!;
}
public record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}

//"id": 84,
//            "name": "Building Architecture",
//            "category": {
//    "id": 3,
//                "name": "Design, Media & Architecture"
//            },
//            "active_project_count": null,
//            "seo_url": "building_architecture",
//            "seo_info": null,
//            "local": false