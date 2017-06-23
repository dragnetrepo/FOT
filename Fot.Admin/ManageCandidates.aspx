<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCandidates.aspx.cs" Inherits="Fot.Admin.ManageCandidates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
     <style type="text/css">
        
        .FloatRight {
            float: right;
        }

        input.myButton {
            margin-bottom: 1px;
            margin-left: 10px;
        }

    </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>All Candidates</h1>
    
    
    
    <div style="padding: 0px; margin-top: 20px;">
        
               
           <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px; margin-top: 20px;">
            <tr>
                <td style="width: 150px; padding-left: 3px;">Candidate Search</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="300px"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search" CssClass="myButton" OnClick="bttnSearch_Click" />
                </td>
               
            </tr>
        
            <tr>
                <td style="width: 150px; padding-left: 3px;">&nbsp;</td>
                <td class="quiet">
                    <em>Search using candidate Username, First name, Last name or Email</em></td>
               
            </tr>
        
        </table>
    </div>
    <div style="padding: 0px;margin-top: 10px;">
        

        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AllowAutomaticDeletes="True" AutoGenerateColumns="False" DataSourceID="CandidateDataSource" PageSize="25" AllowCustomPaging="True">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataKeyNames="CandidateId" DataSourceID="CandidateDataSource">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="CandidateId" DataType="System.Int32" Display="False" FilterControlAltText="Filter CandidateId column" HeaderText="CandidateId" SortExpression="CandidateId" UniqueName="CandidateId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column" HeaderText="Username" SortExpression="UserName" UniqueName="UserName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column" HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column" HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column" HeaderText="Email" SortExpression="Email" UniqueName="Email">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="MobileNo" FilterControlAltText="Filter MobileNo column" HeaderText="Mobile No" SortExpression="MobileNo" UniqueName="MobileNo">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column" HeaderText="Location" SortExpression="Location" UniqueName="Location">
        </telerik:GridBoundColumn>
                     <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="CandidateId" DataNavigateUrlFormatString="CandidateDetails.aspx?id={0}"  FilterControlAltText="Filter column1 column" UniqueName="column1" Text="View Details">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
        </telerik:GridHyperLinkColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this candidate?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridButtonColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

            <HeaderStyle Font-Bold="True" />

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
        

    </div>
    
    <div>
        
        <asp:ObjectDataSource ID="CandidateDataSource" runat="server" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetCandidates" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.CandidateService" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="CandidateId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSearch" Name="searchTerm" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>

    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="450px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="bttnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="CandidateDataSource" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="CandidateDataSource">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="CandidateDataSource" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    </div>
</asp:Content>
