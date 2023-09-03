namespace Domain.Entities
{
    // Base entity that other entities derive from, contains the id and created at properties
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}