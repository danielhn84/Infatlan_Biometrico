﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="BiometricoWeb.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <title>Inflatlan | Login</title>
    <link rel="stylesheet" href="/vendors/mdi/css/materialdesignicons.min.css">
    <link rel="stylesheet" href="/vendors/base/vendor.bundle.base.css">
    <link rel="stylesheet" href="/css/style.css">
    <link rel="shortcut icon" href="/images/logo_mini.png" />
</head>
<body>
    <div class="container-scroller">
        <div class="container-fluid page-body-wrapper full-page-wrapper">
            <div class="content-wrapper d-flex align-items-stretch auth auth-img-bg">
                <div class="row flex-grow">

                    <div class="col-lg-12 d-flex align-items-center justify-content-start login-half-bg">
                        <div class="card card-rounded align-baseline" style="width: 400px; opacity:1; margin-left:100px">
                            <div class="card-header">
                                <div class="text-center">
                                    <img src="/images/logo.png" alt="logo"/>
                                </div>
                            </div>

                            <div class="card-body">
                                <div class="text-left p-3">
                                    <h4>Bienvenidos | Talento Humano</h4>
                                    <h6 class="font-weight-light">Ingresa tus credenciales.</h6>
                                    <br />
                                    <form id="form1" runat="server">
                                        <div class="form-group">
                                            <label for="exampleInputEmail">Usuario</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend bg-transparent">
                                                    <span class="input-group-text bg-transparent border-right-0">
                                                        <i class="mdi mdi-account-outline text-primary"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TxUsername" class="form-control form-control-lg border-left-0" placeholder="Username"  runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="exampleInputPassword">Password</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend bg-transparent">
                                                    <span class="input-group-text bg-transparent border-right-0">
                                                        <i class="mdi mdi-lock-outline text-primary"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="TxPassword" TextMode="Password" class="form-control form-control-lg border-left-0" placeholder="Password"  runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="my-2 d-flex justify-content-between align-items-center">
                                            <%--<div class="form-check">
                                                <label class="form-check-label text-muted">
                                                    <input type="checkbox" class="form-check-input" id="CBSession" runat="server">
                                                    Mantener la Session
                   
                                                </label>
                                            </div>
                                            <a href="#" class="auth-link text-black">Olvidaste tu password?</a>--%>
                                        </div>
                                        <div class="my-3">
                                            <asp:Button ID="BtnLogin" class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" runat="server" Text="Entrar" OnClick="BtnLogin_Click" />                              
                                        </div>

                                        <div class="my-2 d-flex justify-content-center align-center" style="color:indianred;">
                                            <asp:Label ID="LbMensaje" runat="server" Text=""></asp:Label>
                                        </div>

                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                  
                </div>
            </div>
        </div>
    </div>


    <script src="/vendors/base/vendor.bundle.base.js"></script>
    <script src="/js/off-canvas.js"></script>
    <script src="/js/hoverable-collapse.js"></script>
    <script src="/js/template.js"></script>
</body>
</html>