using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupGuide.Models
{
    [Table(Name = "Alarm")]
    public class AlarmItem : ViewModelBase
    {
        #region IsChecked

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        #endregion

        #region Name

        private string name;

        [Column(IsPrimaryKey = true, CanBeNull = false)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        #endregion

        #region BeginTime

        private string beginTime;

        [Column(CanBeNull = false)]
        public string BeginTime
        {
            get
            {
                return beginTime;
            }
            set
            {
                beginTime = value;
                RaisePropertyChanged(() => BeginTime);
            }
        }

        #endregion

        #region ExpirationTime

        private string expirationTime;

        [Column(CanBeNull = false)]
        public string ExpirationTime
        {
            get
            {
                return expirationTime;
            }
            set
            {
                expirationTime = value;
                RaisePropertyChanged(() => ExpirationTime);
            }
        }

        #endregion

        #region HomeTeam

        private string homeTeam;

        [Column(CanBeNull = false)]
        public string HomeTeam
        {
            get { return homeTeam; }
            set
            {
                homeTeam = value;
                RaisePropertyChanged(() => HomeTeam);
            }
        }

        #endregion

        #region VisitingTeam

        private string visitingTeam;

        [Column(CanBeNull = false)]
        public string VisitingTeam
        {
            get { return visitingTeam; }
            set
            {
                visitingTeam = value;
                RaisePropertyChanged(() => VisitingTeam);
            }
        }

        #endregion

        #region MatchTime

        private string matchTime;

        [Column(CanBeNull = false)]
        public string MatchTime
        {
            get
            {
                return matchTime;
            }
            set
            {
                matchTime = value;
                RaisePropertyChanged(() => MatchTime);
            }
        }

        #endregion
    }
}
