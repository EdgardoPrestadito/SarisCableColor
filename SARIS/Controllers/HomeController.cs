
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrionCoreCableColor.Controllers
{
    
    public class HomeController : BaseController
    {
     

        public HomeController()
        {
           
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        

    }
}