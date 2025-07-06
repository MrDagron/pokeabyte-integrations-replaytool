using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokeAByte.Integrations.ReplayTool.Models;

public class ReplayFile
{
    public byte[] FirstState { get; set; } = [];
    public List<RecordedSaveState> States { get; set; } = [];
    public List<int> Keyframes { get; set; } = [];
}