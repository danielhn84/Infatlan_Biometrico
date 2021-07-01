<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="parkingRequest.aspx.cs" Inherits="BiometricoWeb.pages.servicios.parkingRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
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


    <asp:UpdatePanel runat="server" ID="UPBuzon">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Vehículos Registrados</h4>
                            <p>Ordenados por fecha de creación</p>
                            <div class="row">
                                <div class="row col-12 mt-4">
                                    <asp:LinkButton ID="BtnNewCar" runat="server" title="Editar" class="btn btn-success" style="padding:6px 15px" OnClick="BtnNewCar_Click">
                                        <i class="mdi mdi-plus text-white mr-1"></i><i style="font-style:normal; font-size:medium; display: inline-block; height: 100%; vertical-align: middle;">Nuevo</i>
                                    </asp:LinkButton>
                                </div>
                                <div class="table-responsive mt-4">
                                    <asp:GridView ID="GvCars" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pgr"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false"
                                        AllowPaging="true"
                                        GridLines="None" OnRowCommand="GvCars_RowCommand"
                                        PageSize="10" OnPageIndexChanging="GvCars_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="id" Visible="false" />
                                            <asp:BoundField DataField="modelo" HeaderText="Vehículo" />
                                            <asp:BoundField DataField="color" HeaderText="Color" />
                                            <asp:BoundField DataField="placa" HeaderText="Placa" />                                                    
                                            <asp:BoundField DataField="activo" HeaderText="Estado" />
                                            <asp:TemplateField HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BtnEliminar" runat="server" style="background-color:#f0ad4e" class="btn btn-warning" CommandArgument='<%# Eval("id") %>' CommandName="editarVehiculo">
                                                        <i class="mdi mdi-pencil text-white mr-1"> <i style="font-style:normal; font-weight:100; font-size:medium; display:inline-block; height:100%; vertical-align: middle;">Editar</i></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="BtnEntrar" runat="server" title="Borrar" style="background-color:lightcoral" class="btn" CommandArgument='<%# Eval("id") %>' CommandName="eliminarVehiculo">
                                                        <i class="mdi mdi-delete text-white" style="height:20px;" ></i>
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

                <div class="col-12 grid-margin stretch-card" id="DivSolicitud" runat="server" visible="false">
                    <div class="card">
                        <div class="card-body">
                            <h1 class="card-title">Solicitud</h1>
                            <p class="card-description">Información del solicitante.</p>
                            <br />
                            <div class="col-12">
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Solicitante :</label>
                                            <div class="col-9">
                                                <b><asp:Label Text="" ID="LbUser" runat="server" /></b>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Área :</label>
                                            <div class="col-10">
                                                <b><asp:Label Text="" ID="LbArea" runat="server" /></b>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Vehículo :</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLVehiculo"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2 col-form-label">Zona :</label>
                                            <div class="col-10">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLZona">
                                                    <asp:ListItem Text="Seleccione una opción" Value="0" />
                                                    <asp:ListItem Text="TGU" Value="1" />
                                                    <asp:ListItem Text="SPS" Value="2" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <asp:Button runat="server" Text="Enviar" ID="BtnEnviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-12 grid-margin stretch-card" id="DivInformación" runat="server" visible="false">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Datos Adicionales</h4>
                            <p>Información actual de su parqueo</p>
                            <div class="row">
                                <div class="table-responsive mt-4">
                                    <asp:GridView ID="GvSolicitud" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pgr"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false"
                                        AllowPaging="true"
                                        GridLines="None" OnRowCommand="GvSolicitud_RowCommand"
                                        PageSize="10">
                                        <Columns>
                                            <asp:BoundField DataField="id" Visible="false" />
                                            <asp:BoundField DataField="listaEspera" HeaderText="No. Lista" />
                                            <asp:BoundField DataField="nombreEstado" HeaderText="Estado" />
                                            <asp:BoundField DataField="nombreZona" HeaderText="Zona" />
                                            <asp:BoundField DataField="parqueoNo" HeaderText="No. Parqueo" />
                                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha" />
                                            <asp:TemplateField HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BtnEdit" title="Editar" runat="server" style="background-color:#f0ad4e" class="btn btn-warning" CommandArgument='<%# Eval("id") %>' CommandName="editRequest">
                                                        <i class="mdi mdi-pencil text-white mr-1"> <i style="font-style:normal; font-weight:100; font-size:medium; display:inline-block; height:100%; vertical-align: middle;">Editar</i></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="BtnBorrar" runat="server" title="Borrar" style="background-color:lightcoral" class="btn" CommandArgument='<%# Eval("id") %>' CommandName="eliminarSolicitud">
                                                        <i class="mdi mdi-delete text-white" style="height:20px;" ></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                                <div class="row col-12">
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--MODAL DE VEHICULO--%>
    <div class="modal fade" id="ModalVehiculos" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelModificacion">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LbTitle" runat="server" Text=""></asp:Label>
                                <asp:Label ID="LbCarId" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-12">
                                    <div class="row">
                                        <label class="col-3 col-form-label">Marca :</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxMarca" CssClass="form-control"/>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <label class="col-3 col-form-label">Color :</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxColor" CssClass="form-control"/>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <label class="col-3 col-form-label">Placa :</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxPlaca" CssClass="form-control"/>
                                        </div>
                                    </div>

                                    <div class="row mt-2" runat="server" id="DivEstado" visible="false">
                                        <label class="col-3 col-form-label">Estado :</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLEstado" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Value="1" Text="Activo"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-12 mt-3" runat="server" id="DivMensaje" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                        <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensaje"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateModificacionBotones" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnAceptar" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnAceptar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE QUITAR VEHICULO--%>
    <div class="modal fade" id="ModalConfirmar" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        Esta seguro que desea eliminar este vehículo?
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LbCarIdDelete" runat="server" Visible="false" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnConfirmar" runat="server" Text="Eliminar" class="btn btn-danger" OnClick="BtnConfirmar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE EDITAR SOLICITUD--%>
    <div class="modal fade" id="ModalEditReq" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                Editar Solicitud
                                <asp:Label ID="LbIdReq" runat="server" Text="" Visible="false"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-12">
                                    <div class="row">
                                        <label class="col-3 col-form-label">Zona :</label>
                                        <div class="col-9">
                                            <asp:DropDownList runat="server" ID="DDLZonaEdit" CssClass="form-control">
                                                <asp:ListItem Text="Seleccione una opción" Value="0" />
                                                <asp:ListItem Text="TGU" Value="1" />
                                                <asp:ListItem Text="SPS" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-12 mt-3" runat="server" id="Div2" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                        <asp:Label runat="server" CssClass="col-form-label text-white" ID="Label3"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnEditarReq" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnEditarReq_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE BORRAR SOLICITUD--%>
    <div class="modal fade" id="ModalDeleteReq" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                Seguro que desea eliminar esta solicitud?
                                <asp:Label ID="LbIdRequest" runat="server" Text="" Visible="false"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnDeleteReq" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnDeleteReq_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
