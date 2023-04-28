using System.Text.Json.Serialization;

namespace StarRail.FpsUnlocker;

[JsonSerializable(typeof(GraphicsSettings))]
internal sealed partial class GraphicsSettingsContext : JsonSerializerContext
{
}