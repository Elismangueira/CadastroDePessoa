using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace wfaCRUD
{
    public partial class frmConsultarListaDados : Form
    {
        //string connectionString = "Server=192.168.10.101;Database=bdaula;user=root;password=9kjThhnVcXJP";
        string connectionString = "Server=sql10.freesqldatabase.com;Database=sql10793804;user=sql10793804;password=Kk5pZ7ZIDE";

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
