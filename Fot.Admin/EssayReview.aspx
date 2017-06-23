<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EssayReview.aspx.cs" Inherits="Fot.Admin.EssayReview" %>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h1>Essay Review</h1>
    
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
               <td style="padding: 0px;">
                    <div style="padding: 0px; float: left;"> <asp:Literal ID="lblCandidateName" runat="server"></asp:Literal>
        <asp:HiddenField ID="hidId" runat="server" /></div>
                    
                     <div style="float: right; padding: 0px; padding-right: 5px; padding-top: 2px;">
                         <asp:Button ID="bttnDownload" runat="server" Text="Download Essay" OnClick="bttnDownload_Click" CssClass="myButton2" />
                         </div>
                </td>

            </tr>

            </table>

    </div>
    
       <div style="padding: 10px; border: 1px solid #f0f0f0; margin-top: 40px; margin-left: 50px; width: 1040px;">
        
        <table style="width: 1020px;">
            <tr>
                <td style="font-weight: 700">
                  <div> <div style="float: left">Topic:  <asp:Literal ID="lblEssayTopic" runat="server"></asp:Literal></div>
                      </div>
                </td>
            </tr>
            <tr>
                <td style="height: 250px;">
                    
                    <div style="height: 450px; width: 1020px; padding: 3px; overflow: auto; border: 1px solid #f0f0f0;">
                        <asp:Literal ID="lblEssayContent" runat="server"></asp:Literal>
                    </div>

                </td>
            </tr>
            <tr>
                <td style="height: 50px;" align="center">
                 &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
