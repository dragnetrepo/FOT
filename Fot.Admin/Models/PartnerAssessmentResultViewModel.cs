using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Fot.Admin.Services;

namespace Fot.Admin.Models
{
    public class PartnerAssessmentResultViewModel
    {
        public int CampaignEntryId { get; set; }
        public string CandidateName { get; set; }
        public DateTime DateTested { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int CandidateId { get; set; }

        public int CampaignId { get; set; }

        public bool? CandidatePhoto { get; set; }

        public bool AllowReview { get; set; }

        public string PhotoUrl
        {
            get
            {
                if (CandidatePhoto.HasValue == false || CandidatePhoto.Value == false) return string.Empty;

                var campaignFolder = CampaignId;
                //var str = campaignFolder + "/" + string.Format("{0}_{1}.jpg", CandidateId, CampaignEntryId);
                var str = CampaignEntryId;


                return "<a href='javascript:void(0)' onclick='ShowPhoto(\"" + str + "\")'>View Photo</a>";
            }
        }

        public IEnumerable<PartnerResultViewModel> ResultList { get; set; }


        public string FormatedResultsTable
        {
            get
            {
                var sb = new StringBuilder();

                var str = @" <tr><td class='assessmentContent'>{0}</td>
                                  <td class='assessmentContent'>{1}</td>
                                     <td class='assessmentContent'>{2}</td></tr>";

               
                
                foreach (var result in ResultList)
                {
                    var page = result.AssessmentType == AssessmentType.MCQ ? "ResultReview.aspx" : "EssayReview.aspx";

                    var urlStr = string.Empty;

                    if (result.AssessmentType == AssessmentType.MCQ)
                    {
                        

                         urlStr = result.PartnerOwned || AllowReview? string.Format("<a href='{0}?id={1}'>Review</a>", page, result.EntryId) : "&nbsp;";  //show review only if partner owns the assessment

                         //urlStr = string.Format("<a href='{0}?id={1}'>Review</a>", page, result.EntryId); //show review regardless of ownership
                    }
                    else
                    {
                        urlStr = string.Format("<a href='{0}?id={1}'>Review</a>", page, result.EntryId);
                    }
                    

                    var temp = String.Format(str, result.AssessmentName, result.TestScore, urlStr);

                    sb.Append(temp);
                }


                return sb.ToString();
            }
        }


        public string StartAndEndTime
        {
            get
            {
                if (!StartTime.HasValue) return string.Empty;

                var str =
                    "<tr><td class='timeHeader'><strong>Start Time:</strong> " + StartTime.Value.ToString("hh:mm:ss tt") + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>End Time:</strong> " + EndTime.Value.ToString("hh:mm:ss tt") + "</td>" +
                    "<td class='timeHeader' style='width: 100px;'>&nbsp;</td><td class='timeHeader' style='width: 100px;'>&nbsp;</td></tr>";

                return str;
            }
        }
    }


    public class PartnerResultViewModel
    {
        public int EntryId { get; set; }

        public int AssessmentId { get; set; }
        public bool PartnerOwned { get; set; }
        public string AssessmentName { get; set; }
        public int? TestScore { get; set; }
        public AssessmentType AssessmentType { get; set; }
    }
}