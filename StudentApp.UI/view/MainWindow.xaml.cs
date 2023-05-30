using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Wpfcurd.command;
using Wpfcurd.Models;

namespace Wpfcurd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            client.BaseAddress = new Uri("https://localhost:44362/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            
            clearCommand = new Relaycommand(clearData); ;
            Student student = new Student();
            DataContext = student;


        }
        public void clearData()
        {
            txtName.Clear();
            txtRoll.Clear();
            txtStudentId.Clear();
            txtsrch.Clear();
           
        }
        public bool isvalid()
        {
            if (txtName.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (txtRoll.Text == string.Empty)
            {
                MessageBox.Show("Roll is required", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }

        private async Task GetStudents()
        {
            //Sends Request
            HttpResponseMessage response =  await client.GetAsync("api/students");
            if (response.IsSuccessStatusCode)
            {
                //it reads the respone content and converts to collection type .Readaync is method to convert IEnumerable
                var students = await response.Content.ReadAsAsync<IEnumerable<Student>>();
                //it binds students collection to datagrid
                dgStudent.DataContext = students;
            }
            else
            {
                MessageBox.Show("Error Code: " + response.StatusCode + " Message: " + response.ReasonPhrase);
            }
        }



        //private async Task GetStudentsbyid(int id)
        //{
        //    var url = "api/students/" + id;
        //    HttpResponseMessage response = await client.GetAsync(url);
        //    if (response.IsSuccessStatusCode)

        //    {
        //        var students =await  response.Content.ReadAsAsync<Student>();
        //        dgStudent.DataContext = new List<Student> { students };
        //    }
        //    else
        //    {
        //        MessageBox.Show("Student Id with" + id.ToString() + " not found");

        //    }
        //}


     
        //Event Habler for button click
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isvalid())
            {
                //object is created and values are assinged to object to store
                var student = new Student()
                {
                    Name = txtName.Text,
                    Roll = txtRoll.Text
                };

                if (string.IsNullOrEmpty(txtStudentId.Text))
                {
                    // Insert operation
                    //sending request in json format
                    var response =  client.PostAsJsonAsync("api/students", student).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Student saved", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                       //clearing textboxes
                        txtStudentId.Text = "";
                        txtName.Text = "";
                        txtRoll.Text = "";
                        GetStudents();
                    }
                    else
                    {
                        MessageBox.Show("Error Code: " + response.StatusCode + "\nMessage: " + response.ReasonPhrase, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }

                else
                {
                    //passing id 
                    student.StudentId = int.Parse(txtStudentId.Text);
                    var response =client.PutAsJsonAsync("api/students/" + student.StudentId, student).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Student updated", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        txtStudentId.Text = "";
                        txtName.Text = "";
                        txtRoll.Text = "";
                        GetStudents();
                    }
                    else
                    {
                        MessageBox.Show("Error Code: " + response.StatusCode + " Message: " + response.ReasonPhrase);
                    }


                }

            }
        }
    
        void btnEditStudent(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var student = (Student)button.DataContext;
            txtStudentId.Text = student.StudentId.ToString();
            txtName.Text = student.Name;
            txtRoll.Text = student.Roll;

        }
        void btnDeleteStudent(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var student = (Student)button.DataContext;
            txtName.Text = student.Name;
            txtRoll.Text = student.Roll;
            var id = student.StudentId.ToString();
            txtStudentId.Text = id;

            var url = "api/students/" + id;
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("User Deleted");
                GetStudents();
            }
            else
            {
                MessageBox.Show("Error Code: " + response.StatusCode + "\nMessage: " + response.ReasonPhrase);
            }
        }

        private Relaycommand clearCommand;
        public Relaycommand ClearCommand
        {
            get
            {
                return clearCommand;
            }
            set
            {
                clearCommand = value;
            }

        }


        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var searchValue = txtsrch.Text.Trim();

            // Validate input
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                //if (int.TryParse(searchValue,out int id))
                //if(Convert.ToInt32(searchValue)>0)
                // {
                // Search by ID
                if (searchValue.All(char.IsDigit))
                {
                    // Search by ID
                    int id = Convert.ToInt32(searchValue);
                    GetStudentById(id);
                }
                else
                {
                    // Search by name
                    await SearchStudentByName(searchValue);
                }
            }
            else
            {
                MessageBox.Show("Please provide an ID or a name to search for.");
            }
        }
        private async Task SearchStudentByName(string name)
        {
             HttpResponseMessage response = await client.GetAsync($"api/students?name={Uri.EscapeDataString(name)}");
          //  HttpResponseMessage response = await client.GetAsync($"api/students?name={name}");

            if (response.IsSuccessStatusCode)
            {
                var students = await response.Content.ReadAsAsync<List<Student>>();

                
                    dgStudent.DataContext = students;
                  
                
            }
            else
            {
                MessageBox.Show("Error Code: " + response.StatusCode + "\nMessage: " + response.ReasonPhrase);
            }
        }

        private async Task GetStudentById(int id)
        {
            HttpResponseMessage response =await client.GetAsync($"api/students/{id}");

            if (response.IsSuccessStatusCode)
            {
                var student = await response.Content.ReadAsAsync<Student>();

                
                    dgStudent.DataContext = new List<Student> { student };
                      //lblMessage.Content = "student Found";
                    MessageBox.Show("Student Found:\n\n");
                dgStudent.DataContext = "";
                
               
            }
            else 
            {
                MessageBox.Show("Error Code: " + response.StatusCode + "\nMessage: " + response.ReasonPhrase);
                
            }
          
        }


       
        private void btnLoadStudents_Click(object sender, RoutedEventArgs e)
        {
            GetStudents();
        }
    }
}
