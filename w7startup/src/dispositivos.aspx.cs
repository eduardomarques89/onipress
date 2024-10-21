using System;
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace global
{
    public partial class dispositivos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                DbCommand cmd;

                if (!string.IsNullOrEmpty(hdfId.Value))
                {
                    // Atualização do registro
                    string updateQuery = @"
                        UPDATE OniPres_dispositivo 
                        SET nome = @nome, empresa = @empresa, unidade = @unidade, 
                            bloco = @bloco, [status] = @status, identificador = @identificador, 
                            numero_ip = @numero_ip, [entrada/saida] = @funcao 
                        WHERE id = @id";
                    cmd = db.GetSqlStringCommand(updateQuery);
                    db.AddInParameter(cmd, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));
                }
                else
                {
                    // Inserção de novo registro
                    string insertQuery = @"
                        INSERT INTO OniPres_dispositivo 
                        (nome, empresa, unidade, bloco, [status], identificador, 
                         numero_ip, [entrada/saida]) 
                        VALUES (@nome, @empresa, @unidade, @bloco, @status, 
                                @identificador, @numero_ip, @funcao)";
                    cmd = db.GetSqlStringCommand(insertQuery);
                }

                // Parâmetros comuns para inserção e atualização
                db.AddInParameter(cmd, "@nome", DbType.String, txtNome.Text);
                db.AddInParameter(cmd, "@empresa", DbType.String, ddlEmpresas.SelectedValue);
                db.AddInParameter(cmd, "@unidade", DbType.String, ddlUnidades.SelectedValue);
                db.AddInParameter(cmd, "@bloco", DbType.String, ddlBloco.SelectedValue);
                db.AddInParameter(cmd, "@status", DbType.String, ddlStatus.SelectedValue);
                db.AddInParameter(cmd, "@identificador", DbType.String, txtIdetificador.Text);
                db.AddInParameter(cmd, "@numero_ip", DbType.String, txtNumeroIP.Text);
                db.AddInParameter(cmd, "@funcao", DbType.String, ddlFuncao.SelectedValue);

                db.ExecuteNonQuery(cmd);
                lblMensagem.Text = "Operação realizada com sucesso!";

                LimparCampos();
                hdfId.Value = string.Empty;
                BindData();
                FecharModal();
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
            txtIdetificador.Text = string.Empty;
            ddlFuncao.SelectedIndex = 0;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = @"
                SELECT d.id, d.nome, e.nome_fantasia AS empresa, 
                       u.nome AS unidade, b.nome AS bloco, 
                       d.numero_ip, f.nome AS funcao 
                FROM OniPres_dispositivo d
                JOIN OniPres_empresa e ON e.id = d.empresa
                JOIN OniPres_unidade u ON u.id = d.unidade
                JOIN OniPres_bloco b ON b.id = d.bloco
                JOIN OniPres_FuncaoDispositivo f ON d.[entrada/saida] = f.id
                WHERE d.[status] = 'Ativo' 
                AND d.nome LIKE '%' + @nome + '%'";

            sdsDados.SelectParameters.Clear();
            sdsDados.SelectParameters.Add("nome", txtBuscar.Text);
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
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                string query = "DELETE FROM OniPres_dispositivo WHERE id = @id";
                DbCommand cmd = db.GetSqlStringCommand(query);
                db.AddInParameter(cmd, "@id", DbType.Int32, id);
                db.ExecuteNonQuery(cmd);

                lblMensagem.Text = "Registro excluído com sucesso!";
                BindData();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao excluir: " + ex.Message;
            }
        }

        private void EditarRegistro(int id)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                string query = @"
                    SELECT id, nome, empresa, unidade, bloco, 
                           identificador, numero_ip, [entrada/saida] 
                    FROM OniPres_dispositivo 
                    WHERE id = @id";
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
                    txtNumeroIP.Text = dr["numero_ip"].ToString();
                    txtIdetificador.Text = dr["identificador"].ToString();
                    ddlFuncao.SelectedValue = dr["entrada/saida"].ToString();

                    AbrirModal();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao carregar registro: " + ex.Message;
            }
        }

        private void BindData()
        {
            gdvDados.DataBind();
        }

        private void AbrirModal()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal",
                "$('#" + pnlModal.ClientID + "').modal('show');", true);
        }

        private void FecharModal()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal",
                "$('#" + pnlModal.ClientID + "').modal('hide');", true);
        }
    }
}
