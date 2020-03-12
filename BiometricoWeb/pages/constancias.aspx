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
                <a class="nav-item nav-link" id="A1" data-toggle="tab" href="#navMiBuzon" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-account" > </i>Mis Solicitudes</a>
                <a runat="server" visible="false" class="nav-item nav-link" id="BuzonGenerales" data-toggle="tab" href="#navRegistros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-folder" > </i>Solicitudes</a>
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
                                                    <div class="col-11">
                                                        <asp:TextBox runat="server" TextMode="MultiLine" Rows="6" PlaceHolder="T/C, Banco X, L." ID="TxDestino" CssClass="form-control"></asp:TextBox>
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
                                                        <asp:TextBox runat="server" ID="TxRepresentante" CssClass="form-control"></asp:TextBox>
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
                                                    <label class="col-2 col-form-label">RTN</label>
                                                    <div class="col-10">
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
                                                    <label class="col-2 col-form-label">Contacto</label>
                                                    <div class="col-10">
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
                                                    <label class="col-2 col-form-label">Lugar</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxLugar" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Ciudad</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxCiudad" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Teléfono</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxTelefono" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 ">Fecha Inicio</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" TextMode="Date" ID="TxFechaInicio" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Evento</label>
                                                    <div class="col-10">
                                                        <asp:TextBox runat="server" ID="TxEvento" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="form-group row">
                                                    <label class="col-2 col-form-label">Fecha Fin</label>
                                                    <div class="col-10">
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
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GvMisConstancias_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GvMisConstancias_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Fecha" />
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
                                    <h4 class="card-title">Solicitudes de constancias creadas</h4>
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
                                                    <asp:BoundField HeaderText="Fecha" />
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
