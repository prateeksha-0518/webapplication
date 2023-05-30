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

namespace code.Controllers
{
    [ExceptionFilter]
    public class StudentsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetEmployees()
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



        //[ExceptionFilter]
        //[Route("api/EF/{id}")]// Apply the ExceptionFilter to the specific action

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
        [HttpGet]
        public IHttpActionResult Get(string name)
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



        [HttpPost]
        public IHttpActionResult Post([FromBody] Student employee)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                dbContext.Students.Add(employee);
                dbContext.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = employee.StudentId }, employee);
            }
        }

        [HttpPut]
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

        [HttpDelete]
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
