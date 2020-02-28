<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="politicas.aspx.cs" Inherits="BiometricoWeb.pages.politicas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }

        function showMe() {
            var x = document.getElementById("DivVestimenta");
            if (x.style.display === "none") {
                x.style.display = "block";
            }
            $('a#LIVestimenta').attr({
                target: '_blank',
                href: 'plantilla/PoliticaVestimenta.pdf'
            });

            //window.open('plantilla/PoliticaVestimenta.pdf');
        }

    </script>
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #ffffff; opacity: 0.7; margin: 0;">
                <span style="display: inline-block; height: 100%; vertical-align: middle;"></span>
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="/images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="display: inline-block; vertical-align: middle;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Politicas</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="col-12 grid-margin stretch-card">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title">POLITICAS INGRESADAS</h4>
                <p style="font-weight:500;" class="card-description">
                    Lea el documento y confirme que leyó las políticas.
                </p>

                <div class="col-md-12">
                    <asp:UpdatePanel runat="server" ID="UPPoliticas">
                        <ContentTemplate>
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:GridView ID="GVBusqueda" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pgr"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false"
                                        AllowPaging="true"
                                        GridLines="None"
                                        PageSize="10" OnRowCommand="GVBusqueda_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="idPolitica" HeaderText="No." Visible="false"  />
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                            <asp:BoundField DataField="estadoLeido" HeaderText="Estado" />
                                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha de Creación" />
                                            <asp:TemplateField HeaderText="Ver" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Button ID="BtnVer" runat="server" Text="Ver" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idPolitica") %>' CommandName="verPolitica" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
