using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Fot.Lan.AppServices;
using Fot.Lan.Models;

namespace Fot.Lan
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AppService
    {
        [OperationContract]
        public List<CandidateDetailsViewModel> GetCandidatesForSummary()
        {

            var service = new CandidateService();

            return service.GetCandidatesForSummary();

        }

        [OperationContract]
        public List<CandidateDetailsViewModel> GetCandidates()
        {
            var service = new CandidateService();

            return service.GetCandidates();
        }

        [OperationContract]
        public List<RequiredAssessment> GetRequiredAssessments()
        {
            var service = new CandidateService();

            return service.GetRequiredAssessments();
        }

        [OperationContract]
        public List<RequiredAssessment> GetRequiredAssessmentList(List<RequiredAssessment> list)
        {
            var service = new CandidateService();


            return service.GetRequiredAssessmentList(list);
        }

        [OperationContract]
        public void AddRequiredAssessments(List<RequiredAssessment> realList)
        {
            var service = new CandidateService();

            service.AddRequiredAssessments(realList);

        }

        [OperationContract]
        public void AddStaff(List<AdminUser> staffList)
        {
            var service = new CandidateService();

            service.AddStaff(staffList);
        }

        [OperationContract]
        public void DeleteAll(DateTime currentDate)
        {
            var service = new CandidateService();

            service.DeleteAll(currentDate);
        }

        [OperationContract]
        public void DeleteEverything()
        {
            var service = new CandidateService();

            service.DeleteEverything();
        }

        [OperationContract]
        public void Add(List<Candidate> itemList)
        {
            var service = new CandidateService();

            service.Add(itemList);

        }

        [OperationContract]
        public bool CanTriggerEndOfDay()
        {
            var service = new CandidateService();

            return service.CanTriggerEndOfDay();
        }

        [OperationContract]
        public void SaveBundle(AssessmentPackage assessmentPackage)
        {
            var service = new CandidateService();

             service.SaveBundle(assessmentPackage);
        }



        //Settings Service Entries Begin Here

        [OperationContract]
        public void SetImageCaptureSetting(bool flag)
        {
            var service = new SettingService();

            service.SetImageCaptureSetting(flag);
        }

        [OperationContract]
        public bool GetImageCapatureSetting()
        {

            var service = new SettingService();

            return service.GetImageCapatureSetting();

        }

        [OperationContract]
        public string GetCaptureAdminUsername()
        {
            var service = new SettingService();

            return service.GetCaptureAdminUsername();
        }

        [OperationContract]
        public string GetCaptureAdminPassword()
        {
            var service = new SettingService();

            return service.GetCaptureAdminPassword();
        }

        [OperationContract]
        public void UpdateAdminUsernameAndPassword(string username, string password)
        {
            var service = new SettingService();

            service.UpdateAdminUsernameAndPassword(username,password);

        }


        [OperationContract]
        public List<AdminUserViewModel> GetStaffList()
        {
            var service = new SettingService();

            return service.GetStaffList();
        }

        [OperationContract]
        public List<AdminUserViewModel> GetPendingStaffList()
        {
            var service = new SettingService();

            return service.GetPendingStaffList();
        }

        [OperationContract]
        public List<PhotoLog> GetPhotoLogs()
        {
            var service = new SettingService();

            return service.GetPhotoLogs();

        }

        [OperationContract]
        public void SynchronizeStaffList(List<int> list)
        {
            var service = new SettingService();

            service.SynchronizeStaffList(list);
        }


        // Service entries for Syncher

        [OperationContract]
        public List<TestedViewModel> GetTestedList()
        {
            var service = new SettingService();

            return service.GetTestedList();
        }



        [OperationContract]
        public void SynchronizeCandidate(int entryId)
        {
            var service = new SettingService();

            service.SynchronizeCandidate(entryId);
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
