using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using pix_dynamic_payload_generator.net.Models;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace global
{
    public partial class acessos : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();
        private string apiSession;

        protected async void EnviarDados_Click(object sender, EventArgs e)
        {
            lblErro.Text = string.Empty;

            if (!ValidarCampos())
            {
                lblErro.Text = "Preencha todos os campos corretamente.";
                return;
            }

            try
            {
                int idUsuario = Convert.ToInt32(Session["Id"]);
                string UserId = Session["UserId"]?.ToString();
                string telefone = Session["telefone"]?.ToString();

                if (string.IsNullOrEmpty(UserId))
                {
                    lblErro.Text = "Id do usuário inválida.";
                    return;
                }

                string dataInicial = txtDataInicial.Text;
                string dataFinal = txtDataFinal.Text;

                await AtualizarDadosNoBanco(idUsuario, dataInicial, dataFinal);

                if (string.IsNullOrEmpty(apiSession))
                {
                    apiSession = await ObterSessaoApi();
                    if (string.IsNullOrEmpty(apiSession))
                    {
                        lblErro.Text = "Erro ao obter sessão da API.";
                        return;
                    }
                }

                string token = Request.QueryString["codigo"];

                if (!string.IsNullOrEmpty(token))
                {
                    string codigo = Request.QueryString["codigo"];
                    string idUser = Session["UserId"].ToString();
                    string link = "https://w7onipress.azurewebsites.net/src/identidade.aspx?codigo=" + codigo + "&user=" + idUser;

                    Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                    using (DbCommand insertCommandDisparos = db.GetSqlStringCommand(
                        "INSERT INTO Onipres_Disparos (telefone, mensagem, data, status, token) values (@telefone, @mensagem, GETDATE(), @status, @token)"))
                    {
                        db.AddInParameter(insertCommandDisparos, "@telefone", DbType.String, telefone);
                        db.AddInParameter(insertCommandDisparos, "@mensagem", DbType.String, "Olá, o seu acesso foi liberado! Acesse o link para liberar a cancela " + link);
                        db.AddInParameter(insertCommandDisparos, "@status", DbType.String, "Enviando");
                        db.AddInParameter(insertCommandDisparos, "@token", DbType.String, token);

                        await Task.Run(() => db.ExecuteNonQuery(insertCommandDisparos));
                    }

                    int? timeZoneId = await CriarTimeZone(UserId);

                    if (timeZoneId.HasValue)
                    {
                        await CriarTimeSpan(timeZoneId.Value);

                        int? accessRuleId = await CriarRegraDeAcesso(UserId);

                        if (accessRuleId.HasValue)
                        {
                            await CriarRegraDeAcessoTimeZone(accessRuleId.Value, timeZoneId.Value);
                            await CriarRegraDeAcessoPorUsuario(UserId, accessRuleId.Value);

                            lblReposta.Text = "Visitante criado com sucesso!";;

                            string cod = Request.QueryString["codigo"];
                            string User = Session["UserId"].ToString();
                            Response.Redirect($"identidade.aspx?codigo={cod}&user={User}", false);
                        }
                    }
                    else
                    {
                        lblErro.Text = "Erro ao obter o ID da API.";
                    }
                }
                else
                {
                    lblErro.Text = "Token não informado";
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao processar dados: " + ex.Message;
            }
        }

        private bool ValidarCampos()
        {
            DateTime parsedDate;
            return !string.IsNullOrWhiteSpace(txtDataInicial.Text) &&
                   DateTime.TryParse(txtDataInicial.Text, out parsedDate) &&
                   !string.IsNullOrWhiteSpace(txtHourInicial.Text) &&
                   DateTime.TryParse(txtHourFinal.Text, out parsedDate) &&
                   !string.IsNullOrWhiteSpace(txtDataFinal.Text) &&
                   DateTime.TryParse(txtDataFinal.Text, out parsedDate);
        }

        private async Task AtualizarDadosNoBanco(int idUsuario, string dataInicial, string dataFinal)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            using (DbCommand insertCommand = db.GetSqlStringCommand(
                "UPDATE OniPres_Acesso SET data_initial = @data_i, data_final = @data_f WHERE id_uduario = @id;"))
            {
                db.AddInParameter(insertCommand, "@data_i", DbType.String, dataInicial);
                db.AddInParameter(insertCommand, "@data_f", DbType.String, dataFinal);
                db.AddInParameter(insertCommand, "@device", DbType.String, 0);
                db.AddInParameter(insertCommand, "@id", DbType.Int32, idUsuario);

                await Task.Run(() => db.ExecuteNonQuery(insertCommand));
            }
        }

        private async Task<string> ObterSessaoApi()
        {
            string ip = Session["ip"].ToString();

            try
            {
                string loginUrl = "http://" + ip + ":8113/login.fcgi";
                var loginBody = new { login = "admin", password = "admin" };

                var loginContent = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(loginBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage loginResponse = await client.PostAsync(loginUrl, loginContent);
                loginResponse.EnsureSuccessStatusCode();

                string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
                var loginResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(loginResponseBody);
                return loginResponseData?.session;
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao obter sessão da API: " + ex.Message;
                return null;
            }
        }

        private async Task<int?> CriarTimeZone(string UserId)
        {
            string ip = Session["ip"].ToString();

            try
            {
                string apiUrl = $"http://{ip}:8113/create_objects.fcgi?session={apiSession}";
                var requestBody = new
                {
                    @object = "time_zones",
                    values = new[] { new { name = $"Entrada {UserId}" } }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
                
                return responseData?.ids != null && responseData.ids.Count > 0
                    ? (int?)Convert.ToInt32(responseData.ids[0])
                    : null;
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao enviar dados para a API: " + ex.Message;
                return null;
            }
        }

        private async Task CriarTimeSpan(int timeZoneId)
        {
            string ip = Session["ip"].ToString();

            if (string.IsNullOrEmpty(ip))
            {
                lblErro.Text = "Dispositivo não encontrado.";
                return;
            }

            try
            {
                TimeSpan horaInicial, horaFinal;
                
                if (!TimeSpan.TryParse(txtHourInicial.Text, out horaInicial))
                {
                    lblErro.Text = "Hora inicial não é válida.";
                    return;
                }

                if (!TimeSpan.TryParse(txtHourFinal.Text, out horaFinal))
                {
                    lblErro.Text = "Hora final não é válida.";
                    return;
                }

                int resultStart = (int)horaInicial.TotalSeconds;
                int resultEnd = (int)horaFinal.TotalSeconds;

                if (!DateTime.TryParse(txtDataInicial.Text, out DateTime dataInicial))
                {
                    lblErro.Text = "Data inicial não é válida.";
                    return;
                }

                if (!DateTime.TryParse(txtDataFinal.Text, out DateTime dataFinal))
                {
                    lblErro.Text = "Data final não é válida.";
                    return;
                }

                DayOfWeek diaSemanaInicial = dataInicial.DayOfWeek;
                DayOfWeek diaSemanaFinal = dataFinal.DayOfWeek;

                int sun = 0, mon = 0, tue = 0, wed = 0, thu = 0, fri = 0, sat = 0;

                switch (diaSemanaInicial)
                {
                    case DayOfWeek.Sunday:
                        sun = 1;
                        break;
                    case DayOfWeek.Monday:
                        mon = 1;
                        break;
                    case DayOfWeek.Tuesday:
                        tue = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        wed = 1;
                        break;
                    case DayOfWeek.Thursday:
                        thu = 1;
                        break;
                    case DayOfWeek.Friday:
                        fri = 1;
                        break;
                    case DayOfWeek.Saturday:
                        sat = 1;
                        break;
                }

                string apiUrl = $"http://{ip}:8113/create_objects.fcgi?session={apiSession}";
                var requestBody = new
                {
                    @object = "time_spans",
                    values = new[]
                    {
                        new
                        {
                            time_zone_id = timeZoneId,
                            start = resultStart,
                            end = resultEnd,
                            sun = sun,
                            mon = mon,
                            tue = tue,
                            wed = wed,
                            thu = thu,
                            fri = fri,
                            sat = sat,
                        }
                    }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao criar time span: " + ex.Message;
            }
        }

        private async Task<int?> CriarRegraDeAcesso(string UserId)
        {
            string ip = Session["ip"].ToString();

            try
            {
                string apiUrl = $"http://{ip}:8113/create_objects.fcgi?session={apiSession}";
                var requestBody = new
                {
                    @object = "access_rules",
                    values = new[] { new { name = $"Entrada {UserId}", type = 1, priority = 0 } }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);

                return responseData?.ids != null && responseData.ids.Count > 0
                ? (int?)Convert.ToInt32(responseData.ids[0])
                : null;

            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao criar regra de acesso: " + ex.Message;
                return null;
            }
        }

        private async Task CriarRegraDeAcessoTimeZone(int accessRuleId, int timeZoneId)
        {
            string ip = Session["ip"].ToString();

            try
            {
                string apiUrl = $"http://{ip}:8113/create_objects.fcgi?session={apiSession}";
                var requestBody = new
                {
                    @object = "access_rule_time_zones",
                    values = new[] { new { access_rule_id = accessRuleId, time_zone_id = timeZoneId } }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao criar regra de acesso para time zone: " + ex.Message;
            }
        }

        private async Task CriarRegraDeAcessoPorUsuario(string UserId, int accessRuleId)
        {
            string ip = Session["ip"].ToString();

            try
            {
                string apiUrl = $"http://{ip}:8113/create_objects.fcgi?session={apiSession}";
                var requestBody = new
                {
                    @object = "user_access_rules",
                    values = new[] { new { user_id = Convert.ToInt32(UserId), access_rule_id = accessRuleId } }
                };

                var content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao associar regra de acesso ao usuário: " + ex.Message;
            }
        }
    }
}
