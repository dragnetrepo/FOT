using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Fot.Admin.Models;
using Fot.Admin.Services;


namespace Fot.Admin.Infrastructure
{
    public class AdminRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        { 
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {

            var admin = new AdminUserService().GetAdminUserByName(username);

            var list = new List<string>();

            if (admin.IsGlobalAdmin) list.Add(RoleModel.Admin);
            if (admin.CanAuthor || admin.IsGlobalAdmin)list.Add(RoleModel.Author);
            if (admin.CanSchedule || admin.IsGlobalAdmin) list.Add(RoleModel.Schedule);
            if (admin.HasResultsAccess || admin.IsGlobalAdmin) list.Add(RoleModel.Result);
            if (admin.HasFinancialsAccess || admin.IsGlobalAdmin) list.Add(RoleModel.Finance);
            if (admin.HasUsersAccess || admin.IsGlobalAdmin) list.Add(RoleModel.Users);
            if (admin.HasPartnerUsersAccess || admin.IsGlobalAdmin) list.Add(RoleModel.PartnerUsers);
            if (admin.HasCenterUsersAccess || admin.IsGlobalAdmin) list.Add(RoleModel.CenterUsers);
            if (admin.IsPartnerAdmin) list.Add(RoleModel.PartnerAdmin);
            if (admin.IsCenterAdmin) list.Add(RoleModel.CenterAdmin);
            if(admin.IsCaptureAdmin) list.Add(RoleModel.CaptureAdmin);
            
   

            
            return list.ToArray();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
       
    }
}