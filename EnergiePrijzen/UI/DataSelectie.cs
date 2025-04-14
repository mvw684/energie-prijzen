using System.Windows.Forms;

namespace EnergiePrijzen.UI
{
    public partial class DataSelectie : Form {

        private readonly Config.Settings settings;
        
        public DataSelectie(Config.Settings settings) {
            this.settings = settings;
            InitializeComponent();
        }
    }
}
