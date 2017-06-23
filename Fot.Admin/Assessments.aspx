<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Assessments.aspx.cs" Inherits="Fot.Admin.Assessments" %>
<%@ Import Namespace="Fot.Admin.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        
        .FloatRight {
            float: right;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Assessments</h1>
      

    <div style="padding:0px; padding-bottom: 0px; margin-top: 20px">
       <asp:Button ID="bttnAdd" runat="server" Text="Add Assessment" OnClick="bttnAdd_Click" />
        <asp:Button ID="bttnImportAssessment" runat="server" Text="Import Assessment From File" CssClass="FloatRight" OnClick="bttnImportAssessment_Click" />

    </div>
    
    
     <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">
        
            <tr>
                <td style="width: 200px; padding-left: 3px;">Search By Assessment Name</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="335px" style="margin-right: 5px;"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search"  style="margin-bottom: 1px" OnClick="bttnSearch_Click" />
                </td>
               
                <td align="right">
                    <asp:Button ID="bttnDownload" runat="server" Text="Download Assessment Summaries"  style="margin-bottom: 1px" OnClick="bttnDownload_Click" />
                </td>
               
            </tr>
        
        </table>
    

       <div style=" padding:0px;">
        

           <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" Skin="Windows7" AllowAutomaticDeletes="True" AutoGenerateColumns="False" DataSourceID="BundleDataSource" ItemType="Fot.Admin.Models.AssessmentViewModel" PageSize="20" >
               <ClientSettings>
                   <Selecting AllowRowSelect="True" />
               </ClientSettings>
<MasterTableView DataSourceID="BundleDataSource" DataKeyNames="AssessmentId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="AssessmentId" DataType="System.Int32" Display="False" FilterControlAltText="Filter AssessmentId column" HeaderText="AssessmentId" SortExpression="AssessmentId" UniqueName="AssessmentId">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Assessment" SortExpression="Name" UniqueName="Name">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="AssessmentType" FilterControlAltText="Filter AssessmentType column" HeaderText="Type" SortExpression="AssessmentType" UniqueName="AssessmentType">
            <HeaderStyle Width="50px" />
            <ItemStyle Width="50px" />
        </telerik:GridBoundColumn>
                 
<%--        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn1 column" UniqueName="TemplateColumn1" HeaderText="Valid?" Visible="False">
              <HeaderStyle Width="60px" />
                   <ItemStyle Width="60px" />
            <ItemTemplate>
                 <img src='images/<%# Convert.ToBoolean(Eval("IsValid")) ? "accept.png" : "exclamation.png" %>' style="height: 14px; vertical-align: middle" /></div>
            </ItemTemplate>
        </telerik:GridTemplateColumn>--%>
        
         <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="AssessmentId" DataNavigateUrlFormatString="Summary.aspx?id={0}" FilterControlAltText="Filter summarycolumn column" Text="Summary" UniqueName="summarycolumn">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
        </telerik:GridHyperLinkColumn>
                 
               <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="AssessmentId" DataNavigateUrlFormatString="AddOrEditAssessment.aspx?id={0}" FilterControlAltText="Filter column column" Text="Edit / Update" UniqueName="column">
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridHyperLinkColumn>
        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" UniqueName="TemplateColumn">
            <HeaderStyle Width="100px" />
            <ItemStyle Width="100px" />
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="linkButton1" NavigateUrl='<%# ((AssessmentType)Eval("AssessmentType")) == AssessmentType.MCQ ? "Questions.aspx?id=" + Eval("AssessmentId") : "EssayTopics.aspx?id=" + Eval("AssessmentId") %>' Text='<%# ((AssessmentType)Eval("AssessmentType")) == AssessmentType.MCQ? "Questions ( " + Eval("QuestionCount") + " )" : "Essay Topics ( " + Eval("QuestionCount") + " )" %>'></asp:HyperLink>
            </ItemTemplate>
        </telerik:GridTemplateColumn>

         <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" UniqueName="TemplateColumn">
            <HeaderStyle Width="60px" />
            <ItemStyle Width="60px" />
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="linkDelete" CommandName="Delete" Text="Delete" Visible='<%# Convert.ToBoolean(Convert.ToInt32(Eval("QuestionCount")) == 0) %>' OnClientClick="return confirm('Delete this assessment?');"></asp:LinkButton>
                </ItemTemplate>
             </telerik:GridTemplateColumn>
        
  
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>

               <HeaderStyle Font-Bold="True" />

            

               <PagerStyle PageSizes="20,30,40,50,100" PageButtonCount="15" />

            

<FilterMenu EnableImageSprites="False"></FilterMenu>
           </telerik:RadGrid>
        

           <asp:ObjectDataSource ID="BundleDataSource" runat="server" DeleteMethod="Delete" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" SelectMethod="GetAssessments" StartRowIndexParameterName="startRow" TypeName="Fot.Admin.Services.AssessmentService" EnablePaging="True">
               <DeleteParameters>
                   <asp:Parameter Name="AssessmentId" Type="Int32" />
               </DeleteParameters>
                  <SelectParameters>
                 <asp:ControlParameter ControlID="txtSearch" Name="searchTerm" PropertyName="Text" Type="String" />
            </SelectParameters>
           </asp:ObjectDataSource>
        

    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="500px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
        

    </div>
</asp:Content>
