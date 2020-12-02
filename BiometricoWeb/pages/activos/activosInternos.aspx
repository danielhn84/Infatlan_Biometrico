<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="activosInternos.aspx.cs" Inherits="BiometricoWeb.pages.activos.activosInternos" %>
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
                        <h2>Personal Interno</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_in" data-toggle="tab" href="#nav-Entradas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-login" style=""> </i>Entrada</a>
                <a class="nav-item nav-link" id="nav_out" data-toggle="tab" href="#nav-Salidas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-logout" style=""> </i>Salida</a>
                <a class="nav-item nav-link" id="nav_assigns" data-toggle="tab" href="#nav-Asignaciones" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-account-check" style=""> </i>Asignaciones</a>
                <a class="nav-item nav-link" id="nav_history" data-toggle="tab" href="#nav-Historico" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-database" style=""> </i>Historial</a>
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
                                <div class="row"> 
                                    <label class="col-2 col-form-label">Serie del artículo</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBusqueda" AutoPostBack="true" OnTextChanged="TxBusqueda_TextChanged" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="DivResultado" runat="server" visible="false" class="mt-2">
                                    <div class="row"> 
                                        <div class="col-2"></div>
                                        <div class="col-7">
                                            <asp:Label runat="server" Text="" ForeColor="Tomato" ID="LbMensaje" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Resultado--%> 
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" id="DivInfoIN" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="DivBody" >
                                    <h4 class="card-title">Datos del Encargado</h4>
                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Nombre:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbNombre" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">ID Equipo:</label>
                                                <div class="col-5">
                                                    <asp:Label runat="server" Text="" ID="LbIdEquipoEnt" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Tipo:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbTipo" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Marca:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbMarca" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
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
                                                    <asp:Label runat="server" Text="" ID="LbCodInventario" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div runat="server" id="DivEquipoPersonal" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="Div6" >
                                    <h4 class="card-title">Equipo Personal</h4>
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3">Empleado:</label>
                                                <div class="col-9">
                                                    <asp:DropDownList runat="server" ID="DDLEmpleado" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3">Serie:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxSerie" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3">Categoría:</label>
                                                <div class="col-9">
                                                    <asp:DropDownList runat="server" ID="DDLCategoria" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLCategoria_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3">Tipo de Equipo:</label>
                                                <div class="col-9">
                                                    <asp:DropDownList runat="server" ID="DDLTipo" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3">Marca:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxMarca" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3">Modelo:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxModelo" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div runat="server" id="DivRegistrar" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">REGISTRAR INFORMACION</h4>
                                    <div class="row">
                                        <div class="col-12">
                                            <asp:Button ID="BtnGuardar" class="btn btn-success" runat="server" Text="Registrar" OnClick="BtnGuardar_Click" />
                                            <asp:Button ID="BtnCancelar" class="btn btn-secondary" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="tab-pane fade" id="nav-Salidas" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row"> 
                                    <label class="col-2 col-form-label">Serie del artículo</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscarSalida" AutoPostBack="true" OnTextChanged="TxBuscarSalida_TextChanged" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="DivRegistroOut" runat="server" visible="false" class="mt-2">
                                    <div class="row"> 
                                        <div class="col-2"></div>
                                        <div class="col-7">
                                            <asp:Label runat="server" Text="" ForeColor="Tomato" ID="LbMensajeOut" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Resultado--%> 
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" id="DivInfoOUT" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="Div3" >
                                    <h4 class="card-title">Datos del Encargado</h4>
                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Nombre:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbNombreOut" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">ID Equipo:</label>
                                                <div class="col-5">
                                                    <asp:Label runat="server" Text="" ID="LbIdEquipoOut" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Tipo:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbTipoOut" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Marca:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbMarcaOut" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Serie:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbSerieOut" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">No. Inv.</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbInventarioOut" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class ="row">   
                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Fecha Ingreso</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" Text="" ID="LbFechaIn" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="DivRegistroSalida" runat="server" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">REGISTRAR INFORMACION</h4>
                                    <div class="row">
                                        <div class="col-12">
                                            <asp:Button ID="BtnGuardarOut" class="btn btn-success" runat="server" Text="Registrar" OnClick="BtnGuardarOut_Click" />
                                            <asp:Button ID="BtnCancelarOut" class="btn btn-secondary" runat="server" Text="Cancelar" OnClick="BtnCancelarOut_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="tab-pane fade" id="nav-Asignaciones" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row">   
                                    <label class="col-2 col-form-label">Nombre del Empleado</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscaAsignacion" AutoPostBack="true" CssClass="form-control" OnTextChanged="TxBuscaAsignacion_TextChanged"></asp:TextBox>
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
                                <h4 class="card-title">Equipo Asignado a Empleados</h4>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvAsignaciones" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="align-self-lg-start"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None"
                                            PageSize="10" OnPageIndexChanging="GvAsignaciones_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="idEmpleado" HeaderText="No." Visible="false" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="tipoEquipo" HeaderText="Equipo" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="serie" HeaderText="Serie" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="CodInventario" HeaderText="No. Inventario" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="modelo" HeaderText="Modelo" ItemStyle-HorizontalAlign="Left"/>
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

        <div class="tab-pane fade" id="nav-Historico" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row">   
                                    <label class="col-2 col-form-label">Nombre del Empleado</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscaHistorico" AutoPostBack="true" CssClass="form-control" OnTextChanged="TxBuscaHistorico_TextChanged"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Registro de entradas y salidas</h4>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvHistorico" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="align-self-lg-start"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None"
                                            PageSize="10" OnPageIndexChanging="GvHistorico_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="idEquipo" HeaderText="No." Visible="false" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CodInventario" HeaderText="Inventario" Visible="false" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="empleado" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="tipoEquipo" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="serie" HeaderText="Serie" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="CodInventario" HeaderText="No. Inventario" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="marca" HeaderText="Marca" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="accion" HeaderText="Accion" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" ItemStyle-HorizontalAlign="Left"/>
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
