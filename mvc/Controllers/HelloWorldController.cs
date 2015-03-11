using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc.Controllers
{
    public class HelloWorldController : Controller
    {
        //
        // GET: /HelloWorld/

        public ActionResult Index()
        {
            ViewBag.Title = "电影清单";
            return View();
        }


        //
        // GET: /HelloWorld/Welcome/
        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }

        //public string Index()
        //{
        //    return "这是我的<b>默认</b>action...";
        //}

        //
        // GET: /HelloWorld/Welcome/
        //public string WelCome()
        //{
        //    return "这是我的 Welcome 方法...";
        //}
    }
}
