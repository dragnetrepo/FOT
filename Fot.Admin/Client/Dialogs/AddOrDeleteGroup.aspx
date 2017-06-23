<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrDeleteGroup.aspx.cs" Inherits="Fot.Admin.Client.Dialogs.AddOrDeleteGroup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manage Groups</title>

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
        
        

        <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
        
        

        <div style="padding: 10px;">
            <table style="width: 500;">
                <tr>
                    <td style="width: 150px;">Group Name</td>
                    <td>
                        <asp:TextBox ID="txtGroupName" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroupName" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add" OnClick="bttnAdd_Click" />
                    </td>
                </tr>
                </table>
        </div>
        <div style="padding: 10px;">
            
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" Width="600px" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="QuestionGroupDataSource" OnItemDeleted="RadGrid1_ItemDeleted" PageSize="5">
                <ClientSettings>
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
<MasterTableView DataKeyNames="GroupId" DataSourceID="QuestionGroupDataSource">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="GroupId" DataType="System.Int32" Display="False" FilterControlAltText="Filter GroupId column" HeaderText="GroupId" SortExpression="GroupId" UniqueName="GroupId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="GroupName" FilterControlAltText="Filter GroupName column" HeaderText="Group" SortExpression="GroupName" UniqueName="GroupName">
        </telerik:GridBoundColumn>
        <telerik:GridButtonColumn CommandName="Delete" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                <HeaderStyle Font-Bold="True" />

                <PagerStyle Mode="Slider" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
            </telerik:RadGrid>

                        <asp:HiddenField ID="hidAid" runat="server" />

            <asp:ObjectDataSource ID="QuestionGroupDataSource" runat="server" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetGroups" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.QuestionGroupService">
                <DeleteParameters>
                    <asp:Parameter Name="GroupId" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="hidAid" Name="AssessmentId" PropertyName="Value" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

        </div>
        
    </form>
</body>
</html>