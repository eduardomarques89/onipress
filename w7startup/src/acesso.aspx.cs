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
    public partial class acesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void EnviarDados_Click(object sender, EventArgs e)
      //protected async void EnviarDados_Click(object sender, EventArgs e)
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

                    DbCommand insertCommand = db.GetSqlStringCommand(
                        "INSERT INTO OniPres_Acesso (data_initial, data_final, id_companies, id_block, id_unity, id_device) VALUES (@data_i, @data_f, @companies, @block, @unity, @device)");

                    db.AddInParameter(insertCommand, "@data_i", DbType.String, data_initial);
                    db.AddInParameter(insertCommand, "@data_f", DbType.String, data_final);
                    db.AddInParameter(insertCommand, "@companies", DbType.String, txtCompanies.Text);
                    db.AddInParameter(insertCommand, "@block", DbType.String, txtBlock.Text);
                    db.AddInParameter(insertCommand, "@unity", DbType.String, txtUnity.Text);
                    db.AddInParameter(insertCommand, "@device", DbType.String, 0);

                    db.ExecuteNonQuery(insertCommand);

                    //await EnviarParaAPI(txtName.Text);

                    lblReposta.Text = "Dados enviados com sucesso!";
                }
                catch (Exception ex)
                {
                    lblErro.Text = "Erro ao cadastrar dados no Banco Interno! Erro: " + ex;
                }

                LimparCampos();
            }
        }

        //private async Task EnviarParaAPI(string name)
        //{

        //}

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
