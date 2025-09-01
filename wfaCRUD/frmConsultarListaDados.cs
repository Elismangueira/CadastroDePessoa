using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfaCRUD
{
    public partial class frmConsultarListaDados : Form
    {
        string connectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"].ToString();

        public frmConsultarListaDados()
        {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    string strSQL = "select * from tblagenda order by agdnome";

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        var objDados = objCommand.ExecuteReader();

                        if (objDados.HasRows)
                        {
                            dgvListaDados.Rows.Clear();

                            while (objDados.Read())
                            {
                                dgvListaDados.Rows.Add(objDados["agdid"].ToString(), objDados["agdcpf"].ToString(), objDados["agdnome"].ToString(), objDados["agdemail"].ToString(), objDados["agdtelefone"].ToString());
                            }
                        }

                        MessageBox.Show("Registrado com sucesso!");
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao consultar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
