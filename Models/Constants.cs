using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;
using System.Xml.Linq;
using WorldCupGuide.Resources;

namespace WorldCupGuide.Models
{
    public class Constants
    {
        public static string GroupTeams = "ABCDEFGH";

        public static readonly Dictionary<string, string> CountryCode = new Dictionary<string, string>()
        {
            {"A1", AppResources.Brazil}, {"A2", AppResources.Croatia}, {"A3", AppResources.Mexico}, {"A4", AppResources.Cameroon},
            {"B1", AppResources.Spain}, {"B2", AppResources.Holland}, {"B3", AppResources.Chile}, {"B4", AppResources.Australia},
            {"C1", AppResources.Columbia}, {"C2", AppResources.Greece}, {"C3", AppResources.Coast}, {"C4", AppResources.Japan},
            {"D1", AppResources.Uruguay}, {"D2", AppResources.CostaRica}, {"D3", AppResources.England}, {"D4", AppResources.Italy},
            {"E1", AppResources.Switzerland}, {"E2", AppResources.Ecuador}, {"E3", AppResources.France}, {"E4", AppResources.Honduras},
            {"F1", AppResources.Argentina}, {"F2", AppResources.BosniaHerzegovina}, {"F3", AppResources.Iran}, {"F4", AppResources.Nigeria},
            {"G1", AppResources.Germany}, {"G2", AppResources.Portugal}, {"G3", AppResources.Garner}, {"G4", AppResources.America},
            {"H1", AppResources.Belgium}, {"H2", AppResources.Algeria}, {"H3", AppResources.Russia}, {"H4", AppResources.Korea},
        };

        private static List<MatchItem> matchItems;

        public static List<MatchItem> LoadMatchItems()
        {
            if (null == matchItems)
            {
                StreamResourceInfo sri = App.GetResourceStream(new Uri("Assets/Data/match.xml", UriKind.Relative));
                if (null != sri)
                {
                    using (Stream stream = sri.Stream)
                    {
                        if (null != stream)
                        {
                            XDocument document = XDocument.Load(stream);
                            var query = from match in document.Descendants().Elements("m")
                                        select new MatchItem()
                                        {
                                            Date = match.Element("d").Value.ToString(),
                                            Time = match.Element("t").Value.ToString(),
                                            HomeTeamCode = match.Element("h").Value.ToString(),
                                            VisitingTeamCode = match.Element("v").Value.ToString(),
                                        };


                            matchItems = new List<MatchItem>(query);
                        }
                    }
                }

                return matchItems;
            }
            else
            {
                return matchItems;
            }
        }
    }
}
