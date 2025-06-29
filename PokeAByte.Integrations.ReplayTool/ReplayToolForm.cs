using System.Threading;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
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
        
        //TEMP
        _saveStateTimer?.Start();
        //TEMP
        _isRecording = true;
        
        
        InitializeComponent();
    }
    public override void Restart() {
        // executed once after the constructor, and again every time a rom is loaded or reloaded
    }

    protected override void UpdateAfter() {
        // executed after every frame (except while turboing, use FastUpdateAfter for that)
        SaveState();
    }
}