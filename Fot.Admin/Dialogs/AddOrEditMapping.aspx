<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMapping.aspx.cs" Inherits="Fot.Admin.Dialogs.AddOrEditMapping" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map Location</title>
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
                    <td style="width: 150px;">Location</td>
                    <td>
                        <asp:DropDownList ID="listLocation" runat="server" DataSourceID="LocationDatasource" DataTextField="LocationName" DataValueField="LocationId" Width="200px" CssClass="validate[required]" data-prompt-position="bottomRight:-100,10">
                        </asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Map To</td>
                    <td>
                        <asp:DropDownList ID="listMapTo" runat="server" DataSourceID="MappingDataSource" DataTextField="LocationName" DataValueField="LocationId" Width="200px" CssClass="validate[required]" data-prompt-position="bottomRight:-100,10">
                        </asp:DropDownList>
           
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add Mapping" OnClick="bttnAdd_Click" />
                        <asp:Button ID="bttnUpdate" runat="server" Text="Update Mapping" Visible="False" OnClick="bttnUpdate_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>

        <asp:ObjectDataSource ID="MappingDataSource" runat="server" SelectMethod="GetPossibleParentLocations" TypeName="Fot.Admin.Services.LocationService"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="LocationDatasource" runat="server" SelectMethod="GetMappableLocations" TypeName="Fot.Admin.Services.LocationService"></asp:ObjectDataSource>

    </form>
</body>
</html>
