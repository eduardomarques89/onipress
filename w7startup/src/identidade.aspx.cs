﻿using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using pix_dynamic_payload_generator.net.Models;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace global
{
    public partial class identidade : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["codigo"];

                if (!string.IsNullOrEmpty(token))
                {
                    var query = @"SELECT TOP 1 
                                    a.name AS nome, 
                                    p.cpf AS cpf, 
                                    p.celular AS celular, 
                                    a.data_initial AS datai, 
                                    a.data_final AS dataf
                                FROM OniPres_pessoa p
                                JOIN Onipres_Disparos d ON p.celular = d.telefone
                                LEFT JOIN OniPres_Acesso a ON a.id_uduario = p.id
                                WHERE d.token = @token
                                ORDER BY a.data_final DESC;";

                    using (var connection = DatabaseFactory.CreateDatabase("ConnectionString").CreateConnection())
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "@token";
                        parameter.Value = token;
                        command.Parameters.Add(parameter);

                        connection.Open();
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["nome"] != DBNull.Value ? reader["nome"].ToString() : string.Empty;
                                txtCpf.Text = reader["cpf"] != DBNull.Value ? reader["cpf"].ToString() : string.Empty;
                                txtTell.Text = reader["celular"] != DBNull.Value ? reader["celular"].ToString() : string.Empty;

                                txtDataInicial.Text = reader["datai"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["datai"]).ToString("yyyy-MM-dd")
                                    : string.Empty;

                                txtDataFinal.Text = reader["dataf"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["dataf"]).ToString("yyyy-MM-dd")
                                    : string.Empty;
                            }
                            else
                            {
                                lblErro.Text = "Não encontrado";
                            }

                        }
                    }

                    GerarQrCode_Click();
                }
                else
                {
                    lblErro.Text = "Token não informado";
                }
            }
        }

        protected void EnviarFotos_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string folderPath = Server.MapPath("~/upload/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, fileName);
                    fileUpload.SaveAs(filePath);

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

        protected async void CriarImagemAPI()
        {

            string ip = Session["ip"].ToString();

            if (string.IsNullOrEmpty(ip))
            {
                lblErro.Text = "Dispositivo não encontrado.";
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string loginUrl = "http://" + ip + ":8113/login.fcgi";

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
                    string session = loginResponseData.session;

                    string host = "http://" + ip + ":8113/";
                    string userId = Session["UserId"]?.ToString();
                    string apiUrl = $"{host}/user_set_image.fcgi?user_id={userId}&match=1&timestamp={userId}&session={session}";

                    using (var multipartFormDataContent = new MultipartFormDataContent())
                    {
                        var imagePath = Server.MapPath("~/upload/" + fileUpload.FileName);
                        var imageContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imagePath));
                        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                        multipartFormDataContent.Add(imageContent, "image", fileUpload.FileName);

                        HttpResponseMessage apiResponse = await client.PostAsync(apiUrl, multipartFormDataContent);
                        apiResponse.EnsureSuccessStatusCode();

                        string apiResponseBody = await apiResponse.Content.ReadAsStringAsync();
                        lblErro.Text = "Imagem enviada com sucesso: " + apiResponseBody;
                    }
                }
                catch (Exception ex)
                {
                    lblErro.Text = "Erro ao enviar dados para a API: " + ex.Message;
                }
            }
        }

        protected async void GerarQrCode_Click()
        {
            try
            {
                string data = Request.QueryString["codigo"];
                string qrCodeUrlEntrada = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + data + "entrada";
                string qrCodeUrlSaida = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + data + "saida";

                imgQrCode.ImageUrl = qrCodeUrlEntrada;

                Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                string IdUser = Request.QueryString["user"];
                int idUsuario = Convert.ToInt16(Session["Id"]); //id do usuario bd

                DbCommand insertCommand = db.GetSqlStringCommand(
                    "UPDATE OniPres_Acesso SET qrcode_entrada = @entrada, qrcode_saida = @saida WHERE id_uduario = @id");

                db.AddInParameter(insertCommand, "@entrada", DbType.String, qrCodeUrlEntrada);
                db.AddInParameter(insertCommand, "@saida", DbType.String, qrCodeUrlSaida);
                db.AddInParameter(insertCommand, "@id", DbType.String, idUsuario);

                db.ExecuteNonQuery(insertCommand);

                string ip = Session["ip"].ToString();

                if (string.IsNullOrEmpty(ip))
                {
                    lblErro.Text = "Dispositivo não encontrado.";
                    return;
                }

                using (HttpClient client = new HttpClient())
                {
                    string loginUrl = "http://" + ip + ":8113/login.fcgi";
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

                    string host = "http://" + ip + ":8113/";
                    string apiUrl = $"{host}/create_objects.fcgi?session={session}";

                    var requestBody = new
                    {
                        @object = "qrcodes",
                        values = new[]
                        {
                            new
                            {
                                value = data + "entrada" + IdUser,
                                user_id = Convert.ToInt64(IdUser)
                            }
                        }
                    };

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