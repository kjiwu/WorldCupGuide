using Microsoft.Phone.Reactive;
using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WorldCupGuide.Commands;
using WorldCupGuide.CustomControls;
using WorldCupGuide.Database;
using WorldCupGuide.Models;
using WorldCupGuide.Resources;

namespace WorldCupGuide.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Constructor

        public MainPageViewModel()
        {
            InitTeamGroups();
            InitTimeGroups();
            InitAlarms();

            InitPopup();

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
                groupItem.GroupName = String.Format(AppResources.GroupsFormat, team);

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
                              orderby DateTime.Parse(match.Date)
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

        #region Alarms

        private ObservableCollection<AlarmItem> alarms;

        public ObservableCollection<AlarmItem> Alarms
        {
            get
            {
                return alarms;
            }
            set
            {
                alarms = value;
                RaisePropertyChanged(() => Alarms);
            }
        }

        #endregion

        #region InitAlarms

        private void InitAlarms()
        {
            if (null == Alarms)
                Alarms = new ObservableCollection<AlarmItem>();

            Observable.Start(() =>
                {
                    using (AlarmDataContext database = new AlarmDataContext())
                    {
                        foreach (var item in database.Alarms)
                        {
                            Alarms.Add(item);
                        }
                    }
                });
        }

        #endregion

        #region AddAlarmCommand

        public ICommand AddAlarmCommand
        {
            get
            {
                return new AlarmCommand(item =>
                {
                    Debug.WriteLine(item.GetType().Name);
                    MatchItem match = item as MatchItem;
                    string alarmName = String.Format("{0}-{1}", match.HomeTeamCode, match.VisitingTeamCode);
                    Alarm alarm = ScheduledActionService.Find(alarmName) as Alarm;
                    if (null == alarm)
                    {
                        AlarmItem alarmItem = new AlarmItem();
                        alarmItem.HomeTeam = match.HomeTeamCode;
                        alarmItem.VisitingTeam = match.VisitingTeamCode;
                        alarmItem.MatchTime = String.Format("{0} {1}", match.Date, match.TimeOnly);
                        alarmItem.Name = alarmName;
                        alarmItem.BeginTime = DateTime.Parse(alarmItem.MatchTime).Subtract(TimeSpan.FromMinutes(15)).ToString();
                        alarmItem.ExpirationTime = DateTime.Parse(alarmItem.MatchTime).ToString();

                        using (AlarmDataContext databse = new AlarmDataContext())
                        {
                            databse.Alarms.InsertOnSubmit(alarmItem);
                            databse.SubmitChanges();

                            Alarms.Add(alarmItem);
                        }

                        string homeTeamName = String.Empty;
                        if (Constants.CountryCode.ContainsKey(alarmItem.HomeTeam.ToUpper()))
                        {
                            homeTeamName = Constants.CountryCode[alarmItem.HomeTeam.ToUpper()];
                        }

                        string visitingTeamName = String.Empty;
                        if (Constants.CountryCode.ContainsKey(alarmItem.VisitingTeam.ToUpper()))
                        {
                            visitingTeamName = Constants.CountryCode[alarmItem.VisitingTeam.ToUpper()];
                        }

                        string alarmContent = String.Format(AppResources.AlarmContentFormat, homeTeamName, visitingTeamName);

                        alarm = new Alarm(alarmName);
                        alarm.BeginTime = DateTime.Parse(alarmItem.BeginTime);
                        alarm.ExpirationTime = DateTime.Parse(alarmItem.ExpirationTime);
                        alarm.Content = alarmContent;
                        ScheduledActionService.Add(alarm);
                    }
                });
            }
        }

        #endregion

        #region RemoveAlarmCommand

        public ICommand RemoveAlarmCommand
        {
            get
            {
                return new AlarmCommand(x =>
                {
                    AlarmItem alartItem = x as AlarmItem;

                    Alarm alarm = ScheduledActionService.Find(alartItem.Name) as Alarm;
                    if (null != alarm)
                        ScheduledActionService.Remove(alarm.Name);

                    using (AlarmDataContext database = new AlarmDataContext())
                    {
                        var query = from item in database.Alarms where item.Name.Equals(alartItem.Name) select item;
                        database.Alarms.DeleteAllOnSubmit(query);

                        var local = (from item in Alarms where item.Name.Equals(alartItem.Name) select item).ToList();
                        foreach (var ai in local)
                        {
                            Alarms.Remove(ai);
                        }

                        database.SubmitChanges();
                    }
                });
            }
        }

        #endregion

        #region Popup

        Popup popup;

        private void InitPopup()
        {
            popup = new Popup();
            popup.Width = App.Current.Host.Content.ActualWidth;
            popup.Height = App.Current.Host.Content.ActualHeight;

            Grid grid = new Grid() { Background = new SolidColorBrush(Colors.Transparent) };
            grid.Width = popup.Width;
            grid.Height = popup.Height;
            grid.Background = App.Current.Resources["BackgroundBrush"] as ImageBrush;

            Grid contentContainer = new Grid();
            contentContainer.HorizontalAlignment = HorizontalAlignment.Center;
            contentContainer.VerticalAlignment = VerticalAlignment.Center;
            contentContainer.Margin = new Thickness(5);
            contentContainer.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            contentContainer.RowDefinitions.Add(new RowDefinition());
            contentContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            contentContainer.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(contentContainer);

            Image image = new Image();
            image.SetValue(Grid.RowSpanProperty, 2);
            contentContainer.Children.Add(image);

            if ((Constants.OpeningTime - DateTime.Now).Days > 0)
            {
                TextBlock tbkOpeningTimeTitle = new TextBlock() { Text = AppResources.TimeDownTip };
                tbkOpeningTimeTitle.FontSize = 30;
                tbkOpeningTimeTitle.VerticalAlignment = VerticalAlignment.Center;
                tbkOpeningTimeTitle.SetValue(Grid.ColumnProperty, 1);
                tbkOpeningTimeTitle.Foreground = App.Current.Resources["PopupTextBrush"] as SolidColorBrush;
                contentContainer.Children.Add(tbkOpeningTimeTitle);

                TextBlock tbkDays = new TextBlock() { Text = String.Format(AppResources.Days, (Constants.OpeningTime - DateTime.Now).Days) };
                tbkDays.SetValue(Grid.ColumnProperty, 1);
                tbkDays.SetValue(Grid.RowProperty, 1);
                tbkDays.FontSize = 50;
                tbkDays.FontWeight = FontWeights.Black;
                tbkDays.Foreground = App.Current.Resources["PopupTextBrush"] as SolidColorBrush;
                tbkDays.HorizontalAlignment = HorizontalAlignment.Center;
                tbkDays.VerticalAlignment = VerticalAlignment.Center;
                contentContainer.Children.Add(tbkDays);
            }
            else
            {
                List<MatchItem> items = FindMatchingMatch();
                StackPanel sp = new StackPanel();
                sp.SetValue(Grid.RowProperty, 1);
                sp.SetValue(Grid.ColumnSpanProperty, 2);
                foreach (var item in items)
                {
                    MatchItemControl wic = new MatchItemControl();
                    wic.DataContext = item;
                    sp.Children.Add(wic);
                }
                contentContainer.Children.Add(sp);
            }

            popup.Child = grid;
            popup.IsOpen = true;

            var ob = Observable.GenerateWithTime(0, x => x < 1, x => x, x => TimeSpan.FromSeconds(5), x => x + 1);
            ob.ObserveOnDispatcher().Subscribe(x => popup.IsOpen = false);
        }

        #endregion

        #region FindMatchingMatch

        private List<MatchItem> FindMatchingMatch()
        {
            List<MatchItem> result = new List<MatchItem>();

            var query = from item in Constants.LoadMatchItems()
                        where (DateTime.Now - DateTime.Parse(String.Format("{0} {1}", item.Date, item.TimeOnly))).TotalMinutes < 90 &&
                              (DateTime.Now - DateTime.Parse(String.Format("{0} {1}", item.Date, item.TimeOnly))).TotalMinutes > 0
                        select item;

            foreach (var item in query)
            {
                result.Add(item);
            }

            return result;
        }

        #endregion
    }
}
