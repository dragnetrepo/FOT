using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class AccessLogService : ServiceBase
    {

  

        public List<AccessLogViewModel> GetLogs(int AdminId, string LogType, int startRow, int maxRows)
        {
            IEnumerable<AccessLogViewModel> query = null;

            IQueryable<AccessLog> ctx = Context.AccessLogs;

            if (AdminId > 0)
            {
                ctx = ctx.Where(x => x.AdminId == AdminId);
            }

            if (LogType != "0")
            {
                ctx = ctx.Where(x => x.LogEntryType == LogType);
            }

            query = ctx.Select(x => new AccessLogViewModel
            {
                EntryId = x.EntryId,
                Username = x.AdminUser.Username,
                LogEntryType = x.LogEntryType,
                LogEntryDetails = x.LogEntryDetails,
                LogDate = x.LogDate.Value,
                IpAddress = x.IpAddress

            }).OrderByDescending(x => x.LogDate);


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();

        }

        public int CountLogs(int AdminId, string LogType)
        {
            IQueryable<AccessLog> ctx = Context.AccessLogs;

            if (AdminId > 0)
            {
                ctx = ctx.Where(x => x.AdminId == AdminId);
            }

            if (LogType != "0")
            {
                ctx = ctx.Where(x => x.LogEntryType == LogType);
            }

              return ctx.Count();
           
        
        }


        public void LogEntry(AccessLog log)
        {    
                Context.AccessLogs.Add(log);
                Context.SaveChanges();
 
        }
    }
}