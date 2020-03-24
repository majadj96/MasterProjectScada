using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInterface.BaseError;

namespace UserInterface.Model
{
    public class Substation: BindableBase
    {
        private string description;
        private string name;
        private string gid;
        private Breaker breaker;
        private Transformator transformator;
        private TapChanger tapChanger;
        private List<AsynchronousMachine> asynchronousMachines;
        private List<Disconector> disconectors;

        public Substation(string description, string name, string gid)
        {
            AsynchronousMachines = new List<AsynchronousMachine>();
            Disconectors = new List<Disconector>();
            Description = description;
            Name = name;
            Gid = gid;
        }
        public Substation() { }
        public Breaker Breaker { get => breaker; set => breaker = value; }
        public Transformator Transformator { get => transformator; set => transformator = value; }
        public TapChanger TapChanger { get => tapChanger; set => tapChanger = value; }
        public List<AsynchronousMachine> AsynchronousMachines { get => asynchronousMachines; set => asynchronousMachines = value; }
        public List<Disconector> Disconectors { get => disconectors; set => disconectors = value; }
        public string Description { get => description; set => description = value; }
        public string Name { get => name; set => name = value; }
        public string Gid { get => gid; set => gid = value; }
    }
}