using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokeAByte.Integrations.ReplayTool.Models;

public sealed class RecordedSaveState : IComparable<RecordedSaveState>
{
    public int Frame { get; set; }
    public bool IsFlagged { get; set; } = false;
    public string FlagName { get; set; } = "";
    public long SaveTimeMs { get; set; }
    public byte[] StateDelta { get; set; } = [];
    [JsonIgnore] public byte[] FullState { get; set; } = [];
    public bool IsKeyframe { get; set; }

    /*public override string ToString() => !string.IsNullOrWhiteSpace(FlagName) ? 
        $"Frame #{Frame} - {FlagName}" : 
        $"Frame #{Frame}";*/
    public override string ToString()
    {
        var frameName = $"Frame #{Frame}";
        if (!string.IsNullOrWhiteSpace(FlagName))
        {
            frameName += $" - {FlagName}";
        }
        if (IsKeyframe)
        {
            frameName += " (Keyframe)";       
        }
        return frameName;
    }
    public int CompareTo(RecordedSaveState? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null ? 1 : Frame.CompareTo(other.Frame);
    }
}