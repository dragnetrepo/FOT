<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Fot.Admin.Results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .FloatRight {
            float: right;
        }

        input.myButton {
            margin-bottom: 1px;
            /*margin-left: 10px;*/
        }

        .assessmentHeader {
            background-color: #f0f0f0;
            padding: 3px;
            padding-left: 10px;
            font-weight: bold;
            font-size: 12px;
            border: 1px solid #ccc;
            border-right: none;
            border-left: none;
            border-bottom: none;
        }

          .timeHeader {
            background-color: #f0f0f0;
            padding: 3px;
            padding-left: 10px;
            font-size: 12px;
            border: 1px solid #ccc;
            border-right: none;
            border-left: none;
            border-bottom: none;
        }

        .assessmentContent {
            padding: 3px;
            padding-left: 10px;
            font-size: 12px;
            border: 1px solid #ccc;
            border-right: none;
            border-left: none;
            border-bottom: none;
        }

        .topNameStyleBold {
            padding: 3px;
            padding-left: 5px;
            font-size: 14px;
            font-weight: bold;
        }

        .topNameStyle {
            padding: 3px;
            padding-left: 5px;
            font-size: 14px;
        }
    </style>

    <script type="text/javascript">


        function ShowPhoto(photoUrl) {
            var oWnd = radopen("Photo.aspx?id=" + photoUrl, "RadWindow1");
            oWnd.center();

        }



    </script>

</asp:Content>






<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Campaign Results</h1>

    <div style="padding: 0px; margin-top: 20px; font-size: 14px;">

        <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">
            <tr>
                <td style="width: 200px; padding-left: 3px; color: #666;"><strong>Campaign</strong></td>
                <td style="padding: 0px;">
                    <div style="padding: 0px; float: left;">
                        <asp:Literal ID="lblCampaignName" runat="server"></asp:Literal>
                    </div>
                    <div style="float: right; padding: 0px; padding-right: 5px; padding-top: 2px;">
                        <asp:Button ID="bttnBackToCampaignDetails" runat="server" Text="Return To Campaign Details" CssClass="myButton" OnClick="bttnBackToCampaignDetails_Click" />
                    </div>
                </td>

            </tr>

            <tr>
                <td style="padding-left: 3px; color: #666;"><strong>Assessment Bundle</strong></td>
                <td>
                    <asp:Literal ID="lblAssessmentBundle" runat="server"></asp:Literal>
                </td>

            </tr>

            <tr>
                <td style="padding-left: 3px; color: #666;"><strong>Total Results</strong></td>
                <td>
                    <asp:Literal ID="lblTotalTested" runat="server"></asp:Literal>
                </td>

            </tr>

        </table>

    </div>
    <div style="padding: 0; margin-top: 20px; margin-bottom: 0; float: left">

        <div style="float: left;">
            <asp:Button ID="bttnDownload" runat="server" Text="Download Results in Excel" OnClick="bttnDownload_Click" CssClass="myButton" />
        </div>
        <div style="float: left; margin-left: 120px;">
            <asp:DropDownList ID="listDownloadRange" runat="server" Visible="False"></asp:DropDownList>
            <asp:Button ID="bttnDownloadPdf" runat="server" Text="Download PDF Profiles" Style="margin-left: 30px;" OnClick="bttnDownloadPdf_Click" CssClass="myButton" />
        </div>
    </div>
    <div style="padding: 0; margin-top: 20px; margin-bottom: 0; float: right; margin-right: 50px;" runat="server" id="divTopicDownload">

        <div style="float: right;">
            <div style="float: left; padding-top: 5px; padding-right: 5px;">Results With Scores Per Topic</div>
            <asp:Button ID="bttnDownloadTopic" runat="server" Text="Download as Excel" CssClass="myButton" OnClick="bttnDownloadTopic_Click" />
        </div>

    </div>

    <div style="padding: 0px; float: left; width: 99%;">
        <telerik:RadListView ID="RadListView1" runat="server" Skin="Windows7" ItemType="Fot.Admin.Models.AssessmentResultViewModel" AllowPaging="True" DataSourceID="ResultsDataSource" PageSize="12">


            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                <div style="margin-top: 5px;">
                    <telerik:RadDataPager ID="RadDataPager1" runat="server" Skin="Windows7" PageSize="12">

                        <Fields>
                            <telerik:RadDataPagerSliderField />
                        </Fields>
                    </telerik:RadDataPager>
                </div>
            </LayoutTemplate>
            <ItemTemplate>

                <div style="border: 1px solid #ccc; padding: 0px; width: 98%; margin-top: 20px; margin-bottom: 20px;">
                    <table style="width: 100%; margin: 0px;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding: 0px;">
                                <table style="width: 100%; background-color: #edf6fd; margin: 0px;" cellspacing="0">
                                    <tr>
                                        <td class="topNameStyleBold">Candidate Name</td>
                                        <td style="width: 100px;" class="topNameStyleBold">Date Taken</td>
                                        <td style="width: 120px;"><%# Item.PhotoUrl %></td>
                                    </tr>
                                    <tr>
                                        <td class="topNameStyle"><%# Item.CandidateName %></td>
                                        <td class="topNameStyle"><%# Item.DateTested.ToString("dd-MMM-yyyy") %> </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0px;">
                                <table style="width: 100%;" cellpadding="5" cellspacing="0">
                                    <tr>
                                        <td class="assessmentHeader">Assessment Name</td>
                                        <td class="assessmentHeader" style="width: 100px;">Score</td>
                                        <td class="assessmentHeader" style="width: 200px; text-align: right"> <%# string.IsNullOrWhiteSpace(Item.ProctorPlaybackId)? "&nbsp;": "<a style='text-decoration: underline;' href='ProctorStats.aspx?id=" + Item.ProctorPlaybackId + "'>Proctor Stats</a>" %></td>
                                    </tr>

                                    <%# Item.FormatedResultsTable %>
                                    
                                    <%# Item.StartAndEndTime %>
                                   
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>

            </ItemTemplate>


        </telerik:RadListView>
    </div>



    <div>

        <asp:HiddenField ID="hidId" runat="server" />

        <asp:ObjectDataSource ID="ResultsDataSource" runat="server" SelectMethod="GetResults" TypeName="Fot.Admin.Services.AssessmentResultService" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow" EnablePaging="True">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="CampaignId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Height="340px" Skin="Windows7" Width="360px" DestroyOnClose="True" InitialBehaviors="Close, Move" ReloadOnShow="True" Behaviors="Close, Move" VisibleStatusbar="False" Title="Candidate Photo">
        </telerik:RadWindowManager>

    </div>
</asp:Content>
