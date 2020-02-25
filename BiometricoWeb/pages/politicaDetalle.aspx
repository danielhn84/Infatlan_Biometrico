<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="politicaDetalle.aspx.cs" Inherits="BiometricoWeb.pages.politicaDetalle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                        <h2><asp:Label runat="server" Text="" ID="LbTitulo"></asp:Label></h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <iframe runat="server" id="IFramePDF" src="plantilla/PoliticaVestimenta.pdf" style="width:100%; height:340px"></iframe>
    <br /><br />
    <div class="row col-12" style="justify-content:center">
        <div class="col-2">
            <input type="checkbox" runat="server" ID="CBVestimenta" name="CBVestimenta" value="0" class="form-check-input" />He leído las politicas
        </div>
    </div>
    <br />
    <asp:UpdatePanel runat="server" ID="UPBtn">
        <ContentTemplate>
            <div class="row col-12" style="justify-content:center">
                <asp:Button runat="server" ID="BtnEnviarPV" OnClick="BtnEnviarPV_Click" CssClass="btn btn-info" Text="Enviar" />                                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
