namespace Do.Commission.Infrastructure.Model;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public decimal Salary { get; set; }

    public ICollection<PositionHistory> PositionHistories { get; set; } = [];
}
