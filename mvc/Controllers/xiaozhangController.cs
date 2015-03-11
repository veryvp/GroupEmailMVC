using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
using System.Data;
using System.Web.Script.Serialization;
using System.Reflection;

namespace mvc.Controllers
{
    public class xiaozhangController : Controller
    {
        myDBContext db = new myDBContext();
        public ActionResult Index()
        {
            var movies = from m in db.xiaozhang
                         where m.id>0
                         select m;
            return View(movies.ToList());
        }

        public ActionResult Create()
        {
            xiaozhang aa = new xiaozhang();
            aa.username = "asdasd";
            return View();
        }

        [HttpPost]
        public ActionResult Create(xiaozhang newMovie)
        {

            if (ModelState.IsValid)
            {
                db.xiaozhang.Add(newMovie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(newMovie);
        }


        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <returns>返回Json数据</returns>
        public JsonResult GetAll(string queryJson)
        {
            queryJson="{\"Id\":\"1\",\"Name\":\"张三\",\"Status\":\"1\"}";
            var serializer = new JavaScriptSerializer();
            var queryModel = serializer.Deserialize<QueryModel>(queryJson);
            return Json(queryModel);
        }


        public void OnMetadataCreated(ModelMetadata metadata)
        {
            xiaozhang aa = new xiaozhang();


            var fields = aa.GetType().GetProperties();
        }

    }
}
