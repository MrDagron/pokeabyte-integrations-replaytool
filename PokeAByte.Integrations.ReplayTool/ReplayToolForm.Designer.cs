using PokeAByte.Integrations.ReplayTool.UI;

namespace PokeAByte.Integrations.ReplayTool;

public partial class ReplayToolForm
{
    private void InitializeComponent()
    {
            this.doStuffBtn = new System.Windows.Forms.Button();
            this.timeScrubber = new PokeAByte.Integrations.ReplayTool.UI.TimeScrubber();
            
            this.SuspendLayout();
            // 
            // doStuffBtn
            // 
            this.doStuffBtn.Location = new System.Drawing.Point(44, 43);
            this.doStuffBtn.Name = "doStuffBtn";
            this.doStuffBtn.Size = new System.Drawing.Size(75, 23);
            this.doStuffBtn.TabIndex = 0;
            this.doStuffBtn.Text = "Do Stuff";
            this.doStuffBtn.UseVisualStyleBackColor = true;
            this.doStuffBtn.Click += new System.EventHandler(this.doStuffBtn_Click);
            //
            // TimeScrubber
            // 
            this.timeScrubber.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Bottom | 
                  System.Windows.Forms.AnchorStyles.Left) | 
                  System.Windows.Forms.AnchorStyles.Right)));
            this.timeScrubber.BackColor = System.Drawing.Color.Gray;
            this.timeScrubber.Location = new System.Drawing.Point(0, 301);
            this.timeScrubber.Name = "timeScrubber";
            this.timeScrubber.Size = new System.Drawing.Size(376, 35);
            this.timeScrubber.TabIndex = 8;
            this.timeScrubber.Text = "timeScrubber";
            this.timeScrubber.PositionChanged += OnScrubberPositionChanged;
            // 
            // ReplayToolForm
            // 
            this.ClientSize = new System.Drawing.Size(376, 336);
            this.Controls.Add(this.doStuffBtn);
            this.Controls.Add(this.timeScrubber);
            this.Name = "ReplayToolForm";
            this.ResumeLayout(false);

    }

    private System.Windows.Forms.Button doStuffBtn;
    private TimeScrubber timeScrubber;
}