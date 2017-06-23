using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Lan.Models;
using Fot.Lan.Services;

namespace Fot.Lan.admin
{
    public partial class Status : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        public List<StatusViewModel> GetStatusList(int startRow, int maxRows, string searchTerm)
        {
            var ctx = new ServiceBase().Context;


            
            IQueryable<Candidate> items = ctx.Candidates.Where(x => x.AssessmentStarted == true && x.AssessmentCompleted == false);


           

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                items = items.Where(
                       x => 
                       (x.Username.ToLower().Equals(searchTerm.ToLower()) || x.Firstname.ToLower().Equals(searchTerm.ToLower()) ||
                         x.Lastname.ToLower().Equals(searchTerm.ToLower()) || x.MobileNo.Equals(searchTerm)));
            }

            var query = items.Select(x => new StatusViewModel
            {
                Username = x.Username,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                TimeStarted = x.DateTimeStarted,
                SaveCount = x.CandidateAssessment != null ? x.CandidateAssessment.SaveCount : 0,
                LastSaved = x.CandidateAssessment != null ? x.CandidateAssessment.LastUpdated : null,

            });


            if (startRow >= 0)
            {
                query = query.OrderByDescending(x => x.LastSaved).Skip(startRow).Take(maxRows);
            }


            return query.ToList();
        }


        public int CountStatus(string searchTerm)
        {
            var ctx = new ServiceBase().Context;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return ctx.Candidates.Count(x => x.AssessmentStarted == true && x.AssessmentCompleted == false);
            }
            else
            {
                return
                    ctx.Candidates.Count(
                        x => x.AssessmentStarted == true && x.AssessmentCompleted == false &&
                        (x.Username.ToLower().Equals(searchTerm.ToLower()) || x.Firstname.ToLower().Equals(searchTerm.ToLower()) ||
                         x.Lastname.ToLower().Equals(searchTerm.ToLower()) || x.MobileNo.Equals(searchTerm)));
            }

            
        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }
    }
}