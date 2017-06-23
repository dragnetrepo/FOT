<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="Fot.Admin.Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script type="text/javascript">


        function ViewFeedback(ID) {
            var oWnd = radopen("Dialogs/FeedbackMessage.aspx?id=" + ID, "RadWindow1");
            oWnd.center();

        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Candidate Feedback</h1>

    <div style="padding: 0px; margin-top: 30px;">
        <asp:DropDownList ID="listFeedbackTypes" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="FeedbackTypesDataSource" DataTextField="FeedbackReason" DataValueField="EntryId" OnSelectedIndexChanged="listFeedbackTypes_SelectedIndexChanged">
            <asp:ListItem Value="-1">All</asp:ListItem>
            <asp:ListItem Value="0">Other</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div style="padding: 0px; margin-top: 10px;">





        <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="CandidateFeedbackDataSource" AllowAutomaticDeletes="True" PageSize="20" AllowPaging="True">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="CandidateFeedbackDataSource">
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
                    <telerik:GridBoundColumn DataField="Subject" FilterControlAltText="Filter Subject column" HeaderText="Subject" SortExpression="Subject" UniqueName="Subject">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CandidateName" FilterControlAltText="Filter CandidateName column" HeaderText="Candidate" SortExpression="CandidateName" UniqueName="CandidateName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DateSent" DataType="System.DateTime" FilterControlAltText="Filter DateSent column" HeaderText="Date Sent" SortExpression="DateSent" UniqueName="DateSent" DataFormatString="{0:dd-MMM-yyyy}">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                        UniqueName="TemplateColumn">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <a href="#" onclick='ViewFeedback(<%# Eval("EntryId") %>); return false;'>View Feedback</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>

                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <PagerStyle CssClass="validate-skip" />

            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
        <asp:ObjectDataSource ID="CandidateFeedbackDataSource" runat="server" SelectMethod="GetCandidateFeedbacks" TypeName="Fot.Admin.Services.CandidateFeedbackService" EnablePaging="True" MaximumRowsParameterName="maxRows" SelectCountMethod="CountCandidateFeedbacks" StartRowIndexParameterName="startRow">
            <SelectParameters>
                <asp:ControlParameter ControlID="listFeedbackTypes" DefaultValue="0" Name="FeedbackTypeId" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="FeedbackTypesDataSource" runat="server" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetFeedbackTypes" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.CandidateFeedbackService" DeleteMethod="Delete">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>

        <telerik:RadWindowManager
            ID="RadWindowManager1" runat="server" Height="450px" Skin="Windows7"
            Width="650px" Modal="True" VisibleStatusbar="False"
            Behaviors="Close, Move, Reload" DestroyOnClose="True">
        </telerik:RadWindowManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="listFeedbackTypes">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </div>
</asp:Content>
