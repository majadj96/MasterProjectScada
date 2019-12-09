using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class ConnectivityNodeContainer : PowerSystemResource
    {
        public ConnectivityNodeContainer(long gID) : base(gID)
        {
        }

        private List<long> connectivityNodes = new List<long>();

        public List<long> ConnectivityNodes { get => connectivityNodes; set => connectivityNodes = value; }
    }
}
