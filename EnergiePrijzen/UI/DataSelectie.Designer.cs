// Copyright (c) 2025 mvw684

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
            Tracer.Tracers -= TraceFromEvent;
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
            labelSessy = new Label();
            sessyFolder = new TextBox();
            labelResultaat = new Label();
            resultaatFolder = new TextBox();
            labelPeriode = new Label();
            periodeStart = new DateTimePicker();
            periodeEnd = new DateTimePicker();
            textLog = new TextBox();
            SuspendLayout();
            // 
            // jeroenLabel
            // 
            jeroenLabel.AutoSize = true;
            jeroenLabel.Location = new Point(2, 40);
            jeroenLabel.Name = "jeroenLabel";
            jeroenLabel.Size = new Size(41, 15);
            jeroenLabel.TabIndex = 0;
            jeroenLabel.Text = "Jeroen";
            jeroenLabel.DoubleClick += OpenJeroen;
            // 
            // solaredgeLabel
            // 
            solaredgeLabel.AutoSize = true;
            solaredgeLabel.Location = new Point(2, 66);
            solaredgeLabel.Name = "solaredgeLabel";
            solaredgeLabel.Size = new Size(62, 15);
            solaredgeLabel.TabIndex = 1;
            solaredgeLabel.Text = "Solar Edge";
            solaredgeLabel.DoubleClick += OpenSolarEdge;
            // 
            // labelSlimmeMeter
            // 
            labelSlimmeMeter.AutoSize = true;
            labelSlimmeMeter.Location = new Point(2, 91);
            labelSlimmeMeter.Name = "labelSlimmeMeter";
            labelSlimmeMeter.Size = new Size(78, 15);
            labelSlimmeMeter.TabIndex = 2;
            labelSlimmeMeter.Text = "SlimmeMeter";
            labelSlimmeMeter.DoubleClick += OpenSlimmeMeter;
            // 
            // jeroenFolder
            // 
            jeroenFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            jeroenFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            jeroenFolder.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            jeroenFolder.Location = new Point(81, 37);
            jeroenFolder.Name = "jeroenFolder";
            jeroenFolder.Size = new Size(707, 23);
            jeroenFolder.TabIndex = 3;
            // 
            // solaredgeFolder
            // 
            solaredgeFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            solaredgeFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            solaredgeFolder.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            solaredgeFolder.Location = new Point(81, 62);
            solaredgeFolder.Name = "solaredgeFolder";
            solaredgeFolder.Size = new Size(707, 23);
            solaredgeFolder.TabIndex = 4;
            // 
            // slimmeMeterFolder
            // 
            slimmeMeterFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            slimmeMeterFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            slimmeMeterFolder.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            slimmeMeterFolder.Location = new Point(81, 87);
            slimmeMeterFolder.Name = "slimmeMeterFolder";
            slimmeMeterFolder.Size = new Size(707, 23);
            slimmeMeterFolder.TabIndex = 5;
            // 
            // save
            // 
            save.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            save.Location = new Point(632, 164);
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
            compute.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            compute.Location = new Point(713, 164);
            compute.Name = "compute";
            compute.Size = new Size(75, 23);
            compute.TabIndex = 7;
            compute.Text = "compute";
            compute.UseVisualStyleBackColor = true;
            compute.Click += OnCompute;
            // 
            // labelSessy
            // 
            labelSessy.AutoSize = true;
            labelSessy.Location = new Point(2, 114);
            labelSessy.Name = "labelSessy";
            labelSessy.Size = new Size(35, 15);
            labelSessy.TabIndex = 8;
            labelSessy.Text = "Sessy";
            labelSessy.DoubleClick += OpenSessy;
            // 
            // sessyFolder
            // 
            sessyFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            sessyFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            sessyFolder.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            sessyFolder.Location = new Point(81, 111);
            sessyFolder.Name = "sessyFolder";
            sessyFolder.Size = new Size(707, 23);
            sessyFolder.TabIndex = 9;
            // 
            // labelResultaat
            // 
            labelResultaat.AutoSize = true;
            labelResultaat.Location = new Point(2, 138);
            labelResultaat.Name = "labelResultaat";
            labelResultaat.Size = new Size(55, 15);
            labelResultaat.TabIndex = 10;
            labelResultaat.Text = "Resultaat";
            labelResultaat.DoubleClick += OpenResultaat;
            // 
            // resultaatFolder
            // 
            resultaatFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            resultaatFolder.Location = new Point(81, 135);
            resultaatFolder.Name = "resultaatFolder";
            resultaatFolder.Size = new Size(707, 23);
            resultaatFolder.TabIndex = 11;
            // 
            // labelPeriode
            // 
            labelPeriode.AutoSize = true;
            labelPeriode.Location = new Point(2, 19);
            labelPeriode.Name = "labelPeriode";
            labelPeriode.Size = new Size(47, 15);
            labelPeriode.TabIndex = 12;
            labelPeriode.Text = "Periode";
            // 
            // periodeStart
            // 
            periodeStart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            periodeStart.CustomFormat = "yyyy-MM-dd";
            periodeStart.Format = DateTimePickerFormat.Custom;
            periodeStart.Location = new Point(82, 13);
            periodeStart.Name = "periodeStart";
            periodeStart.Size = new Size(146, 23);
            periodeStart.TabIndex = 13;
            // 
            // periodeEnd
            // 
            periodeEnd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            periodeEnd.CustomFormat = "yyyy-MM-dd";
            periodeEnd.Format = DateTimePickerFormat.Custom;
            periodeEnd.Location = new Point(234, 13);
            periodeEnd.Name = "periodeEnd";
            periodeEnd.Size = new Size(146, 23);
            periodeEnd.TabIndex = 14;
            // 
            // textLog
            // 
            textLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textLog.HideSelection = false;
            textLog.Location = new Point(2, 193);
            textLog.Multiline = true;
            textLog.Name = "textLog";
            textLog.ReadOnly = true;
            textLog.Size = new Size(786, 255);
            textLog.TabIndex = 15;
            // 
            // DataSelectie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textLog);
            Controls.Add(periodeEnd);
            Controls.Add(periodeStart);
            Controls.Add(labelPeriode);
            Controls.Add(resultaatFolder);
            Controls.Add(labelResultaat);
            Controls.Add(sessyFolder);
            Controls.Add(labelSessy);
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
        private Label labelSessy;
        private TextBox sessyFolder;
        private Label labelResultaat;
        private TextBox resultaatFolder;
        private Label labelPeriode;
        private DateTimePicker periodeStart;
        private DateTimePicker periodeEnd;
        private TextBox textLog;
    }
}
