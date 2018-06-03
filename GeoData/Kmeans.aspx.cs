using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeoData.Models;
using Microsoft.AspNet.Identity;

namespace GeoData
{
    public partial class Kmeans : System.Web.UI.Page
    {
        public static int countryCount;
        public static List<Data> CountryList;
        public static List<int> CountryListGroup = new List<int>();//1||2
        public static int Group;
        public static int Group1Center;
        public static int Group2Center;
        public static int Kstep = 0;
        public Random random = new Random();

        public static double Group1CenterLat;
        public static double Group1CenterLot;
        public static double Group2CenterLat;
        public static double Group2CenterLot;

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new Country())
            {
                countryCount = db.CountryDatas.Count();
                CountryList = db.CountryDatas.ToList();
                var tite = db.CountryDatas.Where(o => o.Title == "Greece").ToList();
            }

        }

        public string GetEMailHash()
        {
            MD5 hasher = MD5.Create();
            byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(Context.User.Identity.GetUserName()));
            StringBuilder sb = new StringBuilder();

            foreach (byte d in data)
                sb.Append(d.ToString("x2"));

            return sb.ToString();
        }

        public void KmeansAlgorithm(object sender, EventArgs e)
        {
            if (Kstep == 0)
            {
                Random randomCoutry = new Random();
                Group1Center = randomCoutry.Next(0, CountryList.Count-1) + 1;
                Group2Center = randomCoutry.Next(0, CountryList.Count - 1) + 1;

                Group1CenterLat = Convert.ToDouble(CountryList[Group1Center].CoordinatesLant.Replace('.', ','));
                Group1CenterLot = Convert.ToDouble(CountryList[Group1Center].CoordinatesLont.Replace('.', ','));
                Group2CenterLat = Convert.ToDouble(CountryList[Group2Center].CoordinatesLant.Replace('.', ','));
                Group2CenterLot = Convert.ToDouble(CountryList[Group2Center].CoordinatesLont.Replace('.', ','));

                int CountG1 = 0;
                int CountG2 = 0;
                double sumG1Lat = 0;
                double sumG1Lot = 0;
                double sumG2Lat = 0;
                double sumG2Lot = 0;

                
                for (int i = 0; i < CountryList.Count; i++)
                {
                    double distanceG1 = 0;
                    double distanceG2 = 0;

                    distanceG1 = Math.Sqrt(
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.',',')) -
                             Convert.ToDouble(CountryList[Group1Center].CoordinatesLant.Replace('.', ','))), 2) +
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ',')) -
                             Convert.ToDouble(CountryList[Group1Center].CoordinatesLont.Replace('.', ','))), 2));

                    distanceG2 = Math.Sqrt(
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ',')) -
                             Convert.ToDouble(CountryList[Group2Center].CoordinatesLant.Replace('.', ','))), 2) +
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ',')) -
                             Convert.ToDouble(CountryList[Group2Center].CoordinatesLont.Replace('.', ','))), 2));

                    if (distanceG1 < distanceG2)
                    {
                        CountryListGroup.Add(1);
                        CountG1++;
                        sumG1Lat = sumG1Lat + Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ','));
                        sumG1Lot = sumG1Lot + Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ','));
                    }
                    if (distanceG1 > distanceG2)
                    {
                        CountryListGroup.Add(2);
                        CountG2++;
                        sumG2Lat = sumG2Lat + Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ','));
                        sumG2Lot = sumG2Lot + Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ','));
                    }
                }


                Group1CenterLat = sumG1Lat / CountG1;
                Group1CenterLot = sumG1Lot / CountG1;
                Group2CenterLat = sumG2Lat / CountG2;
                Group2CenterLot = sumG2Lot / CountG2;


                Kstep++;
            }
            else
            {
                int CountG1 = 0;
                int CountG2 = 0;
                double sumG1Lat = 0;
                double sumG1Lot = 0;
                double sumG2Lat = 0;
                double sumG2Lot = 0;


                for (int i = 0; i < CountryList.Count; i++)
                {
                    double distanceG1 = 0;
                    double distanceG2 = 0;

                    distanceG1 = Math.Sqrt(
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ',')) -
                             Group1CenterLat), 2) +
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ',')) -
                             Group1CenterLot), 2));

                    distanceG2 = Math.Sqrt(
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ',')) -
                             Group2CenterLat), 2) +
                        Math.Pow(
                            (Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ',')) -
                             Group2CenterLot), 2));

                    if (distanceG1 < distanceG2)
                    {
                        CountryListGroup[i] = 1;
                        CountG1++;
                        sumG1Lat = sumG1Lat + Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ','));
                        sumG1Lot = sumG1Lot + Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ','));
                    }
                    if (distanceG1 > distanceG2)
                    {
                        CountryListGroup[i] = 2;
                        CountG2++;
                        sumG2Lat = sumG2Lat + Convert.ToDouble(CountryList[i].CoordinatesLant.Replace('.', ','));
                        sumG2Lot = sumG2Lot + Convert.ToDouble(CountryList[i].CoordinatesLont.Replace('.', ','));
                    }
                }


                Group1CenterLat = sumG1Lat / CountG1;
                Group1CenterLot = sumG1Lot / CountG1;
                Group2CenterLat = sumG2Lat / CountG2;
                Group2CenterLot = sumG2Lot / CountG2;


                Kstep++;
            }

            KmeansStep.Text = "K means Step " + Kstep;
            if (Kstep == 6)
            {
                Kstep = 0;
                KmeansStep.Text = "K means Start";
                CountryListGroup.Clear();
            }
        }
    }
}