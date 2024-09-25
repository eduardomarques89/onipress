using Microsoft.Practices.EnterpriseLibrary.Common;
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

        //protected async void EnviarDados_Click(object sender, EventArgs e
        protected void EnviarDados_Click(object sender, EventArgs e)
        {
            // Limpar as labels de erro
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

                    // Adiciona os parâmetros
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

                        Response.Redirect("acesso.aspx", true);
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
                    string host = "http://138.94.44.203:8113/";
                    string session = "VmHUViPvsmZYP0s96UNHA5M2";
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
                        "application/json");

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
