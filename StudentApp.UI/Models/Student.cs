using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Wpfcurd.Models
{
    //Idataerror info reports error
    public class Student : INotifyPropertyChanged, IDataErrorInfo
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged(string p)
        //{
        //    ProgressChangedEventHandler ph = PropertyChanged;
        //    if (ph != null)
        //        ph(this, new ProgressChangedEventArgs(p));
        //}
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
        private int _StudentId;
        public int StudentId
        {
            get => _StudentId;
            set
            {
                _StudentId = value;
                OnPropertyChanged();
            }
        }

        private string _Name;
        [Required(ErrorMessage = "{0} is required"), DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 6)]
        public string Name
        {
            get => _Name;
            set
            {
                
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _Roll;
        public string Roll
        {
            get => _Roll;
            set
            {
                _Roll = value;
                OnPropertyChanged();
            }
        }
         
        public string Error
        {
            get { return null; }
        }

        public string this[string PropertyName]
        {
            get
            {
                string error = string.Empty;

                switch (PropertyName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            error = "Name cannot be empty";
                        else if (!Regex.IsMatch(Name, @"^[a-zA-Z]+$"))
                        {
                            error = "Should enter alphabets only!!!";
                        }
                        break;
                    case "Roll":
                        if (string.IsNullOrEmpty(Roll))
                            error = "Roll cannot be empty";
                        break;
                }

                return error;
            }
        }
    }
}
