using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Topology
{
    public class ConnectivityNode : IdentifiedObject
    {
        public ConnectivityNode(long gID) : base(gID)
        {
        }

        private long connectivityNodeContainer = 0;
        private List<Terminal> terminals = new List<Terminal>();

        public long ConnectivityNodeContainer { get => connectivityNodeContainer; set => connectivityNodeContainer = value; }
        public List<Terminal> Terminals { get => terminals; set => terminals = value; }
    }
}
