namespace GGEdu.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
