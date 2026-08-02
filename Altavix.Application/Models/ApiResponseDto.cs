using System.Text.Json.Serialization;
using Altavix.Application.Enums;

namespace Altavix.Application.Models;

public class ApiResponseDto<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    
    [JsonIgnore]
    public ResponseMessageType Type { get; set; } = ResponseMessageType.Info;
    
    [JsonPropertyName("messageType")]
    public string MessageType => Type.ToString().ToLower();
}
