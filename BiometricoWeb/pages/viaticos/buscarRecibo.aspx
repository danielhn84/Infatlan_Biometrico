<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="buscarRecibo.aspx.cs" Inherits="BiometricoWeb.pages.viaticos.buscarRecibo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                        <h2>Liquidaciones</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>

            </div>
        </div>
    </div>
     <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-solicitud-tab" data-toggle="tab" href="#nav-solicitud" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-book"></i>Enviar Recibo</a>
            <a class="nav-item nav-link" id="nav-liquidacion-tab" data-toggle="tab" href="#nav-liquidacion" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Aprobar Recibo</a>
        </div>
    </nav>
    <div class="tab-content" id="nav-tabContent">
        <br />
        <div class="tab-pane fade show active" id="nav-solicitud" role="tabpanel" aria-labelledby="nav-solicitud-tab">
            <%-- CONTENIDO 1--%>       
           
          <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                           <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Enviar recibo</h4>
                                    
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
                                                          GridLines="None"
                                                         HeaderStyle-HorizontalAlign="center"
                                                         PageSize="10" OnRowCommand="GVSolicitud_RowCommand">
                                                         <Columns>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="center">
                                                        
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnAprobar" runat="server" Text="" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("Viaticos") %>' CommandName="Crear">
                                                                        <i class="mdi mdi-comment-search-outline" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                             <asp:BoundField DataField="fechaInicio" HeaderText="Fecha de viaje" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="fechaFin" HeaderText="Finalizado" ItemStyle-HorizontalAlign="center" Visible="false" />
                                                             <asp:BoundField DataField="MotivoViaje" HeaderText="Motivo de viaje" ItemStyle-HorizontalAlign="center" Visible="false"/>
                                                             <asp:BoundField DataField="TipoViaje" HeaderText="Destino" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Empleado" HeaderText="Nombre" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Total" HeaderText="Costo Total" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Viaticos" HeaderText="Código" Visible="false" />
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

         <div class="tab-pane fade show hide" id="nav-liquidacion" role="tabpanel" aria-labelledby="nav-solicitud-tab">
            <%-- CONTENIDO 2--%>       
           
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                           <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Recibos por aprobar</h4>
                                    
                                    <div class="row">
                                        <div class="table-responsive">
                                             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                     <asp:GridView ID="GVARecibo" runat="server"
                                                          CssClass="table"
                                                          PagerStyle-CssClass="pgr"
                                                          HeaderStyle-CssClass="table"
                                                          RowStyle-CssClass="rows"
                                                          AutoGenerateColumns="false"
                                                          AllowPaging="true"
                                                          GridLines="None"
                                                         HeaderStyle-HorizontalAlign="center"
                                                         PageSize="10" OnRowCommand="GVARecibo_RowCommand">
                                                         <Columns>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="center">
                                                        
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnAprobar" runat="server" Text="" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("Viaticos") %>' CommandName="Crear">
                                                                        <i class="mdi mdi-comment-search-outline" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                             <asp:BoundField DataField="fechaInicio" HeaderText="Fecha de viaje" ItemStyle-HorizontalAlign="center" />
                                                             <asp:BoundField DataField="fechaFin" HeaderText="Finalizado" ItemStyle-HorizontalAlign="center" Visible="false" />
                                                             <asp:BoundField DataField="MotivoViaje" HeaderText="Motivo de viaje" ItemStyle-HorizontalAlign="center" Visible="false"/>
                                                             <asp:BoundField DataField="TipoViaje" HeaderText="Destino" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Empleado" HeaderText="Nombre" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Total" HeaderText="Costo Total" ItemStyle-HorizontalAlign="center"/>
                                                             <asp:BoundField DataField="Viaticos" HeaderText="Código" Visible="false" />
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

            <%-- /CONTENIDO 2--%> 
                      </div>
                 
           
             
        
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
