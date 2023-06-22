using code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Wpfcurd.Models
{
    public class StudentService
    {
        HttpClient client = new HttpClient();
        public StudentService()
        {
            client.BaseAddress = new Uri("https://localhost:44362/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
       
        public List<Student> GetAll(int page, int pageSize,string Search)
        {
            HttpResponseMessage response = client.GetAsync($"api/GetStudents?page={page}&pageSize={pageSize}&Search={Search}").Result;
            if (response.IsSuccessStatusCode)
            {
                var students = response.Content.ReadAsAsync<List<Student>>().Result;
                return students.ToList();
            }
            return null;
        }
        public int GetTotalStudentCount()
        {
          HttpResponseMessage response = client.GetAsync("api/GetTotalStudentCount").Result;

         if (response.IsSuccessStatusCode)
            {
                int totalStudents = response.Content.ReadAsAsync<int>().Result;
                return totalStudents;
            }
            else
            {

                return 0;
            }
        }


        public bool Add(Student objNewStudent)
        {
            bool IsAdded = false;
            var objStudent = new Student()
            {
                StudentId = objNewStudent.StudentId,
                Name = objNewStudent.Name,
                Roll = objNewStudent.Roll
            };
            var response = client.PostAsJsonAsync("api/Post", objNewStudent).Result;
            if (response.IsSuccessStatusCode)
            {
                IsAdded = true;
            }
            return IsAdded;
        }
       


        public bool Update(Student objStudentToUpdate)
        {
            bool IsUpdated = false;
            var response = client.PutAsJsonAsync("api/Put/" + objStudentToUpdate.StudentId, objStudentToUpdate).Result;
            //objStudentToUpdate.StudentId = objStudentToUpdate.StudentId;
            //objStudentToUpdate.Name = objStudentToUpdate.Name;
            //objStudentToUpdate.Roll = objStudentToUpdate.Roll;
            if (response.IsSuccessStatusCode)
            {
                IsUpdated = true;
            }
           
            return IsUpdated;

        }
        public bool Delete(int id)
        {
            bool IsDeleted = false;
            var url = "api/Delete/" + id;
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                IsDeleted = true;
            }

            return IsDeleted;
        }

        public List<Student> Search(int id)
        {
            List<Student> objStudentsList = new List<Student>();
            var url = "api/Get/" + id;
            HttpResponseMessage response =  client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var student =  response.Content.ReadAsAsync<Student>().Result;
                if (student != null)
                {
                    objStudentsList.Add(new Student()
                    {
                        StudentId = student.StudentId,
                        Name = student.Name,
                        Roll = student.Roll
                    });

                }
            }
            return objStudentsList;
        }

        public List<Student> SearchbyName(string name)
        {
            List<Student> objStudentsList = new List<Student>();
            var url = "api/Get1?name=" + name;
            HttpResponseMessage response =  client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                
                    var student =  response.Content.ReadAsAsync<List<Student>>().Result;
                    if (student != null)
                    {
                        foreach (var students in student)
                        {
                            objStudentsList.Add(new Student()
                            {
                                StudentId = students.StudentId,
                                Name = students.Name,
                                Roll = students.Roll
                            });
                        }
                    }
                
               
            }
            return objStudentsList;
        }
    }
    }
