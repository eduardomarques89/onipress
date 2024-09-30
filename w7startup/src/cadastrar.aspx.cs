﻿using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
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

        }

        protected async void EnviarDados_Click(object sender, EventArgs e)
        {
            lblNome.Text = "";
            lblCPF.Text = "";
            lblTell.Text = "";
            lblReposta.Text = "";

            bool allFieldsValid = true;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                lblNome.Text = "Preencha com seu Nome!";
                allFieldsValid = false;
            }
            if (string.IsNullOrEmpty(txtCpf.Text))
            {
                lblCPF.Text = "Preencha com seu CPF!";
                allFieldsValid = false;
            }
            if (string.IsNullOrEmpty(txtTell.Text))
            {
                lblTell.Text = "Preencha com seu Telefone!";
                allFieldsValid = false;
            }

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
                                    Session["Id"] = insertedId;

                                    Response.Redirect("acesso.aspx", false);
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
                    string session = loginResponseData.session;
                    string host = "http://192.168.0.204:8013/";

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
                    Console.WriteLine(responseBody);
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
