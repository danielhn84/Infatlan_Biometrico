<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="visitaDatacenter.aspx.cs" Inherits="BiometricoWeb.pages.activos.visitaDatacenter" %>
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
        function abrirModalRegistroEquipo() { $('#ModalEquipo').modal('show'); }
        function cerrarModalRegistroEquipo() { $('#ModalEquipo').modal('hide'); }

        function abrirModalMasInfo() { $('#ModalMasInformacion').modal('show'); }
        function cerrarModalMasInfo() { $('#ModalMasInformacion').modal('hide'); }

        function openAprobarJefeModal() { $('#AprobarJefeModal').modal('show'); }
        function closeAprobarJefeModal() { $('#AprobarJefeModal').modal('hide'); }

        
        
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
                        <h2>Registro de Visitas</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav_Creacion" role="tab" aria-controls="nav-profile" aria-selected="true"><i class="mdi mdi-library-books"></i>Nuevo</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-database" style=""></i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav_Creacion" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Datos Entrada--%>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
                                <h4 class="card-title">Responsable</h4>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Nombre</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ID="TxResponsable" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Identidad</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxIdentidadResponsable" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Departamento</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="TxSubgerencia" Text="" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Jefe</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxJefe" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <br />
                                <br />
                                <h4 class="card-title">Copia</h4>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Nombre</label>
                                            <div class="col-10">

                                            <asp:DropDownList ID="DdlNombreCopia" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DdlNombreCopia_SelectedIndexChanged" ></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Identidad</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxIdentidadCopia" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                                    <br />
                                <br />
                                <h4 class="card-title">Responsable Supervisar Trabajo</h4>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Nombre</label>
                                            <div class="col-10">

                                                <asp:DropDownList ID="DdlSupervisar" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DdlSupervisar_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Identidad</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxIdentidadSupervisar" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




                    <div class="col-12 grid-margin stretch-card" runat="server" id="DivPermisoExtendido">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Permiso Extendido</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-2">No. Permiso</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ID="TxPermisoExtendido" Text="" AutoPostBack="true" CssClass="form-control" OnTextChanged="TxPermisoExtendido_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Especificos</h4>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Fecha Inicio</label>
                                            <div class="col-10">
                                                <asp:TextBox ID="TxInicio" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" AutoPostBack="true" OnTextChanged="TxInicio_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Fecha Fin</label>
                                            <div class="col-9">
                                                <asp:TextBox ID="TxFin" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" AutoPostBack="true" OnTextChanged="TxFin_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12">
                                        <label runat="server" id="LbNotaDias_01" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA. SE VA REQUERIR APROBACIÓN DEL JEFE O SUBGERENTE DEL ÁREA AL QUE PERTENCE.</b> </small></label>
                                        <label runat="server" id="LbNotaDiasMayor16" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA. SE VA CREAR UNA SOLICITUD DE PERMISO EXTENDIDO PARA ESTA SOLICITUD, Y EN LAS PROXIMAS VISITAS NO SE VA REQUERIR APROBACION AL JEFE O SUBGERENTE.</b> </small></label>
                                        <label runat="server" id="LbNotaDiasMenor16" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA. EL RANGO DE FECHAS ES MENOR A 15 DÍAS, DEBE INGRESAR LA SOLICITUD DÍA POR DÍA Y SE VA REQUERIR APROBACIÓN DEL JEFE O SUBGERENTE.</b> </small></label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Acceso a</label>
                                            <div class="col-10">
                                                <asp:RadioButtonList ID="rbAcceso" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                    <asp:ListItem Value="1">CPP&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="2">CPA&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="3">Centro de Cableado&nbsp&nbsp&nbsp</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Tarea</label>
                                            <div class="col-9">
                                                <asp:RadioButtonList ID="rbTipoTarea" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                    <asp:ListItem Value="1">Programada&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="2">Emergencia</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Personal</label>
                                            <div class="col-2">
                                                <asp:CheckBoxList ID="chInterno" runat="server" CssClass="check green col-12" data-checkbox="icheckbox_flat-green">
                                                    <asp:ListItem Value="1" Text="Interno"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                            <div class="col-2">
                                                <asp:CheckBoxList ID="chExterno" runat="server" CssClass="check green col-12" data-checkbox="icheckbox_flat-green">
                                                    <asp:ListItem Value="2" Text="Externo"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">SdC, PdT o NdI</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="txPeticion" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-1">Trabajo</label>
                                            <div class="col-11">
                                                <asp:TextBox runat="server" ID="txTrabajo" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-1">Motivo</label>
                                            <div class="col-11">
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="4" ID="TxMotivo" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-1">Tareas a realizar</label>
                                            <div class="col-11">
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="4" ID="txTareasRealizar" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Participantes</h4>

                                <ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item"><a class="nav-link active" data-toggle="tab" href="#externo" role="tab"><span class="hidden-sm-up"><i class="fa fa-list"></i></span><span class="hidden-xs-down">Personal Externo</span></a> </li>
                                    <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#interno" runat="server" role="tab"><span class="hidden-sm-up"><i class="fa fa-paperclip"></i></span><span class="hidden-xs-down">Personal Interno</span></a> </li>                                 
                                </ul>

                                <div class="tab-content tabcontent-border" style="height: 530px">
                                    <!--PRIMER CONTENIDO-->
                                    <div class="tab-pane active p-20" id="externo" role="tabpanel">
                                        <br />
                                        <br />
                                        <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="divPersonalExterno">
                                            <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                                                data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap
                                                data-tablesaw-mode-switch>
                                                <thead>
                                                    <tr>
                                                        <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Nombre</th>
                                                        <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Identidad o Pasaporte </th>
                                                        <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Empresa</th>
                                                        <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Ingreso de equipo</th>
                                                        <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Permiso Celular</th>
                                                        <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB; align-self: center" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Acción</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="TxNombreVisita" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>
                                                            <asp:TextBox ID="TxIdentidadVisita" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>
                                                        <td>
                                                            <asp:DropDownList ID="DdlEmpresaVisita" runat="server" AutoPostBack="true" class="form-control"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="RbIngresoEquipoVisita" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                                <asp:ListItem Value="2">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="RbPermisoCelular" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                                <asp:ListItem Value="2">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:LinkButton ID="BtnAgregar" runat="server" title="Agregar" class="btn btn-success" OnClick="BtnAgregar_Click">
                                                             <i class="mdi mdi-plus text-white" style="-webkit-text-stroke-width: 1px"></i>
                                                            </asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>

                                        <asp:UpdatePanel runat="server" ID="UPVisitas" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DivExternoNoLectura">
                                                    <div class="table-responsive">
                                                        <!--<table id="bootstrap-data-table" class="table table-striped table-bordered"> -->
                                                        <asp:GridView ID="GvVisitas" runat="server" ShowHeader="false"
                                                            CssClass="table table-bordered"
                                                            PagerStyle-CssClass="pgr"
                                                            HeaderStyle-CssClass="table"
                                                            RowStyle-CssClass="rows"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            GridLines="None"
                                                            HeaderStyle-HorizontalAlign="center" OnPageIndexChanging="GvVisitas_PageIndexChanging"
                                                            PageSize="10" OnRowCommand="GvVisitas_RowCommand" OnRowDeleting="GvVisitas_RowDeleting"
                                                            Style="margin: 30px 0px 20px 0px">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombre" ItemStyle-HorizontalAlign="center" HeaderText="Nombre" ItemStyle-Width="22%" />
                                                                <asp:BoundField DataField="identidad" ItemStyle-HorizontalAlign="center" HeaderText="Identidad" ItemStyle-Width="20%" />
                                                                <asp:BoundField DataField="empresa" ItemStyle-HorizontalAlign="center" HeaderText="Empresa" ItemStyle-Width="15%" />
                                                                <asp:BoundField DataField="ingresoEquipo" ItemStyle-HorizontalAlign="center" HeaderText="Ingeso Equipo" ItemStyle-Width="12%" />
                                                                <asp:BoundField DataField="permisoCel" ItemStyle-HorizontalAlign="center" HeaderText="Permiso Celular" ItemStyle-Width="12%" />
                                                                <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="center" HeaderText="Acción">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="BtnInfo" runat="server" class="btn btn-secondary" Title="Ver" CommandArgument='<%# Eval("identidad") %>' CommandName="informacion"><i class="mdi mdi-information text-white""></i> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>

                                                 <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DivGvExternoLectura" visible="false">
                                                    <div class="table-responsive">
                                                        <!--<table id="bootstrap-data-table" class="table table-striped table-bordered"> -->
                                                        <asp:GridView ID="GvExternoLectura" runat="server" ShowHeader="true"
                                                            CssClass="table table-bordered"
                                                            PagerStyle-CssClass="pgr"
                                                            HeaderStyle-CssClass="table"
                                                            RowStyle-CssClass="rows"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            GridLines="None"
                                                            HeaderStyle-HorizontalAlign="center" OnPageIndexChanging="GvVisitas_PageIndexChanging"
                                                            PageSize="10" OnRowCommand="GvVisitas_RowCommand" OnRowDeleting="GvVisitas_RowDeleting"
                                                            Style="margin: 30px 0px 20px 0px">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombre" ItemStyle-HorizontalAlign="center" HeaderText="Nombre" ItemStyle-Width="22%" />
                                                                <asp:BoundField DataField="identidad" ItemStyle-HorizontalAlign="center" HeaderText="Identidad" ItemStyle-Width="20%" />
                                                                <asp:BoundField DataField="empresa" ItemStyle-HorizontalAlign="center" HeaderText="Empresa" ItemStyle-Width="15%" />
                                                                <asp:BoundField DataField="ingresoEquipo" ItemStyle-HorizontalAlign="center" HeaderText="Ingeso Equipo" ItemStyle-Width="12%" />
                                                                <asp:BoundField DataField="permisoCel" ItemStyle-HorizontalAlign="center" HeaderText="Permiso Celular" ItemStyle-Width="12%" />
                                                                <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="center" HeaderText="Acción">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="BtnInfo" runat="server" class="btn btn-secondary" Title="Ver" CommandArgument='<%# Eval("identidad") %>' CommandName="informacion"><i class="mdi mdi-information text-white""></i> </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                    <div class="tab-pane  p-20" id="interno" role="tabpanel">
                                        <br />
                                        <br />

                                        <asp:UpdatePanel runat="server" ID="UpdatePanel9" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="divPersonalInterno">
                                                    <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                                                        data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap
                                                        data-tablesaw-mode-switch>
                                                        <thead>
                                                            <tr>
                                                                <th scope="col" style="text-align: center; width: 680px; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Nombre</th>
                                                                <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Identidad o Pasaporte </th>
                                                                <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB; align-self: center" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Acción</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlPersonalInterno" Visible="true" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPersonalInterno_SelectedIndexChanged"></asp:DropDownList></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtIdentidadInterno" ReadOnly="true" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>
                                                                <td style="text-align: center">
                                                                    <asp:LinkButton ID="lbAddPersonalInterno" runat="server" title="Agregar" class="btn btn-success" OnClick="lbAddPersonalInterno_Click">
                                                             <i class="mdi mdi-plus text-white" style="-webkit-text-stroke-width: 1px"></i>
                                                                    </asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>


                                                <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DivGvInternoNoLectura" visible="true">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="GvPersonalInterno" runat="server" ShowHeader="false"
                                                            CssClass="table table-bordered"
                                                            PagerStyle-CssClass="pgr"
                                                            HeaderStyle-CssClass="table"
                                                            RowStyle-CssClass="rows"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            GridLines="None"
                                                            HeaderStyle-HorizontalAlign="center"
                                                            PageSize="10"
                                                            Style="margin: 30px 0px 20px 0px">
                                                            <Columns>
                                                                <asp:BoundField DataField="codigoEmpleado" ItemStyle-HorizontalAlign="center" HeaderText="Código" ItemStyle-Width="20%" />
                                                                <asp:BoundField DataField="nombreEmpleado" ItemStyle-HorizontalAlign="center" HeaderText="Nombre" ItemStyle-Width="45%" />
                                                                <asp:BoundField DataField="identidadInterno" ItemStyle-HorizontalAlign="center" HeaderText="Identidad" ItemStyle-Width="20%" />

                                                                <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="center" HeaderText="Acción">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="BtnEliminar" Enabled="true" runat="server" Text="" class="btn btn-danger mr-2" CommandArgument='<%# Eval("codigoEmpleado") %>' CommandName="eliminar"><i class="mdi mdi-trash-can text-white"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>



                                                    <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DivGvInternoLectura" visible="false" >
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="GvInternoLectura" runat="server" ShowHeader="true"
                                                            CssClass="table table-bordered"
                                                            PagerStyle-CssClass="pgr"
                                                            HeaderStyle-CssClass="table"
                                                            RowStyle-CssClass="rows"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            GridLines="None"
                                                            HeaderStyle-HorizontalAlign="center"
                                                            PageSize="10"
                                                            Style="margin: 30px 0px 20px 0px">
                                                            <Columns>
                                                                <asp:BoundField DataField="codigoEmpleado" ItemStyle-HorizontalAlign="center" HeaderText="Código" ItemStyle-Width="20%" />
                                                                <asp:BoundField DataField="nombreEmpleado" ItemStyle-HorizontalAlign="center" HeaderText="Nombre" ItemStyle-Width="45%" />
                                                                <asp:BoundField DataField="identidadInterno" ItemStyle-HorizontalAlign="center" HeaderText="Identidad" ItemStyle-Width="20%" />
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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


            <div class="col-12 grid-margin stretch-card" runat="server" id="DivCrearSolicitud">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Crear Solicitud</h4>

                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnCrearSolicitud" class="btn btn-primary mr-2" runat="server" Text="Enviar Solicitud" OnClick="BtnCrearSolicitud_Click" />
                                    <asp:Button ID="BtnCancelarSolicitud" class="btn btn-danger mr-2" runat="server" Text="Cancelar Solicitud" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        
              <div class="col-12 grid-margin stretch-card" runat="server" id="DivAprobarSolicitudJefe" visible="false">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Aprobar Solicitud</h4>

                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Acción:</label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="DdlAccionJefe" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlAccionJefe_SelectedIndexChanged" >
                                                    <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <asp:TextBox ID="TxMotivoJefe" Visible="false" placeholder="Favor ingresar observación........" class="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <br>
                                <br>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAprobar" class="btn btn-primary mr-2" runat="server" Text="Aprobar" OnClick="BtnAprobar_Click" />
                                    <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>



        </div>
    </div>


    <%--MODAL AGREGAR EQUIPO--%>
    <div class="modal bs-example-modal-lg" id="ModalEquipo" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" style="width: 1200px; top: 290px; left: 39%; transform: translate(-50%, -50%);" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LbEquipo" runat="server" Text="Registro de Equipo"></asp:Label>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <div class="col-1">
                                            <label class="col-form-label">Equipo:</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:DropDownList ID="DdlEquipo" runat="server" class="form-control"></asp:DropDownList>
                                        </div>

                                        <div class="col-1">
                                            <label class="col-form-label">Serie:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="TxSerie" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="col-1">
                                            <label class="col-form-label">Inventario:</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" ID="TxInventario" CssClass="form-control"></asp:TextBox>
                                        </div>


                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="BtnAgregarEquipo" runat="server" title="Agregar" class="btn btn-success" OnClick="BtnAgregarEquipo_Click">
                                                         <i class="mdi mdi-plus text-white"></i>
                                                </asp:LinkButton>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row col-12 mt-3">
                                                <div class="table table-bordered">
                                                    <asp:GridView ID="GvEquipo" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pgr"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false"
                                                        AllowPaging="true"
                                                        GridLines="None"
                                                        PageSize="5">
                                                        <Columns>
                                                            <asp:BoundField DataField="identidad" Visible="false" ItemStyle-Width="27%" />
                                                            <asp:BoundField DataField="id" Visible="true" ItemStyle-Width="27%" />
                                                            <asp:BoundField DataField="equipo" HeaderText="Equipo" ItemStyle-Width="27%" />
                                                            <asp:BoundField DataField="serie" HeaderText="Serie" ItemStyle-Width="27%" />
                                                            <asp:BoundField DataField="inventario" HeaderText="Inventario" ItemStyle-Width="27%" />
                                                            <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="BtnEliminar" runat="server" title="Eliminar" Style="background-color: #d9534f" class="btn" CommandArgument='<%# Eval("id") %>' CommandName="Eliminar">
                                                                <i class="mdi mdi-delete text-white"></i>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>

                                            <div class="col-12" runat="server" id="DivMensajeCorreo" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                                <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensajeCorreo"></asp:Label>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <div class="col-12" runat="server" id="DivMensajeUbic" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                    <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensajeUbic"></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>

                            <asp:Button ID="BtnCancelarEquipo" runat="server" Text="Cancelar" class="btn btn-secondary" OnClick="BtnCancelarEquipo_Click" />
                            <asp:Button ID="BtnAceptarEquipo" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnAceptarEquipo_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



        <%--MODAL AGREGAR EQUIPO--%>
    <div class="modal bs-example-modal-lg" id="ModalMasInformacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" style="width: 1200px; top: 290px; left: 39%; transform: translate(-50%, -50%);" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="Label1" runat="server" Text="Equipos permitidos"></asp:Label>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="row col-12 mt-3">
                                    <div class="table table-bordered">
                                        <asp:GridView ID="GvMasInfo" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None"
                                            PageSize="5">
                                            <Columns>
                                                <asp:BoundField DataField="identidad" Visible="false" ItemStyle-Width="27%" />
                                                <asp:BoundField DataField="id" Visible="true" ItemStyle-Width="27%" />
                                                <asp:BoundField DataField="equipo" HeaderText="Equipo" ItemStyle-Width="27%" />
                                                <asp:BoundField DataField="serie" HeaderText="Serie" ItemStyle-Width="27%" />
                                                <asp:BoundField DataField="inventario" HeaderText="Inventario" ItemStyle-Width="27%" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
 
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="Button1" runat="server" Text="Cancelar" class="btn btn-secondary" />                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--    MODAL  APROBAR JEFE--%>
    <div class="modal fade" id="AprobarJefeModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpTituloAprobarJefeModal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="TituloAprobacionJefe" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdateAprobarJefe" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAprobarJefe" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbAprobarJefePregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnAprobarJefeModal" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnAprobarJefeModal_Click" />
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
