using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace global
{
    public partial class homePage : System.Web.UI.Page
    {
        protected void Morador_Click(object sender, EventArgs e)
        {
            Response.Redirect("code.aspx", true);
        }
        protected void Visitante_Click(object sender, EventArgs e)
        {
            Response.Redirect("code.aspx", true);
        }
        protected void PS_Click(object sender, EventArgs e)
        {
            Response.Redirect("code.aspx", true);
        }

        protected void faq_Click(object sender, EventArgs e)
        {
            Response.Redirect("code.aspx", true);
        }
    }
}
