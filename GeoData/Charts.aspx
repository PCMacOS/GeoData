<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Charts.aspx.cs" Inherits="GeoData.Charts" %>
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
                        <h2 class="text-center title">The Charts</h2>
                        <h4 class="text-center description">It works only if the database has >=10 countries.</h4>
                        <div class="description text-center">
                            <p>Country Count: <%=countryCount%></p>
                        </div>
                        <hr/>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-md-8 ml-auto mr-auto">
                        <% if (countryCount >= 10)
                           {%>
                            
                        <div class="col-lg-3 col-md-4 col-sm-6">
                            <div class="title">
                                <h3>Countres</h3>
                            </div>
                            <% for (int i = 0; i < CountryList.Count; i++)
                               { %>
                            <div class="form-check">
                                <label class="form-check-label">
                                    <input class="form-check-input" type="checkbox" id="<%= CountryList[i].Title %>" value="<%= CountryList[i].Title %>">
                                    <%= CountryList[i].Title %>
                                <span class="form-check-sign">
                                    <span class="check"></span>
                                </span>
                                </label>
                            </div>
                            <% } %>
                        </div>

                        <div class="col-lg-3 col-md-4 col-sm-6">
                            <div class="title">
                                <h3>Criterion</h3>
                            </div>
                            <% string criterionTitle = "";
                                for (int i = 5; i < 10; i++)
                                {
                                    if (i == 5) criterionTitle = "Area Km<sup>2</sup>";
                                    if (i == 6) criterionTitle = "Population (estimate)";
                                    if (i == 7) criterionTitle = "GDP (nominal per capita)";
                                    if (i == 8) criterionTitle = "Gini";
                                    if (i == 9) criterionTitle = "HID";
                            %>
                            <div class="form-check">
                                <label class="form-check-label">
                                    <input class="form-check-input" type="radio" name="exampleRadios" id="exampleRadios1" value="option1">
                                    <%=criterionTitle %>
                                <span class="circle">
                                    <span class="check"></span>
                                </span>
                                </label>
                            </div> 
                             <% } %>

                        </div>

                        <div class="col-lg-3 col-md-4 col-sm-6">
                            <div class="title">
                                <h3>Toggle Buttons</h3>
                            </div>

                            <div class="togglebutton">
                                <label>
                                    <input type="checkbox" checked="">
                                    <span class="toggle"></span>
                                    Toggle is on
                                </label>
                            </div>
                            <div class="togglebutton">
                                <label>
                                    <input type="checkbox">
                                    <span class="toggle"></span>
                                    Toggle is off
                                </label>
                            </div>
                        </div><div style="text-align: center;" ><asp:Button ID="SelectChart" runat="server" Text="Select Chart" CssClass="btn btn-primary btn-raised" OnClick="SelectChart_OnClick" /></div>
                            
                            <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                            <script>
                                google.charts.load('current', { 'packages': ['map'], "mapsApiKey": "AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY"});
                                google.charts.setOnLoadCallback(drawMap);

                                function drawMap() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Country', 'Population'],
                                        ['China', 'China: 1,363,800,000'],
                                        ['India', 'India: 1,242,620,000'],
                                        ['US', 'US: 317,842,000'],
                                        ['Indonesia', 'Indonesia: 247,424,598'],
                                        ['Brazil', 'Brazil: 201,032,714'],
                                        ['Pakistan', 'Pakistan: 186,134,000'],
                                        ['Nigeria', 'Nigeria: 173,615,000'],
                                        ['Bangladesh', 'Bangladesh: 152,518,015'],
                                        ['Russia', 'Russia: 146,019,512'],
                                        ['Japan', 'Japan: 127,120,000']
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



