<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="responsableActivos.aspx.cs" Inherits="BiometricoWeb.pages.activos.responsableActivos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/select2.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />

    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function openModal() { $('#ModalActivo').modal('show'); }
        function closeModal() { $('#ModalActivo').modal('hide'); }
        function openAsign() { $('#ModalAsignar').modal('show'); }
        function closeAsign() { $('#ModalAsignar').modal('hide'); }
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
                        <h2>Responsable</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div runat="server" visible="true">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="misActivos" data-toggle="tab" href="#nav_misActivos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-file" style=""></i>Mis Activos</a>
                <a class="nav-item nav-link" id="asignados" data-toggle="tab" href="#nav_asignados" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-file" style=""></i>Asignaciones</a>
                <a class="nav-item nav-link" id="pendientes" data-toggle="tab" href="#nav_pendientes" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-bookmark" style=""></i>Pendientes</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav_misActivos" role="tabpanel" aria-labelledby="nav_crear">
            <br />
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Activos Asignados</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GvMisActivos" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="10" OnPageIndexChanging="GvMisActivos_PageIndexChanging"> 
                                                <Columns>
                                                    <asp:BoundField DataField="idActivo" HeaderText="ID" />
                                                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                                                    <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnVer" runat="server" title="Ver" style="background-color:#5bc0de" class="btn" CommandArgument='<%# Eval("idActivo") %>' CommandName="infoActivo">
                                                                <i class="mdi mdi-information text-white mr-2"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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

        <div class="tab-pane fade" id="nav_asignados" role="tabpanel" aria-labelledby="nav_crear">
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Activos Asignados</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GvActivos" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GvActivos_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GvActivos_PageIndexChanging"> 
                                                <Columns>
                                                    <asp:BoundField DataField="idActivo" HeaderText="ID" />
                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                    <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="Asignado" HeaderText="Estado" />
                                                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnAsignar" runat="server" title="Entrar" style="background-color:#5bc0de" class="btn" CommandArgument='<%# Eval("idActivo") %>' CommandName="AsignarActivo">
                                                                <i class="mdi mdi-arrow-right-bold-circle-outline text-white mr-2"></i>Asignar
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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

        <div class="tab-pane fade" id="nav_pendientes" role="tabpanel" aria-labelledby="nav_crear">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Tipos de documentos</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GvBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GvBusqueda_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GvBusqueda_PageIndexChanging" > 
                                                <Columns>
                                                    <asp:BoundField DataField="idActivo" HeaderText="ID" />
                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnVer" runat="server" title="Entrar" style="background-color:#5bc0de" class="btn" CommandArgument='<%# Eval("idActivo") %>' CommandName="VerActivo">
                                                                <i class="mdi mdi-arrow-right-bold-circle-outline text-white mr-2"></i>Completar
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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

    <%--MODAL ACTIVO--%>
    <div class="modal fade" id="ModalActivo" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelModificacionTipo">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>Activo
                                <asp:Label ID="LbIdActivo" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="row" runat="server" visible="false" id="DivHW">
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Marca</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxMarca" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Modelo</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxModelo" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Memoria</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxMemoria" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Disco</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxDisco" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Procesador</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxProcesador" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Ubicación</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxUbicacion" Rows="2" TextMode="MultiLine" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" runat="server" visible="false" id="DivRED">
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Marca</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxMarcaRED" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Modelo</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxModeloRED" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Ubicación</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxUbicacionRED" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" runat="server" visible="false" id="DivUPS">
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Marca</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxMarcaUPS" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Modelo</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxModeloUPS" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Kva</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxKvaUPS" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Watts</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxWattsUPS" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Ubicación</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxUbicacionUPS" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnAceptar_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL ASIGNAR--%>
    <div class="modal fade" id="ModalAsignar" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelAsignar">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>Activo
                                <asp:Label ID="LbIdActivoAsignar" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="col-12 row">
                        <div class="form-group col-12">
                            <label class="col-12">Empleado</label>
                            <div class="col-12">
                                <asp:DropDownList runat="server" AutoPostBack="true" ID="DDLEmpleado" CssClass="select2 form-control custom-select" Width="100%"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group col-12">
                            <label class="col-12">Autorizado para Salir</label>
                            <div class="col-12">
                                <asp:DropDownList runat="server" ID="DDLAutorizado" CssClass="form-control">
                                    <asp:ListItem Text="Si" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnAsignar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnAsignar_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="/js/select2.js"></script>
    <link href="/css/select2.css" rel="stylesheet" />
</asp:Content>
