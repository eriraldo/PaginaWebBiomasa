using ProjectWebPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectWebPage.Controllers
{
    public class TransportistaController : Controller
    {
        // GET: Transportista
        public ActionResult Transportista()
        {
            return View();
        }

        public ActionResult MenuTrans(Login login)
        {

            return View();
        }

        public ActionResult MostrarViajes()
        {
            return View();
        }
    }
}