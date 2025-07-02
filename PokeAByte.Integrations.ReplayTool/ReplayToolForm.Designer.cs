using System.Drawing;
using System.Windows.Forms;
using PokeAByte.Integrations.ReplayTool.UI;

namespace PokeAByte.Integrations.ReplayTool;

public partial class ReplayToolForm
{
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplayToolForm));
            this.doStuffBtn = new System.Windows.Forms.Button();
            this.EDPSLabel = new System.Windows.Forms.Label();
            this.savesTabControl = new System.Windows.Forms.TabControl();
            this.savesTabAllSaves = new System.Windows.Forms.TabPage();
            this.allSavesRenameSave = new System.Windows.Forms.Button();
            this.allSavesListView = new System.Windows.Forms.ListView();
            this.allSavesDeleteSaveBtn = new System.Windows.Forms.Button();
            this.savesTabFlaggesSaves = new System.Windows.Forms.TabPage();
            this.movieGroupBox = new System.Windows.Forms.GroupBox();
            this.bk2FileLabel = new System.Windows.Forms.Label();
            this.bk2BrowseBtn = new System.Windows.Forms.Button();
            this.bk2FileTextBox = new System.Windows.Forms.TextBox();
            this.bk2Label = new System.Windows.Forms.Label();
            this.baseStateLabel = new System.Windows.Forms.Label();
            this.currentStateLabel = new System.Windows.Forms.Label();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.saveIntervalLabel = new System.Windows.Forms.Label();
            this.saveIntervalSaveBtn = new System.Windows.Forms.Button();
            this.saveInterval = new System.Windows.Forms.NumericUpDown();
            this.currentKeyLabel = new System.Windows.Forms.Label();
            this.avgFpsLabel = new System.Windows.Forms.Label();
            this.currentFrameLabel = new System.Windows.Forms.Label();
            this.currentSystemLabel = new System.Windows.Forms.Label();
            this.currentGameLabel = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.takeControlBtn = new System.Windows.Forms.Button();
            this.forceSaveBtn = new System.Windows.Forms.Button();
            this.recordMovieBtn = new System.Windows.Forms.Button();
            this.playMovieBtn = new System.Windows.Forms.Button();
            this.playCollectBtn = new System.Windows.Forms.Button();
            this.timeScrubber = new PokeAByte.Integrations.ReplayTool.UI.TimeScrubber();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.savesTabControl.SuspendLayout();
            this.savesTabAllSaves.SuspendLayout();
            this.movieGroupBox.SuspendLayout();
            this.statusGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveInterval)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // doStuffBtn
            // 
            this.doStuffBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.doStuffBtn.Location = new System.Drawing.Point(300, 68);
            this.doStuffBtn.Name = "doStuffBtn";
            this.doStuffBtn.Size = new System.Drawing.Size(182, 23);
            this.doStuffBtn.TabIndex = 0;
            this.doStuffBtn.Text = "Do Stuff (Debug)";
            this.doStuffBtn.UseVisualStyleBackColor = true;
            this.doStuffBtn.Click += new System.EventHandler(this.doStuffBtn_Click);
            // 
            // EDPSLabel
            // 
            this.EDPSLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.EDPSLabel.Location = new System.Drawing.Point(3, 94);
            this.EDPSLabel.Name = "EDPSLabel";
            this.EDPSLabel.Size = new System.Drawing.Size(489, 18);
            this.EDPSLabel.TabIndex = 9;
            this.EDPSLabel.Text = "Loading...";
            this.EDPSLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // savesTabControl
            // 
            this.savesTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.savesTabControl.Controls.Add(this.savesTabAllSaves);
            this.savesTabControl.Controls.Add(this.savesTabFlaggesSaves);
            this.savesTabControl.Location = new System.Drawing.Point(16, 100);
            this.savesTabControl.Name = "savesTabControl";
            this.savesTabControl.SelectedIndex = 0;
            this.savesTabControl.Size = new System.Drawing.Size(495, 299);
            this.savesTabControl.TabIndex = 10;
            // 
            // savesTabAllSaves
            // 
            this.savesTabAllSaves.Controls.Add(this.allSavesRenameSave);
            this.savesTabAllSaves.Controls.Add(this.allSavesListView);
            this.savesTabAllSaves.Controls.Add(this.allSavesDeleteSaveBtn);
            this.savesTabAllSaves.Location = new System.Drawing.Point(4, 22);
            this.savesTabAllSaves.Name = "savesTabAllSaves";
            this.savesTabAllSaves.Padding = new System.Windows.Forms.Padding(3);
            this.savesTabAllSaves.Size = new System.Drawing.Size(487, 273);
            this.savesTabAllSaves.TabIndex = 0;
            this.savesTabAllSaves.Text = "All Saves";
            this.savesTabAllSaves.UseVisualStyleBackColor = true;
            // 
            // allSavesRenameSave
            // 
            this.allSavesRenameSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.allSavesRenameSave.Location = new System.Drawing.Point(406, 244);
            this.allSavesRenameSave.Name = "allSavesRenameSave";
            this.allSavesRenameSave.Size = new System.Drawing.Size(75, 23);
            this.allSavesRenameSave.TabIndex = 2;
            this.allSavesRenameSave.Text = "Rename";
            this.allSavesRenameSave.UseVisualStyleBackColor = true;
            // 
            // allSavesListView
            // 
            this.allSavesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allSavesListView.HideSelection = false;
            this.allSavesListView.Location = new System.Drawing.Point(0, 0);
            this.allSavesListView.Name = "allSavesListView";
            this.allSavesListView.Size = new System.Drawing.Size(487, 238);
            this.allSavesListView.TabIndex = 0;
            this.allSavesListView.UseCompatibleStateImageBehavior = false;
            // 
            // allSavesDeleteSaveBtn
            // 
            this.allSavesDeleteSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.allSavesDeleteSaveBtn.Location = new System.Drawing.Point(325, 244);
            this.allSavesDeleteSaveBtn.Name = "allSavesDeleteSaveBtn";
            this.allSavesDeleteSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.allSavesDeleteSaveBtn.TabIndex = 1;
            this.allSavesDeleteSaveBtn.Text = "Delete";
            this.allSavesDeleteSaveBtn.UseVisualStyleBackColor = true;
            // 
            // savesTabFlaggesSaves
            // 
            this.savesTabFlaggesSaves.Location = new System.Drawing.Point(4, 22);
            this.savesTabFlaggesSaves.Name = "savesTabFlaggesSaves";
            this.savesTabFlaggesSaves.Padding = new System.Windows.Forms.Padding(3);
            this.savesTabFlaggesSaves.Size = new System.Drawing.Size(571, 275);
            this.savesTabFlaggesSaves.TabIndex = 1;
            this.savesTabFlaggesSaves.Text = "Flagged Saves";
            this.savesTabFlaggesSaves.UseVisualStyleBackColor = true;
            // 
            // movieGroupBox
            // 
            this.movieGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.movieGroupBox.Controls.Add(this.bk2FileLabel);
            this.movieGroupBox.Controls.Add(this.bk2BrowseBtn);
            this.movieGroupBox.Controls.Add(this.bk2FileTextBox);
            this.movieGroupBox.Controls.Add(this.bk2Label);
            this.movieGroupBox.Location = new System.Drawing.Point(12, 12);
            this.movieGroupBox.Name = "movieGroupBox";
            this.movieGroupBox.Size = new System.Drawing.Size(499, 82);
            this.movieGroupBox.TabIndex = 11;
            this.movieGroupBox.TabStop = false;
            this.movieGroupBox.Text = "Movie";
            // 
            // bk2FileLabel
            // 
            this.bk2FileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bk2FileLabel.AutoSize = true;
            this.bk2FileLabel.Location = new System.Drawing.Point(9, 59);
            this.bk2FileLabel.Name = "bk2FileLabel";
            this.bk2FileLabel.Size = new System.Drawing.Size(82, 13);
            this.bk2FileLabel.TabIndex = 101;
            this.bk2FileLabel.Text = "No Bk2 Loaded";
            // 
            // bk2BrowseBtn
            // 
            this.bk2BrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bk2BrowseBtn.Location = new System.Drawing.Point(407, 32);
            this.bk2BrowseBtn.Name = "bk2BrowseBtn";
            this.bk2BrowseBtn.Size = new System.Drawing.Size(77, 23);
            this.bk2BrowseBtn.TabIndex = 2;
            this.bk2BrowseBtn.Text = "Browse";
            this.bk2BrowseBtn.UseVisualStyleBackColor = true;
            // 
            // bk2FileTextBox
            // 
            this.bk2FileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bk2FileTextBox.Location = new System.Drawing.Point(9, 32);
            this.bk2FileTextBox.Name = "bk2FileTextBox";
            this.bk2FileTextBox.ReadOnly = true;
            this.bk2FileTextBox.Size = new System.Drawing.Size(392, 20);
            this.bk2FileTextBox.TabIndex = 100;
            this.bk2FileTextBox.TabStop = false;
            // 
            // bk2Label
            // 
            this.bk2Label.AutoSize = true;
            this.bk2Label.Location = new System.Drawing.Point(6, 16);
            this.bk2Label.Name = "bk2Label";
            this.bk2Label.Size = new System.Drawing.Size(46, 13);
            this.bk2Label.TabIndex = 0;
            this.bk2Label.Text = "BK2 File";
            // 
            // baseStateLabel
            // 
            this.baseStateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseStateLabel.AutoSize = true;
            this.baseStateLabel.Location = new System.Drawing.Point(9, 626);
            this.baseStateLabel.Name = "baseStateLabel";
            this.baseStateLabel.Size = new System.Drawing.Size(72, 13);
            this.baseStateLabel.TabIndex = 12;
            this.baseStateLabel.Text = "Current State:";
            // 
            // currentStateLabel
            // 
            this.currentStateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentStateLabel.AutoSize = true;
            this.currentStateLabel.Location = new System.Drawing.Point(78, 626);
            this.currentStateLabel.Name = "currentStateLabel";
            this.currentStateLabel.Size = new System.Drawing.Size(27, 13);
            this.currentStateLabel.TabIndex = 13;
            this.currentStateLabel.Text = "N/A";
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusGroupBox.Controls.Add(this.saveIntervalLabel);
            this.statusGroupBox.Controls.Add(this.saveIntervalSaveBtn);
            this.statusGroupBox.Controls.Add(this.saveInterval);
            this.statusGroupBox.Controls.Add(this.currentKeyLabel);
            this.statusGroupBox.Controls.Add(this.avgFpsLabel);
            this.statusGroupBox.Controls.Add(this.currentFrameLabel);
            this.statusGroupBox.Controls.Add(this.doStuffBtn);
            this.statusGroupBox.Controls.Add(this.currentSystemLabel);
            this.statusGroupBox.Controls.Add(this.currentGameLabel);
            this.statusGroupBox.Controls.Add(this.fpsLabel);
            this.statusGroupBox.Controls.Add(this.EDPSLabel);
            this.statusGroupBox.Location = new System.Drawing.Point(16, 405);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(495, 115);
            this.statusGroupBox.TabIndex = 14;
            this.statusGroupBox.TabStop = false;
            // 
            // saveIntervalLabel
            // 
            this.saveIntervalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveIntervalLabel.AutoSize = true;
            this.saveIntervalLabel.Location = new System.Drawing.Point(297, 13);
            this.saveIntervalLabel.Name = "saveIntervalLabel";
            this.saveIntervalLabel.Size = new System.Drawing.Size(70, 13);
            this.saveIntervalLabel.TabIndex = 16;
            this.saveIntervalLabel.Text = "Save Interval";
            // 
            // saveIntervalSaveBtn
            // 
            this.saveIntervalSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveIntervalSaveBtn.Location = new System.Drawing.Point(407, 29);
            this.saveIntervalSaveBtn.Name = "saveIntervalSaveBtn";
            this.saveIntervalSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveIntervalSaveBtn.TabIndex = 15;
            this.saveIntervalSaveBtn.Text = "Save";
            this.saveIntervalSaveBtn.UseVisualStyleBackColor = true;
            // 
            // saveInterval
            // 
            this.saveInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveInterval.Location = new System.Drawing.Point(300, 29);
            this.saveInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.saveInterval.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.saveInterval.Name = "saveInterval";
            this.saveInterval.Size = new System.Drawing.Size(101, 20);
            this.saveInterval.TabIndex = 14;
            this.saveInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // currentKeyLabel
            // 
            this.currentKeyLabel.AutoSize = true;
            this.currentKeyLabel.Location = new System.Drawing.Point(6, 81);
            this.currentKeyLabel.Name = "currentKeyLabel";
            this.currentKeyLabel.Size = new System.Drawing.Size(65, 13);
            this.currentKeyLabel.TabIndex = 13;
            this.currentKeyLabel.Text = "Current Key:";
            // 
            // avgFpsLabel
            // 
            this.avgFpsLabel.AutoSize = true;
            this.avgFpsLabel.Location = new System.Drawing.Point(6, 29);
            this.avgFpsLabel.Name = "avgFpsLabel";
            this.avgFpsLabel.Size = new System.Drawing.Size(52, 13);
            this.avgFpsLabel.TabIndex = 12;
            this.avgFpsLabel.Text = "Avg. Fps:";
            // 
            // currentFrameLabel
            // 
            this.currentFrameLabel.AutoSize = true;
            this.currentFrameLabel.Location = new System.Drawing.Point(6, 68);
            this.currentFrameLabel.Name = "currentFrameLabel";
            this.currentFrameLabel.Size = new System.Drawing.Size(76, 13);
            this.currentFrameLabel.TabIndex = 11;
            this.currentFrameLabel.Text = "Current Frame:";
            // 
            // currentSystemLabel
            // 
            this.currentSystemLabel.AutoSize = true;
            this.currentSystemLabel.Location = new System.Drawing.Point(6, 55);
            this.currentSystemLabel.Name = "currentSystemLabel";
            this.currentSystemLabel.Size = new System.Drawing.Size(81, 13);
            this.currentSystemLabel.TabIndex = 10;
            this.currentSystemLabel.Text = "Current System:";
            // 
            // currentGameLabel
            // 
            this.currentGameLabel.AutoSize = true;
            this.currentGameLabel.Location = new System.Drawing.Point(6, 42);
            this.currentGameLabel.Name = "currentGameLabel";
            this.currentGameLabel.Size = new System.Drawing.Size(75, 13);
            this.currentGameLabel.TabIndex = 1;
            this.currentGameLabel.Text = "Current Game:";
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(6, 16);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(27, 13);
            this.fpsLabel.TabIndex = 0;
            this.fpsLabel.Text = "Fps:";
            // 
            // pauseBtn
            // 
            this.pauseBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pauseBtn.Location = new System.Drawing.Point(3, 3);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(162, 23);
            this.pauseBtn.TabIndex = 15;
            this.pauseBtn.Text = "Pause Emulator";
            this.pauseBtn.UseVisualStyleBackColor = true;
            // 
            // takeControlBtn
            // 
            this.takeControlBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.takeControlBtn.Location = new System.Drawing.Point(3, 32);
            this.takeControlBtn.Name = "takeControlBtn";
            this.takeControlBtn.Size = new System.Drawing.Size(162, 23);
            this.takeControlBtn.TabIndex = 16;
            this.takeControlBtn.Text = "Take Control";
            this.takeControlBtn.UseVisualStyleBackColor = true;
            // 
            // forceSaveBtn
            // 
            this.forceSaveBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.forceSaveBtn.Location = new System.Drawing.Point(339, 32);
            this.forceSaveBtn.Name = "forceSaveBtn";
            this.forceSaveBtn.Size = new System.Drawing.Size(162, 23);
            this.forceSaveBtn.TabIndex = 17;
            this.forceSaveBtn.Text = "Force Save Recording";
            this.forceSaveBtn.UseVisualStyleBackColor = true;
            // 
            // recordMovieBtn
            // 
            this.recordMovieBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.recordMovieBtn.Location = new System.Drawing.Point(171, 3);
            this.recordMovieBtn.Name = "recordMovieBtn";
            this.recordMovieBtn.Size = new System.Drawing.Size(162, 23);
            this.recordMovieBtn.TabIndex = 18;
            this.recordMovieBtn.Text = "Record Movie";
            this.recordMovieBtn.UseVisualStyleBackColor = true;
            // 
            // playMovieBtn
            // 
            this.playMovieBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.playMovieBtn.Location = new System.Drawing.Point(171, 32);
            this.playMovieBtn.Name = "playMovieBtn";
            this.playMovieBtn.Size = new System.Drawing.Size(162, 23);
            this.playMovieBtn.TabIndex = 19;
            this.playMovieBtn.Text = "Play Movie";
            this.playMovieBtn.UseVisualStyleBackColor = true;
            // 
            // playCollectBtn
            // 
            this.playCollectBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.playCollectBtn.Location = new System.Drawing.Point(339, 3);
            this.playCollectBtn.Name = "playCollectBtn";
            this.playCollectBtn.Size = new System.Drawing.Size(162, 23);
            this.playCollectBtn.TabIndex = 20;
            this.playCollectBtn.Text = "Play Movie / Collect States";
            this.playCollectBtn.UseVisualStyleBackColor = true;
            // 
            // timeScrubber
            // 
            this.timeScrubber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeScrubber.BackColor = System.Drawing.Color.Gray;
            this.timeScrubber.ControlMousePosition = ((System.ValueTuple<int, int>)(resources.GetObject("timeScrubber.ControlMousePosition")));
            this.timeScrubber.Location = new System.Drawing.Point(0, 642);
            this.timeScrubber.Name = "timeScrubber";
            this.timeScrubber.Size = new System.Drawing.Size(527, 35);
            this.timeScrubber.TabIndex = 8;
            this.timeScrubber.Text = "timeScrubber";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.pauseBtn);
            this.flowLayoutPanel1.Controls.Add(this.recordMovieBtn);
            this.flowLayoutPanel1.Controls.Add(this.playCollectBtn);
            this.flowLayoutPanel1.Controls.Add(this.takeControlBtn);
            this.flowLayoutPanel1.Controls.Add(this.playMovieBtn);
            this.flowLayoutPanel1.Controls.Add(this.forceSaveBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 562);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(507, 61);
            this.flowLayoutPanel1.TabIndex = 21;
            // 
            // ReplayToolForm
            // 
            this.ClientSize = new System.Drawing.Size(527, 677);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.currentStateLabel);
            this.Controls.Add(this.baseStateLabel);
            this.Controls.Add(this.movieGroupBox);
            this.Controls.Add(this.savesTabControl);
            this.Controls.Add(this.timeScrubber);
            this.Name = "ReplayToolForm";
            this.savesTabControl.ResumeLayout(false);
            this.savesTabAllSaves.ResumeLayout(false);
            this.movieGroupBox.ResumeLayout(false);
            this.movieGroupBox.PerformLayout();
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveInterval)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private System.Windows.Forms.Button doStuffBtn;
    private TimeScrubber timeScrubber;
    private Label EDPSLabel;
    private TabControl savesTabControl;
    private TabPage savesTabAllSaves;
    private TabPage savesTabFlaggesSaves;
    private ListView allSavesListView;
    private Button allSavesRenameSave;
    private Button allSavesDeleteSaveBtn;
    private GroupBox movieGroupBox;
    private Button bk2BrowseBtn;
    private TextBox bk2FileTextBox;
    private Label bk2Label;
    private Label bk2FileLabel;
    private Label baseStateLabel;
    private Label currentStateLabel;
    private GroupBox statusGroupBox;
    private Label avgFpsLabel;
    private Label currentFrameLabel;
    private Label currentSystemLabel;
    private Label currentGameLabel;
    private Label fpsLabel;
    private Label currentKeyLabel;
    private Button saveIntervalSaveBtn;
    private NumericUpDown saveInterval;
    private Label saveIntervalLabel;
    private Button pauseBtn;
    private Button takeControlBtn;
    private Button forceSaveBtn;
    private Button recordMovieBtn;
    private Button playMovieBtn;
    private Button playCollectBtn;
    private FlowLayoutPanel flowLayoutPanel1;
}