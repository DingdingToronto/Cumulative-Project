using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MySql.Data.MySqlClient;
using SchoolProject.Models;
using System.Web.Http.Cors;
using System.Diagnostics;

namespace Cumulative_JiabaoDing.Controllers
{
    public class TeacherDataController : ApiController
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
        [Route("api/TeacherData/ListTeacher/{SearchKey?}")]
        // http://localhost:62499/api/TeacherData/ListTeacher
        public IEnumerable<Teacher> ListTeacher(string SearchKey = null, double SearchId = 0.00, DateTime? SearchDate = null, string SearchNumber = null)

        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * FROM Teachers WHERE " +
                "(LOWER(teacherfname) LIKE LOWER(@key) OR " +
                "LOWER(teacherlname) LIKE LOWER(@key) OR " +
                "LOWER(teacherfname + ' ' + teacherlname) LIKE LOWER(@key)) AND " +
                "(salary = @key2 OR @key2 = 0.00) AND " +
                "(hiredate = @key3 OR @key3 IS NULL OR @key3 = '0001-01-01') AND " +
                "(LOWER(employeenumber) LIKE LOWER(@key4))"; ;
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Parameters.AddWithValue("@key2", SearchId);
            cmd.Parameters.AddWithValue("@key3", SearchDate);
            cmd.Parameters.AddWithValue("@key4", "%" + SearchNumber + "%");



            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);
                DateTime TeacherDate = Convert.ToDateTime(ResultSet["hiredate"]);
                string TeacherNumber = ResultSet["employeenumber"].ToString();



                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherDate = TeacherDate;
                NewTeacher.TeacherNumber = TeacherNumber;
                

                //Add the Author Name to the List
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return Teachers;
        }


        /// <summary>
        /// Returns an individual author from the database by specifying the primary key authorid
        /// </summary>
        /// <param name="id">the author's ID in the database</param>
        /// <returns>An author object</returns>
        [System.Web.Http.HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * FROM Teachers WHERE Teachers.teacherid = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<string> teacherClasses = new List<string>();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);
                DateTime TeacherDate = Convert.ToDateTime(ResultSet["hiredate"]);
                string TeacherNumber = ResultSet["employeenumber"].ToString();
                

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherDate = TeacherDate;
                NewTeacher.TeacherNumber = TeacherNumber;
               


            }


            return NewTeacher;
        }


        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            Debug.WriteLine(NewTeacher.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES ( @TeacherFname, @TeacherLname, @Employeenumber, @Hiredate, @Salary)";
            
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@Employeenumber", NewTeacher.TeacherNumber);
            cmd.Parameters.AddWithValue("@Hiredate", NewTeacher.TeacherDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.TeacherSalary);


            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();



        }

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

    }


}
