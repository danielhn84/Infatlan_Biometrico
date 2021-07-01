<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="parkingList.aspx.cs" Inherits="BiometricoWeb.pages.servicios.parkingList" %>
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

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="navCategoria2" runat="server" data-toggle="tab" href="#catB" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-sort-ascending"> </i>Lista de Espera</a>
                <a class="nav-item nav-link" id="navCategoria1" runat="server" data-toggle="tab" href="#catA" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-star-circle"> </i>Categoria A</a>
                <a class="nav-item nav-link" id="navParqueos" runat="server" data-toggle="tab" href="#parkingList" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-car"> </i>Parqueos</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="catB" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UPCategoriaB" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row mb-2" runat="server" visible="false"> 
                                    <label class="col-2 col-form-label">Zona</label>
                                    <div class="col-7">
                                        <asp:DropDownList runat="server" ID="DDLZonaB" CssClass="form-control" OnSelectedIndexChanged="DDLZonaB_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="Seleccione una opción"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="TGU"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="SPS"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <label class="col-2 col-form-label">Nombre</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscarNombreB" AutoPostBack="true" OnTextChanged="TxBuscarNombreB_TextChanged" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPListaB" runat="server">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Listado en Espera</h4>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvListaB" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None" OnRowCommand="GvListaB_RowCommand"
                                            PageSize="10" OnPageIndexChanging="GvListaB_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="No." Visible="false"/>
                                                <asp:TemplateField HeaderText="No. Lista">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnParqueo" runat="server" style="background-color:#5cb85c" class="btn btn-success" Text=""></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="idLista" HeaderText="No. Lista" Visible="false"/>
                                                <asp:BoundField DataField="nombreEmpleado" HeaderText="Nombre"/>
                                                <asp:BoundField DataField="nombreZona" HeaderText="Zona"/>
                                                <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha"/>
                                                <asp:TemplateField HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnEdit" runat="server" style="background-color:#5bc0de" class="btn btn-info" CommandArgument='<%# Eval("id") %>' CommandName="editarLista">
                                                            <i class="mdi mdi-sync text-white mr-1"> <i style="font-style:normal; font-weight:100; font-size:medium; display:inline-block; height:100%; vertical-align: middle;">Mover</i></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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

        <div class="tab-pane fade" id="catA" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <asp:UpdatePanel ID="UPCategoriaA" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row mb-2"> 
                                    <label class="col-2 col-form-label">Zona</label>
                                    <div class="col-7">
                                        <asp:DropDownList runat="server" ID="DDLZonaA" CssClass="form-control" OnSelectedIndexChanged="DDLZonaA_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="Seleccione una opción"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="TGU"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="SPS"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <label class="col-2 col-form-label">Nombre</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscarNombreA" AutoPostBack="true" OnTextChanged="TxBuscarNombreA_TextChanged" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPListaA" runat="server">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Listado en Espera</h4>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvListaA" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None" OnRowCommand="GvListaA_RowCommand"
                                            PageSize="10" OnPageIndexChanging="GvListaA_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="No." Visible="false" />
                                                <asp:TemplateField HeaderText="No. Lista">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnParqueo" runat="server" style="background-color:#5cb85c" class="btn btn-success" Text=""></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="idLista" HeaderText="No. Lista" Visible="false"/>
                                                <asp:BoundField DataField="nombreEmpleado" HeaderText="Nombre" />
                                                <asp:BoundField DataField="fechaModifico" HeaderText="Fecha"/>
                                                <asp:BoundField DataField="nombreZona" HeaderText="Zona"/>
                                                <asp:TemplateField HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnEdit" runat="server" style="background-color:#5bc0de" class="btn btn-info" CommandArgument='<%# Eval("id") %>' CommandName="editarLista">
                                                            <i class="mdi mdi-sync text-white mr-1"> <i style="font-style:normal; font-weight:100; font-size:medium; display:inline-block; height:100%; vertical-align: middle;">Mover</i></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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

        <div class="tab-pane fade" id="parkingList" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Búsqueda--%>
            <asp:UpdatePanel ID="UPParqueos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Búsqueda</h4>
                                <div class="row mb-2"> 
                                    <label class="col-2 col-form-label">Zona</label>
                                    <div class="col-7">
                                        <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control" OnSelectedIndexChanged="DDLZonaB_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="Seleccione una opción"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="TGU"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="SPS"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Todos"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row"> 
                                    <label class="col-2 col-form-label">Nombre</label>
                                    <div class="col-7">
                                        <asp:TextBox runat="server" ID="TxBuscarParqueo" AutoPostBack="true" OnTextChanged="TxBuscarParqueo_TextChanged" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPListaParqueos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Listado en Espera</h4>
                                <div class="row">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvParqueos" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None"
                                            PageSize="10" OnPageIndexChanging="GvParqueos_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="No." Visible="false"/>
                                                <asp:TemplateField HeaderText="No. Parqueo">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnParqueo" runat="server" style="background-color:#5cb85c" class="btn btn-success" Text=""></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="parqueo" HeaderText="Parqueo" Visible="false"/>
                                                <asp:BoundField DataField="nombreEmpleado" HeaderText="Nombre"/>
                                                <asp:BoundField DataField="nombreZona" HeaderText="Zona"/>
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

    <%--MODAL DE EDITAR LISTA--%>
    <div class="modal fade" id="ModalEditList" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                Cambiar de Listado
                                <asp:Label ID="LbListId" runat="server" Text="" Visible="false"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-12">
                                    <div class="row">
                                        <label class="col-3 col-form-label">Categoria :</label>
                                        <div class="col-9">
                                            <asp:DropDownList runat="server" ID="DDLCategoria" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12 mt-3" runat="server" id="DivAlerta" visible="false" style="display: flex; background-color: tomato; justify-content: center">
                                <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbAlerta"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" class="btn btn-success" OnClick="BtnGuardar_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
