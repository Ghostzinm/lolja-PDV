using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOLja
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;
            string senha = textBoxSenha.Text;

            string conexaoBanco = "server=localhost;user id=root;password=;database=db_loljaauto";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Por favor, insira um email válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    using (MySql.Data.MySqlClient.MySqlConnection conexao = new MySql.Data.MySqlClient.MySqlConnection(conexaoBanco))
                    {
                        conexao.Open();
                        string consultaC = "SELECT * FROM tb_clientes WHERE email = @Email AND senha = @Senha";
                        string consultaF = "SELECT * FROM tb_fornecedores WHERE email = @Email AND senha = @Senha";
                        using (MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(consultaC, conexao))
                        {
                            comando.Parameters.AddWithValue("@Email", email);
                            comando.Parameters.AddWithValue("@Senha", senha);
                            using (MySql.Data.MySqlClient.MySqlDataReader leitor = comando.ExecuteReader())
                            {
                                if (leitor.Read())
                                {
                                    MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Form1 fom1 = new Form1();
                                    fom1.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Email ou senha incorretos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        using (MySql.Data.MySqlClient.MySqlCommand comando = new MySql.Data.MySqlClient.MySqlCommand(consultaF, conexao))
                        {
                            comando.Parameters.AddWithValue("@Email", email);
                            comando.Parameters.AddWithValue("@Senha", senha);
                            using (MySql.Data.MySqlClient.MySqlDataReader leitor = comando.ExecuteReader())
                            {
                                if (leitor.Read())
                                {
                                    MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Form1 fom1 = new Form1();
                                    fom1.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Email ou senha incorretos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }


                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cad_Cliente cliente = new Cad_Cliente();
            cliente.Show();

        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
