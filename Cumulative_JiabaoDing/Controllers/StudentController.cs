using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using Cumulative_JiabaoDing.Controllers;

namespace SchoolProject.Controllers
{
    public class StudentController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListStudent (string SearchKey = null, string SearchId = null)
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> Students = controller.ListStudent(SearchKey,SearchId);
            return View(Students);
        }

        
        public ActionResult ShowStudent(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);


            return View(NewStudent);
        }
        
       
      

    }
}