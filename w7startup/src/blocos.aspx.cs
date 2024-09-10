
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
    public partial class blocos : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                if (!string.IsNullOrEmpty(hdfId.Value))
                {
                    // Atualizar registro existente
                    string updateQuery = "UPDATE OniPres_bloco SET nome = @nome, empresa = @empresa, unidade = @unidade, [status] = @status WHERE id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtNome.Text);
                    db.AddInParameter(updateCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(updateCommand, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                    db.AddInParameter(updateCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {
                    // Adicionar novo registro
                    string insertQuery = "INSERT INTO OniPres_bloco (nome, empresa, unidade, [status]) VALUES (@nome, @empresa, @unidade, @status)";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@nome", DbType.String, txtNome.Text);
                    db.AddInParameter(insertCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(insertCommand, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                    db.AddInParameter(insertCommand, "@status", DbType.String, ddlStatus.SelectedValue);

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
            txtNome.Text = string.Empty;
            ddlEmpresas.SelectedIndex = 0;
            ddlUnidades.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select b.id, b.nome, e.nome_fantasia empresa, u.nome unidade from OniPres_bloco b join OniPres_empresa e on e.id = b.empresa join OniPres_unidade u on u.id = b.unidade where b.[status] = 'Ativo' and b.nome like '%" + txtBuscar.Text + "%'";
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
            string query = "DELETE FROM OniPres_bloco WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);

            BindData();
        }

        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "SELECT id, nome, empresa, unidade FROM OniPres_bloco WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                hdfId.Value = dr["id"].ToString();
                txtNome.Text = dr["nome"].ToString();
                ddlEmpresas.SelectedValue = dr["empresa"].ToString();
                ddlUnidades.SelectedValue = dr["unidade"].ToString();
                ddlStatus.SelectedValue = "Ativo";

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#" + pnlModal.ClientID + "').modal('show');", true);
            }
        }

        private void BindData()
        {
            gdvDados.DataBind();
        }
    }
}