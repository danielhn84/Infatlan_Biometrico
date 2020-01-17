<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="salidas.aspx.cs" Inherits="BiometricoWeb.pages.salidas" %>
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
                        <h2>Salida de Equipo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Nuevo" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus" style=""> </i>Nuevo</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-clipboard" style=""> </i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Nuevo" role="tabpanel" aria-labelledby="nav-cargar-tab">
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

            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Datos</h4>

                        <div class="row">
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Departamento</label>
                                    <div class="col-9">
                                        <asp:DropDownList ID="DDLDepartamento" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Tipo</label>
                                    <div class="col-9">
                                        <asp:DropDownList ID="DDLTipo" runat="server" class="form-control">
                                            <asp:ListItem Value="" Text="Seleccione una opción"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Compra"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Entrega"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Reparación</label>
                                    <div class="col-9">
                                        <asp:DropDownList ID="DDLReparar" runat="server" class="form-control">
                                            <asp:ListItem Value="" Text="Seleccione una opción"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="SI"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="NO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-3">Observaciones</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox runat="server" ID="TxObservaciones" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-6">
                                <div class="form-group row">
                                    <label class="col-3">Autorizado Por</label>
                                    <div class="col-9">
                                        <asp:TextBox runat="server" ID="TxAutoriza" CssClass="form-control"></asp:TextBox>
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
                        <asp:Button runat="server" ID="BtnGuardar" Text="Guardar" CssClass="btn btn-success" OnClick="BtnGuardar_Click"/>
                        <asp:Button runat="server" ID="BtnBorrar" Text="Cancelar" CssClass="btn btn-secondary"/>
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
                                <asp:TextBox runat="server" ID="TxBuscaSerie" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
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
                                                <asp:BoundField HeaderText="No." />
                                                <asp:BoundField HeaderText="Nombre" />
                                                <asp:BoundField HeaderText="Artículo" />
                                                <asp:BoundField HeaderText="Serie" />
                                                <asp:BoundField HeaderText="No. Inventario" />
                                                <asp:BoundField HeaderText="Destinatario" />
                                                <asp:BoundField HeaderText="Autorizado Por" />
                                            </Columns>
                                        </asp:GridView>
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
