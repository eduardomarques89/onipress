using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web.UI;

namespace global
{
    public partial class cadastrarView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idUsuario = Convert.ToInt32(Session["Id"]);

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "SELECT nome, cpf, celular FROM OniPres_pessoa WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, idUsuario);

            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                txtName.Text = row["nome"].ToString();
                txtCpf.Text = row["cpf"].ToString();
                txtTell.Text = row["celular"].ToString();
            }
            else
            {
                lblErro.Text = "Usuário não encontrado.";
                txtName.Text = string.Empty;
                txtCpf.Text = string.Empty;
                txtTell.Text = string.Empty;
            }
        }
        protected void AvancarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("acessoView.aspx", false);
        }
    }
}
