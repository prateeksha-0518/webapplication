﻿using System;
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
        StudentService objStudentService;
        public StudentViewModel()
        {
            objStudentService = new StudentService();
            LoadData();
            currentPage = 1;
            PageSize = 10;
            CurrentStudent = new Student();
            editCommand = new Relaycommand(Edit, CanEdit, false);
            SelectedStudent = new Student();
            clearCommand = new Relaycommand(clearData, Canclear, false);
            saveCommand = new Relaycommand(Save, CanSave, false);
            searchCommand = new Relaycommand(Search, CanSearch, false);
            deleteCommand = new Relaycommand(Delete, CanDelete, false);
            previousPageCommand = new Relaycommand(PreviousPage,CanPreviousPage,false);
            nextPageCommand = new Relaycommand(NextPage,CanNextPage,false);
            // loadCommand = new Relaycommand(Load);


        }

        private int pageSize = 10; // Number of items per page
        private int currentPage = 1; // Current page number
        private int totalItems; // Total number of items
        private int totalPages; // Total number of pages

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; NotifyPropertyChanged("PageSize"); }
        }

        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; NotifyPropertyChanged("CurrentPage"); }
        }
        private bool CanNextPage(Object Parameter)
        {
            return CurrentPage < TotalPages;
               
        }
        private bool CanPreviousPage(Object Parameter)
        {
            return CurrentPage > 1;
        }
        public int TotalItems
        {
            get { return totalItems; }
            set { totalItems = value; NotifyPropertyChanged("TotalItems"); }
        }

        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; NotifyPropertyChanged("TotalPages"); }
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
        private void LoadData()
        {
           
            int PageSize = 10;
            StudentsList = new ObservableCollection<Student>(objStudentService.GetAll());
            TotalItems = StudentsList.Count;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
            var paginatedStudents = StudentsList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            StudentsList = new ObservableCollection<Student>(paginatedStudents);
        }
        private Relaycommand nextPageCommand;
        public Relaycommand NextPageCommand
        {
            get { return nextPageCommand; }
            set { nextPageCommand = value; }
        }

        public void NextPage(Object Parameter)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadData();
            }
        }

        private Relaycommand previousPageCommand;
        public Relaycommand PreviousPageCommand
        {
            get { return previousPageCommand; }
            set { previousPageCommand = value; }
        }

        public void PreviousPage(Object Parameter)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadData();
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

        private void clearData(object parameter)
        {
           
            CurrentStudent.Name = "";
            CurrentStudent.Roll = "";
        }

        private bool Canclear(object parameter)
        {
            return true;

        }
        private void clear()
        {
            clearData(null);
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
        private bool CanEdit(object parameter)
        { 
                return true;
            
        }
        public void Edit(object parameter)
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
        
        private bool CanSave(object parameter)
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

        private void Save(object parameter)
        {
             if (currentStudent.StudentId == 0)
                {
                    var IsSaved = objStudentService.Add(CurrentStudent);
                    if (IsSaved)
                    {
                        MessageBox.Show("Student Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        LoadData();
                    clearData("");
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
                    ClearCommand.Execute("");
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
       
        private String searchText;
        public string SearchText
        {
            get
            { return searchText; }
            set
            {
                searchText = value;NotifyPropertyChanged("SearchText");
            }
        }
        private bool CanSearch(object parameter)
        {        
                return true;     
        }


        public void Search(object parameter)
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
        private bool CanDelete(object parameter)
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
        private void Delete(object parameter)
            {
                var IsDelete = objStudentService.Delete(CurrentStudent.StudentId);
                if (IsDelete)
                {
                    MessageBox.Show("Student Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    LoadData();
                    clearData(ClearCommand);
                }
                else
                {
                    MessageBox.Show("Student not Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }

        }
    }

