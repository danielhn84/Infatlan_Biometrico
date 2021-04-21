<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="visitaDCPersonal.aspx.cs" Inherits="BiometricoWeb.pages.activos.visitaDCPersonal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <script type="text/javascript">
        function openModal() { $('#modalEquipos').modal('show'); }
        function closeModal() { $('#modalEquipos').modal('hide'); }
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
                        <h2>Personal</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                    <asp:Label Text="" Visible="false" ID="LbIdSolicitud" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UPInternos" runat="server" Visible="false">
        <ContentTemplate>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Personal Interno</h4>
                        <div class="row">
                            <div class="table-responsive">
                                <asp:GridView ID="GvInternos" runat="server"
                                    CssClass="mydatagrid"
                                    PagerStyle-CssClass="pgr"
                                    HeaderStyle-CssClass="align-self-lg-start"
                                    RowStyle-CssClass="rows"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    GridLines="None" OnRowCommand="GvInternos_RowCommand"
                                    PageSize="10" OnPageIndexChanging="GvInternos_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="codigoEmpleado" HeaderText="No." ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPExternos" runat="server" Visible="false">
        <ContentTemplate>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Personal Externo</h4>
                        <div class="row">
                            <div class="table-responsive">
                                <asp:GridView ID="GvExterno" runat="server"
                                    CssClass="mydatagrid"
                                    PagerStyle-CssClass="pgr"
                                    HeaderStyle-CssClass="align-self-lg-start"
                                    RowStyle-CssClass="rows"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    GridLines="None" OnRowCommand="GvExterno_RowCommand"
                                    PageSize="10" OnPageIndexChanging="GvExterno_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="idRow" HeaderText="No." ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="identidad" HeaderText="Identidad" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="empresaNombre" HeaderText="Empresa" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="cantEquipos" HeaderText="Equipos" ItemStyle-HorizontalAlign="Left"/>
                                        <asp:TemplateField HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Button ID="BtnRegistrar" runat="server" Text="Equipo" class="btn btn-inverse-primary  mr-2" CommandArgument='<%# Eval("idRow") %>' CommandName="addEquipo" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="UPRegistrar">
        <ContentTemplate>
            <div class="col-12 grid-margin stretch-card" runat="server" id="DivRegistrar" visible="true">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Registrar</h4>
                        <div class="row">
                            <div class="col-12">
                                <asp:Button ID="BtnCancelar" class="btn btn-secondary" runat="server" Text="Cancelar" />
                                <asp:Button Text="Guardar" runat="server" CssClass="btn btn-success" ID="BtnGuardar" OnClick="BtnGuardar_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--MODAL PARA AGREGAR EQUIPO--%>
    <div class="modal fade" id="modalEquipos" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelPermiso">
                                <asp:Label ID="LbTitle" runat="server" Text="Registrar Equipo"></asp:Label>
                                <asp:Label ID="LbIdPersona" runat="server" Text=""></asp:Label>
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
                                    <label class="col-12 col-form-label">Tipo de Equipo</label>
                                    <asp:DropDownList runat="server" ID="DDLTipo" CssClass="form-control"></asp:DropDownList>
                                </div>

                                <div class="col-12">
                                    <label class="col-12 col-form-label">Serie</label>
                                    <asp:TextBox ID="TxSerie" class="form-control" runat="server"></asp:TextBox>
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
