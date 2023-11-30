using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using SchoolProject.Models;

namespace Cumulative_JiabaoDing.Controllers
{
    public class ClassDataController : ApiController
    {

        private SchoolDbContext Blog = new SchoolDbContext();


        [HttpGet]
        [Route("api/ClassData/ListClass/{SearchKey?}")]
       
        public IEnumerable<Class> ListClass(int SearchId = 0, string SearchCode = null, string SearchName = null, DateTime? SearchStartDate = null, DateTime? SearchFinishDate = null, string SearchTeacherName = null)

        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * FROM classes LEFT JOIN teachers ON classes.TeacherId = teachers.TeacherId WHERE (ClassId = @key1 OR @key1 = 0) AND (LOWER(ClassCode) LIKE LOWER(@key2) OR @key2 IS NULL) AND (LOWER(ClassName) LIKE LOWER(@key3) OR @key3 IS NULL) AND (StartDate = @key4 OR @key4 IS NULL) AND (FinishDate = @key5 OR @key5 IS NULL) AND (LOWER(teacherlname) LIKE LOWER(@key6) OR @key6 IS NULL);";



            cmd.Parameters.AddWithValue("@key1", SearchId);
            cmd.Parameters.AddWithValue("@key2", SearchCode != null ? "%" + SearchCode + "%" : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@key3", SearchName != null ? "%" + SearchName + "%" : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@key4", SearchStartDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@key5", SearchFinishDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@key6", SearchTeacherName);



            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Class> Classes = new List<Class> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                DateTime ClassStartDate = Convert.ToDateTime(ResultSet["startdate"]);
                DateTime ClassFinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                string ClassName = ResultSet["classname"].ToString();
                string TeacherName = ResultSet["teacherlname"].ToString();



                Class NewClass = new Class();
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.ClassStartDate = ClassStartDate;
                NewClass.ClassFinishDate = ClassFinishDate;
                NewClass.ClassName = ClassName;
                NewClass.TeacherName = TeacherName;


                //Add the Author Name to the List
                Classes.Add(NewClass);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return Classes;
        }


        /// <summary>
        /// Returns an individual author from the database by specifying the primary key authorid
        /// </summary>
        /// <param name="id">the author's ID in the database</param>
        /// <returns>An author object</returns>
        [System.Web.Http.HttpGet]
        public Class FindClass(int id)
        {
            Class NewClass = new Class();

            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT classes.*, teachers.* FROM classes LEFT JOIN teachers ON classes.teacherid = teachers.teacherid WHERE classes.classid = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                DateTime ClassStartDate = Convert.ToDateTime(ResultSet["startdate"]);
                DateTime ClassFinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                string ClassName = ResultSet["classname"].ToString();
                string TeacherName = ResultSet["teacherlname"].ToString();

                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.ClassStartDate = ClassStartDate;
                NewClass.ClassFinishDate = ClassFinishDate;
                NewClass.ClassName = ClassName;
                NewClass.TeacherName = TeacherName;
                
            }


            return NewClass;
        }




    }
}
