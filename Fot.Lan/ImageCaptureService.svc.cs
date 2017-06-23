using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Fot.Lan.Models;
using Fot.Lan.Services;

namespace Fot.Lan
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ImageCaptureService
    {
     

        [OperationContract]
        public ResponseObj GetCandidates(string username, string password)
        {
            var settingService = new SettingService();
            if(settingService.LoginIsValid(username, password))
            {
                return new ResponseObj
                {
                    Succeeded = true,
                    Candidates = new CandidateService().GetUncapturedCandidates()
                   

                };
            }
            else
            {
                return new ResponseObj
                    {
                        Succeeded = false,
                        ErrorMessage = "Invalid username or password."

                    };
            }
        }



     

        [OperationContract]
        public ResponseObj UpdateCandidate(int CandidateId, byte[] imgBytes, string username, string password)
        {
            
            var settingService = new SettingService();
            if (settingService.LoginIsValid(username, password))
            {

                var adminUser = new StaffService().GetAdminUser(username);

                new CandidateService().UpdateCandidatePhoto(CandidateId, imgBytes, adminUser.AdminId);

                return new ResponseObj
                    {
                        Succeeded = true
                    };
            }
            else
            {
                return new ResponseObj
                    {
                        Succeeded = false,
                        ErrorMessage = "Invalid username or password."

                    };
            }
        }

        // Add more operations here and mark them with [OperationContract]

        [OperationContract]
        public StaffResponseObj GetStaffList(string username, string password)
        {
            var settingService = new SettingService();
            if (settingService.LoginIsValid(username, password))
            {
                var staffService = new StaffService();
                return new StaffResponseObj
                {
                    Succeeded = true,
                    StaffList = staffService.GetStaffList(),
                    CapturePhase = staffService.GetCapturePhase()
                };
            }
            else
            {
                return new StaffResponseObj
                {
                    Succeeded = false,
                    ErrorMessage = "Invalid username or password."

                };
            }
        }





        [OperationContract]
        public StaffResponseObj UpdateStaff(int AdminId, byte[] imgBytes, string username, string password, CapturePhase phase)
        {

            var settingService = new SettingService();
            if (settingService.LoginIsValid(username, password))
            {
                var staffService = new StaffService();

                var adminUser = staffService.GetAdminUser(username);
                staffService.UpdatePhoto(AdminId, imgBytes, adminUser.AdminId, phase);

                return new StaffResponseObj
                {
                    Succeeded = true
                };
            }
            else
            {
                return new StaffResponseObj
                {
                    Succeeded = false,
                    ErrorMessage = "Invalid username or password."

                };
            }
        }
    }


    public class ResponseObj
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public List<CandidateViewModel> Candidates { get; set; } 

        
    }

    public class StaffResponseObj
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public CapturePhase CapturePhase { get; set; }
        public List<StaffViewModel> StaffList { get; set; }


    }


    public enum CapturePhase
    {
        None = 0,
        PreTest = 1,
        PostTest = 2
    }
}
