<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="cargarArchivos.aspx.cs" Inherits="BiometricoWeb.pages.cargaConsolidado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/smart_wizard.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_circles.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_arrows.css" rel="stylesheet" type="text/css" />
    <link href="/css/smart_wizard_theme_dots.css" rel="stylesheet" type="text/css" />
    <link href="/css/GridStyle.css" rel="stylesheet" />
    <link href="/css/pager.css" rel="stylesheet" />
    <link href="/css/breadcrumb.css" rel="stylesheet" />
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
                        <h2>Cargar Información</h2>
                        <p class="mb-md-0">Recursos Humanos</p>
                    </div>
                </div>
                <div class="d-flex justify-content-between align-items-end flex-wrap">
                </div>
            </div>
        </div>
    </div>
    <div runat="server" visible="true">   
        <nav>
            <div class="nav nav-pills " id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="nav_cargar_tab" data-toggle="tab" href="#nav-Compensatorio" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-alarm-plus" > </i>Compensatorio</a>
                <a class="nav-item nav-link" id="nav_cargarPermisos_tab" data-toggle="tab" href="#nav-Permisos" role="tab" aria-controls="nav-profile" aria-selected="false"><i class="mdi mdi-door-open" > </i>Permisos</a>
            </div>
        </nav>
    </div>
    <div class="tab-content" id="nav-tabContent">
        <div class="tab-pane fade show active" id="nav-Compensatorio" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <div class="row">
                <div class="col-8 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Cargar Tiempo Compensatorio</h4>
                            <small>Ingrese la información en una plantilla de compensatorio.</small>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <div class="col-sm-9">

                                        <div class="form-horizontal form-label-left">
                                            <div class="form-group">
                                                <label class="control-label col-md-2 col-sm-2 col-xs-12" for="first-name"></label>
                                                <div class="col-md-3 col-sm-3 col-xs-12">
                                                    <asp:FileUpload runat="server" ID="FUCompensatorio" AllowMultiple="false" ClientIDMode="AutoID" />
                                                </div>
                                            </div>
                                            <div class="form-group col-md-3 col-sm-3 col-xs-12">
                                                <asp:Button ID="BtnSubirCompensatorio" class="btn btn-sm btn-info" runat="server" Text="Subir Cambios" OnClick="BtnSubirCompensatorio_Click" OnClientClick="return postbackButtonClick();" />
                                            </div>

                                            <div class="footer">
                                                <div class="stats">
                                                    <i class="fa fa-info"></i>
                                                    <asp:Label ID="LabelMensaje" runat="server" Text="Recuerda verificar el Excel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-4 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Plantilla</h4>
                            <small>Descargue la plantilla antes de subir la información</small>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <ul class="list-unstyled project_files">
                                        <li><a href="plantilla/PlantillaCompensatorio.xlsx"><i class="fa fa-file-excel-o"></i>PlantillaCompensatorio.xlsx</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="tab-pane fade" id="nav-Permisos" role="tabpanel" aria-labelledby="nav-cargar-tab">
            <br />
            <div class="row">
                <div class="col-8 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Cargar Permisos</h4>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <div class="col-sm-9">
                                        <div class="form-horizontal form-label-left">
                                            <div class="form-group">
                                                <label class="control-label col-md-2 col-sm-2 col-xs-12" for="first-name"></label>
                                                <div class="col-md-3 col-sm-3 col-xs-12">
                                                    <asp:FileUpload runat="server" ID="FUPermisos"  AllowMultiple="false" ClientIDMode="AutoID" />
                                                </div>
                                            </div>
                                            <div class="form-group col-md-3 col-sm-3 col-xs-12">
                                                <asp:Button ID="BtnSubirPermisos" class="btn btn-sm btn-info" runat="server" Text="Subir" OnClick="BtnSubirPermisos_Click" />
                                            </div>

                                            <div class="footer">
                                                <div class="stats">
                                                    <i class="fa fa-info"></i>
                                                    <asp:Label ID="LabelPermisos" runat="server" Text="Recuerda verificar el Excel"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-4 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Plantilla</h4>
                            <small>Descargue la plantilla antes de subir la información</small>
                            <div class="col-md-6">
                                <div class="form-group row">
                                    <ul class="list-unstyled project_files">
                                        <li><a href="plantilla/PlantillaPermisos.xlsx"><i class="fa fa-file-excel-o"></i>PlantillaPermisos.xlsx</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
