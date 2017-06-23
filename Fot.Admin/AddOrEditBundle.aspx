<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOrEditBundle.aspx.cs" Inherits="Fot.Admin.AddOrEditBundle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        input.myButton {
            margin-bottom: 0px;
            margin-left: 10px;
           
        }


    </style>
    
  

       <script type="text/javascript">


           $(document).ready(function () {
               
               $("#chkShowPassFailMessage").change(function () {

                   ShowHideMinScore();
               });


               ShowHideMinScore();
               

            
           });
           




           function ShowHideMinScore() {

               var x = $("#chkShowPassFailMessage:checked").val();

               //  alert(x);

               if (x == undefined) {


                   if ($("#txtMinScore").val() === "")
                       $("#txtMinScore").val("999");



                   $("#rowMinScore").css("visibility", "hidden");
                  
               }
               else {

                   if ($("#txtMinScore").val() === "999")
                       $("#txtMinScore").val("");



                   $("#rowMinScore").css("visibility", "visible");
                   
               }


           }


    </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Add Assessment Bundle</h1>
    
      <div style=" text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding:10px;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 200px;">Bundle Name</td>
                <td>
                    <asp:TextBox ID="txtBundleName" runat="server" Width="690px" data-errormessage-value-missing="Assessment bundle name is required!"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtBundleName">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Description</td>
                <td>
                            <telerik:radeditor ID="editor" Runat="server" EditModes="Design" 
                                Skin="Windows7" Height="190px" Width="700px">
                                       <ImageManager DeletePaths="~/img" UploadPaths="~/img" ViewPaths="~/img" />
            <Tools>
                <telerik:EditorToolGroup>
                    <telerik:EditorTool Name="ImageManager" />
                    <telerik:EditorTool Name="FontName" />
                    <telerik:EditorTool Name="FontSize" />
                    <telerik:EditorTool Name="ForeColor" />
                    <telerik:EditorTool Name="Cut" />
                    <telerik:EditorTool Name="Copy" />
                    <telerik:EditorTool Name="Bold" />
                    <telerik:EditorTool Name="Underline" />
                    <telerik:EditorTool Name="Italic" />
                    <telerik:EditorTool Name="TableWizard" />
                    <telerik:EditorTool Name="InsertTable" />
                    <telerik:EditorTool Name="InsertSymbol" />
                    <telerik:EditorTool Name="InsertUnorderedList" />
                    <telerik:EditorTool Name="InsertOrderedList" />
                    <telerik:EditorTool Name="JustifyCenter" />
                    <telerik:EditorTool Name="JustifyFull" />
                    <telerik:EditorTool Name="JustifyLeft" />
                    <telerik:EditorTool Name="JustifyRight" />
                    <telerik:EditorTool Name="Superscript" />
                    <telerik:EditorTool Name="Subscript" />
                    <telerik:EditorTool Name="Indent" />
                </telerik:EditorToolGroup>
            </Tools>
                                <Content>
</Content>
                            </telerik:radeditor>
                        </td>
            </tr>
            <tr>
                <td>Result Option</td>
                <td>
                    <asp:CheckBox ID="chkShowResultsOnSubmit" runat="server" Text="Show Results When Candidate Submits" />
                </td>
            </tr>
            <tr>
                <td>Persistence Option</td>
                <td>
                    <asp:CheckBox ID="chkSaveAsYouGo" runat="server" Text="Save assessments periodically during test" Checked="True" />
                </td>
            </tr>
            <tr>
                <td>Notification Option</td>
                <td>
                    <asp:CheckBox ID="chkSendNotification" runat="server" Text="Send result notification email after test" />
                </td>
            </tr>
            <tr>
                <td>Result Status Option</td>
                <td>
                    <asp:CheckBox ID="chkShowPassFailMessage" runat="server" Text="Show Pass / Fail Message When Candidate Submits" ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td>Assessment Selection Option</td>
                <td>
                    <asp:CheckBox ID="chkAllowAssessmentSelection" runat="server" Text="Allow Assessment Selection" ClientIDMode="Static" />
                </td>
            </tr>
            <tr id="rowMinScore" <%--style="visibility: hidden"--%>>
                <td>Min Aggregate Pass Score</td>
                <td>
                    <asp:TextBox ID="txtMinScore" runat="server" ClientIDMode="Static" Width="50px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMinScore" CssClass="Formerror" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtMinScore" CssClass="Formerror" ErrorMessage="CompareValidator" Operator="DataTypeCheck" Type="Integer">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="bttnAdd" runat="server" Text="Add" OnClick="bttnAdd_Click" />
                    <asp:Button ID="bttnUpdate" runat="server" Text="Update" Visible="False" OnClick="bttnUpdate_Click" />
                    <asp:HiddenField ID="hidId" runat="server" />
                </td>
            </tr>
        </table>
        </div>
    <div style="padding:10px; border: 1px solid #f3ebeb; min-height: 250px;" id="divAssessments" runat="server" Visible="False">
        <table style="width: 100%;">
            <tr>
                <td style="width: 150px;">Assessments</td>
                <td>
                    <div style="float: left"><asp:DropDownList ID="listAssessments" runat="server" Width="650px" DataSourceID="NotInBundleDataSource" DataTextField="Name" DataValueField="AssessmentId">
                      
                    </asp:DropDownList></div><div style="float: left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="listAssessments" CssClass="Formerror" ValidationGroup="top"></asp:RequiredFieldValidator> </div>
                     <div style="float: left"><asp:Button ID="bttnAddSelected" runat="server" Text="Add Selected"  CssClass="myButton" OnClick="bttnAddSelected_Click" ValidationGroup="top"/></div>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            </table>
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="BundleAssessmentsDataSource" OnItemDeleted="RadGrid1_ItemDeleted">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
<MasterTableView DataSourceID="BundleAssessmentsDataSource" DataKeyNames="EntryId">
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
        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Assessment" SortExpression="Name" UniqueName="Name">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="AssessmentType" FilterControlAltText="Filter AssessmentType column" HeaderText="Type" SortExpression="AssessmentType" UniqueName="AssessmentType">
            <HeaderStyle Width="120px" />
            <ItemStyle Width="120px" />
        </telerik:GridBoundColumn>
        <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" FilterControlAltText="Filter column column" Text="Remove" UniqueName="column" ConfirmText="Remove this assessment from this bundle?">
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
        <asp:ObjectDataSource ID="BundleAssessmentsDataSource" runat="server" SelectMethod="GetAssessmentsInBundle" TypeName="Fot.Admin.Services.AssessmentService" DeleteMethod="DeleteAssessmentFromBundle">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="BundleId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="NotInBundleDataSource" runat="server" SelectMethod="GetAssessmentsNotInBundle" TypeName="Fot.Admin.Services.AssessmentService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="BundleId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="260px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
        </div>
</asp:Content>
