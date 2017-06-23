using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.DTO;

namespace Fot.Admin.Services
{
    public class AdminUserService : ServiceBase
    {
        public IQueryable<AdminUser> Users
        {
            get { return Context.AdminUsers.Where(x => x.IsGlobalAdmin == false); }
        }

        public List<AdminUser> GetCaptureAdmins(int CenterId)
        {
            return Context.AdminUsers.Where(x => x.IsCaptureAdmin && x.CenterId == CenterId).ToList();
        }


        public List<AdminUser> GetAllAdmins()
        {
            return Context.AdminUsers.ToList();
        }


        public bool LoginIsValid(string username, string password)
        {
            AdminUser item = Context.AdminUsers.FirstOrDefault(x => x.Username.Equals(username));

            var hashedPassword = FotSecurity<string>.Hash(password);


            if (item != null)
            {
                if (item.Password.Equals(hashedPassword))
                {

                    if (item.Active)
                    {
                        item.LastLoginDate = DateTime.Now;
                        item.FailedLoginAttempts = 0;
                        Context.SaveChanges();

                        new AccessLogService().LogEntry(new AccessLog { AdminId = item.AdminId, LogEntryType = "User Logged In", LogEntryDetails = "User Logged In", LogDate = DateTime.Now, IpAddress = HttpContext.Current.Request.UserHostAddress, UserAgent = HttpContext.Current.Request.UserAgent });
                    }
                    return true;
                }
                else
                {
                    item.FailedLoginAttempts = item.FailedLoginAttempts + 1;

                    if (item.FailedLoginAttempts == 3)
                    {
                        item.Active = false;
                    }

                    Context.SaveChanges();

                    new AccessLogService().LogEntry(new AccessLog { AdminId = item.AdminId, LogEntryType = "User Login Attempt", LogEntryDetails = "Failed Login attempt. Wrong password supplied.", LogDate = DateTime.Now, IpAddress = HttpContext.Current.Request.UserHostAddress, UserAgent = HttpContext.Current.Request.UserAgent });

                    return false;
                }
            }
            else
            {
                return false;
            }
        }




        public List<AdminUser> GetAdminUsers(int startRow, int maxRows)
        {

            IEnumerable<AdminUser> query = Users.Where(x => x.IsCenterAdmin == false && x.IsPartnerAdmin == false).OrderBy(x => x.AdminId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int Count()
        {
            return Users.Count(x => x.IsCenterAdmin == false && x.IsPartnerAdmin == false);
        }




        public List<PartnerAdminViewModel> GetPartnerAdmins(int startRow, int maxRows)
        {

            IEnumerable<PartnerAdminViewModel> query = Users.Where(x => x.IsPartnerAdmin && x.HasUsersAccess).OrderBy(x => x.AdminId).Select(
                 x => new PartnerAdminViewModel
                     {
                         AdminId = x.AdminId,
                         Username = x.Username,
                         Password = x.Password,
                         Firstname = x.Firstname,
                         Lastname = x.Lastname,
                         MobileNo = x.MobileNo,
                         Active = x.Active,
                         LastLoginDate = x.LastLoginDate,
                         PartnerName = x.Partner.PartnerName
                     });


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int CountPartnerAdmins()
        {
            return Users.Count(x => x.IsPartnerAdmin && x.HasUsersAccess);
        }

        public List<PartnerAdminViewModel> GetPartnerAdminUsers(int PartnerId, int startRow, int maxRows)
        {

            IEnumerable<PartnerAdminViewModel> query = Users.Where(x => x.IsPartnerAdmin && x.PartnerId == PartnerId && x.HasUsersAccess == false).OrderBy(x => x.AdminId).Select(
                 x => new PartnerAdminViewModel
                 {
                     AdminId = x.AdminId,
                     Username = x.Username,
                     Password = x.Password,
                     Firstname = x.Firstname,
                     Lastname = x.Lastname,
                     MobileNo = x.MobileNo,
                     Active = x.Active,
                     LastLoginDate = x.LastLoginDate,
                     PartnerName = x.Partner.PartnerName
                 });


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int CountPartnerAdminUsers(int PartnerId)
        {
            return Users.Count(x => x.IsPartnerAdmin && x.PartnerId == PartnerId && x.HasUsersAccess == false);
        }




        public List<CenterAdminViewModel> GetCenterAdmins(int startRow, int maxRows)
        {

            IEnumerable<CenterAdminViewModel> query = Users.Where(x => x.IsCenterAdmin && x.Center.IsPrivateCenter == false).OrderBy(x => x.AdminId).Select(
                    x => new CenterAdminViewModel
                        {
                            AdminId = x.AdminId,
                            Username = x.Username,
                            Password = x.Password,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            MobileNo = x.MobileNo,
                            Active = x.Active,
                            LastLoginDate = x.LastLoginDate,
                            CenterName = x.Center.CenterName
                        });

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int CountCenterAdmins()
        {
            return Users.Count(x => x.IsCenterAdmin && x.Center.IsPrivateCenter == false);
        }


        public List<CenterAdminViewModel> GetCenterAdminsForPartner(int startRow, int maxRows)
        {
            var currentAdmin = GetCurrentAdmin();

            return
                Users.Where(x => x.IsCenterAdmin && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == currentAdmin.PartnerId).OrderBy(x => x.AdminId).Skip(startRow).Take(maxRows).Select(
                    x => new CenterAdminViewModel
                    {
                        AdminId = x.AdminId,
                        Username = x.Username,
                        Password = x.Password,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname,
                        MobileNo = x.MobileNo,
                        Active = x.Active,
                        LastLoginDate = x.LastLoginDate,
                        CenterName = x.Center.CenterName
                    }).ToList();
        }

        public int CountCenterAdminsForPartner()
        {
            var currentAdmin = GetCurrentAdmin();

            return Users.Count(x => x.IsCenterAdmin && x.Center.IsPrivateCenter && x.Center.OwnerPartnerId == currentAdmin.PartnerId);
        }


        public AdminUser GetAdminUser(int id)
        {
            return Context.AdminUsers.Find(id);
        }

        public AdminUser GetAdminUserByName(string username)
        {
            return Context.AdminUsers.FirstOrDefault(x => x.Username.Equals(username));
        }

        public bool UsernameExists(string username)
        {
            return Context.AdminUsers.Any(x => x.Username.Equals(username));
        }

        public AppMessage AddAdminUser(AdminUser item)
        {
            if (UsernameExists(item.Username))
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "Specified Email already exists. Please choose a unique email.",
                        Status = MessageStatus.Error
                    };
            }

            try
            {
                item.Password = FotSecurity<string>.Hash(item.Password);
                Context.AdminUsers.Add(item);
                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Added user successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public AppMessage AddPartnerAdminUser(AdminUser item)
        {
            if (UsernameExists(item.Username))
            {
                return new AppMessage
                {
                    IsDone = false,
                    Message = "Specified Email already exists. Please choose a unique email.",
                    Status = MessageStatus.Error
                };
            }

            if (PartnerAlreadyHasAnAdmin(item.PartnerId.Value))
            {
                return new AppMessage
                {
                    IsDone = false,
                    Message = "Specified partner already has an admin. You can only add one admin per partner. This admin is responsible for creating other users.",
                    Status = MessageStatus.Error
                };
            }


            try
            {
                item.Password = FotSecurity<string>.Hash(item.Password);
                Context.AdminUsers.Add(item);
                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Added user successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        private bool PartnerAlreadyHasAnAdmin(int PartnerId)
        {
            return Context.AdminUsers.Any(x => x.PartnerId == PartnerId);
        }

        public AppMessage UpdateAdminUser(AdminUser item)
        {
            try
            {




                Context.Entry(item).State = EntityState.Modified;

                if (!string.IsNullOrWhiteSpace(item.Password))
                {

                    item.Password = FotSecurity<string>.Hash(item.Password);
                    Context.Entry(item).Property(x => x.Password).IsModified = true;
                }
                else
                {
                    Context.Entry(item).Property(x => x.Password).IsModified = false;
                }


                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Updated user successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public void DeleteAdminUser(int AdminId)
        {
            AdminUser item = Context.AdminUsers.Find(AdminId);

            if (item != null)
            {
                Context.AdminUsers.Remove(item);
                Context.SaveChanges();
            }
        }

        public AppMessage ChangePassword(string username, string oldpassword, string newpassword)
        {
            AdminUser user = GetCurrentAdmin();

            var realAdmin = Context.AdminUsers.FirstOrDefault(x => x.AdminId == user.AdminId);

            var oldHashedPassword = FotSecurity<string>.Hash(oldpassword);


            if (realAdmin.Password.Equals(oldHashedPassword))
            {
                realAdmin.Password = FotSecurity<string>.Hash(newpassword);

                realAdmin.LastPasswordChangedDate = DateTime.Today;

                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Password changed successfully.", Status = MessageStatus.Success };
            }
            else
            {
                return new AppMessage { IsDone = true, Message = "Invalid old password", Status = MessageStatus.Error };
            }
        }

        public AppMessage ChangePassword(string username, string newpassword)
        {
            AdminUser user = GetAdminUserByName(username);

            var realAdmin = Context.AdminUsers.FirstOrDefault(x => x.AdminId == user.AdminId);


            realAdmin.Password = FotSecurity<string>.Hash(newpassword);

            realAdmin.LastPasswordChangedDate = DateTime.Today;

            Context.SaveChanges();

            return new AppMessage { IsDone = true, Message = "Password changed successfully.", Status = MessageStatus.Success };

        }

        public AdminUser GetCurrentAdmin()
        {

            return (HttpContext.Current.Session["CURRENT_ADMIN"] as AdminUser) ?? ((HttpContext.Current.Session["CURRENT_ADMIN"] = GetAdminUserByName(HttpContext.Current.User.Identity.Name)) as AdminUser);
        }
    }
}