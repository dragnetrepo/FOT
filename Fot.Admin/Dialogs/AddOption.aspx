<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOption.aspx.cs" Inherits="Fot.Admin.Dialogs.AddOption" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title>Add Option</title>

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
                    <td style="width: 100px;">Option Type</td>
                    <td>
                        <asp:RadioButtonList ID="listOptionType" runat="server" AutoPostBack="True" CellPadding="1" CellSpacing="4" RepeatDirection="Horizontal" OnSelectedIndexChanged="listOptionType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="1">Text Option</asp:ListItem>
                            <asp:ListItem Value="2">Image Option</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trText" runat="server">
                    <td style="width: 150px; vertical-align: top;">Option Text</td>
                    <td>
                        <asp:TextBox ID="txtOptionText" runat="server" Width="450px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOptionText" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Correct Option?</td>
                    <td>
                        <asp:CheckBox ID="chkCorrect" runat="server" Text="Set as correct option" />
                    </td>
                </tr>
                <tr id="trImage" runat="server" Visible="False">
                    <td style="width: 150px;">Option Image</td>
                    <td>
                        <asp:FileUpload ID="fileImage" runat="server" />
                    </td>
                </tr>
                <tr id="trImage2" runat="server" Visible="False">
                    <td style="width: 150px;">&nbsp;</td>
                    <td>
                        <strong style="color: #CC0000;">Image must have a maximum width of 700px and a maximum height of 90px</strong></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add Option" OnClick="bttnAdd_Click" />
                        <asp:HiddenField ID="hidQId" runat="server" />
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
