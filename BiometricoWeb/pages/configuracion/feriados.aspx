<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="feriados.aspx.cs" Inherits="BiometricoWeb.pages.configuracion.feriados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function openModal() { $('#PuestosModal').modal('show'); }
        function openModalDescriptor() { $('#ModalDescriptor').modal('show'); }


        function mostrarDepto() { $('#DivDepto').style('display', 'block'); }



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

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server">
        <ContentTemplate>
            <div class="row" id="DivBusqueda" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Días Feriados</h4>
                            <p>Listado de días de vacaciones nacionales e internacionales</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Días Feriados</h4>
                        <p>Listado de días de vacaciones nacionales e internacionales.</p>
                        <div class="col-md-12">
                            <div class="form-group row">
                                <label class="col-sm-1 col-form-label">Buscar</label>
                                <div class="col-sm-6">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TxBuscarPuesto" runat="server" placeholder="Ej. Programador - Presione afuera para proceder" class="form-control"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button ID="BtnNuevo" runat="server" Text="Nuevo Feriado" class="btn btn-primary" OnClick="BtnNuevo_Click" />
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
                                                PageSize="10"
                                                AutoGenerateColumns="false" >
                                                <Columns>
                                                    <asp:BoundField DataField="idFechasFestivas" HeaderText="Id Puesto" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="dia" HeaderText="Dia" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="monthNombre" HeaderText="Mes" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="tipo" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="activo" HeaderText="Estado" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderStyle-Width="60px" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnPuestoModificar" runat="server" Text="Modificar" class="btn btn-inverse-primary  mr-2" CommandArgument='<%# Eval("idFechasFestivas") %>' CommandName="ActualizarEstado" />
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


    <%--MODAL DE MODIFICACION--%>
    <div class="modal fade" id="modalDia" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LbTitulo" runat="server" Text=""></asp:Label>
                                <asp:Label ID="LbDiaId" Visible="false" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateModificarPuesto" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-12">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Nombre</label>
                                            <div class="col-9">
                                                <asp:TextBox ID="TxNombre" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Mes</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLMes">
                                                    <asp:ListItem Value = "0" Text = "Seleccione una opción." ></asp:ListItem>
                                                    <asp:ListItem Value = "1" Text = "Enero" ></asp:ListItem>
                                                    <asp:ListItem Value = "2" Text = "Febrero" ></asp:ListItem>
                                                    <asp:ListItem Value = "3" Text = "Marzo" ></asp:ListItem>
                                                    <asp:ListItem Value = "4" Text = "Abril" ></asp:ListItem>
                                                    <asp:ListItem Value = "5" Text = "Mayo" ></asp:ListItem>
                                                    <asp:ListItem Value = "6" Text = "Junio" ></asp:ListItem>
                                                    <asp:ListItem Value = "7" Text = "Julio" ></asp:ListItem>
                                                    <asp:ListItem Value = "8" Text = "Agosto" ></asp:ListItem>
                                                    <asp:ListItem Value = "9" Text = "Septiembre" ></asp:ListItem>
                                                    <asp:ListItem Value = "10" Text = "Octubre" ></asp:ListItem>
                                                    <asp:ListItem Value = "11" Text = "Noviembre" ></asp:ListItem>
                                                    <asp:ListItem Value = "12" Text = "Diciembre" ></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Día</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLDia"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Tipo</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLTipo" AutoPostBack="true" OnSelectedIndexChanged="DDLTipo_SelectedIndexChanged">
                                                    <asp:ListItem Text="Seleccione una opción." Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Feriado Internacional" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Feriado Nacional" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12" id="DivDepto" runat="server" visible="false">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Departamentos</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLDepto" AutoPostBack="true" OnSelectedIndexChanged="DDLDepto_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12" id="DivMunicipio" runat="server" visible="false">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Municipio</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLmunicipio"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Duracion</label>
                                            <div class="col-9">
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="DDLDuracion" AutoPostBack="true" OnSelectedIndexChanged="DDLDuracion_SelectedIndexChanged">
                                                    <asp:ListItem Text="Día Completo" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Parcial" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12" id="DivParcial" runat="server" visible="false">
                                        <div class="form-group row">
                                            <label class="col-3 col-form-label">Hora Inicio</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxInicio" TextMode="Time" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12" runat="server" id="DivEstado" visible="false">
                                        <div class="form-group row">
                                            <label class="col-sm-3 col-form-label">Estado</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="DDLEstado" runat="server" class="form-control">
                                                    <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
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

    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="modalConfirm" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="Label1" runat="server" Text="Seguro que desea borrar este día?"></asp:Label>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnBorrar" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnBorrar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
