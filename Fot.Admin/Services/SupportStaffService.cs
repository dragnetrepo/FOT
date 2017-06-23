using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.DTO;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using ExcelLibrary.SpreadSheet;
using Mindscape.Raygun4Net;

namespace Fot.Admin.Services
{
    public class SupportStaffService : ServiceBase
    {
        public List<CenterUser> GetAllStaff(int CenterId)
        {
            return Context.CenterUsers.Where(x => x.CenterId == CenterId).ToList();
        }



        public void DeleteStaff(int UserId)
        {
            CenterUser item = Context.CenterUsers.Find(UserId);

            if (item != null)
            {
                Context.CenterUsers.Remove(item);
                Context.SaveChanges();
            }
        }

        public bool EmailExists(string email)
        {
            return Context.CenterUsers.Any(x => x.Email.Equals(email));
        }


        public CenterUser GetStaff(int id)
        {
            return Context.CenterUsers.Find(id);
        }

        public AppMessage AddStaff(CenterUser item)
        {
            if (EmailExists(item.Email))
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
               
                Context.CenterUsers.Add(item);
                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Added user successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }

        public AppMessage UpdateStaff(CenterUser item)
        {
            try
            {

                Context.Entry(item).State = EntityState.Modified;


                Context.SaveChanges();

                return new AppMessage { IsDone = true, Message = "Updated user successfully.", Status = MessageStatus.Success };
            }

            catch (Exception)
            {
                return new AppMessage { IsDone = false, Message = "An error occured.", Status = MessageStatus.Error };
            }
        }




        public bool UpdateStaffPhoto(List<PersonnelPhotoUpdateModel> staffList)
        {

            try
            {

                foreach (var staff in staffList)
                {

                    if (staff.IsSupportStaff)
                    {

                        var temp = Context.CenterUsers.FirstOrDefault(x => x.UserId == staff.UserId);

                        if(temp == null) continue;

                        var item =
                            Context.TestDayPhotoes.FirstOrDefault(
                                x => x.CenterUserId == staff.UserId && x.TestDate == staff.TestDate);

                        if (item != null) continue;

                        var entry = new TestDayPhoto();

                        entry.CenterUserId = staff.UserId;
                        entry.TestDate = staff.TestDate;

                        var photoFolder = HttpContext.Current.Server.MapPath(UrlMapper.RootStaffPhotosDirectory);

                        var preFileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + ".jpg";
                        var postFileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + ".jpg";

                        using (var preImage = System.Drawing.Image.FromStream(new MemoryStream(staff.PreTestPhoto)))
                        {
                            preImage.Save(Path.Combine(photoFolder, preFileName), ImageFormat.Jpeg);
                        }


                        using (var postImage = System.Drawing.Image.FromStream(new MemoryStream(staff.PostTestPhoto)))
                        {
                            postImage.Save(Path.Combine(photoFolder, postFileName), ImageFormat.Jpeg);
                        }




                        entry.PostTestPhotoFileName = postFileName;
                        entry.PreTestPhotoFileName = preFileName;

                        entry.PreTestCapturedByAdminId = staff.PreTestCapturedByAdminId;
                        entry.PostTestCapturedByAdminId = staff.PostTestCapturedByAdminId;


                        Context.TestDayPhotoes.Add(entry);



                    }
                    else
                    {
                        var temp = Context.AdminUsers.FirstOrDefault(x => x.AdminId == staff.UserId);

                        if (temp == null) continue;

                        var item =
                            Context.TestDayPhotoes.FirstOrDefault(
                                x => x.AdminId == staff.UserId && x.TestDate == staff.TestDate);

                        if (item != null) continue;

                        var entry = new TestDayPhoto();

                        entry.AdminId = staff.UserId;
                        entry.TestDate = staff.TestDate;

                        var photoFolder = HttpContext.Current.Server.MapPath(UrlMapper.RootStaffPhotosDirectory);

                        var preFileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + ".jpg";
                        var postFileName = System.Guid.NewGuid().ToString().Replace("-", string.Empty) + ".jpg";

                        using (var preImage = System.Drawing.Image.FromStream(new MemoryStream(staff.PreTestPhoto)))
                        {
                            preImage.Save(Path.Combine(photoFolder, preFileName), ImageFormat.Jpeg);
                        }


                        using (var postImage = System.Drawing.Image.FromStream(new MemoryStream(staff.PostTestPhoto)))
                        {
                            postImage.Save(Path.Combine(photoFolder, postFileName), ImageFormat.Jpeg);
                        }


                        entry.PostTestPhotoFileName = postFileName;
                        entry.PreTestPhotoFileName = preFileName;

                        entry.PreTestCapturedByAdminId = staff.PreTestCapturedByAdminId;
                        entry.PostTestCapturedByAdminId = staff.PostTestCapturedByAdminId;


                        Context.TestDayPhotoes.Add(entry);


                    }

                }

                Context.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                new RaygunClient().Send(ex);

                return false;
            }
        }
        
            
      
       
    }
}