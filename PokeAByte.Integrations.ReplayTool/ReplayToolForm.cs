using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Logic.Services;

namespace PokeAByte.Integrations.ReplayTool;

[ExternalTool("PokeAByte.Integrations.ReplayTool")]
public sealed partial class ReplayToolForm : ToolFormBase, IExternalToolForm
{
    protected override string WindowTitleStatic => "PokeAByte Replay Tool";
    private MainForm PokeAByteMainForm => (MainForm)MainForm;
    
    private readonly SaveStateService _saveStateService;
    public ReplayToolForm()
    {
        _saveStateService = new SaveStateService();
        ConfigureSaveStateTimer();
        InitializeComponent();
    }
    public override void Restart() {
        // executed once after the constructor, and again every time a rom is loaded or reloaded
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        if (!string.IsNullOrWhiteSpace(assemblyDirectory))
        {
            assemblyDirectory = assemblyDirectory.Substring(0, assemblyDirectory.LastIndexOf('\\'));
        }

        var path = assemblyDirectory ?? "";
        path += "\\saveStates.json";
        _saveStateService.LoadFromFile(path);
        _saveStateService.ReconstructSaveStates();
    }

    private int test = 0;
    protected override void UpdateAfter() {
        // executed after every frame (except while turboing, use FastUpdateAfter for that)
        SaveState();
        var state = _saveStateService.GetReconstructedState(test++);
        if (state is not null)
            EmulatorHelper.LoadStateBinary(PokeAByteMainForm, state);
    }
}