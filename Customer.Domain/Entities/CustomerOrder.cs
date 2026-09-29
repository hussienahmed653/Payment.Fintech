using Customer.Domain.Enums;
using System.Reflection.Metadata.Ecma335;

namespace Customer.Domain.Entities;

public class CustomerOrder
{
    public Guid Guid { get; set; } = Guid.CreateVersion7();
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public CustomerStatus Status { get; set; } = CustomerStatus.Pending;
    public string? Description { get; set; }
}
