using System;
using System.IO;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using BizHawk.Emulation.Common;

namespace PokeAByte.Integrations.ReplayTool.Logic;

public static class EmulatorHelper
{
    public static int GetFrame(MainForm mainForm)
    {
        if (mainForm.Emulator is not null)
        {
            return mainForm.Emulator.Frame;
        }
        Log.Error(nameof(EmulatorHelper),"Failed to get frame: Emulator is null");
        return -1;
    }

    public static byte[] GetStateBinary(MainForm mainForm, int lastSize = 0)
    {
        var statable = mainForm.Emulator?.AsStatable();
        if (statable is null)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to get state binary: Emulator is null");
            return [];
        }
        try
        {
            using var memoryStream = lastSize > 0 ?
                new MemoryStream(lastSize) :
                new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);
            statable.SaveStateBinary(binaryWriter);
            binaryWriter.Flush();
            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to get state binary: {e}", e);
            return [];
        }
    }

    public static void LoadStateBinary(MainForm mainForm, byte[] stateBinary)
    {
        var statable = mainForm.Emulator?.AsStatable();
        
        if (statable is null)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to load state binary: Emulator is null");
            return;
        }

        if (stateBinary.Length == 0)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to load state binary: State binary is empty");
            return;
        }

        statable.LoadStateBinary(stateBinary);
    }
}