namespace BariVsAddon.BariExtension
{
    partial class PromptStopDebuggerDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.questionPanel = new System.Windows.Forms.Panel();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.questionLabel = new System.Windows.Forms.Label();
            this.keepDebuggingButton = new System.Windows.Forms.Button();
            this.stopDebuggingButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.questionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // questionPanel
            // 
            this.questionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.questionPanel.BackColor = System.Drawing.SystemColors.Window;
            this.questionPanel.Controls.Add(this.iconBox);
            this.questionPanel.Controls.Add(this.questionLabel);
            this.questionPanel.Location = new System.Drawing.Point(0, 0);
            this.questionPanel.Name = "questionPanel";
            this.questionPanel.Size = new System.Drawing.Size(370, 51);
            this.questionPanel.TabIndex = 0;
            // 
            // iconBox
            // 
            this.iconBox.Location = new System.Drawing.Point(9, 9);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(32, 32);
            this.iconBox.TabIndex = 1;
            this.iconBox.TabStop = false;
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Location = new System.Drawing.Point(47, 9);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(161, 13);
            this.questionLabel.TabIndex = 0;
            this.questionLabel.Text = "Do you want to stop debugging?";
            // 
            // keepDebuggingButton
            // 
            this.keepDebuggingButton.Location = new System.Drawing.Point(259, 57);
            this.keepDebuggingButton.Name = "keepDebuggingButton";
            this.keepDebuggingButton.Size = new System.Drawing.Size(95, 23);
            this.keepDebuggingButton.TabIndex = 1;
            this.keepDebuggingButton.Text = "Keep debugging";
            this.keepDebuggingButton.UseVisualStyleBackColor = true;
            this.keepDebuggingButton.Click += new System.EventHandler(this.KeepDebuggingButtonClick);
            // 
            // stopDebuggingButton
            // 
            this.stopDebuggingButton.Location = new System.Drawing.Point(158, 57);
            this.stopDebuggingButton.Name = "stopDebuggingButton";
            this.stopDebuggingButton.Size = new System.Drawing.Size(95, 23);
            this.stopDebuggingButton.TabIndex = 2;
            this.stopDebuggingButton.Text = "Stop debugging";
            this.stopDebuggingButton.UseVisualStyleBackColor = true;
            this.stopDebuggingButton.Click += new System.EventHandler(this.StopDebuggingButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(57, 57);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(95, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // PromptStopDebuggerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 86);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.stopDebuggingButton);
            this.Controls.Add(this.keepDebuggingButton);
            this.Controls.Add(this.questionPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromptStopDebuggerDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bari Extension";
            this.questionPanel.ResumeLayout(false);
            this.questionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel questionPanel;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.Button keepDebuggingButton;
        private System.Windows.Forms.Button stopDebuggingButton;
        private System.Windows.Forms.Button cancelButton;

    }
}