using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergiePrijzen.Data {
    public class AggregatedDataBase<TData> : ITimeStampedData<TData> {
        
        private TimeStamp timestamp;
        public TimeStamp TimeStamp {
            get => timestamp;
            init => timestamp = value;
        }

    }
}
