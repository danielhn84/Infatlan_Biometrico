<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="tipoDocumentos.aspx.cs" Inherits="BiometricoWeb.pages.documentacion.tipoDocumentos" %>
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
    <script type="text/javascript">
        function openModal() { $('#ModalEditarDoc').modal('show'); }
        function openModalRef() { $('#ModalReferencias').modal('show'); }
        function closeModal() { $('#ModalEditarDoc').modal('hide'); }
    </script>
    <style>
        .hiddencol { display: none; }
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
                        <h2>
                            <asp:Literal Text="" runat="server" ID="TxTitulo" />
                        </h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <a href="crearDocumentos.aspx" class="btn btn-primary">Volver</a>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server">
        <ContentTemplate>
            <div class="row" id="DivBusqueda" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="col-sm-12 mb-2 row" runat="server" visible="false">
                                        <div class="col-6 row">
                                            <label class="col-sm-2 col-form-label">Depto</label>
                                            <div class="col-10">
                                                <asp:DropDownList runat="server" ID="DDLDepartamento" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLDepartamento_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-5 row">
                                            <label class="col-sm-2 col-form-label">Area</label>
                                            <div class="col-10">
                                                <asp:DropDownList runat="server" ID="DDLArea" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-1 row">
                                            <asp:Button Text="Buscar" runat="server" CssClass="btn btn-primary" ID="BtnBuscar" OnClick="BtnBuscar_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="col-md-12 row">
                                <div class="row col-6">
                                    <label class="col-sm-2 col-form-label">Buscar</label>
                                    <div class="col-sm-10">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxBuscar" runat="server" placeholder="Ej. Gerencia - Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscar_TextChanged"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="Div1" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="align-self-lg-start"
                                                RowStyle-CssClass="rows"
                                                GridLines="None"
                                                AllowPaging="true"
                                                PageSize="10" OnRowCommand="GVBusqueda_RowCommand"
                                                AutoGenerateColumns="false" OnPageIndexChanging="GVBusqueda_PageIndexChanging" >
                                                <Columns>
                                                    <asp:BoundField DataField="idDocumento" Visible="false" HeaderText="Id" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnNiveles" runat="server" Text="" title="" class="btn btn-default"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="codigo" HeaderText="Código" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="nivelConfidencialidad" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="activo" HeaderText="Estado" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="firma" HeaderText="Firma" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnReferencias" runat="server" title="Referencias" style="background-color:#0b5a79" class="btn" CommandArgument='<%# Eval("idDocumento") %>' CommandName="docReferencias">
                                                                <i class="mdi mdi-link-variant text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnVer" runat="server" Text="Entrar" style="background-color:#5cb85c" class="btn text-white font-weight-bold" CommandArgument='<%# Eval("idDocumento") %>' CommandName="verDocumento" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar" runat="server" title="Editar" style="background-color:#f0ad4e" class="btn" CommandArgument='<%# Eval("idDocumento") %>' CommandName="editarDoc">
                                                                <i class="mdi mdi-cogs text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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

    <%--MODAL EDITAR DOCUMENTO--%>
    <div class="modal fade" id="ModalEditarDoc" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelDescarga">
                                <asp:Literal runat="server" ID="LiEditarDoc"></asp:Literal>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Vigencia</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLVigencia" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Actual"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Historico"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Nombre</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control" ID="TxNombre" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4">Confirmación de lectura</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLConfirmacion" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4">Nivel Confidencial</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLNivelConfidencialidad" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="DivCorreos" visible="false" class="row col-12 mt-3">   
                                    <div class="row col-6">   
                                        <label class="col-4 col-form-label">Fecha Envío</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" ID="TxFecha" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Recordatorios</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLRecordatorios" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Cada 3 días"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Cada 7 días"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="row col-12">
                                    <div class="row col-6 mt-3">
                                        <label class="col-4 col-form-label">Estado</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLEstado" CssClass="form-control">
                                                <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6 mt-3">
                                        <div class="col-5">
                                            <label class="col-form-label">Derechos de autor</label>
                                        </div>
                                        <div class="col-7">
                                            <label class="col-form-label custom-control custom-switch m-b-0">
                                                <input type="checkbox" runat="server" id="CBxConfidencial" class="custom-control-input">
                                                <span class="custom-control-label"></span>
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 mt-3" runat="server" id="DivMensaje" visible="false" style="display: flex; background-color:tomato; justify-content:center">
                                    <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbAdvertencia"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEditarDoc" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnEditarDoc_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL REFERENCIAS--%>
    <div class="modal fade" id="ModalReferencias" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="row col-12">
                                    <div class="col-12">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GvReferencias" runat="server"
                                                        CssClass="mydatagrid"
                                                        PagerStyle-CssClass="pgr"
                                                        HeaderStyle-CssClass="align-self-lg-start"
                                                        RowStyle-CssClass="rows"
                                                        GridLines="None"
                                                        AllowPaging="true"
                                                        PageSize="10" OnRowCommand="GvReferencias_RowCommand"
                                                        AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:BoundField DataField="idReferencia" HeaderText="Id" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="codigo" HeaderText="Código" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="nombre" HeaderText="Documento" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnVer" runat="server" Text="Entrar" style="background-color:#5cb85c" class="btn text-white font-weight-bold" CommandArgument='<%# Eval("idReferencia") %>' CommandName="verDocumento" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
