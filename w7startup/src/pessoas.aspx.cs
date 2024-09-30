
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
    public partial class pessoas : System.Web.UI.Page
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
                    string updateQuery = "UPDATE OniPres_pessoa SET nome = @nome, tipo_acesso = @tipo, cpf = @cpf, celular = @celular, email = @email, empresa = @empresa, unidade = @unidade, bloco = @bloco, dispositivo = @dispositivo, [status] = @status WHERE id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtNomeCliente.Text);
                    db.AddInParameter(updateCommand, "@tipo", DbType.String, ddlTipo.SelectedValue);
                    db.AddInParameter(updateCommand, "@cpf", DbType.String, txtCPFCNPJ.Text);
                    db.AddInParameter(updateCommand, "@celular", DbType.String, txtCelular.Text);
                    db.AddInParameter(updateCommand, "@email", DbType.String, txtEmail.Text);
                    db.AddInParameter(updateCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(updateCommand, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                    db.AddInParameter(updateCommand, "@bloco", DbType.String, ddlBloco.SelectedValue);
                    db.AddInParameter(updateCommand, "@dispositivo", DbType.String, ddlDispositivo.SelectedValue);
                    db.AddInParameter(updateCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {
                    // Adicionar novo registro
                    string insertQuery = "INSERT INTO OniPres_pessoa (nome, tipo_acesso, cpf, celular, email, empresa, unidade, bloco, dispositivo, [status]) VALUES (@nome, @tipo, @cpf, @celular, @email, @empresa, @unidade, @bloco, @dispositivo, @status)";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@nome", DbType.String, txtNomeCliente.Text);
                    db.AddInParameter(insertCommand, "@tipo", DbType.String, ddlTipo.SelectedValue);
                    db.AddInParameter(insertCommand, "@cpf", DbType.String, txtCPFCNPJ.Text);
                    db.AddInParameter(insertCommand, "@celular", DbType.String, txtCelular.Text);
                    db.AddInParameter(insertCommand, "@email", DbType.String, txtEmail.Text);
                    db.AddInParameter(insertCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(insertCommand, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                    db.AddInParameter(insertCommand, "@bloco", DbType.String, ddlBloco.SelectedValue);
                    db.AddInParameter(insertCommand, "@dispositivo", DbType.String, ddlDispositivo.SelectedValue);
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
            ddlTipo.SelectedIndex = 0;
            txtCPFCNPJ.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlEmpresas.SelectedIndex = 0;
            ddlUnidades.SelectedIndex = 0;
            ddlBloco.SelectedIndex = 0;
            ddlDispositivo.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select p.id, p.nome, t.nome as acesso, p.cpf, p.celular, e.nome_fantasia as empresa , u.nome as unidade, b.nome as bloco, d.nome as dispositivo from OniPres_pessoa p join OniPres_tipoPessoa t on p.tipo_acesso = t.id join OniPres_empresa e on e.id = p.empresa join OniPres_unidade u on u.id = p.unidade join OniPres_bloco b on b.id = p.bloco join OniPres_dispositivo d on d.id = p.dispositivo where p.[status] = 'Ativo' and p.nome like '%" + txtBuscar.Text + "%'";
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
            string query = "DELETE FROM OniPres_pessoa WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);

            BindData();
        }

        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "SELECT id, nome, tipo_acesso, cpf, celular, email, empresa, unidade, bloco, dispositivo FROM OniPres_pessoa WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                hdfId.Value = dr["id"].ToString();
                txtNomeCliente.Text = dr["nome"].ToString();
                ddlTipo.SelectedValue = dr["tipo_acesso"].ToString();
                txtCPFCNPJ.Text = dr["cpf"].ToString();
                txtCelular.Text = dr["celular"].ToString();
                txtEmail.Text = dr["email"].ToString();
                ddlEmpresas.SelectedValue = dr["empresa"].ToString();
                ddlUnidades.SelectedValue = dr["unidade"].ToString();
                ddlBloco.SelectedValue = dr["bloco"].ToString();
                ddlDispositivo.SelectedValue = dr["dispositivo"].ToString();
                ddlStatus.SelectedValue = "Ativo";

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#" + pnlModal.ClientID + "').modal('show');", true);
            }
        }

        private void BindData()
        {
            gdvDados.DataBind();
        }

        protected void ddlEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUnidades.DataBind();
            ddlBloco.DataBind();
            ddlDispositivo.DataBind();
        }
    }
}