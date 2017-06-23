using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using Fot.Admin.Models;
using Fot.Admin.Services;
using Mindscape.Raygun4Net;

namespace Fot.Admin
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FotService
    {


        public string VersionHash = "15ba233d69c54d4683c11e09f531f68e";

        [OperationContract]
        public SchedulePackage GetSchedules(string username, string password, string hash)
        {
            if (VersionHash == hash)
            {
                if (new AdminUserService().LoginIsValid(username, password))
                {
                    var user = new AdminUserService().GetAdminUserByName(username);
                    if (user.Active)
                    {

                        if (user.LastPasswordChangedDate.HasValue == false ||
                            DateTime.Today.Subtract(user.LastPasswordChangedDate.Value).TotalDays > 90)
                        {


                            var message = user.LastPasswordChangedDate.HasValue
                                ? "Password change is required at least once every 3 months. Please login to http://admin.fot.com.ng to change your password."
                                : "Password change is required on first login. Please login to http://admin.fot.com.ng to change your password.";


                            return new SchedulePackage
                            {
                                IsDone = false,
                                ErrorMessage = message

                            };

                        }




                        if (user.IsCenterAdmin)
                        {
                            var scheduleDownloadService = new ScheduleDownloadService();

                            if (!scheduleDownloadService.EndOfDayTriggered(user.CenterId.Value))
                            {
                                var service = new TestScheduleService();


                                var package = new SchedulePackage
                                {
                                    ScheduleList = service.GetTestSchedules(user.CenterId.Value),
                                    AssessmentList = service.GetAssessmentsRequiredForCenter(user.CenterId.Value),
                                    StaffList = service.GetSupportStaff(user.CenterId.Value),
                                    IsDone = true,
                                    DownloadDate = DateTime.Today


                                };

                                new ScheduleDownloadService().AddEntry(user.CenterId.Value);

                                new AccessLogService().LogEntry(new AccessLog
                                {
                                    AdminId = user.AdminId,
                                    LogEntryType = "Schedule Download",
                                    LogEntryDetails = "User downloaded schedules",
                                    LogDate = DateTime.Now,
                                    IpAddress = HttpContext.Current.Request.UserHostAddress,
                                    UserAgent = HttpContext.Current.Request.UserAgent
                                });

                                return package;
                            }
                            else
                            {
                                return new SchedulePackage
                                {
                                    IsDone = false,
                                    ErrorMessage =
                                        "Specified center has already triggered the 'End of Day' event. No more schedules can be downloaded for the rest of the day."

                                };
                            }
                        }
                        else
                        {
                            return new SchedulePackage
                            {
                                IsDone = false,
                                ErrorMessage =
                                    "Specified admin user does not belong to a center and cannot download a test schedule."

                            };
                        }
                    }
                    else
                    {
                        return new SchedulePackage
                        {
                            IsDone = false,
                            ErrorMessage = "Specified admin user account is inactive."

                        };
                    }
                }
                else
                {
                    return new SchedulePackage
                    {
                        IsDone = false,
                        ErrorMessage = "Invalid username or password."

                    };
                }
            }
            else
            {
                return new SchedulePackage
                {
                    IsDone = false,
                    ErrorMessage = "You are using an unsupported version of LanMonitor. Contact the administrator for the most recent version."

                };
            }
        }


        [OperationContract]
        public BundlePackage GetAssessmentPackage(string username, string password, int BundleId)
        {
            if (new AdminUserService().LoginIsValid(username, password))
            {
                var user = new AdminUserService().GetAdminUserByName(username);

                if (user.Active)
                {

                    if (user.LastPasswordChangedDate.HasValue == false || DateTime.Today.Subtract(user.LastPasswordChangedDate.Value).TotalDays > 90)
                    {


                        var message = user.LastPasswordChangedDate.HasValue ? "Password change is required at least once every 3 months. Please login to http://admin.fot.com.ng to change your password." : "Password change is required on first login. Please login to http://admin.fot.com.ng to change your password.";


                        return new BundlePackage
                        {
                            IsDone = false,
                            ErrorMessage = message

                        };

                    }


                    if (user.IsCenterAdmin)
                    {
                        var bundle = new DTOService().GetBundlePackage(BundleId);
                        bundle.DownloadDate = DateTime.Today;

                        new AccessLogService().LogEntry(new AccessLog { AdminId = user.AdminId, LogEntryType = "Assessment Bundle Download", LogEntryDetails = "User downloaded [" + bundle.BundleName + "] ", LogDate = DateTime.Now, IpAddress = HttpContext.Current.Request.UserHostAddress, UserAgent = HttpContext.Current.Request.UserAgent });

                        return bundle;

                    }
                    else
                    {
                        return new BundlePackage
                            {
                                IsDone = false,
                                ErrorMessage =
                                    "Specified admin user does not belong to a center and cannot download an assessment."

                            };
                    }
                }
                else
                {
                    return new BundlePackage
                    {
                        IsDone = false,
                        ErrorMessage = "Specified admin user account is inactive."

                    };
                }
            }
            else
            {
                return new BundlePackage
                {
                    IsDone = false,
                    ErrorMessage = "Invalid username or password."

                };
            }
        }

        [OperationContract]
        public List<AssessmentBundleViewModel> GetRequiredAssessments(int CenterId)
        {

            return new TestScheduleService().GetAssessmentsRequiredForCenter(CenterId);


        }



        [OperationContract]
        public bool ResultUpdate(ResultUpdateModel result)
        {
            try
            {
                return new ResultUpdateService().ResultUpdate(result);

            }
            catch (Exception ex)
            {
               // var client =  new RaygunClient();

               // result.CandidatePhoto = null;

               // var dict = new Dictionary<string, ResultUpdateModel>();
               // dict.Add(Guid.NewGuid().ToString(), result);

               //client.Send(ex, null, dict);

                return false;
            }


        }


      


        [OperationContract]
        public SchedulePackage TriggerEndOfDay(string username, string password, List<PhotoLogModel> items)
        {
            if (new AdminUserService().LoginIsValid(username, password))
            {
                var user = new AdminUserService().GetAdminUserByName(username);

                if (user.Active)
                {

                    if (user.LastPasswordChangedDate.HasValue == false || DateTime.Today.Subtract(user.LastPasswordChangedDate.Value).TotalDays > 90)
                    {


                        var message = user.LastPasswordChangedDate.HasValue ? "Password change is required at least once every 3 months. Please login to http://admin.fot.com.ng to change your password." : "Password change is required on first login. Please login to http://admin.fot.com.ng to change your password.";


                        return new SchedulePackage
                        {
                            IsDone = false,
                            ErrorMessage = message

                        };

                    }



                    if (user.IsCenterAdmin)
                    {
                        var flag = new ScheduleDownloadService().TriggerEndOfDay(user.CenterId.Value);


                        if (flag)
                        {
                            var ctx = new ServiceBase().Context;

                            var logs =
                                items.Select(
                                    x =>
                                        new PhotoLog
                                        {
                                            CandidateId = x.CandidateId,
                                            AdminUserId = x.AdminUserId,
                                            ExpungeDate = x.ExpungeDate
                                        }).ToList();
                            ctx.PhotoLogs.AddRange(logs);

                            ctx.SaveChanges();
                        }


                        var res = new SchedulePackage
                            {
                                IsDone = flag,
                                ErrorMessage = flag ? "" : "Could not trigger end of day."
                            };

                        if(flag)
                        new AccessLogService().LogEntry(new AccessLog { AdminId = user.AdminId, LogEntryType = "End Of Day", LogEntryDetails = "User triggered end of day.", LogDate = DateTime.Now, IpAddress = HttpContext.Current.Request.UserHostAddress, UserAgent = HttpContext.Current.Request.UserAgent });

                        return res;
                    }
                    else
                    {
                        return new SchedulePackage
                            {
                                IsDone = false,
                                ErrorMessage = "Specified admin is not a center admin."
                            };
                    }
                }
                else
                {
                    return new SchedulePackage
                    {
                        IsDone = false,
                        ErrorMessage = "Specified admin user account is inactive."

                    };
                }

            }
            else
            {
                return new SchedulePackage { IsDone = false, ErrorMessage = "Invalid login details." };
            }
        }


        [OperationContract]
        public SchedulePackage PersonnelPhotoUpdate(string username, string password, List<PersonnelPhotoUpdateModel> staffList)
        {

            if (new AdminUserService().LoginIsValid(username, password))
            {
                var user = new AdminUserService().GetAdminUserByName(username);

                if (user.Active)
                {

                    if (user.LastPasswordChangedDate.HasValue == false || DateTime.Today.Subtract(user.LastPasswordChangedDate.Value).TotalDays > 90)
                    {


                        var message = user.LastPasswordChangedDate.HasValue ? "Password change is required at least once every 3 months. Please login to http://admin.fot.com.ng to change your password." : "Password change is required on first login. Please login to http://admin.fot.com.ng to change your password.";


                        return new SchedulePackage
                        {
                            IsDone = false,
                            ErrorMessage = message

                        };

                    }



                    if (user.IsCenterAdmin)
                    {
                        var flag = new SupportStaffService().UpdateStaffPhoto(staffList);
                        var res = new SchedulePackage
                        {
                            IsDone = flag,
                            ErrorMessage = flag ? "" : "Could not synchronize personnel photos"
                        };

                        if(flag)
                        new AccessLogService().LogEntry(new AccessLog { AdminId = user.AdminId, LogEntryType = "Personnel Photo Sync", LogEntryDetails = "User synchronized personnel photos.", LogDate = DateTime.Now, IpAddress = HttpContext.Current.Request.UserHostAddress, UserAgent = HttpContext.Current.Request.UserAgent });

                        return res;
                    }
                    else
                    {
                        return new SchedulePackage
                        {
                            IsDone = false,
                            ErrorMessage = "Specified admin is not a center admin."
                        };
                    }
                }
                else
                {
                    return new SchedulePackage
                    {
                        IsDone = false,
                        ErrorMessage = "Specified admin user account is inactive."

                    };
                }

            }
            else
            {
                return new SchedulePackage { IsDone = false, ErrorMessage = "Invalid login details." };
            }
           
        }
        
        
        // Add more operations here and mark them with [OperationContract]
    }
}
