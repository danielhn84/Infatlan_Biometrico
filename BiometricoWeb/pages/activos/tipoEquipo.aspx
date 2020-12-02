<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="tipoEquipo.aspx.cs" Inherits="BiometricoWeb.pages.activos.tipoEquipo" %>
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
        function openModal() { $('#modalTipos').modal('show'); }
        function closeModal() { $('#modalTipos').modal('hide'); }
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
                        <h2>Tipo de Equipos</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Búsqueda</h4>
                        <div class="row"> 
                            <label class="col-2">Nombre</label>
                            <div class="col-7">
                                <asp:TextBox runat="server" ID="TxBusqueda" AutoPostBack="true" CssClass="form-control" OnTextChanged="TxBusqueda_TextChanged"></asp:TextBox>
                            </div>
                            <asp:Button Text="Agregar" runat="server" ID="BtnCrear" OnClick="BtnCrear_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Registros creados</h4>
                        <div class="row">
                            <div class="table-responsive">
                                <asp:GridView ID="GVBusqueda" runat="server"
                                    CssClass="mydatagrid"
                                    PagerStyle-CssClass="pgr"
                                    HeaderStyle-CssClass="align-self-lg-start"
                                    RowStyle-CssClass="rows"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    GridLines="None"
                                    PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="idTipoEquipo" HeaderText="No." ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="categoria" HeaderText="Categoria" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="fechaCreacion" HeaderText="Creado" ItemStyle-HorizontalAlign="Left"/>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <%--MODAL PARA CREAR TIPO DE EQUIPO--%>
    <div class="modal fade" id="modalTipos" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelPermiso">
                                <asp:Label ID="LbTitle" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UPCrearTipo" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-12">
                                    <label class="col-12 col-form-label">Categoría</label>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="DDLCategorias"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <label class="col-12 col-form-label">Nombre</label>
                                    <asp:TextBox ID="TxNombre" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <div class="col-sm-12" runat="server" id="DivMensaje" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                        <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensaje"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-inverse-dark" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnAceptar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
