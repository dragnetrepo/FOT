using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class CampaignStats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForUpdate(id);
                }
                else
                {
                    Response.Redirect(UrlMapper.ManageCampaigns);
                }

            }
        }


        private void LoadForUpdate(int id)
        {
            var item = new CampaignService().GetCampaign(id);

            if (item != null)
            {
                lblCampaignName.Text = item.CampaignName;
                hidId.Value = id.ToString();

            }
        }


        protected void bttnBackToCampaignDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignDetails + "?id=" + hidId.Value);
        }


        public List<CampaignStat> GetStats()
        {

            int id = Int32.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;


            var list = ctx.CampaignEntries.Where(x => x.CampaignId == id && x.Scheduled)
      .Select(x => new { x.EntryId, x.TestSession.Center.CenterName, x.TestSession.Center.Location.LocationName, x.Tested }).ToList();

            var states = list.Select(x => x.LocationName).Distinct().ToList();


            var stats = new List<CampaignStat>();


            foreach (var state in states)
            {
                var stat = new CampaignStat();
                stat.State = state;

                var tempList = list.Where(x => x.LocationName == state).ToList();

                var tempCenters = tempList.Select(x => x.CenterName).Distinct().ToList();


                stat.Scheduled = tempList.Count();
                stat.Tested = tempList.Count(x => x.Tested);


                foreach (var center in tempCenters)
                {
                    var locationStat = new LocationStat();

                    locationStat.CenterName = center;

                    locationStat.Scheduled = tempList.Count(x => x.CenterName == center);

                    locationStat.Tested = tempList.Count(x => x.CenterName == center && x.Tested);

                    stat.Stats.Add(locationStat);
                }

                stats.Add(stat);
            }

            return stats;
        }
    }


    public class CampaignStat
    {
        public string State { get; set; }

        public int Scheduled { get; set; }

        public int Tested { get; set; }

        public List<LocationStat> Stats { get; set; }


        public CampaignStat()
        {
            Stats = new List<LocationStat>();
        }

    }

    public class LocationStat
    {
        public string CenterName { get; set; }

        public int Scheduled { get; set; }

        public int Tested { get; set; }
    }
}