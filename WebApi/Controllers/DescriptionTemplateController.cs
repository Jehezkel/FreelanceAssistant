using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.DAL;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DescriptionTemplateController : ControllerBase
    {
        private readonly IFLDbContext _fLDbContext;
        private readonly ICurrentUserService _currentUserService;

        public DescriptionTemplateController(IFLDbContext fLDbContext, ICurrentUserService currentUserService)
        {
            _fLDbContext = fLDbContext;
            _currentUserService = currentUserService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DescriptionTemplate[]))]
        public async Task<IActionResult> GetTemplates()
        {
            var result = await _fLDbContext.DescriptionTemplates
                .Where(t => _currentUserService.IsAdmin ? true : t.UserId == _currentUserService.UserId).ToArrayAsync();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTemplate(DescriptionTemplate descriptionTemplate)
        {
            descriptionTemplate.UserId = _currentUserService.UserId!;
            _fLDbContext.DescriptionTemplates.Add(descriptionTemplate);
            await _fLDbContext.SaveChangesAsync();
            return Ok(descriptionTemplate);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTemplate(DescriptionTemplate descriptionTemplate)
        {
            var template = await _fLDbContext.DescriptionTemplates
                .Where(t => _currentUserService.IsAdmin ? true : t.UserId == _currentUserService.UserId && t.Id == descriptionTemplate.Id)
                .FirstOrDefaultAsync();
            if (template is null)
            {
                return NotFound();
            }
            template.Description = descriptionTemplate.Description;
            await _fLDbContext.SaveChangesAsync();
            return Ok(template);
        }
        [HttpDelete]
        [Route("{templateId:int}")]
        public async Task<IActionResult> DeleteTemplate([FromRoute] int templateId)
        {
            var template = await _fLDbContext.DescriptionTemplates
                .Where(t => _currentUserService.IsAdmin ? true : t.UserId == _currentUserService.UserId && t.Id == templateId)
                .FirstOrDefaultAsync();
            if (template is null)
            {
                return NotFound();
            }
            _fLDbContext.DescriptionTemplates.Remove(template);
            await _fLDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
