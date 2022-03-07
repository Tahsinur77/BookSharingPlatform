using BookShare.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BookShare.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SellerOrAdminValidation:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["user"] != null)
            {
                var user = httpContext.Session["user"].ToString();
                var admin = new JavaScriptSerializer().Deserialize<UserModel>(user);
                if (admin.Role == "Admin" || admin.Role == "Seller") return true;
            }

            return false;
        }
    }
}