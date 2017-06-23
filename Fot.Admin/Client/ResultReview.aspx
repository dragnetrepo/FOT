<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="ResultReview.aspx.cs" Inherits="Fot.Admin.Client.ResultReview" %>
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
    <h1>Result Review</h1>
    
      <div style="padding: 0px; margin-top: 20px; font-size: 14px;">

        <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">
            <tr>
                <td style="width: 200px; padding-left: 3px; color: #666;"><strong>Assessment Name</strong></td>
                <td style="padding: 0px;">
                    <div style="padding: 0px; float: left;">
                        <asp:Literal ID="lblAssessmentName" runat="server"></asp:Literal></div>
                    <div style="float: right; padding: 0px; padding-right: 5px; padding-top: 2px;">
                        <asp:Button ID="bttnBackToResults" runat="server" Text="Return To Results Page" CssClass="myButton2" OnClick="bttnBackToResults_Click"  />
                    </div>
                </td>

            </tr>

            <tr>
                <td style="padding-left: 3px; color: #666;"><strong>Candidate Name</strong></td>
                <td>
                    <asp:Literal ID="lblCandidateName" runat="server"></asp:Literal>
                </td>

            </tr>

            <tr>
                <td style="padding-left: 3px; color: #666;">
                        <asp:Button ID="bttnDownloadPDF" runat="server" Text="Download Review as PDF" CssClass="myButton2" OnClick="bttnDownloadPDF_Click" style="margin-left: 0;" />
                    </td>
                <td>
                    &nbsp;</td>

            </tr>

            </table>

    </div>
    
    <div style="padding: 10px; border: 1px solid #f0f0f0; margin-top: 40px; margin-left: 50px; width: 1040px;">
        
        <table style="width: 1020px;">
            <tr>
                <td style="font-weight: 700">
                  <div> <div style="float: left"> <asp:Literal ID="lblQuestionCount" runat="server"></asp:Literal></div>
                      <div style="float: right"> <asp:Literal ID="lblRightOrWrong" runat="server"></asp:Literal></div></div>
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
                   <div style="float: left"><asp:Button ID="bttnPrevious" runat="server" Text="Previous" CssClass="myButton" OnClick="bttnPrevious_Click" /></div> 
                   <div style="float: right"> <asp:Button ID="bttnNext" runat="server" Text="Next" CssClass="myButton" OnClick="bttnNext_Click" /></div>
                </td>
            </tr>
            <tr>
                <td style="height: 50px;" align="center">
                 &nbsp;
                </td>
            </tr>
        </table>
    </div>
    
    <div>
        <asp:HiddenField ID="hidId" runat="server" />
    </div>
</asp:Content>
