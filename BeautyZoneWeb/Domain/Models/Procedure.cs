namespace Domain.Models;

public class Procedure
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual List<Customer> Customers { get; set; }
    public virtual List<Employee> Employees { get; set; }
}