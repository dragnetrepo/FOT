using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class AuthorService : ServiceBase
    {
        public List<AssessmentAuthor> GetAuthors()
        {
            return Context.AssessmentAuthors.OrderByDescending(x => x.AuthorId).ToList();
        }

        public List<AssessmentAuthor> GetAuthors(int startRow, int maxRows)
        {
            return Context.AssessmentAuthors.OrderByDescending(x => x.AuthorId).Skip(startRow).Take(maxRows).ToList();
        }

        public int Count()
        {
            return Context.AssessmentAuthors.Count();
        }

        public AppMessage Add(AssessmentAuthor item)
        {
            try
            {
                Context.AssessmentAuthors.Add(item);
                Context.SaveChanges();


                return new AppMessage
                {
                    IsDone = true,
                    Message = "Added Author successfully.",
                    Status = MessageStatus.Success,
                    Data = item.AuthorId
                };
            }

            catch (Exception ex)
            {
                return new AppMessage
                {
                    IsDone = false,
                    Message = "An error occured." + ex.Message,
                    Status = MessageStatus.Error
                };
            }
        }

        public void Delete(int AuthorId)
        {
            AssessmentAuthor item = Context.AssessmentAuthors.Find(AuthorId);

            if (item != null)
            {
                Context.AssessmentAuthors.Remove(item);
                Context.SaveChanges();
            }
        }



    }
}