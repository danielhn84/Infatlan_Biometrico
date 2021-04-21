<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="visitaDatacenter.aspx.cs" Inherits="BiometricoWeb.pages.activos.visitaDatacenter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />
    <link href="/css/alert.css" rel="stylesheet" />
    <link href="/Content/font-awesome.min.css" rel="stylesheet" />
    <link href="/Content/checkbox1.css" rel="stylesheet" />
    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        } 
    </script>
    <script type="text/javascript">
        function openAprobarJefeModal() { $('#AprobarJefeModal').modal('show'); }
        function closeAprobarJefeModal() { $('#AprobarJefeModal').modal('hide'); }    

        function openAprobarGestorModal() { $('#AprobarGestorModal').modal('show'); }
        function closeAprobarGestorModal() { $('#AprobarGestorModal').modal('hide'); }    

        function openEnviarSolicitudModal() { $('#EnviarSolicitudModal').modal('show'); }
        function closeEnviarSolicitudModal() { $('#EnviarSolicitudModal').modal('hide'); }    
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
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
                                <h6 class="card-subtitle" style="margin:-10px 0px 30px;">Información del responsable.</h6>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="row">
                                            <label class="col-3"><b>Nombre :</b></label>
                                            <asp:Label runat="server" ID="LbResponsable" CssClass="col-9"/>
                                            
                                            <label class="col-3"><b>Area :</b></label>
                                            <asp:Label runat="server" ID="LbSubgerencia" CssClass="col-9"/>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="row">
                                            <label class="col-3"><b>Identidad :</b></label>
                                            <asp:Label runat="server" ID="LbIdentidadResponsable" CssClass="col-9"/>
                                        
                                            <label class="col-3"><b>Jefe :</b></label>
                                            <asp:Label runat="server" ID="LbJefe" CssClass="col-9"/>
                                        </div>
                                    </div>
                                    <div class="col-6" runat="server" visible="false">
                                        <div class="row">
                                            <asp:Label runat="server" ID="LbCorreo" CssClass="col-9"/>
                                            <asp:Label runat="server" ID="LbIdJefe" CssClass="col-9"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Adicionales</h4>
                                <h6 class="card-subtitle" style="margin:-10px 0px 30px;">Complete los campos que se le pden a continuación.</h6>
                                <div class="row">
                                    <div class="col-6" runat="server" id="DivPermisoExtendido">
                                        <div class="row">
                                            <label class="col-3">Permiso Extendido</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLExtendido" AutoPostBack="true" OnSelectedIndexChanged="DDLExtendido_SelectedIndexChanged">
                                                    <asp:ListItem Text="No" Value="0" />
                                                    <asp:ListItem Text="Si" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mt-3">
                                    <div class="col-6">
                                        <div class="row">
                                            <label class="col-3 col-form-label">Fecha Inicio</label>
                                            <div class="col-9">
                                                <asp:TextBox ID="TxInicio" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" AutoPostBack="true" OnTextChanged="TxInicio_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="row">
                                            <label class="col-3 col-form-label">Fecha Fin</label>
                                            <div class="col-9">
                                                <asp:TextBox ID="TxFin" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" AutoPostBack="true" OnTextChanged="TxFin_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6 mt-3">
                                        <div class="row">
                                            <label class="col-3 col-form-label">Supervisor</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DdlSupervisar" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6 mt-3">
                                        <div class="row">
                                            <label class="col-3 col-form-label">Copia</label>
                                            <div class="col-9">
                                                <asp:DropDownList ID="DdlNombreCopia" runat="server" class="form-control" ></asp:DropDownList>
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
                                <h4 class="card-title">Información Especifica</h4>
                                <h6 class="card-subtitle" style="margin:-10px 0px 30px;">Complete los campos que se le pden a continuación.</h6>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="row">
                                            <label class="col-2 col-form-label">Trabajo</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ID="txTrabajo" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-12 mt-3">
                                        <div class="row">
                                            <label class="col-2 col-form-label">Motivo</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="4" ID="TxMotivo" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12 mt-3">
                                        <div class="row">
                                            <label class="col-2 col-form-label">Tareas a realizar</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="4" ID="txTareasRealizar" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-12 mt-3">
                                        <div class="row">
                                            <label class="col-2 col-form-label">SdC, PdT o NdI</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ID="txPeticion" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12 mt-3">
                                        <div class="row">
                                            <label class="col-2 col-form-label">Acceso</label>
                                            <div class="col-10">
                                                <asp:DropDownList runat="server" ID="DDLAcceso" CssClass="form-control">
                                                    <asp:ListItem Value="0" Text="Seleccione una opción"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="DATA CENTER SONISA"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="DATA CENTER LOS ALMENDROS"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="CENTRO DE CABLEADO"></asp:ListItem>
                                                </asp:DropDownList>
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
                                <h6 class="card-subtitle" style="margin:-10px 0px 30px;">Personas que podrán acceder al Data Center.</h6>
                                <div class="row">
                                    <div class="col-12 mt-3">
                                        <div class="row">
                                            <label class="col-2">Personal</label>
                                            <div class="col-2">
                                                <asp:CheckBox Text="Interno" ID="CbxInterno" CssClass="fancy-checkbox col-12" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="CbxInterno_CheckedChanged"/>
                                            </div>
                                            <div class="col-2">
                                                <asp:CheckBox Text="Externo" ID="CbxExterno" CssClass="fancy-checkbox col-12" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="CbxExterno_CheckedChanged"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mt-3" runat="server" id="DivParticipantes" visible="false">
                                    <nav>
                                        <div class="nav nav-pills " id="nav-tab1" role="tablist">
                                            <a class="nav-item nav-link" id="tabInterno" data-toggle="tab" href="#interno" role="tab" runat="server" visible="false">
                                                <i class="mdi mdi-fire"></i>Interno
                                            </a> 
                                            <a class="nav-item nav-link" id="tabExterno" data-toggle="tab" href="#externo" role="tab" runat="server" visible="false">
                                                <i class="mdi mdi-inbox"></i>Externo
                                            </a> 
                                        </div>
                                    </nav>
                                    <div class="tab-content">
                                        <div class="tab-pane p-20" id="interno" role="tabpanel">
                                            <br />
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel9" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="divPersonalInterno">
                                                        <table class="tablesaw table-bordered table-hover table no-wrap"
                                                            data-tablesaw-mode="swipe" data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap data-tablesaw-mode-switch>
                                                            <thead>
                                                                <tr>
                                                                    <th scope="col" style="text-align: center; width: 680px; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Nombre</th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Identidad o Pasaporte </th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB; align-self: center" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Acción</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td><asp:DropDownList ID="ddlPersonalInterno" Visible="true" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPersonalInterno_SelectedIndexChanged"></asp:DropDownList></td>
                                                                    <td><asp:TextBox ID="txtIdentidadInterno" ReadOnly="true" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>
                                                                    <td style="text-align: center">
                                                                        <asp:LinkButton ID="lbAddPersonalInterno" runat="server" title="Agregar" class="btn btn-success" OnClick="lbAddPersonalInterno_Click">
                                                                            <i class="mdi mdi-plus text-white" style="-webkit-text-stroke-width: 1px"></i>
                                                                        </asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <asp:TextBox ID="TxCorreoInterno" ReadOnly="true" runat="server" CssClass="form-control" AutoPostBack="true" Visible="false"></asp:TextBox></td>

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
                                                                PageSize="10" OnRowCommand="GvPersonalInterno_RowCommand"
                                                                Style="margin: 30px 0px 20px 0px">
                                                                <Columns>
                                                                    <asp:BoundField DataField="codigoEmpleado" ItemStyle-HorizontalAlign="center" HeaderText="Código" ItemStyle-Width="10" />
                                                                    <asp:BoundField DataField="nombreEmpleado" ItemStyle-HorizontalAlign="center" HeaderText="Nombre" />
                                                                    <asp:BoundField DataField="identidadInterno" ItemStyle-HorizontalAlign="center" HeaderText="Identidad"/>
                                                                    <asp:TemplateField ItemStyle-Width="10" ItemStyle-HorizontalAlign="center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="BtnEliminar" style="margin:-15px 0px -15px" Enabled="true" runat="server" Text="" class="btn btn-danger mr-2" CommandArgument='<%# Eval("codigoEmpleado") %>' CommandName="eliminar">
                                                                                <i class="mdi mdi-delete text-white"></i>
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div class="tab-pane p-20" id="externo" role="tabpanel">
                                            <br />
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel15" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="divPersonalExterno">
                                                        <table class="tablesaw table-hover table no-wrap" 
                                                            data-tablesaw-mode="swipe" data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap data-tablesaw-mode-switch>
                                                            <thead>
                                                                <tr>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Nombre</th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Identidad o Pasaporte </th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Empresa</th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Equipo</th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Celular</th>
                                                                    <th scope="col" style="text-align: center; background-color: #5D6D7E; color: #D5DBDB; align-self: center" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Acción</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td><asp:TextBox ID="TxNombreVisita" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                                    <td><asp:TextBox ID="TxIdentidadVisita" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>
                                                                    <td><asp:DropDownList ID="DdlEmpresaVisita" runat="server" AutoPostBack="true" class="form-control"></asp:DropDownList></td>
                                                                    <td style="margin:-20px 0px -30px">
                                                                        <asp:RadioButtonList ID="RbIngresoEquipoVisita" RepeatDirection="Horizontal" runat="server" style="margin:-20px  0px -30px" AutoPostBack="True">
                                                                            <asp:ListItem Value="1">Si</asp:ListItem>
                                                                            <asp:ListItem Value="2">No</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td style="margin:-20px  0px -30px">
                                                                        <asp:RadioButtonList ID="RbPermisoCelular" RepeatDirection="Horizontal" runat="server" style="margin:-20px  0px -30px" AutoPostBack="True">
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
                                                                    <asp:GridView ID="GvVisitas" runat="server" ShowHeader="false"
                                                                        CssClass="table table-bordered"
                                                                        PagerStyle-CssClass="pgr"
                                                                        HeaderStyle-CssClass="table"
                                                                        RowStyle-CssClass="rows"
                                                                        AutoGenerateColumns="false"
                                                                        AllowPaging="true"
                                                                        GridLines="None"
                                                                        HeaderStyle-HorizontalAlign="center" OnPageIndexChanging="GvVisitas_PageIndexChanging"
                                                                        PageSize="10" OnRowCommand="GvVisitas_RowCommand"
                                                                        Style="margin: 30px 0px 20px 0px">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="id" ItemStyle-HorizontalAlign="center" HeaderText="No." ItemStyle-Width="10" />
                                                                            <asp:BoundField DataField="nombre" ItemStyle-HorizontalAlign="center" HeaderText="Nombre" />
                                                                            <asp:BoundField DataField="identidad" ItemStyle-HorizontalAlign="center" HeaderText="Identidad" />
                                                                            <asp:BoundField DataField="empresa" ItemStyle-HorizontalAlign="center" HeaderText="Empresa" ItemStyle-Width="10" />
                                                                            <asp:BoundField DataField="ingresoEquipo" ItemStyle-HorizontalAlign="center" HeaderText="Ingeso Equipo" ItemStyle-Width="10" />
                                                                            <asp:BoundField DataField="permisoCel" ItemStyle-HorizontalAlign="center" HeaderText="Permiso Celular" ItemStyle-Width="10" />
                                                                            <asp:TemplateField ItemStyle-Width="10" ItemStyle-HorizontalAlign="center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="BtnElinimarVisita" style="margin:-15px 0px -15px;" runat="server" class="btn btn-danger" Title="Eliminar" CommandArgument='<%# Eval("id") %>' CommandName="eliminar">
                                                                                        <i class="mdi mdi-delete text-white""></i> 
                                                                                    </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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

            <asp:UpdatePanel runat="server" ID="UpdatePanel22" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card" runat="server" id="DivParticipantesVista" visible="false">
                            <div class="card-body">
                                <h4 class="card-title">Participantes</h4>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel20" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DivGvInterLectura" visible="false">
                                            <div class="table-responsive">
                                                <h4 class="card-title">Personal Interno</h4>
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

                                <br>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel21" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DivGvExternoLectura" visible="false">
                                            <div class="table-responsive">
                                                    <h4 class="card-title">Personal Externo</h4>
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
                                                    PageSize="10" OnRowCommand="GvVisitas_RowCommand"
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
                                    <asp:Button ID="BtnCancelarSolicitud" class="btn btn-danger mr-2" runat="server" Text="Cancelar Solicitud" OnClick="BtnCancelarSolicitud_Click" />
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
                                                <asp:DropDownList ID="DdlAccionJefe" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlAccionJefe_SelectedIndexChanged">
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
                                    <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 grid-margin stretch-card" runat="server" id="DivAprobacionGestor" visible="false">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Aprobar Solicitud</h4>

                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Acción:</label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="DdlAccionGestor" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlAccionGestor_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <asp:TextBox ID="TxtMotivoGestor" Visible="false" placeholder="Favor ingresar observación........" class="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <br>
                                <br>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAprobarGestor" class="btn btn-primary mr-2" runat="server" Text="Aprobar"  OnClick="BtnAprobarGestor_Click"/>
                                    <asp:Button ID="BtnCancelarGestor" class="btn btn-danger mr-2" runat="server" Text="Cancelar" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="nav-Registros" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br>
            <div class="row" id="DivBusqueda" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Solicitudes ingresadas</h4>
                            <p>Ordenados por fecha de creación</p>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Buscar</label>
                                    <div class="col-sm-9">
                                        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxBuscarSolicitud" runat="server" placeholder="Búsqueda por solicitud o supervisor trabajo, Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscarSolicitud_TextChanged"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpBusquedaSolicitudes" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVSolicitudes" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true" 
                                                GridLines="None"  OnPageIndexChanging="GVSolicitudes_PageIndexChanging"
                                                PageSize="10">
                                                <Columns>
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="Solicitud" />
                                                    <asp:BoundField DataField="fechaInicio" HeaderText="Fecha Inicio" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="fechaFin" HeaderText="Fecha Fin" />
                                                    <asp:BoundField DataField="acceso" HeaderText="Acceso" />
                                                    <asp:BoundField DataField="peticion" HeaderText="Petición" />
                                                    <asp:BoundField DataField="trabajo" HeaderText="Trabajo" />
                                                    <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                                                    <asp:BoundField DataField="tareasRealizar" HeaderText="Tareas a Realizar" />
                                                    <asp:BoundField DataField="ingreso" HeaderText="Responsable" />
                                                    <asp:BoundField DataField="copia" HeaderText="Copia" />
                                                    <asp:BoundField DataField="supervisorProveedor" HeaderText="Supervisa" />
                                                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                                                </Columns>
                                            </asp:GridView>
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

    <%--MODAL  APROBAR JEFE--%>
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

    <%-- ENVIAR SOLICITUD--%>
    <div class="modal fade" id="EnviarSolicitudModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="lbTitulo" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row p-t-20">
                                <div class="col-6">
                                    <label class="control-label">Fecha Inicio:</label></label>
                                        <asp:TextBox ID="TxFechaInicioModal" AutoPostBack="true" runat="server" TextMode="DateTimeLocal" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="control-label">Fecha Fin:</label></label>    
                                     <asp:TextBox ID="TxFechaFinModal" AutoPostBack="true" runat="server" TextMode="DateTimeLocal" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br>
                            <div class="row p-t-20">
                                <div class="col-12">
                                    <label class="control-label">Tarea:</label></label>
                                    <asp:TextBox ID="TxTareaModal" AutoPostBack="true" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br>
                            <div class="row p-t-20">
                                <div class="col-12">
                                    <label class="control-label">Motivo:</label></label>
                                    <asp:TextBox ID="TxMotivoModal" AutoPostBack="true" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="BtnCerrarSoli" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarSoli" runat="server" Text="Enviar" class="btn btn-success"  OnClick="BtnEnviarSoli_Click"/>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL  APROBAR SUPERVISOR--%>
    <div class="modal fade" id="AprobarGestorModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="TituloAprobacionGestor" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAprobarGestor" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbAprobarGestorPregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarGestor" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarGestor_Click"  />
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
