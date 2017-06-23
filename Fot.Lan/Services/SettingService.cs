using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.DTO;

namespace Fot.Lan.Services
{
    public class SettingService : ServiceBase
    {

        public bool GetImageCapatureSetting()
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("IMAGE_CAPTURE"));

            if (item != null)
            {
                return Convert.ToBoolean(item.SettingValue);
            }
            else
            {
                return false;
            }
        }

        public void SetImageCaptureSetting(bool flag)
        {
            var item = Context.Settings.FirstOrDefault(x => x.SettingName.Equals("IMAGE_CAPTURE"));

            if (item != null)
            {
                item.SettingValue = flag.ToString();

                Context.SaveChanges();

            }
        }


        public bool LoginIsValid(string username, string password)
        {
            var item = Context.AdminUsers.FirstOrDefault(x => x.Username.Equals(username) && x.IsCaptureAdmin);
           
            if(item != null)
            {
                return item.Password.Equals(FotSecurity<string>.Hash(password));
            }
            else
            {
                return false;
            }
        }

    }
}