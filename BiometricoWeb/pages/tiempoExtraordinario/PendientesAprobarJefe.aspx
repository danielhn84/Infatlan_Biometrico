<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="PendientesAprobarJefe.aspx.cs" Inherits="BiometricoWeb.pages.tiempoExtraordinario.PendientesAprobarJefe" %>
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
                        <h2>Tiempo Extraordinario Pendientes Aprobar</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <%-- SECCION 2---%>
    <asp:UpdatePanel ID="UpdateDivBusquedasJefes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <div class="row" id="DivBusqueda" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Autorizaciones Pendientes</h4>
                            <p>Permisos que no han sido autorizados por los jefes.</p>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Buscar</label>
                                    <div class="col-sm-9">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <%--<h4 class="card-title">Solicitudes pendientes aprobar jefes</h4>--%>
                            <%--<p>Ordenados por fecha de creación</p>--%>
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:GridView ID="GVBusquedaPendientesJefe" runat="server"
                                        CssClass="mydatagrid"
                                        PagerStyle-CssClass="pgr"
                                        HeaderStyle-CssClass="header"
                                        RowStyle-CssClass="rows" 
                                        AutoGenerateColumns="false"  OnRowCommand="GVBusquedaPendientesJefe_RowCommand"
                                        AllowPaging="true" OnRowDataBound="GVBusquedaPendientesJefe_RowDataBound"
                                        GridLines="None" OnPageIndexChanging="GVBusquedaPendientesJefe_PageIndexChanging"
                                        PageSize="10" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BtnAprobar" runat="server" Text="Aprobar" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="Aprobar">
                                                                      <i class="mdi mdi-ballot text-primary" ></i>
                                                    </asp:LinkButton>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="aprobacionSubgerente"/>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgEstado" runat="server" Width="18" Height="18"></asp:Image>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                 
                                            <asp:BoundField DataField="idSolicitud" HeaderText="Colaborador." />
                                            <asp:BoundField DataField="nombre" HeaderText="Horas" ItemStyle-HorizontalAlign="Left"/>
                                            <asp:BoundField DataField="descripcion" HeaderText="Inicio" />                                             
                                            <asp:BoundField DataField="fechaInicio" HeaderText="Fin" />
                                            <asp:BoundField DataField="fechaFin" HeaderText="Creación" />
                                            <asp:BoundField DataField="fechaSolicitud" HeaderText="SysAid"  />
                                            <asp:BoundField DataField="sysAid" HeaderText="Trabajo" />
                                            <asp:BoundField DataField="nombreTrabajo" HeaderText="Detalle"  />
                                            <asp:BoundField DataField="detalleTrabajo" HeaderText="Estado"  ItemStyle-HorizontalAlign="Justify" />   
                                            <asp:BoundField DataField="descripcionEstado" />                                        
                                            <%--  <asp:BoundField DataField="codigoSAP" HeaderText="codigo"  Visible="false"/>--%>
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

    <div class="modal fade" id="MasInformacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">


                    <asp:UpdatePanel ID="UpTitulo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="LbMasInformacionColaboraador" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpMensaje" runat="server" UpdateMode="Conditional">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
