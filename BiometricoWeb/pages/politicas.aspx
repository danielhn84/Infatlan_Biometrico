<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="politicas.aspx.cs" Inherits="BiometricoWeb.pages.politicas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }

        function showMe() {
            var x = document.getElementById("DivVestimenta");
            if (x.style.display === "none") {
                x.style.display = "block";
            }
            $('a#LIVestimenta').attr({
                target: '_blank',
                href: 'plantilla/PoliticaVestimenta.pdf'
            });

            //window.open('plantilla/PoliticaVestimenta.pdf');
        }

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

    <div class="row">
        <div class="col-md-12 grid-margin">
            <div class="d-flex justify-content-between flex-wrap">
                <div class="d-flex align-items-end flex-wrap">
                    <div class="mr-md-3 mr-xl-5">
                        <h2>Politicas</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="col-12 grid-margin stretch-card">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title">POLITICAS DE VESTIMENTA</h4>
                <p style="font-weight:500;" class="card-description">
                    Descargue y lea el documento. Luego confirme que leyó las políticas.
                </p>

                <div class="col-md-6">
                    <div class="form-group row">
                        <ul class="list-unstyled project_files">
                            <li><a id="LIVestimenta" style="color:cornflowerblue; cursor:pointer;" onclick="showMe();">DESCARGAR AQUI</a></li>
                        </ul>
                    </div>

                    <div class="row"  id="DivVestimenta" style="display:none">
                        <div class="row col-12 form-check" style="margin-left:5%">
                            <input type="checkbox" runat="server" ID="CBVestimenta" name="CBVestimenta" value="0" class="form-check-input" />He leído las politicas
                        </div>
                        <br />
                        <asp:UpdatePanel runat="server" ID="UPBtn">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="BtnEnviarPV" OnClick="BtnEnviarPV_Click" CssClass="btn btn-info col-12" Text="Enviar" />                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
