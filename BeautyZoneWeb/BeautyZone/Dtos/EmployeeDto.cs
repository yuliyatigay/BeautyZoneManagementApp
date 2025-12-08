namespace BeautyZone.Dtos;

public class EmployeeDto
{
    public string Name { get; set; }
    public virtual List<ProcedureDto> Procedures { get; set; }
}