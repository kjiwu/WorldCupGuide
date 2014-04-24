using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupGuide.Models
{
    public class GroupItem<T> : ObservableCollection<T>
    {
        private string groupName;

        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupName"));
            }
        }
    }
}
