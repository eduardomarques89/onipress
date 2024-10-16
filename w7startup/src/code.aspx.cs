using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using QRCoder;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace global
{
    public partial class code : System.Web.UI.Page
    {
        protected void VoltarHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("homePage.aspx", false);
        }
    }
}
