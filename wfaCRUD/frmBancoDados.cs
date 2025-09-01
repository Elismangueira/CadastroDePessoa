using MySql.Data.MySqlClient;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        string connectionString = String.Empty;

        public frmBancoDados()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"];

            try
            {
                if (String.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("String de conexão não encontrada./n/nA aplicação será fechada.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Erro de configuração.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private bool ValidarCamposDeDados()
        {
            if (String.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Campo nome deve ser preenchido!!!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Campo email deve ser preenchido!!!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Validar.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Campo email inválido!!!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (String.IsNullOrEmpty(txtTelefone.Text))
            {
                MessageBox.Show("Campo telefone deve ser preenchido!!!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Campo cpf deve ser preenchido!!!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Validar.IsValidCpf(textBox1.Text))
            {
                MessageBox.Show("Campo cpf inválido!!!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposDeDados())
                return;

            if (Validar.IsUserExists(txtCodigo.Text, textBox1.Text))
            {
                MessageBox.Show("Usuário já existe no sistema.", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                

            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    string strSQL = "insert into bdaula.tblagenda(agdid, agdnome, agdemail, agdtelefone, agdcpf) values(NULL,";
                    strSQL += "'" + txtNome.Text + "',";
                    strSQL += "'" + txtEmail.Text + "',";
                    strSQL += "'" + txtTelefone.Text + "',";
                    strSQL += "'" + textBox1.Text + "')";

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        objCommand.ExecuteNonQuery();
                        MessageBox.Show("Registrado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    var campoPreenchidoId = !String.IsNullOrEmpty(txtCodigo.Text.Trim());
                    var campoPreenchidoCpf = !String.IsNullOrEmpty(textBox1.Text.Trim());

                    var campoId = "agdid = " + txtCodigo.Text.Trim();
                    var campoCpf = "agdcpf = '" + textBox1.Text.Trim() + "'";

                    string strSQL = "select * from bdaula.tblagenda where ";

                    if (campoPreenchidoId)
                        strSQL += campoId;

                    if (campoPreenchidoCpf && !campoPreenchidoId)
                        strSQL += campoCpf;

                    if (campoPreenchidoCpf && campoPreenchidoId)
                        strSQL += " AND " + campoCpf;

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        var objDados = objCommand.ExecuteReader();

                        if (!objDados.HasRows)
                        {
                            MessageBox.Show("Registro não encontrado!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtCodigo.Focus();
                        }
                        else
                        {
                            objDados.Read();
                            txtCodigo.Text = objDados["agdid"].ToString();
                            txtNome.Text = objDados["agdnome"].ToString();
                            txtEmail.Text = objDados["agdemail"].ToString();
                            txtTelefone.Text = objDados["agdtelefone"].ToString();
                            textBox1.Text = objDados["agdcpf"].ToString();

                            MessageBox.Show("Registro carregado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();


                    var campoPreenchidoId = !String.IsNullOrEmpty(txtCodigo.Text.Trim());
                    var campoPreenchidoCpf = !String.IsNullOrEmpty(textBox1.Text.Trim());

                    var campoId = "agdid = " + txtCodigo.Text.Trim();
                    var campoCpf = "agdcpf = '" + textBox1.Text.Trim() + "'";

                    string strSQL = "select * from bdaula.tblagenda where ";

                    if (campoPreenchidoId)
                        strSQL += campoId;

                    if (campoPreenchidoCpf && !campoPreenchidoId)
                        strSQL += campoCpf;

                    if (campoPreenchidoCpf && campoPreenchidoId)
                        strSQL += " AND " + campoCpf;

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        var objDados = objCommand.ExecuteReader();

                        if (!objDados.HasRows)
                        {
                            MessageBox.Show("Registro não encontrado!", "Validação de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            strSQL += "where ";

                            if (campoPreenchidoId)
                                strSQL += campoId;

                            if (campoPreenchidoCpf && !campoPreenchidoId)
                                strSQL += campoCpf;

                            if (campoPreenchidoCpf && campoPreenchidoId)
                                strSQL += " AND " + campoCpf;

                            objCommand.Connection = objConexao;
                            objCommand.CommandText = strSQL;
                            objCommand.ExecuteNonQuery();

                            MessageBox.Show("Registro alterado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string strSQL = "delete from bdaula.tblagenda where agdid =" + txtCodigo.Text.Trim();

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        objCommand.ExecuteNonQuery();

                        if (MessageBox.Show("Excluir o código selecionado", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            objCommand.ExecuteNonQuery();

                            MessageBox.Show("Registro eliminado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultarListaDados_Click(object sender, EventArgs e)
        {
            frmConsultarListaDados objTela = new frmConsultarListaDados();
            objTela.ShowDialog();
        }
    }
}
