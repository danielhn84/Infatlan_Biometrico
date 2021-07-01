<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="nuevoActivo.aspx.cs" Inherits="BiometricoWeb.pages.activos.nuevoActivo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
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
                        <h2>Nuevo Activo</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Visitas" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-login" style=""> </i>Crear</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-database" style=""> </i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Visitas" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row mb-2"> 
                                    <label class="col-2 col-form-label">Proceso</label>
                                    <div class="col-7">
                                        <asp:DropDownList runat="server" ID="DDLProceso" CssClass="form-control" OnSelectedIndexChanged="DDLProceso_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="1" Text="Personal Interno"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Visitas"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Data Center"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Soporte Técnico"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Nuevo Activo" Selected="True"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>




            <%--Resultado--%> 
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div runat="server" id="DivEntradaVisita" visible="true">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body" runat="server" id="Div2" >
                                    <h4 class="card-title">Datos del Visitante</h4>
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-2 col-form-label">Identidad:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxIdentidad" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-2 col-form-label">Equipo:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxEquipo" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-2 col-form-label">Nombre:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="TxNombre" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-2 col-form-label">Apellido:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxApellido" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-2 col-form-label">Area:</label>
                                                <div class="col-9">
                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="DDLArea"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="form-group row">
                                                <label class="col-2 col-form-label">Motivo:</label>
                                                <div class="col-9">
                                                    <asp:TextBox runat="server" ID="TxDescripcion" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 grid-margin stretch-card" runat="server" id="DivEntrada" visible="true">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">REGISTRAR INFORMACION</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Button ID="BtnGuardar" class="btn btn-success" runat="server" Text="Registrar" />
                                        <asp:Button ID="BtnCancelar" class="btn btn-secondary" runat="server" Text="Cancelar" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="tab-pane fade" id="nav-Registros" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row">   
                                    <label class="col-2">Nombre:</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBusqueda" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Visitas Registradas</h4>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GVBusqueda" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="align-self-lg-start"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None"
                                            PageSize="10">
                                            <Columns>
                                                <asp:BoundField DataField="visitante" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="identidad" HeaderText="Identidad" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="area" HeaderText="Destino" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Entrada" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:BoundField DataField="fechaModificacion" HeaderText="Fecha Salida" ItemStyle-HorizontalAlign="Left"/>
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
