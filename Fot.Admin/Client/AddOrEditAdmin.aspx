<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="AddOrEditAdmin.aspx.cs" Inherits="Fot.Admin.Client.AddOrEditAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">
        td
        {
            padding-top: 5px;
            padding-bottom: 5px;
        }
    </style>



    <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine.js" type="text/javascript"></script>


    <link href="css/validationEngine.jquery.css" rel="stylesheet" />



    <script type="text/javascript">


        $(document).ready(function () {

            $("#form1").validationEngine();
        });


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>
        <asp:Literal runat="server" ID="lblHeader" Text="Add Administrator"></asp:Literal></h1>

    <div style="padding: 10px; text-align: center; height: 30px;">
        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>

    <div style="padding: 10px;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 150px;">Email</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" CssClass="validate[required,custom[email]]" data-errormessage-value-missing="Email is required!" data-errormessage="Enter a valid value for email" data-prompt-position="centerRight"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">Password</td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password" CssClass="validate[required]" data-errormessage-value-missing="Password is required!"  data-prompt-position="centerRight"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">First Name</td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" Width="300px" CssClass="validate[required]" data-errormessage-value-missing="First name is required!"  data-prompt-position="centerRight"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">Last Name</td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" Width="300px" CssClass="validate[required]" data-errormessage-value-missing="Last name is required!"  data-prompt-position="centerRight"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">Mobile Number</td>
                <td>
                    <asp:TextBox ID="txtMobileNumber" runat="server" Width="300px" CssClass="validate[required]" data-errormessage-value-missing="Mobile number is required!"  data-prompt-position="centerRight"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 150px; vertical-align: top;">Permissions</td>
                <td>
                    <asp:CheckBoxList ID="chkListPermissions" runat="server" CellPadding="3" CellSpacing="3" RepeatColumns="5" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Author">Author</asp:ListItem>
                        <asp:ListItem>Schedule</asp:ListItem>
                        <asp:ListItem>Results</asp:ListItem>
                        <asp:ListItem Value="CenterUsers">Center Users</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">Status</td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server" Checked="True" Text="Active" />
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
        <hr />
    </div>

    <div style="padding: 0px; margin-top: 50px; margin-bottom: 50px;" id="divAuthorAssessments" runat="server" visible="False">
        <fieldset style="width: 100%; border: 1px solid #ccc;">
            <legend style="font-size: 18px; font-weight: bold; padding-left: 10px;">Assigned Assessments</legend>

            <div style="padding: 10px;">

                <table style="width: 100%;" cellpadding="4">
                    <tr>
                        <td style="width: 150px;">Assessments</td>
                        <td style="width: 270px;">
                            <asp:DropDownList ID="listAssessments" runat="server" DataSourceID="AssessmentsDataSource" DataTextField="Name" DataValueField="AssessmentId">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="bttnAddAssessment" runat="server" Text="Add Assessment" Style="margin-left: 5px; margin-bottom: 3px;" OnClick="bttnAddAssessment_Click" /></td>
                    </tr>

                </table>

            </div>
            <div style="padding: 10px;">



                <telerik:RadGrid ID="GridAssessments" runat="server" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" AllowAutomaticDeletes="True" PageSize="5" AllowPaging="True" DataSourceID="AuthorAssignedAssessmentsDataSource" OnItemDeleted="GridAssessments_ItemDeleted">
                    <ClientSettings>
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView DataSourceID="AuthorAssignedAssessmentsDataSource" DataKeyNames="EntryId">
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
                            <telerik:GridBoundColumn DataField="AssessmentType" DataType="System.Int32" FilterControlAltText="Filter AssessmentType column" HeaderText="Type" SortExpression="AssessmentType" UniqueName="AssessmentType">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Remove this assessment?" FilterControlAltText="Filter column column" Text="Remove" UniqueName="column">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" />
                            </telerik:GridButtonColumn>
                        </Columns>

                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        </EditFormSettings>
                    </MasterTableView>

                    <HeaderStyle Font-Bold="True" />

                    <PagerStyle CssClass="validate-skip" />

                    <FilterMenu EnableImageSprites="False"></FilterMenu>
                </telerik:RadGrid>

            </div>



        </fieldset>

    </div>

    <div>

        <asp:ObjectDataSource ID="AssessmentsDataSource" runat="server" SelectMethod="GetUnassignedAssessments" TypeName="Fot.Admin.Services.PartnerAuthorAssignedAssessmentService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AdminId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="AuthorAssignedAssessmentsDataSource" runat="server" DeleteMethod="DeleteAssessmentFromAuthor" SelectMethod="GetAssignedAssessments" TypeName="Fot.Admin.Services.PartnerAuthorAssignedAssessmentService" EnablePaging="True" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow" SelectCountMethod="Count">
            <DeleteParameters>
                <asp:Parameter Name="EntryId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AdminId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <telerik:RadWindowManager
            ID="RadWindowManager1" runat="server" Height="300px" Skin="Windows7"
            Width="650px" Modal="True" VisibleStatusbar="False"
            Behaviors="Close, Move, Reload" DestroyOnClose="True">
        </telerik:RadWindowManager>

    </div>

</asp:Content>
