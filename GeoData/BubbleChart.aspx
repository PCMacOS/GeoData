<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BubbleChart.aspx.cs" Inherits="GeoData.BubbleChart" %>
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
                        <h2 class="text-center title">The K means Bubble Chart</h2>
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
                           { %>
                            <div class="col-lg-3 col-md-4 col-sm-6">
                                <div class="title">
                                    <h3>Countres</h3>
                                </div>
                                <% for (int i = 0; i < CountryList.Count; i++)
                                   { %>
                                    <div class="form-check">
                                        <label class="form-check-label" >
                                            <input class="form-check-input" type="checkbox" id="<%= CountryList[i].Title %>" value="<%= CountryList[i].Title %>" checked>
                                            <%= CountryList[i].Title %>
                                            <span class="form-check-sign">
                                                <span class="check"></span>
                                            </span>
                                        </label>
                                    </div>
                                <% } %>
                            </div>
                            <form id="OptionForm" >
                                <div class="col-lg-3 col-md-4 col-sm-6">
                                <div class="title">
                                    <h3>Criterion 1</h3>
                                </div>
                            <select id="Option1Select" runat="server">
                                <option value="7">GDP</option>
                                <option value="8">Gini</option>
                                <option value="9">Hid</option>
                            </select>
                                </div>
                                <div class="col-lg-3 col-md-4 col-sm-6">
                                <div class="title">
                                    <h3>Criterion 2</h3>
                                </div>
                            <select id="Option2Select" runat="server">
                                <option value="7">GDP</option>
                                <option value="8">Gini</option>
                                <option value="9">Hid</option>
                            </select>
                                </div>
                                <div style="text-align: center;" ><asp:Button ID="SelectOption" runat="server" Text="Select the options." CssClass="btn btn-primary btn-raised" onclick="SelectOptions_Click" /></div>
                            </form>
                            <%if (optionSelected){ %>
                            <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                            <script type="text/javascript">
                                google.charts.load('current', { 'packages': ['corechart'] });
                                google.charts.setOnLoadCallback(drawSeriesChart);

                                function drawSeriesChart() {

                                    var data = google.visualization.arrayToDataTable([
                                        ['ID', '<%= option1Title %>', '<%= option2Title %>', 'Group', 'Population'],
                                   <% for (int i = 0; i < CountryList.Count; i++)
                                      { %>
                                        ['<%= CountryList[i].Title %>', <%= Option1List[i].Replace(",", ".") %>, <%= Option2List[i].Replace(",", ".") %>, <%= CountryListGroup[i] %>, <%= CountryList[i].PopulationEstimate.Replace(",", "") %>],
                                   <% } %>
                                    ]);

                                    var options = {
                                        colorAxis: { colors: ['red', 'green'] },
                                        hAxis: { title: '<%= option1Title %>' },
                                        vAxis: { title: '<%= option2Title %>' },
                                        bubble: { textStyle: { fontSize: 11 } }
                                    };

                                    var chart = new google.visualization.BubbleChart(document.getElementById('series_chart_div'));
                                    chart.draw(data, options);
                                }
                            </script>
                            <div style="text-align: center;" ><div id="series_chart_div" style="width: 900px; height: 500px;"></div></div>
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

