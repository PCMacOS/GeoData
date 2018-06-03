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
    public partial class Charts : System.Web.UI.Page
    {
        public static int countryCount;
        public static List<Data> CountryList;

        public static int Criterion; //5Aria,7GDP,8Gini,9Hid
        public static int Chart; //1Bar,2Pie,3Colum
        public static bool optionSelected = false;

        public static string CriterionTitle;
        public static string ChartTitle;

        public static List<string> CriterionList = new List<string>();

        public int criterion;
        public int chart;


        int SelectedCountryCount = 0;
        public static List<Data> SelectedCountryList;
        public static List<string> SelectedCountryNamesList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Criterion = Int32.Parse(CriterionSelect.Value); //5Aria,6pop,7GDP,8Gini,9Hid
            Chart = Int32.Parse(ChartSelect.Value); //1Bar,2Pie,3Colum

            //for (int i = 0; i <= CountryList.Count; i++)
            //{
            //    if (CountryList[i].Title.Checked)
            //}


            using (var db = new Country())
            {
                countryCount = db.CountryDatas.Count();
                CountryList = db.CountryDatas.ToList();
                var tite = db.CountryDatas.Where(o => o.Title == "Greece").ToList();
                int popa = 0;
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

        protected void SelectChart_OnClick(object sender, EventArgs e)
        {

            CriterionList.Clear();

            if (Chart == 1) ChartTitle = "Bar Chart";
            if (Chart == 2) ChartTitle = "Pie Chart";
            if (Chart == 3) ChartTitle = "Column Chart";

            foreach (var i in CountryList)
            {
                if (Criterion == 5) CriterionList.Add(i.AreaKm2.Replace(",",".").Replace(".", ","));
                if (Criterion == 6) CriterionList.Add(i.PopulationEstimate.Replace(",", ""));
                if (Criterion == 7) CriterionList.Add(i.GdpNominalPerCapita.Replace("$", "").Replace(",", "").Replace(".", ","));
                if (Criterion == 8) CriterionList.Add(i.Gini.Replace(".", ","));
                if (Criterion == 9) CriterionList.Add(i.Hdi.Replace(".", ","));

                if (Criterion == 5) CriterionTitle = "Area Km2";
                if (Criterion == 6) CriterionTitle = "Population (estimate)";
                if (Criterion == 7) CriterionTitle = "GDP (nominal per capita)";
                if (Criterion == 8) CriterionTitle = "Gini";
                if (Criterion == 9) CriterionTitle = "HID";




                //CheckBox countryCheckBox = FindControl(i.Title) as CheckBox;
                //if (countryCheckBox.Checked == true)
                //{
                //    //Do Whatever
                //    countSelectCountres++;
                //}
            }
            



            optionSelected = true;
        }
    }
}