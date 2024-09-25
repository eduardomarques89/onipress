using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace global
{
    public partial class identidade : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Verifica se é o primeiro carregamento da página
            {
                GerarQrCode(); // Gera o QR Code
            }
        }

        protected void EnviarFotos_Click(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)
            {
                HttpPostedFile file = fileUpload1.PostedFile;

                byte[] fileData = null;
                using (BinaryReader reader = new BinaryReader(file.InputStream))
                {
                    fileData = reader.ReadBytes(file.ContentLength);
                }

                SalvarImagemNoBanco(fileData);
            }
        }

        private void SalvarImagemNoBanco(byte[] imageData)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                DbCommand insertCommand = db.GetSqlStringCommand(
                    "INSERT INTO OniPres_Acesso (link_acesso) VALUES (@Imagem)");

                db.AddInParameter(insertCommand, "@Imagem", DbType.Binary, imageData);
                db.ExecuteNonQuery(insertCommand);

                lblReposta.Text = "Imagem salva com sucesso!";
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao cadastrar dados no Banco Interno! Erro: " + ex.Message;
            }
        }

        private void GerarQrCode()
        {
            string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=api";
            imgQrCode.ImageUrl = qrCodeUrl;
        }
    }
}
