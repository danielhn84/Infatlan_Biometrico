<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="registroVisitas.aspx.cs" Inherits="BiometricoWeb.pages.activos.registroVisitas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />
    <link href="/css/alert.css" rel="stylesheet" />

    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>

    <script type="text/javascript">
        function abrirModal() { $('#ModalTrabajo').modal('show'); }
        function cerrarModal() { $('#ModalTrabajo').modal('hide'); }
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
                        <h2>Registro de Visitas</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" visible="true">
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav_Creacion" role="tab" aria-controls="nav-profile" aria-selected="true"><i class="mdi mdi-library-books"></i>Nuevo</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Registros" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-database" style=""></i>Registros</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav_Creacion" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <%--Datos Entrada--%>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Generales</h4>
                                <h4 class="card-title">Responsable</h4>
                              <%--  <div class="row">--%>
                                <%--    <div class="col-6">--%>
<%--                                        <div class="form-group row">
                                            <label class="col-2">Fecha Solicitud</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ID="TxFechaSolicitud"  CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>--%>
                                  <%--  </div>--%>
                                  <%--  <div class="col-6">--%>
<%--                                        <div class="form-group row">
                                            <label class="col-3">Tipo Acceso</label>
                                            <div class="col-9">
                                                <asp:RadioButtonList ID="RbCambioTurno" RepeatDirection="Horizontal"  runat="server" AutoPostBack="True" >
                                                    <asp:ListItem Value="1">Permanente&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="0">Temporal</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>--%>
<%--                                        <div class="form-group row">
                                            <label class="col-3">Fecha Solicitud</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxFechaSolicitud"  CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>--%>
                               <%--     </div>--%>
                               <%-- </div>--%>



<%--                                 <div class="row">


                                 </div>--%>


                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Nombre</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ID="TxResponsable" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Identidad</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxIdentidadResponsable" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Departamento</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="TxSubgerencia" Text="" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Jefe</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxJefe" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <br />
                                <br />
                                <h4 class="card-title">Copia</h4>
                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Nombre</label>
                                            <div class="col-10">
                                              
                                            <asp:DropDownList ID="DdlNombreCopia" runat="server" AutoPostBack="true" class="form-control" ></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Identidad</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TxIdentidadCopia" CssClass="form-control" ReadOnly="true" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Permiso Extendido</h4>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-2">No.Permiso</label>
                                            <div class="col-10">
                                                <asp:TextBox runat="server"  ID="TxPermisoExtendido" Text="" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>



                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Datos Especificos</h4>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Fecha Inicio</label>
                                            <div class="col-10">
                                                <asp:TextBox ID="TxInicio" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Fecha Fin</label>
                                            <div class="col-9">
                                                <asp:TextBox ID="TxFin" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Acceso a</label>
                                            <div class="col-10">
                                                <asp:RadioButtonList ID="RadioButtonList2" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                    <asp:ListItem Value="1">CPP&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="2">CPA&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="3">Centro de Cableado&nbsp&nbsp&nbsp</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">Tarea</label>
                                            <div class="col-9">
                                                <asp:RadioButtonList ID="RadioButtonList1" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                    <asp:ListItem Value="1">Programada&nbsp&nbsp&nbsp</asp:ListItem>
                                                    <asp:ListItem Value="2">Emergencia</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-2">Personal</label>
                                            <div class="col-2">
                                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="check green col-12" data-checkbox="icheckbox_flat-green">
                                                    <asp:ListItem Value="1" Text="Interno"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                            <div class="col-2">
                                                <asp:CheckBoxList ID="CheckBoxList2" runat="server" CssClass="check green col-12" data-checkbox="icheckbox_flat-green">
                                                    <asp:ListItem Value="2" Text="Externo"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="form-group row">
                                            <label class="col-3">SdC, PdT o NdI</label>
                                            <div class="col-9">
                                                <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control"  AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                   <div class="row">
                                        <div class="col-12">
                                            <div class="form-group row">
                                                <label class="col-1">Trabajo</label>
                                                <div class="col-11">
                                                    <asp:TextBox runat="server" ID="TextBox3" CssClass="form-control"  AutoPostBack="true"></asp:TextBox>
                                                </div>
<%--                                                <div class="col-1">
                                                    <asp:Button ID="BtnAddTrabajo" Text="+" CssClass="btn btn-primary" runat="server" OnClick="BtnAddTrabajo_Click"/>
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>

                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-1">Motivo</label>
                                            <div class="col-11">
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="4" ID="TxMotivo" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-group row">
                                            <label class="col-1">Tareas a realizar</label>
                                            <div class="col-11">
                                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="4" ID="TextBox2" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                             <br />
                             <br />

                             <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto" runat="server" id="h">
                                <h4 class="card-title"><b>Personal Externo</b></h4>
                                <table class="tablesaw table-bordered table-hover table no-wrap" data-tablesaw-mode="swipe"
                                    data-tablesaw-sortable data-tablesaw-sortable-switch data-tablesaw-minimap
                                    data-tablesaw-mode-switch>
                                    <thead>
                                        <tr>
                                            <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="persist" class="border">Nombre</th>
                                            <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Identidad o Pasaporte </th>
                                            <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Empresa</th>
                                            <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB;" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border">Ingreso de equipo</th>
                                            <th scope="col" style="background-color: #5D6D7E; color: #D5DBDB; align-self:center" data-tablesaw-sortable-col data-tablesaw-priority="2" class="border" >Acción</th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <tr>
                                            <td> <asp:TextBox ID="TxNombre" runat="server"  CssClass="form-control"></asp:TextBox></td>                                                                                        
                                            <td><asp:TextBox ID="TxIdentidad" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>                                         
                                            <td>                                           
                                                <asp:DropDownList runat="server" ID="DdlEmpresa" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                                <asp:ListItem Value="1" Text="GBM"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="AFP"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Tigo"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Ingelmec"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Claro"></asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                           <td>
                                            <asp:RadioButtonList ID="RadioButtonList3" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
                                                <asp:ListItem Value="1">Si&nbsp&nbsp&nbsp</asp:ListItem>
                                                <asp:ListItem Value="2">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="BtnAgregar" runat="server" title="Nuevo" style="background-color:#5cb85c" class="btn btn-success" >
                                                             <i class="mdi mdi-plus text-white" style="-webkit-text-stroke-width: 1px"></i>
                                                </asp:LinkButton></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            </div>
                        </div>
                    </div>
                </contenttemplate>
            </asp:UpdatePanel>


                        <%--Datos Entrada--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <contenttemplate>
                    <div class="col-12 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="card-title">Personal Visita</h4>


                            <%--/TERCERA FILA--%>



                            <asp:UpdatePanel runat="server" ID="UPVisitas" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-md-12 align-self-center" style="margin-left: auto; margin-right: auto">
                                        <div class="table-responsive">
                                            <!--<table id="bootstrap-data-table" class="table table-striped table-bordered"> -->
                                            <asp:GridView ID="GvVisitas" runat="server"
                                                CssClass="table table-bordered"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="table"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None"
                                                HeaderStyle-HorizontalAlign="center"
                                                PageSize="10"
                                                Style="margin: 30px 0px 20px 0px">
                                                <Columns>
                                                    <asp:BoundField DataField="idVisita" HeaderText="Código" Visible="false" ItemStyle-HorizontalAlign="center" />
                                          
                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="center" />
                                                    <asp:BoundField DataField="identidad" HeaderText="Identidad" ItemStyle-HorizontalAlign="center" />
                                                    <asp:BoundField DataField="empresa" HeaderText="Empresa" ItemStyle-HorizontalAlign="center" />
                                                    <%-- <asp:BoundField DataField="IDUbi" HeaderText="Ubi" Visible="true" ItemStyle-HorizontalAlign="center" />--%>
                                                    <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="Btnseleccionar" Enabled="true" runat="server" Text="" class="btn btn-danger mr-2" CommandArgument='<%# Eval("idVisita") %>' CommandName="eliminar"><i class="icon-trash"></i></asp:LinkButton>
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

                </contenttemplate>
            </asp:UpdatePanel>

        </div>
    </div>




    <%--MODAL DE UBICACIONES--%>
    <div class="modal fade" id="ModalTrabajo" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LbIdUbicacion" runat="server" Text="Crear Trabajo"></asp:Label>
                    </h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <div class="col-2" style="margin-left:5%">
                                            <label class="col-form-label">Tipo</label>
                                        </div>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLTipoUbic" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-12" runat="server" id="DivMensajeUbic" visible="false" style="display: flex; background-color:tomato; justify-content:center">
                                    <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensajeUbic"></asp:Label>
                                </div>
                            </div>
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <contenttemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnAceptarUbic" runat="server" Text="Aceptar" class="btn btn-success" />
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
