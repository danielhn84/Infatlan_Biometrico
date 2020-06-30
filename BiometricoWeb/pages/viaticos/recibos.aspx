<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="recibos.aspx.cs" Inherits="BiometricoWeb.pages.viaticos.recibos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!--PARA LLAMAR MODAL-->
    <script type="text/javascript">
        function openModal() { $('#modalEnviar').modal('show'); }
        function openModal2() { $('#modalDevolver').modal('show'); }        
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalEnviar').modal('hide'); }
        function closeModal2() { $('#modalDevolver').modal('hide'); }       
    </script>


     <%--IMAGENES--%>
    <script type="text/javascript">
        //IMAGEN1        
        function img1(input) {          
            if (input.files && input.files[0]) {
                //PRIMERA IMAGEN              
                var reader = new FileReader();
                reader.onload = function (e) {             
                    var ruta1 = e.target.result;
                    document.getElementById('<%=imgRecibo.ClientID%>').src = ruta1;
                    document.getElementById('<%=HFRecibo.ClientID%>').value = 'si';
                }
                reader.readAsDataURL(input.files[0]);
                //PRIMERA IMAGEN              
            }
        }
        //IMAGEN1
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <%-- CONTENIDO 1--%> 
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Viáticos solicitados</h4>

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
                                 <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Fecha inicio de viaje</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtFechaInicio" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Fecha final de viaje</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtFechaFin" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Tipo de viaje</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtTipoViaje" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                 <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Monto Solicitado a liquidar</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtMontoSolicitado" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Monto Liquidado</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtMontoLiquidado" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group row">
                                            <label class="col-sm-12 col-form-label">Diferencia</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtfechaDiferencia" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:TextBox runat="server" Text="" Enabled="false" ID="txtAlerta" CssClass="form-control" style="background-color:red; color:white; text-align:center;"/>  
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
            <%-- /CONTENIDO 1--%>
    <asp:Label Text="" runat="server" ID="LBComentario" CssClass="col-form-label" style="color:red;"/>
    <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Favor subir foto de su boucher de pago.</h4>
                    <asp:FileUpload ID="FURecibo" runat="server" onchange="img1(this);" />
                     <br />
                    <br />
                        <img id="imgRecibo" runat="server" height="350" width="350" src="../../images/vistaPrevia1.JPG" />  
                    <br />
                    <br />
                    <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">  
                        <ContentTemplate>
                            <asp:Button ID="BtnCrear" OnClick="BtnCrear_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Enviar" />
                            <asp:Button ID="BtnDevolver" OnClick="BtnDevolver_Click" Visible="false" CssClass="btn btn-danger mr-2" runat="server" Text="Devolver" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

      <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HFRecibo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

     <!--MODAL APROBAR-->
        <div class="modal bs-example-modal-lg" id="modalEnviar" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel3">¿Desea enviar Recibo?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalEnviar" OnClick="btnModalEnviar_Click" CssClass="btn btn-success mr-3" Text="Enviar" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrar" OnClick="btnModalCerrar_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                         <Triggers>
                        <asp:PostBackTrigger ControlID="btnModalEnviar" />
                    </Triggers>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
     <!-- /MODAL APROBAR-->
     <!--MODAL DEVOLVER-->
        <div class="modal bs-example-modal-lg" id="modalDevolver" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">¿Desea devolver solicitud?</h4>
                    </div>
                    <br />
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                       
                    <div class="row col-12">
                        <div class="align-self-center col-12" style="margin-left: auto; margin-right: auto">
                        <asp:Label Text="Motivo por el que devuelve recibo." CssClass="col-form-label" runat="server" />
                            </div>
                        <div class="align-self-center col-12" style="margin-left: auto; margin-right: auto">
                        <asp:TextBox runat="server" ID="txtcomentario" CssClass="form-control" TextMode="MultiLine" Rows="3"/>
                        </div>
                        <br />
                        <br />
                         <div class="align-self-center col-9" style="margin-left: auto; margin-right: auto">
                       <h5 runat="server" visible="false" id="H5Alerta" class="text-danger"><i class="fa fa-exclamation-triangle"></i>Ingrese motivo por el que devuelve solicitud.</h5>
                            </div>
                    </div>
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalDevolver" OnClick="btnModalDevolver_Click" CssClass="btn btn-success mr-3" Text="Devolver" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrarDevolver" OnClick="btnModalCerrarDevolver_Click"  CssClass="btn btn-danger mr-3" Text="Cerrar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
     <!-- /MODAL DEVOLVER-->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
