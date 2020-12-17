<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCampaign.aspx.cs" Inherits="Fot.Admin.Client.Dialogs.AddCampaign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Campaign</title>

    <link href="../../css/dialog.css" rel="stylesheet" />
    <style type="text/css">
        td
        {
            padding-top: 5px;
            padding-bottom: 5px;
        }
    </style>

    
    <script src="../../Scripts/jquery-1.8.2.min.js"></script>


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


        $(document).ready(function() {


            $("#listCampaignType").change(function() {

                ShowHideDates();

            });
            
            ShowHideDates();

        });
        

        function ShowHideDates() {
            
            var str = $("#listCampaignType option:selected").val();

            if (str === "0") {

                $("#trStartDate").hide();
                $("#trEndDate").hide();
               


            }
            else if (str === "1") {

                $("#trStartDate").show();
                $("#trEndDate").show();
                
            }
        }
        

    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <div style="padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Campaign Name</td>
                    <td>
                        <asp:TextBox ID="txtCampaignName" runat="server" Width="400px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCampaignName" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Assessment Bundle</td>
                    <td>
                        <asp:DropDownList ID="listAssessmentBundles" runat="server" DataSourceID="BundleDataSource" DataTextField="Name" DataValueField="BundleId">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="listAssessmentBundles" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Campaign Type</td>
                    <td>
                        <asp:DropDownList ID="listCampaignType" runat="server" ClientIDMode="Static">
                            <asp:ListItem Value="0">Proctored</asp:ListItem>
                            <asp:ListItem Value="1">Unproctored</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trStartDate" style="display: none">
                    <td style="width: 150px;">Start Date</td>
                    <td>
                        <telerik:RadDateTimePicker ID="txtStartDate" runat="server" Skin="Windows7" Culture="en-US" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." WrapperTableSummary="Table holding date picker control for selection of dates."  TimeView-Interval="00:30:00">
                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

                           <DateInput DisplayDateFormat="dd-MMM-yyyy hh:mm tt" DateFormat="dd-MMM-yyyy hh:mm tt" LabelWidth="40%"></DateInput>

                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                        </telerik:RadDateTimePicker>
                    </td>
                </tr >
                <tr id="trEndDate" style="display: none">
                    <td style="width: 150px;">End Date</td>
                    <td>
                        <telerik:RadDateTimePicker ID="txtEndDate" runat="server" Skin="Windows7" Culture="en-US" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." WrapperTableSummary="Table holding date picker control for selection of dates."  TimeView-Interval="00:30:00">
                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" Skin="Windows7"></Calendar>

                           <DateInput DisplayDateFormat="dd-MMM-yyyy hh:mm tt" DateFormat="dd-MMM-yyyy hh:mm tt" LabelWidth="40%"></DateInput>

                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                        </telerik:RadDateTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Add" OnClick="bttnAdd_Click" />
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

        
        <asp:ObjectDataSource ID="PartnerDataSource" runat="server" SelectMethod="GetNonSelfManagedPartners" TypeName="Fot.Admin.Services.PartnerService">
        </asp:ObjectDataSource>


        <asp:ObjectDataSource ID="BundleDataSource" runat="server"  SelectMethod="GetBundles"  TypeName="Fot.Admin.Services.AssessmentBundleService">
            <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>





    </form>
</body>
</html>
