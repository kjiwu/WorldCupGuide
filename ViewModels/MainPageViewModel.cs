using Microsoft.Phone.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using WorldCupGuide.Models;

namespace WorldCupGuide.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Constructor

        public MainPageViewModel()
        {
            InitTeamGroups();
            InitTimeGroups();

            Groups = teamGroups;
        }

        #endregion

        #region Groups

        ObservableCollection<GroupItem<MatchItem>> groups;

        public ObservableCollection<GroupItem<MatchItem>> Groups
        {
            get
            {
                return groups;
            }
            set
            {
                groups = value;
                RaisePropertyChanged(() => this.Groups);
            }
        }

        #endregion

        #region InitTeamGroups

        ObservableCollection<GroupItem<MatchItem>> teamGroups;
        ObservableCollection<GroupItem<MatchItem>> timeGroups;

        private void InitTeamGroups()
        {
            teamGroups = new ObservableCollection<GroupItem<MatchItem>>();
            foreach (var team in Constants.GroupTeams.ToCharArray())
            {
                GroupItem<MatchItem> groupItem = new GroupItem<MatchItem>();
                groupItem.GroupName = String.Format("{0}组", team);

                var query = from match in Constants.LoadMatchItems()
                            where match.HomeTeamCode.Contains(team.ToString()) &&
                                  match.VisitingTeamCode.Contains(team.ToString())
                            select match;

                var ob = Observable.GenerateWithTime(0, x => x < query.Count(),
                                                     x => query.ElementAt(x),
                                                     x => TimeSpan.FromMilliseconds(10),
                                                     x => x + 1);
                ob.ObserveOnDispatcher().Subscribe(x =>
                {
                    groupItem.Add(x);
                });

                teamGroups.Add(groupItem);
            }
        }


        private void InitTimeGroups()
        {
            timeGroups = new ObservableCollection<GroupItem<MatchItem>>();

            var TimeResult = (from match in Constants.LoadMatchItems()
                              orderby match.Date
                              group match by match.Date into pDate
                              select pDate.Key).Distinct();

            foreach (var date in TimeResult)
            {
                GroupItem<MatchItem> groupItem = new GroupItem<MatchItem>();
                groupItem.GroupName = date;

                var query = from match in Constants.LoadMatchItems()
                            where match.Date.Equals(date)
                            orderby match.Time
                            select match;

                var ob = Observable.GenerateWithTime(0, x => x < query.Count(),
                                                     x => query.ElementAt(x),
                                                     x => TimeSpan.FromMilliseconds(10),
                                                     x => x + 1);
                ob.ObserveOnDispatcher().Subscribe(x =>
                {
                    groupItem.Add(x);
                });

                timeGroups.Add(groupItem);
            }
        }

        #endregion

        #region ShowStyleChanged

        public void ShowStyleChanged(object sender, RoutedEventArgs e)
        {
            int tag = int.Parse((sender as ToggleButton).Tag.ToString());
            switch (tag)
            {
                case 1:
                    Groups = teamGroups;
                    break;
                case 2:
                    Groups = timeGroups;
                    break;
            }
        }

        #endregion
    }
}
