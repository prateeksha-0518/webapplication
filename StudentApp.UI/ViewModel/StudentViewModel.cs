using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpfcurd.command;
using Wpfcurd.Models;

namespace Wpfcurd.ViewModel
{
   
        public class StudentViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }

            }
            private Student _student;
            StudentService objStudentService;
            public StudentViewModel()
            {

                objStudentService = new StudentService();
                LoadData();


                CurrentStudent = new Student();

                editCommand = new Relaycommand(Edit);
                updateCommand = new Relaycommand(Update);
                SelectedStudent = new Student();
                clearCommand = new Relaycommand(clearData);
                saveCommand = new Relaycommand(Save);

                searchCommand = new Relaycommand(Search);


                deleteCommand = new Relaycommand(Delete);
                loadCommand = new Relaycommand(Load);


            }



            private ObservableCollection<Student> studentsList;
            public ObservableCollection<Student> StudentsList
            {
                get
                {
                    return studentsList;
                }
                set { studentsList = value; OnPropertyChanged("StudentsList"); }
            }
            private void LoadData()
            {
                StudentsList = new ObservableCollection<Student>(objStudentService.GetAll());

            }
            public bool isValid()
            {
                if (string.IsNullOrEmpty(CurrentStudent.Name))
                {
                    MessageBox.Show("Name is required", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (string.IsNullOrEmpty(CurrentStudent.Roll))
                {
                    MessageBox.Show("Roll is required", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                MessageBox.Show("Name is required", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
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




            public void clearData()
            {

                //currentStudent.StudentId = int.Parse("");
                currentStudent.Name = "";
                currentStudent.Roll = "";


            }
            //Property that holds value of textboxes
            private Student currentStudent;
            public Student CurrentStudent
            {
                get
                { return currentStudent; }
                set
                {
                    currentStudent = value; OnPropertyChanged("CurrentStudent");
                }
            }
            private Student _selectedStudent;
            public Student SelectedStudent
            {
                get { return _selectedStudent; }
                set
                {
                    _selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));
                }
            }
            private Relaycommand editCommand;
            public Relaycommand EditCommand
            {
                get
                {
                    return editCommand;
                }
                set
                {
                    value = editCommand;
                    OnPropertyChanged(nameof(Edit));
                }
            }
            public void Edit()
            {

                if (SelectedStudent != null)
                {
                    CurrentStudent = SelectedStudent;
                }
            }

            private Relaycommand updateCommand;
            public Relaycommand UpdateCommand
            {
                get
                {
                    return updateCommand;
                }
            }
            public void Update()
            {
                var IsUpdated = objStudentService.Update(CurrentStudent);

                if (IsUpdated)
                {
                    MessageBox.Show("Student Updated", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    LoadData();
                    clearData();
                }
                else
                {
                    MessageBox.Show("Update failed", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }


            private Relaycommand saveCommand;
            public Relaycommand SaveCommand
            {
                get
                {

                    return saveCommand;
                }
            }

            public void Save()
            {
                  var IsSaved = objStudentService.Add(CurrentStudent);
                    if (IsSaved)
                    {
                        MessageBox.Show("Student Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        LoadData();
                        clearData();
                    }
                    else
                        MessageBox.Show("Student failed to save", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                
            }

            private Relaycommand searchCommand;
            public Relaycommand SearchCommand
            {
                get
                {
                    return searchCommand;
                }
                set
                {
                    searchCommand = value;
                }

            }

            private String searchText;
            public string SearchText
            {
                get
                { return searchText; }
                set
                {
                    searchText = value; OnPropertyChanged("SearchText");
                }
            }
            public async void Search()
            {
                var objStudent = await objStudentService.Search(Convert.ToInt32(SearchText));
                if (objStudent != null)
                {
                    StudentsList = new ObservableCollection<Student>(objStudent);
                    
                }
                else
                {
               
                    MessageBox.Show("Student not found", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }



            private Relaycommand loadCommand;
            public Relaycommand LoadCommand
            {
                get
                {
                    return loadCommand;
                }

            }
            public void Load()
            {
                LoadData();
            }


            private Relaycommand deleteCommand;
            public Relaycommand DeleteCommand
            {
                get
                {
                    return deleteCommand;
                }
            }

            private void Delete()
            {
                var IsDelete = objStudentService.Delete(CurrentStudent.StudentId);
                if (IsDelete)
                {
                    MessageBox.Show("Student Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    LoadData();

                    clearData();
                }
                else
                {
                    MessageBox.Show("Student not Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }

        }
    }

