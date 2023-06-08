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
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("api/GetEmployees")]
        public IHttpActionResult GetStudents()
        {
            using (DatabaseContext  dbContext = new DatabaseContext())
            {
                var Employees = dbContext.Students.ToList();
                if (Employees.Count == 0)
                {
                    throw new Exception();

                }
                else
                {
                    return Ok(Employees);
                }
            }
        }
       
        /// <summary>
        /// Get students by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
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
