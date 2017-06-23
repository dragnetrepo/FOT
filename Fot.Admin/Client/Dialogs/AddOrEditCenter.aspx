<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditCenter.aspx.cs" Inherits="Fot.Admin.Client.Dialogs.AddOrEditCenter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Center</title>
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
        <div style="padding: 10px; display: none">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Center Name</td>
                    <td>
                        <asp:TextBox ID="txtCenterName" runat="server" Width="400px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCenterName" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px; vertical-align: top;">Address</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="400px" Height="30px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Location</td>
                    <td>
                        <asp:DropDownList ID="listLocation" runat="server" DataSourceID="LocationDataSoruce" DataTextField="LocationName" DataValueField="LocationId" Width="200px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="listLocation" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Capacity</td>
                    <td>
                        <div style="float: left; margin-right: 50px;"><asp:TextBox ID="txtCapacity" runat="server" Width="60px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCapacity" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtCapacity" CssClass="Formerror" ErrorMessage="CompareValidator" Operator="DataTypeCheck" Type="Integer">*</asp:CompareValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Status</td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
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
        
        <asp:ObjectDataSource ID="LocationDataSoruce" runat="server" SelectMethod="GetPossibleParentLocations" TypeName="Fot.Admin.Services.LocationService"></asp:ObjectDataSource>
        
    </form>
</body>
</html>
