using Do.Commission.Application.Converters;
using System.Text.Json.Serialization;

namespace Do.Commission.Application.Dtos;

public sealed class EmployeeDto
{
    [JsonPropertyName("documento_key")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("position")]
    public int PositionId { get; set; }

    [JsonPropertyName("salary")]
    [JsonConverter(typeof(OneDecimalConverter))]
    public decimal Salary { get; set; }

    [JsonPropertyName("commission")]
    [JsonConverter(typeof(OneDecimalConverter))]
    public decimal Commission { get; set; }


}
