<%@ Page Title="Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Fot.Client.Results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Assessment Results</h1>


    <div style="padding: 0px;">


        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AllowAutomaticDeletes="True" AutoGenerateColumns="False" DataSourceID="BundleDataSource" OnItemCommand="RadGrid1_ItemCommand">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="BundleDataSource" DataKeyNames="EntryId">
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>

                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>

                <Columns>
                    <telerik:GridBoundColumn DataField="EntryId" DataType="System.Int32" Display="False" FilterControlAltText="Filter EntryId column" HeaderText="EntryId" SortExpression="EntryId" UniqueName="EntryId">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CampaignName" FilterControlAltText="Filter Name column" HeaderText="Campaign" SortExpression="CampaignName" UniqueName="CampaignName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AssessmentName" FilterControlAltText="Filter AssessmentName column" HeaderText="Assessment" SortExpression="AssessmentName" UniqueName="AssessmentName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TestDate" FilterControlAltText="Filter Name column" HeaderText="Test Date" SortExpression="TestDate" DataType="System.DateTime" UniqueName="TestDate" DataFormatString="{0:dd-MMM-yyyy}">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn>
                        <HeaderStyle Width="120px" />
                        <ItemStyle Width="120px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="bttnDownload" Text="Download Result"
                                CommandArgument='<%# Eval("EntryId") %>'
                                CommandName="Download" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>

                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <PagerStyle Mode="Slider" />

            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>


        <asp:ObjectDataSource ID="BundleDataSource" runat="server" SelectMethod="GetEntries" TypeName="Fot.Client.Results">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="CandidateId" SessionField="USERID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>


        <telerik:RadWindowManager
            ID="RadWindowManager1" runat="server" Height="500px" Skin="Windows7"
            Width="650px" Modal="True" VisibleStatusbar="False"
            Behaviors="Close, Move, Reload" DestroyOnClose="True">
        </telerik:RadWindowManager>


    </div>
</asp:Content>
