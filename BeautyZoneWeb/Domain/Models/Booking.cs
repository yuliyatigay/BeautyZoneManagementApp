namespace Domain.Models;

public class Booking
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual BeautyTech Specialist { get; set; }
    public virtual Procedure Procedure { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}