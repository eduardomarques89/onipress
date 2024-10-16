using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using pix_dynamic_payload_generator.net.Models;
using System;
using System.Data;
using System.Data.Common;
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

                    // Add the token parameter
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

            bool allFieldsValid = true;

            if (allFieldsValid)
            {
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
                        DbCommand acessoCommand = db.GetSqlStringCommand(
                            "INSERT INTO OniPres_Acesso (name, id_uduario) VALUES (@name, @id)");

                        db.AddInParameter(acessoCommand, "@id", DbType.Int32, Convert.ToInt32(insertedId));
                        db.AddInParameter(acessoCommand, "@name", DbType.String, txtName.Text);

                        db.ExecuteNonQuery(acessoCommand);

                        try
                        {
                            DbCommand selectCommand = db.GetSqlStringCommand(
                                "SELECT * FROM OniPres_acesso WHERE id_uduario = @id");

                            db.AddInParameter(selectCommand, "@id", DbType.Int32, Convert.ToInt32(insertedId));

                            using (IDataReader reader = db.ExecuteReader(selectCommand))
                            {
                                if (reader.Read())
                                {
                                    string nome = reader["name"].ToString();

                                    Session["Nome"] = nome;
                                    Session["Telefone"] = txtTell.Text;
                                    Session["Id"] = insertedId;
                                }
                                else
                                {
                                    lblErro.Text = "Nenhum usuário encontrado com o ID inserido.";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblErro.Text = "Erro ao consultar dados no Banco Interno! Erro: " + ex.Message;
                        }

                        await EnviarParaAPI(txtName.Text);
                    }
                    else
                    {
                        lblErro.Text = "Erro ao obter o ID inserido.";
                    }
                }
                catch (Exception ex)
                {
                    lblErro.Text = "Erro ao cadastrar dados no Banco Interno! Erro: " + ex.Message;
                }

                LimparCampos();
            }
        }

        private async Task EnviarParaAPI(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string loginUrl = "http://192.168.0.201:8013/login.fcgi";

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
                    string host = "http://192.168.0.201:8013/";

                    string apiUrl = $"{host}/create_objects.fcgi?session={session}";

                    var requestBody = new
                    {
                        @object = "users",
                        values = new[]
                        {
                    new
                    {
                        name = name,
                        registration = "",
                        password = "",
                        salt = ""
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

                    var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
                    int id = responseObject.ids[0];
                    Session["UserId"] = id;

                    string codigo = Request.QueryString["codigo"]; ;
                    Response.Redirect($"acesso.aspx?codigo={codigo}", false);
                }
                catch (Exception ex)
                {
                    lblErro.Text = "Erro ao enviar dados para a API: " + ex;
                }
            }
        }

        protected void LimparCampos()
        {
            txtName.Text = "";
            txtCpf.Text = "";
            txtTell.Text = "";
        }
    }
}
