using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public List<Student> GetAll()
        {
            List<Student> objStudentsList = new List<Student>();
            HttpResponseMessage response = client.GetAsync("api/students").Result;
            if (response.IsSuccessStatusCode)
            {
                //it reads the respone content and converts to collection type .Readaync is method to convert IEnumerable
                var students = response.Content.ReadAsAsync<IEnumerable<Student>>().Result;
                //returns list instead of passing ienumerable.list provides flexibility in modifying list
                return students.ToList();
            }
            //return objStudentsList;
            return null;
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
            var response = client.PostAsJsonAsync("api/students", objNewStudent).Result;
            if (response.IsSuccessStatusCode)
            {
                IsAdded = true;
            }
            return IsAdded;
        }


        public bool Update(Student objStudentToUpdate)
        {
            bool IsUpdated = false;
            var response = client.PutAsJsonAsync("api/students/" + objStudentToUpdate.StudentId, objStudentToUpdate).Result;
            //objStudentToUpdate.StudentId = objStudentToUpdate.StudentId;
            //objStudentToUpdate.Name = objStudentToUpdate.Name;
            //objStudentToUpdate.Roll = objStudentToUpdate.Roll;
            if (response.IsSuccessStatusCode)
            {
                IsUpdated = true;
            }
            else
            {
                MessageBox.Show("Student Failed to save", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            return IsUpdated;

        }
        public bool Delete(int id)
        {

            bool IsDeleted = false;
            var url = "api/students/" + id;
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
            var url = "api/students/" + id;
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
        //public  async Task<Student> SearchById(int id)
        //{
        //    Student objstudent = null;
        //    var url = "api/students/" + id;
        //    HttpResponseMessage response =  client.GetAsync(url).Result;
        //    if(response.IsSuccessStatusCode)
        //    {
        //           objstudent= response.Content.ReadAsAsync<Student>().Result;
        //    }
        //    return objstudent;
        //}

        public List<Student> SearchbyName(string name)
        {
            List<Student> objStudentsList = new List<Student>();
            var url = "api/students?name=" + name;
            HttpResponseMessage response =  client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return objStudentsList;
        }
    }
    }
