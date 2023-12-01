using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using Cumulative_JiabaoDing.Controllers;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null, double SearchId = 0.00, DateTime? SearchDate = null, string SearchNumber = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeacher(SearchKey,SearchId,SearchDate,SearchNumber);
            return View(Teachers);
        }

        //GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }

        //POST : /Teacher/Create
        [HttpPost]
        
        public ActionResult Create(string TeacherFname, string TeacherLname, double TeacherSalary, DateTime TeacherDate, string TeacherNumber)
        {
            // Server-side validation
            if (string.IsNullOrWhiteSpace(TeacherFname) ||
                string.IsNullOrWhiteSpace(TeacherLname) ||
                string.IsNullOrWhiteSpace(TeacherNumber) ||
                TeacherSalary <= 0 ||
                TeacherDate == DateTime.MinValue)
            {
                // Invalid data, return to the create view with an error message
                TempData["ErrorMessage"] = "Please fill all";
                return RedirectToAction("Create");
            }

            // Data is valid, create a new Teacher instance
            Teacher NewTeacher = new Teacher
            {
                TeacherFname = TeacherFname,
                TeacherLname = TeacherLname,
                TeacherSalary = TeacherSalary,
                TeacherDate = TeacherDate,
                TeacherNumber = TeacherNumber
            };

            // Call the AddTeacher method in your TeacherDataController to add the teacher to the database
            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            // Redirect to the list of teachers or any other appropriate action
            return RedirectToAction("List");
        }


        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }


        //GET : /Teacher/Ajax_New
        public ActionResult Ajax_New()
        {
            return View();

        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }




    }
}