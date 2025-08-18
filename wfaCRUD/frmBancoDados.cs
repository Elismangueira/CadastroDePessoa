using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfaCRUD
{
    public partial class frmBancoDados : Form
    {
        string connectionString = "Server=192.168.10.101;Database=bdaula;user=root;password=9kjThhnVcXJP";
        //string connectionString = "Server=sql111.infinityfree.com;Database=f0_39700899_cadastrodepessoa;user=if0_39700899;password=0URz1EHbRGhx";

        public frmBancoDados()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool ValidarCamposDeDados()
        {
            if (String.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Campo nome deve ser preenchido!!!", "Validação de dados");
                return false;
            }
                

            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Campo email deve ser preenchido!!!", "Validação de dados");
                return false;
            }

            if (String.IsNullOrEmpty(txtTelefone.Text))
            {
                MessageBox.Show("Campo telefone deve ser preenchido!!!", "Validação de dados");
                return false;
            }

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Campo cpf deve ser preenchido!!!", "Validação de dados");
                return false;
            }

            return true;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposDeDados())
                return;

            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    string strSQL = "insert into tblagenda(agdid, agdnome, agdemail, agdtelefone, agdcpf) values(NULL,";
                    strSQL += "'" + txtNome.Text + "',";
                    strSQL += "'" + txtEmail.Text + "',";
                    strSQL += "'" + txtTelefone.Text + "',";
                    strSQL += "'" + textBox1.Text + "')";

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        objCommand.ExecuteNonQuery();
                        MessageBox.Show("Registrado com sucesso!");
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao inserir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    string strSQL = "select * from tblagenda where agdid = " + txtCodigo.Text.Trim();

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        var objDados = objCommand.ExecuteReader();

                        if (!objDados.HasRows)
                        {
                            MessageBox.Show("Registro não encontrado!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCodigo.Focus();
                        }
                        else
                        {
                            objDados.Read();
                            txtNome.Text = objDados["agdnome"].ToString();
                            txtEmail.Text = objDados["agdemail"].ToString();
                            txtTelefone.Text = objDados["agdtelefone"].ToString();
                            textBox1.Text = objDados["agdcpf"].ToString();
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

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    string strSQL = "select * from tblagenda where agdid = " + txtCodigo.Text.Trim();

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        var objDados = objCommand.ExecuteReader();

                        if (!objDados.HasRows)
                        {
                            MessageBox.Show("Registro não encontrado!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCodigo.Focus();
                        }
                        else
                        {
                            if (!objDados.IsClosed) { objDados.Close(); }

                            strSQL = "update tblagenda  set ";
                            strSQL += "agdnome = '" + txtNome.Text + "',";
                            strSQL += "agdemail = '" + txtEmail.Text + "',";
                            strSQL += "agdtelefone = '" + txtTelefone.Text + "',";
                            strSQL += "agdcpf = '" + txtCPF.Text + "'";
                            strSQL += "where agdid = " + txtCodigo.Text.Trim();

                            objCommand.Connection = objConexao;
                            objCommand.CommandText = strSQL;
                            objCommand.ExecuteNonQuery();

                            MessageBox.Show("Registro alterado com sucesso!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao alterar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            textBox1.Text = "";
            txtCodigo.Focus();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    string strSQL = "delete from tblagenda where agdid =" + txtCodigo.Text.Trim();

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        objCommand.ExecuteNonQuery();

                        if (MessageBox.Show("Excluir o código selecionado", "Banco de Dados", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            objCommand.ExecuteNonQuery();

                            MessageBox.Show("Registro eliminado com sucesso!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultarListaDados_Click(object sender, EventArgs e)
        {
            frmConsultarListaDados objTela = new frmConsultarListaDados();
            objTela.ShowDialog();
        }
    }
}
