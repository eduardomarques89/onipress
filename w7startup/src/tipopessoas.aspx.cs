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

namespace global
{
    public partial class tipopessoas : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand command = db.GetSqlStringCommand(
                "INSERT INTO OniPres_tipoPessoa (nome, [status]) VALUES (@nome, @status)");

            db.AddInParameter(command, "@nome", DbType.String, txtNome.Text);
            db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);

            try
            {
                db.ExecuteNonQuery(command);

                lblMensagem.Text = "Adicionado com sucesso!";
                txtNome.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro: " + ex;
            }
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * FROM OniPres_tipoPessoa"))
            {
                if (reader.Read())
                {
                    txtNome.Text = reader["nome"].ToString();
                    ddlStatus.SelectedValue = reader["status"].ToString();
                    lblMensagem.Text = "";
                }
            }
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select nome, status from OniPres_tipoPessoa where nome like '%" + txtBuscar.Text + "%'";
            gdvDados.DataBind();
        }
    }
}