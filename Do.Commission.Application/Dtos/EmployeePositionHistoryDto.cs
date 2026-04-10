using System.Text.Json.Serialization;

namespace Do.Commission.Application.Dtos;

public sealed class EmployeePositionHistoryDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("employee_id")]
    public int EmployeeId { get; set; }

    [JsonPropertyName("position_id")]
    public int PositionId { get; set; }

    [JsonPropertyName("department_id")]
    public int? DepartmentId { get; set; }

    [JsonPropertyName("department_name")]
    public string? DepartmentName { get; set; }

    [JsonPropertyName("project_id")]
    public int? ProjectId { get; set; }

    [JsonPropertyName("project_name")]
    public string? ProjectName { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("reason_change")]
    public string ReasonChange { get; set; } = string.Empty;
}
