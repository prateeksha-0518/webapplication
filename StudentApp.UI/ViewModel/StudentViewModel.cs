
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpfcurd.command;
using Wpfcurd.Models;

namespace Wpfcurd.ViewModel
{

    public class StudentViewModel : ViewModelBase
    {
        private ObservableCollection<Student> students;
        private int currentPage;
        private int totalPages;

        StudentService objStudentService;
        public ObservableCollection<Student> Students
        {
            get { return students; }
            set
            {
                students = value;
                NotifyPropertyChanged(nameof(Students));
            }
        }
        
    
        public StudentViewModel()
        {
           objStudentService = new StudentService();
            currentPage = 1;
            previousPageCommand = new Relaycommand(PreviousPage, CanPreviousPage,false);
            nextPageCommand = new Relaycommand(NextPage, CanNextPage, false);
            CurrentStudent = new Student();
            editCommand = new Relaycommand(Edit, CanEdit, false);
            SelectedStudent = new Student();
            clearCommand = new Relaycommand(clearData, Canclear, false);
            saveCommand = new Relaycommand(Save, CanSave, false);
            searchCommand = new Relaycommand(LoadData, CanSearch,false);
            deleteCommand = new Relaycommand(Delete, CanDelete, false);
            LoadData();
        }

    
      
        private int totalItems; // Total number of items


        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;

                NotifyPropertyChanged(nameof(CurrentPage));
                LoadData();
            }
        }

        public int TotalPages
        {
            get { return totalPages; }
            set
            {
                totalPages = value;
                NotifyPropertyChanged(nameof(TotalPages));
            }
        }

        private bool CanNextPage()
        {
            return CurrentPage < TotalPages;
               
        }
        private bool CanPreviousPage()
        {
            return CurrentPage > 1;
        }
        public int TotalItems
        {
            get { return totalItems; }
            set { totalItems = value; NotifyPropertyChanged("TotalItems"); }
        }

       

        private ObservableCollection<Student> studentsList;
        public ObservableCollection<Student> StudentsList
        {
            get
            {
                return studentsList;
            }
            set { studentsList = value; NotifyPropertyChanged("StudentsList"); }
        }
       
        public void LoadData()
        {
            int pageSize = 10;
            var students = objStudentService.GetAll(CurrentPage,pageSize,SearchText);
            StudentsList = new ObservableCollection<Student>(students);
            int Totalstudents = objStudentService.GetTotalStudentCount();
            {
                TotalPages = (int)Math.Ceiling((double)Totalstudents / pageSize);
            }

        }

        private Relaycommand nextPageCommand;
        public Relaycommand NextPageCommand
        {
            get { return nextPageCommand; }
            set { nextPageCommand = value; }
        }

        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
               
            }
        }

        private Relaycommand previousPageCommand;
        public Relaycommand PreviousPageCommand
        {
            get { return previousPageCommand; }
            set { previousPageCommand = value; }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
              
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

        private void clearData()
        {
           
            CurrentStudent.Name = "";
            CurrentStudent.Roll = "";
        }
        private string _SortField;
        public string SortField
        {
            get { return _SortField; }
            set
            {
                _SortField = value;
                NotifyPropertyChanged(nameof(SortField));
              
            }
        }
        private string _SortOrder;
        public string SortOrder
        {
            get { return _SortOrder; }
            set
            {
                _SortOrder = value;
                NotifyPropertyChanged(nameof(SortOrder));
               
            }
        }
        private bool Canclear()
        {
            return true;

        }
        private void clear()
        {
            clearData();
        }
        //Property that holds value of textboxes
        private Student currentStudent;
        public Student CurrentStudent
        {
            get
            { return currentStudent; }
            set
            {
                currentStudent = value; NotifyPropertyChanged("CurrentStudent");
            }
        }
        
        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                NotifyPropertyChanged(nameof(SelectedStudent));
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
                NotifyPropertyChanged(nameof(Edit));
            }
        }
        private bool CanEdit()
        { 
                return true;
            
        }
        public void Edit()
        {

            if (SelectedStudent != null)
            {
                CurrentStudent = SelectedStudent;
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
        
        private bool CanSave()
        {
            if (string.IsNullOrEmpty(CurrentStudent.Name) || string.IsNullOrEmpty(currentStudent.Roll))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Save()
        {
             if (currentStudent.StudentId == 0)
                {
                    var IsSaved = objStudentService.Add(CurrentStudent);
                    if (IsSaved)
                    {
                        MessageBox.Show("Student Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        LoadData();
                    clearData();
                }
                    else
                    {
                        MessageBox.Show("Student failed to save", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
         }
                else

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
       
        private string searchText;
        public string SearchText
        {
            get
            { return searchText; }
            set
            {
                searchText = value;NotifyPropertyChanged("SearchText");
            }
        }
        private bool CanSearch()
        {        
                return true;     
        }


        public void Search()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                LoadData();
                return;
            }
             else if (SearchText.All(char.IsDigit))
                {
                    var objStudent =   objStudentService.Search(Convert.ToInt32(SearchText));
                    if (objStudent != null && objStudent.Count > 0)
                    {
                        StudentsList = new ObservableCollection<Student>(objStudent);
                    }
                    else
                    {
                        StudentsList = null;
                        MessageBox.Show("Student not found", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
                else
                {
                    var objStudentbyName = objStudentService.SearchbyName(SearchText);
                    if ( objStudentbyName != null&&objStudentbyName.Count>0)
                    {
                        StudentsList = new ObservableCollection<Student>(objStudentbyName);
                    }
                    else
                    {
                    StudentsList = null;
                    MessageBox.Show("Student not found", "Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
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
        private bool CanDelete()
        {
          if (string.IsNullOrEmpty(CurrentStudent.Name) || string.IsNullOrEmpty(currentStudent.Roll))
            {
                return false;
            }
            else
            {
                return true;
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

