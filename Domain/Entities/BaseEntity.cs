namespace Domain.Entities
{
    // Base entity that other entities derive from
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}