using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bibliography.Lib.Models
{
    public class Contributor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _firstName;
        public string? FirstName
        {
            get => _firstName;
            set { _firstName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName))); }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName))); }
        }

        private ContributorRole _role;
        public ContributorRole Role
        {
            get => _role;
            set { _role = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Role))); }
        }
    }

}
