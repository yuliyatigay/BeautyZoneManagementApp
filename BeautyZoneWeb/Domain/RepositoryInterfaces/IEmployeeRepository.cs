using Domain.Models;

namespace Domain.RepositoryInterfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployees();
    Task CreateEmployee(Employee master);
    Task<Employee> GetEmployeeById(Guid id);
    Task<Employee> GetEmployeeByPhonenumber(string number);
    Task<List<Employee>> GetEmployeesByProcedure(string procedureName);
    Task UpdateEmployee(Employee master);
    Task DeleteEmployee(Employee master);
}