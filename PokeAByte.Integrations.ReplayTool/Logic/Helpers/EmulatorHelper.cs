using System;
using System.IO;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using BizHawk.Emulation.Common;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic.Helpers;

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

    public static EmulatorSaveModel? SaveState(MainForm mainForm, int lastSize = 0)
    {
        if (mainForm.Emulator is null)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to get state binary: Emulator is null");
            return null;
        }
        var frame = mainForm.Emulator.Frame;
        var statable = mainForm.Emulator?.AsStatable();
        if (statable is null)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to get state binary: Statable emulator is null");
            return null;
        }
        try
        {
            using var memoryStream = lastSize > 0 ?
                new MemoryStream(lastSize) :
                new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);
            statable.SaveStateBinary(binaryWriter);
            binaryWriter.Flush();
            return new EmulatorSaveModel
            {
                Frame = frame,
                SaveState = memoryStream.ToArray()
            };
        }
        catch (Exception e)
        {
            Log.Error(nameof(EmulatorHelper), "Failed to get state binary: {e}", e);
            return null;
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