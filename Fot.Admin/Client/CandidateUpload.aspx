<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="CandidateUpload.aspx.cs" Inherits="Fot.Admin.Client.CandidateUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
        .auto-style1 {
            font-size: small;
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h1>Candidate Bulk Upload</h1>
    
    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Campaign:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblCampaignName" runat="server"></asp:Literal>
        </div>
        <div style="float: right">
            <asp:Button ID="bttnBackToCampaignCandidates" runat="server" Text="Return To Campaign Candidates" OnClick="bttnBackToCampaignCandidates_Click" CssClass="myButton" />
        </div>
    </div>

     <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
    
     <div style="padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 200px;">Excel File Column Format</td>
                    <td style="border-bottom: 1px solid #ccc;border-top: 1px solid #ccc">
                        <strong>ClientUniqueID&nbsp;&nbsp;&nbsp;&nbsp; Email&nbsp;&nbsp;&nbsp;&nbsp; Password&nbsp;&nbsp;&nbsp;&nbsp; Firstname&nbsp;&nbsp;&nbsp;&nbsp; Lastname&nbsp;&nbsp;&nbsp;&nbsp; Mobile&nbsp;&nbsp;&nbsp;&nbsp; Location</strong></td>
                </tr>
                <tr>
                    <td style="width: 150px;">&nbsp;</td>
                    <td class="auto-style1">
                        ClientUniqueID is optional but when provided is expected to be unique per campaign<br />
                        ClientUniqueID column should be in the excel file regardless of whether the values are present<br />
                        Email should be valid</td>
                </tr>
                <tr>
                    <td style="width: 150px;">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 150px;">Select Excel File</td>
                    <td>
                        <asp:FileUpload ID="fileUpload" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnAdd" runat="server" Text="Upload To Campaign" OnClick="bttnAdd_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                </tr>
                </table>
        </div>
    
    <div style="margin-top: 30px; padding: 10px;" id="divIssues" runat="server" Visible="False">
        <fieldset style="border: 1px solid #f0f0f0; padding: 10px; padding-bottom: 50px; padding-top: 30px;"><legend style="font-size: 18px; color: #666;">Issues Encountered</legend>
              <div style="margin-top: 10px;">
                <asp:Button ID="bttnDownloadIssuesList" runat="server" Text="Download Issues List" OnClick="bttnDownloadIssuesList_Click"  />
            </div>
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="IssuesDataSource" PageSize="20">
                <ClientSettings>
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
<MasterTableView DataSourceID="IssuesDataSource">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="RowNumber" DataType="System.Int32" FilterControlAltText="Filter RowNumber column" HeaderText="Row#" SortExpression="RowNumber" UniqueName="RowNumber">
        </telerik:GridBoundColumn>
         <telerik:GridBoundColumn DataField="ClientUniqueId" FilterControlAltText="Filter ClientUniqueId column" HeaderText="Client Unique ID" SortExpression="ClientUniqueId" UniqueName="ClientUniqueId">
        </telerik:GridBoundColumn>
          <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Password" FilterControlAltText="Filter Password column" HeaderText="Password" SortExpression="Password" UniqueName="Password">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Firstname" FilterControlAltText="Filter Firstname column" HeaderText="Firstname" SortExpression="Firstname" UniqueName="Firstname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Lastname" FilterControlAltText="Filter Lastname column" HeaderText="Lastname" SortExpression="Lastname" UniqueName="Lastname">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="MobileNo" SortExpression="MobileNo" UniqueName="MobileNo">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column" HeaderText="Location" SortExpression="Location" UniqueName="Location">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Issue" FilterControlAltText="Filter Issue column" HeaderText="Issue" SortExpression="Issue" UniqueName="Issue">
        </telerik:GridBoundColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

                <HeaderStyle Font-Bold="True" />

                <PagerStyle Mode="Slider" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
            </telerik:RadGrid>
            

        </fieldset>
        <asp:ObjectDataSource ID="IssuesDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="CountIssues" SelectMethod="GetIssues" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Client.CandidateUpload" EnablePaging="True"></asp:ObjectDataSource>
    </div>
       
</asp:Content>
