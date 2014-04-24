using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupGuide.Models;

namespace WorldCupGuide.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Constructor

        public MainPageViewModel()
        {
            InitTeamGroups();
        }

        #endregion

        #region TeamGroups

        ObservableCollection<GroupItem<MatchItem>> teamGroups;

        public ObservableCollection<GroupItem<MatchItem>> TeamGroups
        {
            get
            {
                return teamGroups;
            }
            set
            {
                teamGroups = value;
                RaisePropertyChanged(() => this.TeamGroups);
            }
        }

        #endregion

        #region InitTeamGroups

        private void InitTeamGroups()
        {
            TeamGroups = new ObservableCollection<GroupItem<MatchItem>>();
            foreach (var team in Constants.GroupTeams.ToCharArray())
            {
                GroupItem<MatchItem> groupItem = new GroupItem<MatchItem>();
                groupItem.GroupName = String.Format("{0}组", team);

                groupItem.Add(new MatchItem());
                groupItem.Add(new MatchItem());
                groupItem.Add(new MatchItem());


                TeamGroups.Add(groupItem);
            }
        }

        #endregion

    }
}
