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
        
        public ActionResult Create(string TeacherFname, string TeacherLname, string TeacherNumber, double TeacherSalary = 0.00, DateTime? TeacherDate = null)
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
                TeacherDate = (DateTime)TeacherDate,
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

        /// <summary>
        /// Routes to a dynamically generated "Author Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Author</param>
        /// <returns>A dynamic "Update Author" webpage which provides the current information of the author and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Author/Update/5</example>
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }


        /// <summary>
        /// Handles a POST request containing updated information about an existing teacher in the system. Sends the updated details to the API and redirects to the "Teacher Show" page to display the current information of the updated teacher.
        /// </summary>
        /// <param name="id">The identifier of the teacher to update</param>
        /// <param name="TeacherFname">The updated first name of the teacher</param>
        /// <param name="TeacherLname">The updated last name of the teacher</param>
        /// <param name="TeacherNumber">The updated teacher number</param>
        /// <param name="TeacherSalary">The updated salary of the teacher (defaults to 0.00 if not provided)</param>
        /// <param name="TeacherDate">The updated date associated with the teacher (defaults to null if not provided)</param>
        /// <returns>A dynamic webpage displaying the current information of the updated teacher.</returns>
        /// <example>
        /// POST: /Teacher/Update/10
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///     "TeacherFname": "Christine",
        ///     "TeacherLname": "Bittle",
        ///     "TeacherNumber": "12345",
        ///     "TeacherSalary": 50000.00,
        ///     "TeacherDate": "2023-12-14"
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string TeacherNumber, double TeacherSalary = 0.00, DateTime? TeacherDate = null)
        {
            if (string.IsNullOrWhiteSpace(TeacherFname) ||
                string.IsNullOrWhiteSpace(TeacherLname) ||
                string.IsNullOrWhiteSpace(TeacherNumber) ||
                TeacherSalary <= 0 ||
                TeacherDate == DateTime.MinValue)
            {
                // Invalid data, return to the update view with an error message
                TempData["ErrorMessage"] = "Please fill all";
                return RedirectToAction("Update");
            }

            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.TeacherNumber = TeacherNumber;
            TeacherInfo.TeacherSalary = TeacherSalary;
            TeacherInfo.TeacherDate = (DateTime)TeacherDate;


            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("Show/" + id);
        }




    }
}