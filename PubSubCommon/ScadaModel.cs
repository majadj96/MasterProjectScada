using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace PubSubCommon
{
    [DataContract]
    class ScadaModel
    {
        private string _topicName;
        private string _eventData;
        private List<ScadaUIExchangeModel> points = new List<ScadaUIExchangeModel>();

        [DataMember]
        public string TopicName { get { return _topicName; } set { _topicName = value; } }

        [DataMember]
        public string EventData { get { return _eventData; } set { _eventData = value; } }
        [DataMember]
        public List<ScadaUIExchangeModel> Points {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

    }
}
