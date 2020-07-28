<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="reportes.aspx.cs" Inherits="BiometricoWeb.pages.mantenimiento.reportes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="DIVLenarLiqui">
        <h3 class="card-title" style="color: #808080;"><i class="" ></i>Reportes</h3>
        <h5 class="card-subtitle">Reportes generados se descargaran automaticamente</h5><br />
        <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
            data-tablesaw-sortable="data-tablesaw-sortable-switch data-tablesaw-minimap data-tablesaw-mode-switch">
            <thead>
                <tr>
                    <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Nombre Reporte</th>
                    <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Seleccione Parametro </th>
                    <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB; text-align:center;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Reporte </th>            
                </tr>
            </thead>
            <tbody>

                <tr>
                    <tr>
                        <td class="title">Reporte por departamento</td>
                        <td>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="DDLReporteDepto"  AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="text-align:center;">
                            <asp:LinkButton ID="btnReporteDeptos" OnClick="btnReporteDeptos_Click" runat="server" Text="" class="btn btn-inverse-primary mr-2"><i class="mdi mdi-cloud-download" ></i></asp:LinkButton>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="title">Reporte por empleado</td>
                        <td>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="DDLReporteEmpleado" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="text-align:center;">
                            <asp:LinkButton ID="btnReporteEmpleados" OnClick="btnReporteEmpleados_Click" runat="server" Text="" class="btn btn-inverse-primary mr-2"><i class="mdi mdi-cloud-download" ></i></asp:LinkButton>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="title">Reporte por motivo de viaje</td>
                        <td>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="DDLMotivoViaje" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="text-align:center;">
                        <asp:LinkButton ID="btnReporteMotivoViaje" OnClick="btnReporteMotivoViaje_Click" runat="server" Text="" class="btn btn-inverse-primary mr-2"><i class="mdi mdi-cloud-download" ></i></asp:LinkButton>                         
                            </td>
                    </tr>
                </tr>

            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
