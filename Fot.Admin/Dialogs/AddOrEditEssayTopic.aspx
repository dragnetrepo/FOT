<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditEssayTopic.aspx.cs" Inherits="Fot.Admin.Dialogs.AddOrEditEssayTopic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Add Essay Topic</title>

    <link href="../css/dialog.css" rel="stylesheet" />
    <style type="text/css">
        
          td {padding-top:5px;padding-bottom: 5px;}

    </style>

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
        <div style="padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px; vertical-align: top;">Essay Topic</td>
                    <td>
                        <asp:TextBox ID="txtTopic" runat="server" Width="400px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTopic" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add" OnClick="bttnAdd_Click" />
                        <asp:Button ID="bttnUpdate" runat="server" Text="Update" Visible="False" OnClick="bttnUpdate_Click" />
                        <asp:HiddenField ID="hidTid" runat="server" />
                        <asp:HiddenField ID="hidAid" runat="server" />
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
