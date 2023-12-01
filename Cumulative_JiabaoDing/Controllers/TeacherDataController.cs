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
    /// <summary>
    /// Controller for handling teacher-related operations in the school database.
    /// </summary>
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext Blog = new SchoolDbContext();

        /// <summary>
        /// Returns a list of teachers based on specified search parameters.
        /// </summary>
        /// <param name="SearchKey">Search key for filtering teachers by name.</param>
        /// <param name="SearchId">Search criteria for teacher salary.</param>
        /// <param name="SearchDate">Search criteria for teacher hire date.</param>
        /// <param name="SearchNumber">Search key for filtering teachers by employee number.</param>
        /// <returns>
        /// A list of Teacher objects based on the provided search parameters.
        /// </returns>
        /// <example>GET api/TeacherData/ListTeacher</example>
        [HttpGet]
        [Route("api/TeacherData/ListTeacher/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeacher(string SearchKey = null, double SearchId = 0.00, DateTime? SearchDate = null, string SearchNumber = null)
        {
            // Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            // Open the connection between the web server and the database
            Conn.Open();

            // Establish a new command (query) for the database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL QUERY
            cmd.CommandText = "SELECT * FROM Teachers WHERE " +
                "(LOWER(teacherfname) LIKE LOWER(@key) OR " +
                "LOWER(teacherlname) LIKE LOWER(@key) OR " +
                "LOWER(teacherfname + ' ' + teacherlname) LIKE LOWER(@key)) AND " +
                "(salary = @key2 OR @key2 = 0.00) AND " +
                "(hiredate = @key3 OR @key3 IS NULL OR @key3 = '0001-01-01') AND " +
                "(LOWER(employeenumber) LIKE LOWER(@key4))";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Parameters.AddWithValue("@key2", SearchId);
            cmd.Parameters.AddWithValue("@key3", SearchDate);
            cmd.Parameters.AddWithValue("@key4", "%" + SearchNumber + "%");

            // Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            // Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                // Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);
                DateTime TeacherDate = Convert.ToDateTime(ResultSet["hiredate"]);
                string TeacherNumber = ResultSet["employeenumber"].ToString();

                // Create a new Teacher object and populate its properties
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherDate = TeacherDate;
                NewTeacher.TeacherNumber = TeacherNumber;

                // Add the Teacher to the list
                Teachers.Add(NewTeacher);
            }

            // Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            // Return the final list of teachers
            return Teachers;
        }

        /// <summary>
        /// Returns an individual teacher from the database by specifying the primary key teacherid.
        /// </summary>
        /// <param name="id">The teacher's ID in the database.</param>
        /// <returns>An object representing the teacher with the matching ID.</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            // Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            // Open the connection between the web server and the database
            Conn.Open();

            // Establish a new command (query) for the database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL QUERY
            cmd.CommandText = "SELECT * FROM Teachers WHERE Teachers.teacherid = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                double TeacherSalary = Convert.ToDouble(ResultSet["salary"]);
                DateTime TeacherDate = Convert.ToDateTime(ResultSet["hiredate"]);
                string TeacherNumber = ResultSet["employeenumber"].ToString();

                // Populate the properties of the NewTeacher object
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.TeacherDate = TeacherDate;
                NewTeacher.TeacherNumber = TeacherNumber;
            }

            // Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            // Return the teacher with the matching ID
            return NewTeacher;
        }

        /// <summary>
        /// Adds a new teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object representing the new teacher with fields that map to the columns of the teacher's table. Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"John",
        ///	"TeacherLname":"Doe",
        ///	"TeacherNumber":"T123456",
        ///	"TeacherDate":"2023-01-01",
        ///	"TeacherSalary":50000.00
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            // Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            // Log the first name of the new teacher to the debug output
            Debug.WriteLine(NewTeacher.TeacherFname);

            // Open the connection between the web server and the database
            Conn.Open();

            // Establish a new command (query) for the database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL QUERY
            cmd.CommandText = "INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) " +
                  "VALUES (@TeacherFname, @TeacherLname, @Employeenumber, @Hiredate, @Salary)";

            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@Employeenumber", NewTeacher.TeacherNumber);
            cmd.Parameters.AddWithValue("@Hiredate", NewTeacher.TeacherDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.TeacherSalary);

            cmd.Prepare();

            // Execute the query to insert the new teacher into the database
            cmd.ExecuteNonQuery();

            // Close the connection between the MySQL Database and the WebServer
            Conn.Close();
        }

        /// <summary>
        /// Deletes a teacher from the connected MySQL Database if the ID of that teacher exists. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            // Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            // Open the connection between the web server and the database
            Conn.Open();

            // Establish a new command (query) for the database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // Execute the query to delete the teacher from the database
            cmd.ExecuteNonQuery();

            // Close the connection between the MySQL Database and the WebServer
            Conn.Close();
        }
    }
}

