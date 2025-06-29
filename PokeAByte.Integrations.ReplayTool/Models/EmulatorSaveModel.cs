namespace PokeAByte.Integrations.ReplayTool.Models;

public record EmulatorSaveModel
{
    public int Frame { get; set; }
    public byte[] SaveState { get; set; } = [];
}