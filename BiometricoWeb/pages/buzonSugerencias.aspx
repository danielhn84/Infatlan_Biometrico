<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="buzonSugerencias.aspx.cs" Inherits="BiometricoWeb.pages.buzonSugerencias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script type="text/javascript">
        function openModal() { $('#ModalInfo').modal('show'); }
    </script>

    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
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

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#navCompensatorio" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-message-text"> </i>Nuevo</a>
                <a class="nav-item nav-link" id="A1" data-toggle="tab" href="#navMiBuzon" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-account" > </i>Mis Sugerencias</a>
                <a runat="server" visible="false" class="nav-item nav-link" id="BuzonGenerales" data-toggle="tab" href="#navRegistros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-folder" > </i>Sugerencias</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="navCompensatorio" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel runat="server" ID="UPBuzon">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h1 class="card-title">Buzón de Sugerencias</h1>
                                    <b><p class="mb-md-0 card-description" style="font-weight:500;">¡Es un gusto ayudarte!</p></b>
                                    <p class="card-description">Para asistirte de la mejor manera, requerimos que completes la siguiente información:</p>
                                    <br />
                                    <div class="col-12">
                                        <div class="row">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Motivo</label>
                                                    <div class="col-9">
                                                        <asp:DropDownList runat="server" ID="DDLMotivo" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Urgencia</label>
                                                    <div class="col-9">
                                                        <asp:DropDownList runat="server" ID="DDLUrgencia" CssClass="form-control">
                                                            <asp:ListItem Value="0" Text="Seleccione"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Baja"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Media"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Alta"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-12">
                                                <div class="form-group row">
                                                    <label class="col-1 col-form-label">Mensaje</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" PlaceHolder="Describe tu experiencia..." TextMode="MultiLine" Rows="6" ID="TxMensaje" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-12">
                            <asp:Button runat="server" Text="Enviar" ID="BtnEnviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click"/>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="tab-pane fade" id="navMiBuzon" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Buzón de solicitudes</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GvMensajes" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GvMensajes_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GvMensajes_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="idSugerencia" HeaderText="No." />
                                                    <asp:BoundField DataField="TipoMensaje" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="urgencia" HeaderText="Urgencia" />
                                                    <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" />
                                                    <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar2" runat="server" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idSugerencia") %>' CommandName="VerMensaje">
                                                                <i class="mdi mdi-email text-success" ></i>
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

        <div class="tab-pane fade" id="navRegistros" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UPBuzonGeneral" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Permisos creados</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GVBusqueda_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="idSugerencia" HeaderText="No." />
                                                    <asp:BoundField DataField="TipoMensaje" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="sugerencia" Visible="false" HeaderText="Mensaje" />
                                                    <asp:BoundField DataField="urgencia" HeaderText="Urgencia" />
                                                    <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" />
                                                    <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar2" runat="server" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idSugerencia") %>' CommandName="VerMensaje">
                                                                <i class="mdi mdi-email text-success" ></i>
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

    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="ModalInfo" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelConfirmar">
                                <b><asp:Label runat="server" ID="LbTitulo" CssClass="col-form-label"></asp:Label></b>
                            </h4>
                            <asp:Label runat="server" ID="LbMensaje" CssClass="col-form-label"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            </div>
        </div>
    </div>
            
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
