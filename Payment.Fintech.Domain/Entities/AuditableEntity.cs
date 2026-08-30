namespace Payment.Fintech.Domain.Entities;

public class AuditableEntity
{
    public Guid CreatedByGuid { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public Guid? UpdatedByGuid { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
