<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="salidas.aspx.cs" Inherits="BiometricoWeb.pages.salidas" %>
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
                        <h2>Salida de Equipo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Nuevo" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus" style=""> </i>Nuevo</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-clipboard" style=""> </i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Nuevo" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row"> 
                                    <label class="col-2">Serie del artículo</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBusqueda" AutoPostBack="true" OnTextChanged="TxBusqueda_TextChanged" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <asp:Label runat="server" ID="TxMensaje" ForeColor="CornflowerBlue" CssClass="col-form-label" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Resultado--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" id="DivEntradas" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="DivBody" >
                                    <h4 class="card-title">Datos de Entrada</h4>
                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">ID:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbIdEntrada" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Nombre:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbNombreEntrada" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Artículo:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbArticuloEntrada" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Serie:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbSerieEntrada" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">No. Inv:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbInventarioEntrada" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Entrada:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbFechaEntrada" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <%--Datos Salida--%>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos generales</h4>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Nombre</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxNombre" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Artículo</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DDLArticulo" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Serie</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="TxSerie" Text="" AutoPostBack="true" CssClass="form-control"></asp:TextBox>                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">No. Inventario</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox runat="server" ID="TxInventario" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Motivo</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DDLMotivo" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Observaciones</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox runat="server" ID="TxObservaciones" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">   
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Autorizado Por</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLAutorizado"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Grabar Salida--%>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="BtnGuardar" Text="Guardar" CssClass="btn btn-success" OnClick="BtnGuardar_Click"/>
                                <asp:Button runat="server" ID="BtnCancelar" Text="Cancelar" CssClass="btn btn-secondary" OnClick="BtnCancelar_Click"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="nav-Registros" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row">   
                                    <label class="col-2">Serie del artículo</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscaSerie" AutoPostBack="true" CssClass="form-control" OnTextChanged="TxBuscaSerie_TextChanged"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Salidas Creadas</h4>
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
                                                <asp:BoundField DataField="id" HeaderText="No." />
                                                <asp:BoundField DataField="nombreSalida" HeaderText="Nombre" />
                                                <asp:BoundField DataField="articulo" HeaderText="Artículo" />
                                                <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                <asp:BoundField DataField="inventario" HeaderText="No. Inventario" />
                                                <asp:BoundField DataField="fechaSalida" HeaderText="Fecha" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
