namespace WebApi.Models
{
    public class DescriptionTemplate
    {
        public int Id { get; set; }
        public AppUser User { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
