<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="docAreas.aspx.cs" Inherits="BiometricoWeb.pages.documentacion.docAreas" %>
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
        function openModal() { $('#ModalEditarDoc').modal('show'); }
        function openModalRef() { $('#ModalReferencias').modal('show'); }
        function closeModal() { $('#ModalEditarDoc').modal('hide'); }
    </script>
    <style>
        .hiddencol { display: none; }
    </style>
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
                        <h2>Departamentos</h2>
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
                            <div class="col-md-12 row">
                                <div class="row col-6">
                                    <label class="col-sm-2 col-form-label">Buscar</label>
                                    <div class="col-sm-10">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxBuscar" runat="server" placeholder="Ej. Gerencia - Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscar_TextChanged"></asp:TextBox>
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
                            <div class="row">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdateGridView" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="align-self-lg-start"
                                                RowStyle-CssClass="rows"
                                                GridLines="None"
                                                AllowPaging="true"
                                                PageSize="10"
                                                AutoGenerateColumns="false" OnPageIndexChanging="GVBusqueda_PageIndexChanging" >
                                                <Columns>
                                                    <asp:BoundField DataField="idEmpleado" HeaderText="ID" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnReferencias" runat="server" title="Referencias" style="background-color:#0b5a79" class="btn" CommandArgument='<%# Eval("idDocumento") %>' CommandName="docReferencias">
                                                                <i class="mdi mdi-link-variant text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
