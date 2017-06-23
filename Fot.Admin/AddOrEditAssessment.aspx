<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOrEditAssessment.aspx.cs" Inherits="Fot.Admin.AddOrEditAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .padd
        {
            margin-top: 10px;
        }

        input.myButton
        {
            margin-bottom: 0px;
            margin-left: 10px;
        }
    </style>

    <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine.js" type="text/javascript"></script>


    <link href="css/validationEngine.jquery.css" rel="stylesheet" />

    <script type="text/javascript">

        $(document).ready(function () {

            $("#chkTimed").change(function () {

                ShowHideDuration();
            });



            $("#listType").change(function () {

                ShowHideMCQAttributes();

            });


            $("#listRetrievalOptions").change(function () {

                ShowHideAdvancedConfiguration();

            });


            $("#bttnAddTopic").click(function () {

                AddTopic();
                return false;

            });

            $("#bttnAddDifficultyLevel").click(function () {

                AddLevel();
                return false;

            });

            ShowHideDuration();
            ShowHideAdvancedConfiguration();
            ShowHideMCQAttributes();



            $("#form1").validationEngine();

        });


        function ShowHideAdvancedConfiguration() {

            var str = $("#listRetrievalOptions option:selected").val();

            if (str === "Simple") {

                $("#bttnConfigure").hide();
                $("#txtQuestionsPerTest").removeAttr("readonly");


            }
            else if (str === "Advanced") {

                $("#bttnConfigure").show();
                $("#txtQuestionsPerTest").attr("readonly", "readonly");
            }

        }


        function ShowHideMCQAttributes() {

            var str = $("#listType option:selected").val();

            if (str === "1") {

                $("#trRandonmizationOptions").show();
                $("#trRetrievalOptions").show();
                $("#trQuestionsPerTest").show();
                $("#divTopics").show();
                $("#divShowCalculator").show();
            }
            else if (str === "2") {

                $("#trRandonmizationOptions").hide();
                $("#trRetrievalOptions").hide();
                $("#trQuestionsPerTest").hide();
                $("#divTopics").hide();
                $("#divShowCalculator").hide();
            }

        }

        function ShowHideDuration() {

            var x = $("#chkTimed:checked").val();

            //  alert(x);

            if (x == undefined) {


                if ($("#txtDuration").val() === "")
                    $("#txtDuration").val("999");



                $("#txtDuration").css("visibility", "hidden");
                $("#lblDuration").css("visibility", "hidden");
            }
            else {

                if ($("#txtDuration").val() === "999")
                    $("#txtDuration").val("");



                $("#txtDuration").css("visibility", "visible");
                $("#lblDuration").css("visibility", "visible");
            }


        }


    </script>

    <script type="text/javascript">

        function AddTopic() {
            var oWnd = radopen("Dialogs/AddOrEditTopic.aspx?aid=<%=hidId.Value%>", "RadWindow1");
            oWnd.center();

        }

        function AddLevel() {
            var oWnd = radopen("Dialogs/AddOrEditLevel.aspx?aid=<%=hidId.Value%>", "RadWindow1");
                oWnd.center();

            }

            function UpdateTopic(ID) {
                var oWnd = radopen("Dialogs/AddOrEditTopic.aspx?tid=" + ID, "RadWindow1");
                oWnd.center();

            }

            function UpdateLevel(ID) {
                var oWnd = radopen("Dialogs/AddOrEditLevel.aspx?lid=" + ID, "RadWindow1");
                oWnd.center();

            }


            function refreshGrid() {

                var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                           radMgr.ajaxRequest("Rebind");
                       }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Add Assessment</h1>
    <div style="padding-top: 10px;">
        <asp:Button ID="bttnQuestions" runat="server" Text="Questions" Style="float: right; margin-bottom: 5px;" OnClick="bttnQuestions_Click" Visible="False" />
    </div>
    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>
    <div style="padding-left: 20px;">

        <table style="width: 100%;">
            <tr>
                <td style="width: 160px;">Assessment Name</td>
                <td>
                    <asp:TextBox ID="txtAssessmentName" runat="server" Width="690px" CssClass="validate[required]" data-errormessage-value-missing="Assessment name is required!" ClientIDMode="Static"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Instructions</td>
                <td>
                    <telerik:RadEditor ID="editor" runat="server" EditModes="Design"
                        Skin="Windows7" Height="240px" Width="700px">
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
                    </telerik:RadEditor>
                </td>
            </tr>
            <tr>
                <td>Duration Option</td>
                <td>
                    <div style="float: left; vertical-align: middle; width: 500px;">


                        <div style="width: 120px; text-align: left; height: 20px; float: left; vertical-align: middle; margin-top: 5px;">
                            <asp:CheckBox ID="chkTimed" runat="server"
                                Text="Timed?" ClientIDMode="Static" Checked="True" CssClass="padd" />
                        </div>
                        <div style="width: 70px; text-align: left; height: 20px; float: left; margin-top: 5px;">
                            <span id="lblDuration">Duration:</span>
                        </div>
                        <div style="width: 70px; text-align: left; height: 20px; float: left;">
                            <asp:TextBox ID="txtDuration" runat="server" Width="40px" ClientIDMode="Static" CssClass="validate[required,custom[integer]]" data-errormessage-value-missing="Duration is required!"></asp:TextBox>

                        </div>
                   



                    </div>
                </td>
            </tr>
            <tr>
                <td>Assessment Type</td>
                <td>
                    <div>
                        <div style="float: left">
                            <asp:DropDownList ID="listType" runat="server" Width="250px" ClientIDMode="Static" DataSourceID="AssessmentTypeDataSource" DataTextField="Text" DataValueField="Value">
                            </asp:DropDownList>
                        </div>
                        <div style="float: left; margin-left: 20px; padding-top: 8px;" id="divShowCalculator">
                            <asp:CheckBox ID="chkShowCalculator" runat="server" Text="Show Calculator On-Screen During Assessment" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Assessment Author</td>
                <td>
                    <asp:DropDownList ID="listAuthor" runat="server" Width="250px" DataSourceID="AuthorDataSource" DataTextField="AuthorName" DataValueField="AuthorId" AppendDataBoundItems="True">
                        <asp:ListItem Value="">Select Assessment Author</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trRandonmizationOptions" runat="server" clientidmode="Static" visible="False">
                <td>Randomization Options</td>
                <td>
                    <div style="padding-right: 10px; float: left">
                        <asp:CheckBox ID="chkRandomizeQuestions" runat="server" Text="Randomize Questions" Checked="True" />
                    </div>
                    <div style="float: left;">
                        <asp:CheckBox ID="chkRandomizeOptions" runat="server" Text="Randomize Options" Checked="True" />
                    </div>
                </td>
            </tr>
            <tr id="trRetrievalOptions" runat="server" clientidmode="Static" visible="False">
                <td>Retrieval Options</td>
                <td>
                    <asp:DropDownList ID="listRetrievalOptions" runat="server" Width="250px" ClientIDMode="Static">
                        <asp:ListItem Value="Simple">Simple</asp:ListItem>
                        <asp:ListItem Value="Advanced">Advanced</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="bttnConfigure" runat="server" Text="Configure ..." CssClass="myButton" ClientIDMode="Static" OnClick="bttnConfigure_Click" />
                </td>
            </tr>
            <tr id="trQuestionsPerTest" runat="server" clientidmode="Static" visible="False">
                <td>Questions Per Test</td>
                <td><div style="float: left">
                    <asp:TextBox ID="txtQuestionsPerTest" runat="server" Width="40px" ClientIDMode="Static" CssClass="validate[custom[integer]]"></asp:TextBox>
                    </div>
                      <div style="float: left; margin-left: 20px; padding-top: 8px;">
                            <asp:CheckBox ID="chkFixedQuestions" runat="server" Text="Fixed Questions For All Candidates (Based on the questions per test)" />
                        </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="bttnAdd" runat="server" Text="Add Assessment" OnClick="bttnAdd_Click" />
                    <asp:Button ID="bttnUpdate" runat="server" Text="Update Assessment" Visible="False" OnClick="bttnUpdate_Click" />
                    <asp:HiddenField ID="hidId" runat="server" />
                </td>
            </tr>
        </table>

    </div>
    <div style="padding: 10px; border: 1px solid #f3ebeb; min-height: 250px;" id="divTopics" runat="server" visible="False" clientidmode="Static">

        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Windows7" Width="100%" CausesValidation="False" Font-Bold="False">
            <Tabs>
                <telerik:RadTab runat="server" Selected="True" Text="Assessment Topics" Font-Bold="True">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Question Difficulty Levels" Font-Bold="True">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>

        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Width="100%" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server">
                <div style="padding: 10px;">

                    <button id="bttnAddTopic">Add Topic</button>

                    <telerik:RadGrid ID="topicsGrid" runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="TopicDataSource" AllowPaging="True">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                        <MasterTableView DataSourceID="TopicDataSource" DataKeyNames="TopicId">
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="TopicId" DataType="System.Int32" Display="False" FilterControlAltText="Filter TopicId column" HeaderText="TopicId" SortExpression="TopicId" UniqueName="TopicId">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Topic" FilterControlAltText="Filter Topic column" HeaderText="Topic" SortExpression="Topic" UniqueName="Topic">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                                    UniqueName="TemplateColumn">
                                    <HeaderStyle Width="80px" />
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <a href="#" onclick='UpdateTopic(<%# Eval("TopicId") %>)'>Edit / Update</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn CommandName="Delete"
                                    ConfirmText="Delete this topic?" FilterControlAltText="Filter column column"
                                    Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <HeaderStyle Font-Bold="True" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView2" runat="server">
                <div style="padding: 10px;">
                    <button id="bttnAddDifficultyLevel">Add Difficulty Level</button>

                    <telerik:RadGrid ID="difficultyLevelGrid" runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" Skin="Windows7" AutoGenerateColumns="False" DataSourceID="DifficultyDataSource" AllowPaging="True">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                        <MasterTableView DataSourceID="DifficultyDataSource" DataKeyNames="LevelId">
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="LevelId" DataType="System.Int32" Display="False" FilterControlAltText="Filter LevelId column" HeaderText="LevelId" SortExpression="LevelId" UniqueName="LevelId">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LevelName" FilterControlAltText="Filter LevelName column" HeaderText="Difficulty Level" SortExpression="LevelName" UniqueName="LevelName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LevelWeight" DataType="System.Int32" FilterControlAltText="Filter LevelWeight column" HeaderText="Scale" SortExpression="LevelWeight" UniqueName="LevelWeight">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                                    UniqueName="TemplateColumn">
                                    <HeaderStyle Width="80px" />
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <a href="#" onclick='UpdateLevel(<%# Eval("LevelId") %>)'>Edit / Update</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn CommandName="Delete"
                                    ConfirmText="Delete this level?" FilterControlAltText="Filter column column"
                                    Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <HeaderStyle Font-Bold="True" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                </div>

            </telerik:RadPageView>
        </telerik:RadMultiPage>
        <asp:ObjectDataSource ID="TopicDataSource" runat="server" SelectMethod="GetTopics" TypeName="Fot.Admin.Services.AssessmentTopicService" DeleteMethod="Delete" EnablePaging="True" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow">
            <DeleteParameters>
                <asp:Parameter Name="TopicId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AssessmentId" PropertyName="Value" Type="Int32" DefaultValue="" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DifficultyDataSource" runat="server" SelectMethod="GetLevels" TypeName="Fot.Admin.Services.QuestionDifficultyLevelService" DeleteMethod="Delete" EnablePaging="True" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow">
            <DeleteParameters>
                <asp:Parameter Name="LevelId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AssessmentId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="AssessmentTypeDataSource" runat="server" SelectMethod="GetAssesmentTypes" TypeName="Fot.Admin.Infrastructure.FixedDataSources"></asp:ObjectDataSource>


        <asp:ObjectDataSource ID="AuthorDataSource" runat="server" SelectMethod="GetAuthors" TypeName="Fot.Admin.AddOrEditAssessment"></asp:ObjectDataSource>


    </div>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="260px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="topicsGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="topicsGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="difficultyLevelGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="difficultyLevelGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="topicsGrid" />
                    <telerik:AjaxUpdatedControl ControlID="Label1" />
                    <telerik:AjaxUpdatedControl ControlID="difficultyLevelGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
