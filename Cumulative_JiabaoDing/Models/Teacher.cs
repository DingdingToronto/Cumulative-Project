using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Teacher
    {
        //The following fields define an Author
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public double TeacherSalary;
        public DateTime TeacherDate;
        public string TeacherNumber;
        public string[] TeacherClass;
    }
}