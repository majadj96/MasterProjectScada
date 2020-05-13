using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryCore.Interfaces
{
    [ServiceContract]
    public interface IAlarmEventRepository
    {

        [OperationContract]
        void AddEvent(Event newEvent);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
