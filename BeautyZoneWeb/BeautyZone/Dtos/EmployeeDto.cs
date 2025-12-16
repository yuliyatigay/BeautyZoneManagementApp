namespace BeautyZone.Dtos;

public class EmployeeDto
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public virtual List<Guid> Procedures { get; set; }
}