using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text.RegularExpressions;
using Context;
using Fot.Admin.ext.Models;
using Fot.Admin.Models;
using Fot.Admin.Services;
using Fot.DTO;

namespace Fot.Admin.ext
{
    [ServiceContract(Namespace = "admin.fot.com.ng/Integration")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class IntegrationService
    {
        [OperationContract]
        public List<SessionTime> GetSessions()
        {
            var list = new List<SessionTime>
            {
                new SessionTime {Index = 1, TimeText = "6:00 AM"},
                new SessionTime {Index = 2, TimeText = "6:30 AM"},
                new SessionTime {Index = 3, TimeText = "7:00 AM"},
                new SessionTime {Index = 4, TimeText = "7:30 AM"},
                new SessionTime {Index = 5, TimeText = "8:00 AM"},
                new SessionTime {Index = 6, TimeText = "8:30 AM"},
                new SessionTime {Index = 7, TimeText = "9:00 AM"},
                new SessionTime {Index = 8, TimeText = "9:30 AM"},
                new SessionTime {Index = 9, TimeText = "10:00 AM"},
                new SessionTime {Index = 10, TimeText = "10:30 AM"},
                new SessionTime {Index = 11, TimeText = "11:00 AM"},
                new SessionTime {Index = 12, TimeText = "11:30 AM"},
                new SessionTime {Index = 13, TimeText = "12:00 PM"},
                new SessionTime {Index = 14, TimeText = "12:30 PM"},
                new SessionTime {Index = 15, TimeText = "1:00 PM"},
                new SessionTime {Index = 16, TimeText = "1:30 PM"},
                new SessionTime {Index = 17, TimeText = "2:00 PM"},
                new SessionTime {Index = 18, TimeText = "2:30 PM"},
                new SessionTime {Index = 19, TimeText = "3:00 PM"},
                new SessionTime {Index = 20, TimeText = "3:30 PM"},
                new SessionTime {Index = 21, TimeText = "4:00 PM"},
                new SessionTime {Index = 22, TimeText = "4:30 PM"},
                new SessionTime {Index = 23, TimeText = "5:00 PM"},
                new SessionTime {Index = 24, TimeText = "5:30 PM"},
                new SessionTime {Index = 25, TimeText = "6:00 PM"}
            };


            return list;
        }

        [OperationContract]
        public EndOfDayInfo GetEndOfDayInfo(int campaignId, int centerId, DateTime testDate)
        {
            var Context = MyContext.GetContext();

            var date = testDate;

            var item =
                Context.Centers.Where(
                    x =>
                        x.TestSessions.Any(
                            t =>
                                t.TestDate.Equals(date) && t.CampaignEntries.Count(p => p.CampaignId == campaignId) > 0 &&
                                t.CenterId == centerId)).
                    Select(x => new
                    {
                        TotalScheduled =
                            x.TestSessions.Where(y => y.TestDate.Equals(date)).Sum(y => y.CampaignEntries.Count(q => q.CampaignId == campaignId)),
                        TotalSessionsCount =
                            x.TestSessions.Count(
                                y =>
                                    y.TestDate.Equals(date) &&
                                    y.CampaignEntries.Count(p => p.CampaignId == campaignId) > 0),
                        TotalSessions =
                            x.TestSessions.Where(
                                y =>
                                    y.TestDate.Equals(date) &&
                                    y.CampaignEntries.Count(p => p.CampaignId == campaignId) > 0)
                                .OrderBy(t => t.TimeIndex),
                        TotalTested =
                            x.TestSessions.Where(y => y.TestDate.Equals(date))
                                .Sum(y => y.CampaignEntries.Count(q => q.Tested && q.CampaignId == campaignId)),
                        TotalPhotoCaptured = x.TestSessions.Where(y => y.TestDate.Equals(date)).Sum(y => y.CampaignEntries.Count(q => q.PhotoCaptured.HasValue && q.PhotoCaptured.Value && q.CampaignId == campaignId)),
                        DownloadedSchedule = x.ScheduleDownloads.Any(s => s.EntryDate.Equals(date) && s.Downloaded),
                        TriggeredEndOfDay =
                            x.ScheduleDownloads.Any(s => s.EntryDate.Equals(date) && s.EndOfDayTriggered)
                    }).FirstOrDefault();

            if (item != null)
            {
                return new EndOfDayInfo
                {
                    TotalCandidates = item.TotalScheduled,
                    TotalTested = item.TotalTested,
                    TotalPhotoCaptured = item.TotalPhotoCaptured,
                    DownloadedSchedule = item.DownloadedSchedule,
                    TriggeredEndOfDay = item.TriggeredEndOfDay,
                    TotalSessions = item.TotalSessionsCount,
                    StartTime = item.TotalSessions.First().TimeText,
                    EndTime = item.TotalSessions.Last().TimeText
                };
            }
            else
            {
                return new EndOfDayInfo();
            }
        }

        [OperationContract]
        public CampaignModel GetCampaign(int campaignId)
        {
            var Context = MyContext.GetContext();

            var item = Context.Campaigns.FirstOrDefault(x => x.CampaignId == campaignId && x.IsUnproctored == false);

            if (item == null) return null;

            return new CampaignModel {CampaignId = item.CampaignId, CampaignName = item.CampaignName};
        }

        [OperationContract]
        public List<CampaignModel> GetCampaigns(int partnerId)
        {
            var Context = MyContext.GetContext();

            var list = Context.Campaigns
                .Where(x => x.PartnerId == partnerId && x.IsUnproctored == false)
                .Select(x => new CampaignModel {CampaignId = x.CampaignId, CampaignName = x.CampaignName})
                .ToList();


            return list;
        }

        [OperationContract]
        public List<CenterModel> GetCenters(List<int> centerIDs)
        {
            var Context = MyContext.GetContext();

            var list =
                Context.Centers.Where(x => centerIDs.Contains(x.CenterId) && x.IsPrivateCenter == false)
                    .Select(x => new CenterModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        Address = x.Address,
                        LocationId = x.LocationId,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        RatePerTested = x.RatePerTested,
                        Active = x.Active
                    }).ToList();

            return list;
        }

        [OperationContract]
        public List<CenterModel> GetAllCenters()
        {
            var Context = MyContext.GetContext();

            var list =
                Context.Centers.Where(x => x.IsPrivateCenter == false)
                    .Select(x => new CenterModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        Address = x.Address,
                        LocationId = x.LocationId,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        RatePerTested = x.RatePerTested,
                        Active = x.Active
                    }).ToList();


            return list;
        }

        [OperationContract]
        public List<CenterModel> GetPartnerCenters(int partnerId)
        {
            var Context = MyContext.GetContext();

            var list =
                Context.Centers.Where(x => x.Active && x.IsPrivateCenter && x.OwnerPartnerId == partnerId)
                    .Select(x => new CenterModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        Address = x.Address,
                        LocationId = x.LocationId,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        RatePerTested = x.RatePerTested,
                        Active = x.Active
                    }).ToList();


            return list;
        }

        [OperationContract]
        public List<PartnerModel> GetPartners(List<int> partnerIDs)
        {
            var Context = MyContext.GetContext();

            var list =
                Context.Partners.Where(x => partnerIDs.Contains(x.PartnerId))
                    .Select(x => new PartnerModel
                    {
                        PartnerId = x.PartnerId,
                        PartnerName = x.PartnerName,
                        Address = x.Address,
                        DateCreated = x.DateCreated
                    }).ToList();

            return list;
        }


        [OperationContract]
        public List<AssessmentModel> GetPartnerAssessments(List<int> assessmentIDs)
        {
            var Context = MyContext.GetContext();

            var list =
                Context.Assessments.Where(x => assessmentIDs.Contains(x.AssessmentId) && x.OwnerPartnerId.HasValue)
                    .Select(x => new AssessmentModel
                    {
                        AssessmentId = x.AssessmentId,
                        AssessmentName = x.Name,
                        PartnerId = x.OwnerPartnerId
                    }).ToList();

            return list;
        }

        [OperationContract]
        public Status ChangeAdminPassword(string username, string oldPassword, string newPassword)
        {
            var Context = MyContext.GetContext();

            AdminUser user = Context.AdminUsers.FirstOrDefault(x => x.Username.Equals(username));

            if (user == null) return new Status {Succeeded = false, Message = "Specified username was not found."};

            var oldHashedPassword = FotSecurity<string>.Hash(oldPassword);


            if (user.Password.Equals(oldHashedPassword))
            {
                user.Password = FotSecurity<string>.Hash(newPassword);

                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Password changed successfully."};
            }
            else
            {
                return new Status {Succeeded = false, Message = "Invalid old password"};
            }
        }


        [OperationContract]
        public List<AssessmentModel> GetAssessmentsByPartnerId(int partnerId)
        {
            var Context = MyContext.GetContext();

            var list =
                Context.Assessments.Where(x => x.OwnerPartnerId.HasValue && x.OwnerPartnerId == partnerId)
                    .Select(x => new AssessmentModel
                    {
                        AssessmentId = x.AssessmentId,
                        AssessmentName = x.Name,
                        PartnerId = x.OwnerPartnerId
                    }).ToList();

            return list;
        }


        [OperationContract]
        public Status UpdatePartnerDetails(int partnerId, string partnerName, string address)
        {
            var Context = MyContext.GetContext();

            var partner = Context.Partners.Find(partnerId);

            if (partner == null) return new Status {Succeeded = false, Message = "Specified partner was not found."};

            try
            {
                partner.PartnerName = partnerName;
                partner.Address = address;

                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Partner details updated successfully."};
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }


        [OperationContract]
        public Status CreatePartnerAndAdmin(string partnerName, string address, string adminEmail, string mobileNumber,
            string firstName,
            string lastName, string password, decimal costPerTestPublic, decimal costPerTestPrivate)
        {
            var Context = MyContext.GetContext();

            if (new AdminUserService().UsernameExists(adminEmail))
                return new Status {Succeeded = false, Message = "Specified admin username has already been used."};

            var emailIsValid = Regex.IsMatch(adminEmail, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (!emailIsValid) return new Status {Succeeded = false, Message = "Specified email is invalid."};

            try
            {
                var partner = new Partner
                {
                    PartnerName = partnerName,
                    IsSelfManaged = true,
                    Address = address,
                    DateCreated = DateTime.Now,
                    CostPerTestPublic = costPerTestPublic,
                    CostPerTestPrivate = costPerTestPrivate
                    
                };

                var admin = new AdminUser
                {
                    Username = adminEmail,
                    Firstname = firstName,
                    Lastname = lastName,
                    MobileNo = mobileNumber,
                    Active = true,
                    CanAuthor = true,
                    CanSchedule = true,
                    HasResultsAccess = true,
                    HasUsersAccess = true,
                    HasCenterUsersAccess = true

                };

                admin.Password = FotSecurity<string>.Hash(password);

                admin.Partner = partner;
                admin.IsPartnerAdmin = true;

                partner.AdminUsers.Add(admin);

                Context.Partners.Add(partner);

                Context.SaveChanges();

                return new Status
                {
                    Succeeded = true,
                    Message = "Partner and admin created successfully.",
                    OptionalId = partner.PartnerId
                };
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }


        [OperationContract]
        public Status UpdateAdmin(int adminId, string mobileNumber, string firstName, string lastName, string password)
        {
            var Context = MyContext.GetContext();

            var admin = Context.AdminUsers.FirstOrDefault(x => x.AdminId == adminId);

            if (admin == null) return new Status {Succeeded = false, Message = "Specified Admin was not found."};


            try
            {
                admin.Firstname = firstName;
                admin.Lastname = lastName;
                admin.MobileNo = mobileNumber;

                if (!string.IsNullOrWhiteSpace(password))
                {
                    admin.Password = FotSecurity<string>.Hash(password);
                }


                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Admin updated successfully."};
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }

        [OperationContract]
        public bool UsernameExists(string username)
        {
            var Context = MyContext.GetContext();

            return Context.AdminUsers.Any(x => x.Username.Equals(username));
        }


        [OperationContract]
        public PartnerAdminModel GetPartnerAdmin(string username)
        {
            var Context = MyContext.GetContext();

            var admin = Context.AdminUsers.Where(x => x.Username.Equals(username) && x.IsPartnerAdmin).Include(x => x.Partner).FirstOrDefault();

            if (admin != null)
            {
                var model = new PartnerAdminModel
                {
                    AdminId = admin.AdminId,
                    PartnerId = admin.PartnerId,
                    Email = admin.Username,
                    Mobile = admin.MobileNo,
                    Firstname = admin.Firstname,
                    Lastname = admin.Lastname,
                    Active = admin.Active,
                    PartnerName = admin.Partner.PartnerName
                };

                return model;
            }

            else
            {
                return null;
            }
        }


        [OperationContract]
        public List<AdminModel> GetPartnerAdminsByPartnerId(int partnerId)
        {
            var Context = MyContext.GetContext();

            var list = Context.AdminUsers.Where(x => x.IsPartnerAdmin && x.PartnerId == partnerId)
                .Select(admin =>
                    new AdminModel
                    {
                        AdminId = admin.AdminId,
                        PartnerId = admin.PartnerId,
                        Email = admin.Username,
                        Mobile = admin.MobileNo,
                        Firstname = admin.Firstname,
                        Lastname = admin.Lastname
                    }).ToList();

            return list;
        }


        [OperationContract]
        public Status ActivateTestCenter(int centerId)
        {
            var Context = MyContext.GetContext();

            var center = Context.Centers.Find(centerId);

            if (center == null) return new Status {Succeeded = false, Message = "Specified center was not found."};

            if (center.Active) return new Status {Succeeded = true, Message = "Specified center was already active."};

            center.Active = true;

            Context.SaveChanges();

            return new Status {Succeeded = true, Message = "Center activated successfully."};
        }

        [OperationContract]
        public Status DeactivateTestCenter(int centerId)
        {
            var Context = MyContext.GetContext();

            var center = Context.Centers.Find(centerId);

            if (center == null) return new Status {Succeeded = false, Message = "Specified center was not found."};

            if (center.Active == false)
                return new Status {Succeeded = true, Message = "Specified center was already inactive."};

            center.Active = false;

            Context.SaveChanges();

            return new Status {Succeeded = true, Message = "Center deactivated successfully."};
        }


        [OperationContract]
        public Status ActivateAdmin(string username)
        {
            var Context = MyContext.GetContext();

            var adminUser = Context.AdminUsers.FirstOrDefault(x => x.Username.Equals(username));

            if (adminUser == null) return new Status { Succeeded = false, Message = "Specified admin user was not found." };

            if (adminUser.Active) return new Status { Succeeded = true, Message = "Specified admin user was already active." };

            adminUser.Active = true;

            Context.SaveChanges();

            return new Status { Succeeded = true, Message = "Admin user activated successfully." };
        }

        [OperationContract]
        public Status DeactivateAdmin(string username)
        {
            var Context = MyContext.GetContext();

            var adminUser = Context.AdminUsers.FirstOrDefault(x => x.Username.Equals(username));

            if (adminUser == null) return new Status { Succeeded = false, Message = "Specified admin user was not found." };

            if (adminUser.Active == false)
                return new Status { Succeeded = true, Message = "Specified admin user was already inactive." };

            adminUser.Active = false;

            Context.SaveChanges();

            return new Status { Succeeded = true, Message = "Admin user deactivated successfully." };
        }


        [OperationContract]
        public Status CreateTestCenter(string centerName, string address, int locationId, decimal ratePerTested,
            uint capacityPerSession)
        {
            var Context = MyContext.GetContext();

            var location = Context.Locations.Find(locationId);

            if (location == null) return new Status {Succeeded = false, Message = "Specified location is invalid"};

            var center = new Center
            {
                CenterName = centerName,
                Address = address,
                RatePerTested = ratePerTested,
                CapacityPerSession = (int) capacityPerSession,
                Active = false,
                LocationId = location.LocationId
            };


            try
            {
                Context.Centers.Add(center);

                Context.SaveChanges();

                return new Status
                {
                    Succeeded = true,
                    Message = "Center created successfully.",
                    OptionalId = center.CenterId
                };
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }

        [OperationContract]
        public Status CreatePartnerTestCenter(int partnerId, string centerName, string address, int locationId,
            decimal ratePerTested, uint capacityPerSession)
        {
            var Context = MyContext.GetContext();

            var location = Context.Locations.Find(locationId);

            if (location == null) return new Status {Succeeded = false, Message = "Specified location id is invalid"};

            var partner = Context.Partners.Find(partnerId);

            if (partner == null) return new Status {Succeeded = false, Message = "Specified partner id is invalid"};


            var center = new Center
            {
                CenterName = centerName,
                Address = address,
                RatePerTested = ratePerTested,
                CapacityPerSession = (int) capacityPerSession,
                Active = true,
                IsPrivateCenter = true,
                LocationId = location.LocationId,
                OwnerPartnerId = partner.PartnerId
            };


            try
            {
                Context.Centers.Add(center);

                Context.SaveChanges();

                return new Status
                {
                    Succeeded = true,
                    Message = "Partner Center created successfully.",
                    OptionalId = center.CenterId
                };
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }


        [OperationContract]
        public Status UpdateTestCenter(int centerId, string centerName, decimal? ratePerTested, string address,
            int locationId, int? capacityPerSession)
        {
            var Context = MyContext.GetContext();

            var location = Context.Locations.Find(locationId);

            if (location == null) return new Status {Succeeded = false, Message = "Specified location id is invalid"};

            var center = Context.Centers.Find(centerId);

            if (center == null) return new Status {Succeeded = false, Message = "Specified center id is invalid"};


            center.CenterName = centerName;
            center.Address = address;
            center.LocationId = locationId;

            if (ratePerTested.HasValue) center.RatePerTested = ratePerTested;

            if (capacityPerSession.HasValue) center.CapacityPerSession = capacityPerSession.Value;

            try
            {
                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Center updated successfully."};
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }


        [OperationContract]
        public Status UpdateTestCenterAddress(int centerId, string address)
        {
            var Context = MyContext.GetContext();

            var center = Context.Centers.Find(centerId);

            if (center == null) return new Status {Succeeded = false, Message = "Specified center id is invalid"};

            center.Address = address;

            try
            {
                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Center address updated successfully."};
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }


        [OperationContract]
        public Status UpdateTestCenterCapacity(int centerId, uint capacityPerSession)
        {
            var Context = MyContext.GetContext();

            var center = Context.Centers.Find(centerId);

            if (center == null) return new Status {Succeeded = false, Message = "Specified center id is invalid"};

            center.CapacityPerSession = (int) capacityPerSession;


            try
            {
                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Center capacity updated successfully."};
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }


        [OperationContract]
        public List<LocationModel> GetAllLocations()
        {
            var Context = MyContext.GetContext();

            var list = Context.Locations.Select(x => new LocationModel
            {
                LocationId = x.LocationId,
                LocationName = x.LocationName
            }).ToList();


            return list;
        }


        [OperationContract]
        public Status CreateLocation(string locationName)
        {
            var Context = MyContext.GetContext();


            var location = new Location {LocationName = locationName.ToUpper()};


            try
            {
                Context.Locations.Add(location);

                Context.SaveChanges();

                return new Status
                {
                    Succeeded = true,
                    Message = "Location created successfully.",
                    OptionalId = location.LocationId
                };
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }

        [OperationContract]
        public Status UpdateLocation(int locationId, string locationName)
        {
            var Context = MyContext.GetContext();

            var location = Context.Locations.Find(locationId);

            if (location == null) return new Status {Succeeded = false, Message = "Specified location is invalid"};


            try
            {
                location.LocationName = locationName.ToUpper();

                Context.SaveChanges();

                return new Status {Succeeded = true, Message = "Location updated successfully."};
            }

            catch (Exception ex)
            {
                return new Status {Succeeded = false, Message = "An error occured. Error: " + ex.Message};
            }
        }

        [OperationContract]
        public List<LocationModel> GetLocations(List<int> locationIds)
        {
            var Context = MyContext.GetContext();

            var list = Context.Locations.Where(x => locationIds.Contains(x.LocationId))
                .Select(x => new LocationModel
                {
                    LocationId = x.LocationId,
                    LocationName = x.LocationName
                }).ToList();


            return list;
        }


        [OperationContract]
        public List<PartnerAdminModel> GetPartnerAdmins(string searchTerm, int startRow, int maxRows)
        {
            var Context = MyContext.GetContext();

            var list = new List<PartnerAdminModel>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                list = Context.AdminUsers.Where(x => x.IsPartnerAdmin)
                    .Select(x => new PartnerAdminModel
                    {
                        AdminId = x.AdminId,
                        PartnerId = x.PartnerId,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname,
                        Email = x.Username,
                        Mobile = x.MobileNo,
                        PartnerName = x.Partner.PartnerName,
                        Active = x.Active
                    }).OrderBy(x => x.PartnerName)
                    .Skip(startRow)
                    .Take(maxRows)
                    .ToList();
            }
            else
            {
                list =
                    Context.AdminUsers.Where(
                        x =>
                            (x.Username.Equals(searchTerm) || x.Firstname.Equals(searchTerm) ||
                             x.Lastname.Equals(searchTerm) || x.MobileNo.Equals(searchTerm)) && x.IsPartnerAdmin)
                        .Select(x => new PartnerAdminModel
                        {
                            AdminId = x.AdminId,
                            PartnerId = x.PartnerId,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            Email = x.Username,
                            Mobile = x.MobileNo,
                            PartnerName = x.Partner.PartnerName,
                            Active = x.Active
                        }).OrderBy(x => x.PartnerName)
                        .Skip(startRow)
                        .Take(maxRows)
                        .ToList();
            }


            return list;
        }

        [OperationContract]
        public int CountPartnerAdmins(string searchTerm)
        {
            var Context = MyContext.GetContext();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Context.AdminUsers.Count(x => x.IsPartnerAdmin);
            }
            else
            {
                return
                    Context.AdminUsers.Count(
                        x =>
                            (x.Username.Equals(searchTerm) || x.Firstname.Equals(searchTerm) ||
                             x.Lastname.Equals(searchTerm) || x.MobileNo.Equals(searchTerm)) && x.IsPartnerAdmin);
            }
        }

        [OperationContract]
        public List<PartnerModel> GetAllPartners(string searchTerm, int startRow, int maxRows)
        {
            var Context = MyContext.GetContext();

            var list = new List<PartnerModel>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                list = Context.Partners
                    .Select(x => new PartnerModel
                    {
                        PartnerId = x.PartnerId,
                        PartnerName = x.PartnerName,
                        Address = x.Address,
                        DateCreated = x.DateCreated
                    }).OrderBy(x => x.PartnerName)
                    .Skip(startRow)
                    .Take(maxRows)
                    .ToList();
            }
            else
            {
                list = Context.Partners.Where(x => x.PartnerName.Contains(searchTerm) || x.Address.Contains(searchTerm))
                    .Select(x => new PartnerModel
                    {
                        PartnerId = x.PartnerId,
                        PartnerName = x.PartnerName,
                        Address = x.Address,
                        DateCreated = x.DateCreated
                    }).OrderBy(x => x.PartnerName)
                    .Skip(startRow)
                    .Take(maxRows)
                    .ToList();
            }


            return list;
        }

        [OperationContract]
        public int CountAllPartners(string searchTerm)
        {
            var Context = MyContext.GetContext();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Context.Partners.Count();
            }
            else
            {
                return Context.Partners.Count(x => x.PartnerName.Contains(searchTerm) || x.Address.Contains(searchTerm));
            }
        }


        [OperationContract]
        public List<CenterModel> GetAllDragnetCenters(string searchTerm, int startRow, int maxRows)
        {
            var Context = MyContext.GetContext();

            var list = new List<CenterModel>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                list = Context.Centers.Where(x => x.IsPrivateCenter == false)
                    .Select(x => new CenterModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        Address = x.Address,
                        LocationId = x.LocationId,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        RatePerTested = x.RatePerTested,
                        Active = x.Active
                    })
                    .OrderBy(x => x.CenterName)
                    .Skip(startRow)
                    .Take(maxRows)
                    .ToList();
            }
            else
            {
                list =
                    Context.Centers.Where(
                        x =>
                            x.IsPrivateCenter == false &&
                            (x.CenterName.Contains(searchTerm) || x.Address.Contains(searchTerm) ||
                             x.Location.LocationName.Contains(searchTerm)))
                        .Select(x => new CenterModel
                        {
                            CenterId = x.CenterId,
                            CenterName = x.CenterName,
                            Address = x.Address,
                            LocationId = x.LocationId,
                            LocationName = x.Location.LocationName,
                            CapacityPerSession = x.CapacityPerSession,
                            RatePerTested = x.RatePerTested
                        })
                        .OrderBy(x => x.CenterName)
                        .Skip(startRow)
                        .Take(maxRows)
                        .ToList();
            }


            return list;
        }

        [OperationContract]
        public int CountAllDragnetCenters(string searchTerm)
        {
            var Context = MyContext.GetContext();


            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Context.Centers.Count(x => x.IsPrivateCenter == false);
            }
            else
            {
                return
                    Context.Centers.Count(
                        x =>
                            x.IsPrivateCenter == false &&
                            (x.CenterName.Contains(searchTerm) || x.Address.Contains(searchTerm) ||
                             x.Location.LocationName.Contains(searchTerm)));
            }
        }

        [OperationContract]
        public CenterModel GetCenter(int centerId)
        {
            var Context = MyContext.GetContext();

            var item = Context.Centers.Where(x => x.CenterId == centerId).Include(x => x.Location).FirstOrDefault();


            if (item == null) return null;

            var center = new CenterModel
            {
                CenterId = item.CenterId,
                Address = item.Address,
                CenterName = item.CenterName,
                LocationId = item.LocationId,
                LocationName = item.Location.LocationName,
                CapacityPerSession = item.CapacityPerSession,
                RatePerTested = item.RatePerTested,
                Active = item.Active
            };

            return center;
        }

        [OperationContract]
        public List<CenterModel> GetActiveDragnetCentersInLocation(int locationId)
        {
              var Context = MyContext.GetContext();

              var  list = Context.Centers.Where(x => x.Active && x.IsPrivateCenter == false && x.LocationId == locationId)
                    .Select(x => new CenterModel
                    {
                        CenterId = x.CenterId,
                        CenterName = x.CenterName,
                        Address = x.Address,
                        LocationId = x.LocationId,
                        LocationName = x.Location.LocationName,
                        CapacityPerSession = x.CapacityPerSession,
                        RatePerTested = x.RatePerTested,
                        Active = x.Active
                    })
                    .OrderBy(x => x.CenterName)
                    .ToList();

            return list;
        }


        [OperationContract]
        public PartnerModel GetPartner(int partnerId)
        {
            var Context = MyContext.GetContext();

            var item = Context.Partners.Where(x => x.PartnerId == partnerId)
                .Select(x => new PartnerModel
                {
                    PartnerId = x.PartnerId,
                    PartnerName = x.PartnerName,
                    Address = x.Address,
                    DateCreated = x.DateCreated
                })
                .FirstOrDefault();

            return item;
        }


        [OperationContract]
        public Status CreateTestCenterAndAdmin(string centerName, decimal ratePerTested, uint capacityPerSession, string address, int locationId, string adminEmail, string mobileNumber,
            string firstName,
            string lastName, string password)
        {
            var Context = MyContext.GetContext();

            if (new AdminUserService().UsernameExists(adminEmail))
                return new Status { Succeeded = false, Message = "Specified admin username has already been used." };

            var location = Context.Locations.Find(locationId);

            if (location == null) return new Status { Succeeded = false, Message = "Specified location id is invalid" };



            var emailIsValid = Regex.IsMatch(adminEmail, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (!emailIsValid) return new Status { Succeeded = false, Message = "Specified email is invalid." };

            try
            {
                var center = new Center
                {
                    CenterName = centerName,
                    IsPrivateCenter = false,
                    Address = address,
                    LocationId = locationId,
                    RatePerTested = ratePerTested,
                    CapacityPerSession = (int)capacityPerSession,
                    Active = false
                    
                };

                var admin = new AdminUser
                {
                    Username = adminEmail,
                    Firstname = firstName,
                    Lastname = lastName,
                    MobileNo = mobileNumber,
                    Active = true
                };

                admin.Password = FotSecurity<string>.Hash(password);

                admin.Center = center;
                admin.IsCenterAdmin = true;

                center.AdminUsers.Add(admin);

                Context.Centers.Add(center);

                Context.SaveChanges();

                return new Status
                {
                    Succeeded = true,
                    Message = "Center and admin created successfully.",
                    OptionalId = center.CenterId
                };
            }

            catch (Exception ex)
            {
                return new Status { Succeeded = false, Message = "An error occured. Error: " + ex.Message };
            }
        }


        [OperationContract]
        public Status CreateTestCenterAdmin(int centerId, string adminEmail, string mobileNumber, string firstName, string lastName, string password)
        {
            var Context = MyContext.GetContext();

            if (new AdminUserService().UsernameExists(adminEmail))
                return new Status { Succeeded = false, Message = "Specified admin username has already been used." };

            var center = Context.Centers.Find(centerId);

            if (center == null) return new Status { Succeeded = false, Message = "Specified center id is invalid" };



            var emailIsValid = Regex.IsMatch(adminEmail, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (!emailIsValid) return new Status { Succeeded = false, Message = "Specified email is invalid." };

            try
            {

                var admin = new AdminUser
                {
                    Username = adminEmail,
                    Firstname = firstName,
                    Lastname = lastName,
                    MobileNo = mobileNumber,
                    Active = true
                };

                admin.Password = FotSecurity<string>.Hash(password);

                admin.Center = center;
                admin.IsCenterAdmin = true;

                Context.AdminUsers.Add(admin);

                Context.SaveChanges();

                return new Status
                {
                    Succeeded = true,
                    Message = "Test center admin created successfully.",
                    OptionalId = center.CenterId
                };
            }

            catch (Exception ex)
            {
                return new Status { Succeeded = false, Message = "An error occured. Error: " + ex.Message };
            }
        }



        [OperationContract]
        public List<CenterAdminModel> GetDragnetCenterAdmins(string searchTerm, int startRow, int maxRows)
        {
            var Context = MyContext.GetContext();

            var list = new List<CenterAdminModel>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                list = Context.AdminUsers.Where(x => x.IsCenterAdmin && x.Center.IsPrivateCenter == false)
                    .Select(x => new CenterAdminModel
                    {
                        AdminId = x.AdminId,
                        CenterId = x.CenterId,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname,
                        Email = x.Username,
                        Mobile = x.MobileNo,
                        CenterName = x.Center.CenterName,
                        CenterLocation = x.Center.Location.LocationName,
                        Active = x.Active
                    }).OrderBy(x => x.CenterName)
                    .Skip(startRow)
                    .Take(maxRows)
                    .ToList();
            }
            else
            {
                list =
                    Context.AdminUsers.Where(
                        x =>
                            (x.Username.Equals(searchTerm) || x.Firstname.Equals(searchTerm) ||
                             x.Lastname.Equals(searchTerm) || x.MobileNo.Equals(searchTerm)) && x.IsCenterAdmin && x.Center.IsPrivateCenter == false)
                     .Select(x => new CenterAdminModel
                     {
                         AdminId = x.AdminId,
                         CenterId = x.CenterId,
                         Firstname = x.Firstname,
                         Lastname = x.Lastname,
                         Email = x.Username,
                         Mobile = x.MobileNo,
                         CenterName = x.Center.CenterName,
                         CenterLocation = x.Center.Location.LocationName,
                         Active = x.Active
                     }).OrderBy(x => x.CenterName)
                        .Skip(startRow)
                        .Take(maxRows)
                        .ToList();
            }


            return list;
        }

        [OperationContract]
        public int CountDragnetCenterAdmins(string searchTerm)
        {
            var Context = MyContext.GetContext();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Context.AdminUsers.Count(x => x.IsCenterAdmin && x.Center.IsPrivateCenter == false);
            }
            else
            {
                return
                    Context.AdminUsers.Count(
                        x =>
                            (x.Username.Equals(searchTerm) || x.Firstname.Equals(searchTerm) ||
                             x.Lastname.Equals(searchTerm) || x.MobileNo.Equals(searchTerm)) && x.IsCenterAdmin && x.Center.IsPrivateCenter == false);
            }
        }


        [OperationContract]
        public List<CenterAdminModel> GetPartnerCenterAdmins(int partnerId, string searchTerm, int startRow, int maxRows)
        {
            var Context = MyContext.GetContext();

            var list = new List<CenterAdminModel>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                list = Context.AdminUsers.Where(x => x.IsCenterAdmin && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == partnerId)
                    .Select(x => new CenterAdminModel
                    {
                        AdminId = x.AdminId,
                        CenterId = x.CenterId,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname,
                        Email = x.Username,
                        Mobile = x.MobileNo,
                        CenterName = x.Center.CenterName,
                        CenterLocation = x.Center.Location.LocationName,
                        Active = x.Active
                    }).OrderBy(x => x.CenterName)
                    .Skip(startRow)
                    .Take(maxRows)
                    .ToList();
            }
            else
            {
                list =
                    Context.AdminUsers.Where(
                        x =>
                            (x.Username.Equals(searchTerm) || x.Firstname.Equals(searchTerm) ||
                             x.Lastname.Equals(searchTerm) || x.MobileNo.Equals(searchTerm)) && x.IsCenterAdmin && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == partnerId)
                     .Select(x => new CenterAdminModel
                     {
                         AdminId = x.AdminId,
                         CenterId = x.CenterId,
                         Firstname = x.Firstname,
                         Lastname = x.Lastname,
                         Email = x.Username,
                         Mobile = x.MobileNo,
                         CenterName = x.Center.CenterName,
                         CenterLocation = x.Center.Location.LocationName,
                         Active = x.Active
                     }).OrderBy(x => x.CenterName)
                        .Skip(startRow)
                        .Take(maxRows)
                        .ToList();
            }


            return list;
        }

        [OperationContract]
        public int CountPartnerCenterAdmins(int partnerId, string searchTerm)
        {
            var Context = MyContext.GetContext();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Context.AdminUsers.Count(x => x.IsCenterAdmin && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == partnerId);
            }
            else
            {
                return
                    Context.AdminUsers.Count(
                        x =>
                            (x.Username.Equals(searchTerm) || x.Firstname.Equals(searchTerm) ||
                             x.Lastname.Equals(searchTerm) || x.MobileNo.Equals(searchTerm)) && x.IsCenterAdmin && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == partnerId);
            }
        }



        [OperationContract]
        public CenterAdminModel GetTestCenterAdmin(string email)
        {
            var Context = MyContext.GetContext();

            var item = Context.AdminUsers.Where(x => x.Username.Equals(email) && x.IsCenterAdmin)
                .Select(x => new CenterAdminModel
                {
                    AdminId = x.AdminId,
                    CenterId = x.CenterId,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Email = x.Username,
                    Mobile = x.MobileNo,
                    CenterName = x.Center.CenterName,
                    CenterLocation = x.Center.Location.LocationName,
                    Active = x.Active
                })
                .FirstOrDefault();

            return item;
        }


        [OperationContract]
        public bool PartnerLogin(string email, string password)
        {
            return new AdminUserService().LoginIsValid(email, password);
        }


        [OperationContract]
        public bool DragnetTCALogin(string email, string password)
        {
            return new AdminUserService().LoginIsValid(email, password);
        }


        [OperationContract]
        public SessionExportStatus CreateTestSessions(int centerId, DateTime testDate, List<SessionTime> sessionList)
        {
            var service = new TestSessionService();

            int succeeded = 0;
            int failed = 0;
            var IssuesList = new List<Status>();
            foreach (var sessionTime in sessionList)
            {
                var session = new TestSession
                {
                    CenterId = centerId,
                    TestDate = testDate,
                    TimeIndex = sessionTime.Index,
                    TimeText = sessionTime.TimeText
                };

                var ret = service.Add(session);

                if (ret.IsDone)
                {
                    succeeded++;
                }
                else
                {
                    failed++;
                    IssuesList.Add(new Status{Succeeded = false, Message = ret.Message, OptionalId = sessionTime.Index});
                }
            }

            return new SessionExportStatus{Succeeded = succeeded, Failed = failed, IssueList = IssuesList};
        }


        [OperationContract]
        public CandidateExportStatus ExportCandidates(int campaignId, List<CandidateModel> candidates)
        {

            var campaign = new CampaignService().GetCampaign(campaignId);


            if(campaign == null) return new CandidateExportStatus{ExportStatus = new Status{Succeeded = false, Message = "Invalid campaign id."}};


            var candidateService = new CandidateService();
            var entryService = new CampaignEntryService();
            

            int totalSucceeded = 0;
            int errorCount = 0;

            var issuesList = new List<UploadTempViewModel>();

            var locationList = new LocationService().GetLocations(-1, -1);


            var context = ContextManager.AsSingleton<FotContext>();

            context.Configuration.ValidateOnSaveEnabled = false;

            int rowIndex = 0;

            foreach(var can in candidates)
            {

                rowIndex++;

                var candidate = new Candidate();

                try
                {


                    candidate.ClientUniqueID = string.IsNullOrWhiteSpace(can.ClientUniqueId) ? null : can.ClientUniqueId;
                    candidate.Email = can.Email;
                    candidate.Password = can.Password;
                    candidate.FirstName = can.Firstname;
                    candidate.LastName = can.Lastname;
                    candidate.MobileNo = can.MobileNo;

                    var location = can.Location.ToUpper();

                    candidate.LocationId = locationList.Any(x => x.LocationName.Equals(location))
                                               ? locationList.First(x => x.LocationName.Equals(location)).LocationId
                                               : default(int?);
                    candidate.DateAdded = DateTime.Today;

                    #region Validation Region

                    var match = Regex.IsMatch(candidate.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                    if (!candidate.LocationId.HasValue)
                    {
                        issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = location, Issue = "Invalid location" });
                        errorCount++;
                        continue;
                    }


                    if (!match)
                    {

                        issuesList.Add(new UploadTempViewModel
                        {
                            RowNumber = rowIndex,
                            ClientUniqueId = candidate.ClientUniqueID,
                            Email = candidate.Email,
                            Firstname = candidate.FirstName,
                            Lastname = candidate.LastName,
                            MobileNo = candidate.MobileNo,
                            Password = candidate.Password,
                            Location = location,
                            Issue = "Invalid Email"
                        });
                        errorCount++;
                        continue;
                    }

                    var mobileNumberValid = Regex.IsMatch(candidate.MobileNo, @"^\d{11}$");


                    if (!mobileNumberValid)
                    {
                        issuesList.Add(new UploadTempViewModel
                        {
                            RowNumber = rowIndex,
                            ClientUniqueId = candidate.ClientUniqueID,
                            Email = candidate.Email,
                            Firstname = candidate.FirstName,
                            Lastname = candidate.LastName,
                            MobileNo = candidate.MobileNo,
                            Password = candidate.Password,
                            Location = location,
                            Issue = "Invalid Mobile Number. Mobile number should be 11 digits (GSM format)"
                        });
                        errorCount++;
                        continue;
                    }
                    #endregion


                    var app = candidateService.Add(candidate);

                    if (app.IsDone)
                    {
                        var entry = new CampaignEntry
                        {
                            CampaignId = campaignId,
                            CandidateId = (int)app.Data
                        };

                        if (campaign.IsUnproctored)
                        {
                            entry.CandidateAssessment = new CandidateAssessment
                            {
                                CandidateGuid = Guid.NewGuid().ToString().Replace("-", "")
                            };
                        }


                        var tempApp = entryService.Add(entry);

                        if (tempApp.IsDone)
                        {
                            totalSucceeded++;
                        }
                        else
                        {
                            issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = location, Issue = tempApp.Message });
                            errorCount++;
                        }


                    }
                    else
                    {
                        issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = location, Issue = app.Message });
                        errorCount++;
                    }

                }
                catch (Exception ex)
                {
                    issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = can.Location.ToUpper(), Issue = "An error occured. " + ex.Message });
                    errorCount++;
                }
            }

           


            int rows = candidateService.Context.Database.ExecuteSqlCommand(
                   "update Candidate set Username =  'DRG' + REPLACE(STR(CandidateId, 7), SPACE(1), '0') where Username is null");

            context.Configuration.ValidateOnSaveEnabled = true;
            



            return new CandidateExportStatus{ExportStatus = new Status{Succeeded = true, Message = ""}, TotalSucceeded = totalSucceeded, TotalFailed = errorCount, IssueList = issuesList};
        }



        [OperationContract]
        public List<PartnerModel> GetAllPartnersNonPaged()
        {
            var Context = MyContext.GetContext();

            var list = new List<PartnerModel>();

                list = Context.Partners
                    .Select(x => new PartnerModel
                    {
                        PartnerId = x.PartnerId,
                        PartnerName = x.PartnerName,
                        Address = x.Address,
                        DateCreated = x.DateCreated
                    }).OrderBy(x => x.PartnerName)
                    .ToList();
       


            return list;
        }

        [OperationContract]
        public List<CenterAdminModel> GetTestCenterAdmins(int centerId)
        {
            var Context = MyContext.GetContext();

            var list = new List<CenterAdminModel>();

         
                list = Context.AdminUsers.Where(x => x.IsCenterAdmin && x.CenterId == centerId)
                    .Select(x => new CenterAdminModel
                    {
                        AdminId = x.AdminId,
                        CenterId = x.CenterId,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname,
                        Email = x.Username,
                        Mobile = x.MobileNo,
                        CenterName = x.Center.CenterName,
                        CenterLocation = x.Center.Location.LocationName,
                        Active = x.Active
                    }).OrderBy(x => x.CenterName)
                    .ToList();
            
          


            return list;
        }


        [OperationContract]
        public List<TotalTestedByCenterModel> GetTotalTestedInCentersByCampaignId(int campaignId)
        {
            var Context = MyContext.GetContext();

            var list =
                        Context.Centers.Where(
                            x => x.TestSessions.Any(t => t.CampaignEntries.Any(z => z.CampaignId == campaignId))).
                            Select(x => new TotalTestedByCenterModel
                            {
                                CenterId = x.CenterId,
                                CenterName = x.CenterName,
                                LocationId = x.LocationId,
                                LocationName = x.Location.LocationName,
                                TotalTested = x.TestSessions.Sum(y => y.CampaignEntries.Count(q => q.CampaignId == campaignId && q.Tested))
                            }).ToList();

            return list;
        }
    }
}