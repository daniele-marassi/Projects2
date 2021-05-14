using System.Text.Json;

namespace Mair.DS.Common.JsonDeserializers
{
    public static class MairJsonSerializer
    {
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
