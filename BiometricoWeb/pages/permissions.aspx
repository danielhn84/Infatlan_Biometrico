<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="permissions.aspx.cs" Inherits="BiometricoWeb.pages.permissions" %>

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
        var updateProgress = null;com

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function javas(e) {
            var ddl = document.getElementById("<%= DDLTipoPermiso.ClientID %>").value;
            if (e.checked && (ddl == '1004' || ddl == '1007' || ddl == '1011')) {
                $('#ModalToken').modal('show');
            }
        }

        function openModal() { $('#InformativoModal').modal('show'); }
        function openEdicionModal() { $('#DocumentoModal').modal('show'); }
        function openDescargarModal() { $('#DescargaModal').modal('show'); }
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
                        <h2>Solicitud de Permiso</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-datos" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus"></i>Crear Permiso</a>
            <a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Mis Permisos</a>

            <a style="margin-left: auto; font-size: large; color: lightslategray" class="nav-item nav-link align-content-lg-end">Dias de vacaciones pendientes
                <b><asp:Label ID="LbNumeroVaciones" runat="server" Text="0"></asp:Label></b>
            </a>
            
        </div>
        <%--compensatorio--%>
        <div class="col-md-6" style="float:right; height:100%; visibility:visible">
            <a style="margin-left:auto; text-align:right; font-size: large; color: lightslategray" class="nav-item nav-link align-content-lg-end">Horas de tiempo compensatorio
                <b style="margin-right:-10px;"><asp:Label ID="LbCompensatorio" runat="server" Text="0"></asp:Label></b>
            </a>
        </div>
    </nav>

    <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-datos" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="form-check form-check-flat form-check-primary" style="margin-left: auto;">
                <label class="form-check-label">
                    <input type="checkbox" name="CbEmergencias" value="0" class="form-check-input" onclick="javas(this);" runat="server" id="CbEmergencias" />Presione aqui si su solicitud es de emergencia
                </label>
            </div>

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Tipo de Permiso</h4>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Tipo Permiso</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ClientIDMode="AutoID" ID="DDLTipoPermiso" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDLTipoPermiso_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Motivo</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxMotivo" class="form-control" runat="server" TextMode="SingleLine" Visible="true" MaxLength="20"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6" runat="server" id="DIVCompensacion" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Compensación</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLCompensacion" runat="server" class="form-control">
                                                        <asp:ListItem Value="0">Reposición día trabajo</asp:ListItem>
                                                        <asp:ListItem Value="0">Interrupción Vacaciones</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6" runat="server" id="DIVCompensacionFecha" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Fecha</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxFechaCompensacion" class="form-control" runat="server" TextMode="date" Visible="true" MaxLength="20"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6" runat="server" id="DIVParientes" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Pariente</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLParientes" runat="server" class="form-control">
                                                        <asp:ListItem Value="0">Seleccione un pariente</asp:ListItem>
                                                        <asp:ListItem Value="1">Padres</asp:ListItem>
                                                        <asp:ListItem Value="2">Hijos(as)</asp:ListItem>
                                                        <asp:ListItem Value="3">Conyugues</asp:ListItem>
                                                        <asp:ListItem Value="4">Yernos</asp:ListItem>
                                                        <asp:ListItem Value="5">Nueras</asp:ListItem>
                                                        <asp:ListItem Value="6">Abuelos</asp:ListItem>
                                                        <asp:ListItem Value="7">Hermanos(as)</asp:ListItem>
                                                        <asp:ListItem Value="8">Nietos</asp:ListItem>
                                                        <asp:ListItem Value="9">Cuñadas</asp:ListItem>
                                                        <asp:ListItem Value="10">Suegros</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6" runat="server" id="DIVDocumentos" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Documentos</label>
                                                <div class="col-sm-9">
                                                    <asp:FileUpload ID="FUDocumentoPermiso" runat="server" class="form-control" />
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
                            <h4 class="card-title">Ingreso de Solicitud</h4>

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
                                                    <asp:DropDownList ID="DDLEmpleado" runat="server" class="fstdropdown-select form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Jefe</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLJefe" runat="server" class="fstdropdown-select form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanelFechas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Desde</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxFechaInicio" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Hasta</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxFechaRegreso" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
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
                            <h4 class="card-title">Crear permiso</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrearPermiso" class="btn btn-primary mr-2" runat="server" Text="Crear Permiso" OnClick="BtnCrearPermiso_Click" />
                                        <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="tab-pane fade" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Permisos creados</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <%--<asp:UpdatePanel ID="UpdateGridView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="10" OnRowCommand="GVBusqueda_RowCommand" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnMotivo" runat="server" Text="Motivo" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("idPermiso") %>' CommandName="MotivoPermiso">
                                                                        <i class="mdi mdi-comment-search-outline text-primary" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar" runat="server" Text="Editar" class="btn btn-inverse-danger mr-2" CommandArgument='<%# Eval("idPermiso") %>' CommandName="EditarPermiso">
                                                                        <i class="mdi mdi-image-filter" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnDocumento" runat="server" Text="Descargar" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idPermiso") %>' CommandName="DocumentoPermiso">
                                                                        <i class="mdi mdi-download text-primary" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="idPermiso" HeaderText="No." />
                                                    <asp:BoundField DataField="fechaInicio" HeaderText="Inicio" />
                                                    <asp:BoundField DataField="fechaRegreso" HeaderText="Fin" />
                                                    <asp:BoundField DataField="idTipoPermiso" HeaderText="Permiso" />
                                                    <asp:BoundField DataField="fechaSolicitud" HeaderText="Solicitud" />
                                                    <asp:BoundField DataField="autorizado" HeaderText="Autorizado" />
                                                </Columns>
                                            </asp:GridView>
                                            <%--</ContentTemplate>
               
                                            </asp:UpdatePanel>--%>
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

    <div class="modal fade" id="InformativoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdateLabelPermiso" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelUsuario">Permiso Solicitado</h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Label ID="LbIncapacidadInformacion" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdateAutorizar" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Label ID="LbInformacionPermiso" runat="server" Text="Label"></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdateAutorizarMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAutorizarMensaje" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateUsuarioBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarPermiso" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarPermiso_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnEnviarPermiso" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="DocumentoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelPermiso">Subir archivos - 
                                <asp:Label ID="LbPermisoSubir" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">
                                        <h4>Documentos</h4>
                                    </label>
                                    <div class="col-sm-12">
                                        <asp:FileUpload ID="FUSubirArchivoEdicion" runat="server" class="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <div class="col-sm-12" style="text-align: justify;">
                                        Recuerda que los documentos aqui ingresados son de alta confidencialidad, por favor te pedimos que no compartas tu clave para que estos documentos sean solo para el area de recursos humanos.
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="Label3" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEditarPermiso" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEditarPermiso_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnEditarPermiso" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="DescargaModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelDescarga">Descargar archivo de permiso - 
                                <asp:Label ID="LbPermisoDescarga" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-sm-12 col-form-label">
                                        <h4>Privacidad de documentos</h4>
                                    </label>
                                    <div class="col-sm-12" style="text-align: justify">
                                        Este documento adjunto es confidencial, especialmente en lo que hace referencia a los datos personales que puedan contener y se dirigen exclusivamente al destinatario referenciado. Si usted no lo es y ha descargado este archivo o tiene conocimiento del mismo por cualquier motivo, le rogamos nos lo comunique por este medio y proceda a borrarlo, y que, en todo caso, se abstenga de utilizar, reproducir, alterar, archivar o comunicar a terceros el documento adjunto.
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="Label1" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnDescargarArchivo" runat="server" Text="Descargar" class="btn btn-success" OnClick="BtnDescargarArchivo_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnDescargarArchivo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalToken" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 650px; top: 200px; left: 50%; transform: translate(-50%, -50%);">


                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabel">Token de Emergencias</h4>
                    
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-sm-12 col-form-label"><b> Ingrese el Token</b> <br /> Si no lo tiene solicítelo a Recursos Humanos.</label>
                                    
                                    <asp:TextBox runat="server" ID="TxToken" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" >
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensajeToken" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="BtnCancelTk" CssClass="btn btn-secondary" Text="Cerrar" OnClick="BtnCancelTk_Click" />
                            <asp:Button ID="BtnContinuar" runat="server" Text="Continuar" class="btn btn-success" OnClick="BtnContinuar_Click" />
                        </ContentTemplate>
                        <%--<Triggers>
                                    <asp:PostBackTrigger ControlID="BtnContinuar" />
                                </Triggers>--%>
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
