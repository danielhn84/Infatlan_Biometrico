<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="transporte.aspx.cs" Inherits="BiometricoWeb.pages.mantenimiento.transporte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        function openModal() { $('#modalCrear').modal('show'); }
        function openModal2() { $('#modalModificar').modal('show'); }
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalCrear').modal('hide'); }
        function closeModal2() { $('#modalModificar').modal('hide'); }
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
                        <h2>Transportes</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
     <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-solicitud-tab" data-toggle="tab" href="#nav-solicitud" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-book"></i>Lista de transportes</a>
            <%--<a class="nav-item nav-link" id="nav-hotel-tab" data-toggle="tab" href="#nav-hotel" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Nuevo Hotel</a>
            <a class="nav-item nav-link" id="nav-habitacion-tab" data-toggle="tab" href="#nav-habitacion" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Nueva Habitación</a>--%>
        </div>
    </nav>
     <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-solicitud" role="tabpanel" aria-labelledby="nav-solicitud-tab">
          <%--BUSQUEDA--%>
            <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Buscar transporte</h4>

                    <asp:UpdatePanel ID="UPBuscarTra" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>   
                            <div class="row col-12">
                                    <asp:TextBox ID="txtbuscarTransporte" OnTextChanged="txtbuscarTransporte_TextChanged" runat="server" placeholder="ingrese transporte - Presione afuera para proceder" CssClass="form-control col-10" AutoPostBack="true"></asp:TextBox>
                                    <asp:LinkButton Text="Nuevo" runat="server" ID="btnNewTransporte" OnClick="btnNewTransporte_Click" CssClass="btn btn-info col-2" />                                
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
            <%--BUSQUEDA--%>
            
            <%-- CONTENIDO 1--%>       
           
          <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                           <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Lista de transportes</h4>
                                    
                                    <div class="row">
                                        <div class="table-responsive">
                                             <asp:UpdatePanel ID="UpdateGridView" runat="server" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <asp:GridView ID="GVSolicitud" runat="server"
                                                          CssClass="table"
                                                          PagerStyle-CssClass="pgr"
                                                          HeaderStyle-CssClass="table"
                                                          RowStyle-CssClass="rows"
                                                          AutoGenerateColumns="false"
                                                          AllowPaging="true"
                                                          GridLines="None" OnPageIndexChanging="GVSolicitud_PageIndexChanging"
                                                         HeaderStyle-HorizontalAlign="center"
                                                         PageSize="10" OnRowCommand="GVSolicitud_RowCommand">
                                                         <Columns>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="center">
                                                        
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnAprobar" runat="server" Text="" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("ID") %>' CommandName="Crear">
                                                                        <i class="mdi mdi-comment-search-outline" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                             <asp:BoundField DataField="ID" HeaderText="Codigo" Visible="true" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="Transporte" HeaderText="Transporte" Visible="true" ItemStyle-HorizontalAlign="center" />
                                                             <%--<asp:BoundField DataField="Hotel" HeaderText="Hotel" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="Habitacion" HeaderText="Habitación" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="Costo" HeaderText="Costo" ItemStyle-HorizontalAlign="center"/>--%>
                                                             
                                                         </Columns>
                                                     </asp:GridView>
                                                 </ContentTemplate>

                                             </asp:UpdatePanel>
                                         <%--</div>--%>
                                     
                                 </div> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                 </ContentTemplate>
             </asp:UpdatePanel>

            <%-- /CONTENIDO 1--%> 
                      </div>
         </div>

      <!--MODAL HOTELES-->
        <div class="modal bs-example-modal-lg" id="modalCrear" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">¿Desea crear transporte?</h4>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UPTransporte" UpdateMode="Conditional">
                        <ContentTemplate>
                            <br />
                             <div class="row col-md-12 align-self-center" style="margin-left: auto; margin-right: auto">
                                    <%--<div class="row">--%>
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Transporte</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox runat="server" ID="txtModalTransporte" CssClass="form-control"/>
                                                </div>
                                            </div>
                                        </div>     
                                 </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                   
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnCrearTransporte" OnClick="btnCrearTransporte_Click" CssClass="btn btn-success mr-3" Text="Crear" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnCerrarTransporte" OnClick="btnCerrarTransporte_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL HOTELES-->

     <!--MODAL HABITACION-->
        <div class="modal bs-example-modal-lg" id="modalModificar" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel2">¿Desea modificar transporte?</h4>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                        <ContentTemplate>
                            <br />
                             <div class="row col-md-12 align-self-center" style="margin-left: auto; margin-right: auto">
                                    <%--<div class="row">--%>
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Transporte</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox runat="server" ID="txtModalModificarTransporte" CssClass="form-control"/>
                                                </div>
                                            </div>
                                        </div>     
                                 </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModificarTransporte" OnClick="btnModificarTransporte_Click" CssClass="btn btn-success mr-3" Text="Modificar" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnCerrarModTransporte" OnClick="btnCerrarModTransporte_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL HABITACION-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
