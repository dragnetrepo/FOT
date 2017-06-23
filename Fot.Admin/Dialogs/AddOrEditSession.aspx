<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditSession.aspx.cs" Inherits="Fot.Admin.Dialogs.AddOrEditSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Session</title>
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

            $("#txtDate").addClass("validate[required]").attr("data-errormessage-value-missing", "Date is required!").attr("data-prompt-position", "centerRight");


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
                    <td style="width: 150px;">Center Name</td>
                    <td>
                        <asp:DropDownList ID="listCenters" runat="server" DataSourceID="CenterDataSource" DataTextField="CenterName" DataValueField="CenterId" CssClass="validate[required]" data-errormessage-value-missing="Center is required!" data-prompt-position="centerRight">
                        </asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Date</td>
                    <td>
                        <telerik:RadDatePicker ID="txtDate" runat="server" Culture="en-US" Skin="Windows7" >
                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

                            <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="40%"></DateInput>

                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Time</td>
                    <td>
                        <asp:DropDownList ID="listSessionTime" runat="server" Width="100px">
                            <asp:ListItem Value="1">6:00 AM</asp:ListItem>
                            <asp:ListItem Value="2">6:30 AM</asp:ListItem>
                            <asp:ListItem Value="3">7:00 AM</asp:ListItem>
                            <asp:ListItem Value="4">7:30 AM</asp:ListItem>
                            <asp:ListItem Value="5">8:00 AM</asp:ListItem>
                            <asp:ListItem Value="6">8:30 AM</asp:ListItem>
                            <asp:ListItem Selected="True" Value="7">9:00 AM</asp:ListItem>
                            <asp:ListItem Value="8">9:30 AM</asp:ListItem>
                            <asp:ListItem Value="9">10:00 AM</asp:ListItem>
                            <asp:ListItem Value="10">10:30 AM</asp:ListItem>
                            <asp:ListItem Value="11">11:00 AM</asp:ListItem>
                            <asp:ListItem Value="12">11:30 AM</asp:ListItem>
                            <asp:ListItem Value="13">12:00 PM</asp:ListItem>
                            <asp:ListItem Value="14">12:30 PM</asp:ListItem>
                            <asp:ListItem Value="15">1:00 PM</asp:ListItem>
                            <asp:ListItem Value="16">1:30 PM</asp:ListItem>
                            <asp:ListItem Value="17">2:00 PM</asp:ListItem>
                            <asp:ListItem Value="18">2:30 PM</asp:ListItem>
                            <asp:ListItem Value="19">3:00 PM</asp:ListItem>
                            <asp:ListItem Value="20">3:30 PM</asp:ListItem>
                            <asp:ListItem Value="21">4:00 PM</asp:ListItem>
                            <asp:ListItem Value="22">4:30 PM</asp:ListItem>
                            <asp:ListItem Value="23">5:00 PM</asp:ListItem>
                            <asp:ListItem Value="24">5:30 PM</asp:ListItem>
                            <asp:ListItem Value="25">6:00 PM</asp:ListItem>
                        </asp:DropDownList>
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

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:ObjectDataSource ID="CenterDataSource" runat="server" MaximumRowsParameterName="" SelectMethod="GetCenters" StartRowIndexParameterName="" TypeName="Fot.Admin.Services.CenterService">
            <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

    </form>
</body>
</html>
