using System.Text.Json.Serialization;

namespace WebApplication.Data
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DataType
    {
        HUMIDITY,
        CO2,
        TEMPERATURE
    }
}