using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupGuide.Models
{
    public class MatchItem : ViewModelBase
    {
        #region Date

        private string date;
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        #endregion

        #region Time

        private string time;

        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        #endregion
        
    }
}
