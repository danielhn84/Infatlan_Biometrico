<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="asignacion.aspx.cs" Inherits="BiometricoWeb.pages.activos.asignacion" %>
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
        //function openModals() { $('#M').modal('show'); }
        function closeModal() { $('#ModalConfirmar').modal('hide'); }
    </script>
    <link href="/css/select2.css" rel="stylesheet" type="text/css" />
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
                        <h2>Asignación de Equipo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UPDatosAsignación" runat="server" UpdateMode="Always" >
        <ContentTemplate>
            <div class="row" id="DivBusqueda" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Datos Generales</h4>
                            <div class="col-12 row">
                                <div class="form-group col-6">
                                    <label class="col-12">Tipo de Equipo</label>
                                    <div class="col-12">
                                        <asp:DropDownList runat="server" ID="DDLTipoEquipo" AutoPostBack="true" CssClass="select2 form-control custom-select" OnSelectedIndexChanged="DDLTipoEquipo_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group col-6">
                                    <label class="col-12">Equipo</label>
                                    <div class="col-12">
                                        <asp:DropDownList runat="server" ID="DDLEquipo" CssClass="select2 form-control custom-select"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12 row">
                                <div class="form-group col-6">
                                    <label class="col-12">Empleado</label>
                                    <div class="col-12">
                                        <asp:DropDownList runat="server" ID="DDLEmpleado" CssClass="select2 form-control custom-select"></asp:DropDownList>
                                    </div>
                                </div>
                            
                                <div class="form-group col-6">
                                    <label class="col-12">Autorizado para Salir</label>
                                    <div class="col-12">
                                        <asp:DropDownList runat="server" ID="DDLAutorizado" CssClass="form-control">
                                            <asp:ListItem Text="Si" Value ="2"></asp:ListItem>
                                            <asp:ListItem Text="No" Value ="1"></asp:ListItem>
                                        </asp:DropDownList>
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
                            <h4 class="card-title">Asignar Equipo</h4>
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAsignar" class="btn btn-primary mr-2" runat="server" Text="Asignar" OnClick="BtnAsignar_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--MODAL DE CONFIRMACION--%>
    <div class="modal fade" id="ModalConfirmar" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelConfirmar">
                                <b><asp:Label runat="server" Text="Seguro que desea asignar este equipo?" CssClass="col-form-label"></asp:Label></b>
                            </h4>
                            <asp:Label runat="server" ID="LbMensaje" CssClass="col-form-label"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <script src="/js/select2.js"></script>
    <link href="/css/select2.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function test() {
            $('.select2').select2();
            $('.ajax').select2({
                ajax: {
                    url: 'https://api.github.com/search/repositories',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            q: params.term, // search term
                            page: params.page
                        };
                    },
                    processResults: function (data, params) {
                        params.page = params.page || 1;
                        return {
                            results: data.items,
                            pagination: {
                                more: (params.page * 30) < data.total_count
                            }
                        };
                    },
                    cache: true
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1,
            });
        });
    </script>

</asp:Content>
