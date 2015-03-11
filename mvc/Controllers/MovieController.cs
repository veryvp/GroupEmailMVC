using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
using System.Data;
using System.Web.Script.Serialization;

namespace mvc.Controllers
{
    public class MovieController : Controller
    {
        myDBContext db = new myDBContext();
        public ActionResult Index()
        {
            var movies = from m in db.Movie
                         where m.ReleaseDate > new DateTime(1984, 6, 1)
                         select m;
            return View(movies.ToList());
        }


        public ActionResult Index1()
        {
            ViewBag.Page = 1;
            ViewBag.PageCount=10;
            ViewBag.RecordCount = 100;


            var dt = new DataTable("test");
            dt.Columns.Add("name", typeof(string));
            var row = dt.NewRow();
            row["name"] = "梅西";
            dt.Rows.Add(row);
            return View(dt);
        }

        //
        //// GET: /Movies/

        //public ActionResult Index()
        //{
        //    return View();
        //}




        public ActionResult Index2()
        {
            return View();
        }


        public ActionResult AjaxByJquery()
        {
            ViewBag.Page = 1;
            ViewBag.PageCount = 10;
            ViewBag.RecordCount = 100;


            var dt = new DataTable("test");
            dt.Columns.Add("name", typeof(string));
            var row = dt.NewRow();
            row["name"] = "C罗";
            dt.Rows.Add(row);
            return View(dt);
        }


        public string ajaxdata()
        {
            Pageing Pageing1 = new Pageing();
            Pageing1.Page = 1;
            Pageing1.PageCount = 10;
            Pageing1.RecordCount = 100;
            Pageing1.Type = "0";

            var dt = new DataTable("test");
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            var row = dt.NewRow();
            row["id"] = "1";
            row["name"] = "C罗";
            dt.Rows.Add(row);
            var row1 = dt.NewRow();
            row1["id"] = "2";
            row1["name"] = "梅西";
            dt.Rows.Add(row1);

            Pageing1.Data = dt;
            return Pageing1.ToJson();
        }



        //创建电影的查看页面
        public ActionResult Create()
        {
            return View();
        }

        //创建电影的提交页面
        [HttpPost]
        public ActionResult Create(Movie newMovie)
        {

            if (ModelState.IsValid)
            {
                db.Movie.Add(newMovie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(newMovie);
        }
    }
}
