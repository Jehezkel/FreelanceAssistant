using WebApi.Models;

namespace WebApi.Services
{
    public interface IDTemplateService
    {
        public Task<DescriptionTemplate> GetTemplates();
        public Task<int> CreateTemplate(DescriptionTemplate template);
        public Task UpdateTemplate(DescriptionTemplate template);
        public Task DeleteTemplate(int templateId);
    }
}
