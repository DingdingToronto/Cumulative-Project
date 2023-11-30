using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MySql.Data.MySqlClient;
using SchoolProject.Models;

namespace Cumulative_JiabaoDing.Controllers
{
    public class StudentDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext Blog = new SchoolDbContext();

        //This Controller Will access the authors table of our blog database.
        /// <summary>
        /// Returns a list of Teacher in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeacher</example>
        /// <returns>
        /// A list of Teacher objects.
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudent/{SearchKey?}")]
        // http://localhost:62499/api/TeacherData/ListTeacher
        public IEnumerable<Student> ListStudent(string SearchKey = null, string SearchId = null, DateTime? SearchDate = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * FROM students WHERE " +
                  "(LOWER(studentfname) LIKE LOWER(@key) OR LOWER(studentlname) LIKE LOWER(@key) OR LOWER(CONCAT(studentfname, ' ', studentlname)) LIKE LOWER(@key)) " +
                  "AND LOWER(studentnumber) LIKE LOWER(@key2)" + "AND (enroldate= @key3 OR @key3 IS NULL OR @key3 = '0001-01-01')";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Parameters.AddWithValue("@key2", "%" + SearchId + "%");
            cmd.Parameters.AddWithValue("@key3", SearchDate);
            cmd.Prepare();


            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Student> Students = new List<Student> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);


                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.StudentEnrolDate = StudentEnrolDate;

                //Add the Author Name to the List
                Students.Add(NewStudent);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return Students;
        }


        /// <summary>
        /// Returns an individual author from the database by specifying the primary key authorid
        /// </summary>
        /// <param name="id">the author's ID in the database</param>
        /// <returns>An author object</returns>
        [System.Web.Http.HttpGet]
        public Student FindStudent(int id)
        {
            Student NewStudent = new Student();

            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Students where Studentid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);


                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.StudentEnrolDate = StudentEnrolDate;
            }


            return NewStudent;
        }

       
        
        

    }


}
