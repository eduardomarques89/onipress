
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
    public partial class empresa : System.Web.UI.Page
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
                    string updateQuery = "UPDATE OniPres_empresa SET nome_fantasia = @nome, razao_social = @razao, cpf_cnpj = @cpf, celular = @celular, email = @email, cep = @cep, endereco = @endereco, num = @num, bairro = @bairro, complemento = @complemento, estado = @estado, cidade = @cidade, [status] = @status WHERE id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtNomeCliente.Text);
                    db.AddInParameter(updateCommand, "@razao", DbType.String, txtRazaoSocial.Text);
                    db.AddInParameter(updateCommand, "@cpf", DbType.String, txtCPFCNPJ.Text);
                    db.AddInParameter(updateCommand, "@celular", DbType.String, txtCelular.Text);
                    db.AddInParameter(updateCommand, "@email", DbType.String, txtCelular.Text);
                    db.AddInParameter(updateCommand, "@cep", DbType.String, txtCEP.Text);
                    db.AddInParameter(updateCommand, "@endereco", DbType.String, txtEndereco.Text);
                    db.AddInParameter(updateCommand, "@num", DbType.String, txtNum.Text);
                    db.AddInParameter(updateCommand, "@bairro", DbType.String, txtBairro.Text);
                    db.AddInParameter(updateCommand, "@complemento", DbType.String, txtComplemento.Text);
                    db.AddInParameter(updateCommand, "@estado", DbType.String, ddlUF.SelectedValue);
                    db.AddInParameter(updateCommand, "@cidade", DbType.String, txtCidade.Text);
                    db.AddInParameter(updateCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {
                    // Adicionar novo registro
                    string insertQuery = "insert into OniPres_empresa (nome_fantasia, razao_social, [cpf_cnpj], celular, email, CEP, endereco, num, bairro, complemento, estado, cidade, [status]) values (@nome, @razao, @cpf, @celular, @email, @cep, @endereco, @num, @bairro, @complemento, @estado, @cidade, @status)";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@nome", DbType.String, txtNomeCliente.Text);
                    db.AddInParameter(insertCommand, "@razao", DbType.String, txtRazaoSocial.Text);
                    db.AddInParameter(insertCommand, "@cpf", DbType.String, txtCPFCNPJ.Text);
                    db.AddInParameter(insertCommand, "@celular", DbType.String, txtCelular.Text);
                    db.AddInParameter(insertCommand, "@email", DbType.String, txtEmail.Text);
                    db.AddInParameter(insertCommand, "@cep", DbType.String, txtCEP.Text);
                    db.AddInParameter(insertCommand, "@endereco", DbType.String, txtEndereco.Text);
                    db.AddInParameter(insertCommand, "@num", DbType.String, txtNum.Text);
                    db.AddInParameter(insertCommand, "@bairro", DbType.String, txtBairro.Text);
                    db.AddInParameter(insertCommand, "@complemento", DbType.String, txtComplemento.Text);
                    db.AddInParameter(insertCommand, "@estado", DbType.String, ddlUF.SelectedValue);
                    db.AddInParameter(insertCommand, "@cidade", DbType.String, txtCidade.Text);
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
            txtNomeCliente.Text = string.Empty;
            txtRazaoSocial.Text = string.Empty;
            txtCPFCNPJ.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
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
            sdsDados.SelectCommand = "select id, nome_fantasia, razao_social, cpf_cnpj, celular, email, CEP, endereco, bairro, estado, cidade from OniPres_empresa where [status] = 'Ativo' and nome_fantasia like '%" + txtBuscar.Text + "%'";
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
            string query = "DELETE FROM OniPres_empresa WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);

            BindData();
        }

        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "select id, nome_fantasia, razao_social, cpf_cnpj, celular, email, CEP, endereco, num, bairro, complemento, estado, cidade, [status] from OniPres_empresa where id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                
                DataRow dr = ds.Tables[0].Rows[0];

                string estado = dr["estado"].ToString().Trim();

                hdfId.Value = dr["id"].ToString();
                txtNomeCliente.Text = dr["nome_fantasia"].ToString();
                txtRazaoSocial.Text = dr["razao_social"].ToString();
                txtCPFCNPJ.Text = dr["cpf_cnpj"].ToString();
                txtCelular.Text = dr["cpf_cnpj"].ToString();
                txtEmail.Text = dr["cpf_cnpj"].ToString();
                txtCEP.Text = dr["cpf_cnpj"].ToString();
                txtEndereco.Text = dr["cpf_cnpj"].ToString();
                txtNum.Text = dr["cpf_cnpj"].ToString();
                txtBairro.Text = dr["cpf_cnpj"].ToString();
                txtComplemento.Text = dr["cpf_cnpj"].ToString();
                txtCidade.Text = dr["cpf_cnpj"].ToString();
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