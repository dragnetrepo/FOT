<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Context" %>
<%@ Import Namespace="Fot.Admin.Infrastructure" %>
<%@ Import Namespace="Fot.Admin.Models" %>

<script type="text/C#" runat="server">
    

    public void Page_Load(object s, System.EventArgs e)
    {

        string id = Request.QueryString["id"];


        byte[] imageBytes = GetImage(id);

        Response.Buffer = true;
        Response.ContentType = "image/jpeg";
        Response.BinaryWrite(imageBytes);
        Response.Flush();

    }


    private byte[] GetImage(string canonicalId)
    {
        byte[] imageBytes = null;

        var context = ContextManager.AsSingleton<FotContext>();

        var entry = context.CampaignEntries.Where(x => x.Candidate.ClientUniqueID == canonicalId)
                           .Select(x => new { EntryId = x.EntryId, CandidateId = x.Candidate.CandidateId, CampaignId = x.CampaignId })
                           .FirstOrDefault();
        if (entry != null)
        {
            var campaignFolder = UrlMapper.RootPhotosDirectory + entry.CampaignId.ToString();

            var imageUrl = Path.Combine(campaignFolder,
                string.Format("{0}_{1}.jpg", entry.CandidateId, entry.EntryId));

            if (File.Exists(HttpContext.Current.Server.MapPath(imageUrl)))
            {
                imageBytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageUrl));
            }
            else
            {
                var photoUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/ext"), "no_pic_captured.jpg");

                imageBytes = File.ReadAllBytes(photoUrl);
            }
        }
        else
        {
            var photoUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/ext"), "invalid_candidate_id.jpg");

            imageBytes = File.ReadAllBytes(photoUrl);
        }

        return imageBytes;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



</script>