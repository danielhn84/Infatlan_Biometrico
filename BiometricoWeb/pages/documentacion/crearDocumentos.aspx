<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="crearDocumentos.aspx.cs" Inherits="BiometricoWeb.pages.documentacion.crearDocumentos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <script type="text/javascript">
        function openModal() { $('#ModalCargar').modal('show'); }
        function openModalCorreos() { $('#ModalCorreos').modal('show'); }
        function closeModal() { $('#ModalCargar').modal('hide'); }
        function closeModalCorreos() { $('#ModalCorreos').modal('hide'); }
    </script>
    <link href="/css/breadcrumb.css" rel="stylesheet" />
    <link href="/css/fstdropdown.css" rel="stylesheet" />
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

    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#navTiposDoc" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-library-books"> </i>Secciones</a>
                <a class="nav-item nav-link" id="A1" data-toggle="tab" href="#navCrear" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-plus" > </i>Crear</a>
            </div>
        </nav>
    </div>

    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="navTiposDoc" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Tipos de documentos</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GvTipos" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true"
                                                GridLines="None" OnRowCommand="GvTipos_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GvTipos_PageIndexChanging"> 
                                                <Columns>
                                                    <asp:BoundField DataField="idTipoDoc" HeaderText="No." />
                                                    <asp:BoundField DataField="nombre" HeaderText="Tipo" />
                                                    <asp:BoundField DataField="activo" HeaderText="Estado" />
                                                    <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnEditar" runat="server" title="Nuevo" style="background-color:#5cb85c" class="btn btn-success" CommandArgument='<%# Eval("idTipoDoc") %>' CommandName="NuevoDoc">
                                                                <i class="mdi mdi-plus text-white"></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="BtnEntrar" runat="server" title="Entrar" style="background-color:#5bc0de" class="btn" CommandArgument='<%# Eval("idTipoDoc") %>' CommandName="EntrarDoc">
                                                                <i class="mdi mdi-redo text-white"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="tab-pane fade" id="navCrear" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <br />
            <asp:UpdatePanel ID="UPBuzonGeneral" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--MODAL CARGA--%>
    <div class="modal fade" id="ModalCargar" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelDescarga">Agregar documento de 
                                <asp:Literal runat="server" ID="LitTitulo"></asp:Literal>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="row col-12">
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Nombre</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="TxNombre" />
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Categoría</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLCategoria" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="DDLCategoria_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3" runat="server" id ="DivEmpleados" visible="false">
                                    <div class="row col-6">
                                    </div>
                                    <div class="row col-6">
                                        <div class="col-4 ml-2"></div>
                                        <asp:LinkButton Text="Agregar" runat="server" ID="LBAgregarCorreos" OnClick="LBAgregarCorreos_Click"/>
                                    </div>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="row col-6">
                                        <label class="col-4">Confirmación de lectura</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLConfirmacion" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <label class="col-4">Envío de correo</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLCorreo" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLCorreo_SelectedIndexChanged">
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DivCorreos" visible="false" class="row col-12 mt-3">   
                                    <div runat="server" id="DivRecurrencia" visible="false" class="row col-6">
                                        <label class="col-4 col-form-label">Frecuencia</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLRecurrencia" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLRecurrencia_SelectedIndexChanged">
                                                <asp:ListItem Value="0" Text="Una vez"></asp:ListItem>
                                                <%--<asp:ListItem Value="1" Text="Específica"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row col-6">   
                                        <label class="col-4 col-form-label">Fecha Envío</label>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" ID="TxFecha" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-6">
                                        <div class="col-4"></div>
                                        <div class="col-8">
                                            <label class="custom-control custom-switch m-b-0">
                                                <input type="checkbox" runat="server" id="CBxAdjunto" class="custom-control-input">
                                                <span class="custom-control-label">Documento adjunto</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DivSiempre" visible="false" class="row col-12 mt-3">   
                                    <div class="row col-6">
                                        <label class="col-4 col-form-label">Cada</label>
                                        <div class="col-4">
                                            <asp:TextBox runat="server" ID="TxFrecuencia" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-4">
                                            <asp:DropDownList runat="server" ID="DDLFormatoFrecuencia" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Día(s)"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Mes(es)"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Año(s)"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div runat="server" class="row col-6">   
                                        <label class="col-4 col-form-label">Durante</label>
                                        <div class="col-4">
                                            <asp:TextBox runat="server" ID="TxDurante" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-4">
                                            <asp:DropDownList runat="server" ID="DDLDurante" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="Día(s)"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Mes(es)"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Año(s)"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row col-12">
                                    <div class="row col-6 mt-3">
                                        <label class="col-4 col-form-label">Estado</label>
                                        <div class="col-8">
                                            <asp:DropDownList runat="server" ID="DDLEstado" CssClass="form-control">
                                                <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                
                                    <div class="row col-6 mt-3">
                                        <label class="col-4 col-form-label">Archivo</label>
                                        <div class="col-8">
                                            <asp:FileUpload runat="server" ID="FUArchivo" CssClass="form-control"></asp:FileUpload>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 mt-3" runat="server" id="DivMensaje" visible="false" style="display: flex; background-color:tomato; justify-content:center">
                                    <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbAdvertencia"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnCargar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnCargar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnCargar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--MODAL AGREGAR CORREOS--%>
    <div class="modal fade" id="ModalCorreos" role="dialog" aria-labelledby="exampleModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header table-dark">
                    <h4 class="modal-title" id="ModalLabelModificacionTipo">
                        <asp:Label CssClass=" text-white" ID="Label1" runat="server" Text="Agregar correos"></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="row col-12">
                                    <div class="col-3" style="margin-left:2%">
                                        <label>Empleado</label>
                                    </div>
                                    <div class="col-6">
                                        <asp:DropDownList runat="server" ID="DDLEmpleados" AutoPostBack="true" CssClass="fstdropdown-select form-control"></asp:DropDownList>
                                    </div>
                                    <asp:LinkButton ID="BtnAgregarCorreo" runat="server" title="Agregar" class="btn btn-success" OnClick="BtnAgregarCorreo_Click">
                                        <i class="mdi mdi-plus text-white"></i>
                                    </asp:LinkButton>
                                </div>
                                <div class="row col-12 mt-3">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GvCorreos" runat="server"
                                            CssClass="mydatagrid"
                                            PagerStyle-CssClass="pgr"
                                            HeaderStyle-CssClass="header"
                                            RowStyle-CssClass="rows"
                                            AutoGenerateColumns="false"
                                            AllowPaging="true"
                                            GridLines="None" OnPageIndexChanging="GvCorreos_PageIndexChanging"
                                            PageSize="5" OnRowCommand="GvCorreos_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="idEmpleado" Visible="false" />
                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                                <asp:BoundField DataField="emailEmpresa" HeaderText="correo" />
                                                <asp:TemplateField HeaderText="Seleccione" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="BtnEditar" runat="server" title="Nuevo" Style="background-color: #d9534f" class="btn" CommandArgument='<%# Eval("idEmpleado") %>' CommandName="BorrarCorreo">
                                                                <i class="mdi mdi-delete text-white"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div class="col-12" runat="server" id="DivMensajeCorreo" visible="false" style="display: flex; background-color:tomato; justify-content:center">
                                    <asp:Label runat="server" CssClass="col-form-label text-white" ID="LbMensajeCorreo"></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                            <asp:Button ID="BtnAgregar" runat="server" Text="Aceptar" class="btn btn-success" OnClick="BtnAgregar_Click"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="/js/fstdropdown.js"></script>
    <script>
        function setDrop() {
            if (!document.getElementById('third').classList.contains("fstdropdown-select"))
                document.getElementById('third').className = 'fstdropdown-select';
            setFstDropdown();
        }
        setFstDropdown();
        function removeDrop() {
            if (document.getElementById('third').classList.contains("fstdropdown-select")) {
                document.getElementById('third').classList.remove('fstdropdown-select');
                document.getElementById("third").fstdropdown.dd.remove();
            }
        }
        function addOptions(add) {
            var select = document.getElementById("fourth");
            for (var i = 0; i < add; i++) {
                var opt = document.createElement("option");
                var o = Array.from(document.getElementById("fourth").querySelectorAll("option")).slice(-1)[0];
                var last = o == undefined ? 1 : Number(o.value) + 1;
                opt.text = opt.value = last;
                select.add(opt);
            }
        }
        function removeOptions(remove) {
            for (var i = 0; i < remove; i++) {
                var last = Array.from(document.getElementById("fourth").querySelectorAll("option")).slice(-1)[0];
                if (last == undefined)
                    break;
                Array.from(document.getElementById("fourth").querySelectorAll("option")).slice(-1)[0].remove();
            }
        }
        function updateDrop() {
            document.getElementById("fourth").fstdropdown.rebind();
        }
    </script>
</asp:Content>
