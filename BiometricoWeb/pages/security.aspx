﻿<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="security.aspx.cs" Inherits="BiometricoWeb.pages.security" %>
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
                        <h2>Entrada de Equipo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Entradas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus" style=""> </i>Nuevo</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-database" style=""> </i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Entradas" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Datos Generales</h4>

                        <div class="row">
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Serie</label>
                                    <div class="col-9">
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="TxSerie" Text="" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Artículo</label>
                                    <div class="col-9">
                                        <asp:DropDownList ID="DDLArticulos" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Nombre</label>
                                    <div class="col-9">
                                        <asp:TextBox runat="server" ID="TxNombre" CssClass="form-control"></asp:TextBox>
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
                                        <asp:TextBox runat="server" ID="TxDestinatario" CssClass="form-control"></asp:TextBox>
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
                        <h4 class="card-title">Crear Entrada</h4>

                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="BtnGuardar" class="btn btn-success" runat="server" Text="Crear" OnClick="BtnGuardar_Click" />
                                    <asp:Button ID="BtnCancelar" class="btn btn-secondary" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="nav-Registros" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Búsqueda</h4>
                        <div class="row">   
                            <label class="col-2">Serie del artículo</label>
                            <div class="col-7">
                                <asp:TextBox runat="server" ID="TxBusqueda" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class ="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Entradas Creadas</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="10" OnPageIndexChanging="GVBusqueda_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="id" HeaderText="No." />
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                                    <asp:BoundField DataField="articulo" HeaderText="Artículo" />
                                                    <asp:BoundField DataField="serie" HeaderText="Serie" />
                                                    <asp:BoundField DataField="inventario" HeaderText="No. Inventario" />
                                                    <asp:BoundField DataField="destinatario" HeaderText="Destinatario" />
                                                    <asp:BoundField DataField="fechaEntrada" HeaderText="Fecha" />
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
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
