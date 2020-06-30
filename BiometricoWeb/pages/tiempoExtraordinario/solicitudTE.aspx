<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="solicitudTE.aspx.cs" Inherits="BiometricoWeb.pages.tiempoExtraordinario.solicitudTE" %>
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
        function closeModal() { $('#VisualizarImagen').modal('show'); }
        function openModalImagen() { $('#VisualizarImagen').modal('show'); }

        function openModal() { $('#InformativoModal').modal('show'); }

        function openDescargarModal() { $('#DescargaModal').modal('show'); }

        function closeAprobarJefeModal() { $('#AprobarJefeModal').modal('hide'); }
        function openAprobarJefeModal() { $('#AprobarJefeModal').modal('show'); }

        function closeAprobarSubgerenteModal() { $('#AprobarSubgerenteModal').modal('hide'); }
        function openAprobarSubgerenteModal() { $('#AprobarSubgerenteModal').modal('show'); }

        function closeAprobarRRHHModal() { $('#AprobarRRHHModal').modal('hide'); }
        function openAprobarRRHHModal() { $('#AprobarRRHHModal').modal('show'); }

        function closeMasInformacionColaborador() { $('#MasInformacionColaborador').modal('hide'); }
        function OpenMasInformacionColaborador() { $('#MasInformacionColaborador').modal('show'); }


        function closeSolicitudModificada() { $('#SolicitudModificada').modal('hide'); }
        function OpenSolicitudModificada() { $('#SolicitudModificada').modal('show'); }

       
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>

    <script type="text/javascript">
        function javas(e) {
            if (e.checked) {
                $('#VisualizarImagen').modal('show');
             <%--   document.getElementById('<%=Checkbox1.ClientID%>').checked = false;--%>
            }
        }
    </script>

    <script type="text/javascript">
        function showpreview1(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var ruta8 = e.target.result;
                    document.getElementById('<%=Img1.ClientID%>').src = ruta8;
                    document.getElementById('<%=TxHojaServicio.ClientID%>').value = 'si';
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
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
                        <h2 class="text-themecolor">
                            <asp:Label ID="TituloPagina" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label>
                        </h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <nav>
        <div class="nav nav-pills " id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-datos-tab" data-toggle="tab" href="#nav-crearSolicitud" role="tab" aria-controls="nav-home" aria-selected="true"><i class="mdi mdi-plus"></i>Solicitud</a>
            <a class="nav-item nav-link" runat="server" visible="true" id="nav_tecnicos_tab" data-toggle="tab" href="#nav-tecnicos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-book"></i>Mis Solicitudes</a>
            <a class="nav-item nav-link" runat="server" visible="true" id="nav_modificarSolicitud_tab" data-toggle="tab" href="#nav-modificar" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi  mdi-pencil"></i>Pendiente Modificar</a>
        </div>
    </nav>

    <div class="tab-content" id="nav-tabContent">
        <br />
        <%-- SECCION 1---%>
        <div class="tab-pane fade show active" id="nav-crearSolicitud" role="tabpanel" aria-labelledby="nav-datos-tab">
            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title"><b>Datos Generales Empleado</b> </h4>
                            <label runat="server" id="Label3" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA. SI TIENE PERMISOS PENDIENTES DE APROBACIÓN SOLICITE A SU JEFE INMEDIATO QUE SE LO AUTORICE PRONTAMENTE Y POSTERIORMENTE A TALENTO HUMANO .</b> </small></label>
                            <label runat="server" id="Label19" class="col-sm-12" style="text-align: center; color: tomato"><small><b>SI EN EL DIA INGRESA MAS DE UNA SOLICITUD DE LA MISMA FECHA DE INICIO LAS REDUCCIONES SOLO SE VAN A EFECTUAR EN LA PRIMER SOLICITUD QUE RRHH APRUEBE</b> </small></label>
                            <br>
                            <br>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <label class="col-sm-2 col-form-label" runat="server" visible="false" id="Titulo">Solicitud de Empleado</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="DDLEmpleadoSolicitud" Visible="false" runat="server" class="fstdropdown-select form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLEmpleadoSolicitud_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">No. Empleado</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxEmpleado" ReadOnly="true" placeholder="" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Jefe</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxJefe" ReadOnly="true" placeholder="" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Subgerencia</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxSubgerencia" ReadOnly="true" placeholder="" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Turno</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxTurno" ReadOnly="true" placeholder="" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <label class="col-sm-6" style="text-align: right">Cambio de turno</label>
                                        <div class="col-sm-6">
                                            <asp:RadioButtonList ID="RbCambioTurno" RepeatDirection="Horizontal" Width="90px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RbCambioTurno_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="row" runat="server" id="rowSolicitudCambioTurno" visible="false">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label" runat="server" id="lbNoEmpleado">No. Empleado</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="DDLCambioTurnoColaborador" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DDLCambioTurnoColaborador_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label">Turno</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="TxCambioTurno" ReadOnly="true" placeholder="" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:TextBox ID="TxMotivoCambioTurno" placeholder="Ingresar motivo del cambio de turno....." class="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title"><b>Datos Generales Solicitud</b></h4>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <div class="col-md-9 col-form-label" style="text-align: right">
                                                    <label style="text-align: center"><b>TOTAL HORAS</b></label>
                                                </div>

                                                <div class="col-md-3" style="text-align: right">
                                                    <asp:TextBox ID="TxTotalHoras" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center; font-size: large" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>DIURNAS</b></label>
                                                    <asp:TextBox ID="TxHrDiurnas" ReadOnly="true" Text="00:00 (0.0 Hrs)" CssClass="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>NOCTURNAS</b></label>
                                                    <asp:TextBox ID="TxHrNoc" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>NOCTURNAS/NOCTURNAS</b></label>
                                                    <asp:TextBox ID="TxHrNocNoc" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>DOMINGOS/ FERIADOS</b></label>
                                                    <asp:TextBox ID="TxHrDomFeriado" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br>

                            <asp:UpdatePanel ID="UpdatePanelFechas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-sm-6" style="text-align: right">Trabajo realizado de forma:</label>
                                                <div class="col-sm-6">
                                                    <asp:RadioButtonList ID="RbFormaTrabajo" RepeatDirection="Horizontal" Width="160px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RbFormaTrabajo_SelectedIndexChanged">
                                                        <asp:ListItem Value="1">Remota</asp:ListItem>
                                                        <asp:ListItem Value="2">Presencial</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Fecha Inicio</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="TxFechaInicio" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" OnTextChanged="TxFechaInicio_TextChanged" AutoPostBack="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Fecha Fin</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxFechaRegreso" placeholder="1900-12-31 00:00:00" class="form-control" runat="server" TextMode="DateTimeLocal" OnTextChanged="TxFechaRegreso_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label runat="server" id="LbModificar" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>SE VA A MANTENER EL MISMO ESTADO QUE SE REGISTRO CUANDO CREO LA SOLICITUD</b> </small></label>
                                            <label runat="server" id="LbFechaRangoBien" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA.LA FECHA FIN DE LA SOLICITUD ESTA DENTRO DEL RANGO ESTABLECIDO (SOLICITUD SERA APROBADA POR JEFE/SUPLENTE) .</b> </small></label>

                                            <label runat="server" id="LbFechaRangoMal" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA.LA FECHA FIN DE LA SOLICITUD SUPERA EL RANGO ESTABLECIDO (SE VA REQUERIR APROBACION DEL SUBGERENTE) .</b> </small></label>
                                        </div>
                                        <br>
                                        <br>
                                        <br>
                                    </div>
                                    <div class="row" runat="server" id="DivAprobacionSubGerente" visible="false">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Motivo:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdlMotivoAprobacionSubgerente" runat="server" class="form-control" AutoPostBack="true">
                                                        <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Enfermedad"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Viaje"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Más detalle:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxSoliAprobacionSubGerente" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2" Text=""></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" id="DivComentarioJefe" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-sm-2 col-form-label">Comentario Jefe:</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="TxComentarioJefe" ReadOnly="true" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2" Text=""></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" id="DivConductor">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Solicito Conductor:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdlConductor" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlConductor_SelectedIndexChanged">
                                                        <asp:ListItem Value="2" Text="Seleccione opción..."></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Detalle:</label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList ID="DdlConductorNombre" runat="server" class="form-control" Visible="false">
                                                        <asp:ListItem Value="0" Text="Seleccione Conductor..."></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Anibal Figueroa"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Omar Santos"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="TxMotivoNoConductor" ReadOnly="true" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2" Text=""></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <div class="col-md-2">
                                                    <label class=" col-form-label">Trabajo:</label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="DdlTipoTrabajo" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="DdlTipoTrabajo_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" id="DivCategoria" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <asp:Label ID="TituloCategoria" runat="server" Text="" class="col-md-2 col-form-label"></asp:Label>

                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="DdlTipoDescripcion" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <div class="col-md-2">
                                                    <label class="col-form-label">Detalle Trabajo:</label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:TextBox ID="TxDescripcionTrabajo" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row" runat="server" id="DivSysAid">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Petición:</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="TxPeticion" placeholder="" class="form-control" runat="server" OnTextChanged="TxPeticion_TextChanged" AutoPostBack="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label">Titulo- Categoría:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="TxTituloSysaid" ReadOnly="true" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group row">
                                        <label class="col-sm-6" runat="server" id="LbCambiarHoja" visible="false" style="text-align: right">Desea cambiar la hoja de servicio:</label>
                                        <div class="col-sm-6">
                                            <asp:RadioButtonList ID="RbCambioHoja" Visible="false" RepeatDirection="Horizontal" Width="160px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RbCambioHoja_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                <asp:ListItem Value="2">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">

                                                <div class="col-md-2">
                                                    <label class="col-form-label" runat="server" visible="false" id="lbHojaServicio">Hoja de servicio:</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:FileUpload ID="FuHojaServicio" runat="server" onchange="showpreview1(this);" class="form-control" Visible="false" />
                                                    <asp:TextBox ID="TxImagenSubida" ReadOnly="true" placeholder="" class="form-control" runat="server" Visible="false"></asp:TextBox>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="col-md-12" style="align-content: center">
                                                            <asp:LinkButton runat="server" ID="btnVisualizarHoja" OnClick="btnVisualizarHoja_Click" CssClass="btn btn-primary mr-2"><i class="mdi mdi-image" visible="false"></i></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="btnDescargarHoja" OnClick="btnDescargarHoja_Click" CssClass="btn btn-success mr-2"><i class="mdi mdi-download" visible="false"></i></asp:LinkButton>
                                                        </div>
                                                        <asp:HiddenField ID="TxHojaServicio" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpDivUnaFecha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row" runat="server" id="DivUnaFecha" visible="false">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Reducciones Tiempo Extraordinario</h4>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group row">
                                                        <div class="col-md-9 col-form-label" style="text-align: right">
                                                            <label style="text-align: center"><b>TOTAL HORAS</b></label>
                                                        </div>

                                                        <div class="col-md-3" style="text-align: right">
                                                            <asp:TextBox ID="TxRealTotal" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center; font-size: large" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group row">
                                                        <div class="col-md-3">
                                                            <label class="col-sm-12" style="text-align: center"><b>DIURNAS</b></label>
                                                            <asp:TextBox ID="TxRealDiurnas" ReadOnly="true" Text="00:00 (0.0 Hrs)" CssClass="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label class="col-sm-12" style="text-align: center"><b>NOCTURNAS</b></label>
                                                            <asp:TextBox ID="TxRealNoc" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <label class="col-sm-12" style="text-align: center"><b>NOCTURNAS/NOCTURNAS</b></label>
                                                            <asp:TextBox ID="TxRealNocNoc" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <label class="col-sm-12" style="text-align: center"><b>DOMINGOS/ FERIADOS</b></label>
                                                            <asp:TextBox ID="TxRealDomingoFeriados" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br>

                                    <p class="mb-md-0"><b>ENTRADAS / SALIDAS DIAS:</b></p>
                                    <br>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Entradas</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="TxEntradas" ReadOnly="true" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Salidas</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="TxSalidas" ReadOnly="true" placeholder="" class="form-control" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- <br>--%>
                                    <hr>
                                    <p class="mb-md-0"><b>ENTRADA TURNO:</b></p>
                                    <br>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Entrada  Turno</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxEntradaTurno" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Entrada Biometrico</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxEntradaBio" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Tiempo Descontar</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxDescontarEntrada" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" visible="false" id="DivDescontarEntrada">
                                        <div class="col-md-12" style="text-align: center; color: tomato">
                                            <small><b>
                                                <asp:Label ID="LbJustificacionHEB" runat="server" Text="" Class="col-md-12" Style="text-align: center; color: tomato"></asp:Label></b> </small>
                                        </div>
                                    </div>
                                    <%-- <br>--%>
                                    <hr>


                                    <br>
                                    <p class="mb-md-0"><b>SALIDA TURNO:</b></p>
                                    <br>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Salida Turno</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxSalidaTurno" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Salida Biometrico</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxSalidaBio" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Tiempo Descontar</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxDescontarSalida" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" visible="false" id="DivDescontarSalida">
                                        <div class="col-md-12" style="text-align: center; color: tomato">
                                            <small><b>
                                                <asp:Label ID="TxJustificacionHSB" runat="server" Text="" Class="col-md-12" Style="text-align: center; color: tomato"></asp:Label></b> </small>
                                        </div>
                                    </div>
                                    <br>
                                    <hr>




                                    <br>

                                    <p class="mb-md-0"><b>ENTRADAS / SALIDAS ALMUERZO:</b></p>
                                    <br>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Salida</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxSalidaAlmuerzo" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Entrada</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxEntradaAlmuerzo" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group row">
                                                <label class="col-sm-6 col-form-label">Tiempo Descontar</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="TxTotalAlmuerzo" ReadOnly="true" placeholder="" class="form-control" runat="server" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" visible="true" id="DivTotalAlmuerzo">
                                        <div class="col-md-12" style="text-align: center; color: tomato;">
                                            <small><b>
                                                <asp:Label ID="LbTotalAlmuerzo" runat="server" Text="" Class="col-md-12" Style="text-align: center; color: tomato"></asp:Label></b> </small>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" visible="false" id="DivAlmuerzo">
                                        <div class="col-md-12" style="text-align: center; color: tomato">
                                            <small><b>
                                                <asp:Label ID="LbReintegroAlmuerzo" runat="server" Text="" Class="col-md-12" Style="text-align: center; color: tomato"></asp:Label></b> </small>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="row" runat="server" id="DivAprobacionRealRRHH" visible="true">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Aprobación Real RRHH</h4>
                            <asp:UpdatePanel ID="UpdatePanel26" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <div class="col-md-9 col-form-label" style="text-align: right">
                                                    <label style="text-align: center"><b>TOTAL HORAS</b></label>
                                                </div>

                                                <div class="col-md-3" style="text-align: right">
                                                    <asp:TextBox ID="TxTotRRHH" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center; font-size: large" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>DIURNAS</b></label>
                                                    <asp:TextBox ID="TxTotDiurnasRRHH" ReadOnly="true" Text="00:00 (0.0 Hrs)" CssClass="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>NOCTURNAS</b></label>
                                                    <asp:TextBox ID="TxTotNocRRHH" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>NOCTURNAS/NOCTURNAS</b></label>
                                                    <asp:TextBox ID="TxTotNocNocRRHH" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="col-sm-12" style="text-align: center"><b>DOMINGOS/ FERIADOS</b></label>
                                                    <asp:TextBox ID="TxTotDomFeriadoRRHH" ReadOnly="true" Text="00:00 (0.0 Hrs)" class="form-control" runat="server" BackColor="#85C1E9" Style="text-align: center" ForeColor="White" Font-Bold="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" id="Div1">
                                        <div class="col-md-12" style="text-align: center; color: tomato">
                                            <b>
                                                <asp:Label ID="LbMensajeRRHH" runat="server" Text="" Class="col-md-12" Style="text-align: center; color: tomato"></asp:Label></b>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="DivCrearSolicitud">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Crear Solicitud</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePrincipalBotones" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnCrearSolicitud" class="btn btn-primary mr-2" runat="server" Text="Enviar Solicitud" OnClick="BtnCrearSolicitud_Click" />
                                        <asp:Button ID="BtnCancelarSolicitud" class="btn btn-danger mr-2" runat="server" Text="Cancelar Solicitud" OnClick="BtnCancelarSolicitud_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="DivSolicitudModificada" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Enviar Solicitud Modificada</h4>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnEnviarModificada" class="btn btn-primary mr-2" runat="server" Text="Enviar Solicitud" OnClick="BtnEnviarModificada_Click" />
                                        <asp:Button ID="BtnCancelarModificada" class="btn btn-danger mr-2" runat="server" Text="Cancelar Solicitud" OnClick="BtnCancelarModificada_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="DivAprobarJefe" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">¿Esta seguro de autorizar la solicitud?</h4>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <label runat="server" id="LbModificarSolicitud" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA.ESTE MENSAJE APARECE UNICAMENTE EN LAS SOLICITUDES QUE SE REQUIRIO QUE EL COLABORADOR REALIZARA RESPECTIVA MODIFICACIÓN: </b></small></label>
                                        <asp:Label ID="LbMensajeModificar" runat="server" Visible="false" Text="" class="col-sm-12" Style="text-align: center; color: tomato"></asp:Label>

                                        <br>
                                        <br>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Acción:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdEstadoSoliJefe" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdEstadoSoliJefe_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Devolver a Colaborador (modificar)"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6" runat="server" id="DivMotivo" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Motivo:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdlMotivosCancelacion" runat="server" class="form-control" AutoPostBack="true">
                                                        <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Horas de trabajo no justificadas"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Solicitud duplicada"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <label runat="server" id="LbAlerta" visible="false" class="col-sm-12" style="text-align: center; color: tomato"><small><b>NOTA. LA FECHA DE LA SOLICITUD SUPERA EL RANGO ESTABLECIDO, SE SOLICITARÁ LA APROBACIÓN DEL SUBGERENTE DEL AREA QUE PERTENCE, FAVOR INGRESAR OBSERVACIÓN PARA RETROALIMENTAR AL SUBGERENTE.</b> </small></label>
                                        <div class="col-md-12">
                                            <asp:TextBox ID="TxObservacion" Visible="false" placeholder="Favor ingresar observación........" class="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <br>
                                    <br>
                                </ContentTemplate>
                            </asp:UpdatePanel>



                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnEnviarAprobacionJefe" class="btn btn-primary mr-2" runat="server" Text="Enviar" OnClick="BtnEnviarAprobacionJefe_Click" />
                                        <asp:Button ID="BtnCancelarJefe" class="btn btn-danger mr-2" runat="server" Text="Cancelar" OnClick="BtnCancelarJefe_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="DivAprobacionSubgerenteSolicitud" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">¿Esta seguro de autorizar la solicitud?</h4>
                            <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Acción:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdlAprobacionSubgerente" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlAprobacionSubgerente_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>



                                        <div class="col-md-12">
                                            <asp:TextBox ID="TxCancelacionSubgerente" Visible="false" placeholder="Favor ingresar observación de cancelacion........" class="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <br>
                                    <br>
                                </ContentTemplate>
                            </asp:UpdatePanel>



                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAprobacionSubGerente" class="btn btn-primary mr-2" runat="server" Text="Enviar" OnClick="BtnAprobacionSubGerente_Click" />
                                        <asp:Button ID="Button2" class="btn btn-danger mr-2" runat="server" Text="Cancelar" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" runat="server" id="DivAprobarRRHH" visible="false">
                <div class="col-12 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">¿Esta seguro de autorizar la solicitud?</h4>
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Acción:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdlAccionRRHH" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlAccionRRHH_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="Seleccione opción..."></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6" runat="server" id="DivMotivoCancelacionRRHH" visible="false">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Motivo:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="DdlMotivosCancelacionRRHH" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <asp:TextBox ID="TxMotivoRRHH" Visible="false" placeholder="Favor ingresar observación........" class="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <br>
                                    <br>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnEnviarRRHH" class="btn btn-primary mr-2" runat="server" Text="Enviar" OnClick="BtnEnviarRRHH_Click" />
                                        <asp:Button ID="BtnCancelar" class="btn btn-danger mr-2" runat="server" Text="Cancelar"  OnClick="BtnCancelar_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <br />
        <%-- FIN SECCION 1---%>
        <%-- SECCION 2---%>
        <div class="tab-pane fade" id="nav-tecnicos" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpdateDivBusquedas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Solicitudes creadas</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVBusqueda" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false" OnPageIndexChanging="GVBusqueda_PageIndexChanging"
                                                AllowPaging="true" OnRowCommand="GVBusqueda_RowCommand"
                                                GridLines="None"
                                                PageSize="10">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnMotivo" runat="server" Text="Motivo" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="DetalleSolicitud">
                                                                        <i class="mdi mdi-comment-search-outline text-primary" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnDocumento" runat="server" Text="Descargar" class="btn btn-inverse-success mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="HojaServicio">
                                                                        <i class="mdi mdi-download text-primary" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="No." />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Hrs Solicitadas" />
                                                    <asp:BoundField DataField="fechaInicio" HeaderText="Inicio" />
                                                    <asp:BoundField DataField="fechaFin" HeaderText="Fin" />
                                                    <asp:BoundField DataField="fechaSolicitud" HeaderText="Solicitud" />
                                                    <asp:BoundField DataField="sysAid" HeaderText="SysAid" />
                                                    <asp:BoundField DataField="nombreTrabajo" HeaderText="Trabajo" />
                                                    <asp:BoundField DataField="descripcionEstado" HeaderText="Estado" />



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
        <%-- FIN SECCION 2---%>

        <%-- SECCION 3---%>
        <div class="tab-pane fade" id="nav-modificar" role="tabpanel" aria-labelledby="nav-tecnicos-tab">
            <asp:UpdatePanel ID="UpPendienteModificar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <h4 class="card-title">Solicitudes  Pendientes Modificar</h4>
                                    <p>Ordenados por fecha de creación</p>
                                    <div class="row">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GVPendienteModificar" runat="server"
                                                CssClass="mydatagrid"
                                                PagerStyle-CssClass="pgr"
                                                HeaderStyle-CssClass="header"
                                                RowStyle-CssClass="rows"
                                                AutoGenerateColumns="false"
                                                AllowPaging="true" 
                                                GridLines="None" OnRowCommand="GVPendienteModificar_RowCommand"
                                                PageSize="10" OnPageIndexChanging="GVPendienteModificar_PageIndexChanging" >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnMotivo" runat="server" Text="Motivo" class="btn btn-inverse-primary mr-2" CommandArgument='<%# Eval("idSolicitud") %>' CommandName="DetalleSolicitud">
                                                                         <i class="mdi mdi-ballot text-primary" ></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
            
                                                    <asp:BoundField DataField="idSolicitud" HeaderText="No." />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Hrs Solicitadas" />
                                                    <asp:BoundField DataField="fechaInicio" HeaderText="Inicio" />
                                                    <asp:BoundField DataField="fechaFin" HeaderText="Fin" />
                                                    <asp:BoundField DataField="fechaSolicitud" HeaderText="Creacion" />
                                                    <asp:BoundField DataField="sysAid" HeaderText="SysAid" />
                                                    <asp:BoundField DataField="nombreTrabajo" HeaderText="Trabajo" />
                                                    <asp:BoundField DataField="descripcionEstado" HeaderText="Estado" />
                                                    <asp:BoundField DataField="observacionAprobacionJefe" HeaderText="Observación" />
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
        <%-- SECCION 3---%>
    </div>

    <%--    MODAL INFORMATIVA--%>
    <div class="modal fade" id="InformativoModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <h4 class="modal-title">Solicitud Tiempo Extraordinario</h4>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdateAutorizarMensaje" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbInformacionTE" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbInformacionPreguntaTE" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel48" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarSolicitud" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarSolicitud_Click" />
                        </contenttemplate>
                        <triggers>
                            <asp:PostBackTrigger ControlID="BtnEnviarSolicitud" />
                        </triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--    MODAL DESCARGAR ARCHIVO--%>
    <div class="modal fade" id="DescargaModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title" id="ModalLabelDescarga">Descargar Hoja Servicio 
                                <asp:Label ID="LbPermisoDescarga" runat="server" Text=""></asp:Label>
                            </h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <div class="col-md-12">
                                <div class="form-group row">
                                    <label class="col-sm-12 col-form-label">
                                        <h4>Privacidad de documentos</h4>
                                    </label>
                                    <div class="col-sm-12" style="text-align: justify">
                                        Este documento adjunto es confidencial, especialmente en lo que hace referencia a los datos personales que puedan contener y se dirigen exclusivamente al destinatario referenciado. Si usted no lo es y ha descargado este archivo o tiene conocimiento del mismo por cualquier motivo, le rogamos nos lo comunique por este medio y proceda a borrarlo, y que, en todo caso, se abstenga de utilizar, reproducir, alterar, archivar o comunicar a terceros el documento adjunto.
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="Label1" runat="server" Text="" Class="col-sm-12" Style="color: indianred; text-align: center;"></asp:Label>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnDescargarArchivo" runat="server" Text="Descargar" class="btn btn-success" OnClick="BtnDescargarArchivo_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnDescargarArchivo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--    MODAL  VISUALIZAR LA IMAGEN--%>
    <div class="modal fade" id="VisualizarImagen" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title" id="ModalLabelUsuario">Hoja de Servicio</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <img id="Img1" height="520" width="470" src="../../images/vistaPrevia1.JPG" style="border-width: 0px;" runat="server" />
                </div>


                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdateUsuarioBotones" runat="server">
                        <contenttemplate>
                            <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>--%>
                            <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" class="btn btn-secondary" OnClick="BtnCerrar_Click" />
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--    MODAL  APROBAL JEFE--%>
    <div class="modal fade" id="AprobarJefeModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">


                    <asp:UpdatePanel ID="UpTituloAprobarJefeModal" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="TituloAprobacionJefe" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </contenttemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdateAprobarJefe" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAprobarJefe" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbAprobarJefePregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnAprobarJefeModal" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnAprobarJefeModal_Click" />
                        </contenttemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--    MODAL  APROBAL SUBGERENTE--%>
    <div class="modal fade" id="AprobarSubgerenteModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <asp:UpdatePanel ID="UpTituloSubgerente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="LbTituloSubGerente" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdateAprobarSubgerente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAprobarSubgerente" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbAprobarSubgerentePregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel23" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarAprobaciónSubgerente" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarAprobaciónSubgerente_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--    MODAL  APROBAL RRHH--%>
    <div class="modal fade" id="AprobarRRHHModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">


                    <asp:UpdatePanel ID="UpTituloAprobarRRHHModal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="LBTituloRRHH" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdateAprobarRRHH" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbAprobarRRHH" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbAprobarRRHHPregunta" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnAprobarRrhhModal" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnAprobarRrhhModal_Click" />
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

   <%--    MODAL INFORMATIVA--%>
    <div class="modal fade" id="SolicitudModificada" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">

                    <h4 class="modal-title">Solicitud Tiempo Extraordinario Modificada</h4>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbInformacionTEModificada" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbInformacionPreguntaTEModificada" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            <asp:Button ID="BtnEnviarSoliModificada" runat="server" Text="Enviar" class="btn btn-success" OnClick="BtnEnviarSoliModificada_Click"/>
                        </contenttemplate>
                        <triggers>
                            <asp:PostBackTrigger ControlID="BtnEnviarSoliModificada" />
                        </triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="MasInformacionColaborador" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 600px; top: 320px; left: 50%; transform: translate(-50%, -50%);">
                <div class="modal-header">


                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="modal-title">
                                <asp:Label ID="LbMasInformacionColaboraador" runat="server" Text="" Style="margin-left: auto; margin-right: auto"></asp:Label></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label ID="LbMensaje1" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: justify;"></asp:Label>
                                <asp:Label ID="LbMensaje2" runat="server" Text="" Class="col-sm-12" Style="color: black; text-align: center;"></asp:Label>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
