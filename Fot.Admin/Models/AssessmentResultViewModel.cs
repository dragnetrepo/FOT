using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Fot.Admin.Infrastructure;

namespace Fot.Admin.Models
{
    public class AssessmentResultViewModel
    {
        public int CampaignEntryId { get; set; }

        public int CandidateId { get; set; }

        public int CampaignId { get; set; }
        public string CandidateName { get; set; }
        public DateTime DateTested { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? CandidatePhoto { get; set; }

        public string PhotoUrl
        {
            get
            {
                if (CandidatePhoto.HasValue == false || CandidatePhoto.Value == false) return string.Empty;

                var campaignFolder =  CampaignId;
                //var str = campaignFolder + "/" + string.Format("{0}_{1}.jpg", CandidateId, CampaignEntryId);
                var str = CampaignEntryId;
                

                return "<a href='javascript:void(0)' onclick='ShowPhoto(\"" + str + "\")'>View Photo</a>";
            }
        }
        public IEnumerable<ResultViewModel> ResultList { get; set; }


        public string FormatedResultsTable
        {
            get
            {
                var sb = new StringBuilder();

                var str = @" <tr><td class='assessmentContent'>{0}</td>
                                  <td class='assessmentContent'>{1}</td>
                                     <td class='assessmentContent'><a href='{3}?id={2}'>Review</a></td></tr>";
                foreach (var result in ResultList)
                {
                    var page = result.AssessmentType == AssessmentType.MCQ ? "ResultReview.aspx" : "EssayReview.aspx";
                    var temp = String.Format(str, result.AssessmentName, result.TestScore, result.EntryId,page);

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


    public class ResultViewModel
    {
        public int EntryId { get; set; }

        public int AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public int? TestScore { get; set; }
        public AssessmentType AssessmentType { get; set; }
    }
}