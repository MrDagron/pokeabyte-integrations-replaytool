namespace PokeAByte.Integrations.ReplayTool.Models;

public record RecordingSettings
{
    public int SaveStateIntervalMs { get; set; } = 1000; //every 1 second
    public int KeyframeIntervalCount { get; set; } = 60; //every 60 states will make a keyframe
    public string FileName { get; set; } = "";
    public string FilePath { get; set; } = "";
}