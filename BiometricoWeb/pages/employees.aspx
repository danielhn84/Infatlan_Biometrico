<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="employees.aspx.cs" Inherits="BiometricoWeb.pages.employees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />

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
    <div runat="server" visible="false">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-datos" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus" > </i>Crear Empleados</a>
                <%--<a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book" > </i>Modificaciones</a>--%>
            </div>
        </nav>
    </div>
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-datos" role="tabpanel" aria-labelledby="nav-datos-tab">
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
                                                    <asp:DropDownList ID="DDLCrearArea" runat="server" class="form-control"></asp:DropDownList>
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
                                                    <asp:DropDownList ID="DDLJefatura2" runat="server" class="fstdropdown-select form-control"></asp:DropDownList>
                                                    <%--<asp:DropDownList ID="DDLJefatura" runat="server" class="form-control"></asp:DropDownList>--%>
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
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Tarjeta</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxTarjeta" placeholder="ej. 00123456" class="form-control" runat="server"></asp:TextBox>
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
