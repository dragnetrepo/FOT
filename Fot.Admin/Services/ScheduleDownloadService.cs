using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class ScheduleDownloadService : ServiceBase
    {



        public bool EntryExists(int CenterId, DateTime Date)
        {
            return Context.ScheduleDownloads.Any(x => x.CenterId == CenterId && x.EntryDate.Equals(Date));
        }

        public void AddEntry(int CenterId)
        {
            if(EntryExists(CenterId, DateTime.Today))
            {
                return;
                
            }

            var item = new ScheduleDownload
                {
                    CenterId = CenterId,
                    Downloaded = true,
                    EndOfDayTriggered = false,
                    EntryDate = DateTime.Today
                };

            Context.ScheduleDownloads.Add(item);

            Context.SaveChanges();

        }



        public bool EndOfDayTriggered(int CenterId)
        {
            var item =
                Context.ScheduleDownloads.FirstOrDefault(
                    x => x.CenterId == CenterId && x.EntryDate.Equals(DateTime.Today));

            if(item != null)
            {
                return item.EndOfDayTriggered;
            }
            else
            {
                return false;
            }
        }

        public bool TriggerEndOfDay(int CenterId)
        {
            
            if (EntryExists(CenterId, DateTime.Today))
            {
                var item =
                    Context.ScheduleDownloads.FirstOrDefault(
                        x => x.CenterId == CenterId && x.EntryDate.Equals(DateTime.Today));

                if(item != null)
                {
                    item.EndOfDayTriggered = true;

                    Context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }
    }
}