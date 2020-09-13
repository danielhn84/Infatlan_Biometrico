<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="UpdateEmployees.aspx.cs" Inherits="BiometricoWeb.pages.UpdateEmployees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />
    <link href="/css/alert.css" rel="stylesheet" />

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
    <style>
        /* The container */
        .container {
          display: block;
          position: relative;
          padding-left: 35px;
          margin-bottom: 12px;
          cursor: pointer;
          font-size: 22px;
          -webkit-user-select: none;
          -moz-user-select: none;
          -ms-user-select: none;
          user-select: none;
        }

        /* Hide the browser's default checkbox */
        .container input {
          position: absolute;
          opacity: 0;
          cursor: pointer;
          height: 0;
          width: 0;
        }

        /* Create a custom checkbox */
        .checkmark {
          position: absolute;
          top: 0;
          left: 0;
          height: 25px;
          width: 25px;
          background-color: #eee;
        }

        /* On mouse-over, add a grey background color */
        .container:hover input ~ .checkmark {
          background-color: #ccc;
        }

        /* When the checkbox is checked, add a blue background */
        .container input:checked ~ .checkmark {
          background-color: #2196F3;
        }

        /* Create the checkmark/indicator (hidden when not checked) */
        .checkmark:after {
          content: "";
          position: absolute;
          display: none;
        }

        /* Show the checkmark when checked */
        .container input:checked ~ .checkmark:after {
          display: block;
        }

        /* Style the checkmark/indicator */
        .container .checkmark:after {
          left: 9px;
          top: 5px;
          width: 5px;
          height: 10px;
          border: solid white;
          border-width: 0 3px 3px 0;
          -webkit-transform: rotate(45deg);
          -ms-transform: rotate(45deg);
          transform: rotate(45deg);
        }
    </style>
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
    <div runat="server" visible="false">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book" > </i>Modificaciones</a>
                <%--<a class="nav-item nav-link" id="nav_cargar_tab" data-toggle="tab" href="#nav-cargar" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-upload" > </i>Cargar Compensatorio</a>--%>
            </div>
        </nav>
    </div>
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card" id="21">
                                <div class="card-body">
                                    <h4 class="card-title">Empleados creados</h4>

                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Buscar</label>
                                            <div class="col-sm-9">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxBuscarEmpleado" runat="server" placeholder="Ej. Elvin - Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscarEmpleado_TextChanged"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card" id="212">
                                <div class="card-body">
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

        <div class="tab-pane fade" id="nav-cargar" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Empleados creados</h4>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Cargar</label>
                                    <div class="col-sm-9">
                                                
                                        <div class="form-horizontal form-label-left">
                                            <div class="form-group">
                                                <label class="control-label col-md-2 col-sm-2 col-xs-12" for="first-name"></label>
                                                <div class="col-md-3 col-sm-3 col-xs-12">
                                                    <asp:FileUpload runat="server" ID="FUCompensatorio"  AllowMultiple="false" ClientIDMode="AutoID" />
                                                </div>
                                            </div>
                                            <div class="form-group col-md-3 col-sm-3 col-xs-12">
                                                <asp:Button ID="BtnSubirCompensatorio" class="btn btn-sm btn-info" runat="server" Text="Subir Cambios" OnClick="BtnSubirCompensatorio_Click" OnClientClick="return postbackButtonClick();" />
                                            </div>

                                            <div class="footer">
                                                <div class="stats">
                                                    <i class="fa fa-info"></i>
                                                    <asp:Label ID="LabelMensaje" runat="server" Text="Recuerda verificar el Excel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
                
        </div>
    </div>

    <%--MODAL DE MODIFICACION--%>
    <div class="modal fade" id="EmpleadoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
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
                            <div class="row col-12">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Estado</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLEstado" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0">Seleccione una opción</asp:ListItem>
                                                <asp:ListItem Value="True">Activado</asp:ListItem>
                                                <asp:ListItem Value="False">Desactivado</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Identidad</label>
                                        <div class="col-9">
                                            <asp:TextBox ID="TxModIdentidad" placeholder="ej. 0801190000123" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row col-12" style="margin-top:-2%">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3">Codigo SAP</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModCodigoSAP" placeholder="ej.80000000" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Ciudad</label>
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

                            <div class="row col-12" style="margin-top:-2%">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Nombre</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModNombre" placeholder="ej. Carlos Jose Hernandez" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Area</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModArea" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row col-12" style="margin-top:-2%">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Nacimiento</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModNacimiento" placeholder="" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Telefono</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModTelefono" placeholder="ej. 99900012" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row col-12" style="margin-top:-2%">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">Email Empresa</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModEmailEmpresa" placeholder="ej. test@bancatlan.hn" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">Email Personal</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModEmailPersonal" placeholder="ej. test@hotmail.com" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row col-12" style="margin-top:-0.5%">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Turnos</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModTurnos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Puesto</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModPuestos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row col-12" style="margin-top:-2%">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">Jefe</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLModJefatura" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3 col-form-label">ADUser</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxModADUser" placeholder="ej. egutierrez" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row col-12" style="margin-top:-2%">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label"></label>
                                        <div class="col-sm-9">
                                            <label class="container">Autorizar Permisos
                                                <input type="checkbox" runat="server" id="CBxPermisos"><span class="checkmark"></span>
                                            </label>
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
                            <asp:Button ID="BtnModEmpleados" runat="server" Text="Modificar" class="btn btn-success" OnClick="BtnModEmpleados_Click" />
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
    <script src="/js/fstdropdown.js"></script>
    <script>
        function setDrop() {
            if (!document.getElementById('third').classList.contains("fstdropdown-select"))
                document.getElementById('third').className = 'fstdropdown-select';
            setFstDropdown();
        }
        setFstDropdown();
        function removeDrop() {
            if (document.getElementById('third').classList.contains("fstdropdown-select")) {
                document.getElementById('third').classList.remove('fstdropdown-select');
                document.getElementById("third").fstdropdown.dd.remove();
            }
        }
        function addOptions(add) {
            var select = document.getElementById("fourth");
            for (var i = 0; i < add; i++) {
                var opt = document.createElement("option");
                var o = Array.from(document.getElementById("fourth").querySelectorAll("option")).slice(-1)[0];
                var last = o == undefined ? 1 : Number(o.value) + 1;
                opt.text = opt.value = last;
                select.add(opt);
            }
        }
        function removeOptions(remove) {
            for (var i = 0; i < remove; i++) {
                var last = Array.from(document.getElementById("fourth").querySelectorAll("option")).slice(-1)[0];
                if (last == undefined)
                    break;
                Array.from(document.getElementById("fourth").querySelectorAll("option")).slice(-1)[0].remove();
            }
        }
        function updateDrop() {
            document.getElementById("fourth").fstdropdown.rebind();
        }
    </script>
</asp:Content>
