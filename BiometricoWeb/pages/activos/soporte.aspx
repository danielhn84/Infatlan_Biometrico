<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="soporte.aspx.cs" Inherits="BiometricoWeb.pages.activos.soporte" %>
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
                        <h2>Soporte Técnico</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Entradas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus" style=""> </i>Entrada</a>
                <a class="nav-item nav-link" id="nav_salidas" data-toggle="tab" href="#nav-Salidas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus" style=""> </i>Salida</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-database" style=""> </i>Registros</a>
                <a class="nav-item nav-link" id="nav_autorizaciones_tab" data-toggle="tab" href="#nav-Aprobaciones" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-checkbox-marked-circle" style=""> </i>Aprobaciones</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Entradas" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row mb-2"> 
                                    <label class="col-2 col-form-label">Proceso</label>
                                    <div class="col-7">
                                        <asp:DropDownList runat="server" ID="DDLProceso" CssClass="form-control" OnSelectedIndexChanged="DDLProceso_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="1" Text="Personal Interno"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Visitas"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Data Center"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Soporte Técnico" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

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
                    <div runat="server" id="DivSalidas" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="DivBody" >
                                    <h4 class="card-title">Datos de Salida</h4>
                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">ID:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbIdSalida" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Nombre:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbNombreSalida" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Artículo:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbArticuloSalida" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Serie:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbSerieSalida" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">No. Inv.</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbInventarioSalida" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Salida:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbFechaSalida" Font-Bold="true"></asp:Label>
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

            <%--Datos Entrada--%>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
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
                                                <asp:DropDownList ID="DDLArticulos" runat="server" class="form-control"></asp:DropDownList>
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
                                    <div class="col-md-6">
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
                                            <label class="col-3">Departamento</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DDLDepartamento" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Destinatario</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxDestinatario" CssClass="form-control"></asp:TextBox>
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
                                            <div class="col-9">
                                                <asp:TextBox runat="server" TextMode="MultiLine" ID="TxObservaciones" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Grabar Entrada--%>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Crear Entrada</h4>
                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnGuardar" class="btn btn-success" runat="server" Text="Crear" OnClick="BtnGuardar_Click" />
                                    <asp:Button ID="BtnCancelar" class="btn btn-secondary" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="nav-Salidas" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row"> 
                                    <label class="col-2 col-form-label">Serie del artículo</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBusquedaSalida" AutoPostBack="true" OnTextChanged="TxBusquedaSalida_TextChanged" CssClass="form-control"></asp:TextBox>
                                        <b><asp:Label runat="server" ID="LbAprobacion" Visible="false" CssClass="col-form-label" Text=""></asp:Label></b>
                                    </div>
                                    <asp:Label runat="server" ID="LbMensajeSalida" ForeColor="CornflowerBlue" CssClass="col-form-label" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Resultado--%>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" id="DivEntradas" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="DivBodySalida" >
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
            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos generales</h4>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2 col-form-label">Nombre</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxNombreSalida" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Artículo</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DDLArticuloSalida" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2 col-form-label">Serie</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="TxSerieSalida" Text="" AutoPostBack="true" CssClass="form-control"></asp:TextBox>                                                
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">No. Inventario</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox runat="server" ID="TxInventarioSalida" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2 col-form-label">Motivo</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DDLMotivoSalida" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Observaciones</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox runat="server" ID="TxObservacionesSalida" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
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
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="BtnGuardarSalida" Text="Guardar" CssClass="btn btn-success" OnClick="BtnGuardarSalida_Click"/>
                                <asp:Button runat="server" ID="BtnCancelarSalida" Text="Cancelar" CssClass="btn btn-secondary" OnClick="BtnCancelarSalida_Click"/>
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
                                <h4 class="card-title">Entradas Creadas</h4>
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
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                                <asp:BoundField DataField="articulo" HeaderText="Artículo" />
                                                <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                <asp:BoundField DataField="inventario" HeaderText="No. Inventario" />
                                                <asp:BoundField DataField="destinatario" HeaderText="Destinatario" />
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
        </div>

        <div class="tab-pane fade" id="nav-Aprobaciones" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Aprobaciones de Salida</h4>
                                <p>Ordenados por fecha de creación</p>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GVAprobaciones" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None" OnRowCommand="GVAprobaciones_RowCommand"
                                            PageSize="10" OnPageIndexChanging="GVAprobaciones_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                                                <asp:BoundField DataField="Empleado" HeaderText="Nombre" />
                                                <asp:BoundField DataField="Articulo" HeaderText="Artículo" />
                                                <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                <asp:BoundField DataField="observaciones" HeaderText="observaciones" />
                                                <asp:BoundField DataField="fecha" HeaderText="Fecha" />
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
