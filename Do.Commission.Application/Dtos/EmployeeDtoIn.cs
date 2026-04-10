using Do.Commission.Application.Converters;
using System.Text.Json.Serialization;

namespace Do.Commission.Application.Dtos;

public sealed class EmployeeDtoIn
{
    [JsonPropertyName("documento")]
    [JsonIgnore (Condition = JsonIgnoreCondition.Never)]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int? PositionId { get; set; }

    [JsonPropertyName("salary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonConverter(typeof(OneDecimalConverter))]
    public decimal? Salary { get; set; }

}
