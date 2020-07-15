<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="reportes.aspx.cs" Inherits="BiometricoWeb.pages.tiempoExtraordinario.reportes" %>
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
        function closeInformacion() { $('#Informacion').modal('hide'); }
        function OpenInformacion() { $('#Informacion').modal('show'); }
    </script>

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
                        <h2>Reporteria</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title"><b>Listado de reportes disponibles</b> </h4>
                    <br>
                    <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <label class="col-sm-1 col-form-label">Reporte</label>
                                        <div class="col-sm-11">
                                            <asp:DropDownList ID="DdlReporte" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlReporte_SelectedIndexChanged" >
                                                <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Carga Archivo SAP"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Facturacion Banco Atlántida"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Enviar Consolidado"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Avances Proyectos/Propuestas"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-2 col-form-label" runat="server" id="LbMes" visible="false">Mes</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="DdlMes" visible="false" runat="server" class="form-control" AutoPostBack="true" >
                                                <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Enero"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-2 col-form-label"  runat="server" id="LbQuincena" visible="false">Quincena</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="DdlQuincena" visible="false" runat="server" class="form-control" AutoPostBack="true" >
                                                <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Primera Quincena"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Segunda Quincena"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>


                              <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-2 col-form-label" runat="server" id="LbFactBanco" visible="false">Mes</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="DdlFactBanco" visible="false" runat="server" class="form-control" AutoPostBack="true" >
                                                <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Enero-Febrero...........16 Enero al 15 Febrero"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Febrero-Marzo...........16 Febrero al 15 Marzo"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Marzo-Abril.............16 Marzo al 15 Abril"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Abril-Mayo..............16 Abril al 15 Mayo"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Mayo-Junio..............16 Mayo al 15 Junio"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Junio-Julio.............16 Junio al 15 Julio"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Julio-Agosto............16 Julio al 15 Agosto"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Agosto-Septiembre.......16 Agosto al 15 Septiembre"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Septiembre-Octubre......16 Septiembre al 15 Octubre"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Octubre-Noviembre......16 Octubre al 15 Noviembre"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="Noviembre-Diciembre....16 Noviembre al 15 Diciembre"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="Diciembre-Enero........16 Diciembre al 15 Enero"></asp:ListItem>
                                            </asp:DropDownList>
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

    <asp:UpdatePanel ID="UpdateDivProyectoPropuesta" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row" runat="server" id="RowProyectoPropuesta" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:GridView ID="GVProyectoPropuesta" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pgr"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false" 
                                        AllowPaging="true" OnRowDataBound="GVProyectoPropuesta_RowDataBound"
                                        GridLines="None" OnPageIndexChanging="GVProyectoPropuesta_PageIndexChanging" 
                                        PageSize="10">
                                        <Columns>
                                            <asp:BoundField DataField="nombreTrabajo1" HeaderText="Trabajo" />
                                            <asp:BoundField DataField="nombreTrabajo2" HeaderText="Tipo" />                                       
                                            <asp:BoundField DataField="pagoHr" HeaderText="Pago por Hr" />
                                            <asp:BoundField DataField="totalHrs" HeaderText="Hrs Aprobadas" />        
                                            <asp:BoundField DataField="horasRegistradas" HeaderText="Hrs Registradas" />   
                                            <asp:BoundField DataField="faltantes" HeaderText="Hrs Faltantes" />   
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



    <asp:UpdatePanel ID="UpdatePanel48" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row" runat="server" id="RowBotones">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title"><b>Reportería</b> </h4>
                            <br>

                            <asp:Button ID="BtnCancelar" class="btn btn-danger" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                            <asp:Button ID="BtnDescargar" class="btn btn-success" runat="server" Text="Enviar" OnClick="BtnDescargar_Click" />

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="Informacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">
                    <h4 class="modal-title">Reporteria</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbInformacion" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbInformacionPregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnDescargarModal" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnDescargarModal_Click"  UseSubmitBehavior="false"  data-dismiss="modal"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnDescargarModal" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>                      
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
