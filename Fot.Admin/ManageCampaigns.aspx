<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCampaigns.aspx.cs" Inherits="Fot.Admin.ManageCampaigns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            $("#bttnAddCampaign").click(function () {

                AddCampaign();
                return false;
            });


        });

        function AddCampaign() {
            var oWnd = radopen("Dialogs/AddCampaign.aspx", "RadWindow1");
            oWnd.center();

        }



    


        function refreshGrid() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                radMgr.ajaxRequest("Rebind");
            }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Campaigns</h1>
    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 0px; margin-top: 30px;">
        <button id="bttnAddCampaign">Add Campaign</button>
        
        <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">
            <tr>
                <td style="width: 200px; padding-left: 3px;">Partner</td>
                <td>
                    <asp:DropDownList ID="listPartners" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="PartnerDataSource" DataTextField="PartnerName" DataValueField="PartnerId" OnSelectedIndexChanged="listPartners_SelectedIndexChanged">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
            </tr>
        
            <tr>
                <td style="width: 200px; padding-left: 3px;">Search By Campaign Name</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="335px" style="margin-right: 5px;"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search"  style="margin-bottom: 1px" OnClick="bttnSearch_Click"/>
                </td>
               
            </tr>
        
        </table>
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CampaignDataSource" PageSize="20">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="CampaignDataSource" DataKeyNames="CampaignId">
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>

                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>

                <Columns>
                    <telerik:GridBoundColumn DataField="CampaignId" DataType="System.Int32" Display="False" FilterControlAltText="Filter CampaignId column" HeaderText="CampaignId" SortExpression="CampaignId" UniqueName="CampaignId">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CampaignName" FilterControlAltText="Filter CampaignName column" HeaderText="Campaign" SortExpression="CampaignName" UniqueName="CampaignName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PartnerName" FilterControlAltText="Filter PartnerName column" HeaderText="Partner" SortExpression="PartnerName" UniqueName="PartnerName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CampaignType" FilterControlAltText="Filter column2 column" HeaderText="Campaign Type" UniqueName="column2">
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CandidateCount" DataFormatString="{0:#,##0}" DataType="System.Int32" FilterControlAltText="Filter CandidateCount column" HeaderText="Candidates" SortExpression="CandidateCount" UniqueName="CandidateCount">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridBoundColumn>
                         <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="CampaignId" DataNavigateUrlFormatString="CampaignDetails.aspx?id={0}" FilterControlAltText="Filter column1 column" Text="Manage Campaign" UniqueName="column1">
                             <HeaderStyle Width="120px" />
                             <ItemStyle Width="120px" />
                    </telerik:GridHyperLinkColumn>
                         <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this campaign?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridButtonColumn>
                </Columns>

                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <PagerStyle PageSizes="20,30,40,50,100" PageButtonCount="15" />

            <FilterMenu EnableImageSpries="False"></FilterMenu>
        </telerik:RadGrid>
    </div>
    
    <div>
        <asp:ObjectDataSource ID="CampaignDataSource" runat="server" DeleteMethod="Delete" SelectMethod="GetCampaigns" TypeName="Fot.Admin.Services.CampaignService" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="CampaignId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="listPartners" Name="PartnerId" PropertyName="SelectedValue" Type="Int32" />
                 <asp:ControlParameter ControlID="txtSearch" Name="searchTerm" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>

        
        <asp:ObjectDataSource ID="PartnerDataSource" runat="server" SelectMethod="GetNonSelfManagedPartners" TypeName="Fot.Admin.Services.PartnerService">
        </asp:ObjectDataSource>

    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="450px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="listPartners">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    </div>
</asp:Content>
