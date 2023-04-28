using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarRail.FpsUnlocker;

internal sealed class GraphicsSettings
{
    [JsonPropertyName("FPS")]
    public int Fps { get; set; } = default!;

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}