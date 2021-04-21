<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="registroVisitaSeguridad.aspx.cs" Inherits="BiometricoWeb.pages.activos.registroVisitaSeguridad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />
    <link href="/css/alert.css" rel="stylesheet" />

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

    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Data Center</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav_Programada" role="tab" aria-controls="nav-profile" aria-selected="true"><i class="mdi mdi-clock"></i>Programada</a>
                <a class="nav-item nav-link" id="nav_cargarEmergencia_tab" data-toggle="tab" href="#nav-Emergencia" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-library-books" style=""></i>Emergencia</a>
            </div>
        </nav>
    </div>
    <br>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav_Programada" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdateDivAutorizadas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row" id="Div1" runat="server">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Solicitudes Autorizadas</h4>
                                    <div class="row mb-2">
                                        <label class="col-2 col-form-label">Proceso</label>
                                        <div class="col-7">
                                            <asp:DropDownList runat="server" ID="DDLProceso" CssClass="form-control" OnSelectedIndexChanged="DDLProceso_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="1" Text="Personal Interno"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Visitas"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Data Center" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Soporte Técnico"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-2 col-form-label">Buscar</label>
                                        <div class="col-7">
                                            <asp:TextBox ID="TxSolicitud" runat="server" placeholder="Busqueda por Solicitud" class="form-control" AutoPostBack="true" OnTextChanged="TxSolicitud_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVBusquedas" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GVBusquedas_RowCommand"
                                                PageSize="10">
                                                <Columns>
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="Id" />
                                                    <asp:BoundField DataField="fechaInicio" HeaderText="Fecha Inicio" />
                                                    <asp:BoundField DataField="fechaFin" HeaderText="Fecha Fin" />
                                                    <asp:BoundField DataField="accesoNombre" HeaderText="Acceso" />
                                                    <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                                                    <asp:BoundField DataField="nombre" HeaderText="Responsable" />
                                                    <asp:TemplateField HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnAprobar" runat="server" Text="Entrar" class="btn btn-inverse-primary  mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="AutorizarEntrada" />
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


    <div class="modal fade" id="EnviarSolicitudModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="lbTitulo" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row p-t-20">
                                <div class="col-6">
                                    <label class="control-label">Fecha Inicio:</label></label>
                                        <asp:TextBox ID="TxFechaInicioModal" AutoPostBack="true" runat="server" TextMode="DateTimeLocal" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="control-label">Fecha Fin:</label></label>    
                                     <asp:TextBox ID="TxFechaFinModal" AutoPostBack="true" runat="server" TextMode="DateTimeLocal" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br>
                            <div class="row p-t-20">
                                <div class="col-12">
                                    <label class="control-label">Tarea:</label></label>
                                    <asp:TextBox ID="TxTareaModal" AutoPostBack="true" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br>
                            <div class="row p-t-20">
                                <div class="col-12">
                                    <label class="control-label">Motivo:</label></label>
                                    <asp:TextBox ID="TxMotivoModal" AutoPostBack="true" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="true" class="form-control"></asp:TextBox>
                                </div>
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="BtnCerrarSoli" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarSoli" runat="server" Text="Enviar" class="btn btn-success" />
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
