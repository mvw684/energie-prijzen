using System.Drawing;
using System.Windows.Forms;

namespace EnergiePrijzen.UI
{
    partial class DataSelectie {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            jeroenLabel = new Label();
            solaredgeLabel = new Label();
            SuspendLayout();
            // 
            // jeroenLabel
            // 
            jeroenLabel.AutoSize = true;
            jeroenLabel.Location = new Point(41, 38);
            jeroenLabel.Name = "jeroenLabel";
            jeroenLabel.Size = new Size(41, 15);
            jeroenLabel.TabIndex = 0;
            jeroenLabel.Text = "Jeroen";
            // 
            // solaredgeLabel
            // 
            solaredgeLabel.AutoSize = true;
            solaredgeLabel.Location = new Point(42, 77);
            solaredgeLabel.Name = "solaredgeLabel";
            solaredgeLabel.Size = new Size(62, 15);
            solaredgeLabel.TabIndex = 1;
            solaredgeLabel.Text = "Solar Edge";
            // 
            // DataSelectie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(solaredgeLabel);
            Controls.Add(jeroenLabel);
            Name = "DataSelectie";
            Text = "Energie Prijzen";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label jeroenLabel;
        private Label solaredgeLabel;
    }
}
