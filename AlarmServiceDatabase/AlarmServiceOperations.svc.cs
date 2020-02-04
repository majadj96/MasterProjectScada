using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AlarmEventService.Server;
using ScadaCommon.Database;

namespace AlarmEventService
{
    [ServiceKnownType(typeof(Alarm))]
    public class AlarmServiceOperations : IAlarmServiceOperations
    {
        public bool AcknowledgeAlarm(IAlarm alarm)
        {
            using (AccessDB db = new AccessDB())
            {
                try
                {
                    Alarm aa = db.Alarms.Find(alarm.ID);
                    if (aa != null)
                    {
                        aa.AlarmAcknowledged = (DateTime)alarm.AlarmAcknowledged;
                        aa.Username = alarm.Username;
                        db.SaveChanges();
                        //log
                        return true;
                    }
                    else
                    {
                        //log
                        return false;
                    }
                }
                catch
                {
                    //log
                    return false;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        public bool AddAlarm(IAlarm alarm)
        {
            using (AccessDB db = new AccessDB())
            {
                try
                {

                    db.Alarms.Add(new Alarm()
                    {
                        GiD = alarm.GiD,
                        AlarmReported = alarm.AlarmReported,
                        AlarmReportedBy = alarm.AlarmReportedBy,
                        Message = alarm.Message,
                        PointName = alarm.PointName,
                        Username = alarm.Username
                    });
                    //log
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    //log
                    return false;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        public bool DeleteAlarm(int id)
        {
            using (AccessDB db = new AccessDB())
            {
                try
                {
                    Alarm a = db.Alarms.Find(id);
                    db.Alarms.Remove(a);
                    //log
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    //log
                    return false;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        public List<Alarm> GetAllAlarms()
        {
            using (AccessDB db = new AccessDB())
            {
                try
                {
                    List<Alarm> list = db.Alarms.ToList();
                    if (list.Count > 0)
                    {
                        //log
                        return list;
                    }
                    else
                    {
                        //log
                        return null;
                    }
                }
                catch
                {
                    //log
                    return null;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
    }
}
