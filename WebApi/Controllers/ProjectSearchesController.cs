using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DAL;
using WebApi.DTOs.ProjectSearch;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class ProjectSearchController : ControllerBase
{
    private readonly IFLDbContext _fLDbContext;
    private readonly ICurrentUserService _currentUserService;

    public ProjectSearchController(IFLDbContext fLDbContext, ICurrentUserService currentUserService)
    {
        _fLDbContext = fLDbContext;
        _currentUserService = currentUserService;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectSearch[]))]
    public async Task<IActionResult> GetProjectSearches()
    {
        var result = await _fLDbContext.ProjectSearches
            .Where(p => _currentUserService.IsAdmin ? true : p.UserId == _currentUserService.UserId).ToArrayAsync();
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateProjectSearch(ProjectSearch projectSearch)
    {
        projectSearch.UserId = _currentUserService.UserId!;
        _fLDbContext.ProjectSearches.Add(projectSearch);
        await _fLDbContext.SaveChangesAsync();
        return Ok(projectSearch);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateTemplate(ProjectSearch projectSearch)
    {
        var search = await _fLDbContext.ProjectSearches
            .Where(p => _currentUserService.IsAdmin ? true : p.UserId == _currentUserService.UserId && p.Id == projectSearch.Id)
            .FirstOrDefaultAsync();
        if (search is null)
        {
            return NotFound();
        }
        // template.Description = projectSearch.Description;
        await _fLDbContext.SaveChangesAsync();
        return Ok(search);
    }
    [HttpDelete]
    [Route("{searchId:int}")]
    public async Task<IActionResult> DeleteTemplate([FromRoute] int searchId)
    {
        var search = await _fLDbContext.ProjectSearches
            .Where(p => _currentUserService.IsAdmin ? true : p.UserId == _currentUserService.UserId && p.Id == searchId)
            .FirstOrDefaultAsync();
        if (search is null)
        {
            return NotFound();
        }
        _fLDbContext.ProjectSearches.Remove(search);
        await _fLDbContext.SaveChangesAsync();
        return Ok();
    }
}