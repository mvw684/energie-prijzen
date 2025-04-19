using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnergiePrijzen.Config;

namespace EnergiePrijzen.Data {
    internal class DataGenerator {
        private Settings settings;

        public DataGenerator(Settings settings) => this.settings = settings;

        internal void GenerateData() => throw new NotImplementedException();
    }
}
