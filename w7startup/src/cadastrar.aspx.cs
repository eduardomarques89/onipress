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

namespace global
{
    public partial class cadastrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.QueryString["codigo"];
            if (string.IsNullOrEmpty(token))
            {
                lblErro.Text = "Token não informado.";
                return;
            }

            CarregarDadosVisitante(token);
        }

        private void CarregarDadosVisitante(string token)
        {
            string query = "SELECT * FROM Onipres_Disparos WHERE token = @token";

            using (var connection = DatabaseFactory.CreateDatabase("ConnectionString").CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@token", SqlDbType.VarChar) { Value = token });

                connection.Open();
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtName.Text = reader["nome_visitante"]?.ToString() ?? string.Empty;
                        txtCpf.Text = reader["cpf_visitante"]?.ToString() ?? string.Empty;
                        txtTell.Text = reader["telefone_visitante"]?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        lblErro.Text = "Visitante não encontrado.";
                    }
                }
            }
        }

        protected async void EnviarDados_Click(object sender, EventArgs e)
        {
            lblReposta.Text = "";
            try
            {
                string token = Request.QueryString["codigo"];
                string ip = await ObterIpPorTelefone(txtTell.Text);
                Session["ip"] = ip;

                int insertedId = await InserirPessoaNoBancoAsync();
                Session["telefone"] = txtTell.Text;
                Session["Id"] = insertedId;

                if (insertedId > 0)
                {
                    await EnviarParaAPI(txtName.Text, insertedId);
                    Response.Redirect($"acesso.aspx?codigo={token}", false); 
                }
                else
                {
                    lblErro.Text = "Erro ao inserir registro no banco.";
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao cadastrar dados: " + ex.Message;
            }
            finally
            {
                LimparCampos();  
            }
        }

        private async Task<int> InserirPessoaNoBancoAsync()
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            var command = db.GetSqlStringCommand(
                "INSERT INTO OniPres_pessoa (nome, cpf, celular, datacriacao) " +
                "OUTPUT INSERTED.id VALUES (@name, @cpf, @tell, GETDATE())"
            );

            db.AddInParameter(command, "@name", DbType.String, txtName.Text ?? string.Empty);
            db.AddInParameter(command, "@cpf", DbType.String, txtCpf.Text ?? string.Empty);
            db.AddInParameter(command, "@tell", DbType.String, txtTell.Text ?? string.Empty);

            int pessoaId = await Task.Run(() =>
            {
                object result = db.ExecuteScalar(command);
                return result != null ? Convert.ToInt32(result) : 0;
            });

            int acessoId = await InserirAcesso(pessoaId);
            Session["acessoId"] = acessoId;

            return pessoaId;
        }

        private async Task<int> InserirAcesso(int pessoaId)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            var command = db.GetSqlStringCommand(
                "INSERT INTO OniPres_Acesso (name, id_uduario) VALUES (@name, @id); " +
                "SELECT CAST(scope_identity() AS int);"
            );

            db.AddInParameter(command, "@name", DbType.String, txtName.Text ?? string.Empty);
            db.AddInParameter(command, "@id", DbType.Int32, pessoaId);

            int acessoId = await Task.Run(() =>
            {
                object result = db.ExecuteScalar(command);
                return result != null ? Convert.ToInt32(result) : 0;
            });

            return acessoId;
        }

        private async Task EnviarParaAPI(string name, int pessoaId)
        {
            if (string.IsNullOrEmpty(Session["ip"].ToString()))
            {
                lblErro.Text = "Dispositivo não encontrado.";
                return;
            }

            try
            {
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(60);

                    string session = await ObterSessaoAsync(client, Session["ip"].ToString());
                    if (string.IsNullOrEmpty(session))
                    {
                        lblErro.Text = "Sessão não obtida.";
                        return;
                    }
                    await CriarUsuarioNaAPI(client, Session["ip"].ToString(), session, name);
                    try
                    {
                        string id = Session["UserId"]?.ToString();
                        if (string.IsNullOrEmpty(id))
                        {
                            lblErro.Text = "ID de usuário não encontrado na sessão.";
                            return;
                        }

                        Response.Redirect($"acesso.aspx?codigo=" + Request.QueryString["codigo"].ToString() +"", false);
                    }
                    catch (Exception ex)
                    {
                        lblErro.Text = $"Erro ao gerar link: {ex.Message}.";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                lblErro.Text = $"Erro de conexão: {ex.Message}. Verifique se a API está online.";
            }
            catch (TaskCanceledException)
            {
                lblErro.Text = "Timeout: A API demorou muito para responder.";
            }
            catch (Exception ex)
            {
                lblErro.Text = $"Erro inesperado: {ex.Message}";
            }
        }

        private async Task<string> ObterSessaoAsync(HttpClient client, string ip)
        {
            string loginUrl = $"http://{ip}:8113/login.fcgi";
            var loginBody = new { login = "admin", password = "admin" };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(loginBody),
                Encoding.UTF8, "application/json"
            );

            HttpResponseMessage response = await client.PostAsync(loginUrl, content);
            response.EnsureSuccessStatusCode();

            dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(
                await response.Content.ReadAsStringAsync()
            );

            return responseData?.session;
        }

        private async Task CriarUsuarioNaAPI(HttpClient client, string ip, string session, string name)
        {
            string apiUrl = $"http://{ip}:8113/create_objects.fcgi?session={session}";
            var requestBody = new
            {
                @object = "users",
                values = new[] { new { name = name, registration = "", password = "", salt = "" } }
            };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8, "application/json"
            );

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(
                await response.Content.ReadAsStringAsync()
            );

            int id = responseObject?.ids?[0] ?? 0;
            if (id == 0)
            {
                lblErro.Text = "ID do usuário não encontrado.";
                throw new Exception("Falha ao criar usuário na API.");
            }

            Session["UserId"] = id;
        }

        private async Task<string> ObterIpPorTelefone(string telefone)
        {
            string query = @"
                select distinct numero_ip from OniPres_disparos d join OniPres_dispositivo di on di.empresa = d.idempresa where token = @token";

            using (var connection = DatabaseFactory.CreateDatabase("ConnectionString").CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@token", SqlDbType.VarChar) { Value = Request.QueryString["codigo"].ToString() });

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        string ip = reader["numero_ip"]?.ToString() ?? string.Empty;
                        return ip;
                    }
                }
            }
            return null;
        }

        protected void LimparCampos()
        {
            txtName.Text = string.Empty;
            txtCpf.Text = string.Empty;
            txtTell.Text = string.Empty;
        }
    }
}
