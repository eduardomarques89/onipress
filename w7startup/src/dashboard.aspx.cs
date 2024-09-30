
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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;

namespace global
{
    public partial class dashboard : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //    try
                //    {
                //        lblMensagemBoasVindas.Text = "Bem-vindo, " + Session["nomeusuario"].ToString();
                //    }
                //    catch (Exception ex)
                //    {
                //        lblMensagemBoasVindas.Text = ex.Message;
                //    }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                             "select count(*) as tot from OniPres_empresa where [status] = 'Ativo'"))
                {
                    if (reader.Read())
                    {
                        lblEmpresas.Text = reader["tot"].ToString();
                    }
                    else
                    {
                        lblEmpresas.Text = "0";
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                             "select count(*) as tot from OniPres_pessoa p join OniPres_tipoPessoa t on p.tipo_acesso = t.id where t.nome = 'Visitante'"))
                {
                    if (reader.Read())
                    {
                        lblVisitantes.Text = reader["tot"].ToString();
                    }
                    else
                    {
                        lblVisitantes.Text = "0";
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                    "select count(*) as tot from OniPres_dispositivo where [status] = 'Ativo'"))
                {
                    if (reader.Read())
                    {
                        lblDispositivos.Text = reader["tot"].ToString();
                    }
                    else
                    {
                        lblDispositivos.Text = "0";
                    }
                }
            }
        }



    }
}