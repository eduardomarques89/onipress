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
        public void Page_Load(object sender, EventArgs e)
        {
            string link = "https://api.whatsapp.com/send?phone=5519989269441&text=Ol%C3%A1%20tudo%20bem,%20estou%20localizado%20no%20condom%C3%ADnio%20c%C3%B3digo%20=%202";
            string qrCodeUrlEntrada = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data= " + link;
            imgQrCode.ImageUrl = qrCodeUrlEntrada;
        }

        protected void VoltarHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("homePage.aspx", false);
        }
    }
}