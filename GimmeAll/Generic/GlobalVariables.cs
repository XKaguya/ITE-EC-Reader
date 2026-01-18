using System.Text.Json;
using System.Text.Json.Serialization;

namespace ITE_EC_Reader.Generic;

public class GlobalVariables
{
    [JsonPropertyName("EcAddrPort")]
    public byte EcAddrPort { get; set; } = 0x4E;

    [JsonPropertyName("EcDataPort")]
    public byte EcDataPort { get; set; } = 0x4F;

    [JsonPropertyName("EcAddrPort2")]
    public byte EcAddrPort2 { get; set; } = 0x2E;

    [JsonPropertyName("EcDataPort2")]
    public byte EcDataPort2 { get; set; } = 0x2F;

    [JsonPropertyName("Address")]
    public ushort Address { get; set; } = 0x0000;

    [JsonIgnore]
    public bool UseSecondaryPorts { get; set; } = false;

    [JsonIgnore]
    public byte ActiveAddr => UseSecondaryPorts ? EcAddrPort2 : EcAddrPort;

    [JsonIgnore]
    public byte ActiveData => UseSecondaryPorts ? EcDataPort2 : EcDataPort;

    public void SaveToFile(string filePath)
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public static GlobalVariables LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return new GlobalVariables();
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<GlobalVariables>(json) ?? new GlobalVariables();
    }
}