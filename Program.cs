using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Bytescout.Spreadsheet;

namespace ISS_API
{    
    class Program
    {

        static void Main(string[] args)
        {
            Spreadsheet workbook = new Spreadsheet();
            Worksheet sheet = workbook.Workbook.Worksheets.Add("Coordinates");
            sheet.Cell("A1").Value = "Longitude";
            sheet.Cell("A2").Value = "Latitude";
            sheet.Cell("A3").Value = "Time";
            TimeSpan ts = new TimeSpan(0, 1, 0);
            int i = 2;
            while (true)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.open-notify.org/iss-now.json");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                    List<string> TestList = responseFromServer.Split(new char[] { '\"' }).ToList();
                    float Lon = float.Parse(TestList[5]);
                    float Lat = float.Parse(TestList[9]);
                    string Time = TestList[16].Substring(2, 10);
                    string cellNameA = "A" + i;
                    string cellNameB = "B" + i;
                    string cellNameC = "C" + i;
                    sheet[cellNameA].Value = Lon;
                    sheet[cellNameB].Value = Lat;
                    sheet[cellNameC].Value = Time;
                    i++;
                    workbook.SaveAs("iss_tracking.xlsx");
                    response.Close();
                    workbook.Close();
                    Thread.Sleep(ts);
                }
            }             
        }

    }
}
