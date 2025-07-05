using System.Drawing;
using System.Windows.Forms;
using PokeAByte.Integrations.ReplayTool.UI;

namespace PokeAByte.Integrations.ReplayTool;

public partial class ReplayToolForm
{
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplayToolForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadReplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveReplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeReplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playbacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordingSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collectStatesFromInputFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeRecordingFromCurrentStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeRecordingFromEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playFromStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyboardShortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainFormTabs = new System.Windows.Forms.TabControl();
            this.recordingTab = new System.Windows.Forms.TabPage();
            this.numberOfStatesLabel = new System.Windows.Forms.Label();
            this.recordingTimeLabel = new System.Windows.Forms.Label();
            this.recordingPauseEmulatorBtn = new System.Windows.Forms.Button();
            this.recordBtn = new System.Windows.Forms.Button();
            this.playbackTab = new System.Windows.Forms.TabPage();
            this.playbackScrubber = new PokeAByte.Integrations.ReplayTool.UI.TimeScrubber();
            this.playbackBtn = new System.Windows.Forms.Button();
            this.lastStatePlaybackBtn = new System.Windows.Forms.Button();
            this.firstStatePlaybackBtn = new System.Windows.Forms.Button();
            this.stopPlaybackBtn = new System.Windows.Forms.Button();
            this.saveStateTabs = new System.Windows.Forms.TabControl();
            this.saveStatesPage = new System.Windows.Forms.TabPage();
            this.flaggedSavesPage = new System.Windows.Forms.TabPage();
            this.keyframeSavesPage = new System.Windows.Forms.TabPage();
            this.EDPSLabel = new System.Windows.Forms.Label();
            this.tcpServerLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.mainFormTabs.SuspendLayout();
            this.recordingTab.SuspendLayout();
            this.playbackTab.SuspendLayout();
            this.saveStateTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem,
            this.playbacToolStripMenuItem,
            this.playbackToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1282, 24);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadReplayToolStripMenuItem,
            this.saveReplayToolStripMenuItem,
            this.closeReplayToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fIleToolStripMenuItem.Text = "FIle";
            // 
            // loadReplayToolStripMenuItem
            // 
            this.loadReplayToolStripMenuItem.Name = "loadReplayToolStripMenuItem";
            this.loadReplayToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.loadReplayToolStripMenuItem.Text = "Open Replay";
            // 
            // saveReplayToolStripMenuItem
            // 
            this.saveReplayToolStripMenuItem.Name = "saveReplayToolStripMenuItem";
            this.saveReplayToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.saveReplayToolStripMenuItem.Text = "Save Replay";
            // 
            // closeReplayToolStripMenuItem
            // 
            this.closeReplayToolStripMenuItem.Name = "closeReplayToolStripMenuItem";
            this.closeReplayToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.closeReplayToolStripMenuItem.Text = "Close Replay";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // playbacToolStripMenuItem
            // 
            this.playbacToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordingSettingsToolStripMenuItem,
            this.collectStatesFromInputFileToolStripMenuItem,
            this.resumeRecordingFromCurrentStateToolStripMenuItem,
            this.resumeRecordingFromEndToolStripMenuItem});
            this.playbacToolStripMenuItem.Name = "playbacToolStripMenuItem";
            this.playbacToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.playbacToolStripMenuItem.Text = "Recording";
            // 
            // recordingSettingsToolStripMenuItem
            // 
            this.recordingSettingsToolStripMenuItem.Name = "recordingSettingsToolStripMenuItem";
            this.recordingSettingsToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.recordingSettingsToolStripMenuItem.Text = "Recording Settings";
            // 
            // collectStatesFromInputFileToolStripMenuItem
            // 
            this.collectStatesFromInputFileToolStripMenuItem.Name = "collectStatesFromInputFileToolStripMenuItem";
            this.collectStatesFromInputFileToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.collectStatesFromInputFileToolStripMenuItem.Text = "Collect States from Input File";
            // 
            // resumeRecordingFromCurrentStateToolStripMenuItem
            // 
            this.resumeRecordingFromCurrentStateToolStripMenuItem.Name = "resumeRecordingFromCurrentStateToolStripMenuItem";
            this.resumeRecordingFromCurrentStateToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.resumeRecordingFromCurrentStateToolStripMenuItem.Text = "Resume Recording from Selected State";
            // 
            // resumeRecordingFromEndToolStripMenuItem
            // 
            this.resumeRecordingFromEndToolStripMenuItem.Name = "resumeRecordingFromEndToolStripMenuItem";
            this.resumeRecordingFromEndToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.resumeRecordingFromEndToolStripMenuItem.Text = "Resume Recording from End";
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playFromStartToolStripMenuItem});
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.playbackToolStripMenuItem.Text = "Playback";
            // 
            // playFromStartToolStripMenuItem
            // 
            this.playFromStartToolStripMenuItem.Name = "playFromStartToolStripMenuItem";
            this.playFromStartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.playFromStartToolStripMenuItem.Text = "Play from Start";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyboardShortcutsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // keyboardShortcutsToolStripMenuItem
            // 
            this.keyboardShortcutsToolStripMenuItem.Name = "keyboardShortcutsToolStripMenuItem";
            this.keyboardShortcutsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.keyboardShortcutsToolStripMenuItem.Text = "Keyboard Shortcuts";
            // 
            // mainFormTabs
            // 
            this.mainFormTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainFormTabs.Controls.Add(this.recordingTab);
            this.mainFormTabs.Controls.Add(this.playbackTab);
            this.mainFormTabs.Location = new System.Drawing.Point(0, 40);
            this.mainFormTabs.Name = "mainFormTabs";
            this.mainFormTabs.SelectedIndex = 0;
            this.mainFormTabs.Size = new System.Drawing.Size(1282, 488);
            this.mainFormTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.mainFormTabs.TabIndex = 23;
            // 
            // recordingTab
            // 
            this.recordingTab.BackColor = System.Drawing.Color.Transparent;
            this.recordingTab.Controls.Add(this.numberOfStatesLabel);
            this.recordingTab.Controls.Add(this.recordingTimeLabel);
            this.recordingTab.Controls.Add(this.recordingPauseEmulatorBtn);
            this.recordingTab.Controls.Add(this.recordBtn);
            this.recordingTab.Location = new System.Drawing.Point(4, 22);
            this.recordingTab.Name = "recordingTab";
            this.recordingTab.Padding = new System.Windows.Forms.Padding(3);
            this.recordingTab.Size = new System.Drawing.Size(1274, 462);
            this.recordingTab.TabIndex = 0;
            this.recordingTab.Text = "Recording";
            // 
            // numberOfStatesLabel
            // 
            this.numberOfStatesLabel.AutoSize = true;
            this.numberOfStatesLabel.Location = new System.Drawing.Point(460, 5);
            this.numberOfStatesLabel.Name = "numberOfStatesLabel";
            this.numberOfStatesLabel.Size = new System.Drawing.Size(92, 13);
            this.numberOfStatesLabel.TabIndex = 3;
            this.numberOfStatesLabel.Text = "Number of States:";
            // 
            // recordingTimeLabel
            // 
            this.recordingTimeLabel.AutoSize = true;
            this.recordingTimeLabel.Location = new System.Drawing.Point(265, 5);
            this.recordingTimeLabel.Name = "recordingTimeLabel";
            this.recordingTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.recordingTimeLabel.TabIndex = 2;
            this.recordingTimeLabel.Text = "Recording Time: ";
            // 
            // recordingPauseEmulatorBtn
            // 
            this.recordingPauseEmulatorBtn.Location = new System.Drawing.Point(132, 0);
            this.recordingPauseEmulatorBtn.Name = "recordingPauseEmulatorBtn";
            this.recordingPauseEmulatorBtn.Size = new System.Drawing.Size(127, 23);
            this.recordingPauseEmulatorBtn.TabIndex = 1;
            this.recordingPauseEmulatorBtn.Text = "Pause Emulator";
            this.recordingPauseEmulatorBtn.UseVisualStyleBackColor = true;
            // 
            // recordBtn
            // 
            this.recordBtn.Location = new System.Drawing.Point(0, 0);
            this.recordBtn.Name = "recordBtn";
            this.recordBtn.Size = new System.Drawing.Size(126, 23);
            this.recordBtn.TabIndex = 0;
            this.recordBtn.Text = "Start Recording";
            this.recordBtn.UseVisualStyleBackColor = true;
            this.recordBtn.Click += new System.EventHandler(this.recordBtn_Click);
            // 
            // playbackTab
            // 
            this.playbackTab.BackColor = System.Drawing.Color.Transparent;
            this.playbackTab.Controls.Add(this.playbackScrubber);
            this.playbackTab.Controls.Add(this.playbackBtn);
            this.playbackTab.Controls.Add(this.lastStatePlaybackBtn);
            this.playbackTab.Controls.Add(this.firstStatePlaybackBtn);
            this.playbackTab.Controls.Add(this.stopPlaybackBtn);
            this.playbackTab.Controls.Add(this.saveStateTabs);
            this.playbackTab.Location = new System.Drawing.Point(4, 22);
            this.playbackTab.Name = "playbackTab";
            this.playbackTab.Padding = new System.Windows.Forms.Padding(3);
            this.playbackTab.Size = new System.Drawing.Size(1274, 462);
            this.playbackTab.TabIndex = 1;
            this.playbackTab.Text = "Playback";
            // 
            // playbackScrubber
            // 
            this.playbackScrubber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playbackScrubber.BackColor = System.Drawing.SystemColors.ControlDark;
            this.playbackScrubber.ControlMousePosition = ((System.ValueTuple<int, int>)(resources.GetObject("playbackScrubber.ControlMousePosition")));
            this.playbackScrubber.Location = new System.Drawing.Point(428, 427);
            this.playbackScrubber.Name = "playbackScrubber";
            this.playbackScrubber.Size = new System.Drawing.Size(838, 27);
            this.playbackScrubber.TabIndex = 0;
            this.playbackScrubber.Text = "timeScrubber1";
            // 
            // playbackBtn
            // 
            this.playbackBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playbackBtn.Location = new System.Drawing.Point(8, 431);
            this.playbackBtn.Name = "playbackBtn";
            this.playbackBtn.Size = new System.Drawing.Size(97, 23);
            this.playbackBtn.TabIndex = 2;
            this.playbackBtn.Text = "Play";
            this.playbackBtn.UseVisualStyleBackColor = true;
            // 
            // lastStatePlaybackBtn
            // 
            this.lastStatePlaybackBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lastStatePlaybackBtn.Location = new System.Drawing.Point(317, 431);
            this.lastStatePlaybackBtn.Name = "lastStatePlaybackBtn";
            this.lastStatePlaybackBtn.Size = new System.Drawing.Size(97, 23);
            this.lastStatePlaybackBtn.TabIndex = 5;
            this.lastStatePlaybackBtn.Text = "Go to Last State";
            this.lastStatePlaybackBtn.UseVisualStyleBackColor = true;
            // 
            // firstStatePlaybackBtn
            // 
            this.firstStatePlaybackBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.firstStatePlaybackBtn.Location = new System.Drawing.Point(111, 431);
            this.firstStatePlaybackBtn.Name = "firstStatePlaybackBtn";
            this.firstStatePlaybackBtn.Size = new System.Drawing.Size(97, 23);
            this.firstStatePlaybackBtn.TabIndex = 4;
            this.firstStatePlaybackBtn.Text = "Go to First State";
            this.firstStatePlaybackBtn.UseVisualStyleBackColor = true;
            // 
            // stopPlaybackBtn
            // 
            this.stopPlaybackBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopPlaybackBtn.Location = new System.Drawing.Point(214, 431);
            this.stopPlaybackBtn.Name = "stopPlaybackBtn";
            this.stopPlaybackBtn.Size = new System.Drawing.Size(97, 23);
            this.stopPlaybackBtn.TabIndex = 3;
            this.stopPlaybackBtn.Text = "Stop";
            this.stopPlaybackBtn.UseVisualStyleBackColor = true;
            // 
            // saveStateTabs
            // 
            this.saveStateTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveStateTabs.Controls.Add(this.saveStatesPage);
            this.saveStateTabs.Controls.Add(this.flaggedSavesPage);
            this.saveStateTabs.Controls.Add(this.keyframeSavesPage);
            this.saveStateTabs.Location = new System.Drawing.Point(3, 3);
            this.saveStateTabs.Name = "saveStateTabs";
            this.saveStateTabs.SelectedIndex = 0;
            this.saveStateTabs.Size = new System.Drawing.Size(1268, 411);
            this.saveStateTabs.TabIndex = 1;
            // 
            // saveStatesPage
            // 
            this.saveStatesPage.Location = new System.Drawing.Point(4, 22);
            this.saveStatesPage.Name = "saveStatesPage";
            this.saveStatesPage.Padding = new System.Windows.Forms.Padding(3);
            this.saveStatesPage.Size = new System.Drawing.Size(1260, 385);
            this.saveStatesPage.TabIndex = 0;
            this.saveStatesPage.Text = "Save States";
            this.saveStatesPage.UseVisualStyleBackColor = true;
            // 
            // flaggedSavesPage
            // 
            this.flaggedSavesPage.Location = new System.Drawing.Point(4, 22);
            this.flaggedSavesPage.Name = "flaggedSavesPage";
            this.flaggedSavesPage.Padding = new System.Windows.Forms.Padding(3);
            this.flaggedSavesPage.Size = new System.Drawing.Size(1260, 385);
            this.flaggedSavesPage.TabIndex = 1;
            this.flaggedSavesPage.Text = "Flagged Saves";
            this.flaggedSavesPage.UseVisualStyleBackColor = true;
            // 
            // keyframeSavesPage
            // 
            this.keyframeSavesPage.Location = new System.Drawing.Point(4, 22);
            this.keyframeSavesPage.Name = "keyframeSavesPage";
            this.keyframeSavesPage.Padding = new System.Windows.Forms.Padding(3);
            this.keyframeSavesPage.Size = new System.Drawing.Size(1260, 385);
            this.keyframeSavesPage.TabIndex = 1;
            this.keyframeSavesPage.Text = "Keyframes";
            this.keyframeSavesPage.UseVisualStyleBackColor = true;
            // 
            // EDPSLabel
            // 
            this.EDPSLabel.AutoSize = true;
            this.EDPSLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EDPSLabel.Location = new System.Drawing.Point(0, 24);
            this.EDPSLabel.Name = "EDPSLabel";
            this.EDPSLabel.Size = new System.Drawing.Size(54, 13);
            this.EDPSLabel.TabIndex = 24;
            this.EDPSLabel.Text = "Loading...";
            // 
            // tcpServerLabel
            // 
            this.tcpServerLabel.AutoSize = true;
            this.tcpServerLabel.Location = new System.Drawing.Point(464, 24);
            this.tcpServerLabel.Name = "tcpServerLabel";
            this.tcpServerLabel.Size = new System.Drawing.Size(104, 13);
            this.tcpServerLabel.TabIndex = 25;
            this.tcpServerLabel.Text = "Waiting for overlay...";
            // 
            // ReplayToolForm
            // 
            this.ClientSize = new System.Drawing.Size(1282, 528);
            this.Controls.Add(this.tcpServerLabel);
            this.Controls.Add(this.EDPSLabel);
            this.Controls.Add(this.mainFormTabs);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReplayToolForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainFormTabs.ResumeLayout(false);
            this.recordingTab.ResumeLayout(false);
            this.recordingTab.PerformLayout();
            this.playbackTab.ResumeLayout(false);
            this.saveStateTabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fIleToolStripMenuItem;
    private ToolStripMenuItem playbacToolStripMenuItem;
    private ToolStripMenuItem loadReplayToolStripMenuItem;
    private ToolStripMenuItem saveReplayToolStripMenuItem;
    private ToolStripMenuItem closeReplayToolStripMenuItem;
    private ToolStripMenuItem exitToolStripMenuItem;
    private ToolStripMenuItem playbackToolStripMenuItem;
    private ToolStripMenuItem recordingSettingsToolStripMenuItem;
    private ToolStripMenuItem settingsToolStripMenuItem;
    private ToolStripMenuItem keyboardShortcutsToolStripMenuItem;
    private ToolStripMenuItem collectStatesFromInputFileToolStripMenuItem;
    private ToolStripMenuItem resumeRecordingFromCurrentStateToolStripMenuItem;
    private ToolStripMenuItem resumeRecordingFromEndToolStripMenuItem;
    private ToolStripMenuItem playFromStartToolStripMenuItem;
    private TabControl mainFormTabs;
    private TabPage recordingTab;
    private TabPage playbackTab;
    private Label EDPSLabel;
    private Button recordBtn;
    private Label numberOfStatesLabel;
    private Label recordingTimeLabel;
    private Button recordingPauseEmulatorBtn;
    private TabControl saveStateTabs;
    private TabPage saveStatesPage;
    private TabPage flaggedSavesPage;
    private TabPage keyframeSavesPage;
    private Button playbackBtn;
    private Button firstStatePlaybackBtn;
    private Button stopPlaybackBtn;
    private Button lastStatePlaybackBtn;
    private TimeScrubber playbackScrubber;
    private Label tcpServerLabel;
}