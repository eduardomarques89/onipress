using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.UI;

namespace global
{
    public partial class login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string cript = Criptografia.Encrypt(txtSenha.Text).Replace("+", "=");

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand emailCommand = db.GetSqlStringCommand(
                "SELECT * FROM OniPres_usuario WHERE status = 'ATIVO' AND email = @Email");
            db.AddInParameter(emailCommand, "@Email", DbType.String, txtEmail.Text);

            DbCommand senhaCommand = db.GetSqlStringCommand(
                "SELECT * FROM OniPres_usuario where senha = @senha");
            db.AddInParameter(senhaCommand, "@senha", DbType.String, cript);

            using (IDataReader readerEmail = db.ExecuteReader(emailCommand))
            {
                if (readerEmail.Read())
                {
                    using (IDataReader readerSenha = db.ExecuteReader(senhaCommand))
                    {
                        if (readerSenha.Read())
                        {
                            Response.Redirect("dashboard.aspx", true);
                        }
                        else
                        {
                            lblMensagem.Text = "Senha incorreta";
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = "Email incorreto.";
                }
            }
        }

        protected void lkbEsqueceuSenha_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand selectCommand = db.GetSqlStringCommand(
                "SELECT * FROM OniPres_usuario WHERE email = @Email");
            db.AddInParameter(selectCommand, "@Email", DbType.String, txtEmail.Text);

            using (IDataReader reader = db.ExecuteReader(selectCommand))
            {
                if (reader.Read())
                {
                    string pw = reader["nome"].ToString();
                    string cript = Criptografia.Encrypt(pw).Replace("+", "=");

                    DbCommand updateCommand = db.GetSqlStringCommand(
                        "UPDATE OniPres_usuario SET senha = @senha WHERE email = @Email");
                    db.AddInParameter(updateCommand, "@senha", DbType.String, cript);
                    db.AddInParameter(updateCommand, "@Email", DbType.String, txtEmail.Text);

                    db.ExecuteNonQuery(updateCommand);
                    
                    lblMensagem.Text = "Dados atualizados com sucesso! Sua senha é o primeiro nome cadastrado!";
                }
                else
                {
                    lblMensagem.Text = "Esse e-mail não está cadastrado! Tente novamente!";
                }
            }
        }

        protected void lkbCadastro_Click(object sender, EventArgs e)
        {
            Response.Redirect("cadastro.aspx", false);
        }
    }
}
