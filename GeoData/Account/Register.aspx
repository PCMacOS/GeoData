<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="GeoData.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    <div class="page-header header-filter" filter-color="purple" style="background-image: url('../Image/bg7.jpg')">
    <div class="container">
      <div class="row">
        <div class="col-md-10 ml-auto mr-auto">
          <div class="card card-signup">
            <h2 class="card-title text-center">Register</h2>
            <div class="card-body">
              <div class="row">
                <div class="col-md-5 ml-auto">
                  <div class="info info-horizontal">
                    <div class="icon icon-rose">
                      <i class="material-icons">search</i>
                    </div>
                    <div class="description">
                      <h4 class="info-title"> Search countries</h4>
                      <p class="description">
                          Search for the countries of your choice in order to get the words...
                      </p>
                    </div>
                  </div>
                  <div class="info info-horizontal">
                    <div class="icon icon-primary">
                      <i class="material-icons">group_work</i>
                    </div>
                    <div class="description">
                      <h4 class="info-title">Grouping countries</h4>
                      <p class="description">
                          We will automate grouping of your chosen countries...
                      </p>
                    </div>
                  </div>
                  <div class="info info-horizontal">
                    <div class="icon icon-info">
                      <i class="material-icons">timeline</i>
                    </div>
                    <div class="description">
                      <h4 class="info-title">Statistics and Graphs</h4>
                      <p class="description">
                          And we will provide you with rich statistics and graphs for them.
                      </p>
                    </div>
                  </div>
                </div>
                <div class="col-md-5 mr-auto">
                  <div class="social text-center">
                    <br/>
                    <h4> Fill the form.. </h4>
                  </div>
                    

                    <p class="text-danger">
                        <asp:Literal runat="server" ID="ErrorMessage" />
                    </p>
                  <form class="form">
                    <div class="form-group">
                      <div class="input-group">
                        <div class="input-group-prepend">
                          <span class="input-group-text">
                            <i class="material-icons">face</i>
                          </span>
                        </div>
                          <asp:TextBox runat="server" ID="UserName" CssClass="form-control" TextMode="SingleLine" placeholder="User Name..." />
                          <div class="input-group"><asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                                      CssClass="text-danger" ErrorMessage="The User Name field is required." /></div>
                          
                      </div>
                    </div>
                    <div class="form-group">
                      <div class="input-group">
                        <div class="input-group-prepend">
                          <span class="input-group-text">
                            <i class="material-icons">mail</i>
                          </span>
                        </div>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" placeholder="Email..." />
                          <div class="input-group"><asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                                      CssClass="text-danger" ErrorMessage="The email field is required." /></div>
                      </div>
                    </div>
                    <div class="form-group">
                      <div class="input-group">
                        <div class="input-group-prepend">
                          <span class="input-group-text">
                            <i class="material-icons">lock_outline</i>
                          </span>
                        </div>
                          <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password" />
                          <div class="input-group"><asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                                      CssClass="text-danger" ErrorMessage="The password field is required." /></div>
                      </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text">
                                    <i class="material-icons">lock_outline</i>
                                </span>
                            </div>
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" placeholder="Repet Password"/>
                            <div class="input-group"><asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." /></div>
                            <div class="input-group"><asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                                  CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." /></div>
                        </div>
                    </div>
                    <div class="text-center">
                        <asp:Button runat="server" OnClick="CreateUser_Click" Text="Get Started" CssClass="btn btn-primary btn-round" />
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    
        
        
        
        <footer class="footer <%if (Request.Url.PathAndQuery == "/Home")
                                {
                                    Response.Write("footer-default");
                                } %>">
            <div class="container">
                <div class="copyright float-right">
                    &copy;
                    <script>
                        document.write(new Date().getFullYear())
                    </script>, made with <i class="material-icons">favorite</i> and 💉 by
                    <a href="https://www.psmakos.tk" target="_blank">icsd15111 & icsd15101</a> for Web Programing Class.
                </div>
            </div>
        </footer>
  </div>
    
    

    
</asp:Content>
