<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailBruger.aspx.cs" Inherits="MailServer.MailBruger" Async ="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="~/App_Start/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="container"> <!-- container start -->
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:Label ID="lblChatBoxHilsen" runat="server"></asp:Label>
            <br />
            Chat<br />

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
         <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>
       
           <asp:Timer ID="Timer2" runat="server" OnTick="Timer1_Tick" Interval="100"></asp:Timer>
         
             <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"  CellPadding="4" ForeColor="Black" GridLines="Horizontal" OnRowDataBound="GridViewLimit">
                 
                

                 <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                 <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                 <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                 <SortedAscendingCellStyle BackColor="#F7F7F7" />
                 <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                 <SortedDescendingCellStyle BackColor="#E5E5E5" />
                 <SortedDescendingHeaderStyle BackColor="#242121" />

             </asp:GridView>
       

         <br />
         <br />
 </ContentTemplate>
</asp:UpdatePanel>

                        
            
            <asp:TextBox ID="chatBox" runat="server" placeholder="Enter your text"></asp:TextBox>
            <br />
            <asp:Button ID="ButtonChat" runat="server" OnClick="Button1_Click" Text="Send" />
            <br />
            <br />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Delete" />
            <br />
            <br />
            <br />

            

            <br />
         <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">

             <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
             <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
             <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
             <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F7F7F7" />
             <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
             <SortedDescendingCellStyle BackColor="#E5E5E5" />
             <SortedDescendingHeaderStyle BackColor="#242121" />

         </asp:GridView>
            <br />

            <br />
            
        </div>
    </form>
</body>
</html>
