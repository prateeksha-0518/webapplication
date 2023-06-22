using code.context;
using code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using code.CustomException;
using System.Diagnostics;
using System.Threading;

namespace code.Controllers
{
    [ExceptionFilter]
    public class StudentsController : ApiController
    {
        /// <summary>
        /// Get All Students
        /// </summary>
        /// <returns></returns>
        
        //[HttpGet]
        //[Route("api/GetStudents")]
        //public IHttpActionResult GetStudents()
        //{
        //    using (DatabaseContext dbContext = new DatabaseContext())
        //    {
        //        var Employees = dbContext.Students.ToList();
        //        if (Employees.Count == 0)
        //        {
        //            throw new Exception();

        //        }
        //        else
        //        {
        //            return Ok(Employees);
        //        }
        //    }
        //}
        [HttpGet]
        [Route("api/GetStudents")]
        public IHttpActionResult GetStudents(int page = 1, int pageSize = 10, string Search = "", string SortField = "Id", string SortOrder = "asc")
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var students = dbContext.Students
                .OrderBy(s => s.StudentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


                students = students
                .Where(s => string.IsNullOrEmpty(Search) || s.Name.Contains(Search) || s.StudentId.ToString().Contains(Search))
                .ToList();


                switch (SortField.ToLower())
                {
                    case "id":
                        students = (SortOrder.ToLower() == "desc")
                        ? students.OrderByDescending(s => s.StudentId).ToList()
                        : students.OrderBy(s => s.StudentId).ToList();
                        break;
                    case "name":
                        students = (SortOrder.ToLower() == "desc")
                        ? students.OrderByDescending(s => s.Name).ToList()
                        : students.OrderBy(s => s.Name).ToList();
                        break;
                    case "roll":
                        students = (SortOrder.ToLower() == "desc")
                        ? students.OrderByDescending(s => s.Roll).ToList()
                        : students.OrderBy(s => s.Roll).ToList();
                        break;
                    default:
                        // Default sorting by Id in ascending order
                        students = students.OrderBy(s => s.StudentId).ToList();
                        break;
                }

                return Ok(students);
            }
        }
        [HttpGet]
        [Route("api/GetTotalStudentCount")]
        public int GetTotalStudentCount()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                return dbContext.Students.Count();
            }
        }

        [HttpGet]
        [Route("api/Get/{id}")]
        public IHttpActionResult Get(int id)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var entity = dbContext.Students.FirstOrDefault(e => e.StudentId == id);
                if (entity != null)
                {
                    return Ok(entity);
                }
                else
                {
                    throw new NullReferenceException(); // Throw NullReferenceException
                }
            }
        }
        /// <summary>
        /// Get students by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        
        [HttpGet]
        [Route("api/Get1/{name}")]
        public IHttpActionResult Get1(string name)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var students = dbContext.Students.Where(s => s.Name.ToLower() == name.ToLower()).ToList();

                if (students.Count > 0)
                {
                    return Ok(students);
                }
                else
                {
                   
                    throw new NullReferenceException(); ;
                }
            }
        }

        /// <summary>
        /// Inserting students
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("api/Post")]
        public IHttpActionResult Post([FromBody] Student employee)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                dbContext.Students.Add(employee);
                dbContext.SaveChanges();
                return Ok(employee);
            }
        }
        /// <summary>
        /// Updating students
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPut]

        [Route("api/Put/{id}")]
        //[Route("api/students/{id}")]
        public IHttpActionResult Put(int id, [FromBody] Student employee)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var entity = dbContext.Students.FirstOrDefault(e => e.StudentId == id);
                if (entity == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                else
                {
                    entity.Name = employee.Name;
                    entity.Roll = employee.Roll;
                    dbContext.SaveChanges();
                    return Ok(entity);
                }
            }
        }
        /// <summary>
        /// Deleting students by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpDelete]
       
        [Route("api/Delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var entity = dbContext.Students.FirstOrDefault(e => e.StudentId == id);
                if (entity == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                else
                {
                    dbContext.Students.Remove(entity);
                    dbContext.SaveChanges();
                    return Ok();
                }
            }
        }
    }
}
