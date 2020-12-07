<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="docGrupos.aspx.cs" Inherits="BiometricoWeb.pages.documentacion.docGrupos" %>

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
        function openModal() { $('#ModalCrear').modal('show'); }
        function openModalVer() { $('#ModalVer').modal('show'); }
        function closeModal() { $('#ModalCrear').modal('hide'); }
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
                        <h2>Grupos</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server">
        <ContentTemplate>
            <div class="row" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <div class="col-md-12 row">
                                <div class="row col-12">
                                    <label class="col-sm-2 col-form-label">Buscar</label>
                                    <div class="col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxBuscar" runat="server" placeholder="Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscar_TextChanged"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-2">
                                        <asp:Button Text="Nuevo" runat="server" CssClass="btn btn-primary" ID="BtnNuevo" OnClick="BtnNuevo_Click"/>
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
                                                    <asp:BoundField DataField="idGrupo" HeaderText="No." ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="empleado" HeaderText="Usuario Creación" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:TemplateField HeaderStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnVer" runat="server" title="Ver" Visible="true" style="background-color:#5cb85c" class="btn" CommandArgument='<%# Eval("idGrupo") %>' CommandName="verGrupo">
                                                                <i class="mdi mdi-eye text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar" runat="server" title="Editar" Visible="true" style="background-color:#f0ad4e" class="btn" CommandArgument='<%# Eval("idGrupo") %>' CommandName="editarGrupo">
                                                                <i class="mdi mdi-pencil text-white"></i>
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

    <%--MODAL CREAR GRUPO--%>
    <div class="modal fade" id="ModalCrear" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelDescarga"> 
                                <asp:Literal runat="server" ID="LitTitulo"></asp:Literal>
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
                                        <label class="col-4 col-form-label">Nombre</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxNombre" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Empleado</label>
                                        <div class="col-7">
                                            <asp:DropDownList runat="server" ID="DDLEmpleado" CssClass="select2 form-control custom-select" style="width:100%"></asp:DropDownList>
                                        </div>
                                        <div class="col-1">
                                            <asp:Button Text="+" ID="BtnAgregarEmpleado" runat="server" CssClass="btn btn-success" OnClick="BtnAgregarEmpleado_Click" />
                                        </div>
                                    </div>
                                </div>
                                

                                <div class="row col-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvEmpleados" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None" OnPageIndexChanging="GvEmpleados_PageIndexChanging"
                                            PageSize="5" OnRowCommand="GvEmpleados_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="idEmpleado" Visible="false" />
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                                <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnBorrar" runat="server" title="Borrar" Style="background-color: #d9534f" class="btn" CommandArgument='<%# Eval("idEmpleado") %>' CommandName="BorrarEmpleado">
                                                                <i class="mdi mdi-delete text-white"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>


                                
                                <div class="row col-12" runat="server" visible="false" id="DivEstado">
                                    <div class="col-6 mt-3">
                                        <label class="col-4 col-form-label">Estado</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLEstado" CssClass="form-control">
                                                <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 mt-3" runat="server" id="DivMensaje" visible="false" style="display: flex; background-color:tomato; justify-content:center">
                                    <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensaje"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEditar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnEditar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL VER GRUPO--%>
    <div class="modal fade" id="ModalVer" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
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
                                <div class="row col-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvViewEmpleados" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="table"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None" OnPageIndexChanging="GvViewEmpleados_PageIndexChanging"
                                            PageSize="5">
                                            <Columns>
                                                <asp:BoundField DataField="idEmpleado" HeaderText="ID" />
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
