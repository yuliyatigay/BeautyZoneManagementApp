using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomers();
    Task<Customer> GetCustomerById(Guid id);
    Task CreateCustomer(Customer customer);
    Task UpdateCustomer(Customer customer);
    Task DeleteCustomer(Customer customer);
}
