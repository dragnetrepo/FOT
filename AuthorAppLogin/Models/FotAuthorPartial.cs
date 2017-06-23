using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorApp.Models
{
    public partial class FotAuthorContext
    {
        public FotAuthorContext()
            : base(@"metadata=res://*/Models.FotAuthor.csdl|res://*/Models.FotAuthor.ssdl|res://*/Models.FotAuthor.msl;provider=System.Data.SqlServerCe.4.0;provider connection string=""data source=|DataDirectory|\Data\FotAuthor.sdf;Password=EnterGodMode247;""")
        {

        }
    }
}
