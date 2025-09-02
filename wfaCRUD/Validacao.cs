using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace wfaCRUD
{
    /// <summary>
    /// Realiza a validação do CPF
    /// </summary>
    public static class Validar
    {
        static string connectionString = String.Empty;

        static Validar()
        {
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

        public static bool IsUserExists(string id, string cpf)
        {
            try
            {
                using (var objConexao = new MySqlConnection(connectionString))
                {
                    objConexao.Open();

                    var campoPreenchidoId = !String.IsNullOrEmpty(id.Trim());
                    var campoPreenchidoCpf = !String.IsNullOrEmpty(cpf.Trim());

                    var campoId = "agdid = " + id.Trim();
                    var campoCpf = "agdcpf = " + cpf.Trim();

                    string strSQL = "select agdcpf from tblagenda where ";

                    if (campoPreenchidoId)
                        strSQL += campoId;

                    if (campoPreenchidoCpf && !campoPreenchidoId)
                        strSQL += campoCpf;

                    if (campoPreenchidoCpf && campoPreenchidoId)
                        strSQL += " AND " + campoCpf;

                    using (var objCommand = new MySqlCommand(strSQL, objConexao))
                    {
                        var objDados = objCommand.ExecuteScalar();

                        return objDados != null;
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro verificar se o usuário existe.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidTelefone (string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
            {
                return false;
            }

            string pattern = @"/d";
            return Regex.IsMatch(telefone, pattern);
        }

        public static bool IsValidCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
