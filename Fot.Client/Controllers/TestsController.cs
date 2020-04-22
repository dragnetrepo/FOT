using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fot.Client.Infrastructure;
using Fot.Client.Models;
using Fot.Client.Services;
using Newtonsoft.Json;

namespace Fot.Client.Controllers
{

    [AllowAnonymous]
    public class TestsController : Controller
    {
        public ActionResult TakeTest(string id)
        {

            var vm = new ProviewInitModel();

            var ctx = new ServiceBase().Context;


            var candidate = ctx.CandidateAssessments.Where(x => x.CandidateGuid == id).Select(x => new { x.CampaignEntry.Campaign.EnableProctoring, x.CampaignEntry.Candidate.Username }).FirstOrDefault();

            if (candidate != null)
            {
                vm.Username = candidate.Username;

                vm.ProctoringEnabled = candidate.EnableProctoring;
            }

            vm.SessionToken = Guid.NewGuid().ToString();

            vm.ProviewToken = ConfigurationManager.AppSettings["ProctorToken"];




            return View(vm);
        }


        [AllowAnonymous]
        public ActionResult Test(string id)
        {

            var service = new CandidateService();

            var item = service.GetCandidateByScheduleId(id);

            AppBundle ret = null;

            if (item != null)
            {
                if (item.AssessmentCompleted)
                {
                    ret = new AppBundle
                    {
                        flagsuccess = false,
                        error_message = "Candidate has already completed the assessment."
                    };
                }
                else
                {
                    try
                    {
                        var bundleService = new BundleService();
                        ret = bundleService.GetAppBundle(item.CandidateGuid, item.BundleId);

                        if (ret != null)
                        {
                            ret.flagsuccess = true;


                            //  service.CandidateStarted(item);




                        }
                        else
                        {
                            ret = new AppBundle
                            {
                                flagsuccess = false,
                                error_message = "An error occured trying to retrieve the assessment. null returned"
                            };
                        }
                    }
                    catch (Exception ex)
                    {

                        ret = new AppBundle
                        {
                            flagsuccess = false,
                            error_message = "An error occured trying to retrieve the assessment. exception: " + ex.Message
                        };
                    }


                }

            }
            else
            {
                ret = new AppBundle
                {
                    flagsuccess = false,
                    error_message = "No scheduled assessment exists for the specified id."
                };
            }

            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = ret;


            return jsonNetResult;
        }


        [HttpPost]
        public ActionResult SaveState(string id, AppBundle bundle)
        {
            var bundleService = new BundleService();
            bundleService.SaveState(id, bundle);

            var ret = new ResultResponse { Succeeded = true };

            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = ret;


            return jsonNetResult;

        }

        public ActionResult SubmitTest(string id, AssessmentResponse[] responses)
        {
            var service = new AssessmentService();




            var ret = service.AssessmentSubmitted(id, responses);


            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = ret;


            return jsonNetResult;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SavePlayBackId(ProctorModel model)
        {
            var ctx = new ServiceBase().Context;


            var entry = ctx.CandidateAssessments.Where(x => x.CandidateGuid == model.CandidateGuid).Select(x => x.CampaignEntry).FirstOrDefault();

            if(entry != null)
            {
                entry.ProctorPlaybackId = model.PlaybackId;

                ctx.SaveChanges();
            }

            var ret = new ResultResponse { Succeeded = true };

            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = ret;


            return jsonNetResult;
        }
    }
}
