<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Fot.Admin.Client.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
      <style type="text/css">
        
          input.myButton
        {
            margin-bottom: 1px;
            margin-left: 10px;
              width: 120px;
        }

              input.myButton2
        {
            margin-bottom: 1px;
            margin-left: 10px;
              
        }

                  .divOption
        {
            margin: 0px;
            padding: 5px;
            border: 1px solid #ccc;
            border-top: none;
            height: 70px;
            clear: both;
        }

    </style>
    
       <script type="text/javascript">


           $(document).ready(function () {


               $(".divOption:first").css("border-top", "1px solid #ccc");


           });

       </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Campaign Reports / Question Performance</h1>
    
    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Campaign:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblCampaignName" runat="server"></asp:Literal>
        </div>
        <div style="float: right">
            <asp:Button ID="bttnBackToCampaignDetails" runat="server" Text="Return To Campaign Details" OnClick="bttnBackToCampaignDetails_Click"  />
        </div>
    </div>
        <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Assessment:</strong>
        </div>
        <div style="float: left">
            <asp:DropDownList ID="listAssessments" runat="server" DataSourceID="AssessmentListDataSource" DataTextField="Name" DataValueField="AssessmentId"></asp:DropDownList>
        </div>
        <div style="float: left; margin-left: 10px;">
            <asp:Button ID="bttnShowDetails" runat="server" Text="Show Details" OnClick="bttnShowDetails_Click"  />
        
      
        <asp:HiddenField ID="hidId" runat="server" />


        </div>
    </div>
    
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="200px" Width="300px" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
    <div style="margin-top: 30px;" runat="server" ID="divReport" Visible="False">
        <div style="padding: 10px; border: 1px solid #f0f0f0; width: 1040px;">
        
        <table style="width: 1020px;">
            <tr>
                <td style="font-weight: 700">
                  <div> <div style="float: left"> <asp:Literal ID="lblQuestionCount" runat="server"></asp:Literal></div>
                      <div style="float: right"> Total times this question has been served: <asp:Literal ID="lblQuestionShownCount" runat="server"></asp:Literal></div></div>
                </td>
            </tr>
            <tr runat="server" ID="trTopic">
                <td><div style="font-weight: 700; width: 150px; float: left">Topic:</div>
                   <div style="float: left"> <asp:Literal ID="lblTopic" runat="server"></asp:Literal></div>
                    </td>
            </tr>
            <tr runat="server" ID="trLevel">
                <td><div style="font-weight: 700; width: 150px; float: left">Difficulty Level:</div>
                    <div style="float: left"> <asp:Literal ID="lblDifficultyLevel" runat="server"></asp:Literal></div>
                </td>
            </tr>
            <tr>
                <td style="height: 250px;">
                    <div style="height: 250px; width: 1020px; padding: 3px; overflow: auto; border: 1px solid #f0f0f0;">
                        <img  id="imgQuestion" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 50px;">
                    <asp:Literal ID="lblAdditionalText" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="height: 300px; vertical-align: top; padding: 5px;">
                    <asp:Literal ID="lblOptions" runat="server"></asp:Literal>;</td>
            </tr>
            <tr>
                <td>
                   <div style="float: left"><asp:Button ID="bttnPrevious" runat="server" Text="Previous" CssClass="myButton" OnClick="bttnPrevious_Click"  /></div> 
                   <div style="float: right"> <asp:Button ID="bttnNext" runat="server" Text="Next" CssClass="myButton" OnClick="bttnNext_Click"  /></div>
                </td>
            </tr>
            <tr>
                <td style="height: 50px;" align="center">
                 &nbsp;
                </td>
            </tr>
        </table>
    </div>
      
    </div>
    </telerik:RadAjaxPanel>
    <div>
        <asp:ObjectDataSource ID="AssessmentListDataSource" runat="server" TypeName="Fot.Admin.Services.PartnerAssessmentService" SelectMethod="GetMCQAssessmentsInBundle">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
        </telerik:RadAjaxLoadingPanel>
    </div>

</asp:Content>
