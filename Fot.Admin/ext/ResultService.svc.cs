using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Fot.Admin.ext.Models;
using Telerik.Web.UI.com.hisoftware.api2;

namespace Fot.Admin.ext
{
    [ServiceContract(Namespace = "admin.fot.com.ng/CandidateResults")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ResultService
    {
        [OperationContract]
        public ResultResponse GetResult(string CandidateUniqueId, string APIKey)
        {

            if (string.IsNullOrWhiteSpace(CandidateUniqueId)) return null;

            var Context = MyContext.GetContext();

            var entry =
                Context.CampaignEntries.Where(x => x.Candidate.ClientUniqueID.Equals(CandidateUniqueId) && x.Tested && x.Campaign.Partner.APIKey.Equals(APIKey))
                .Include(x => x.AssessmentResults)
                .Include(x => x.Campaign.Partner)
                .Include(x => x.TestSession.Center)
                .Include(x => x.TestSession.Center.Location)
                .FirstOrDefault();

            

            if (entry != null)
            {

                var assessments = Context.Assessments.Select(x => new {x.AssessmentId, x.Name}).Distinct().ToList();

                var infos =
                    entry.AssessmentResults.Select(
                        x =>
                            new AssessmentInfo
                            {
                                AssessmentName = assessments.First(y => y.AssessmentId == x.AssessmentId).Name,
                                Score = x.TestScore.Value
                            }).ToList();

                var result = new CandidateResult
                {
                    AggregateScore = infos.Sum(x => x.Score),
                    Assessments = infos,
                    CandidateUniqueId = CandidateUniqueId,
                    TestCenter = entry.TestSession.Center.Location.LocationName + " / " + entry.TestSession.Center.CenterName,
                    DateTested = entry.DateTested.Value
                };

                return new ResultResponse {Succeeded = true, ErrorMessage = string.Empty, Result = result};

            }
            else
            {
                return new ResultResponse { Succeeded = false, ErrorMessage = "Candidate ID is either invalid or no test scores exist" };
            }


        }

        // Add more operations here and mark them with [OperationContract]
    }
}
