namespace PokeAByte.Integrations.ReplayTool.Models;

public record PlaybackState
{
    public required int Frame { get; init; }
    public required bool IsFlagged { get; init; }
    public required string FlagName { get; init; } = "";
    public required byte[] SaveState { get; init; } = [];
    public required bool IsKeyframe { get; init; }
}