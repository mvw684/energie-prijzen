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
            labelSlimmeMeter = new Label();
            jeroenFolder = new TextBox();
            solaredgeFolder = new TextBox();
            slimmeMeterFolder = new TextBox();
            save = new Button();
            compute = new Button();
            SuspendLayout();
            // 
            // jeroenLabel
            // 
            jeroenLabel.AutoSize = true;
            jeroenLabel.Location = new Point(2, 29);
            jeroenLabel.Name = "jeroenLabel";
            jeroenLabel.Size = new Size(41, 15);
            jeroenLabel.TabIndex = 0;
            jeroenLabel.Text = "Jeroen";
            // 
            // solaredgeLabel
            // 
            solaredgeLabel.AutoSize = true;
            solaredgeLabel.Location = new Point(2, 55);
            solaredgeLabel.Name = "solaredgeLabel";
            solaredgeLabel.Size = new Size(62, 15);
            solaredgeLabel.TabIndex = 1;
            solaredgeLabel.Text = "Solar Edge";
            // 
            // labelSlimmeMeter
            // 
            labelSlimmeMeter.AutoSize = true;
            labelSlimmeMeter.Location = new Point(2, 80);
            labelSlimmeMeter.Name = "labelSlimmeMeter";
            labelSlimmeMeter.Size = new Size(78, 15);
            labelSlimmeMeter.TabIndex = 2;
            labelSlimmeMeter.Text = "SlimmeMeter";
            // 
            // jeroenFolder
            // 
            jeroenFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            jeroenFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            jeroenFolder.AutoCompleteSource = AutoCompleteSource.FileSystem;
            jeroenFolder.Location = new Point(81, 26);
            jeroenFolder.Name = "jeroenFolder";
            jeroenFolder.Size = new Size(707, 23);
            jeroenFolder.TabIndex = 3;
            // 
            // solaredgeFolder
            // 
            solaredgeFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            solaredgeFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            solaredgeFolder.AutoCompleteSource = AutoCompleteSource.FileSystem;
            solaredgeFolder.Location = new Point(81, 52);
            solaredgeFolder.Name = "solaredgeFolder";
            solaredgeFolder.Size = new Size(707, 23);
            solaredgeFolder.TabIndex = 4;
            // 
            // slimmeMeterFolder
            // 
            slimmeMeterFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            slimmeMeterFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            slimmeMeterFolder.AutoCompleteSource = AutoCompleteSource.FileSystem;
            slimmeMeterFolder.Location = new Point(81, 77);
            slimmeMeterFolder.Name = "slimmeMeterFolder";
            slimmeMeterFolder.Size = new Size(707, 23);
            slimmeMeterFolder.TabIndex = 5;
            // 
            // save
            // 
            save.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            save.Location = new Point(632, 415);
            save.Name = "save";
            save.Size = new Size(75, 23);
            save.TabIndex = 6;
            save.Text = "&Save";
            save.TextImageRelation = TextImageRelation.ImageAboveText;
            save.UseVisualStyleBackColor = true;
            save.Click += OnSaveClick;
            // 
            // compute
            // 
            compute.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            compute.Location = new Point(713, 415);
            compute.Name = "compute";
            compute.Size = new Size(75, 23);
            compute.TabIndex = 7;
            compute.Text = "compute";
            compute.UseVisualStyleBackColor = true;
            // 
            // DataSelectie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(compute);
            Controls.Add(save);
            Controls.Add(slimmeMeterFolder);
            Controls.Add(solaredgeFolder);
            Controls.Add(jeroenFolder);
            Controls.Add(labelSlimmeMeter);
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
        private Label labelSlimmeMeter;
        private TextBox jeroenFolder;
        private TextBox solaredgeFolder;
        private TextBox slimmeMeterFolder;
        private Button save;
        private Button compute;
    }
}
