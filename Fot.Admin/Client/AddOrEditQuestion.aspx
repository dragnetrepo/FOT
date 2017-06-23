<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="AddOrEditQuestion.aspx.cs" Inherits="Fot.Admin.Client.AddOrEditQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .divOption
        {
            margin: 0px;
            padding: 5px;
            border: 1px solid #ccc;
            border-top: none;
            height: 70px;
            clear: both;
        }




        input.myButton, button.myButton
        {
            margin-bottom: 0px;
            margin-left: 10px;
        }
    </style>



    <script type="text/javascript">



        $(document).ready(function () {



            $("#bttnAddOption").click(function () {

                AddOption();
                return false;

            });


            $("#bttnAddDelGroup").click(function () {

                AddGroup();
                return false;

            });



            $(".divOption:first").css("border-top", "1px solid #ccc");



        });

        function AddOption() {
            var oWnd = radopen("Dialogs/AddOption.aspx?qid=<%=hidQId.Value%>", "RadWindow1");

            oWnd.center();

        }

        function AddGroup() {
            var oWnd = radopen("Dialogs/AddOrDeleteGroup.aspx?aid=<%=hidAId.Value%>", "RadWindow1");

            oWnd.set_height(450);

            oWnd.center();

        }



        function refreshGrid() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

            radMgr.ajaxRequest("Rebind");
        }

        function refreshGroups() {

            var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

            radMgr.ajaxRequest("RebindGroups");
        }


        function ConfirmDelete() {

            return window.confirm("Delete this option?");

        }


    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Add Question</h1>

    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Assessment Name:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblAssessmentName" runat="server"></asp:Literal>
        </div>
        <div style="float: right">
             <asp:Button ID="bttnAddNew" runat="server" Text="Add New Question" style="margin-right: 20px;" OnClick="bttnAddNew_Click"/>
            <asp:Button ID="bttnBackToQuestions" runat="server" Text="Return To Questions" OnClick="bttnBackToQuestions_Click" />
        </div>
    </div>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 10px;">
        <table style="width: 100%;">
            <tr>
                <td style="vertical-align: top; width: 150px">Question</td>
                <td>
                    <telerik:RadEditor ID="editor" runat="server" EditModes="Design"
                        Skin="Windows7" Height="240px" Width="900px">
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
                <td style="vertical-align: top; width: 150px">Additional Text</td>
                <td>
                    <asp:TextBox ID="txtAdditionalText" runat="server" Width="890px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 150px">Options Type</td>
                <td>

                    <div style="float: left; margin-right: 50px;">
                        <asp:DropDownList ID="listOptionsType" runat="server" Width="200px">
                            <asp:ListItem Value="Single">Single Option</asp:ListItem>
                            <asp:ListItem Value="Multiple">Multiple Options</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="width: 120px; float: left;">Group</div>
                    <div style="float: left;">
                        <div style="float: left">
                            <asp:DropDownList ID="listGroup" runat="server" Width="200px" AppendDataBoundItems="True" DataSourceID="QuestionGroupDataSource" DataTextField="GroupName" DataValueField="GroupId">
                                <asp:ListItem Value="0">Not Specified</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div style="float: right">
                            <button id="bttnAddDelGroup" class="myButton">Add / Edit Groups</button></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 150px">Topic / Section</td>
                <td>
                    <div style="float: left; margin-right: 50px;">
                        <asp:DropDownList ID="listTopic" runat="server" Width="200px" AppendDataBoundItems="True" DataSourceID="TopicDataSource" DataTextField="Topic" DataValueField="TopicId">
                            <asp:ListItem Value="0">Not Specified</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="width: 120px; float: left;">Difficulty Level</div>
                    <div style="float: left;">
                        <asp:DropDownList ID="listLevel" runat="server" Width="200px" AppendDataBoundItems="True" DataSourceID="DifficultyDataSource" DataTextField="LevelName" DataValueField="LevelId">
                            <asp:ListItem Value="0">Not Specified</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 150px">Options Layout</td>
                <td>
                        <asp:DropDownList ID="listOptionsLayout" runat="server" Width="200px">
                            <asp:ListItem Value="1" Selected="True">Vertical</asp:ListItem>
                            <asp:ListItem Value="0">Horizontal</asp:ListItem>
                        </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="bttnAdd" runat="server" Text="Add Question" OnClick="bttnAdd_Click" />
                    <asp:Button ID="bttnUpdate" runat="server" Text="Update Question" Visible="False" OnClick="bttnUpdate_Click" />
                    <asp:HiddenField ID="hidAId" runat="server" />
                    <asp:HiddenField ID="hidQId" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <div style="padding: 10px; margin-top: 20px; border: 1px solid #f0f0f0; min-height: 400px" id="divOptions" runat="server" Visible="False">
        <div style="padding-right: 10px; margin-bottom: 20px;">
            <strong>Question Options</strong>
        </div>
        <button id="bttnAddOption">Add Option</button>

        <div style="padding: 10px; border: 1px solid #f0f0f0; min-height: 300px">
            <telerik:RadListView ID="RadListView1" runat="server" DataKeyNames="AnswerId" OnItemCommand="RadListView1_ItemCommand" OnItemDeleting="RadListView1_ItemDeleting" Skin="Windows7" DataSourceID="OptionsDataSource" ItemType="Fot.Admin.Models.AnswerViewModel">


                <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </LayoutTemplate>
                <ItemTemplate>


                    <div class="divOption">
                        <div style="float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;">
                            <%# Item.IsCorrect ? "<img src='../images/accept.png'>" : string.Empty %>
                        </div>
                        <div style="float: left; margin-right: 20px; margin-top: 5px;">

                            <%# Item.IsImage ? "<img src='ImageHandler.ashx?t=a&id=" + Item.AnswerId +"' style='height: 70px; vertical-align: top;'>" : Item.AnswerText %>
                        </div>

                        <div style="float: right;">
                            <asp:Button ID="Button2" runat="server" Text="Delete" CssClass="myButton" CommandName="Delete" CommandArgument='<%# Item.AnswerId %>' />
                        </div>
                        <div style="float: right; margin-right: 10px;">
                            <asp:Button ID="Button1" runat="server" Text="Set As Correct Option" CssClass="myButton" CommandName="Set" CommandArgument='<%# Item.AnswerId %>' Visible='<%# Item.IsCorrect == false %>' />
                            <asp:Button ID="Button3" runat="server" Text="Unset As Correct Option" CssClass="myButton" CommandName="Unset" CommandArgument='<%# Item.AnswerId %>' Visible='<%# Item.IsCorrect && Item.AnswerType.Equals("Multiple") %>' />
                        </div>
                    </div>

                </ItemTemplate>
                <AlternatingItemTemplate>

                    <div class="divOption" style="background-color: #edf6fd;">
                        <div style="float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;">
                            <%# Item.IsCorrect ? "<img src='../images/accept.png'>" : string.Empty %>
                        </div>
                        <div style="float: left; margin-right: 20px; margin-top: 5px;">

                            <%# Item.IsImage ? "<img src='ImageHandler.ashx?t=a&id=" + Item.AnswerId +"' style='height: 70px; vertical-align: top;'>" : Item.AnswerText %>
                        </div>

                        <div style="float: right;">
                            <asp:Button ID="Button2" runat="server" Text="Delete" CssClass="myButton" CommandName="Delete" CommandArgument='<%# Item.AnswerId %>' />
                        </div>
                        <div style="float: right; margin-right: 10px;">
                            <asp:Button ID="Button1" runat="server" Text="Set As Correct Option" CssClass="myButton" CommandName="Set" CommandArgument='<%# Item.AnswerId %>' Visible='<%# Item.IsCorrect == false %>' />
                            <asp:Button ID="Button3" runat="server" Text="Unset As Correct Option" CssClass="myButton" CommandName="Unset" CommandArgument='<%# Item.AnswerId %>' Visible='<%# Item.IsCorrect && Item.AnswerType.Equals("Multiple") %>' />
                        </div>
                    </div>
                </AlternatingItemTemplate>

            </telerik:RadListView>
        </div>

    </div>

    <div>
        <asp:ObjectDataSource ID="OptionsDataSource" runat="server" SelectMethod="GetAnswers" TypeName="Fot.Admin.Services.PartnerAssessmentAnswerService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidQId" Name="QuestionId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="QuestionGroupDataSource" runat="server" SelectMethod="GetGroups" TypeName="Fot.Admin.Services.QuestionGroupService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidAid" Name="AssessmentId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DifficultyDataSource" runat="server" SelectMethod="GetLevels" TypeName="Fot.Admin.Services.QuestionDifficultyLevelService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidAId" Name="AssessmentId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="TopicDataSource" runat="server" SelectMethod="GetTopics" TypeName="Fot.Admin.Services.AssessmentTopicService">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidAId" Name="AssessmentId" PropertyName="Value" Type="Int32" DefaultValue="" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadListView1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblStatus" />
                        <telerik:AjaxUpdatedControl ControlID="RadListView1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="listGroup" />
                        <telerik:AjaxUpdatedControl ControlID="RadListView1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager
            ID="RadWindowManager1" runat="server" Height="350px" Skin="Windows7"
            Width="680px" Modal="True" VisibleStatusbar="False"
            Behaviors="Close, Move, Reload" DestroyOnClose="True">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
        </telerik:RadAjaxLoadingPanel>
    </div>

</asp:Content>
