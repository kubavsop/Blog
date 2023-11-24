using System.Text.Json.Serialization;

namespace Blog.API.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female
}