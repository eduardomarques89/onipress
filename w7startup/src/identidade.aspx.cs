using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace global
{
    public partial class identidade : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void EnviarFotos_Click(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fileUpload1.FileName);

                    string folderPath = Server.MapPath("~/upload/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = folderPath + fileName;
                    fileUpload1.SaveAs(filePath);

                    SalvarImagemNoBanco(filePath);

                    lblReposta.Text = "Arquivo enviado com sucesso!";
                }
                catch (Exception ex)
                {
                    lblErro.Text = "Erro ao enviar arquivo: " + ex.Message;
                }
            }
            else
            {
                lblErro.Text = "Nenhum arquivo foi selecionado.";
            }
        }

        private void SalvarImagemNoBanco(string caminhoArquivo)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                int idUsuario = Convert.ToInt32(Session["Id"]);

                DbCommand insertCommand = db.GetSqlStringCommand(
                    "Update OniPres_Acesso set link_acesso = @Imagem where id_uduario = @id");

                db.AddInParameter(insertCommand, "@Imagem", DbType.String, caminhoArquivo);
                db.AddInParameter(insertCommand, "@id", DbType.Int32, idUsuario);  

                db.ExecuteNonQuery(insertCommand);

                lblReposta.Text = "Imagem salva com sucesso!";
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao cadastrar dados no Banco Interno! Erro: " + ex.Message;
            }
        }

        protected async void GerarQrCode_Click(object sender, EventArgs e)
        {
            string data = Guid.NewGuid().ToString();

            string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + data;
            imgQrCode.ImageUrl = qrCodeUrl;

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            int idUsuario = Convert.ToInt32(Session["IdUser"]);

            DbCommand insertCommand = db.GetSqlStringCommand(
                    "Update OniPres_Acesso set qrcode = @qrcode where id_uduario = @id");

            db.AddInParameter(insertCommand, "@qrcode", DbType.String, qrCodeUrl);
            db.AddInParameter(insertCommand, "@id", DbType.Int32, idUsuario);

            db.ExecuteNonQuery(insertCommand);

            await EnviarQrCodeParaAPI(idUsuario, qrCodeUrl);
        }

        private async Task EnviarQrCodeParaAPI(int userId, string qrCodeUrl)
        {
            string session = Session["Session"].ToString();
            string host = "http://138.94.44.203:8113/";
            string apiUrl = $"{host}/create_objects.fcgi?session={session}";

            var requestBody = new
            {
                @object = "qrcodes",
                values = new[]
                {
            new
            {
                value = qrCodeUrl,
                user_id = userId
            }
        }
            };

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync();
            }
        }

    }
}
