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
            txtDataInicial.Text = "";
            txtHourInicial.Text = "";
            txtDataFinal.Text = "";
            txtHourFinal.Text = "";
            lblReposta.Text = "";

            bool allFieldsValid = true;

            if (string.IsNullOrEmpty(txtCompanies.Text))
            {
                lblCompanies.Text = "Preencha o local da visita";
                allFieldsValid = false;
            }
            if (string.IsNullOrEmpty(txtBlock.Text))
            {
                lblBlock.Text = "Preencha com o Bloco!";
                allFieldsValid = false;
            }
            if (string.IsNullOrEmpty(txtUnity.Text))
            {
                lblUnity.Text = "Preencha com a Unidade!";
                allFieldsValid = false;
            }

            string data_initial = txtDataInicial.Text + txtHourInicial.Text;
            string data_final = txtDataFinal.Text + txtHourFinal.Text;

            if (allFieldsValid)
            {
                try
                {
                    Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                    int idUsuario = Convert.ToInt32(Session["Id"]);
                    int idUser = Convert.ToInt32(Session["IdUser"]);
                    string session = Session["Session"].ToString();

                    DbCommand insertCommand = db.GetSqlStringCommand(
                        "UPDATE OniPres_Acesso SET data_initial = @data_i, data_final = @data_f, id_companies = @companies, id_block = @block, id_unity = @unity, id_device = @device WHERE id_uduario = @id;");

                    db.AddInParameter(insertCommand, "@data_i", DbType.String, data_initial);
                    db.AddInParameter(insertCommand, "@data_f", DbType.String, data_final);
                    db.AddInParameter(insertCommand, "@companies", DbType.String, txtCompanies.Text);
                    db.AddInParameter(insertCommand, "@block", DbType.String, txtBlock.Text);
                    db.AddInParameter(insertCommand, "@unity", DbType.String, txtUnity.Text);
                    db.AddInParameter(insertCommand, "@device", DbType.String, 0);
                    db.AddInParameter(insertCommand, "@id", DbType.Int32, idUsuario);

                    db.ExecuteNonQuery(insertCommand);

                    var timeZoneId = await EnviarParaAPI(idUser, session);

                    await CriarTimeSpan(data_initial, data_final, timeZoneId, session);

                    var accessRuleId = await CriarRegraDeAcesso(idUser);

                    await CriarRegraDeAcessoTimeZone(accessRuleId, timeZoneId);

                    await CriarRegraDeAcessoPorUsuario(idUser, accessRuleId);

                    Response.Redirect("identidade.aspx", true);
                }
                catch (Exception ex)
                {
                    lblErro.Text = "Erro ao cadastrar dados no Banco Interno! Erro: " + ex.Message;
                }

                LimparCampos();
            }
        }

        private async Task<int> EnviarParaAPI(int userId, string session)
        {
            try
            {
                string host = "http://192.168.0.204:8013/";
                string apiUrl = $"{host}/create_objects.fcgi?session={session}";

                var requestBody = new
                {
                    @object = "time_zones",
                    values = new[]
                    {
                        new
                        {
                            name = $"Entrada {userId}" 
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

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);

                return responseData.id;
            }
            catch (Exception ex)    
            {
                lblErro.Text = "Erro ao enviar dados para a API: " + ex.Message;
                return 0;
            }
        }

        private async Task CriarTimeSpan(string start, string end, int timeZoneId, string session)
        {
            try
            {
                string host = "http://192.168.0.204:8013/";
                string apiUrl = $"{host}/create_objects.fcgi?session={session}";

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
                            sun = 0,
                            mon = 0,
                            tue = 0,
                            wed = 0,
                            thu = 0,
                            fri = 1,
                            sat = 0,
                            hol1 = 0,
                            hol2 = 0,
                            hol3 = 0
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

                await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao enviar dados para a API: " + ex.Message;
            }
        }

        private async Task<int> CriarRegraDeAcesso(int userId)
        {
            string session = Session["Session"].ToString();
            string host = "http://192.168.0.204:8013/";
            string apiUrl = $"{host}/create_objects.fcgi?session={session}";

            var requestBody = new
            {
                @object = "access_rules",
                values = new[]
                {
            new
            {
                name = $"Entrada {userId}",
                type = 1,
                priority = 0
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

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);

                return responseData.id;
            }
        }

        private async Task CriarRegraDeAcessoTimeZone(int accessRuleId, int timeZoneId)
        {
            string session = Session["Session"].ToString();
            string host = "http://192.168.0.204:8013/";
            string apiUrl = $"{host}/create_objects.fcgi?session={session}";

            var requestBody = new
            {
                @object = "access_rule_time_zones",
                values = new[]
                {
            new
            {
                access_rule_id = accessRuleId,
                time_zone_id = timeZoneId
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

        private async Task CriarRegraDeAcessoPorUsuario(int userId, int accessRuleId)
        {
            string session = Session["Session"].ToString();
            string host = "http://192.168.0.204:8013/";
            string apiUrl = $"{host}/create_objects.fcgi?session={session}";

            var requestBody = new
            {
                @object = "user_access_rules",
                values = new[]
                {
            new
            {
                user_id = userId,
                access_rule_id = accessRuleId
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


        protected void LimparCampos()
        {
            txtDataInicial.Text = "";
            txtHourInicial.Text = "";
            txtDataFinal.Text = "";
            txtHourFinal.Text = "";
            txtCompanies.Text = "";
            txtBlock.Text = "";
            txtUnity.Text = "";
        }
    }
}
