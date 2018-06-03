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
    public partial class BubbleChart : System.Web.UI.Page
    {
        public static int countryCount;
        public static List<Data> CountryList;

        public static List<int> CountryListGroup = new List<int>();//1||2

        public static int option1; //5Aria,7GDP,8Gini,9Hid
        public static int option2;
        public static bool optionSelected = false;

        public static string option1Title;
        public static string option2Title;

        public static List<string> Option1List = new List<string>();
        public static List<string> Option2List = new List<string>();

        public static double Group1CenterOption1;
        public static double Group1CenterOption2;
        public static double Group2CenterOption1;
        public static double Group2CenterOption2;

        public static int Group1Center;
        public static int Group2Center;
        public static int Kstep;
        public Random random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            option1 = Int32.Parse(Option1Select.Value); //5Aria,7GDP,8Gini,9Hid
            option2 = Int32.Parse(Option2Select.Value);

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

        protected void SelectOptions_Click(object sender, EventArgs e)
        {
            Kstep = 0;
            Option1List.Clear();
            Option2List.Clear();
            CountryListGroup.Clear();
            //Option Datas
                foreach (var i in CountryList)
                {
                    //if (option1 == 5) Option1List.Add(i.AreaKm2.Replace(",",".").Replace(".", ","));
                    //if (option2 == 5) Option2List.Add(i.AreaKm2.Replace(",", ".").Replace(".", ","));
                    if (option1 == 7) Option1List.Add(i.GdpNominalPerCapita.Replace("$", "").Replace(",", "").Replace(".", ","));
                    if (option2 == 7) Option2List.Add(i.GdpNominalPerCapita.Replace("$", "").Replace(",", "").Replace(".", ","));
                    if (option1 == 8) Option1List.Add(i.Gini.Replace(".", ","));
                    if (option2 == 8) Option2List.Add(i.Gini.Replace(".", ","));
                    if (option1 == 9) Option1List.Add(i.Hdi.Replace(".", ","));
                    if (option2 == 9) Option2List.Add(i.Hdi.Replace(".", ","));
                }



                //Option Titles
                {
                    //if (option1 == 5) option1Title = "Area km2";
                    //if (option2 == 5) option2Title = "Area km2";
                    if (option1 == 7) option1Title = "GDP Nominal (per person)";
                    if (option2 == 7) option2Title = "GDP Nominal (per person)";
                    if (option1 == 8) option1Title = "Gini";
                    if (option2 == 8) option2Title = "Gini";
                    if (option1 == 9) option1Title = "Hid";
                    if (option2 == 9) option2Title = "Hid";
                }



                for (int step = 0; step < 10; step++)
                {
                    if (Kstep == 0)
                    {
                        Random randomCoutry = new Random();
                        Group1Center = randomCoutry.Next(0, CountryList.Count - 1) + 1;
                        Group2Center = randomCoutry.Next(0, CountryList.Count - 1) + 1;

                        Group1CenterOption1 = Convert.ToDouble(Option1List[Group1Center]);
                        Group1CenterOption2 = Convert.ToDouble(Option2List[Group1Center]);
                        Group2CenterOption1 = Convert.ToDouble(Option1List[Group2Center]);
                        Group2CenterOption2 = Convert.ToDouble(Option2List[Group2Center]);

                        int CountG1 = 0;
                        int CountG2 = 0;
                        double sumG1Option1 = 0;
                        double sumG1Option2 = 0;
                        double sumG2Option1 = 0;
                        double sumG2Option2 = 0;


                        for (int i = 0; i < CountryList.Count; i++)
                        {
                            double distanceG1 = 0;
                            double distanceG2 = 0;

                            distanceG1 = Math.Sqrt(
                                Math.Pow(
                                    (Convert.ToDouble(Option1List[i]) -
                                     Convert.ToDouble(Option1List[Group1Center])), 2) +
                                Math.Pow(
                                    (Convert.ToDouble(Option2List[i]) -
                                     Convert.ToDouble(Option2List[Group1Center])), 2));

                            distanceG2 = Math.Sqrt(
                                Math.Pow(
                                    (Convert.ToDouble(Option1List[i]) -
                                     Convert.ToDouble(Option1List[Group2Center])), 2) +
                                Math.Pow(
                                    (Convert.ToDouble(Option2List[i]) -
                                     Convert.ToDouble(Option2List[Group2Center])), 2));

                            if (distanceG1 < distanceG2)
                            {
                                CountryListGroup.Add(1);
                                CountG1++;
                                sumG1Option1 = sumG1Option1 + Convert.ToDouble(Option1List[i]);
                                sumG1Option2 = sumG1Option2 + Convert.ToDouble(Option2List[i]);
                            }
                            if (distanceG1 > distanceG2)
                            {
                                CountryListGroup.Add(2);
                                CountG2++;
                                sumG2Option1 = sumG2Option1 + Convert.ToDouble(Option1List[i]);
                                sumG2Option2 = sumG2Option2 + Convert.ToDouble(Option2List[i]);
                            }
                        }


                        Group1CenterOption1 = sumG1Option1 / CountG1;
                        Group1CenterOption2 = sumG1Option2 / CountG1;
                        Group2CenterOption1 = sumG2Option1 / CountG2;
                        Group2CenterOption2 = sumG2Option2 / CountG2;


                        Kstep++;
                    }
                    else
                    {
                        int CountG1 = 0;
                        int CountG2 = 0;
                        double sumG1Option1 = 0;
                        double sumG1Option2 = 0;
                        double sumG2Option1 = 0;
                        double sumG2Option2 = 0;


                        for (int i = 0; i < CountryList.Count; i++)
                        {
                            double distanceG1 = 0;
                            double distanceG2 = 0;

                            distanceG1 = Math.Sqrt(
                                Math.Pow(
                                    (Convert.ToDouble(Option1List[i]) -
                                     Group1CenterOption1), 2) +
                                Math.Pow(
                                    (Convert.ToDouble(Option2List[i]) -
                                     Group1CenterOption2), 2));

                            distanceG2 = Math.Sqrt(
                                Math.Pow(
                                    (Convert.ToDouble(Option1List[i]) -
                                     Group2CenterOption1), 2) +
                                Math.Pow(
                                    (Convert.ToDouble(Option2List[i]) -
                                     Group2CenterOption2), 2));

                            if (distanceG1 < distanceG2)
                            {
                                CountryListGroup[i] = 1;
                                CountG1++;
                                sumG1Option1 = sumG1Option1 + Convert.ToDouble(Option1List[i]);
                                sumG1Option2 = sumG1Option2 + Convert.ToDouble(Option2List[i]);
                            }
                            if (distanceG1 > distanceG2)
                            {
                                CountryListGroup[i] = 2;
                                CountG2++;
                                sumG2Option1 = sumG2Option1 + Convert.ToDouble(Option1List[i]);
                                sumG2Option2 = sumG2Option2 + Convert.ToDouble(Option2List[i]);
                            }
                        }


                        Group1CenterOption1 = sumG1Option1 / CountG1;
                        Group1CenterOption2 = sumG1Option2 / CountG1;
                        Group2CenterOption1 = sumG2Option1 / CountG2;
                        Group2CenterOption2 = sumG2Option2 / CountG2;


                        Kstep++;
                    }
                }
            






            optionSelected = true;
        }
    }
}