using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class TapChanger:PowerSystemResource
    {
        private int highStep;
        private int lowStep;
        private int normalStep;

        public TapChanger(string gID) : base(gID)
        {
        }

        public int HighStep { get => highStep; set => highStep = value; }
        public int LowStep { get => lowStep; set => lowStep = value; }
        public int NormalStep { get => normalStep; set => normalStep = value; }
    }
}
