<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditLocation.aspx.cs" Inherits="Fot.Admin.Dialogs.AddOrEditLocation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Location</title>
   <link href="../css/dialog.css" rel="stylesheet" />
    <style type="text/css">
        
          td {padding-top:5px;padding-bottom: 5px;}

    </style>
    
        <script src="../Scripts/jquery-1.8.2.min.js"></script>
     <script src="../js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="../js/jquery.validationEngine.js" type="text/javascript"></script>
    

    <link href="../css/validationEngine.jquery.css" rel="stylesheet" />
    
    

       <script type="text/javascript">


           $(document).ready(function () {

               $("#form1").validationEngine();
           });


    </script>

    <script type="text/javascript">

        function GetRadWindow() {
            var oWindow = null; if (window.radWindow)
                oWindow = window.radWindow; else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow; return oWindow;
        }

        function CallFunctionOnParentPage(fnName) {
            //  alert("Calling the function " + fnName + " defined on the parent page");
            var oWindow = GetRadWindow();
            if (oWindow.BrowserWindow[fnName] && typeof (oWindow.BrowserWindow[fnName]) == "function") {
                oWindow.BrowserWindow[fnName](oWindow);
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 10px; display: none;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Location</td>
                    <td>
                        <asp:TextBox ID="txtLocation" runat="server" Width="300px" CssClass="validate[required]" data-errormessage-value-missing="Location is required!" data-prompt-position="bottomRight:-100,10"></asp:TextBox>
                     
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add" OnClick="bttnAdd_Click" />
                        <asp:Button ID="bttnUpdate" runat="server" Text="Update" Visible="False" OnClick="bttnUpdate_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                </tr>
                </table>
        </div>
        <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
        
    </form>
</body>
</html>
