<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="employees.aspx.cs" Inherits="BiometricoWeb.pages.employees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function openEmpleadosModal() {
            $('#EmpleadoModal').modal('show');
        }

        function openPasswordModal() {
            $('#PasswordModal').modal('show');
        }

        var url = document.location.toString();
        if (url.match('#')) {
            $('.nav-tabs a[href="#' + url.split('#')[1] + '"]').tab('show');
        }

        $('.nav-tabs a').on('shown.bs.tab', function (e) {
            window.location.hash = e.target.hash;
        })
    </script>

    <script>
        $(window).on('resize', function () {
            $('body').children('.blockMsg').css({
                top: ($(window).height() - 40) / 2 + 'px',
                left: ($(window).width() - 200) / 2 + 'px'
            });
        });
    </script>
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
                        <h2>Mantenimiento Empleados</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                </div>
            </div>
        </div>
    </div>
    <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-datos" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus" > </i>Crear Empleados</a>
            <a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book" > </i>Modificaciones</a>
        </div>
    </nav>
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-datos" role="tabpanel" aria-labelledby="nav-datos-tab">
            <br />
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Información general del empleado</h4>
                            <p class="card-description">
                                Ingrese los campos requeridos               
                            </p>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">No. Empleado</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearNoEmpleado" placeholder="ej. 80000" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Identidad</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearIdentidad" placeholder="ej. 0801190000123" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Codigo SAP</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearCodigoSAP" placeholder="ej.80000000" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Ciudad</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLCrearCiudad" runat="server" class="form-control">
                                                        <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                                        <asp:ListItem Value="CHOLUTECA">CHOLUTECA</asp:ListItem>
                                                        <asp:ListItem Value="COMAYAGUA">COMAYAGUA</asp:ListItem>
                                                        <asp:ListItem Value="DANLI">DANLI</asp:ListItem>
                                                        <asp:ListItem Value="JUTICALPA">JUTICALPA</asp:ListItem>
                                                        <asp:ListItem Value="LA CEIBA">LA CEIBA</asp:ListItem>
                                                        <asp:ListItem Value="ROATÁN">ROATÁN</asp:ListItem>
                                                        <asp:ListItem Value="SAN PEDRO SULA">SAN PEDRO SULA</asp:ListItem>
                                                        <asp:ListItem Value="SANTA ROSA DE C">SANTA ROSA DE C</asp:ListItem>
                                                        <asp:ListItem Value="SIGUATEPEQUE">SIGUATEPEQUE</asp:ListItem>
                                                        <asp:ListItem Value="TEGUCIGALPA">TEGUCIGALPA</asp:ListItem>
                                                        <asp:ListItem Value="TEGUCIGALPA BA">TEGUCIGALPA BA</asp:ListItem>
                                                        <asp:ListItem Value="TOCOA">TOCOA</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Nombre</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearNombre" placeholder="ej. Carlos Jose Hernandez" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div> 
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Area</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLCrearArea" runat="server" class="form-control">
                                                        <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                                        <asp:ListItem Value="CONVENIO INFATLAN BASA">CONVENIO INFATLAN BASA</asp:ListItem>
                                                        <asp:ListItem Value="DESARROLLO Y MANTENIMIENTO">DESARROLLO Y MANTENIMIENTO</asp:ListItem>
                                                        <asp:ListItem Value="GERENCIA GENERAL">GERENCIA GENERAL</asp:ListItem>
                                                        <asp:ListItem Value="OPERACIONES TI">OPERACIONES TI</asp:ListItem>
                                                        <asp:ListItem Value="PROYECTOS Y PROCESOS">PROYECTOS Y PROCESOS</asp:ListItem>
                                                        <asp:ListItem Value="SOPORTE Y COMUNICACIONES">SOPORTE Y COMUNICACIONES</asp:ListItem>
                                                        <asp:ListItem Value="SUBGERENCIA ADMINISTRATIVO">SUBGERENCIA ADMINISTRATIVO</asp:ListItem>
                                                        <asp:ListItem Value="TALENTO HUMANO">TALENTO HUMANO</asp:ListItem>
                                                        <asp:ListItem Value="TALENTO HUMANO">CALIDAD Y CUMPLIMIENTO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Nacimiento</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearFechaNacimiento" placeholder="" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Telefono</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearTelefono" placeholder="ej. 99900012" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Email Empresa</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearEmailEmpresa" placeholder="ej. test@bancatlan.hn" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Email Personal</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxCrearEmailPersonal" placeholder="ej. test@hotmail.com" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Turnos</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLTurnos" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Puesto</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLPuestos" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Jefe</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLJefatura" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">ADUser</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxAdUser" placeholder="ej. egutierrez" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Dirección</h4>
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <label for="exampleInputPassword4">Descripcion del Empleado</label>
                                        <asp:TextBox ID="TxCrearDireccion" placeholder="..." class="form-control" runat="server" TextMode="MultiLine" Style="height: 100px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Comentarios</h4>
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <label for="exampleInputPassword4">Comentarios del Empleado</label>
                                        <asp:TextBox ID="TxCrearComentarios" placeholder="..." class="form-control" runat="server" TextMode="MultiLine" Style="height: 100px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Configuraciones</h4>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Biometrico</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLCrearRelojes" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Privilegio</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLCrearRole" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Normal</asp:ListItem>
                                                <asp:ListItem Value="1">Enroll</asp:ListItem>
                                                <asp:ListItem Value="2">Admin</asp:ListItem>
                                                <asp:ListItem Value="3">Super Admin</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Guardar Empleado</h4>
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnGuardarCambio" class="btn btn-primary mr-2" runat="server" Text="Crear Empleado" OnClick="BtnGuardarCambio_Click" />
                                        <asp:Button ID="BtnCancelarCambio" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelarCambio_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <p>
                                Al guardar el empleado tambien se crea el registro en el reloj selecccionado
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Empleados creados</h4>

                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Buscar</label>
                                            <div class="col-sm-9">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxBuscarEmpleado" runat="server" placeholder="Ej. Elvin - Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscarEquipo_TextChanged"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel ID="UpdateGridView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GVBusqueda" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pgr"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false"
                                                        AllowPaging="true"
                                                        GridLines="None"
                                                        PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging" OnRowCommand="GVBusqueda_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px" Visible="false">
                                                                <HeaderTemplate>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnUsuarioPassword" runat="server" Text="Password" class="btn btn-inverse-primary " CommandArgument='<%# Eval("idEmpleado") %>' CommandName="UsuarioPassword" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                                                <HeaderTemplate>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnUsuarioModificar" runat="server" Text="Modificar" class="btn btn-inverse-primary  mr-2" CommandArgument='<%# Eval("idEmpleado") %>' CommandName="UsuarioModificar" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="idEmpleado" HeaderText="No." />
                                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                                            <asp:BoundField DataField="area" HeaderText="Area" />
                                                            <asp:BoundField DataField="ciudad" HeaderText="Ciudad" Visible="false" />
                                                            <asp:BoundField DataField="identidad" HeaderText="Identidad" Visible="false" />
                                                            <asp:BoundField DataField="telefono" HeaderText="Telefono" Visible="false" />
                                                            <asp:BoundField DataField="estado" HeaderText="Estado" Visible="false" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--MODAL DE MODIFICACION--%>
    <div class="modal fade" id="EmpleadoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 1000px; top: 370px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelModificacion">

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                Modificar Empleado
                                <asp:Label ID="LbModNoEmpleado" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateModificarUsuario" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Estado</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLEstado" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                                <asp:ListItem Value="True">Activado</asp:ListItem>
                                                <asp:ListItem Value="False">Desactivado</asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Identidad</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModIdentidad" placeholder="ej. 0801190000123" class="form-control" runat="server"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Codigo SAP</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModCodigoSAP" placeholder="ej.80000000" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Ciudad</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModCiudad" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                                <asp:ListItem Value="CHOLUTECA">CHOLUTECA</asp:ListItem>
                                                <asp:ListItem Value="COMAYAGUA">COMAYAGUA</asp:ListItem>
                                                <asp:ListItem Value="DANLI">DANLI</asp:ListItem>
                                                <asp:ListItem Value="JUTICALPA">JUTICALPA</asp:ListItem>
                                                <asp:ListItem Value="LA CEIBA">LA CEIBA</asp:ListItem>
                                                <asp:ListItem Value="ROATÁN">ROATÁN</asp:ListItem>
                                                <asp:ListItem Value="SAN PEDRO SULA">SAN PEDRO SULA</asp:ListItem>
                                                <asp:ListItem Value="SANTA ROSA DE C">SANTA ROSA DE C</asp:ListItem>
                                                <asp:ListItem Value="SIGUATEPEQUE">SIGUATEPEQUE</asp:ListItem>
                                                <asp:ListItem Value="TEGUCIGALPA">TEGUCIGALPA</asp:ListItem>
                                                <asp:ListItem Value="TEGUCIGALPA BA">TEGUCIGALPA BA</asp:ListItem>
                                                <asp:ListItem Value="TOCOA">TOCOA</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Nombre</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModNombre" placeholder="ej. Carlos Jose Hernandez" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Area</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModArea" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                                <asp:ListItem Value="CONVENIO INFATLAN BASA">CONVENIO INFATLAN BASA</asp:ListItem>
                                                <asp:ListItem Value="DESARROLLO Y MANTENIMIENTO">DESARROLLO Y MANTENIMIENTO</asp:ListItem>
                                                <asp:ListItem Value="GERENCIA GENERAL">GERENCIA GENERAL</asp:ListItem>
                                                <asp:ListItem Value="OPERACIONES TI">OPERACIONES TI</asp:ListItem>
                                                <asp:ListItem Value="PROYECTOS Y PROCESOS">PROYECTOS Y PROCESOS</asp:ListItem>
                                                <asp:ListItem Value="SOPORTE Y COMUNICACIONES">SOPORTE Y COMUNICACIONES</asp:ListItem>
                                                <asp:ListItem Value="SUBGERENCIA ADMINISTRATIVO">SUBGERENCIA ADMINISTRATIVO</asp:ListItem>
                                                <asp:ListItem Value="TALENTO HUMANO">TALENTO HUMANO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Nacimiento</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModNacimiento" placeholder="" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Telefono</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModTelefono" placeholder="ej. 99900012" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Email Empresa</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModEmailEmpresa" placeholder="ej. test@bancatlan.hn" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Email Personal</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModEmailPersonal" placeholder="ej. test@hotmail.com" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Turnos</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModTurnos" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Puesto</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModPuestos" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Jefe</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModJefatura" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">ADUser</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModADUser" placeholder="ej. egutierrez" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateModificacionBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnModEmpleados" runat="server" Text="Modificar" class="btn btn-primary" OnClick="BtnModEmpleados_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%--MODAL DE PASSWORD EMPLEADO--%>
    <div class="modal fade" id="PasswordModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelQA">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                Password Empleado
                                <asp:Label ID="LbEmpleadoPassword" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Password</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxModPassword" placeholder="" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Confirmar</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="TxModPasswordConfirmar" placeholder="" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdateUsuarioMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbUsuarioMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnCambiarPassword" runat="server" Text="Certificar" class="btn btn-primary" OnClick="BtnCambiarPassword_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
