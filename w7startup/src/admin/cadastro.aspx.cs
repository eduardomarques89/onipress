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
    public partial class cadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Código do Page_Load aqui
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string cript = Criptografia.Encrypt(txtSenha.Text).Replace("+", "=");

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand insertCommand = db.GetSqlStringCommand(
                "INSERT INTO OniPres_usuario (nome, email, senha, status) values (@nome, @email, @senha, 'ATIVO')");
            db.AddInParameter(insertCommand, "@nome", DbType.String, txtNome.Text);
            db.AddInParameter(insertCommand, "@email", DbType.String, txtEmail.Text);
            db.AddInParameter(insertCommand, "@senha", DbType.String, cript);


            using (IDataReader reader1 = db.ExecuteReader(insertCommand))
            {
                db.ExecuteNonQuery(insertCommand);
                Response.Redirect("dashboard.aspx", true);
            }
        }

        protected void lkbLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx", false);
        }
    }
}
