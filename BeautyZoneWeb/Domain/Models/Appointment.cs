namespace Domain.Models;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid ProcedureId { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Procedure Procedure { get; set; }
    public virtual Customer Customer { get; set; }
}