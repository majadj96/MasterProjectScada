using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AlarmEventServiceDatabase.Server;
using Common.AlarmEvent;

namespace AlarmEventServiceDatabase
{
    [ServiceKnownType(typeof(Alarm))]
    public class AlarmServiceOperations : IAlarmServiceOperations
    {
        public bool AcknowledgeAlarm(Alarm alarm)
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
                        aa.AlarmAck = true;

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

        public void AddAlarm(Alarm alarm)
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
                        Username = alarm.Username,
                         AlarmAck = false
                    });
                    //log
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //log
                    return;
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
