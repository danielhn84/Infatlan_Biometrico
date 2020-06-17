<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="solicitudesCanceladasSubgerente.aspx.cs" Inherits="BiometricoWeb.pages.tiempoExtraordinario.solicitudesCanceladasSubgerente" %>
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
        function closeDescargarHojaServicioModal() { $('#DescargaHojaServicioModal').modal('show'); }
        function openDescargarHojaServicioModal() { $('#DescargaHojaServicioModal').modal('show'); }


        function closeMasInformacionModal() { $('#MasInformacion').modal('show'); }
        function openMasInformacionModal() { $('#MasInformacion').modal('show'); }
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
                        <h2>Solicitudes Canceladas Subgerente</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- INICIO BUSQUEDA---%>
    <div class="row" id="DivBusqueda" runat="server">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Solicitudes canceladas</h4>
                    <p>Ordenados por fecha de cancelación</p>
                    <div class="col-md-6">
                        <div class="form-group row">
                            <label class="col-sm-3 col-form-label">Buscar</label>
                            <div class="col-sm-9">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="TxBuscarEmpleado" runat="server" placeholder="Ej. Elvin - Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscarEmpleado_TextChanged"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- FIN BUSQUEDA---%>

    <%-- SECCION 2---%>
    <asp:UpdatePanel ID="UpBusquedaCanceladasSubgerente" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:GridView ID="GVBusquedaCanceladasSubgerente" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pgr"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows"
                                        AutoGenerateColumns="false"
                                        AllowPaging="true" OnRowCommand="GVBusquedaCanceladasSubgerente_RowCommand"
                                        GridLines="None" OnRowDataBound="GVBusquedaCanceladasSubgerente_RowDataBound"
                                        PageSize="10" OnPageIndexChanging="GVBusquedaCanceladasSubgerente_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BtnMasInformacion" runat="server" Text="Mas Informacion" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="Solicitud">
                                                                        <i class="mdi mdi-comment-search-outline text-primary" ></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BtnDocumento" runat="server" Text="Descargar" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="HojaServicio">
                                                                        <i class="mdi mdi-download text-primary" ></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="idSolicitud" HeaderText="No." />
                                            <asp:BoundField DataField="nombre" HeaderText="Colaborador" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                                            <asp:BoundField DataField="fechaInicio" HeaderText="Inicio" />
                                            <asp:BoundField DataField="fechaFin" HeaderText="Fin" />
                                            <asp:BoundField DataField="nombreTrabajo" HeaderText="Trabajo" />
                                            <asp:BoundField DataField="fechaSolicitud" HeaderText="Creación" />
                                            <asp:BoundField DataField="horaAproboSubgerente" HeaderText="Cancelo Subgerente" />
                                            <asp:BoundField DataField="motivoCanceloSubgerente" HeaderText="Motivo" />
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
    <%-- FIN SECCION 2---%>

    <%--    MODAL DESCARGAR ARCHIVO--%>
    <div class="modal fade" id="DescargaHojaServicioModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelDescarga">Descargar Hoja de Servicio - 
                                <asp:Label ID="LbHojaDescarga" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-sm-12 col-form-label">
                                        <h4>Privacidad de documentos</h4>
                                    </label>
                                    <div class="col-sm-12" style="text-align: justify">
                                        Este documento adjunto es confidencial, especialmente en lo que hace referencia a los datos personales que puedan contener y se dirigen exclusivamente al destinatario referenciado. Si usted no lo es y ha descargado este archivo o tiene conocimiento del mismo por cualquier motivo, le rogamos nos lo comunique por este medio y proceda a borrarlo, y que, en todo caso, se abstenga de utilizar, reproducir, alterar, archivar o comunicar a terceros el documento adjunto.
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnDescarga" runat="server" Text="Descargar" class="btn btn-success" OnClick="BtnDescarga_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnDescarga" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%--    FIN MODAL DESCARGAR ARCHIVO--%>

    <%-- MAS INFORMACION--%>
    <div class="modal fade" id="MasInformacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpTituloMasInformacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="LbMasInformacion" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpDetalle" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensaje1" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbMensaje2" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%-- FIN MAS INFORMACION--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
