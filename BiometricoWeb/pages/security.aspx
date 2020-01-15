<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="security.aspx.cs" Inherits="BiometricoWeb.pages.security" %>
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
                        <h2>Entrada/Salida de Equipo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server">
        <ContentTemplate>
            <div class="row" id="DivBusqueda" runat="server">

                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Datos Generales</h4>

                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3">Nombre</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxNombre" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3">Artículo</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLArticulo" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3">Serie</label>
                                        <div class="col-9">
                                            <asp:TextBox ReadOnly="true" runat="server" ID="TxSerie" CssClass="form-control"></asp:TextBox>                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">No. Inventario</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox runat="server" ID="TxInventario" CssClass="form-control"></asp:TextBox>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3">Destinatario</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxDestino" CssClass="form-control"></asp:TextBox>                                            
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
                            <h4 class="card-title">Datos Adicionales</h4>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">Departamento</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxDepartamento" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">Artículo</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DropDownList1" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">Serie</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control"></asp:TextBox>                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-3">No. Inventario</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TextBox3" CssClass="form-control"></asp:TextBox>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-3">Destinatario</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox runat="server" ID="TextBox4" CssClass="form-control"></asp:TextBox>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
