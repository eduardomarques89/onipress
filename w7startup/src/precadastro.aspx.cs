
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Threading.Tasks;
using pix_dynamic_payload_generator.net;
using pix_dynamic_payload_generator.net.Requests.RequestServices;
using System.Runtime.InteropServices;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Web.UI.WebControls;

namespace global
{
    public partial class precadastro : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                if (!string.IsNullOrEmpty(hdfId.Value))
                {
                    // Atualizar registro existente
                    string updateQuery = "UPDATE OniPres_pessoa SET telefone = @telefone, tipo_acesso = @tipo WHERE id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtTelefone.Text);
                    db.AddInParameter(updateCommand, "@tipo", DbType.String, ddlTipo.SelectedValue);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {
                    // Adicionar novo registro
                    string insertQuery = "INSERT INTO OniPres_precadastro (telefone, tipopessoa) VALUES (@telefone, @tipo)";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@telefone", DbType.String, txtTelefone.Text);
                    db.AddInParameter(insertCommand, "@tipo", DbType.String, ddlTipo.SelectedValue);

                    db.ExecuteNonQuery(insertCommand);
                    lblMensagem.Text = "Adicionado com sucesso!";
                }

                LimparCampos();
                hdfId.Value = string.Empty;

                gdvDados.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#" + pnlModal.ClientID + "').modal('hide');", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao salvar: " + ex.Message;
            }
        }



        private void LimparCampos()
        {
            txtTelefone.Text = string.Empty;
            ddlTipo.SelectedIndex = 0;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select pc.id, pc.telefone, pc.tipopessoa from OniPres_precadastro pc join OniPres_tipoPessoa pp on pp.id = pc.tipopessoa pc.nome like '%" + txtBuscar.Text + "%'";
            BindData();
        }

        protected void gdvDados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Excluir")
            {
                ExcluirRegistro(id);
            }
            else if (e.CommandName == "Editar")
            {
                EditarRegistro(id);
            }
        }

        private void ExcluirRegistro(int id)
        {
            // Implementar a lógica de exclusão do registro
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "DELETE FROM OniPres_precadastro WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);

            BindData();
        }

        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "SELECT * FROM OniPres_preadastro WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                hdfId.Value = dr["id"].ToString();
                txtTelefone.Text = dr["nome"].ToString();
                ddlTipo.SelectedValue = dr["tipo_acesso"].ToString();

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#" + pnlModal.ClientID + "').modal('show');", true);
            }
        }

        private void BindData()
        {
            gdvDados.DataBind();
        }
    }
}