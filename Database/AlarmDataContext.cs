using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using WorldCupGuide.Models;

namespace WorldCupGuide.Database
{
    public class AlarmDataContext : DataContext
    {
        private const string ConnectionString = "Data Source=isostore:/DB/alarm.sdf";

        public AlarmDataContext()
            : base(ConnectionString)
        {
            if (!DatabaseExists())
            { 
                using(IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.DirectoryExists("DB"))
                    {
                        isf.CreateDirectory("DB");
                    }
                }

                CreateDatabase();
            }

            
        }

        public Table<AlarmItem> Alarms;
    }
}
