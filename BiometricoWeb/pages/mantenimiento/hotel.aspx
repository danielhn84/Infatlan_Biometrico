<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="hotel.aspx.cs" Inherits="BiometricoWeb.pages.mantenimiento.hotel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function openModal() { $('#modalHotel').modal('show'); }
        function openModal2() { $('#modalHabitacion').modal('show'); }
        function openModal3() { $('#modalCrear').modal('show'); }
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalHotel').modal('hide'); }
        function closeModal2() { $('#modalHabitacion').modal('hide'); }
        function closeModal3() { $('#modalCrear').modal('hide'); }
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
                        <h2>Hoteles</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
     <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-solicitud-tab" data-toggle="tab" href="#nav-solicitud" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-book"></i>Hoteles</a>
            <a class="nav-item nav-link" id="nav-hotel-tab" data-toggle="tab" href="#nav-hotel" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Nuevo Hotel</a>
            <a class="nav-item nav-link" id="nav-habitacion-tab" data-toggle="tab" href="#nav-habitacion" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Nueva Habitación</a>
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
                    <h4 class="card-title">Buscar Hotel</h4>

                    <asp:UpdatePanel ID="UPBuscarHotel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                               
                                    <asp:TextBox ID="txtbuscarHotel" OnTextChanged="txtbuscarHotel_TextChanged" runat="server" placeholder="ingrese hotel - Presione afuera para proceder" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                    <%--<asp:Button Text="Nuevo hotel" runat="server" ID="btnNweHotel" OnClick="btnNweHotel_Click" CssClass="btn btn-info" />--%>                                
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
                                    <h4 class="card-title">Lista de hoteles</h4>
                                    
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
                                                            <asp:LinkButton ID="btnAprobar" runat="server" Text="" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("IDHabitacion") %>' CommandName="Crear">
                                                                        <i class="mdi mdi-comment-search-outline" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                             <asp:BoundField DataField="IDHotel" HeaderText="Codigo" Visible="false" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="IDHabitacion" HeaderText="CodigoHabitar" Visible="false" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="Hotel" HeaderText="Hotel" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="Habitacion" HeaderText="Habitación" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="Costo" HeaderText="Costo" ItemStyle-HorizontalAlign="center"/>
                                                             
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
        <div class="tab-pane fade hide active" id="nav-hotel" role="tabpanel" aria-labelledby="nav-hotel-tab">
              <%--NUEVO HOTEL--%>
            <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Nuevo Hotel</h4>
                  <asp:UpdatePanel ID="UpdatePanelHoteles" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Nombre de Hotel</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtNewHotel" placeholder="Ingrese nuevo hotel..." CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Ubicación</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLUbicacion" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">¿Desayuno?</label>
                                                <div class="col-sm-11">
                                                   <asp:DropDownList runat="server" ID="DDLDesayuno" AutoPostBack="true" CssClass="form-control">
                                                       <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                       <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                   </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                                       
                                </ContentTemplate>
                            </asp:UpdatePanel>
                </div>
            </div>
        </div>
            </div>
            <%--NUEVO HOTEL--%>
             <%--ENVIAR HOTEL--%>
             <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear Hotel</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnEnviarHotel" OnClick="btnEnviarHotel_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Crear" />
                                       
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--ENVIAR HOTEL--%>
            </div>
        <div class="tab-pane fade hide active" id="nav-habitacion" role="tabpanel" aria-labelledby="nav-habitacion-tab">
             <%--NUEVO HABITACION--%>
            <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Nueva Habitación</h4>
                  <asp:UpdatePanel ID="UPHabitaciones" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">                                      
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Hotel</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLHoteleria" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>                                                                            
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Habitación</label>
                                                <div class="col-sm-11">
                                                     <asp:TextBox AutoPostBack="true" ID="txtHabitacion" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Precio($)</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtPrecio" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>                                  
                                </ContentTemplate>
                            </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
            <%--NUEVO HABITACION--%>
             <%--ENVIAR HABITACION--%>
             <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear Habitación</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCrearHabitacion" OnClick="btnCrearHabitacion_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Crear" />
                                       
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--ENVIAR HABITACION--%>
            </div>
        
        </div>

      <!--MODAL HOTELES-->
        <div class="modal bs-example-modal-lg" id="modalHotel" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">¿Desea crear hotel?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCrearHotel" OnClick="btnModalCrearHotel_Click" CssClass="btn btn-success mr-3" Text="Crear" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarHotel" OnClick="btnModalCerrarHotel_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
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
        <div class="modal bs-example-modal-lg" id="modalHabitacion" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel2">¿Desea crear habitación?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCrearHabitacion" OnClick="btnModalCrearHabitacion_Click" CssClass="btn btn-success mr-3" Text="Crear" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarHabitacion" OnClick="btnModalCerrarHabitacion_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
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

     <!--MODAL MODIFICAR HOTEL-->
        <div class="modal bs-example-modal-lg" id="modalCrear" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel3">¿Desea modificar información hotel?</h4>
                    </div>
                   <asp:UpdatePanel ID="UPModalHoteles" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row col-md-12 align-self-center" style="margin-left: auto; margin-right: auto">
                                    <%--<div class="row">--%>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Hotel</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox runat="server" ID="txtModalHotel" CssClass="form-control"/>
                                                </div>
                                            </div>
                                        </div>     
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Ubicación</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLModalUbicacion" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">¿Desayuno?</label>
                                                <div class="col-sm-11">
                                                   <asp:DropDownList runat="server" ID="DDLModalDesayuno" AutoPostBack="true" CssClass="form-control">
                                                       <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                       <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                   </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row col-md-12 align-self-center" style="margin-left: auto; margin-right: auto">                                      
                                                                                                              
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Habitación</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox runat="server" ID="txtModalHabitacion" CssClass="form-control"/>  
                                                </div>
                                            </div>
                                        </div>     
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Precio($)</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtModalPrecio" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>                  
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-2">
                                    <asp:Button runat="server" ID="btnModalModificar" OnClick="btnModalModificar_Click" CssClass="btn btn-success mr-3" Text="Modificar" />
                               </div>
                                <div class="row col-2">
                                    <asp:Button runat="server" ID="btnModalCerrarModificar" OnClick="btnModalCerrarModificar_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL MODIFICAR HOTEL-->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
