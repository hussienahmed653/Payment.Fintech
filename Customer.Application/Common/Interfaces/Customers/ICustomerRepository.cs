namespace Customer.Application.Common.Interfaces.Customers;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerResponse>> GetCustomerAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerResponse>> GetCustomerByBusinessTypeAsync(string businessType, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Customer> GetCustomerByGuidAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Customer>> SearchAsync(string search, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Customer>> FilterAsync(CustomerFilterSpecification spec, CancellationToken cancellationToken = default);
    Task<bool> CustomerIsExistsAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<bool> EmailIsExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default);
    Task UpdateCustomerAsync(Domain.Entities.Customer customer, CancellationToken cancellationToken = default);
    Task DeleteCustomerAsync(Domain.Entities.Customer customer, CancellationToken cancellationToken = default);
}
