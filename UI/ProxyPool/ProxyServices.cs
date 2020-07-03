using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.ProxyPool
{
    public static class ProxyServices
    {
        private static CommandingServiceProxy commandingServiceProxy = null;
        private static AlarmEventServiceProxy alarmEventServiceProxy = null;

        private static readonly object lockCommandingProxy = new object();
        private static readonly object lockAlarmEventProxy = new object();

        public static CommandingServiceProxy CommandingServiceProxy
        {
            get
            {
                if (commandingServiceProxy == null)
                {
                    lock (lockCommandingProxy)
                    {
                        if (commandingServiceProxy == null)
                        {
                            commandingServiceProxy = new CommandingServiceProxy("UICommandingService");
                            commandingServiceProxy.Open();
                        }
                    }
                }
                return commandingServiceProxy;
            }
        }

        public static AlarmEventServiceProxy AlarmEventServiceProxy
        {
            get
            {
                if (alarmEventServiceProxy == null)
                {
                    lock (lockAlarmEventProxy)
                    {
                        if (alarmEventServiceProxy == null)
                        {
                            alarmEventServiceProxy = new AlarmEventServiceProxy("AlarmEventService");
                            alarmEventServiceProxy.Open();
                        }
                    }
                }
                return alarmEventServiceProxy;
            }
        }
    }
}
