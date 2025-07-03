using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokeAByte.Integrations.ReplayTool.Models;

public record SaveStateModel
{
    public byte[] FirstState { get; set; } = [];
    [JsonIgnore] public bool HasBeenReconstructed { get; set; } = false;
    public List<SaveState> SaveStates { get; set; } = [];
    public List<int> Keyframes { get; set; } = [];
}

public record SaveState : IComparable<SaveState>
{
    public int Key { get; set; }
    public int Frame { get; set; }
    public bool IsFlagged { get; set; } = false;
    public string FlagName { get; set; } = "";
    public long SaveTimeMs { get; set; }
    public byte[] StateDelta { get; set; } = [];
    [JsonIgnore] public byte[] FullState { get; set; } = [];
    public bool IsKeyframe { get; set; }

    public override string ToString() => !string.IsNullOrWhiteSpace(FlagName) ? 
        $"#{Key} - Frame #{Frame} - {FlagName}" : 
        $"#{Key} - Frame #{Frame}";

    public int CompareTo(SaveState? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null ? 1 : Key.CompareTo(other.Key);
    }
}