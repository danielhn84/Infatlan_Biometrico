<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="birthdayCalendar.aspx.cs" Inherits="BiometricoWeb.pages.birthdayCalendar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <asp:Calendar id="calendar1" runat="server">

           <OtherMonthDayStyle ForeColor="LightGray">
           </OtherMonthDayStyle>

           <TitleStyle BackColor="Blue"
                       ForeColor="White">
           </TitleStyle>

           <DayStyle BackColor="gray">
           </DayStyle>

           <SelectedDayStyle BackColor="LightGray"
                             Font-Bold="True">
           </SelectedDayStyle>

      </asp:Calendar>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
