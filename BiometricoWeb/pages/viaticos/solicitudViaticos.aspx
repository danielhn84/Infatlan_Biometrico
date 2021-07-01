<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="solicitudViaticos.aspx.cs" Inherits="BiometricoWeb.pages.viaticos.solicitudViaticos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!--PARA LLAMAR MODAL-->
    <script type="text/javascript">
        function openModal() { $('#modalViaticos').modal('show'); }
        function openModal2() { $('#modalViaticosA').modal('show'); }
        function openModal3() { $('#modalCotizar').modal('show'); }
        function openModal4() { $('#modalCancelar').modal('show'); }
        function openModalTransporte() { $('#modalTransporte').modal('show'); }
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalViaticos').modal('hide'); }
        function closeModal2() { $('#modalViaticosA').modal('hide'); }
        function closeModal3() { $('#modalCotizar').modal('hide'); }
        function closeModal4() { $('#modalCancelar').modal('hide'); }
        function closeModalTransporte() { $('#modalTransporte').modal('hide'); }
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
                        <h2>Solicitud de Viáticos</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
     <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-datos" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus"></i>Crear Solicitud</a>
            <a class="nav-item nav-link" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Mis Solicitudes</a>

            
        </div>
        
        <div class="col-md-6" style="float:right; height:100%; visibility:visible">
           
        </div>
    </nav>
    <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-datos" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="" style="margin-left: auto;" runat="server">
                <%--<label class="form-check-label">
                    <input type="checkbox" name="CbEmergencias" value="0" class="" onclick="javas('sumar');" runat="server" id="CbEmergencias" />Presione aqui si su solicitud es de emergencia
                </label>--%>
                <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Label Text="*Tiene solicitud de viáticos pendiente por terminar. No puede crear nueva solicitud" ID="LBEstado" style="color:red;"  runat="server" Visible="false" />
                <br /> 
                <br />
                <asp:CheckBox ID="CBEmergencia" OnCheckedChanged="CBEmergencia_CheckedChanged" AutoPostBack="true" runat="server" Text="Presione aqui si su solicitud es de emergencia" />
            </ContentTemplate>
        </asp:UpdatePanel>
                    
            </div>

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Solicitud de viáticos</h4>
                           <asp:UpdatePanel ID="UpdatePanelViaticos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Inicio</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="TxFechaInicio" OnTextChanged="TxFechaInicio_TextChanged" placeholder="1900-12-31 00:00:00" CssClass="form-control" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Finaliza</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="TxFechaRegreso" AutoPostBack="true" OnTextChanged="TxFechaRegreso_TextChanged" placeholder="1900-12-31 00:00:00" CssClass="form-control" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Total</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="txtTiempoViaticos" ReadOnly="true" Text="Dias: 0    horas: 0" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Motivo viaje</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLMotivoViaje" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Tipo de viaje</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" OnTextChanged="DDLTipoViaje_TextChanged" AutoPostBack="true" ID="DDLTipoViaje" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Transporte</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" AutoPostBack="true" OnTextChanged="DDLTransporte_TextChanged" ID="DDLTransporte" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="DIVMotivoVehiculo" visible="false" class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                
                                                <label class="col-sm-12 col-form-label">El uso de vehículo propio queda a cuenta y riesgo del colaborador. 
                                                    Cualquier tipo de daño INFATLAN no se hará cargo.</label>
                                               <%-- <label class="col-sm-12 col-form-label">Motivo por el que usa vehiculo propio</label>--%>
                                                <div class="col-sm-12">
                                                    <asp:TextBox ID="txtMotivoVehiculo" placeholder="Ingrese motivo por el que usa vehiculo propio..." CssClass="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                      <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Nombre</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLEmpleado" AutoPostBack="true" OnTextChanged="DDLEmpleado_TextChanged" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Código SAP</label>
                                                <div class="col-sm-11">
                                                   <asp:TextBox ID="txtCodSAP" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Puesto laboral</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="txtPuesto" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <%--<asp:UpdatePanel runat="server">
                                            <ContentTemplate>   --%>            
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Destino Inicial</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLDestinoI" OnTextChanged="DDLDestinoI_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Destino Final</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" ID="DDLDestinoF" OnTextChanged="DDLDestinoF_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Kilometros - Peajes</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="txtKmPeaje" ReadOnly="true" Text="Km: 0    Peajes: 0" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                             <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Hotel</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" Enabled="false" AutoPostBack="true" OnTextChanged="DDLHotel_TextChanged" ID="DDLHotel" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Habitación</label>
                                                <div class="col-sm-11">
                                                    <asp:DropDownList runat="server" AutoPostBack="true" Enabled="false" OnTextChanged="DDLHabitacion_TextChanged" ID="DDLHabitacion" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Costo</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="txtCostoHotel" ReadOnly="true" Text="L. 0" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="DIVNuevoHotel" class="row" visible="false">
                                         <div class="col-md-4" runat="server" id="DIVPais" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-10 col-form-label">País al que se dirige</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox ID="txtNewPais" placeholder="Ingrese país..." CssClass="form-control" runat="server" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>    
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-10 col-form-label">Hotel en el que se hospedó</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox ID="txtNewHotel" placeholder="Ingrese nuevo nombre de hotel..." CssClass="form-control" runat="server" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>                                      
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label"></label>
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                               
                                                <div class="col-sm-11">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBox Text="¿Hotel incluye desayuno?" ID="CBDesayuno" OnCheckedChanged="CBDesayuno_CheckedChanged" runat="server" AutoPostBack="true" />
                                                    </ContentTemplate>
                                                 </asp:UpdatePanel>
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

             <div class="row" runat="server" id="DIVVehiculo" visible="false">
            <div class="col-12 grid-margin stretch-card">
                <div class="card">
                    <div class="card-body">
                         <h4 class="card-title">Vehículos</h4>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Descripción</label>
                                            <div class="col-sm-11">
                                                <asp:DropDownList runat="server" OnTextChanged="DDLVehiculo_TextChanged" AutoPostBack="true" ID="DDLVehiculo" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Placa</label>
                                            <div class="col-sm-11">
                                                <asp:TextBox runat="server" ID="txtplaca" CssClass="form-control" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-4 col-form-label">Serie</label>
                                            <div class="col-sm-11">
                                                <asp:TextBox runat="server" ID="txtSerie" CssClass="form-control" Enabled="false" />
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

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                   <div class="card-body">
                    <h3 class="card-title" style="color: #808080;"><i class="fa fa-image" style="margin-left: 10px"></i>Calculo de viáticos</h3>
                    
                     <asp:Button ID="btnCalcular" OnClick="btnCalcular_Click" CssClass="btn btn-primary mr-6" runat="server" Text="Calcular viáticos" />
                     <br />
                     <br />
                    <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                        data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap
                        data-tablesaw-mode-switch>
                        <thead>
                            <tr>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Hospedaje</th>                              
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Desayuno </th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Almuerzo</th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Cena</th>    
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Depreciación</th>                              
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Transporte </th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Emergencia</th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Peaje</th>    
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Circulación</th>


                            </tr>
                        </thead>
                        <tbody>  
                           
                            <tr>                               
                                <td><asp:Label runat="server" ID="LBMonedaHospedaje" Text="L."></asp:Label> <asp:Label runat="server" ID="LBHospedaje" Text="0"></asp:Label></td>   
                                <td> <asp:Label runat="server" ID="LBMonedaDesayuno" Text="L."></asp:Label> <asp:Label runat="server" ID="LBDesayuno" Text="0"></asp:Label></td>                                
                                <td> <asp:Label runat="server" ID="LBMonedaAlmuerzo" Text="L."></asp:Label> <asp:Label runat="server" ID="LBAlmuerzo" Text="0"></asp:Label></td>
                                <td> <asp:Label runat="server" ID="LBMonedaCena" Text="L."></asp:Label> <asp:Label runat="server" ID="LBCena" Text="0"></asp:Label></td>  
                                <td> <asp:Label runat="server" ID="LBMonedaDepresiacion" Text="L."></asp:Label> <asp:Label runat="server" ID="LBDepresiacion" Text="0"></asp:Label></td>   
                                <td> <asp:Label runat="server" ID="LBMonedaTransporte" Text="L."></asp:Label> <asp:Label runat="server" ID="LBTransporte" Text="0"></asp:Label></td>                                
                                <td> <asp:Label runat="server" ID="LBMonedaEmergencia" Text="L."></asp:Label> <asp:Label runat="server" ID="LBEmergencia" Text="0"></asp:Label></td>
                                <td> <asp:Label runat="server" ID="LBMonedaPeaje" Text="L."></asp:Label> <asp:Label runat="server" ID="LBPeaje" Text="0"></asp:Label></td>
                                <td> <asp:Label runat="server" ID="LBMonedaCirculacion" Text="L."></asp:Label> <asp:Label runat="server" ID="LBCirculacion" Text="0"></asp:Label></td>
                            </tr>                              
                           
                        </tbody>
                    </table>
                                       <%--PARTE 2--%>
                         <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                        data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap
                        data-tablesaw-mode-switch>
                        <thead>
                            <tr>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Sub Total= <asp:Label runat="server" ID="LBMonedaSubTotal" Text="L."></asp:Label> <asp:Label runat="server" ID="LBSubTotal" Text="0"></asp:Label></th>                              
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Total Solicitado= <asp:Label runat="server" ID="LBMonedaTotal" Text="L."></asp:Label> <asp:Label runat="server" ID="LBTotal" Text="0"></asp:Label> </th>
                                <th scope="col" style="background-color:#5D6D7E;color:#D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Total a liquidar= <asp:Label runat="server" ID="LBMonedaTSoli" Text="L."></asp:Label> <asp:Label runat="server" ID="LBTSoli" Text="0"></asp:Label> </th>

                            </tr>
                        </thead>
                        <tbody>  
                           
                           <%-- <tr>                               
                                <td> Sub Total</td>   
                                <td> Total</td>                                                               
                            </tr>     --%>                         
                           
                        </tbody>
                    </table>
                </div>
                                </ContentTemplate>                                
                            </asp:UpdatePanel>
                            <br />
                            <br />
                            <asp:Label Text="" runat="server" ID="LBComentarioJefe" style="color:red;" />
                        </div>
                    </div>
                </div>
            </div>

             <%--COTIZACION--%>
             <div class="row" runat="server" id="DIVCotiza" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Cotización</h4>
                           <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="form-group row">
                                                <label class="col-sm-8 col-form-label">Compañia de vuelo</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtCompañia"  CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Costo($)</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="txtcosto" AutoPostBack="true"  CssClass="form-control" runat="server" Enabled="false" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                       
                                    </div>
                                     <div class="row">                                                                         
                                         <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Comentario</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox AutoPostBack="true" ID="txtcomentario"  CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
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
            <%--COTIZACION--%>

            <div class="row" runat="server" id="DIVComentarioAprob" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Comentario</h4>
                            <div class="form-group">
                              <asp:TextBox runat="server" ID="txtcomentarioAprobar" CssClass="form-control" TextMode="MultiLine" Rows="3"  />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
           
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear Solicitud</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrearPermiso" OnClick="BtnCrearPermiso_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Crear Solicitud" />
                                        <asp:Button ID="BtnDevolverCotizacion" OnClick="BtnDevolverCotizacion_Click" Visible="false" CssClass="btn btn-success mr-2" runat="server" Text="Devolver Cotización" />
                                        <asp:Button ID="BtnCancelar" OnClick="BtnCancelar_Click" Visible="false" class="btn btn-danger mr-2" runat="server" Text="Devolver solicitud" />
                                        <asp:Button ID="BtnCancelarSolicitud" OnClick="BtnCancelarSolicitud_Click" Visible="false" class="btn btn-warning mr-2" runat="server" Text="Cancelar solicitud" />
                                        <asp:Button ID="BtnPrueba" OnClick="BtnPrueba_Click" Visible="false" class="btn btn-warning mr-2" runat="server" Text="PRUEBA" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="tab-pane fade" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Permisos creados</h4>
                                    <p>Top 10 viáticos finalizados</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel ID="UpdateGridView" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                            <asp:GridView ID="GVViaticosTerminados" runat="server"
                                                CssClass="table"
                                                          PagerStyle-CssClass="pgr"
                                                          HeaderStyle-CssClass="table"
                                                          RowStyle-CssClass="rows"
                                                          AutoGenerateColumns="false"
                                                          AllowPaging="true"
                                                          GridLines="None"
                                                         HeaderStyle-HorizontalAlign="center"
                                                         PageSize="10">
                                                         <Columns>                                                           
                                                             <asp:BoundField DataField="fechaInicio" HeaderText="Fecha de viaje" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="fechaFin" HeaderText="Finalizado" ItemStyle-HorizontalAlign="center" Visible="false" />
                                                             <asp:BoundField DataField="MotivoViaje" HeaderText="Motivo de viaje" ItemStyle-HorizontalAlign="center" Visible="false"/>
                                                             <asp:BoundField DataField="TipoViaje" HeaderText="Destino" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Empleado" HeaderText="Nombre" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Total" HtmlEncode=False DataFormatString="{0:n}" HeaderText="Costo Total" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Viaticos" HeaderText="Código" Visible="false" />
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
        </div>
    </div>
    <!--MODAL SOLICITUD -->
        <div id="modalViaticos" class="modal bs-example-modal-lg" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel2">¿Desea enviar solicitud?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModarEnviar" OnClick="btnModarEnviar_Click"  CssClass="btn btn-success mr-2" Text="Solicitar" />
                                </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrar" OnClick="btnModalCerrar_Click"  CssClass="btn btn-danger mr-2" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
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
                        <h4 class="modal-title" id="myLargeModalLabel">¿Desea devolver solicitud?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalDevolver" OnClick="btnModalDevolver_Click"  CssClass="btn btn-success mr-2" Text="Devolver" />
                                </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarDevolver" OnClick="btnModalCerrarDevolver_Click"   CssClass="btn btn-danger mr-2" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL DEVOLVER -->
     <!--MODAL DEVOLVER COTIZACION-->
        <div class="modal bs-example-modal-lg" id="modalCotizar" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel3">¿Desea devolver cotización?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCotizar" OnClick="btnModalCotizar_Click" CssClass="btn btn-success mr-3" Text="Devolver" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarCotiza" OnClick="btnModalCerrarCotiza_Click"  CssClass="btn btn-danger mr-3" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL DEVOLVER COTIZACION-->
        <!--MODAL CANCELAR-->
        <div class="modal bs-example-modal-lg" id="modalCancelar" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">¿Desea cancelar solicitud?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalECancelar" OnClick="btnModalECancelar_Click" CssClass="btn btn-success mr-3" Text="Cancelar" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalECerrar" OnClick="btnModalECerrar_Click"  CssClass="btn btn-danger mr-3" Text="Cerrar" />
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
     <!--MODAL CONFIRMAR TRANSPORTE-->
        <div class="modal bs-example-modal-lg" id="modalTransporte" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">Uso de Vehículo Personal</h4>
                    </div><br />
                   <div class="row col-12">
                       <asp:Label Text="&nbsp&nbspEl uso de vehículo propio queda a cuenta y riesgo del colaborador." runat="server" />
                       <asp:Label Text="&nbsp&nbspCualquier tipo de daño INFATLAN no se hará cargo." runat="server" /><br /><br />
                       <asp:Label Text="&nbsp&nbspAl aceptar esta condición confirma que está de acuerdo con lo &nbsp&nbspmencionado, caso contrario no se le permitirá utilizar vehículo &nbsp&nbsppersonal." runat="server" />
                   </div><br />
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="BtnConfirmarTransporte" OnClick="BtnConfirmarTransporte_Click" CssClass="btn btn-success mr-3" Text="Aceptar" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="BtnRechazarTransporte" OnClick="BtnRechazarTransporte_Click"  CssClass="btn btn-danger mr-3" Text="Rechazar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL CONFIRMAR TRANSPORTE-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
