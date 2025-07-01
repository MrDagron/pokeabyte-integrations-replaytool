using System;
using System.Windows.Forms;

namespace PokeAByte.Integrations.ReplayTool.UI
{
    public class EditNameDialog : Form
    {
        
        private Button cancelButton;
        private TextBox newNameTextBox;
        private Button okButton;

        public string NewName = "";

        public EditNameDialog(string flagName)
        {
            NewName = flagName;
            KeyPreview = true;
            KeyDown += EditNameDialog_KeyDown;
            InitializeComponent();
        }

        private void EditNameDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult = DialogResult.OK;
                NewName = newNameTextBox.Text;
                Close();
            }
        }

        private void InitializeComponent()
        {
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.newNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(421, 9);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(340, 9);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Submit";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // newNameTextBox
            // 
            this.newNameTextBox.Location = new System.Drawing.Point(12, 12);
            this.newNameTextBox.Name = "newNameTextBox";
            this.newNameTextBox.Size = new System.Drawing.Size(322, 20);
            this.newNameTextBox.TabIndex = 2;
            // 
            // EditName
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(503, 44);
            this.Controls.Add(this.newNameTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "EditName";
            this.Text = "Rename";
            this.ResumeLayout(false);
            this.PerformLayout();
            if (!string.IsNullOrEmpty(NewName))
            {
                newNameTextBox.Text = NewName;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            NewName = newNameTextBox.Text;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
