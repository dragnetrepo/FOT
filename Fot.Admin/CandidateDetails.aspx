<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CandidateDetails.aspx.cs" Inherits="Fot.Admin.CandidateDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Candidate Details</h1>
    
    
      <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
    
        <div style="padding: 10px; margin: 10px; border: 1px solid #f0f0f0;">
        

            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Username</td>
                    <td>
                        <asp:Label ID="lblUsername" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                    <td rowspan="5" valign="top" style="width: 200px;">
                        <asp:Image ID="CandidatePicture" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">First Name</td>
                    <td>
                        <asp:Label ID="lblFname" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Last Name</td>
                    <td>
                        <asp:Label ID="lblLname" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Mobile Number</td>
                    <td>
                        <asp:Label ID="lblMobile" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Location</td>
                    <td>
                        <asp:DropDownList ID="listLocations" runat="server" AppendDataBoundItems="True" DataSourceID="LocationDataSoruce" DataTextField="LocationName" DataValueField="LocationId">
                            <asp:ListItem Value="">Select a location</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="listLocations" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnUpdate" runat="server" Text="Update" OnClick="bttnUpdate_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                </table>
        

    </div>
    
       <div style="margin-top: 30px; padding: 10px;">
        <fieldset style="border: 1px solid #f0f0f0; padding: 10px; padding-bottom: 50px; padding-top: 30px;"><legend style="font-size: 18px; color: #666;">Candidate Campaigns</legend>
            
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CandidateCampaignsDataSource" PageSize="20">
                <ClientSettings>
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
<MasterTableView DataSourceID="CandidateCampaignsDataSource">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="EntryId" DataType="System.Int32" FilterControlAltText="Filter EntryId column" HeaderText="EntryId" SortExpression="EntryId" UniqueName="EntryId" Display="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="CampaignName" FilterControlAltText="Filter CampaignName column" HeaderText="Campaign" SortExpression="CampaignName" UniqueName="CampaignName">
        </telerik:GridBoundColumn>
        <telerik:GridCheckBoxColumn DataField="Scheduled" DataType="System.Boolean" FilterControlAltText="Filter Scheduled column" HeaderText="Scheduled" SortExpression="Scheduled" UniqueName="Scheduled">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridCheckBoxColumn>
        <telerik:GridCheckBoxColumn DataField="Tested" DataType="System.Boolean" FilterControlAltText="Filter Tested column" HeaderText="Tested" SortExpression="Tested" UniqueName="Tested">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridCheckBoxColumn>
        <telerik:GridBoundColumn DataField="DateTested" FilterControlAltText="Filter DateTested column" HeaderText="Date Tested" SortExpression="DateTested" UniqueName="DateTested" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
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
        <asp:ObjectDataSource ID="CandidateCampaignsDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="CountCandidateCampaigns" SelectMethod="GetCandidateCampaigns" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.CandidateService" EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CandidateId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
           </asp:ObjectDataSource>
    </div>
    
    <div>
        
        

        <asp:ObjectDataSource ID="LocationDataSoruce" runat="server" SelectMethod="GetLocations" TypeName="Fot.Admin.Services.LocationService">
            <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        

    </div>
</asp:Content>
