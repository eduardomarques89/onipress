using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using pix_dynamic_payload_generator.net.Models;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace global
{
    public partial class cadastrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.QueryString["codigo"];

            if (!string.IsNullOrEmpty(token))
            {
                var query = "select * from Onipres_Disparos where token = @token";

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
                            txtName.Text = reader["nome_visitante"].ToString();
                            txtCpf.Text = reader["cpf_visitante"].ToString();
                            txtTell.Text = reader["telefone_visitante"].ToString();
                        }
                        else
                        {
                            lblErro.Text = "Não encontrado";
                        }
                    }
                }
            }
            else
            {
                lblErro.Text = "Token não informado";
            }
        }

        protected async void EnviarDados_Click(object sender, EventArgs e)
        {
            lblReposta.Text = "";

            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                DbCommand insertCommand = db.GetSqlStringCommand(
                    "INSERT INTO OniPres_pessoa (nome, cpf, celular, datacriacao) " +
                    "OUTPUT INSERTED.id VALUES (@name, @cpf, @tell, GETDATE())");

                db.AddInParameter(insertCommand, "@name", DbType.String, txtName.Text);
                db.AddInParameter(insertCommand, "@cpf", DbType.String, txtCpf.Text);
                db.AddInParameter(insertCommand, "@tell", DbType.String, txtTell.Text);

                object insertedId = db.ExecuteScalar(insertCommand);

                if (insertedId != null)
                {
                    await EnviarParaAPI(txtName.Text);
                }
                else
                {
                    lblErro.Text = "Erro ao obter o ID inserido.";
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao cadastrar dados: " + ex.Message;
            }

            LimparCampos();
        }

        private async Task EnviarParaAPI(string name)
        {
            if (string.IsNullOrEmpty(Request.QueryString["codigo"]))
            {
                lblErro.Text = "Token não informado.";
                return;
            }

            string token = Request.QueryString["codigo"];

            try
            {
                string ip = await ObterIpPorTelefone(Request.QueryString["telefone_visitante"]);

                if (string.IsNullOrEmpty(ip))
                {
                    lblErro.Text = "Dispositivo não encontrado.";
                    return;
                }

                using (HttpClient client = new HttpClient())
                {
                    string loginUrl = $"http://{ip}:8013/login";

                    var loginBody = new { login = "admin", password = "admin" };
                    var loginContent = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(loginBody),
                        Encoding.UTF8, "application/json"
                    );

                    HttpResponseMessage loginResponse = await client.PostAsync(loginUrl, loginContent);
                    loginResponse.EnsureSuccessStatusCode();

                    var loginResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(
                        await loginResponse.Content.ReadAsStringAsync()
                    );

                    string session = loginResponseData?.session;
                    if (string.IsNullOrEmpty(session))
                    {
                        lblErro.Text = "Sessão não obtida.";
                        return;
                    }

                    string apiUrl = $"http://{ip}:8013/create_objects.fcgi?session={session}";

                    var requestBody = new
                    {
                        @object = "users",
                        values = new[]
                        {
                            new { name = name, registration = "", password = "", salt = "" }
                        }
                    };

                    var content = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                        Encoding.UTF8, "application/json"
                    );

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(
                        await response.Content.ReadAsStringAsync()
                    );

                    int id = responseObject?.ids?[0] ?? 0;
                    if (id == 0)
                    {
                        lblErro.Text = "ID do usuário não encontrado.";
                        return;
                    }

                    Session["UserId"] = id;
                    Response.Redirect($"acesso.aspx?codigo={token}", false);
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao enviar dados para a API: " + ex.Message;
            }
        }

        private async Task<string> ObterIpPorTelefone(string telefone)
        {
            string query = @"
                SELECT d.numero_ip AS ip 
                FROM OniPres_empresa e 
                JOIN OniPres_dispositivo d ON e.id = d.empresa 
                JOIN OniPres_pessoa p ON e.id = p.empresa 
                WHERE p.celular = @telefone";

            using (var connection = DatabaseFactory.CreateDatabase("ConnectionString").CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                command.Parameters.Add(new SqlParameter("@telefone", SqlDbType.VarChar) { Value = telefone });

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return reader["ip"].ToString();
                    }
                }
            }

            return null;
        }

        protected void LimparCampos()
        {
            txtName.Text = "";
            txtCpf.Text = "";
            txtTell.Text = "";
        }
    }
}
