using WebApi.Models;

namespace WebApi.Services
{
    public interface IDTemplateService
    {
        public Task<BidTemplate> GetTemplates();
        public Task<int> CreateTemplate(BidTemplate template);
        public Task UpdateTemplate(BidTemplate template);
        public Task DeleteTemplate(int templateId);
    }
}
