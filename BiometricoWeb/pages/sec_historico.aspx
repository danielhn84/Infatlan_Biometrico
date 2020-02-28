<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="sec_historico.aspx.cs" Inherits="BiometricoWeb.pages.sec_historico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
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
                        <h2>Histórico de Entradas y Salidas</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="col-12 grid-margin stretch-card">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title">Búsqueda</h4>
                <div class="row">   
                    <label class="col-2">Serie del artículo</label>
                    <div class="col-7">
                        <asp:TextBox runat="server" ID="TxBusqueda" AutoPostBack="true" CssClass="form-control" OnTextChanged="TxBusqueda_TextChanged"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Registros Creados</h4>
                        <p>Ordenados por fecha de creación</p>
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
                                    PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="Id" />
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre Entrada" />
                                        <asp:BoundField DataField="nombreSalida" HeaderText="Nombre Salida" />
                                        <asp:BoundField DataField="Articulo" HeaderText="Artículo" />
                                        <asp:BoundField DataField="serie" HeaderText="Serie" />
                                        <asp:BoundField DataField="inventario" HeaderText="No. Inventario" />
                                        <asp:BoundField DataField="fechaEntrada" HeaderText="Entrada" />
                                        <asp:BoundField DataField="fechaSalida" HeaderText="Salida" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
