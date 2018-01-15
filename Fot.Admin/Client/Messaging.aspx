<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="Messaging.aspx.cs" Inherits="Fot.Admin.Client.Messaging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
           <style type="text/css">
        
        .FloatRight {
            float: right;
        }

        input.myButton {
            margin-bottom: 1px;
            margin-left: 10px;
        }

    </style>
    
        <script type="text/javascript">
            
            var editor = null;
            function OnClientLoad(sender) {
                editor = sender;
            }

            $(function () {

                $('#txtSms').bind('keypress', function () {
                    setTimeout(ShowValue, 1);
                });

                $("#txtSms").bind('blur', ShowValue);

                $("#chkPreview").change(ShowPreview);

            });

            function ShowPreview() {

                if ($('#chkPreview').is(":checked")) {


                    var index = $("#listReceipients :selected").val();


                    var content = editor.get_html();

                    content = GetContent(content, index);


                    $("#divPreview").html(content);

                    $("#divPreview").show();
                } else {
                    $("#divPreview").hide();
                }


            }


            function GetContent(content, index) {

                var temp = content;




                if (index === "1") {

                    temp = temp.replace("[[NAME]]", "John Ggboko");
                    temp = temp.replace("[[USERNAME]]", "DRG00878888");
                    temp = temp.replace("[[PASSWORD]]", "r$78ju76");

                }

                else if (index === "2") {

                    temp = temp.replace("[[NAME]]", "John Ggboko");
                    temp = temp.replace("[[USERNAME]]", "DRG00878888");
                    temp = temp.replace("[[PASSWORD]]", "r$78ju76");
                    temp = temp.replace("[[CENTER_NAME]]", "Chams City Lagos");
                    temp = temp.replace("[[CENTER_ADDRESS]]", "Plot 6 Isaac John Street, GRA, Ikeja");
                    temp = temp.replace("[[LOCATION]]", "LAGOS");
                    temp = temp.replace("[[TEST_DATE]]", "09-Jan-2014");
                    temp = temp.replace("[[TEST_TIME]]", "9:30 AM");

                }
                else if (index === "3") {

                    temp = temp.replace("[[NAME]]", "John Ggboko");
                    temp = temp.replace("[[USERNAME]]", "DRG00878888");
                    temp = temp.replace("[[PASSWORD]]", "r$78ju76");
                }

                else if (index === "4") {

                        temp = temp.replace("[[NAME]]", "John Ggboko");
                        temp = temp.replace("[[USERNAME]]", "DRG00878888");
                        temp = temp.replace("[[UNIQUE_ID]]", "NIN0000717");
                        temp = temp.replace("[[PASSWORD]]", "r$78ju76");

                        temp = temp.replace("[[DATE_TESTED]]", "01-Jan-2013");
                        temp = temp.replace("[[TEST_CENTER]]", "ChamsCity @ Lagos");

                        temp = temp.replace("[[ASSESSMENT_SCORES]]", getScores());
                }

                else if (index === "5") {

                    temp = temp.replace("[[NAME]]", "John Ggboko");
                    temp = temp.replace("[[USERNAME]]", "DRG00878888");
                    temp = temp.replace("[[PASSWORD]]", "r$78ju76");
                }

        return temp;

    }


    function getScores() {

        var htmlStr = "<table style='width:100%;'> " +
              "<tr>" +
              " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; border-bottom:1px solid #f0f0f0;'>Assessment Name 1</td> " +
               " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width:50px; border-bottom:1px solid #f0f0f0;'>36</td> " +
              " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width:50px; border-bottom:1px solid #f0f0f0;'>43%</td> " +
              "</tr> " +
              " <tr> " +
              "<td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; border-bottom:1px solid #f0f0f0;'>Assessment Name 2</td> " +
              " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width:50px; border-bottom:1px solid #f0f0f0;'>41</td> " +
              " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width:50px; border-bottom:1px solid #f0f0f0;'>54%</td> " +
              "</tr> " +
              " <tr> " +
              " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0;'>Aggregate</td> " +
               " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0; width:50px;'>38</td> " +
              " <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0; width:50px;'>48.5%</td> " +
              "</tr>" +
              " <tr> " +
              "<td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>  " +
              "<td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td> " +
              "</tr> " +
              "</table>";

        return htmlStr;
    }


            function ShowValue() {

                var item = $("#txtSms");

                var total = 160;

                var currentLength = item.val().length;

                var remaining = total - currentLength;



                if (remaining < 0) {

                    $("#txtSms").val(item.val().substr(0, 160));
                    remaining = 0;
                    $("#lblCount").css("color", "red");
                }
                else {
                    $("#lblCount").css("color", "#696969");
                }

                var str = remaining == 1 ? "character" : "characters";

                $("#lblCount").text(remaining + " " + str + " remaining");
            }

            function ShowHideMessaging() {

                var x = $("#chkScheduleMessage:checked").val();


                if (x == undefined) {

                    $("#trSmsHeader").hide();
                    $("#trSms").hide();
                    $("#trEmailSubject").hide();
                    $("#trEmail").hide();


                }
                else {

                    $("#trSmsHeader").show();
                    $("#trSms").show();
                    $("#trEmailSubject").show();
                    $("#trEmail").show();
                }

            }


            function validateCheckBoxList(sender, args) {

                args.IsValid = ($("#chkListSessions :checkbox:checked").length > 0);

            }

        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <h1>Candidate Messaging</h1>

    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Campaign:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblCampaignName" runat="server"></asp:Literal>
        </div>
        <div style="float: right">
            <asp:Button ID="bttnBackToCampaignDetails" runat="server" Text="Return To Campaign Details" OnClick="bttnBackToCampaignDetails_Click" CssClass="myButton" CausesValidation="False" />
        </div>
    </div>

    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 10px;">

        <table style="width: 100%;">
            <tr>
                <td>Message Type</td>
                <td>
                    <asp:DropDownList ID="listMessageType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listMessageType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="1">Email + SMS</asp:ListItem>
                        <asp:ListItem Value="2">Email Only</asp:ListItem>
                        <asp:ListItem Value="3">SMS Only</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Message Receipients</td>
                <td>
                    <div style="float: left">
                    <asp:DropDownList ID="listReceipients" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listReceipients_SelectedIndexChanged" ClientIDMode="Static">
                        <asp:ListItem Selected="True" Value="1">All Candidates In The Campaign</asp:ListItem>
                        <asp:ListItem Value="2">Scheduled Candidates In The Campaign</asp:ListItem>
                        <asp:ListItem Value="3">Unscheduled Candidates In The Campaign</asp:ListItem>
                        <asp:ListItem Value="4">Tested Candidates In The Campaign</asp:ListItem>
                        <asp:ListItem Value="5">Untested Candidates In The Campaign</asp:ListItem>
                    </asp:DropDownList></div>
                    <div style="float: left; margin-left: 10px; padding-top: 5px;">
                        <asp:Label ID="lblCandidateCount" runat="server" style="font-weight: 700"></asp:Label></div>
                </td>
            </tr>
            <tr runat="server" id="trCenter" visible="False">
                <td>Test Center</td>
                <td>
                    <asp:DropDownList ID="listCenters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listCenters_SelectedIndexChanged" DataTextField="CenterName" DataValueField="CenterId">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="Formerror" ErrorMessage="RequiredFieldValidator" ControlToValidate="listCenters">Select a center!</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr runat="server" id="trSessions" visible="False">
                <td style="vertical-align: top;">Sessions</td>
                <td>
                    <div style="width: 600px; height: 80px; overflow: auto; padding: 5px; border: 1px solid #ccc; float: left">
                        <asp:CheckBoxList ID="chkListSessions" runat="server" CellPadding="2" CellSpacing="2" ClientIDMode="Static">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:CheckBoxList>

                    </div>
                    <div style="float: left;">
                        <asp:CustomValidator ID="CustomValidator" runat="server" ErrorMessage="Select a session!" ClientValidationFunction="validateCheckBoxList" CssClass="Formerror"></asp:CustomValidator></div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="trSmsHeader" runat="server">
                <td style="width: 150px; vertical-align: top;">SMS Header</td>
                <td>
                    <asp:TextBox ID="txtSmsHeader" runat="server">DRAGNET</asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtSmsHeader" CssClass="Formerror" Display="Dynamic"
                        ErrorMessage="Enter message header"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtSmsHeader" CssClass="Formerror" Display="Dynamic"
                        ErrorMessage="Header should be a word between 3 and 11 characters"
                        ValidationExpression="[a-zA-Z0-9]{3,11}"></asp:RegularExpressionValidator>

                </td>
            </tr>
            <tr id="trSms" runat="server">
                <td style="width: 150px; vertical-align: top;">SMS Message</td>
                <td>
                    <div style="float: left;">
                    <div>
                        <asp:TextBox ID="txtSms" runat="server" Height="60px" Width="350px" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div style="padding-left: 5px;" id="lblCount">
                        160 characters remaining
                    </div></div>     <div style="float: left; border: 1px solid #f0f0f0; padding: 5px; vertical-align: top;">

                        <asp:Literal ID="lblSmsPlaceholders" runat="server">
                                          <strong>Allowed Placeholders</strong><br />
                               
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />
                        </asp:Literal>
                    </div>

                </td>
            </tr>
            <tr id="trEmailSubject" runat="server">
                <td style="width: 150px; vertical-align: top;">Email Subject</td>
                <td>
                    <asp:TextBox ID="txtEmailSubject" runat="server" Width="490px"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="txtEmailSubject" CssClass="Formerror" Display="Dynamic"
                        ErrorMessage="Enter email subject"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="trEmail" runat="server">
                <td style="width: 150px; vertical-align: top;">Email Message</td>
                <td>
                    <div style="float: left;">
                        <telerik:RadEditor ID="editor" runat="server" EditModes="Design, Preview"
                            Skin="Windows7" Height="200px" Width="700px" OnClientLoad="OnClientLoad">
                            <ImageManager DeletePaths="~/img" UploadPaths="~/img" ViewPaths="~/img" />
                            <Tools>
                                <telerik:EditorToolGroup>
                                    <telerik:EditorTool Name="Cut" /> 
                                    <telerik:EditorTool Name="Copy" />
                                    <telerik:EditorTool Name="Bold" />
                                    <telerik:EditorTool Name="Underline" />
                                    <telerik:EditorTool Name="Italic" />
                                    <telerik:EditorTool Name="InsertSymbol" />
                                    <telerik:EditorTool Name="InsertUnorderedList" />
                                    <telerik:EditorTool Name="InsertOrderedList" />
                                    <telerik:EditorTool Name="JustifyCenter" />
                                    <telerik:EditorTool Name="JustifyFull" />
                                    <telerik:EditorTool Name="JustifyLeft" />
                                    <telerik:EditorTool Name="JustifyRight" />
                                    <telerik:EditorTool Name="Indent" />
                                </telerik:EditorToolGroup>
                            </Tools>
                            <Content>
                            </Content>
                        </telerik:RadEditor>
                    </div>
                    <div style="float: left; border: 1px solid #f0f0f0; padding: 5px; vertical-align: top;">

                        <asp:Literal ID="lblPlaceHolders" runat="server">
                                          <strong>Allowed Placeholders</strong><br />
                                    [[NAME]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />
                        </asp:Literal>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="bttnSend" runat="server" Text="Send Message" OnClick="bttnSend_Click" />
                         &nbsp; <label for="chkPreview"><input type="checkbox" value="1" id="chkPreview" runat="server" ClientIDMode="Static"/>Preview Message</label>
                </td>
            </tr>

        </table>

    </div>
       <div id="divPreview" style="padding: 10px; border: 1px solid #ccc; background-color:#f9f9f9; width: 684px; min-height: 500px;margin-left: 162px; display:none;">
        
        
    </div>
    <asp:HiddenField ID="hidId" runat="server" />
</asp:Content>
