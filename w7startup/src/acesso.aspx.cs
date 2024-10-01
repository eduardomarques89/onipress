using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace global
{
    public partial class acesso : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void EnviarDados_Click(object sender, EventArgs e)
        {
            lblErro.Text = string.Empty;
            bool allFieldsValid = ValidarCampos();

            if (!allFieldsValid)
            {
                lblErro.Text = "Preencha todos os campos corretamente.";
                return;
            }

            try
            {
                int idUsuario = Convert.ToInt32(Session["Id"]);
                string IdUser = Convert.ToString(Session["ApiResponseId"]);

                string dataInicial = txtDataInicial.Text;
                string dataFinal = txtDataFinal.Text;

                await AtualizarDadosNoBanco(idUsuario, dataInicial, dataFinal);

                int? timeZoneId = await EnviarParaAPI(IdUser);
                if (timeZoneId > 0)
                {
                    await CriarTimeSpan(dataInicial, dataFinal, timeZoneId.Value);
                    int? accessRuleId = await CriarRegraDeAcesso(IdUser);
                    if (accessRuleId.HasValue)
                    {
                        await CriarRegraDeAcessoTimeZone(accessRuleId.Value, timeZoneId.Value);
                        await CriarRegraDeAcessoPorUsuario(IdUser, accessRuleId.Value);
                    }

                    Response.Redirect("identidade.aspx", true);
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao processar dados: " + ex.Message;
            }
        }

        private bool ValidarCampos()
        {
            return !string.IsNullOrWhiteSpace(txtDataInicial.Text) &&
                   !string.IsNullOrWhiteSpace(txtHourInicial.Text) &&
                   !string.IsNullOrWhiteSpace(txtDataFinal.Text) &&
                   !string.IsNullOrWhiteSpace(txtHourFinal.Text) &&
                   ddlCompanies.SelectedIndex > 0 &&
                   ddlBlock.SelectedIndex > 0 &&
                   ddlUnity.SelectedIndex > 0;
        }

        private async Task AtualizarDadosNoBanco(int idUsuario, string dataInicial, string dataFinal)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            using (DbCommand insertCommand = db.GetSqlStringCommand(
                "UPDATE OniPres_Acesso SET data_initial = @data_i, data_final = @data_f, id_companies = @companies, id_block = @block, id_unity = @unity, id_device = @device WHERE id_uduario = @id;"))
            {
                db.AddInParameter(insertCommand, "@data_i", DbType.String, dataInicial);
                db.AddInParameter(insertCommand, "@data_f", DbType.String, dataFinal);
                db.AddInParameter(insertCommand, "@companies", DbType.String, ddlCompanies.SelectedValue);
                db.AddInParameter(insertCommand, "@block", DbType.String, ddlBlock.SelectedValue);
                db.AddInParameter(insertCommand, "@unity", DbType.String, ddlUnity.SelectedValue);
                db.AddInParameter(insertCommand, "@device", DbType.String, 0);
                db.AddInParameter(insertCommand, "@id", DbType.Int32, idUsuario);

                db.ExecuteNonQuery(insertCommand);
            }
        }

        private async Task<int?> EnviarParaAPI(string userId)
        {
            try
            {
                string loginUrl = "http://192.168.0.207:8013/login.fcgi";
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
                string session = loginResponseData?.session;

                if (string.IsNullOrEmpty(session))
                {
                    lblErro.Text = "Erro ao obter sessão da API.";
                    return null;
                }

                string apiUrl = $"http://192.168.0.207:8013/create_objects.fcgi?session={session}";
                var requestBody = new
                {
                    @object = "time_zones",
                    values = new[] { new { name = $"Entrada {userId}" } }
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

                return responseData?.id != null ? (int?)Convert.ToInt32(responseData.id) : null;
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao enviar dados para a API: " + ex.Message;
                return null;
            }
        }


        private async Task CriarTimeSpan(string start, string end, int timeZoneId)
        {
            try
            {
                string loginUrl = "http://192.168.0.207:8013/login.fcgi";
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
                string session = loginResponseData?.session;

                if (string.IsNullOrEmpty(session))
                {
                    lblErro.Text = "Erro ao obter sessão da API.";
                    return;
                }

                string apiUrl = $"http://192.168.0.207:8013/create_objects.fcgi?session={session}";
                var requestBody = new
                {
                    @object = "time_spans",
                    values = new[]
                    {
                        new
                        {
                            time_zone_id = timeZoneId,
                            start = start,
                            end = end,
                            fri = 1
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

        private async Task<int?> CriarRegraDeAcesso(string userId)
        {
            try
            {
                string loginUrl = "http://192.168.0.207:8013/login.fcgi";
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
                string session = loginResponseData?.session;

                string apiUrl = $"http://192.168.0.207:8013/create_objects.fcgi?session={session}";
                var requestBody = new
                {
                    @object = "access_rules",
                    values = new[] { new { name = $"Entrada {userId}", type = 1, priority = 0 } }
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

                return responseData?.id; // Retorna null se id for nulo
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao criar regra de acesso: " + ex.Message;
                return null;
            }
        }

        private async Task CriarRegraDeAcessoTimeZone(int accessRuleId, int timeZoneId)
        {
            try
            {
                string loginUrl = "http://192.168.0.207:8013/login.fcgi";
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
                string session = loginResponseData?.session;

                string apiUrl = $"http://192.168.0.207:8013/create_objects.fcgi?session={session}";
                var requestBody = new
                {
                    @object = "access_rules_timezones",
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

        private async Task CriarRegraDeAcessoPorUsuario(string userId, int accessRuleId)
        {
            try
            {
                string loginUrl = "http://192.168.0.207:8013/login.fcgi";
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
                string session = loginResponseData?.session;

                string apiUrl = $"http://192.168.0.207:8013/create_objects.fcgi?session={session}";
                var requestBody = new
                {
                    @object = "access_rules_users",
                    values = new[] { new { access_rule_id = accessRuleId, user_id = userId } }
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
                lblErro.Text = "Erro ao criar regra de acesso por usuário: " + ex.Message;
            }
        }
    }
}
