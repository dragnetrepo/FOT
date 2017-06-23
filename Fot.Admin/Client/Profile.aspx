<%@ Page Title="" Language="C#" MasterPageFile="~/Client/PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Fot.Admin.Client.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Partner Profile / Wallet Details</h1>

    <div style="padding: 10px; border: 1px solid #f0f0f0; min-height: 80px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 14px;">
        <div style="width: 100%; margin: 10px; float: left; clear: both">
            <div style="float: left; padding-right: 10px; width: 120px;">
                <strong>Partner Name:</strong>
            </div>
            <div style="float: left">
                <asp:Literal ID="lblPartnerName" runat="server"></asp:Literal>
            </div>
        </div>
        <div style="width: 100%; margin: 10px; float: left; clear: both;">
            <div style="float: left; padding-right: 10px; width: 120px;">
                <strong>Wallet Balance:</strong>
            </div>
            <div style="float: left">
                <asp:Literal ID="lblBalance" runat="server"></asp:Literal>
            </div>
        </div>

        <asp:HiddenField ID="hidId" runat="server" />

    </div>

    <div style="padding: 0px; margin-top: 50px;">
        <fieldset style="width: 100%; border: 1px solid #ccc;">
            <legend style="font-size: 14px; font-weight: bold; padding-left: 10px;">Wallet Deposit (Credit) Entries</legend>

            <div style="padding: 10px;">

                <telerik:RadGrid ID="GridFeedback" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" AllowAutomaticDeletes="True" AllowPaging="True" DataSourceID="PartnerWalletDataSource">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView DataSourceID="PartnerWalletDataSource">
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
                            <telerik:GridBoundColumn DataField="Amount" DataFormatString="{0:#,##0.00}" DataType="System.Decimal" FilterControlAltText="Filter Amount column" HeaderText="Amount" SortExpression="Amount" UniqueName="Amount">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Reference" FilterControlAltText="Filter Reference column" HeaderText="Reference" SortExpression="Reference" UniqueName="Reference">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EntryDate" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime" FilterControlAltText="Filter EntryDate column" HeaderText="Entry Date" SortExpression="EntryDate" UniqueName="EntryDate">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" />
                            </telerik:GridBoundColumn>
                        </Columns>

                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        </EditFormSettings>
                    </MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

                    <FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>

           <asp:ObjectDataSource ID="PartnerWalletDataSource" runat="server" DeleteMethod="Delete" SelectMethod="GetWalletEntries" TypeName="Fot.Admin.Services.PartnerWalletEntryService">
               <DeleteParameters>
                   <asp:Parameter Name="EntryId" Type="Int32" />
               </DeleteParameters>
               <SelectParameters>
                   <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
               </SelectParameters>
           </asp:ObjectDataSource>

            </div>

        </fieldset>

    </div>
    
    
    
      <div style="padding: 0px; margin-top: 50px;">
        <fieldset style="width: 100%; border: 1px solid #ccc;">
            <legend style="font-size: 14px; font-weight: bold; padding-left: 10px;">Wallet Debit Entries</legend>

            <div style="padding: 10px;">

                <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" AllowAutomaticDeletes="True" AllowPaging="True" DataSourceID="DebitDataSource">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView DataSourceID="DebitDataSource">
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
                            <telerik:GridBoundColumn DataField="Amount" DataFormatString="{0:#,##0.00}" DataType="System.Decimal" FilterControlAltText="Filter Amount column" HeaderText="Amount" SortExpression="Amount" UniqueName="Amount">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CampaignName" FilterControlAltText="Filter CampaignName column" HeaderText="Campaign" SortExpression="CampaignName" UniqueName="CampaignName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CandidateName" FilterControlAltText="Filter CandidateName column" HeaderText="Candidate" SortExpression="CandidateName" UniqueName="CandidateName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DebitDate" DataFormatString="{0:dd-MMM-yyyy}" DataType="System.DateTime" FilterControlAltText="Filter DebitDate column" HeaderText="Debit Date" SortExpression="DebitDate" UniqueName="DebitDate">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" />
                            </telerik:GridBoundColumn>
                        </Columns>

                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        </EditFormSettings>
                    </MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

                    <FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>





                <asp:ObjectDataSource ID="DebitDataSource" runat="server" SelectMethod="GetDebitEntries" TypeName="Fot.Admin.Services.PartnerWalletDebitService">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hidId" Name="PartnerId" PropertyName="Value" Type="Int32" />
                         <asp:Parameter DefaultValue="-1" Name="startRow" Type="Int32" />
                <asp:Parameter DefaultValue="-1" Name="maxRows" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>





            </div>



        </fieldset>

    </div>

</asp:Content>
