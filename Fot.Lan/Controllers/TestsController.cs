using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fot.Lan.Infrastructure;
using Fot.Lan.Models;
using Fot.Lan.Services;
using Newtonsoft.Json;

namespace Fot.Lan.Controllers
{

    [AllowAnonymous]
    public class TestsController : Controller
    {
        public ActionResult TakeTest(string id)
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Test(string id)
        {

            var service = new CandidateService();

            AppBundle ret = null;

            var item = service.GetCandidate(id);

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

                            ret.candidate_photo = item.CandidatePhoto;

                            service.CandidateStarted(item);



                            
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
                    catch
                    {

                        ret = new AppBundle
                        {
                            flagsuccess = false,
                            error_message = "An error occured trying to retrieve the assessment. exception"
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


        [HttpPost]
        public ActionResult Feedback(string id, FeedbackViewModel vm)
        {

            var ctx = new ServiceBase().Context;


            var entry = ctx.Candidates.FirstOrDefault(x => x.CandidateGuid == id);

            if (entry != null)
            {
                var item = new TestFeedback();

                item.CandidateEntryId = entry.CandidateEntryId;
                item.Directions = vm.Directions;
                item.WaitTime = vm.WaitTime;
                item.Professionalism = vm.Professionalism;
                item.StartTime = vm.StartTime;
                item.Briefing = vm.Briefing;
                item.Registration = vm.Registration;
                item.Overall = vm.Overall;
                item.UnsatisfactoryArea = vm.UnsatisfactoryArea;
                item.SatisfactoryArea = vm.SatisfactoryArea;
                item.Comments = vm.Comments;

                ctx.TestFeedbacks.Add(item);
                ctx.SaveChanges();
            }



            return Content("<span></span>");

        }
    }
}
