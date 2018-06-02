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
        public static List<string> SelectedCountryList;
        public int criterion; //Ειναι το index της λιστας των χωρων.
        public int chart;
        protected void Page_Load(object sender, EventArgs e)
        {
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
            int cou = 0;
            for (int i = 0; i < CountryList.Count; i++)
            {
                CheckBox countryCheckBox = FindControl(CountryList[i].Title) as CheckBox;
                if (countryCheckBox.Checked == true)
                {
                    //Do Whatever
                    cou++;
                }
            }

            int popa = 0;
        }
    }
}