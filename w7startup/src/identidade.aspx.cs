using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

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

                    string filePath = Path.Combine(folderPath, fileName);
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

                //int idUsuario = Convert.ToInt32(Session["Id"]);
                int idUsuario = 47; // Exemplo de ID fixo, ajustar conforme necessário

                DbCommand insertCommand = db.GetSqlStringCommand(
                    "UPDATE OniPres_Acesso SET link_acesso = @Imagem WHERE id_uduario = @id");

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
            try
            {
                // Geração do QR Code
                string data = Guid.NewGuid().ToString();
                string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + data;
                imgQrCode.ImageUrl = qrCodeUrl;

                Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                string IdUser = Convert.ToString(Session["ApiResponseId"]);
                int idUsuario = Convert.ToInt16(Session["Id"]);

                // Salvando o QR Code no banco
                DbCommand insertCommand = db.GetSqlStringCommand(
                    "UPDATE OniPres_Acesso SET qrcode = @qrcode WHERE id_uduario = @id");

                db.AddInParameter(insertCommand, "@qrcode", DbType.String, qrCodeUrl);
                db.AddInParameter(insertCommand, "@id", DbType.String, idUsuario);

                db.ExecuteNonQuery(insertCommand);

                // Login na API
                using (HttpClient client = new HttpClient())
                {
                    string loginUrl = "http://192.168.0.204:8013/login.fcgi";
                    var loginBody = new
                    {
                        login = "admin",
                        password = "admin"
                    };

                    var loginContent = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(loginBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    HttpResponseMessage loginResponse = await client.PostAsync(loginUrl, loginContent);
                    loginResponse.EnsureSuccessStatusCode();

                    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
                    var loginResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(loginResponseBody);
                    string session = loginResponseData?.session;

                    if (string.IsNullOrEmpty(session))
                    {
                        lblErro.Text = "Erro ao obter sessão da API.";
                        return;
                    }

                    // Montando e enviando a requisição para a API
                    string host = "http://192.168.0.204:8013/";
                    string apiUrl = $"{host}/create_objects.fcgi?session={session}";

                    var requestBody = new
                    {
                        @object = "qrcodes",
                        values = new[]
                        {
                            new
                            {
                                value = qrCodeUrl,
                                user_id = Convert.ToInt64(IdUser)
                            }
                        }
                    };

                    // Convertendo o requestBody para JSON e imprimindo para debug
                    string requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                    Console.WriteLine("Request Body: " + requestBodyJson);

                    var content = new StringContent(
                        requestBodyJson,
                        Encoding.UTF8,
                        "application/json"
                    );

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Erro na solicitação: " + response.StatusCode + " - " + responseContent);
                        lblErro.Text = "Erro na solicitação: " + responseContent;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro durante a geração do QR Code ou envio para a API: " + ex.Message;
            }
        }
    }
}
