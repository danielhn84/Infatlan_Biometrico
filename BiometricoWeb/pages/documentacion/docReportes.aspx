<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="docReportes.aspx.cs" Inherits="BiometricoWeb.pages.documentacion.docReportes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <script type="text/javascript">
        function openModal() { $('#ModalConfirmar').modal('show'); }
        function closeModal() { $('#ModalConfirmar').modal('hide'); }
    </script>
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />
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
                        <h2>
                            <asp:Literal Text="Reportes de lectura y aprobación" runat="server" ID="TxTitulo" />
                        </h2>
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
                    <h4 class="card-title">Datos Generales</h4>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Tipo Documento</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ClientIDMode="AutoID" ID="DDLTipoPDoc" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDLTipoPDoc_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Documento</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLDocumento" runat="server" class="form-control"></asp:DropDownList>
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
    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Crear Reporte</h4>

                    <div class="form-group">
                        <asp:UpdatePanel ID="UPanelReporte" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="BtnCrearReporte" class="btn btn-primary mr-2" runat="server" Text="Crear" OnClick="BtnCrearReporte_Click" />
                                <%--<asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="ModalConfirmar" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="ModalLabelConfirmar">
                        <b><asp:Label runat="server" Text="Reporte de Lectura" ID="LbTitulo" CssClass="col-form-label"></asp:Label></b>
                    </h4>
                </div>
                <div class="card-body">
                    <asp:Label runat="server" Text="Está seguro que desea descargar el reporte seleccionado?" ID="LbMensaje" CssClass="col-form-label"></asp:Label>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
