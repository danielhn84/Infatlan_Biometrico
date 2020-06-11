<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="descriptorPuestos.aspx.cs" Inherits="BiometricoWeb.pages.descriptorPuestos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12 grid-margin">
                    <div class="d-flex justify-content-between flex-wrap">
                        <div class="d-flex align-items-end flex-wrap">
                            <div class="mr-md-3 mr-xl-5">
                                <h2>Descripción de Puestos</h2>
                                <p class="mb-md-0">Recursos Humanos</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <%--<div class="row" id="DivBusqueda" runat="server">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Puestos o Posiciones</h4>
                            <p>Puestos de trabajo existentes.</p>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-sm-1 col-form-label">Buscar</label>
                                    <div class="col-sm-6">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TxBuscarPuesto" runat="server" placeholder="Ej. Programador - Presione afuera para proceder" class="form-control" AutoPostBack="true" OnTextChanged="TxBuscarPuesto_TextChanged"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="btnNuevo" runat="server" Text="Crear Puesto" class="btn btn-primary" OnClick="btnNuevo_Click" />                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>

              <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                        <h4 class="card-title">Puesto Actual</h4>
                        <h6>
                        <asp:Label Style="font-weight: 500;" class="card-description" runat="server" ID="Label1">Información descriptiva de su puesto de trabajo.</asp:Label>
                        </h6>
                        <div class="col-md-12">
                            <asp:UpdatePanel ID="udpDescriptorActual" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="table-responsive">

                                            <asp:GridView ID="GVDescriptorActual" runat="server"
                                                CssClass="mydatagrid"
                                                HeaderStyle-HorizontalAlign="Center"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                PageSize="10" 
                                                OnPageIndexChanging="GVDescriptorActual_PageIndexChanging" 
                                                OnRowCommand="GVDescriptorActual_RowCommand">

                                                <Columns>

                                                    <asp:BoundField DataField="idPuesto" HeaderText="Id Puesto" Visible="true" />
                                                    <asp:BoundField DataField="nombre" HeaderText="Puesto"  HeaderStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                                    <asp:TemplateField HeaderStyle-Width="150px" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderText="Seleccione">
                                                        <ItemTemplate>
                                                            <asp:Button ID="BtnEntrarActual" Enabled="true" runat="server" Text="Entrar" class="btn btn-inverse-success  mr-2" CommandArgument='<%# Eval("idPuesto") %>' CommandName="EntrarDescriptorActual" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>

                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="updGVAsignados" runat="server">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Puestos Asignados</h4>
                                <h6>
                                    <asp:Label Style="font-weight: 500;" class="card-description" runat="server" ID="lbDescripcionPuesto" Text="Información descriptiva de cada puesto de trabajo a su cargo."></asp:Label>
                                </h6>
                                <div class="col-md-12">
                                    <asp:UpdatePanel ID="udpGVDescriptor" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="table-responsive">

                                                    <asp:GridView ID="GVDescriptor" runat="server"
                                                        CssClass="mydatagrid"
                                                        HeaderStyle-HorizontalAlign="Center"
                                                        PagerStyle-CssClass="pgr"
                                                        HeaderStyle-CssClass="header"
                                                        RowStyle-CssClass="rows"
                                                        AutoGenerateColumns="false"
                                                        AllowPaging="true"
                                                        GridLines="None"
                                                        PageSize="10"
                                                        OnPageIndexChanging="GVDescriptor_PageIndexChanging"
                                                        OnRowCommand="GVDescriptor_RowCommand">

                                                        <Columns>

                                                            <asp:BoundField DataField="idPuesto" HeaderText="Id Puesto" Visible="true" />
                                                            <asp:BoundField DataField="nombre" HeaderText="Puesto" />
                                                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                                            <asp:TemplateField HeaderStyle-Width="150px" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderText="Seleccione">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnEntrar" runat="server" Text="Pendiente" class="btn btn-inverse-primary  mr-2" CommandArgument='<%# Eval("idPuesto") %>' CommandName="EntrarDescriptor"  />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>

                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
