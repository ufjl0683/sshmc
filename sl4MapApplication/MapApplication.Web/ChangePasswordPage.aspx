<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordPage.aspx.cs" Inherits="MapApplication.Web.ChangePasswordPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="text-align: center; height=100%  ">
    <form id="form1" runat="server" style="text-align: center">
   
    
        <div style=" position:absolute; width:485px;height:230px; ;border-width: 1px; border-style: solid; top:0; bottom:0; left:0; right:0; margin:auto;
"   >
            <asp:Label ID="Label1" runat="server" Text="忘記密碼?" BackColor="#5D7B9D" 
                Font-Bold="True" ForeColor="White" Width="485px"></asp:Label>
            <br />
            &nbsp;
            <asp:Label ID="Label2" runat="server" Text="請輸入您的帳號、姓名及Email，以便查詢密碼。" 
                Font-Italic="False" Font-Bold="True"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" 
                Text="帳號:"></asp:Label>
            &nbsp;
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" Text="姓名:"></asp:Label>
            &nbsp;
            <asp:TextBox ID="TextBox2" runat="server" BorderStyle="Inset"></asp:TextBox>
            <asp:CompareValidator ID="CompareValidator1" runat="server" 
             ErrorMessage="X"
             ControlToCompare="TextBox1"
             ControlToValidate="TextBox2" Visible="False" ></asp:CompareValidator>
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" Text="e-mail:"></asp:Label>
            &nbsp;
            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="email_chk" runat="server" 
            ErrorMessage="X"
            ControlToValidate="TextBox3" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="True"></asp:RegularExpressionValidator>
          <br />
            <br />
            
            
            
            <asp:Label ID="Label5" runat="server" Text="輸入錯誤，請重新輸入。" Visible=False 
                ForeColor="Red"></asp:Label>

            <br />
            <asp:Button ID="Button1" runat="server" Text="送出" onclick="Button1_Click1" />

        &nbsp;&nbsp;&nbsp;
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/sshmc/Default.aspx">回登入頁面</asp:HyperLink>
            <br />

        </div>
        
        
        <br />
    
    </form>
</body>
</html>
