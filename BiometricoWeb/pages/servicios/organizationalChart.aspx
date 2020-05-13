<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="organizationalChart.aspx.cs" Inherits="BiometricoWeb.pages.servicios.organizationalChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tree,
.tree ul,
.tree li {
    list-style: none;
    margin: 0;
    padding: 0;
    position: relative;
}

.tree {
    margin: 0 0 1em;
    text-align: center;
}

.tree,
.tree ul {
    display: table;
}

.tree ul {
    width: 100%;
}

.tree li {
    display: table-cell;
    padding: .5em 0;
    vertical-align: top;
}

.tree li:before {
    outline: solid 1px #666;
    content: "";
    left: 0;
    position: absolute;
    right: 0;
    top: 0;
}

.tree li:first-child:before {
    left: 50%;
}

.tree li:last-child:before {
    right: 50%;
}

.tree code,
.tree span {
    border: solid .1em #666;
    border-radius: .2em;
    display: inline-block;
    margin: 0 .2em .5em;
    padding: .2em .5em;
    position: relative;
}

.tree ul:before,
.tree code:before,
.tree span:before {
    outline: solid 1px #666;
    content: "";
    height: .5em;
    left: 50%;
    position: absolute;
}

.tree ul:before {
    top: -.5em;
}

.tree code:before,
.tree span:before {
    top: -.55em;
}

.tree>li {
    margin-top: 0;
}

.tree>li:before,
.tree>li:after,
.tree>li>code:before,
.tree>li>span:before {
    outline: none;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul class="tree">
        <li><span style="background-color: tomato">Gerencia General<p><small>Rigoberto Zelaya</small></p></span>
            <ul>
                <li><span style="background-color: lightblue">Talento Humano<p><small>Gladys Cruz</small></p></span></li>
                <li><span style="background-color: lightblue;">Soporte Técnico<p><small>Maynor Garcia</small></p></span>
                    <ul>
                        <li><span style="background-color: limegreen">Elvis Montoya</span>
                            <ul>
                                <li><span style="background-color: limegreen">Edwin Urrea</span>
                                    <ul>
                                        <li><span style="background-color: limegreen">Josue Garcia</span>
                                            <ul>
                                                <li><span style="background-color: limegreen">Juan Calderon</span>

                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <%--<ul>
                        <li><span style="background-color: limegreen">Elvis Montoya</span></li>
                        <li><span style="background-color: limegreen">Edwin Urrea</span></li>
                        <li><span style="background-color: limegreen">Josue Garcia</span></li>
                        <li><span style="background-color: limegreen">Juan Calderon</span></li>
                        <li><span style="background-color: limegreen">Serguei Hulse</span></li>
                        <li><span style="background-color: limegreen">Jose Membreño</span></li>
                        <li><span style="background-color: limegreen">Luis Mirón</span></li>
                    </ul>--%>
                </li>

                <li><span style="background-color: lightblue">Bodega <p><small>Dina Zepeda</small></p></span>
                    <ul>
                        <li><span>Carlos Moncada</span></li>
                        <li><span>Mariano Palma</span></li>
                        <li><span>Anibal Figueroa</span></li>
                        <li><span>Sandra Ortiz</span></li>
                        <li><span>Omar Ramos</span></li>
                    </ul>
                </li>
                <li><span style="background-color: lightblue">Wendy Ochoa</span> </li>
                <li><span style="background-color: lightblue">Gabriel Lopez</span> </li>
                <li><span style="background-color: lightblue">Ariela  Recarte</span> </li>
                <li><span style="background-color: lightblue">Regina Girón</span> </li>
                <li><span style="background-color: lightblue">Paulo Rivas</span> </li>
                <li><span style="background-color: lightblue"><b>Infraestructura TI</b><p><small>Daniel Henriquez</small></p></span> 


                </li>
            </ul>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
