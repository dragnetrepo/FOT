<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Questions.aspx.cs" Inherits="Fot.Admin.Client.Questions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .divQuestion
        {
            margin: 0px;
            padding: 5px;
            border: 1px solid #ccc;
            height: 150px;
            width: 980px;
        }

        .divOptions
        {
            margin: 0px;
            padding: 5px;
            border: 1px solid #ccc;
            border-top: none;
            height: 35px;
            width: 980px;
        }

        .divQContainer
        {
            margin: 0px;
            margin-top: 20px;
            margin-bottom: 20px;
            padding: 0px;
            width: 980px;
            clear: both;
        }

        input.myButton
        {
            margin-bottom: 0px;
            /*margin-left: 10px;*/
        }
    </style>

    <script type="text/javascript">

        function ConfirmDelete() {

            return window.confirm("Delete this questions?");

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Assessment Questions</h1>

    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Assessment Name:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblAssessmentName" runat="server"></asp:Literal>
        </div>
    </div>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>


    <div style="padding: 10px; padding-bottom: 0px; margin-top: 0px; width: 990px; margin-bottom: 40px; float: left;">
        
        <div style="float: left; margin-right: 10px;">Validity:  </div> <asp:DropDownList ID="listValidity" runat="server" style="float: left; width: 100px;" AutoPostBack="True" OnSelectedIndexChanged="listValidity_SelectedIndexChanged">
            <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
            <asp:ListItem Value="1">Valid</asp:ListItem>
            <asp:ListItem Value="2">Invalid</asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="bttnAdd" runat="server" Text="Add Question" OnClick="bttnAdd_Click" style="float: right;"/>


        <asp:HiddenField ID="hidId" runat="server" />

    </div>
    
    
    

    <div style="padding: 10px; padding-top: 0px; margin-top: 0px">

        <telerik:RadListView ID="RadListView1" runat="server" AllowPaging="True" DataSourceID="QuestionDataSource" DataKeyNames="QuestionId" OnItemCommand="RadListView1_ItemCommand" OnItemDeleting="RadListView1_ItemDeleting" Skin="Windows7" ItemType="Fot.Admin.Models.QuestionViewModel">


            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>

               
                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="RadListView1" Width="990px" PageSize="6" Skin="Windows7">
                        <Fields>
                            <telerik:RadDataPagerSliderField />
                        </Fields>
                    </telerik:RadDataPager>
              
            </LayoutTemplate>
            <ItemTemplate>
                <div class="divQContainer">
                    <div class="divQuestion">
                        <telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" ResizeMode="Crop" CropPosition="Top" Height="140" Width="970" DataValue='<%# Item.QuestionImage %>' />
                    </div>
                    <div class="divOptions">
                        <div style="float: left; margin-right: 50px; margin-top: 5px;"><strong>Options: </strong><%# Item.OptionCount %></div>
                        <div style="float: left; margin-right: 50px; margin-top: 5px;"><strong>Options Type: </strong><%# Item.AnswerType %></div>
                        <div style="float: left; margin-right: 10px; margin-top: 5px;"><strong>Validity: </strong>
                            <img src='../images/<%# Item.ValidQuestion ? "accept.png" : "exclamation.png" %>' style="height: 14px; vertical-align: middle" /></div>

                        <div style="float: right;">
                            <asp:Button ID="Button2" runat="server" Text="Delete" CssClass="myButton" CommandName="Delete" CommandArgument='<%# Item.QuestionId %>' OnClientClick="return ConfirmDelete();" />
                        </div>
                        <div style="float: right; margin-right: 10px;">
                            <asp:Button ID="Button1" runat="server" Text="Edit Question" CssClass="myButton" CommandName="Edit" CommandArgument='<%# Item.QuestionId %>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>


        </telerik:RadListView>


    </div>
    
    <div>

        <asp:ObjectDataSource ID="QuestionDataSource" runat="server" SelectMethod="GetQuestions" TypeName="Fot.Admin.Services.PartnerAssessmentQuestionService" MaximumRowsParameterName="maxRows" SelectCountMethod="QuestionCountByAssessment" StartRowIndexParameterName="startRow" EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AssessmentId" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="listValidity" Name="ValidEntry" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

    </div>
</asp:Content>
