<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MailServer._Default" Async ="true" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 <link href="~/App_Start/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            margin-right: 0px;
        }
        </style>
</head>
<body>    
    <form id="form1" runat="server">
        
        <div id="container"> <!-- wrapper start -->
            <div id="header">
                <asp:Label ID="Label1" runat="server" Font-Size="XX-Large" ForeColor="#CC3300" Text="Label">Mail Server</asp:Label>
                
            </div>
            <h1 id="sprog">Sprog:</h1>
            <br />
                      

            <div class="txtBoxes"> <!-- left 1 start -->
                
                <asp:TextBox ID="TextBox3" runat="server" placeholder="  example@mail.com"></asp:TextBox>
                <br />
                
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" placeholder="  ********"></asp:TextBox>
                <br />
                
                <asp:TextBox ID="TextBox4" runat="server" placeholder="  modtager@mail.com"></asp:TextBox>
            </div> <!-- left end -->

            <div class="infoMail">
                <p>Afsender</p>
                <p>Password</p>
                <p>Modtager</p>
            </div>
           
            <div id="singleMailInfo">
                <p>Text</p>
            </div>

            
                               
           
                
                <br /><br />
            
            <br /><br /><br /><br /><br /><br /><br />

            
                
                <textarea id="TextArea1" runat="server" class="auto-style1" name="S1" cols="20" rows="1" placeholder="Meddelse."></textarea><br />
              
            <div class="xml">
                <p>XML</p>
            </div> 
            
            <asp:CheckBox ID="CheckBox1" runat="server" />
           
               
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Send Mail" />
              
                
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" CausesValidation="True">
                    <asp:ListItem>Dansk</asp:ListItem>
                    <asp:ListItem>English</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
            <br />
                <br />
            <br />
                <br />
            <br />
                <br />
            <br />
                <br />
            <br />
                <br />
            <br />
                <br />
            <br />
                <br />
            <br />
           

            <div class="horizontalLine">
                <p>Login or create user</p>
            </div>
                
                  
               
            
            
                <asp:TextBox ID="TextBox7" runat="server" placeholder="Username"></asp:TextBox>

              
                <asp:TextBox ID="TextBox8" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                        <br />
                <br />
                <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="Mail login" />
                <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Chat Login" />
                  
               
            
            
                <br />
            <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                <br />
            <br />
                <br />
                <br />
            <br />
                <br />
                <br />
             <br />
                <br />
            <br />
                <br />
                <br />
            <br />
                <br />
                <br /> <br />
                <br />
            
                                <br /><br />
                
                <br />
                <br />


              
                <asp:TextBox ID="TextBox5" runat="server" placeholder="Username"></asp:TextBox>

                <asp:TextBox ID="TextBox6" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox> <br/>
                <asp:TextBox ID="TextBox9" runat="server" placeholder="example@gmail.com"></asp:TextBox> 
            <asp:ImageButton ID="ImageButton1" runat="server" CssClass="auto-style1" Height="74px" ImageUrl="~/Pictures/Logo.png" OnClick="ImageButton1_Click" Width="96px" CausesValidation="False" />
            <br/>
                <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Create" />
              

                <asp:GridView ID="GridView1" runat="server" BorderColor="DarkRed" BorderStyle="Double" BorderWidth="3px" CellPadding="4">
                    <HeaderStyle BackColor="DarkRed" ForeColor="White" Font-Bold="true" />
                    <FooterStyle BackColor="DarkRed" ForeColor="White" Font-Bold="false" />
                </asp:GridView>
                <br />
                <br />
                <br />
            <asp:Label ID="OpretTekst" runat="server" Text="Create A User"></asp:Label>
            <asp:Label ID="lblChatServerMessages" runat="server" Text=""></asp:Label><!-- label for ChatServer error messages*/ -->
            <asp:Label ID="lblSuccesChatServer" runat="server" Text=""></asp:Label> <!-- label for ChatServer Success messages*/ -->
            <asp:Label ID="lblMailServer" runat="server" Text=""></asp:Label> <!-- label for mail Server error messages*/ -->
            <asp:Label ID="lblSprog" runat="server" Text="fdfsgdfgsd"></asp:Label>
            
        </div> <!-- wrapper start -->
        
        
    </form>
</body>
</html>