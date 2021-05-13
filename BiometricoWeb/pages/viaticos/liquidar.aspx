<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="liquidar.aspx.cs" Inherits="BiometricoWeb.pages.viaticos.liquidar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!--PARA LLAMAR MODAL-->
    <script type="text/javascript">
        function openModal() { $('#modalViaticos').modal('show'); }
        function openModal2() { $('#modalViaticosA').modal('show'); }
        function openModal3() { $('#modalCancelar').modal('show'); }
        
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalViaticos').modal('hide'); }
        function closeModal2() { $('#modalViaticosA').modal('hide'); }  
        function closeModal3() { $('#modalCancelar').modal('hide'); } 
    </script>

     <!--PARA LLAMAR ARCHIVO-->
    <script type="text/javascript">
               
        function img1(input) {
            if (input.files && input.files[0]) {
                //SI EXISTE ARCHIVO             
                var reader = new FileReader();
                reader.onload = function (e) {
                    var ruta1 = e.target.result;
                    document.getElementById('<%=HFVerRecibo.ClientID%>').value = 'si';
                }
                reader.readAsDataURL(input.files[0]);
                //SI EXISTE ARCHIVO              
            }
        }
        //IMAGEN2
        function img2(input) {

            if (input.files && input.files[0]) {
                //PRIMERA IMAGEN
                var reader = new FileReader();
                reader.onload = function (e) {
                    var ruta2 = e.target.result;
                    document.getElementById('<%=HFValidarRecibos.ClientID%>').src = ruta2;
                    document.getElementById('<%=HFValidarRecibos.ClientID%>').value = 'si';
                }
                reader.readAsDataURL(input.files[0]);
                //PRIMERA IMAGEN              
            }
        }
        //IMAGEN2
        </script>
    <!--PARA LLAMAR ARCHIVO-->

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
                        <h2>Liquidación</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
     <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-solicitud-tab" data-toggle="tab" href="#nav-solicitud" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-book"></i>Liquidaciones</a>
            <%--<a class="nav-item nav-link" id="nav-liquidacion-tab" data-toggle="tab" href="#nav-liquidacion" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Aprobar Liquidación</a>--%>
        </div>
    </nav>
    <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-solicitud" role="tabpanel" aria-labelledby="nav-solicitud-tab">
            <%-- CONTENIDO 1--%> 
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <div class="row">
                    <div class=" col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Fechas de viaje solicitadas</h4>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Inicio</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox AutoPostBack="true" Enabled="false" ID="TxFechaInicio"  CssClass="form-control" runat="server" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Finaliza</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="TxFechaRegreso" Enabled="false" AutoPostBack="true" CssClass="form-control" runat="server" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                 <h4 class="card-title">Fechas de viaje reales</h4>
                                 <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Inicio</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox AutoPostBack="true" ID="txtFechaInicioReal" placeholder="1900-12-31 00:00:00" CssClass="form-control" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Finaliza</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtFechaRegresaReal" AutoPostBack="true" placeholder="1900-12-31 00:00:00" CssClass="form-control" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
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
            <%-- /CONTENIDO 1--%> 
              <%-- CONTENIDO 2--%> 
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Solicitante de Viáticos</h4>

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Solicitante</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtSolicitante" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Código SAP</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtSAP" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Puesto</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtPuesto" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
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
            <%-- /CONTENIDO 2--%> 
            <%--CONTENIDO TABLA LIQUIDACION--%>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Calculo de Liquidaciones</h4>

                                 <%--ARCHIVO--%>
                               
                                       <asp:LinkButton Text="Visualizar Recibos" Visible="false" class="btn btn-primary mr-2" ID="btnArchivo" OnClick="btnArchivo_Click" runat="server" />
                                              
                                 <%--ARCHIVO--%>

                                <h3 runat="server" id="H3Cancelado" visible="false" style="color:red;">Solicitud de viáticos ha sido cancelado, se devolverá monto total asignado.</h3>

                                <%--TABLAS--%>

                                <div class="row col-12" style="margin-left: 10px; margin-left: 10px;">
                                    <asp:Label Text="Agregue archivo PDF con todas las facturas disponibles." runat="server" ID="LBComArchivo" class="col-12"/>
                                    <asp:FileUpload runat="server" accept=".pdf" ID="FULiquidacion" AllowMultiple="false" ClientIDMode="AutoID" CssClass="col-5" onchange="img2(this);"/>                                  
                                </div>
                                <br />
                                  <%--SELECCIONA LOS MONTOS--%>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DIVLenarLiqui">
                                            <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                                                data-tablesaw-sortable="data-tablesaw-sortable-switch data-tablesaw-minimap
                                                 data-tablesaw-mode-switch">
                                                <thead>
                            <tr>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Tipo de factura</th>                              
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">¿Factura? </th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Fecha</th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Monto diario</th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">No. Factura</th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Aceptar</th>

                            </tr>
                        </thead>
                        <tbody>  
                           
                            <tr>                               
                                <td><asp:DropDownList ID="DDLTipoFactura" OnTextChanged="DDLTipoFactura_TextChanged" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList></td>   
                                <td><div style="text-align:center;"><asp:CheckBox ID="CBFactura" OnCheckedChanged="CBFactura_CheckedChanged" Checked="true" AutoPostBack="true" runat="server"  /></div></td>
                                <td><asp:TextBox AutoPostBack="true" ID="txtFechaFactura" OnTextChanged="txtFechaFactura_TextChanged" placeholder="1900-12-31" CssClass="form-control" runat="server" TextMode="Date"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtcantidad" runat="server" CssClass="form-control" TextMode="Number" AutoPostBack="true"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtNoFactura" MaxLength="4" runat="server" CssClass="form-control" TextMode="Number" AutoPostBack="true"></asp:TextBox></td>
                                <td> <asp:LinkButton runat="server" ID="btnAceptarF" OnClick="btnAceptarF_Click" Text="" CssClass="btn btn-success  ti-check-box mr-2"> <i class="mdi mdi-thumb-up" ></i></asp:LinkButton></td>
                            </tr>                              
                           
                        </tbody>
                    </table>
                    </div>
                          
                <%--/SELECCIONA LOS MONTOS--%>
                
                <asp:UpdatePanel runat="server" ID="UPMateriales" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto">
                                        <div class="table-responsive">
                                            <!--<table id="bootstrap-data-table" class="table table-striped table-bordered"> -->
                                            <asp:GridView ID="GVLiquidaciones" runat="server"
                                                CssClass="table table-bordered"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="table"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                HeaderStyle-HorizontalAlign="center"
                                                PageSize="15" OnRowCommand="GVNewMateriales_RowCommand" 
                                                OnPageIndexChanging="GVLiquidaciones_PageIndexChanging"
                                                
                                                Style="margin: 30px 0px 20px 0px">
                                                <Columns>
                                                   <asp:BoundField DataField="idViaticos" HeaderText="Código" Visible="false" ItemStyle-HorizontalAlign="center" />
                                                   <asp:BoundField DataField="Factura" HeaderText="Facturado" Visible="false" ItemStyle-HorizontalAlign="center" />
                                                   <asp:BoundField DataField="IDTipoFactura" HeaderText="Codigo Tipo" Visible="false" ItemStyle-HorizontalAlign="center" />
                                                    <asp:BoundField DataField="TipoFactura" HeaderText="Tipo Factura" ItemStyle-HorizontalAlign="center" />
                                                   <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-HorizontalAlign="center" />
                                                   <asp:BoundField DataField="Monto" HeaderText="Monto" ItemStyle-HorizontalAlign="center" />
                                                   <asp:BoundField DataField="NoFac" HeaderText="No. Factura" ItemStyle-HorizontalAlign="center" />
                                                    <asp:BoundField DataField="Hora" HeaderText="Hora" Visible="false" ItemStyle-HorizontalAlign="center" />
                                                    <asp:TemplateField  HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>                                                           
                                                            <asp:LinkButton ID="Btnseleccionar" Enabled="true" runat="server" Text="" CssClass="btn btn-danger mr-2" CommandArgument='<%# Eval("Hora") %>' CommandName="eliminar"><i class="mdi mdi-delete-empty"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                                <%--TABLAS--%>
                                <br />
                                <br />
                                <%--COMPARACION DE MONTOS--%>
                                
                                 <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                        data-tablesaw-sortable="data-tablesaw-sortable-switch data-tablesaw-minimap
                        data-tablesaw-mode-switch">
                        <thead>
                            <tr>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Monto a liquidar <asp:Label Text=" L. " ID="LBMonedaMontoSolicitado" runat="server" /><asp:Label Text="0" ID="LBMontoSolicitado" runat="server" /></th>                              
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Emergencia <asp:Label Text=" L. " ID="LBMonedaEmergencia" runat="server" /><asp:Label Text="0" ID="LBEmemergencia" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Depreciación <asp:Label Text=" L. " ID="LBMonedaDepreciacion" runat="server" /><asp:Label Text="0" ID="LBDepreciacion" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Monto Real <asp:Label Text=" L. " ID="LBMonedaMontoReal" runat="server" /><asp:Label Text="0" ID="LBMontoReal" runat="server" /></th>                             

                            </tr>
                        </thead>
                                     </table>
                                <%--COMPARACION DE MONTOS--%>
                                         <%--COMPARACION DE TODOS--%>
                                
                                 <table runat="server" id="TBLCostos" visible="false" class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                        data-tablesaw-sortable="data-tablesaw-sortable-switch data-tablesaw-minimap
                        data-tablesaw-mode-switch">
                        <thead>
                            <tr>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Circulación <asp:Label Text=" L. " ID="LBMoneda1" runat="server" /><asp:Label Text="0" ID="LB1" runat="server" /></th>                              
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Hospedaje <asp:Label Text=" L. " ID="LBMoneda2" runat="server" /><asp:Label Text="0" ID="LB2" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Desayuno <asp:Label Text=" L. " ID="LBMoneda3" runat="server" /><asp:Label Text="0" ID="LB3" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Almuerzo <asp:Label Text=" L. " ID="LBMoneda4" runat="server" /><asp:Label Text="0" ID="LB4" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Cena <asp:Label Text=" L. " ID="LBMoneda5" runat="server" /><asp:Label Text="0" ID="LB5" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Transporte <asp:Label Text=" L. " ID="LBMoneda6" runat="server" /><asp:Label Text="0" ID="LB6" runat="server" /></th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Peaje <asp:Label Text=" L. " ID="LBMoneda7" runat="server" /><asp:Label Text="0" ID="LB7" runat="server" /></th>
                              

                            </tr>
                        </thead>
                                     </table>
                                <%--COMPARACION DE TODOS--%>
                                <br />
                                 <asp:TextBox runat="server" Text="" Enabled="false" ID="txtAlerta" CssClass="form-control" style="background-color:red; color:white; text-align:center;"/>
                                <%--<asp:Label Text="" ID="LBAlerta" runat="server" CssClass="col-form-label" /><br />--%>
                                 
                                <%--<asp:Label Text="" ID="LB1" runat="server" CssClass="col-form-label" />
                                <asp:Label Text="" ID="LB2" runat="server" CssClass="col-form-label" />
                                <asp:Label Text="" ID="LB3" runat="server" CssClass="col-form-label" />
                                <asp:Label Text="" ID="LB4" runat="server" CssClass="col-form-label" />
                                <asp:Label Text="" ID="LB5" runat="server" CssClass="col-form-label" />
                                <asp:Label Text="" ID="LB6" runat="server" CssClass="col-form-label" />
                                <asp:Label Text="" ID="LB7" runat="server" CssClass="col-form-label" />--%>
                            </div>
                        </div>
                    </div>
                </div>
        
            </ContentTemplate>
        </asp:UpdatePanel>
            <%--CONTENIDO TABLA LIQUIDACION--%>
                                 <asp:Label Text="" runat="server" ID="LBComentarioJefe" style="color:red;" CssClass="col-form-label"/>

              <%--ENVIAR COMENTARIO CANCELACION--%>
             <div class="row" runat="server" id="DIVMotivo2" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Motivo por el que se cancela solicitud de viaticos</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox runat="server" ID="txtMotivoCanceloLiquidacion" Enabled="false" CssClass="form-control" TextMode="MultiLine" Rows="2"/>                                          
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
            <%--ENVIAR COMENTARIO CANCELACION--%>

             <%--ENVIAR COMENTARIO--%>
             <div class="row" runat="server" id="DIVComentarioLiquidacion" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Comentario</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox runat="server" ID="txtComentarioLiq" CssClass="form-control" TextMode="MultiLine" Rows="2"/>                                          
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
            <%--ENVIAR COMENTARIO--%>
            <%--ENVIAR LIQUIDACION--%>
             <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear Solicitud</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrearLiquidacion" OnClick="BtnCrearLiquidacion_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Crear Liquidación" />
                                        <asp:Button ID="BtnDevolverLiquidacion" OnClick="BtnDevolverLiquidacion_Click" Visible="false" CssClass="btn btn-danger mr-2" runat="server" Text="Devolver Liquidación" />                                        
                                        <asp:Button ID="BtnCancelar" OnClick="BtnCancelar_Click"  CssClass="btn btn-warning mr-2" runat="server" Text="Cancelar Liquidación" /> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--ENVIAR LIQUIDACION--%>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="HFVerRecibo" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="HFValidarRecibos" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
            </div>      
        </div>
     </div>
  </div>

    <!--MODAL SOLICITUD -->
        <div id="modalViaticos" class="modal bs-example-modal-lg" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel2">¿Desea enviar liquidaciones?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModarEnviar" OnClick="btnModarEnviar_Click"  CssClass="btn btn-success mr-2" Text="Solicitar" />
                                </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrar" OnClick="btnModalCerrar_Click" CssClass="btn btn-danger mr-2" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                         <Triggers>
                        <asp:PostBackTrigger ControlID="btnModarEnviar" />
                    </Triggers>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL SOLICITUD -->

    <!--MODAL DEVOLVER -->
        <div class="modal bs-example-modal-lg" id="modalViaticosA" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel">¿Desea devolver liquidaciones?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalDevolver" OnClick="btnModalDevolver_Click" CssClass="btn btn-success mr-2" Text="Devolver" />
                                </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarDevolver" OnClick="btnModalCerrarDevolver_Click" CssClass="btn btn-danger mr-2" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                        <asp:PostBackTrigger ControlID="btnModalDevolver" />
                    </Triggers>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL DEVOLVER -->
     
             <!--MODAL CANCELAR-->
        <div class="modal bs-example-modal-lg" id="modalCancelar" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">¿Desea cancelar solicitud?</h4>
                    </div>
                    <br />
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                       
                    <div class="row col-12">
                        <div class="align-self-center col-12" style="margin-left: auto; margin-right: auto">
                        <asp:Label Text="Motivo por el que cancela solicitud." CssClass="col-form-label" runat="server" />
                            </div>
                        <div class="align-self-center col-12" style="margin-left: auto; margin-right: auto">
                        <asp:TextBox runat="server" ID="txtcomentario" CssClass="form-control" TextMode="MultiLine" Rows="3"/>
                        </div>
                        <br />
                        <br />
                         <div class="align-self-center col-9" style="margin-left: auto; margin-right: auto">
                       <h5 runat="server" visible="false" id="H5Alerta" class="text-danger"><i class="fa fa-exclamation-triangle"></i>Ingrese motivo por el que cancela solicitud.</h5>
                            </div>
                    </div>
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCancelarLiq" OnClick="btnModalCancelarLiq_Click" CssClass="btn btn-warning mr-3" Text="Cancelar" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarLiq" OnClick="btnModalCerrarLiq_Click" CssClass="btn btn-danger mr-3" Text="Cerrar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
     <!-- /MODAL CANCELAR-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
