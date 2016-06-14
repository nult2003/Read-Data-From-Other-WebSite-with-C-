using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadEuroResult2016();
        }
        
        /// <summary>
        /// Reference: http://terrikon.com/soccer/euro-2016
        /// </summary>
        public static void ReadEuroResult2016()
        {
            WebProxy proxy = new WebProxy("http://hcm-proxy:9090");
            proxy.Credentials = new NetworkCredential("nhatlm1", "JapaneS1984", "fsoft.fpt.vn");
            proxy.UseDefaultCredentials = true;
            WebRequest.DefaultWebProxy = proxy;

            string _url = "http://terrikon.com/soccer/euro-2016"; //Website that we need read

            var html = new HtmlDocument();
            WebClient client = new WebClient();
            client.Proxy = proxy;

            string downloadString = client.DownloadString(_url);
            html.LoadHtml(downloadString);
            var root = html.DocumentNode;

            string attribute = "class";
            string attributeName = "maincol";

            var rootGroup = root.Descendants()
                .Where(n => n.GetAttributeValue(attribute, "").Equals(attributeName))
                .FirstOrDefault();

            attributeName = "team-info";
            var rootGroupInfor = root.Descendants()
                .Where(n => n.GetAttributeValue(attribute, "").Equals(attributeName));

            HtmlNodeCollection nodesGroupName = rootGroup.SelectNodes("h2");

            int groupNumber = rootGroupInfor.Count();
            int i = 1;

            foreach (HtmlNode node in nodesGroupName)
            {
                string groupName = node.InnerText;
                Console.WriteLine(groupName + " \n");

                HtmlNodeCollection matches = rootGroup.SelectNodes(@"div[" + i + "]/div[2]/table/tr");

                for (int match = 1; match <= matches.Count; match++)
                {
                    string team1 = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[2]").InnerText;

                    string score = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[3]").InnerText;

                    string team2 = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[4]").InnerText;

                    string date = rootGroup.SelectSingleNode(@"div[" + i + "]/div[2]/table/tr[" + match + "]/td[6]").InnerText;

                    Console.WriteLine(team1 + " " + score + " " + team2 + " " + date + "  " + " \n\n");
                }

                i++;

                if (i > groupNumber)
                    break;
            }


        }
    }
}
