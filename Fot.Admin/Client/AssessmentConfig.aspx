<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="AssessmentConfig.aspx.cs" Inherits="Fot.Admin.Client.AssessmentConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
          input.myButton, button.myButton
        {
            margin-bottom: 0px;
            margin-left: 10px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <h1>Advanced Questions Retrieval Configuration</h1>
    
    <div style=" text-align: center; padding: 0px; margin: 0px; min-height: 20px; margin-top: 10px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 60px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Assessment Name:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblAssessmentName" runat="server"></asp:Literal>
        </div>
         <div style="float: right">
            <asp:Button ID="bttnBackToDetails" runat="server" Text="Return To Assessment Details" CssClass="myButton" CausesValidation="False" OnClick="bttnBackToDetails_Click" />
        </div>

        <div style="clear: both; height: 10px;"> &nbsp;</div>
        <div style="float: left; padding-right: 10px;">
            <strong>Questions Per Test:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblQuestionsPerTest" runat="server"></asp:Literal>
        </div>
    </div>
    
    
    <div style="padding: 10px; margin-top: 20px;">
        
        
        

        <table style="width: 100%;">
            <tr>
                <td style="width: 200px;">Assessment Topic</td>
                <td>
                    <asp:DropDownList ID="listTopics" runat="server" Width="300px" AppendDataBoundItems="True" AutoPostBack="True" DataTextField="Topic" DataValueField="TopicId" OnSelectedIndexChanged="listTopics_SelectedIndexChanged">
                        <asp:ListItem Value="0">Not Specified</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Difficulty Level</td>
                <td>
                            <asp:DropDownList ID="listLevels" runat="server" Width="300px" AppendDataBoundItems="True" AutoPostBack="True" DataTextField="LevelName" DataValueField="LevelId" OnSelectedIndexChanged="listLevels_SelectedIndexChanged">
                                <asp:ListItem Value="0">Not Specified</asp:ListItem>
                            </asp:DropDownList>
                        </td>
            </tr>
            <tr>
                <td>Total Possible Questions</td>
                <td style="font-weight: 700">
                    <asp:Literal ID="lblTotalQuestionsPossible" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>Questions Required</td>
                <td>
                    <asp:TextBox ID="txtNumQuestions" runat="server" Width="40px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNumQuestions" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtNumQuestions" CssClass="Formerror" ErrorMessage="CompareValidator" Operator="DataTypeCheck" Type="Integer">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="bttnAdd" runat="server" Text="Add Config Entry" OnClick="bttnAdd_Click" />
                    <asp:HiddenField ID="hidId" runat="server" />
                </td>
            </tr>
        </table>
        
        
        

    </div>
    <div style="padding: 10px;">
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="ConfigDataSource" OnItemDeleted="RadGrid1_ItemDeleted">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="ConfigDataSource" DataKeyNames="ConfigId">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="ConfigId" DataType="System.Int32" FilterControlAltText="Filter ConfigId column" HeaderText="ConfigId" SortExpression="ConfigId" UniqueName="ConfigId" Display="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Topic" FilterControlAltText="Filter Topic column" HeaderText="Topic" SortExpression="Topic" UniqueName="Topic" EmptyDataText="&lt;i&gt;None&lt;/i&gt;">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="DifficultyLevel" FilterControlAltText="Filter DifficultyLevel column" HeaderText="Difficulty Level" SortExpression="DifficultyLevel" UniqueName="DifficultyLevel" EmptyDataText="&lt;i&gt;None&lt;/i&gt;">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="NumQuestions" DataType="System.Int32" FilterControlAltText="Filter NumQuestions column" HeaderText="Questions" SortExpression="NumQuestions" UniqueName="NumQuestions">
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
        </telerik:GridBoundColumn>
             <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" ConfirmText="Delete this config entry?">
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
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Windows7">
        </telerik:RadWindowManager>
        <asp:ObjectDataSource ID="ConfigDataSource" runat="server" DeleteMethod="Delete" SelectMethod="GetConfigs" TypeName="Fot.Admin.Services.AssessmentOutputConfigService">
            <DeleteParameters>
                <asp:Parameter Name="ConfigId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AssessmentId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

    </div>
</asp:Content>
