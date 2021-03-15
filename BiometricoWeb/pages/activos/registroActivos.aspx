<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="registroActivos.aspx.cs" Inherits="BiometricoWeb.pages.activos.registroActivos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />

    <link href="/css/select2.css" rel="stylesheet" />

    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function openModal() { $('#ModalConfirmar').modal('show'); }
        function closeModal() { $('#ModalConfirmar').modal('hide'); }
        function editarModal() { $('#ModalEditar').modal('show'); }
        function closeEditar() { $('#ModalEditar').modal('hide'); }
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
                        <h2>Activos</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_Principal" data-toggle="tab" href="#nav-Principal" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-login" style=""></i>Registrar</a>
                <a class="nav-item nav-link" id="Registros" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-file" style=""></i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Principal" role="tabpanel" aria-labelledby="nav_crear">
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Categoria</h4>
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Categoría</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLCategorias" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLCategorias_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 grid-margin stretch-card" runat="server" id="DivGenerales" visible="false">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
                                <div class="row col-12">
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Tipo</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLTipo" runat="server" CssClass="select2 form-control custom-select" Width="100%"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Serie</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxSerie" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Inventario</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxInventario" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Responsable</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLResponsable" runat="server" CssClass="select2 form-control custom-select" Width="100%"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPSoftware" runat="server">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card" runat="server" id="DivSoftware" visible="false">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
                                <div class="row col-12">
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Tipo</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLTipoSW" runat="server" CssClass="select2 form-control custom-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Nombre</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxNombreSW" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Proveedor</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxProveedorSW" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Licencia</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxLicenciaSW" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Usuarios</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxUsuariosSW" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Version</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxVersionSW" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Lenguaje</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxLenguajeSW" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Responsable</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLResponsableSW" runat="server" CssClass="select2 form-control custom-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel runat="server" ID="UPRegistrar">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card" runat="server" id="DivRegistrar" visible="false">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Registrar</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Button ID="BtnCancelar" class="btn btn-secondary" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                        <asp:Button Text="Guardar" runat="server" CssClass="btn btn-success" ID="TxGuardar" OnClick="TxGuardar_Click"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="tab-pane fade" id="nav-Registros" role="tabpanel" aria-labelledby="nav_registros">
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
                                                    <asp:BoundField DataField="NombreEmpleado" HeaderText="Responsable" />
                                                    <asp:BoundField DataField="inventario" HeaderText="Inventario" />
                                                    <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnVer" runat="server" title="Actualizar inventario, serie y responsable" style="background-color:#f0ad4e" class="btn" CommandArgument='<%# Eval("idActivo") %>' CommandName="EditarActivo">
                                                                Editar
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

    <%--MODAL GUARDAR--%>
    <div class="modal fade" id="ModalConfirmar" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server" ID="UPCambio">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabe">
                                <asp:Literal ID="LtMensaje" Text="Titulo" runat="server"/>
                            </h4>
                            <small>Seguro que desea guadar este registro?</small>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server" ID="UPBody">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="row col-12" runat="server" id="DivInfoEquipo" visible="false">
                                    <div class="col-12">
                                        <label class="col-3">Tipo:</label>
                                        <asp:Label class="col-9" ID="Lb1" Text="" runat="server" Font-Bold="true" />
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Serie:</label>
                                        <asp:Label class="col-9" ID="Lb2" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Inventario:</label>
                                        <asp:Label class="col-9" ID="Lb3" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Responsable:</label>
                                        <asp:Label class="col-9" ID="Lb4" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                </div>

                                <div class="row col-12" runat="server" id="DivInfoSoftware" visible="false">
                                    <div class="col-12">
                                        <label class="col-3">Tipo:</label>
                                        <asp:Label class="col-9" ID="Lb5" Text="" runat="server" Font-Bold="true" />
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Nombre:</label>
                                        <asp:Label class="col-9" ID="Lb6" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Proveedor:</label>
                                        <asp:Label class="col-9" ID="Lb11" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Licencia:</label>
                                        <asp:Label class="col-9" ID="Lb7" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Usuarios:</label>
                                        <asp:Label class="col-9" ID="Lb8" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Versión:</label>
                                        <asp:Label class="col-9" ID="Lb9" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                    <div class="col-12">
                                        <label class="col-3">Lenguaje:</label>
                                        <asp:Label class="col-9" ID="Lb10" Text="" runat="server" Font-Bold="true"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnGuardarInfo" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnGuardarInfo_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL EDITAR--%>
    <div class="modal fade" id="ModalEditar" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabel">Editar Artículo 
                                <asp:Literal ID="TxIdModal" Text="" runat="server"/>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="row col-12">
                                    <div class="col-12">
                                        <label class="col-12">Numero de Serie:</label>
                                        <asp:TextBox runat="server" ID="TxModSerie" ReadOnly="true" CssClass="form-control"/>
                                    </div>
                                    <div class="col-12 mt-3">
                                        <label class="col-12">Codigo de Inventario:</label>
                                        <asp:TextBox runat="server" ID="TxModInventario" CssClass="form-control"/>
                                    </div>
                                    <div class="col-12 mt-3">
                                        <label class="col-12">Responsable:</label>
                                        <asp:DropDownList runat="server" ID="DDLModResponsable" CssClass="select2 form-control custom-select" Width="100%"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnEditar_Click"/>
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
