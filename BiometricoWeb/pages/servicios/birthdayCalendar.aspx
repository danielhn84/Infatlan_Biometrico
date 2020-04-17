<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="birthdayCalendar.aspx.cs" Inherits="BiometricoWeb.pages.servicios.birthdayCalendar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Cumpleañeros</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h1 class="card-title">CALENDARIO DE CUMPLEAÑOS</h1>
                    <b>
                        <p class="mb-md-0 card-description" style="font-weight: 500;">¡Es un gusto servirte!</p>
                    </b>
                    <p class="card-description">A nombre de toda la empresa te hacemos llegar este saludo para desearte un feliz cumpleaños. Gracias por tu apoyo y tu amistad</p>
                   <%-- <br />--%>

                    <div class="card-group col-md-11 text-center" style="width: auto; margin: auto auto;">
                        <div class="col-md-12">
                            <div class="form-group row">
                                <label class="col-md-3 col-form-label">Búsqueda por Mes:</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlMes" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged">
                                        <asp:ListItem Value="01" Text="SELECCIONE UN MES"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="ENERO"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="FEBRERO"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="MARZO"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="ABRIL"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="MAYO"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="JUNIO"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="JULIO"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="AGOSTO"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="SEPTIEMBRE"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="OCTUBRE"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="NOVIEMBRE"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="DICIEMBRE"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <center>
	                     <img src="../../images/bannerCumple.png" width="400" height="300" >
                    </center>

                    <asp:UpdatePanel ID="UpTitulo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="card-group col-md-11  text-lg-center" style="width: auto; margin: auto auto;">
                                <div class="card border-0">
                                    <div style="background-color: cornflowerblue">
                                        <h2 style="color: white"><strong>
                                            <asp:Label ID="Titulo" runat="server" Text=""></asp:Label></strong></h2>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="card-group col-md-11  text-center" style="width: auto; margin: auto auto;">
                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>LUNES</strong></h5>
                            </div>
                        </div>

                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>MARTES</strong></h5>
                            </div>
                        </div>

                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>MIERCOLES</strong></h5>
                            </div>
                        </div>

                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>JUEVES</strong></h5>
                            </div>
                        </div>

                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>VIERNES</strong></h5>
                            </div>
                        </div>

                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>SABADO</strong></h5>
                            </div>
                        </div>

                        <div class="card border-0">
                            <div style="background-color: cornflowerblue">
                                <h5 style="color: white"><strong>DOMINGO</strong></h5>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-11" style="width: auto; margin: auto auto;">
                        <div class="card gainsboro  border-left border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img1" runat="server" visible="true">
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label1" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center ">
                                            <div>
                                                <div>
                                                    <img id="Img2" runat="server" visible="true" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label2" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img3" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label3" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img4" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label4" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img5" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label5" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img6" runat="server" />
                                                </div>
                                                <div class="ml-auto">
                                                    <asp:Label ID="Label6" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img7" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label7" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-11" style="width: auto; margin: auto auto;">

                        <div class="card gainsboro  border-left border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img8" runat="server" />
                                                </div>
                                                <div class="ml-auto">
                                                    <asp:Label ID="Label8" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img9" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label9" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img10" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label10" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img11" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label11" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img12" runat="server" />

                                                </div>
                                                <div class="ml-auto">
                                                    <asp:Label ID="Label12" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img13" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label13" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img14" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label14" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-11" style="width: auto; margin: auto auto;">

                        <div class="card gainsboro  border-left border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img15" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label15" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img16" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label16" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img17" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label17" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img18" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label18" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img19" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label19" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img20" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label20" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img21" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label21" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-11" style="width: auto; margin: auto auto;">
                        <div class="card gainsboro  border-left border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img22" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label22" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img23" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label23" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img24" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label24" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img25" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label25" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img26" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label26" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img27" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label27" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom">
                            <div class="card-body ">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img28" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label28" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-11" style="width: auto; margin: auto auto;">

                        <div class="card gainsboro  border-left border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <%--<h2 class="counter text-primary">23</h2>--%>
                                                    <img id="Img29" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label29" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <%--    <h2 class="counter text-primary">23</h2>--%>
                                                    <img id="Img30" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label30" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <%--  <h2 class="counter text-primary">23</h2>--%>
                                                    <img id="Img31" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label31" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <%--       <h2 class="counter text-primary">23</h2>--%>
                                                    <img id="Img32" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label32" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img33" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label33" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img34" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label34" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom  ">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img35" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label35" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-11" style="width: auto; margin: auto auto;">

                        <div class="card gainsboro  border-left border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img36" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label36" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img37" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label37" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <%--  <h2 class="counter text-primary">23</h2>--%>
                                                    <img id="Img38" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label38" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card gainsboro border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <%--       <h2 class="counter text-primary">23</h2>--%>
                                                    <img id="Img39" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label39" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img40" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label40" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img41" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label41" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card gainsboro border-bottom">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="d-flex no-block align-items-center">
                                            <div>
                                                <div class="ml-auto">
                                                    <img id="Img42" runat="server" />
                                                </div>

                                                <div class="ml-auto">
                                                    <asp:Label ID="Label42" runat="server" Text="" Font-Size="Small"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-group col-md-11  text-lg-center" style="width: auto; margin: auto auto;">
                        <div class="card">
                            <div style="background-color: cornflowerblue">
                                <h2 style="color: white"><strong>
                                    <asp:Label ID="Label43" runat="server" Text=""></asp:Label></strong></h2>
                            </div>
                        </div>
                    </div>
  
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
