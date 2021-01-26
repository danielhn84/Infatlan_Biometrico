<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="registroActivos.aspx.cs" Inherits="BiometricoWeb.pages.activos.registroActivos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/select2.css" rel="stylesheet" />

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
                        <h2>Activos</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Principal" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-login" style=""></i>Registrar</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Principal" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Categoría</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLCategorias" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLCategorias_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Serie</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxSerie" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-3 col-form-label">Inventario</label>
                                        <div class="col-9">
                                            <asp:TextBox runat="server" ID="TxInventario" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6"> 
                                        <label class="col-3 col-form-label">Responsable</label>
                                        <div class="col-9">
                                            <asp:DropDownList ID="DDLResponsable" runat="server" CssClass="select2 form-control custom-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPPrincipal" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" visible="true" id="DivPI">
                        <asp:UpdatePanel ID="UPPersonalInterno" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div runat="server" id="DivInfoIN" visible="false">
                                    <div class="col-12 grid-margin stretch-card">
                                        <div class="card">
                                            <div class="card-body" runat="server" id="DivBody">
                                                <h4 class="card-title">Datos del Encargado</h4>
                                                <div class="row">
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label class="col-3" style="text-align: right">Nombre:</label>
                                                            <div class="col-9">
                                                                <asp:Label runat="server" Text="" ID="LbNombre" Font-Bold="true"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label class="col-3" style="text-align: right">ID Equipo:</label>
                                                            <div class="col-5">
                                                                <asp:Label runat="server" Text="" ID="LbIdEquipoEnt" Font-Bold="true"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label class="col-3" style="text-align: right">Tipo:</label>
                                                            <div class="col-9">
                                                                <asp:Label runat="server" Text="" ID="LbTipo" Font-Bold="true"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label class="col-3" style="text-align: right">Marca:</label>
                                                            <div class="col-9">
                                                                <asp:Label runat="server" Text="" ID="LbMarca" Font-Bold="true"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label class="col-3" style="text-align: right">Serie:</label>
                                                            <div class="col-9">
                                                                <asp:Label runat="server" Text="" ID="LbSerieSalida" Font-Bold="true"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label class="col-3" style="text-align: right">No. Inv.</label>
                                                            <div class="col-9">
                                                                <asp:Label runat="server" Text="" ID="LbCodInventario" Font-Bold="true"></asp:Label>
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

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="/js/select2.js"></script>
    <link href="/css/select2.css" rel="stylesheet" />
</asp:Content>
