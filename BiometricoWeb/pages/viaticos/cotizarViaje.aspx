<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="cotizarViaje.aspx.cs" Inherits="BiometricoWeb.pages.viaticos.cotizarViaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!--PARA LLAMAR MODAL-->
    <script type="text/javascript">
        function openModal() { $('#modalCotizar').modal('show'); }
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalCotizar').modal('hide'); }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-datos" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus"></i>Cotizar Viaje</a>
         
        </div>
        
        <div class="col-md-6" style="float:right; height:100%; visibility:visible">
           
        </div>
    </nav>
    <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-datos" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="" style="margin-left: auto;" runat="server">
                
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
                                                    <asp:TextBox AutoPostBack="true" ID="TxFechaInicio"  CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Finaliza</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="TxFechaRegreso" AutoPostBack="true"  CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Destino</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtDestino"  CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Motivo viaje</label>
                                                <div class="col-sm-11">
                                                   <asp:TextBox AutoPostBack="true" ID="txtMotivoViaje"  CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Tipo de viaje</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtTipoViaje"  CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Empleado</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtEmpleado"  CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
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
                            <h4 class="card-title">Cotización</h4>
                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="form-group row">
                                                <label class="col-sm-8 col-form-label">Compañia de vuelo</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox AutoPostBack="true" ID="txtCompañia"  CssClass="form-control" runat="server" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Costo($)</label>
                                                <div class="col-sm-11">
                                                    <asp:TextBox ID="txtcosto" AutoPostBack="true"  CssClass="form-control" runat="server" TextMode="Number" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                       
                                    </div>
                                     <div class="row">                                                                         
                                         <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Comentario</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox AutoPostBack="true" ID="txtcomentario"  CssClass="form-control" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
                                                </div>
                                                 <asp:label runat="server" ID="LBComRRHH" style="color:red" class="col-sm-4 col-form-label"></asp:label>
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
                            <h4 class="card-title">Crear Cotización</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrearPermiso" OnClick="BtnCrearPermiso_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Aprobar Cotización" />
                                      
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </div>
     <!--MODAL COTIZAR -->
        <div class="modal bs-example-modal-lg" id="modalCotizar" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel">¿Desea enviar cotización?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalEnviar" OnClick="btnModalEnviar_Click"  CssClass="btn btn-success mr-2" Text="Enviar" />
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
        <!-- /MODAL COTIZAR -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
