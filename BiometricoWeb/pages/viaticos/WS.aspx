<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="WS.aspx.cs" Inherits="BiometricoWeb.pages.viaticos.WS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=distancia-276306&callback=initMap"
  type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

                    <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Formulario total</h4>                                                             
                                
                            </div>
                        </div>
                    </div>
                  </div>
    <asp:TextBox runat="server" ID="txtdes1"></asp:TextBox>
    <asp:TextBox runat="server" ID="txtdes2"></asp:TextBox>
    <asp:Button Text="Calcular" runat="server" ID="btnCalcular" />
    <br />
    <br />
    <asp:TextBox runat="server" ID="txtResultado"></asp:TextBox>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
