using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ISS_API
{    
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection("Data Source={insert URL/datbase location here}; Initial Catalog={insert db name here}; User ID={insert username here}; Password={insert password here}");
            TimeSpan ts = new TimeSpan(0, 1, 0);
            int i = 1;
            while (true)
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.open-notify.org/iss-now.json");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    conn.Open();
                    string responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                    List<string> TestList = responseFromServer.Split(new char[] { '\"' }).ToList();
                    float Lon = float.Parse(TestList[15]);
                    float Lat = float.Parse(TestList[11]);
                    int Time = int.Parse(TestList[6].Substring(2, 10));
                    string query = String.Format("INSERT INTO dbo.[Table] (id,time,Longitude,Latitude) VALUES ({0},{1},{2},{3})", i,Time,Lon,Lat);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    i++;
                    response.Close();
                    conn.Close();
                    Thread.Sleep(ts);
                }
            }
        }
    }
}
