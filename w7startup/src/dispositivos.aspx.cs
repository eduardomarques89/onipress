﻿
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
    public partial class dispositivos : System.Web.UI.Page
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
                    string updateQuery = "UPDATE OniPres_dispositivo SET nome = @nome, empresa = @empresa, unidade = @unidade, bloco = @bloco, [status] = @status, identificador = @identificador, numero_ip = @numero_ip, [entrada/saida] = @funcao WHERE id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtNome.Text);
                    db.AddInParameter(updateCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(updateCommand, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                    db.AddInParameter(updateCommand, "@bloco", DbType.String, ddlBloco.SelectedValue);
                    db.AddInParameter(updateCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(updateCommand, "@numero_ip", DbType.String, txtNumeroIP.Text);
                    db.AddInParameter(updateCommand, "@identificador", DbType.String, txtIdetificador.Text);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {                     
                    string insertQuery = "INSERT INTO OniPres_dispositivo (nome, empresa, unidade, bloco, [status], identificador, numero_ip, [entrada/saida]) VALUES (@nome, @empresa, @unidade, @bloco, @status, @identificador, @numero_ip, @funcao)";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@nome", DbType.String, txtNome.Text);
                    db.AddInParameter(insertCommand, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                    db.AddInParameter(insertCommand, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                    db.AddInParameter(insertCommand, "@bloco", DbType.String, ddlBloco.SelectedValue);
                    db.AddInParameter(insertCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(insertCommand, "@identificador", DbType.String, txtIdetificador.Text);
                    db.AddInParameter(insertCommand, "@funcao", DbType.String, ddlFuncao.SelectedValue);
                    db.AddInParameter(insertCommand, "@numero_ip", DbType.String, txtNumeroIP.Text);

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
            ddlBloco.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            txtNumeroIP.Text = string.Empty;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select d.id, d.nome, e.nome_fantasia as empresa, u.nome as unidade, b.nome as bloco, d.numero_ip, f.nome as funcao from OniPres_dispositivo d  join OniPres_empresa e on e.id = d.empresa join OniPres_unidade u on u.id = d.unidade join OniPres_bloco b on b.id = d.bloco join OniPres_FuncaoDispositivo f on d.[entrada/saida] = f.id where d.[status] = 'Ativo' and d.nome like '%" + txtBuscar.Text + "%'";
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
            string query = "DELETE FROM OniPres_dispositivo WHERE id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            db.ExecuteNonQuery(cmd);

            BindData();

        }

        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "SELECT id, nome, empresa, unidade, bloco, identificador, numero_ip FROM OniPres_dispositivo WHERE id = @id";
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
                ddlBloco.SelectedValue = dr["bloco"].ToString();
                ddlStatus.SelectedValue = "Ativo";
                txtNumeroIP.Text = dr["numero_ip"].ToString();
                txtIdetificador.Text = dr["identificador"].ToString();
                ddlFuncao.SelectedValue = dr["entrada/saida"].ToString();

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#" + pnlModal.ClientID + "').modal('show');", true);
            }
        }

        private void BindData()
        {
            gdvDados.DataBind();
        }
    }
}