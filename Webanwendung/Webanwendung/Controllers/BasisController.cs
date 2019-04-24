using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webanwendung.Models.db;
using Webanwendung.Models;

namespace Webanwendung.Controllers
{
    public class BasisController : Controller
    {
        protected bool IsAdmin()
        {
            if ((Session["isAdmin"] != null) && (Convert.ToBoolean(Session["isAdmin"]) == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool IsLoggedIn()
        {
            if (((Session["isAdmin"] != null) && (Convert.ToBoolean(Session["isAdmin"]) == true)) ||
                            ((Session["isRegisteredUser"] != null) && (Convert.ToBoolean(Session["isRegisteredUser"]) == true)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}