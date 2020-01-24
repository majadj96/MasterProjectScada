using Common.GDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PubSubCommon
{
    [DataContract]
    public class NMSModel
    {
        private string _topicName;

        private string _eventData;

        private List<ResourceDescription> resourceDescs;

        [DataMember]
        public string TopicName { get { return _topicName; } set { _topicName = value; } }

        [DataMember]
        public string EventData { get { return _eventData; } set { _eventData = value; } }

        [DataMember]
        public List<ResourceDescription> ResourceDescs { get => resourceDescs; set => resourceDescs = value; }
    }
}
