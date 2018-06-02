<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="GeoData.Home" %>
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
                        <h2 class="text-center title">How it works</h2>
                        <h4 class="text-center description">Put the name of a country in the search box.</h4>
                        <div class="description text-center">
                            <p>Countries who they are called with two words such as "United States" must be separated by "_" and not by " ".<br> If the country you are looking for, exists in the Wikipedia database, we will show you the country's data. If you are ok then you can save it and see the charts of other countries.</p>
                        </div>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-md-8 ml-auto mr-auto">
                        <form class="contact-form">
                            <div class="row">
                            </div>
                            <div class="form-group bmd-form-group">
                                <link rel="stylesheet" href="//code.jquery.com/ui/1.12.0-rc.2/themes/base/jquery-ui.css">
                                <script src="//code.jquery.com/jquery-1.11.3.js"></script>
                                <script src="//code.jquery.com/ui/1.12.0-rc.2/jquery-ui.min.js"></script>
                                <script>
                                    $(function () {
                                        $("#MainContent_CountrySearch").autocomplete({
                                            source: 'CountryList.txt'
                                        }).focus(function () {
                                            $(this).autocomplete("Search", "");
                                        });
                                    })
                                </script>
                                <asp:TextBox runat="server" ID="CountrySearch" CssClass="form-control" TextMode="Search" placeholder="Insert a Country" />
                            </div>
                            <div class="row">
                                <div class="col-md-4 ml-auto mr-auto text-center">
                                    <asp:Button ID="check_if_exist" runat="server" Text="Check if exist !" CssClass="btn btn-primary btn-raised" OnClick="Check_if_exist"/>
                                </div>
                            </div>
                        </form>
                        <h3 class="text-center description"><asp:Label ID="coutry_status" runat="server" Text=""></asp:Label></h3>
                    </div>
                </div>
                

                <% if (RemoteFileExists("https://en.wikipedia.org/wiki/" + CountrySearch.Text) && !string.IsNullOrEmpty(CountrySearch.Text.Trim()) && exist)
                   { %>
                    <div style="text-align: center;" ><button type="button" id="showCoutry" class="btn btn-primary btn-raised">Show Country Data</button><button type="button" id="hideCoutry" class="btn btn-primary btn-raised" style="display: none;">Hide Country Data</button></div>
                    <br/>
                    <div id="card" style="display: none;">
                    <div class="card">
                                <div class="card-header card-header-primary">
                                    <div style="text-align: center;" ><img alt="<%=image_title%>" src="<%=image_flag%>" class="img-raised rounded img-fluid" width="125" height="83" data-file-width="600" data-file-height="400"><p>Flag</p>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <table class="table">
                                            <tbody>
                                            <tr>
                                                <td> <strong>Title</strong> </td>
                                                <td> <%= title %></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>Capital</strong> </td>
                                                <td> <%= capital %></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>Coordinates</strong> </td>
                                                <td> <%= coordinatesLant %>, <%= coordinatesLont %></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>Area</strong> </td>
                                                <td> <%= area_km2 %>&nbsp;km<sup>2</sup></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>Population</strong> </td>
                                                <td> <%= population_estimate %></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>GDP </strong><small>(per capita)</small></td>
                                                <td> <%= GDP_nominal_per_capita %></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>Gini</strong> </td>
                                                <td> <%= gini %></td>
                                            </tr>
                                            <tr>
                                                <td> <strong>HDI</strong> </td>
                                                <td> <%= hdi %></td>
                                            </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                        
                            </div>
                        <div class="col-md-4 ml-auto mr-auto text-center">
                            <asp:Button ID="AddCountry" runat="server" Text="Add Country" CssClass="btn btn-primary btn-raised" OnClick="AddCountry_OnClick"/>
                            <asp:Button ID="CancelCountry" runat="server" Text="Cancel Country" CssClass="btn btn-primary btn-raised" OnClick="CancelCountry_OnClick"/>
                        </div>
                    </div>

                    <script>
                        $(document).ready(function () {
                            $("#hideCoutry").click(function () {
                                $("#card").hide();
                                $('#showCoutry').show();
                                $('#hideCoutry').hide();
                            });
                            $("#showCoutry").click(function () {
                                $("#card").show();
                                $('#hideCoutry').show();
                                $('#showCoutry').hide();
                            });
                        });
                    </script>
                    <% } %>
                

            
            </div>
        </div>
    </div>

</asp:Content>







