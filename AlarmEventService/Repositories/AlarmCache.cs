using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventService.Repositories
{
    public class AlarmCache
    {
        private Dictionary<string, Alarm> alarmCache = new Dictionary<string, Alarm>();
    }
}
