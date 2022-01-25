using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVM.Base.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (string item in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
            }
        }
    }
}
