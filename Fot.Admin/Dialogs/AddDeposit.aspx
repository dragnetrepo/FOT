<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDeposit.aspx.cs" Inherits="Fot.Admin.Dialogs.AddDeposit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Deposit</title>

    <link href="../css/dialog.css" rel="stylesheet" />
    <style type="text/css">
        td
        {
            padding-top: 5px;
            padding-bottom: 5px;
        }
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
        <div style="padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Deposit Amount</td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" Width="80px" CssClass="validate[required,custom[number]]" data-errormessage-value-missing="Amount is required!" data-errormessage="Enter a valid value for amount" data-prompt-position="bottomRight"></asp:TextBox>
      
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px; vertical-align: top;">Payment Reference</td>
                    <td>
                        <asp:TextBox ID="txtReference" runat="server" Height="40px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add Deposit" OnClick="bttnAdd_Click" />
                        <asp:HiddenField ID="hidPartnerId" runat="server" />
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
