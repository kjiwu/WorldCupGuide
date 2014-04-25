using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                if (!String.IsNullOrEmpty(value))
                {
                    time = String.Format("{0} {1}", Week, value);
                    RaisePropertyChanged(() => Time);
                }
            }
        }

        #endregion

        #region HomeTeamCode

        private string homeTeamCode;
        public string HomeTeamCode
        {
            get
            {
                return homeTeamCode;
            }
            set
            {
                homeTeamCode = value;
                RaisePropertyChanged(()=> HomeTeamCode);
            }
        }

        #endregion

        #region VisitingTeamCode

        private string visitingTeamCode;

        public string VisitingTeamCode
        {
            get
            {
                return visitingTeamCode;
            }
            set
            {
                visitingTeamCode = value;
                RaisePropertyChanged(() => VisitingTeamCode);
            }
        }
        
        #endregion

        #region Score

        private string score = "0:0";

        public string Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                RaisePropertyChanged(() => Score);
            }
        }

        #endregion

        #region Week

        public string Week
        {
            get
            {
                DateTime d = DateTime.Now;
                if (DateTime.TryParse(Date, out d))
                {
                    return WeekString(d.DayOfWeek);
                }

                return null;
            }
        }

        private string WeekString(DayOfWeek week)
        {
            if (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower() == "zh")
            {
                string result = "星期一";
                switch (week)
                {
                    case DayOfWeek.Monday:
                        result = "星期一";
                        break;
                    case DayOfWeek.Tuesday:
                        result = "星期二";
                        break;
                    case DayOfWeek.Wednesday:
                        result = "星期三";
                        break;
                    case DayOfWeek.Thursday:
                        result = "星期四";
                        break;
                    case DayOfWeek.Friday:
                        result = "星期五";
                        break;
                    case DayOfWeek.Saturday:
                        result = "星期六";
                        break;
                    case DayOfWeek.Sunday:
                        result = "星期天";
                        break;
                }
                return result;
            }

            return week.ToString();
        }

        #endregion
    }
}
