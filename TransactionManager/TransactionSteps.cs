﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace TransactionManager
{
    public static class TransactionSteps
    {
        public static void BeginTransaction()
        {
            bool isPrepared = Prepare();
            if (!isPrepared)
            {
                Rollback(); //da li i ovde ide rollback ili samo kad commit failuje? 
                return;
            }

            bool isCommited = Commit();
            if(!isCommited)
            {
                Rollback();
                return;
            }
            else //uspesan commit -> uskladjen je model na svim servisima -> kazi NMS-u da prosledi model na UI
            {
                TMData.NotifyNMSProxy.UpdateUIModel();
            }

            
        }

        private static bool Prepare()
        {
            foreach (var service in TMData.CompleteEnlistedServices)
            {
                try
                {
                    bool iSprepared = service.Prepare();
                    if (!iSprepared)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                }
                
            }
            return true;
        }

        private static bool Commit()
        {
            foreach (var service in TMData.CompleteEnlistedServices)
            {
                try
                {
                    bool isCommited = service.Commit();
                    if (!isCommited)
                    {
                        return false;
                    }                        
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return true;
        }

        private static void Rollback()
        {
            foreach (var service in TMData.CompleteEnlistedServices)
            {
                try
                {
                    service.Rollback();
                }
                catch (Exception)
                {
                }
                
            }
        }


    }
}
