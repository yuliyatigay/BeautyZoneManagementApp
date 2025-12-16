using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServicesInterfaces;

namespace BusinessLogic.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<List<Employee>> GetAllEmployees()
    {
        return await _employeeRepository.GetAllEmployees();
    }

    public async Task<Employee> CreateEmployee(Employee employee)
    {
        var existing = await _employeeRepository.GetEmployeeByPhonenumber(employee.PhoneNumber);
        if (existing != null)
            return existing; 
        await _employeeRepository.CreateEmployee(employee);
        return employee;
    }

    public async Task<Employee> GetEmployeeById(Guid Id)
    {
        return await _employeeRepository.GetEmployeeById(Id);
    }

    public async Task<List<Employee>> GetEmployeeByProcedure(string procedureName)
    {
        return await _employeeRepository.GetEmployeesByProcedure(procedureName);
    }

    public async Task UpdateEmployee(Employee employee)
    {
        await _employeeRepository.UpdateEmployee(employee);
    }

    public async Task DeleteEmployee(Employee employee)
    {
        await _employeeRepository.DeleteEmployee(employee);
    }
}