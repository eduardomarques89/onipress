
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
using gerentefacil;
using Newtonsoft.Json;

namespace global
{
    public partial class unidades : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                if (!string.IsNullOrEmpty(hdfId.Value))
                {
                    // Atualizar registro existente
                    string updateQuery = "UPDATE OniPres_unidade SET nome = @nome, empresa = @empresa, cep = @cep, endereco = @endereco, num = @num, bairro = @bairro, complemento = @complemento, estado = @estado, cidade = @cidade, [status] = @status WHERE id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtNome.Text);
                    db.AddInParameter(updateCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(updateCommand, "@cep", DbType.String, txtCEP.Text);
                    db.AddInParameter(updateCommand, "@endereco", DbType.String, txtEndereco.Text);
                    db.AddInParameter(updateCommand, "@num", DbType.String, txtNum.Text);
                    db.AddInParameter(updateCommand, "@bairro", DbType.String, txtBairro.Text);
                    db.AddInParameter(updateCommand, "@complemento", DbType.String, txtComplemento.Text);
                    db.AddInParameter(updateCommand, "@cidade", DbType.String, txtCidade.Text);
                    db.AddInParameter(updateCommand, "@estado", DbType.String, ddlUF.SelectedValue);
                    db.AddInParameter(updateCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {
                    // Adicionar novo registro
                    string insertQuery = "INSERT INTO OniPres_unidade (nome, empresa, cep, endereco, num, bairro, complemento, estado, cidade, [status]) " +
                                         "VALUES (@nome, @empresa, @cep, @endereco, @num, @bairro, @complemento, @estado, @cidade, @status)";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@nome", DbType.String, txtNome.Text);
                    db.AddInParameter(insertCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(insertCommand, "@cep", DbType.String, txtCEP.Text);
                    db.AddInParameter(insertCommand, "@endereco", DbType.String, txtEndereco.Text);
                    db.AddInParameter(insertCommand, "@num", DbType.String, txtNum.Text);
                    db.AddInParameter(insertCommand, "@bairro", DbType.String, txtBairro.Text);
                    db.AddInParameter(insertCommand, "@complemento", DbType.String, txtComplemento.Text);
                    db.AddInParameter(insertCommand, "@cidade", DbType.String, txtCidade.Text);
                    db.AddInParameter(insertCommand, "@estado", DbType.String, ddlUF.SelectedValue);
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
            txtCEP.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtCidade.Text = string.Empty;
            ddlUF.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                sdsDados.SelectCommand = "SELECT u.id, u.nome AS nome, e.nome_fantasia AS empresa, u.endereco AS endereco, u.cidade AS cidade, u.estado AS estado " +
                                          "FROM OniPres_unidade u " +
                                          "JOIN OniPres_empresa e ON u.empresa = e.id " +
                                          "WHERE u.[status] = 'Ativo' AND u.nome LIKE '%" + txtBuscar.Text + "%'";

                BindData();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao aplicar o filtro: " + ex.Message;
            }
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
            string query = "DELETE FROM OniPres_unidade WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);

            BindData();
        }

        protected void txtCEP_TextChanged(object sender, EventArgs e)
        {
            if (txtCEP.Text.Length > 8)
            {
                string cepnovo = txtCEP.Text.Replace("-", "");
                var info = cep.HttpPost("http://viacep.com.br/ws/" + cepnovo + "/json/");
                dynamic dados = JsonConvert.DeserializeObject<dynamic>(info);
                var end = dados["logradouro"];
                txtEndereco.Text = end.ToString();
                var bairro = dados["bairro"];
                txtBairro.Text = bairro.ToString();
                var cidade = dados["localidade"];
                txtCidade.Text = cidade.ToString();
                var uf = dados["uf"];
                ddlUF.SelectedValue = uf.ToString();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#" + pnlModal.ClientID + "').modal('show');", true);
        }

        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "select id, nome, empresa, cep, endereco, num, bairro, complemento, estado, cidade, [status] from OniPres_unidade where id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string estado = dr["estado"].ToString().Trim();

                hdfId.Value = dr["id"].ToString();
                txtNome.Text = dr["nome"].ToString();
                ddlEmpresas.SelectedValue = dr["empresa"].ToString();
                txtCEP.Text = dr["cep"].ToString();
                txtEndereco.Text = dr["endereco"].ToString();
                txtNum.Text = dr["num"].ToString();
                txtBairro.Text = dr["bairro"].ToString();
                txtComplemento.Text = dr["complemento"].ToString();
                txtCidade.Text = dr["cidade"].ToString();
                ddlUF.SelectedValue = estado;
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