using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;

namespace BusinessLogic.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<List<Customer>> GetAllCustomers()
    {
        return await _customerRepository.GetAllCustomers();
    }

    public async Task<Customer> GetCustomerByPhonenumber(string number)
    {
        return await _customerRepository.GetCustomerByPhonenumber(number);
    }

    public async Task CreateCustomer(Customer customer)
    {
        await _customerRepository.CreateCustomer(customer);
    }

    public async Task UpdateCustomer(Customer customer)
    {
        await _customerRepository.UpdateCustomer(customer);
    }

    public async Task DeleteCustomer(Customer customer)
    {
        await _customerRepository.DeleteCustomer(customer);
    }
}