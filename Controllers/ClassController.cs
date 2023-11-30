using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using Cumulative_JiabaoDing.Controllers;

namespace SchoolProject.Controllers
{
    public class ClassController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListClass(int SearchId = 0, string SearchCode = null, string SearchName = null, DateTime? SearchStartDate = null, DateTime? SearchFinishDate = null, string SearchTeacherName = null)
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> Classes = controller.ListClass(SearchId, SearchCode, SearchName, SearchStartDate, SearchFinishDate, SearchTeacherName);
            return View(Classes);
        }

        public ActionResult ShowClass(int id)
        {
            ClassDataController controller = new ClassDataController();
            Class NewClass = controller.FindClass(id);


            return View(NewClass);
        }




    }
}