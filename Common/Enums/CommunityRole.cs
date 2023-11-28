using System.Text.Json.Serialization;

namespace Blog.API.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityRole
{
    Administrator,
    Subscriber
}