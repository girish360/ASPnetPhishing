using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ASPnetPhishing.Models;
using Microsoft.AspNet.Identity.Owin;
using ASPnetPhishing;

namespace System.Web.Mvc
{
    public static class RestrictedMenuHelper
    {
        public static Boolean GetUserRole(this HtmlHelper html, string roleName)
        {
            var db = new ApplicationDbContext();
            string username = html.ViewContext.HttpContext.User.Identity.GetUserName();
            //get user roles
            var currentUserRoles = System.Web.Security.Roles.GetRolesForUser(username);
            bool isInRole = false;
            foreach(var r in currentUserRoles)
            {
                if (r == roleName)
                {
                    isInRole = true;
                }
            }
            return isInRole;
        }
    }
}