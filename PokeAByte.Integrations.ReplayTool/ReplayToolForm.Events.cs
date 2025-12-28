using System;
using System.IO.Compression;
using BizHawk.Client.Common;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic;

namespace PokeAByte.Integrations.ReplayTool;

//General events for GUI elements
public partial class ReplayToolForm
{
    private void LoadReplay_OnClick(object sender, EventArgs e)
    {
        var index = 0;
        var result = PokeAByteMainForm
            .DialogController
            .ShowFileMultiOpenDialog(
                dialogParent: PokeAByteMainForm, 
                filterStr: "Replay Files (*.stpreplay)|*.stpreplay|All Files (*.*)|*.*",
                filterIndex: ref index,
                initDir: !string.IsNullOrWhiteSpace(_recordingSettings.FilePath) ? 
                    _recordingSettings.FilePath : 
                    _assemblyDirectory,
                windowTitle: "Load Replay")?[0];
        if (result is null || string.IsNullOrWhiteSpace(result))
        {
            PokeAByteMainForm.ShowMessageBox("No file was selected", "Error");;
            return;
        }
        //get the filename
        var filename = result[(result.LastIndexOf('\\') + 1)..result.LastIndexOf('.')];
        Log.Error("", $"F: {filename}");
        //extract the data for the replay
        ZipFile.ExtractToDirectory(result, $"{result}_tmp");
        //Load movie file
        BizHawkMovie.PlayMovie(PokeAByteMainForm, $"{result}_tmp", filename);
        //load replay json
        //load replay
    }
}