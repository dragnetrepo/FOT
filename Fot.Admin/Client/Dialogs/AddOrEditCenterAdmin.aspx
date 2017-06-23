<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditCenterAdmin.aspx.cs" Inherits="Fot.Admin.Client.Dialogs.AddOrEditCenterAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Center Admin</title>

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
                    <td style="width: 150px;">Email</td>
                    <td >
                        <asp:TextBox ID="txtEmail" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Password</td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">First Name</td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Last Name</td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLastName" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Mobile Number</td>
                    <td>
                        <asp:TextBox ID="txtMobileNumber" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMobileNumber" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Center</td>
                    <td>
                        <asp:DropDownList ID="listCenters" runat="server" DataSourceID="CenterDataSource" DataTextField="CenterName" DataValueField="CenterId">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="listCenters" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Status</td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Checked="True" Text="Active" />
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
        
        <asp:ObjectDataSource ID="CenterDataSource" runat="server"  SelectMethod="GetCenters"  TypeName="Fot.Admin.Services.PartnerCenterService">
                  <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
    </form>
</body>
</html>
