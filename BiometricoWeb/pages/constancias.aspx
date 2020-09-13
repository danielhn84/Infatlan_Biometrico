<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="constancias.aspx.cs" Inherits="BiometricoWeb.pages.constancias" %>
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
        function openModal() { $('#ModalConfirmar').modal('show'); }
        function closeModal() { $('#ModalConfirmar').modal('hide'); }
        function openInfo() { $('#ModalInfo').modal('show'); }
        function openDescarga() { $('#DescargaModal').modal('show'); }
        function closeDescarga() { $('#DescargaModal').modal('hide'); }
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
                <a class="nav-item nav-link" id="A1" data-toggle="tab" href="#navMisSolicitudes" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-account" > </i>Mis Solicitudes</a>
                <a runat="server" visible="false" class="nav-item nav-link" id="ConstanciasGenerales" data-toggle="tab" href="#navPendientes" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-tag" > </i>Pendientes</a>
                <a runat="server" visible="false" class="nav-item nav-link" id="Busquedas" data-toggle="tab" href="#navRegistros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-history" > </i>Histórico</a>
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
                                    <h1 class="card-title">Solicitud de Constancias</h1>
                                    <p class="card-description">Favor completa la siguiente información.</p>
                                    <br />
                                    <div class="col-12">
                                        <div class="row">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3">Tipo de Constancia</label>
                                                    <div class="col-9">
                                                        <asp:DropDownList runat="server" AutoPostBack="true" ID="DDLTipoConstancia" CssClass="form-control" OnSelectedIndexChanged="DDLTipoConstancia_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6" runat="server" id="DivCategorias">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Categoria</label>
                                                    <div class="col-10">
                                                        <asp:DropDownList runat="server" AutoPostBack="true" Enabled="false" ID="DDLCategoria" CssClass="form-control" OnSelectedIndexChanged="DDLCategoria_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h1 class="card-title">Datos Adicionales</h1>
                                    <div class="col-12">
                                        <div class="row" runat="server" id="DivFinanciar" visible="false">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Monto</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" TextMode="Number" ID="TxMonto" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2">Plazo (meses)</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxPlazo" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <div class="form-group row">
                                                    <label class="col-1 col-form-label">Destino (detalle)</label>
                                                    <div class="col-5">
                                                        <asp:TextBox runat="server" ID="TxDest1" PlaceHolder="Deudor1" CssClass="form-control"></asp:TextBox>
                                                        <asp:TextBox Visible="false" runat="server" TextMode="MultiLine" Rows="6" PlaceHolder="Favor detallar los montos y el nombre de sus deudores." ID="TxDestino" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-3">
                                                        <asp:TextBox runat="server" ID="TxMont1" PlaceHolder="Lps." CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-2">
                                                        <asp:LinkButton runat="server"  CssClass="btn btn-success" ID="BtnAgregar" OnClick="BtnAgregar_Click">
                                                            <i class="mdi mdi-plus" ></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-12" runat="server" id="Line2" style="margin-top:-3%" visible="false">
                                                <div class="form-group row">
                                                    <div class="col-1"></div>
                                                    <div class="col-5">
                                                        <asp:TextBox runat="server" ID="TxDest2" PlaceHolder="Deudor2" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-3">
                                                        <asp:TextBox runat="server" ID="TxMont2" PlaceHolder="Lps." CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-12" runat="server" id="Line3" visible="false">
                                                <div class="form-group row">
                                                    <div class="col-1"></div>
                                                    <div class="col-5">
                                                        <asp:TextBox runat="server" ID="TxDest3" PlaceHolder="Deudor3" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-3">
                                                        <asp:TextBox runat="server" ID="TxMont3" PlaceHolder="Lps." CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-12" runat="server" id="Line4" visible="false">
                                                <div class="form-group row">
                                                    <div class="col-1"></div>
                                                    <div class="col-5">
                                                        <asp:TextBox runat="server" ID="TxDest4" PlaceHolder="Deudor4" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-3">
                                                        <asp:TextBox runat="server" ID="TxMont4" PlaceHolder="Lps." CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DivDestino" visible="false">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Destino</label>
                                                    <div class="col-9">
                                                        <asp:DropDownList runat="server" ID="DDLDestinoCL" AutoPostBack="true" OnSelectedIndexChanged="DDLDestinoCL_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <div class="row" runat="server" visible="false" id="DivAval">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2">Nombre del Aval</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxAval" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Parentezco</label>
                                                    <div class="col-9">
                                                        <textbox id="asd" Text="Hola"></textbox>
                                                        <asp:DropDownList runat="server" ID="DDLParentezco" CssClass="form-control">
                                                            <asp:ListItem Value="0" Selected="True" Text="Seleccione.." />
                                                            <asp:ListItem Value="1" Text="Padre/Madre" />
                                                            <asp:ListItem Value="2" Text="Hijo/a" />
                                                            <asp:ListItem Value="3" Text="Hermano/a" />
                                                            <asp:ListItem Value="4" Text="Tio/a" />
                                                            <asp:ListItem Value="5" Text="Amigo/a" />
                                                            <asp:ListItem Value="0" Text="Otro" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DivVisa" visible="false">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Representante</label>
                                                    <div class="col-9">
                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="DDLFirmante"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Fecha Emisión</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxFecha" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Pasaporte</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxPasaporte" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">RTN</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxRTN" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Domicilio 1</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxDomicilio1" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Contacto</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxContacto" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Domicilio 2</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxDomicilio2" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Lugar</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxLugar" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Ciudad</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxCiudad" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Teléfono</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxTelefono" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Fecha Inicio</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" TextMode="Date" ID="TxFechaInicio" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Evento</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxEvento" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Fecha Fin</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" TextMode="Date" ID="TxFechaFin" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3 col-form-label">Consulado</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxConsulado" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-3">Dirección del Consulado</label>
                                                    <div class="col-9">
                                                        <asp:TextBox runat="server" ID="TxDirConsul" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" visible="false" id="DivEmbajada">
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2">Nombre Embajada</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxEmbajada" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2">Fecha de la Cita</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxFechaCita" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-12">
                            <asp:Button runat="server" Text="Enviar" ID="BtnEnviar" CssClass="btn btn-primary" OnClick="BtnEnviar_Click" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnEnviar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="tab-pane fade" id="navMisSolicitudes" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Constancias Solicitadas</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GvMisConstancias" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true" OnRowDataBound="GvMisConstancias_RowDataBound"
                                                GridLines="None" OnRowCommand="GvMisConstancias_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GvMisConstancias_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="No." />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" />                                                    
                                                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                                                    <asp:BoundField DataField="Mensaje" HeaderText="Mensaje" ItemStyle-Width="40%" />
                                                    <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar" runat="server" title="Eliminar" style="background-color:transparent" class="btn btn-inverse" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="EliminarMensaje">
                                                                <i class="mdi mdi-delete text-gray"></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="BtnDownload" runat="server" title="Info" style="background-color:transparent" class="btn btn-inverse" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="DescargarInfo">
                                                                <i class="mdi mdi-download text-gray"></i>
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

        <div class="tab-pane fade" id="navPendientes" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UPBuzonGeneral" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Solicitudes de constancias pendientes de aprobar</h4>
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
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="ID" />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                    <asp:BoundField DataField="Destino" HeaderText="Destino" />
                                                    <asp:BoundField DataField="estado" HeaderText="Estado"/>
                                                    <asp:BoundField DataField="usuario" HeaderText="Usuario" />                                                    
                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" />                                                    
                                                    <asp:TemplateField HeaderText="Seleccione" ItemStyle-Width="120">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar2" runat="server" title="Responder" style="background-color:transparent;"  class="btn btn-inverse" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="ResponderSolicitud">
                                                                <i class="mdi mdi-reply text-gray" ></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="LbVer" runat="server" title="Abrir" style="background-color:transparent;"  class="btn btn-inverse" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="VerSolicitud">
                                                                <i class="mdi mdi-email-open text-gray" ></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="LbBorrar" runat="server" title="Borrar" style="background-color:transparent;"  class="btn btn-inverse" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="EliminarSolicitud">
                                                                <i class="mdi mdi-delete text-gray" ></i>
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
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Solicitudes de constancias creadas</h4>

                                    <div class="row">   
                                        <div class="col-1">
                                            <label class="col-form-label">Estado:</label>
                                        </div>
                                        <div class="col-4">
                                            <asp:DropDownList runat="server" AutoPostBack="true" CssClass="form-control" ID="DDLEstadoConstancia" OnSelectedIndexChanged="DDLEstadoConstancia_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVHistorico" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GVHistorico_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GVHistorico_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="ID" />
                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                    <asp:BoundField DataField="Destino" HeaderText="Destino" />
                                                    <asp:BoundField DataField="estado" HeaderText="Estado"/>
                                                    <asp:BoundField DataField="usuario" HeaderText="Usuario" />                                                    
                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" />                                                    
                                                    <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LbVer" runat="server" title="Abrir" style="background-color:transparent;"  class="btn btn-inverse" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="VerSolicitudHistorial">
                                                                <i class="mdi mdi-email-open text-gray" ></i>
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

    <%--MODAL DE INFO--%>
    <div class="modal fade" id="ModalInfo" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelModificacion">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="LbTituloInfo" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div runat="server" id="DivModFinanc" class="col-12" visible="false"> 
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Monto:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbMonto"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Plazo:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbPlazo"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-9">
                                            <div class="form-group row">
                                                <label class="col-2" style="text-align:right">Detalle:</label>
                                                <div class="col-10">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbDetalle"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DivModAval" class="col-12" visible="false"> 
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Nombre:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbAval"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-4" style="text-align:right">Parentezco:</label>
                                                <div class="col-8">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbParentezco"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DivModEmbajada" class="col-12" visible="false"> 
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-4" style="text-align:right">Embajada:</label>
                                                <div class="col-8">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbEmbajada"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Fecha:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbFechaCita"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DivModCapa" class="col-12" visible="false"> 
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-4" style="text-align:right">Representante:</label>
                                                <div class="col-8">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbRepresentante"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-4" style="text-align:right">Fecha Emisión:</label>
                                                <div class="col-8">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbEmision"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Pasaporte:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbPasaporte"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">RTN:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbRTN"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Domicilio 1:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbDomicilio1"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Domicilio 2:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbDomicilio2"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Contacto:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbContacto"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Lugar:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbLugar"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Telefono:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbTelefono"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Evento:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbEvento"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Inicio:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbInicio"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Fin:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbFin"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-3" style="text-align:right">Consulado:</label>
                                                <div class="col-9">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbConsulado"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-4" style="text-align:right">Dirección del Consulado:</label>
                                                <div class="col-8">
                                                    <asp:Label runat="server" CssClass="col-form-label" Font-Bold="true" ID="LbDirConsul"></asp:Label>
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
        </div>
    </div>

    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="ModalConfirmar" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" role="document">
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
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnConfirmar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnConfirmar_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DESCARGA--%>
    <div class="modal fade" id="DescargaModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelDescarga">Descargar archivo de Financiamiento</h4>
                </div>
                <div class="modal-body">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <label class="col-sm-12 col-form-label">Privacidad de documentos</label>
                            <div class="col-sm-12" style="text-align: justify">
                                Este documento adjunto es confidencial, especialmente en lo que hace referencia a los datos personales que puedan contener y se dirigen exclusivamente al destinatario referenciado. Si usted no lo es y ha descargado este archivo o tiene conocimiento del mismo por cualquier motivo, le rogamos nos lo comunique por este medio y proceda a borrarlo, y que, en todo caso, se abstenga de utilizar, reproducir, alterar, archivar o comunicar a terceros el documento adjunto.
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnDescargarArchivo" runat="server" Text="Descargar" class="btn btn-success" OnClick="BtnDescargarArchivo_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnDescargarArchivo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
