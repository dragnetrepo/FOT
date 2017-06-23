<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Fot.Lan.Admin.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Face Of Testing</title>
    <link href="~/css/homestyle.css" rel="stylesheet" type="text/css" />
</head>
<body>

<div id="outer_wrapper_inner" style="left: 0px; top: 0px">
<form id="Form1" runat="server">
  <div id="wrapper">
    <!--header -->
    <div id="header" style="height:80px"><img src="~/images/newlogo2.png" alt="" id="logo" runat="server" /> &nbsp; <img src="~/images/exam-text.png" alt="" id="logo2" runat="server" /></div>
    <!--Menu Area -->
    <!--content area -->
    <div id="content">
    
      <table cellpadding="5" cellspacing="5" width="100%" border="0">
      <tr> 
      <td style="width: 40%">
      
          &nbsp;</td>
      <td style="width: 420px;height:60px;" align="center">
      
          <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </td>
      <td style="width: 40%">
      
      &nbsp;</td>
      </tr>
      <tr ID="divLogin" runat="server">
      <td>
      
          &nbsp;</td>
      <td style="height:300px" valign="top">
      
     <div class="loginDiv">
     
         <table border="0" cellpadding="2" cellspacing="0" 
             style="width:400px;">
             <tr>
                 <td>
                     <h1 style="padding-bottom:20px;color:#003C60">Administrator Login</h1></td>
             </tr>
             <tr>
                 <td>
                     Username</td>
             </tr>
             <tr>
                 <td>
                     <asp:TextBox ID="txtUsername" runat="server" Width="400px"></asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td>
                     Password</td>
             </tr>
             <tr>
                 <td>
                     <asp:TextBox ID="txtPassword" runat="server" Width="400px" TextMode="Password"></asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td align="right">
                     <asp:Button ID="bttnLogin" runat="server" Text="Login" 
                         onclick="bttnLogin_Click" />
                 </td>
             </tr>
         </table>
     
     </div></td>
      <td>
      
      &nbsp;</td>
      </tr>
      </table>

    </div>
  </div>
  <!--footer -->
  
  <br class="clear" />
  </form>
</div>

</body>
</html>
