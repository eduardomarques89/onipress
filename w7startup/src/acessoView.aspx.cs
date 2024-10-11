using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web.UI;

namespace global
{
    public partial class acessoView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idUsuario = Convert.ToInt32(Session["Id"]);

                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                string query = "SELECT * FROM OniPres_Acesso WHERE id_uduario = @id";
                DbCommand cmd = db.GetSqlStringCommand(query);
                db.AddInParameter(cmd, "@id", DbType.Int32, idUsuario);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    txtDataInicial.Text = Convert.ToDateTime(row["data_initial"]).ToString("yyyy-MM-dd");
                    //txtHourInicial.Text = TimeSpan.Parse(row["hora_inicial"].ToString()).ToString(@"hh\:mm");

                    txtDataFinal.Text = Convert.ToDateTime(row["data_final"]).ToString("yyyy-MM-dd");
                    //txtHourFinal.Text = TimeSpan.Parse(row["hora_final"].ToString()).ToString(@"hh\:mm");

                    ddlCompanies.SelectedValue = row["id_companies"].ToString();
                    ddlBlock.SelectedValue = row["id_block"].ToString();
                    ddlUnity.SelectedValue = row["id_unity"].ToString();
                }
            }
        }

        protected void AvancarPagina_Click(object sender, EventArgs e)
        {
            Response.Redirect("identidade.aspx", false);
        }
    }
}
