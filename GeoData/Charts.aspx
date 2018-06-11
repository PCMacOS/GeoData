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
                            <form id="OptionForm" >
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

                        <div class="col-lg-3 col-md-4 col-sm-6">
                            <div class="title">
                                <h3>Criterion</h3>
                            </div>
                            <select id="CriterionSelect" runat="server">
                                <option value="5">Area Km2</option>
                                <option value="6">Population (estimate)</option>
                                <option value="7">GDP</option>
                                <option value="8">Gini</option>
                                <option value="9">Hid</option>
                            </select>
                            <%--<% string criterionTitle = "";
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
                             <% } %>--%>

                        </div>

                        <div class="col-lg-3 col-md-4 col-sm-6">
                            <div class="title">
                                <h3>Charts</h3>
                            </div>
                                <select id="ChartSelect" runat="server">
                                    <option value="1">Bar Chart</option>
                                    <option value="2">Pie Chart</option>
                                    <option value="3">Column Chart</option>
                                </select>
                        </div>
                                <div style="text-align: center;" ><asp:Button ID="SelectChart" runat="server" Text="Select Chart" CssClass="btn btn-primary btn-raised" OnClick="SelectChart_OnClick" /></div>
                            </form>
                        
                            <%if (optionSelected){ %>
                                <%if (Chart == 1){ %>
                            <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                            <script>
                                google.charts.load('current', { packages: ['corechart', 'bar'] });
                                google.charts.setOnLoadCallback(drawMultSeries);

                                function drawMultSeries() {
                                    var data = google.visualization.arrayToDataTable([
                                        ['Country', '<%= CriterionTitle %>'],
                                        <% for (int i = 0; i < CountryList.Count; i++)
                                      { %>
                                        ['<%= CountryList[i].Title %>', <%= CriterionList[i].Replace(",", ".") %>],
                                        <% } %>
                                    ]);

                                    var options = {
                                        title: 'Total <%= ChartTitle %>',
                                        chartArea: { width: '50%' },
                                        hAxis: {
                                            title: '<%= CriterionTitle %>',
                                            minValue: 0
                                        },
                                        vAxis: {
                                            title: 'Country'
                                        }
                                    };

                                    var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
                                    chart.draw(data, options);
                                }
                            </script>
                            <div id="chart_div" style="width: 900px; height: 500px;"></div>
                                <% } %>
                                
                                <%if (Chart == 2){ %>
                                    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                                    <script>
                                        google.charts.load('current', { 'packages': ['corechart'] });
                                        google.charts.setOnLoadCallback(drawChart);

                                        function drawChart() {

                                            var data = google.visualization.arrayToDataTable([
                                                ['Country', '<%= CriterionTitle %>'],
                                                <% for (int i = 0; i < CountryList.Count; i++)
                                           { %>
                                                ['<%= CountryList[i].Title %>', <%= CriterionList[i].Replace(",", ".") %>],
                                                <% } %>
                                            ]);

                                            var options = {
                                                title: '<%= ChartTitle %> (<%= CriterionTitle %>)'
                                            };

                                            var chart = new google.visualization.PieChart(document.getElementById('piechart'));

                                            chart.draw(data, options);
                                        }
                                    </script>
                                    <div id="piechart" style="width: 900px; height: 500px;"></div>
                                <% } %>
                                
                                <%if (Chart == 3){ %>
                                    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
                                    <script>
                                        google.charts.load('current', { packages: ['corechart', 'bar'] });
                                        google.charts.setOnLoadCallback(drawBasic);

                                        function drawBasic() {

                                            var data = new google.visualization.DataTable();
                                            data.addColumn('string', 'Time of Day');
                                            data.addColumn('number', '<%= CriterionTitle %>');

                                            data.addRows([
                                                <% for (int i = 0; i < CountryList.Count; i++)
                                           { %>
                                                ['<%= CountryList[i].Title %>', <%= CriterionList[i].Replace(",", ".") %>],
                                                <% } %>
                                            ]);

                                            var options = {
                                                title: '<%= ChartTitle %>',
                                                hAxis: {
                                                    title: 'Country'
                                                },
                                                vAxis: {
                                                    title: '<%= CriterionTitle %>'
                                                }
                                            };

                                            var chart = new google.visualization.ColumnChart(
                                                document.getElementById('chart_div'));

                                            chart.draw(data, options);
                                        }
                                    </script>
                                    <div id="chart_div" style="width: 900px; height: 500px;"></div>
                                <% } %>
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



