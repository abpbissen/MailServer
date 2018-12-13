<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MailServer.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtUser" placeholder="User Name" runat="server"></asp:TextBox>

            <asp:TextBox ID="txtPass" placeholder="Password" TextMode="Password" runat="server"></asp:TextBox>
            
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />

        </div>
    </form>
    
</body>
</html>
