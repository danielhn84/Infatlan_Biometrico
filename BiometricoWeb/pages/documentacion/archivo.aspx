<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="archivo.aspx.cs" Inherits="BiometricoWeb.pages.documentacion.archivo" %>
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
                        <h2><asp:Label runat="server" Text="" ID="LbTitulo"></asp:Label></h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnVolver" Text="Volver" CssClass="btn btn-primary" runat="server" OnClick="BtnVolver_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<a href="tipoDocumentos.aspx" class="btn btn-primary">Volver</a>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>   
            <iframe runat="server" id="IFramePDF" frameborder="0" style="width:100%; height:340px"></iframe>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br /><br />
    <div runat="server" id="DivLectura" visible="false" class="col-12">
        <asp:UpdatePanel runat="server" ID="UPBtn">
            <ContentTemplate>
                <div class="row col-12" style="display: flex; justify-content: center;">
                    <asp:Button runat="server" ID="BtnLeido" OnClick="BtnLeido_Click" CssClass="btn btn-success" Text="Firmar" />                                
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="ModalConfirmar" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title w-100 text-center" id="ModalLabelConfirmar">
                        <b><asp:Label runat="server" Text="DECLARACION" ID="Label1" CssClass="col-form-label"></asp:Label></b>
                    </h4>
                </div>
                <div class="card-body text-center">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Label runat="server" Text="" ID="LbMensaje" CssClass="col-form-label"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <%--<asp:Button ID="BtnCancelarModal" runat="server" Text="Cancelar" class="btn btn-danger"  OnClick="BtnCancelarModal_Click"/>--%>
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
