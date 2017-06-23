using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Fot.Lan.Models;

namespace Fot.Lan.Services
{
    public class StaffService : ServiceBase
    {

        public List<StaffViewModel> GetStaffList()
        {

            var phase = GetCapturePhase();

            if(phase == CapturePhase.None) return new List<StaffViewModel>();

            if (phase == CapturePhase.PreTest)
            {

                return Context.AdminUsers.Where(x => x.PreTestPhoto == null)
                    .Select(x => new StaffViewModel
                    {
                        AdminId = x.AdminId,
                       Firstname = x.Firstname,
                       Lastname = x.Lastname

                    }).ToList();

            }

           
                return Context.AdminUsers.Where(x => x.PreTestPhoto != null && x.PostTestPhoto == null)
                    .Select(x => new StaffViewModel
                    {
                        AdminId = x.AdminId,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname

                    }).ToList();

            
        }


        public CapturePhase GetCapturePhase()
        {

           var items = Context.AdminUsers.Select(y => new
            {
                HasCandidates = Context.Candidates.Any(),
                HasAnyStarted = Context.Candidates.Any(x => x.AssessmentStarted),
                HasPreTestCapture = Context.AdminUsers.Any(x => x.PreTestPhoto  != null && x.DownloadDate >= DateTime.Today)

            }).FirstOrDefault();

            if (items.HasCandidates && items.HasAnyStarted == false)
            {
                
                return CapturePhase.PreTest;
            }


            if (items.HasCandidates == false && items.HasPreTestCapture)
            {
                
                return CapturePhase.PostTest;
            }



            return CapturePhase.None;
        }

        public void UpdatePhoto(int staffAdminId, byte[] imgBytes, int captureAdminId, CapturePhase phase)
        {
            var item = Context.AdminUsers.FirstOrDefault(x => x.AdminId == staffAdminId && x.Synchronized == false);
            if (item != null)
            {

                if (phase == CapturePhase.PreTest)
                {
                   
                        item.PreTestPhoto = imgBytes;
                        item.PreTestCapturedByAdminId = captureAdminId;
                        
                    
                }
                else if (phase == CapturePhase.PostTest)
                {
                   
                        item.PostTestPhoto = imgBytes;
                        item.PostTestCapturedByAdminId = captureAdminId;
                        
                }

                Context.SaveChanges();
            }

        
        }



        public AdminUser GetAdminUser(string username)
        {

            return Context.AdminUsers.FirstOrDefault(x => x.Username == username);
        }
    }
}
