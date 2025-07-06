namespace PokeAByte.Integrations.ReplayTool.Models;

public record Keyframe
{
    public required int Frame { get; init; }
    public required byte[] SaveState { get; init; } = [];
}