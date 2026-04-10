using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do.Commission.Infrastructure.Model;

public class PositionHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(Position))]
    public int PositionId { get; set; }

    [ForeignKey(nameof(Department))]
    public int? DepartmentId { get; set; }

    [ForeignKey(nameof(Projects))]
    public int? ProjectId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReasonChange { get; set; } = string.Empty;

    public Employee Employee { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public Department? Department { get; set; }
    public Project? Projects { get; set; }
}
