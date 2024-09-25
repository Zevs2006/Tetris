namespace Tetris
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label linesClearedLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.scoreLabel = new System.Windows.Forms.Label();
            this.linesClearedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(400, 30);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(48, 13);
            this.scoreLabel.TabIndex = 0;
            this.scoreLabel.Text = "Счет: 0";
            // 
            // linesClearedLabel
            // 
            this.linesClearedLabel.AutoSize = true;
            this.linesClearedLabel.Location = new System.Drawing.Point(400, 50);
            this.linesClearedLabel.Name = "linesClearedLabel";
            this.linesClearedLabel.Size = new System.Drawing.Size(120, 13);
            this.linesClearedLabel.TabIndex = 1;
            this.linesClearedLabel.Text = "Очистка линий: 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.linesClearedLabel);
            this.Controls.Add(this.scoreLabel);
            this.Name = "Form1";
            this.Text = "Tetris";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
