// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using EnergiePrijzen.Config;
using EnergiePrijzen.Data;

namespace EnergiePrijzen.UI
{
    public partial class DataSelectie : Form {

        private readonly Settings settings;
        private bool executing;

        public DataSelectie(Settings settings) {
            this.settings = settings;
            InitializeComponent();
            jeroenFolder.Text = settings.JeroenPrijzen;
            solaredgeFolder.Text = settings.SolarEdge;
            slimmeMeterFolder.Text = settings.SlimmeMeter;
            sessyFolder.Text = settings.Sessy;
            resultaatFolder.Text = settings.Resultaat;
            aantalBatterijen.Text = settings.NrOfBatteriesToSimulate.ToString(CultureInfo.InvariantCulture);
            if (settings.PeriodeStart.TryParseDate(out TimeStamp? start)) {
                periodeStart.Value = start.Value.Start;
            } else {
                periodeStart.Value = new DateTime(DateTime.Now.Year - 1, 1, 1);
            }
            if (settings.PeriodeEnd.TryParseDate(out TimeStamp? end)) {
                periodeEnd.Value = end.Value.Start;
            } else {
                periodeEnd.Value = new DateTime(DateTime.Now.Year, 1, 1);
            }
            Tracer.Tracers += TraceFromEvent;
        }

        private void TraceFromEvent(object sender, Tracer.TraceMessageEventArgs args) => TraceInternal(args.Message);

        private void TraceInternal(string? message) {
            if (InvokeRequired) {
                _ = Invoke(new Action<string>(TraceInternal), message);
                return;
            }
            message ??= "<null>";
            textLog.AppendText(message + Environment.NewLine);
            if (executing) {
                Application.DoEvents();
            }
        }

        private void OnSaveClick(object sender, EventArgs e) {
            if (UpdateSettings()) {
                settings.Save();
            }
        }

        private void OnCompute(object sender, EventArgs e) {
            if (executing) {
                DialogResult _ = MessageBox.Show("Er is al een berekening bezig", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (UpdateSettings()) {
                try {
                    executing = true;
                    var generator = new DataGenerator(settings);
                    if (!generator.GenerateData()) {
                        DialogResult _ = MessageBox.Show("Data generatie mislukt", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } finally {
                    executing = false;
                }
            }
        }

        bool UpdateSettings() {
            if (ValidateValues()) {
                string jeroenValue = jeroenFolder.Text;
                string solaredgeValue = solaredgeFolder.Text;
                string slimmeMeterValue = slimmeMeterFolder.Text;
                string sessyFolderValue = sessyFolder.Text;
                string resultaatFolderValue = resultaatFolder.Text;
                string aantalBatterijenValue = aantalBatterijen.Text;
                DateTime start = periodeStart.Value;
                DateTime end = periodeEnd.Value;

                settings.JeroenPrijzen = jeroenValue;
                settings.SolarEdge = solaredgeValue;
                settings.SlimmeMeter = slimmeMeterValue;
                settings.Sessy = sessyFolderValue;
                settings.Resultaat = resultaatFolderValue;
                settings.PeriodeStart = start.ToString(TimeStampExtensions.DateFormat);
                settings.PeriodeEnd = end.ToString(TimeStampExtensions.DateFormat);
                settings.NrOfBatteriesToSimulate = int.Parse(aantalBatterijenValue, CultureInfo.InvariantCulture);
                return true;
            }
            return false;
        }

        private bool ValidateValues() {
            string jeroenValue = jeroenFolder.Text;
            string solaredgeValue = solaredgeFolder.Text;
            string slimmeMeterValue = slimmeMeterFolder.Text;
            string sessyFolderValue = sessyFolder.Text;
            string resultaatFolderValue = resultaatFolder.Text;
            string aantalBatterijenValue = aantalBatterijen.Text;
            DateTime start = periodeStart.Value;
            DateTime end = periodeEnd.Value;
            return
                ValidateValue(jeroenValue, "Jeroen prijzen folder") &&
                ValidateValue(solaredgeValue, "Solar Edge download folder") &&
                ValidateValue(slimmeMeterValue, "Slimme meter download folder") &&
                ValidateValue(sessyFolderValue, "Sessy download folder") &&
                ValidateValue(resultaatFolderValue, "Resultaat folder") &&
                ValidateValue(start, end, "Periode start", "Periode eind") &&
                ValidateIntValue(aantalBatterijenValue, "Aantal batterijen om te simuleren");
        }

        private static bool ValidateIntValue(string aantalBatterijenValue, string message) {
            if (!int.TryParse(aantalBatterijenValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int aantalBatterijen)) {
                _ = MessageBox.Show($"{aantalBatterijen} is geen geldige integer voor {message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (aantalBatterijen < 0) {
                _ = MessageBox.Show($"{message} moet >= 0 zijn", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool ValidateValue(
            DateTime start,
            DateTime end, string startName,
            string endName
        ) {
            if (end <= start) {
                _ = MessageBox.Show($"{endName} moet na {startName} zijn", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool ValidateValue(string folder, string name) {
            if (string.IsNullOrEmpty(folder)) {
                _ = MessageBox.Show($"{name} is niet ingevuld", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Directory.Exists(folder)) {
                _ = MessageBox.Show($"{name} bestaat niet", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void OpenJeroen(object sender, System.EventArgs e) {
            string jeroenValue = jeroenFolder.Text;
            if (ValidateValue(jeroenValue, "Jeroen prijzen folder")) {
                _ = Process.Start("explorer.exe", jeroenValue);
            }
        }

        private void OpenSolarEdge(object sender, System.EventArgs e) {
            string solaredgeValue = solaredgeFolder.Text;
            if (ValidateValue(solaredgeValue, "Solar Edge download folder")) {
                _ = Process.Start("explorer.exe", solaredgeValue);
            }
        }

        private void OpenSlimmeMeter(object sender, System.EventArgs e) {
            string slimmeMeterValue = slimmeMeterFolder.Text;
            if (ValidateValue(slimmeMeterValue, "Slimme meter download folder")) {
                _ = Process.Start("explorer.exe", slimmeMeterValue);
            }
        }

        private void OpenSessy(object sender, System.EventArgs e) {
            string sessyFolderValue = sessyFolder.Text;
            if (ValidateValue(sessyFolderValue, "Sessy download folder")) {
                _ = Process.Start("explorer.exe", sessyFolderValue);
            }
        }

        private void OpenResultaat(object sender, System.EventArgs e) {
            string resultaatFolderValue = resultaatFolder.Text;
            if (ValidateValue(resultaatFolderValue, "Resultaat folder")) {
                _ = Process.Start("explorer.exe", resultaatFolderValue);
            }
        }

        private void ShowSettings(object sender, EventArgs e) {
            _ = Process.Start("explorer.exe", SettingsReader.SettingsDirectory.FullName);
        }
    }
}
