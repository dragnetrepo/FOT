<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditLevel.aspx.cs" Inherits="Fot.Admin.Client.Dialogs.AddOrEditLevel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Add Difficulty Level</title>

    <link href="../../css/dialog.css" rel="stylesheet" />
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
                    <td style="width: 150px;">Difficulty Level</td>
                    <td>
                        <asp:TextBox ID="txtDifficultyLevel" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDifficultyLevel" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Scale</td>
                    <td>
                        <asp:DropDownList ID="listScale" runat="server" Width="60px">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem Value="5"></asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                        </asp:DropDownList>
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
