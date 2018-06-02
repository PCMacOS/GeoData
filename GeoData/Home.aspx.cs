using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;
using GeoData.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;

namespace GeoData
{
    public partial class Home : System.Web.UI.Page
    {
        public bool exist;
        public static string image_flag;
        public static string image_title;
        public static string capital;
        public static string coordinatesLant;
        public static string coordinatesLont;
        public static string area_km2;
        public static string population_estimate;
        public static string GDP_nominal_per_capita;
        public static string gini;
        public static string hdi;
        public  static string title;
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
        static readonly Regex trimmer = new Regex(@"\s\s+");

        
        protected void Check_if_exist(object sender, EventArgs eventArgs)
        {
            if (CountrySearch.Text.Trim()=="")
            {
                coutry_status.Text = "You did not search for a country! 😲 <p>Well, how do you think it works? If you do not put a country, we will not give you any result... 😒</p>";
            }
            else if (RemoteFileExists("https://en.wikipedia.org/wiki/" + CountrySearch.Text))
            {
                coutry_status.Text = "Hooray!!! The country of " + CountrySearch.Text + " exist! 😄 <p>Τhe data of the country is the following...</p>";
                //Parsing from wekipedia api
                try
                {

                    int count = 0;
                    int indexOfCut = 0;

                    var jsonUrL = new WebClient().DownloadString("https://en.wikipedia.org/w/api.php?action=query&titles=" + CountrySearch.Text + "&prop=revisions&rvprop=content&format=json&formatversion=2");
                    jsonUrL = trimmer.Replace(jsonUrL, " ");

                    var reg_image_flag = Regex.Match(jsonUrL, @"(?<=image_flag = )(.*)(?=\\n)");
                    var reg_capital = Regex.Match(jsonUrL, @"(?<=capital = \[\[)(.*)(?=\])");
                    var reg_coordinates = Regex.Match(jsonUrL, @"(?<=coordinates = {{Coord\||coordinates = {{coord\||{small\|{{coord\|)(.*)(?=\|type:city|\\n)");
                    var reg_area_km2 = Regex.Match(jsonUrL, @"(?<=area_sq_mi = )(.*)(?=\\n|<)");
                    var reg_population_estimate = Regex.Match(jsonUrL, @"(?<=population_estimate = )(.*)(?=\\n)");
                    var reg_GDP_nominal_per_capita = Regex.Match(jsonUrL, @"(?<=GDP_nominal_per_capita = )(.*)(?=<)");
                    var reg_gini = Regex.Match(jsonUrL, @"(?<=Gini = )(.*)(?=<)");
                    var reg_hdi = Regex.Match(jsonUrL, @"(?<=HDI = )(.*)(?=<)");


                    image_flag = reg_image_flag.Groups[1].Value;
                    image_flag = image_flag.Remove(image_flag.IndexOf('\\'), image_flag.Length - image_flag.IndexOf('\\'));

                    image_flag = Regex.Match(image_flag, @"(?<=File:)(.*)(?=)").Groups[1].Value;
                    if (image_flag == "")
                    {
                        image_flag= reg_image_flag.Groups[1].Value;
                        image_flag = image_flag.Remove(image_flag.IndexOf('\\'), image_flag.Length - image_flag.IndexOf('\\'));
                    }
                    
                    
                    image_flag = image_flag.Remove(image_flag.IndexOf(".svg")+4, image_flag.Length- image_flag.IndexOf(".svg") - 4);
                    image_title = image_flag;

                    image_flag = "https://en.wikipedia.org/wiki/File:" + image_flag.Replace(" ", "_");
                    capital = reg_capital.Groups[1].Value;
                    capital = capital.Remove(capital.IndexOf(']'), capital.Length - capital.IndexOf(']'));
                    string coordinates = reg_coordinates.Groups[1].Value.Replace("|", ".");
                    //coordinates = coordinates.Insert(coordinates.LastIndexOf('W') + 1, "|");
                    // coordinates = coordinates.Remove(coordinates.IndexOf('|'), coordinates.Length - coordinates.IndexOf('|'));
                    for (indexOfCut = 0; indexOfCut < coordinates.Length; indexOfCut++)
                    {
                        if (!Char.IsNumber(coordinates[indexOfCut]) && coordinates[indexOfCut] != '.' && coordinates[indexOfCut] != 'N' && coordinates[indexOfCut] != 'E' && coordinates[indexOfCut] != 'S' && coordinates[indexOfCut] != 'W') break;
                    }
                    coordinates = coordinates.Remove(indexOfCut, coordinates.Length - indexOfCut);
                    for (indexOfCut = 0; indexOfCut < coordinates.Length; indexOfCut++)
                    {
                        if (coordinates[indexOfCut] == '.') count++;
                        if (count==3) break;
                    }

                    bool isAreakm2 = false;
                    if (coordinates != "")
                    {
                        coordinatesLant =
                            ConvertDegreeAngleToDouble(coordinates
                                .Remove(indexOfCut + 1, coordinates.Length - indexOfCut - 1).Remove(indexOfCut - 2, 1)
                                .Remove(indexOfCut - 1)).ToString().Replace(",", ".");
                        coordinatesLont = ConvertDegreeAngleToDouble(coordinates.Remove(0, indexOfCut + 1)).ToString()
                            .Replace(",", ".");
                    }

                    area_km2 = reg_area_km2.Groups[1].Value;

                    if (area_km2 == "" || !Char.IsNumber(area_km2, 0) )
                    {
                        reg_area_km2 = Regex.Match(jsonUrL, @"(?<=area_km2 = )(.*)(?=\\n|<)");
                        area_km2 = reg_area_km2.Groups[1].Value;
                        if (area_km2 == "")
                        {
                            reg_area_km2 = Regex.Match(jsonUrL, @"(?<={{convert\|)(.*)(?=\|km2}}<)");
                            area_km2 = reg_area_km2.Groups[1].Value;
                        }
                        isAreakm2 = true;


                    }
                    area_km2 = area_km2.Remove(area_km2.IndexOf('\\'), area_km2.Length - area_km2.IndexOf('\\'));
                    try
                    {
                        area_km2 = area_km2.Remove(area_km2.IndexOf('<'), area_km2.Length - area_km2.IndexOf('<'));
                    }
                    catch (Exception e)
                    {
                        
                    }
                    
                    count = 0;
                    for (indexOfCut = 0; indexOfCut < area_km2.Length; indexOfCut++)
                    {
                        if (!Char.IsNumber(area_km2[indexOfCut])) count++;
                        if (count == 3) break;
                    }
                    area_km2 = area_km2.Remove(indexOfCut, area_km2.Length - indexOfCut);

                    if (!isAreakm2) area_km2 = (Convert.ToDouble((area_km2.Replace(",", "").Replace(".", ",").Trim())) * 2.58998811).ToString();

                    population_estimate = reg_population_estimate.Groups[1].Value;
                    if (population_estimate != "")
                    {
                        population_estimate = population_estimate.Remove(population_estimate.IndexOf('\\'), population_estimate.Length - population_estimate.IndexOf('\\'));

                        count = 0;
                        for (indexOfCut = 0; indexOfCut < population_estimate.Length; indexOfCut++)
                        {
                            if (Char.IsNumber(population_estimate[indexOfCut]))
                            {
                                break;
                            }
                        }
                        population_estimate = population_estimate.Remove(0, indexOfCut);

                        for (indexOfCut = 0; indexOfCut < population_estimate.Length; indexOfCut++)
                        {
                            if (!Char.IsNumber(population_estimate[indexOfCut]) && population_estimate[indexOfCut] != ',') break;
                        }
                        population_estimate = population_estimate.Remove(indexOfCut, population_estimate.Length - indexOfCut);
                    }
                    
                    


                    GDP_nominal_per_capita = reg_GDP_nominal_per_capita.Groups[1].Value;
                    GDP_nominal_per_capita = GDP_nominal_per_capita.Remove(GDP_nominal_per_capita.IndexOf('<'), GDP_nominal_per_capita.Length - GDP_nominal_per_capita.IndexOf('<'));

                    count = 0;
                    for (indexOfCut = 0; indexOfCut < GDP_nominal_per_capita.Length; indexOfCut++)
                    {
                        if (Char.IsNumber(GDP_nominal_per_capita[indexOfCut]))
                        {
                            break;
                        }
                    }

                    GDP_nominal_per_capita = GDP_nominal_per_capita.Remove(0, indexOfCut).Insert(0, "$");
                    for (indexOfCut = 0; indexOfCut < GDP_nominal_per_capita.Length; indexOfCut++)
                    {
                        if (!Char.IsNumber(GDP_nominal_per_capita[indexOfCut])) count++;
                        if (count == 3) break;
                    }
                    GDP_nominal_per_capita = GDP_nominal_per_capita.Remove(indexOfCut, GDP_nominal_per_capita.Length - indexOfCut);
                    


                    gini = reg_gini.Groups[1].Value.Trim();
                    if (gini != "")
                    {
                        gini = gini.Remove(gini.IndexOf('<'), gini.Length - gini.IndexOf('<'));

                        for (indexOfCut = 0; indexOfCut < gini.Length; indexOfCut++)
                        {
                            if (!Char.IsNumber(gini[indexOfCut]) && gini[indexOfCut] != '.') break;
                        }

                        gini = gini.Remove(indexOfCut, gini.Length - indexOfCut);
                    }

                    hdi = reg_hdi.Groups[1].Value.Trim();
                    hdi = hdi.Remove(hdi.IndexOf('<'), hdi.Length - hdi.IndexOf('<'));
                    try
                    {
                        hdi = hdi.Remove(hdi.IndexOf('\\'), hdi.Length - hdi.IndexOf('\\'));
                    }
                    catch (Exception e)
                    {

                    }

                    var imageUrL = new WebClient().DownloadString(image_flag);
                    reg_image_flag = Regex.Match(imageUrL, @"(?<=<a href="")(.*)(?="" class=""internal"" title="""+image_title+"\">Original file</a>)");
                    image_flag = "https:" + reg_image_flag.Groups[1].Value;
                    exist = true;
                    title = FirstCharToUpper(CountrySearch.Text);
                }
                catch (Exception e)
                {
                    coutry_status.Text = "Nahh!!! The country of " + CountrySearch.Text + " does not exist... 😢 <p>Please look only for real countries and not from your imagination.<p><small>Of course, maybe the parser does not work properly. But this is another story.... 😏</small></p></p>";
                    exist = false;
                }
            }
            else
            {
                coutry_status.Text = "Nahh!!! The country of " + CountrySearch.Text + " does not exist... 😢 <p>Please look only for real countries and not from your imagination.</p>";
            }
        }

        

        public double ConvertDegreeAngleToDouble(string point)
        {
            //Example: 17.21.18S

            var multiplier = (point.Contains("S") || point.Contains("W")) ? -1 : 1; //handle south and west

            point = Regex.Replace(point, "[^0-9.]", ""); //remove the characters

            var pointArray = point.Split('.'); //split the string.

            //Decimal degrees =  
            //   whole number of degrees, 
            //   plus minutes divided by 60, 
            //   plus seconds divided by 3600

            var degrees = Double.Parse(pointArray[0]);
            var minutes = Double.Parse(pointArray[1]) / 60;
            //var seconds = Double.Parse(pointArray[2]) / 3600;
            var seconds = 0;

            return (degrees + minutes + seconds) * multiplier;
        }

        protected void AddCountry_OnClick(object sender, EventArgs e)
        {
            Guid guid = System.Guid.NewGuid();
            var ok = true;
            using (var db = new Country())
            {
                var addCountry = new Data
                {
                    DataId = guid.ToString(),
                    AreaKm2 = area_km2,
                    Capital = capital,
                    CoordinatesLant = coordinatesLant,
                    CoordinatesLont = coordinatesLont,
                    GdpNominalPerCapita = GDP_nominal_per_capita,
                    Gini = gini,
                    Hdi = hdi,
                    ImageFlag = image_flag,
                    PopulationEstimate = population_estimate,
                    Title = title
                };
                if (db.CountryDatas.Any(o => o.Title == title))
                {
                    string countryTitle = title;
                    ClearCountry();
                    coutry_status.Text = "Booo!! The country of " + countryTitle + " has already present in database! 👎<p></p>";
                    return;
                }
                db.CountryDatas.Add(addCountry);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception exception)
                {
                    ok = false;
                    coutry_status.Text = "Oups!! The country of " + title + " has don't added to the database... 😵<p></p>";
                }
                
            }

            if (ok)
            {
                string country = title;
                ClearCountry();
                coutry_status.Text = "Fine! The country of " + country + " added to the database! 😉<p></p>";
            }
        }

        protected void CancelCountry_OnClick(object sender, EventArgs e)
        {
            ClearCountry();
        }

        protected void ClearCountry()
        {
            exist = false;
            image_flag = null;
            image_title = null;
            capital = null;
            coordinatesLant = null;
            coordinatesLont = null;
            area_km2 = null;
            population_estimate = null;
            GDP_nominal_per_capita = null;
            gini = null;
            hdi = null;
            CountrySearch.Text = null;
            coutry_status.Text = null;
        }

        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}