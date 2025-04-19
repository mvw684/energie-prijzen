using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using EnergiePrijzen.Config;
using EnergiePrijzen.Data;

using Windows.Media.Protection.PlayReady;

namespace EnergiePrijzen.UI
{
    public partial class DataSelectie : Form {

        private readonly Settings settings;

        private const string dateFormat = "yyyy-MM-dd";
        private readonly string[] dateFormats = [dateFormat];

        public DataSelectie(Settings settings) {
            this.settings = settings;
            InitializeComponent();
            jeroenFolder.Text = settings.JeroenPrijzen;
            solaredgeFolder.Text = settings.SolarEdge;
            slimmeMeterFolder.Text = settings.SlimmeMeter;
            sessyFolder.Text = settings.Sessy;
            resultaatFolder.Text = settings.Resultaat;
            if(DateTime.TryParseExact(settings.PeriodeStart, dateFormats, null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime start)) {
                periodeStart.Value = start;
            } else {
                periodeStart.Value = new DateTime(DateTime.Now.Year - 1, 1, 1);
            }
            if(DateTime.TryParseExact(settings.PeriodeEnd, dateFormats, null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime end)) {
                periodeEnd.Value = end;
            } else {
                periodeEnd.Value = new DateTime(DateTime.Now.Year, 1, 1);
            }
        }

        private void OnSaveClick(object sender, EventArgs e) {
            if(UpdateSettings()) {
                settings.Save();
            }
        }

        private void OnCompute(object sender, EventArgs e) {
            if (UpdateSettings()) {
                new DataGenerator(settings).GenerateData();
            }
        }

        bool UpdateSettings() {
            if(ValidateValues()) {
                string jeroenValue = jeroenFolder.Text;
                string solaredgeValue = solaredgeFolder.Text;
                string slimmeMeterValue = slimmeMeterFolder.Text;
                string sessyFolderValue = sessyFolder.Text;
                string resultaatFolderValue = resultaatFolder.Text;
                var start = periodeStart.Value;
                var end = periodeEnd.Value;

                settings.JeroenPrijzen = jeroenValue;
                settings.SolarEdge = solaredgeValue;
                settings.SlimmeMeter = slimmeMeterValue;
                settings.Sessy = sessyFolderValue;
                settings.Resultaat = resultaatFolderValue;
                settings.PeriodeStart = start.ToString(dateFormat);
                settings.PeriodeEnd = end.ToString(dateFormat);
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
            var start = periodeStart.Value;
            var end = periodeEnd.Value;
            return
                ValidateValue(jeroenValue, "Jeroen prijzen folder") &&
                ValidateValue(solaredgeValue, "Solar Edge download folder") &&
                ValidateValue(slimmeMeterValue, "Slimme meter download folder") &&
                ValidateValue(sessyFolderValue, "Sessy download folder") &&
                ValidateValue(resultaatFolderValue, "Resultaat folder") &&
                ValidateValue(start, end, "Periode start", "Periode eind");
        }

        private static bool ValidateValue(
            DateTime start,
            DateTime end, string startName,
            string endName
        ) {
            if(end <= start) {
                _ = MessageBox.Show($"{endName} moet na {startName} zijn", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool ValidateValue(string folder, string name) {
            if(string.IsNullOrEmpty(folder)) {
                _ = MessageBox.Show($"{name} is niet ingevuld", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(!Directory.Exists(folder)) {
                _ = MessageBox.Show($"{name} bestaat niet", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void OpenJeroen(object sender, System.EventArgs e) {
            string jeroenValue = jeroenFolder.Text;
            if(ValidateValue(jeroenValue, "Jeroen prijzen folder")) {
                _ = Process.Start("explorer.exe", jeroenValue);
            }
        }

        private void OpenSolarEdge(object sender, System.EventArgs e) {
            string solaredgeValue = solaredgeFolder.Text;
            if(ValidateValue(solaredgeValue, "Solar Edge download folder")) {
                _ = Process.Start("explorer.exe", solaredgeValue);
            }
        }

        private void OpenSlimmeMeter(object sender, System.EventArgs e) {
            string slimmeMeterValue = slimmeMeterFolder.Text;
            if(ValidateValue(slimmeMeterValue, "Slimme meter download folder")) {
                _ = Process.Start("explorer.exe", slimmeMeterValue);
            }
        }

        private void OpenSessy(object sender, System.EventArgs e) {
            string sessyFolderValue = sessyFolder.Text;
            if(ValidateValue(sessyFolderValue, "Sessy download folder")) {
                _ = Process.Start("explorer.exe", sessyFolderValue);
            }
        }

        private void OpenResultaat(object sender, System.EventArgs e) {
            string resultaatFolderValue = resultaatFolder.Text;
            if(ValidateValue(resultaatFolderValue, "Resultaat folder")) {
                _ = Process.Start("explorer.exe", resultaatFolderValue);
            }
        }
    }
}
