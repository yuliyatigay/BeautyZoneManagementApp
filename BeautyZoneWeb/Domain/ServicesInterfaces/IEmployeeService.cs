using Domain.Models;

namespace Domain.ServicesInterfaces;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllEmployees();
    Task CreateEmployee(Employee employee);
    Task<Employee> GetEmployeeById(Guid Id);
    Task<List<Employee>> GetEmployeeByProcedure(string procedureName);
    Task UpdateEmployee(Employee employee);
    Task DeleteEmployee(Employee employee);
}