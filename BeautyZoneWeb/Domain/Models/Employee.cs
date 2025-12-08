namespace Domain.Models;

public class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual List<Procedure> Procedures { get; set; }
}