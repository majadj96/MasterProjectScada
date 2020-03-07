using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class Substation
    {
        private string description;
        private string name;
        private string gid;
        private Transformator transformator;
        private TapChanger tapChanger;
        private List<AsynchronousMachine> asynchronousMachines;
        private List<Disconector> disconectors;
        private List<Breaker> breakers;

        public Substation(string description, string name, string gid)
        {
            AsynchronousMachines = new List<AsynchronousMachine>();
            Disconectors = new List<Disconector>();
            Breakers = new List<Breaker>();
            Description = description;
            Name = name;
            Gid = gid;
        }
        public Substation() { }
        public Transformator Transformator { get => transformator; set => transformator = value; }
        public TapChanger TapChanger { get => tapChanger; set => tapChanger = value; }
        public List<AsynchronousMachine> AsynchronousMachines { get => asynchronousMachines; set => asynchronousMachines = value; }
        public List<Disconector> Disconectors { get => disconectors; set => disconectors = value; }
        public List<Breaker> Breakers { get => breakers; set => breakers = value; }
        public string Description { get => description; set => description = value; }
        public string Name { get => name; set => name = value; }
        public string Gid { get => gid; set => gid = value; }
    }
}