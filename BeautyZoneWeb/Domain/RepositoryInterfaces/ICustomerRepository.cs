using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllCustomers();
    Task<Customer> GetCustomerByPhonenumber(string number);
    Task CreateCustomer(Customer customer);
    Task UpdateCustomer(Customer customer);
    Task DeleteCustomer(Customer customer);
}