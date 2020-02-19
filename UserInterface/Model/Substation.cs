using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class Substation
    {
        private Breaker breaker;
        private Transformator transformator;
        private TapChanger tapChanger;
        private List<AsynchronousMachine> asynchronousMachines;
        private List<Disconector> disconectors;

        public Substation()
        {
            AsynchronousMachines = new List<AsynchronousMachine>();
            Disconectors = new List<Disconector>();
        }

        public Breaker Breaker { get => breaker; set => breaker = value; }
        public Transformator Transformator { get => transformator; set => transformator = value; }
        public TapChanger TapChanger { get => tapChanger; set => tapChanger = value; }
        public List<AsynchronousMachine> AsynchronousMachines { get => asynchronousMachines; set => asynchronousMachines = value; }
        public List<Disconector> Disconectors { get => disconectors; set => disconectors = value; }
    }
}