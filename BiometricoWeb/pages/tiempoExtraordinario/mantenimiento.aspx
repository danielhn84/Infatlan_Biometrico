<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="mantenimiento.aspx.cs" Inherits="BiometricoWeb.pages.tiempoExtraordinario.mantenimiento" %>
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
        function openModal() { $('#InformativoModal').modal('show'); }
        function closeModal() { $('#InformativoModal').modal('hide'); }

        function openModificarModal() { $('#ModificarModal').modal('show'); }
        function closeModificarModal() { $('#ModificarModal').modal('hide'); }

        function openModalTarea() { $('#InformativoModalTarea').modal('show'); }
        function closeModalTarea() { $('#InformativoModalTarea').modal('hide'); }

    </script>

    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
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
                        <h2>Tipo Trabajo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-crearTrabajo" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus"></i>Crear Trabajo</a>
            <a class="nav-item nav-link" runat="server" visible="true" id="nav_ListaTipoTrabajo_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-pencil"></i>Modificar Trabajo</a>
            <a class="nav-item nav-link" runat="server" visible="true" id="nav_subTareasLista_tab" data-toggle="tab" href="#nav-subTareas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus"></i>Agregar SubTarea</a>

        </div>
    </nav>

    <div class="tab-content" id="nav-tabContent">
        <br />
        <%--PRIMERA PARTE--%>
        <div class="tab-pane fade show active" id="nav-crearTrabajo" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title"><b>Creación Tipo de Trabajo</b> </h4>
                            <br>
                            <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Trabajo</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxTrabajo" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
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

            <div class="row" runat="server" id="DivCrearSolicitud">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrear" class="btn btn-primary mr-2" runat="server" Text="Enviar"  OnClick="BtnCrear_Click"/>
                                        <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </div>

        <%-- SECCION 2---%>
        <div class="tab-pane fade" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpdateDivTiposTrabajo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row" id="DivBusqueda" runat="server">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Tipos de trabajo creados</h4>

                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Buscar</label>
                                            <div class="col-sm-9">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="TxBuscar" runat="server" placeholder="Ej. Proyecto.... - Presione afuera para proceder o por Id" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscar_TextChanged" ></asp:TextBox>
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
                            <div class="card">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVBusquedaTrabajos" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"  OnPageIndexChanging="GVBusquedaTrabajos_PageIndexChanging"
                                                GridLines="None"  OnRowCommand="GVBusquedaTrabajos_RowCommand"
                                                PageSize="10">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnModificar" runat="server" Text="Modificar" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idTipoTrabajo") %>' CommandName="Modificar">
                                                                      <i class="mdi mdi-ballot text-primary" ></i>
                                                            </asp:LinkButton>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="idTipoTrabajo" HeaderText="Id. Tipo Trabajo" />
                                                    <asp:BoundField DataField="nombreTrabajo" HeaderText="Trabajo" ItemStyle-HorizontalAlign="Center" />
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
        <%-- FIN SECCION 2---%>

        <%--TERCERA PARTE--%>
        <div class="tab-pane fade show active" id="nav-subTareas" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title"><b>Creación Subtareas</b> </h4>
                            <br>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Trabajo</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DDLTrabajos" runat="server" class="fstdropdown-select form-control" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Tarea</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxTarea" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
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

            <div class="row" runat="server" id="Div1">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrearTarea" class="btn btn-primary mr-2" runat="server" Text="Enviar"  OnClick="BtnCrearTarea_Click" />
                                        <asp:Button ID="BtnCancelarTarea" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelarTarea_Click"  />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </div>


    <%--    MODAL INFORMATIVA--%>
    <div class="modal fade" id="InformativoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title">Creación Tipo de Trabajo</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdateAutorizarMensaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbInformacion" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbInformacionPregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel48" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--    MODAL MODIFICAR--%>
    <div class="modal fade" id="ModificarModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 900px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="Titulo" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Trabajo</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxTrabajoModal" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Estado</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList runat="server" ID="DDLEstado" CssClass="form-control">
                                                <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <asp:UpdatePanel ID="UpdateModal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="col-md-12" runat="server" id="DivAlerta" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensajeModalError"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarModificacion" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarModificacion_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <%--    MODAL INFORMATIVA TAREA--%>
    <div class="modal fade" id="InformativoModalTarea" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title">Creación Subtarea</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="Lb1" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="Lb2" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarTarea" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarTarea_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
