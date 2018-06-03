<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kmeans.aspx.cs" Inherits="GeoData.Kmeans" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="page-header header-filter" data-parallax="true" style="background-image: url('/Image/city-profile.jpg');"></div>
    <div class="main main-raised">
        <div class="profile-content">
            <div class="container">
                <div class="row">
                    <div class="col-md-6 ml-auto mr-auto">
                        <div class="profile">
                            <div class="avatar">
                                <img src="https://www.gravatar.com/avatar/<% Response.Write(GetEMailHash());%>?s=200" alt="Circle Image" class="img-raised rounded-circle img-fluid">
                            </div>
                            <div class="name">
                                <h3 class="title"><%: Context.User.Identity.GetUserName()  %></h3>
                                <script type="text/javascript">
                                    function changeTitle(profile) {
                                        document.title = profile.entry[0].name.givenName;
                                        $("h3").text("Howdy, " + profile.entry[0].name.givenName);
                                    }
                                </script>
                            </div>
                            <hr/>
                        </div>
                    </div>
                </div>
                <script src="https://www.gravatar.com/<% Response.Write(GetEMailHash());%>.json?callback=changeTitle" type="text/javascript"></script>
                <div class="row">
                    <div class="col-md-8 ml-auto mr-auto">
                        <h2 class="text-center title">The K means</h2>
                        <h4 class="text-center description">It works only if the database has >=10 countries.</h4>
                        <div class="description text-center">
                            <p>Country Count: <%=countryCount%></p>
                        </div>
                        <hr/>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-md-8 ml-auto mr-auto">
                        <% if (countryCount >= 10 )
                           {%>
                            
                        
                        <div style="text-align: center;" ><asp:Button ID="KmeansStep" runat="server" Text="K means Start" CssClass="btn btn-primary btn-raised" OnClick="KmeansAlgorithm" /></div>
                            
                            <% if (Kstep != 0)
                               { %>
                            <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                            <script>
                                google.charts.load('current', { 'packages': ['map'], "mapsApiKey": "AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY" });
                                google.charts.setOnLoadCallback(drawMap);

                                function drawMap() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Lat', 'Long', 'Name'],
                                        [<%= Group1CenterLat.ToString(CultureInfo.InvariantCulture) %>, <%= Group1CenterLot.ToString(CultureInfo.InvariantCulture) %>, '<%= CountryList[Group1Center].Title %>'],
                                        [<%= Group2CenterLat.ToString(CultureInfo.InvariantCulture) %>, <%= Group2CenterLot.ToString(CultureInfo.InvariantCulture) %>, '<%= CountryList[Group2Center].Title %>'],
                                    ]);

                                    var options = {
                                        showTooltip: true,
                                        showInfoWindow: true
                                    };

                                    var map = new google.visualization.Map(document.getElementById('chart_div'));

                                    map.draw(data, options);
                                };
                            </script>
                            <div id="chart_div"></div>
                            
                            <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                            <script type="text/javascript">
                                google.charts.load('current', {
                                    'packages': ['geochart'],
                                    // Note: you will need to get a mapsApiKey for your project.
                                    // See: https://developers.google.com/chart/interactive/docs/basic_load_libs#load-settings
                                    'mapsApiKey': 'AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY'
                                });
                                google.charts.setOnLoadCallback(drawRegionsMap);

                                function drawRegionsMap() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Country', 'Group'],
                                        <% for (int i = 0; i < CountryList.Count; i++)
                                           {
                                               try
                                               {


                                        %>
                                        ['<%= CountryList[i].Title %>', <%= CountryListGroup[i] %>],
                                        <% }
                                               catch
                                               {
                                               }
                                           } %>
                                    ]);

                                    var options = {
                                        colorAxis: { colors: ['red', 'black', 'green'] },
                                        backgroundColor: '#81d4fa'};

                                    var chart = new google.visualization.GeoChart(document.getElementById('regions_div'));

                                    chart.draw(data, options);
                                }
                            </script>
                            <div id="regions_div" style="width: 730px; height: 400px;"></div>
                                <% } %>

                        <% }
                           else
                           {%>
                            
                            <h4 class="text-center description">It has not enough countries in the database, please add more...</h4>
                            <br/>
                            <div style="text-align: center;" ><a href="/Home" class="btn btn-primary btn-raised">Add Country</a></div>


                        <%} %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
