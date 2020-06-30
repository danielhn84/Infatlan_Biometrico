<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="costos.aspx.cs" Inherits="BiometricoWeb.pages.mantenimiento.costos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        function openModal() { $('#modalCosto').modal('show'); }
    </script>
    <!--PARA CERRAR MODAL-->
    <script type="text/javascript">
        function closeModal() { $('#modalCosto').modal('hide'); }
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
                        <h2>Costos</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
     <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-solicitud-tab" data-toggle="tab" href="#nav-solicitud" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-book"></i>Costos</a>
            <%--<a class="nav-item nav-link" id="nav-hotel-tab" data-toggle="tab" href="#nav-hotel" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Nuevo Hotel</a>
            <a class="nav-item nav-link" id="nav-habitacion-tab" data-toggle="tab" href="#nav-habitacion" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Nueva Habitación</a>--%>
        </div>
    </nav>
    <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-solicitud" role="tabpanel" aria-labelledby="nav-solicitud-tab">
             <div class="row">
        <div class="col-12 grid-margin stretch-card">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Seleccione categoría y tipo de viaje</h4>
                    <asp:UpdatePanel ID="UPCatTipoV" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                               
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Categoría</label>
                                                <div class="col-sm-12">
                                                    <asp:DropDownList runat="server" ID="DDLCategoria" OnTextChanged="DDLCategoria_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Tipo de viaje</label>
                                                <div class="col-sm-12">
                                                    <asp:DropDownList runat="server" ID="DDLTipoViaje" OnTextChanged="DDLTipoViaje_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
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
                    <h4 class="card-title">Costos por categoría</h4>
                    <asp:UpdatePanel ID="UPCostoTotal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                               
                             <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaCirculacion">Circulación</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox runat="server" ID="txtCirculacion" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaTransporte">Transpote</label>
                                                <div class="col-sm-12">
                                                     <asp:TextBox runat="server" ID="txtTransporte" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>        
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaPeaje">Peaje</label>
                                                <div class="col-sm-12">
                                                     <asp:TextBox runat="server" ID="txtPeaje" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>                                                                            
                                    </div> 
                             <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaHospedaje">Hospedaje</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox runat="server" ID="txtHospedaje" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaDepre">Depreciación</label>
                                                <div class="col-sm-12">
                                                     <asp:TextBox runat="server" ID="txtDepre" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>        
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaAlimento">Alimento</label>
                                                <div class="col-sm-12">
                                                     <asp:TextBox runat="server" ID="txtAlimento" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>                                                                            
                                    </div>
                             <div class="row" runat="server" id="DIVInternacional" visible="false">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaCAB">Centro América y Belice</label>
                                                <div class="col-sm-12">
                                                    <asp:TextBox runat="server" ID="txtCABelice" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBMonedaPaisDolar">Países área dolar</label>
                                                <div class="col-sm-12">
                                                     <asp:TextBox runat="server" ID="txtAreaDolar" CssClass="form-control" TextMode="Number"/>
                                                </div>
                                            </div>
                                        </div>        
                                         <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-12 col-form-label" runat="server" id="LBPaisNoDolar">Países área no dolar</label>
                                                <div class="col-sm-12">
                                                     <asp:TextBox runat="server" ID="txtAreaNoDolar" CssClass="form-control" TextMode="Number"/>
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
                        <h4 class="card-title">Modificar costos</h4>

                        <div class="form-group">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnModCostos" OnClick="btnModCostos_Click" CssClass="btn btn-primary mr-2" runat="server" Text="Modificar" />

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </div>
     <!--MODAL COSTOS-->
        <div class="modal bs-example-modal-lg" id="modalCosto" tabindex="-1" data-backdrop="static" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myLargeModalLabel4">¿Desea modificar costo?</h4>
                    </div>
                   
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div class="modal-footer col-12">
                               <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCrear" OnClick="btnModalCrear_Click" CssClass="btn btn-success mr-3" Text="Modificar" />
                               </div>
                                <div class="row col-3">
                                    <asp:Button runat="server" ID="btnModalCerrar" OnClick="btnModalCerrar_Click" CssClass="btn btn-danger mr-3" Text="Cancelar" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.modal-content -->
            </div>
            <!--/.modal-dialog -->
        </div>
        <!-- /MODAL COSTOS-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
