<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="MailServer.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mailinfo</title>
        <style type="text/css"></style>

    <script language="C#" runat="server">
      void LinkButton_Click(Object sender, EventArgs e) 
      {
         Label2.Text="Beklager noget gik galt. Prøv venligst igen senere. ";
      }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <asp:Image ID="Image2" runat="server" Height="50px" ImageUrl="~/Pictures/Logo.png" Width="70px" />
            <asp:Label ID="Label1" runat="server" Font-Size="XX-Large" ForeColor="#CC3300" Text="Label">Gridview</asp:Label>
            <br />
         <br />
            <asp:GridView ID="BrugerInfo" runat="server" AutoGenerateColumns="False"  
                EmptyDataText="Empty" BackColor="White" BorderColor="#336666" 
                BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
                GridLines="Horizontal" CurrentSortField="ID" CurrentSortDirection="ASC" AllowSorting="True">
                <Headerstyle backcolor="Darkred" forecolor="White" Font-Bold="True"/>
                <Columns>
                    <asp:BoundField DataField="navn" HeaderText="Nane"/>
                    <asp:BoundField DataField="mail" HeaderText="Mail"/>
                    <asp:BoundField DataField="dato" HeaderText="Dato"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Select" Text="Vis" runat="server" CommandArgument='<%# Eval("ID")%>' OnClick="LinkButton_Click"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#333333" />
                <SelectedRowStyle BackColor="Darkred" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <asp:Label ID="Label2" runat="server"></asp:Label>
        </div>
    </form>
