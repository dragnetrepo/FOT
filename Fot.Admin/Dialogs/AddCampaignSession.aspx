<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCampaignSession.aspx.cs" Inherits="Fot.Admin.Dialogs.AddCampaignSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
      <title>Add Campaign Session</title>

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
                <td >
                    <asp:DropDownList ID="listLocations" runat="server" style="margin-bottom: 3px;" AutoPostBack="True" DataSourceID="LocationDataSource" DataTextField="LocationName" DataValueField="LocationId" OnSelectedIndexChanged="listLocations_SelectedIndexChanged">
                      
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >Center</td>
                <td >
                    <asp:DropDownList ID="listCenters" runat="server" style="margin-bottom: 3px;" AutoPostBack="True" DataSourceID="CenterDataSource" DataTextField="CenterName" DataValueField="CenterId" OnSelectedIndexChanged="listCenters_SelectedIndexChanged" >
                     
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >Session</td>
                <td >
                    <asp:DropDownList ID="listSessions" runat="server" style="margin-bottom: 3px;" CssClass="validate[required]" DataSourceID="SessionDataSource" DataTextField="DisplayText" DataValueField="SessionId">
                      
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="bttnAddSession" runat="server" Text="Add Session" OnClick="bttnAddSession_Click" />
                </td>
               
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:ObjectDataSource ID="LocationDataSource" runat="server" SelectMethod="GetLocationsWithCenters" TypeName="Fot.Admin.Services.LocationService"></asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="CenterDataSource" runat="server" SelectMethod="GetCentersInLocation" TypeName="Fot.Admin.Services.CenterService">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="listLocations" Name="LocationId" PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="SessionDataSource" runat="server" SelectMethod="GetAvailableSessions" TypeName="Fot.Admin.Services.TestSessionService">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="listCenters" Name="CenterId" PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:HiddenField ID="hidId" runat="server" />
                    </td>
               
            </tr>
        </table>
    </div>
        <div style="height: 30px; padding: 5px; text-align: center;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
