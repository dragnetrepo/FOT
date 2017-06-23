<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCandidate.aspx.cs" Inherits="Fot.Admin.Dialogs.AddCandidate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Candidate</title>

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
                    <td style="width: 150px;">Client Unique ID</td>
                    <td>
                        <asp:TextBox ID="txtClientUniqueId" runat="server" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Candidate Email</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Candidate Password</td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">First Name</td>
                    <td>
                        <asp:TextBox ID="txtFname" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFname" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Last Name</td>
                    <td>
                        <asp:TextBox ID="txtLname" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLname" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Mobile Number</td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMobile" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Location</td>
                    <td>
                        <asp:DropDownList ID="listLocations" runat="server" DataSourceID="LocationDatasource" DataTextField="LocationName" DataValueField="LocationId" Width="310px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="listLocations" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add To Campaign" OnClick="bttnAdd_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                </tr>
                </table>
        </div>
        <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
        
          <asp:ObjectDataSource ID="LocationDatasource" runat="server" SelectMethod="GetLocations" TypeName="Fot.Admin.Services.LocationService">
              <SelectParameters>
                  <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                  <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
              </SelectParameters>
          </asp:ObjectDataSource>
        
    </form>
</body>
</html>
