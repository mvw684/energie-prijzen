using System.IO;
using System.Windows.Forms;

using EnergiePrijzen.Config;

namespace EnergiePrijzen.UI
{
    public partial class DataSelectie : Form {

        private readonly Settings settings;

        public DataSelectie(Settings settings) {
            this.settings = settings;
            InitializeComponent();
            jeroenFolder.Text = settings.JeroenPrijzen;
            solaredgeFolder.Text = settings.SolarEdge;
            slimmeMeterFolder.Text = settings.SlimmeMeter;
        }

        private void OnSaveClick(object sender, System.EventArgs e) {
            string jeroenValue = jeroenFolder.Text;
            string solaredgeValue = solaredgeFolder.Text;
            string slimmeMeterValue = slimmeMeterFolder.Text;

            if(
                Validate(jeroenValue, "Jeroen prijzin folder") && 
                Validate(solaredgeValue, "Solar Edge download folder") && 
                Validate(slimmeMeterValue, "Slimme meter download folder")
            ) {
                settings.Save();
            }
        }

        private static bool Validate(string folder, string name) {
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
    }
}
