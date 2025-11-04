using MySql.Data.MySqlClient;
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
    public partial class fornecedor : Form
    {
        public fornecedor()
        {
            InitializeComponent();
        }

        private void fornecedor_Load(object sender, EventArgs e)
        {
            ListarClientes();
        }

        // 📌 Botão que abre o formulário de cadastro de cliente
        private void button1_Click(object sender, EventArgs e)
        {
            Cad_Cliente cad_Cliente = new Cad_Cliente();
            cad_Cliente.ShowDialog();

            // Atualiza a tabela após fechar o cadastro
            ListarClientes();
        }

        // 📌 Botão para atualizar manualmente a lista
        private void button2_Click(object sender, EventArgs e)
        {
            ListarClientes();
        }

        // 📌 Método para buscar clientes no banco e listar no DataGridView
        private void ListarClientes()
        {
            string conexaoBanco = "server=localhost;user id=root;password=;database=db_loljaauto";

            using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
            {
                try
                {
                    conexao.Open();
                    string sql = "SELECT nome AS 'Nome', CNPJ AS 'CNPJ', email AS 'E-mail' FROM tb_fonecedores";

                    using (MySqlDataAdapter adaptador = new MySqlDataAdapter(sql, conexao))
                    {
                        DataTable tabela = new DataTable();
                        adaptador.Fill(tabela);
                        dataGridView1.DataSource = tabela;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao listar clientes: " + ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        // 📌 Evento que ocorre quando o usuário clica em uma célula da tabela
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string nome = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
                string cpf = dataGridView1.Rows[e.RowIndex].Cells["CPF"].Value.ToString();
                string email = dataGridView1.Rows[e.RowIndex].Cells["E-mail"].Value.ToString();

                MessageBox.Show($"Cliente selecionado:\n\nNome: {nome}\nCPF: {cpf}\nE-mail: {email}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecione um cliente para excluir.");
                return;
            }

            // Pegando o CPF do cliente selecionado
            string cpf = dataGridView1.CurrentRow.Cells["CPF"].Value.ToString();
            string nome = dataGridView1.CurrentRow.Cells["Nome"].Value.ToString();

            // Confirmação antes de deletar
            DialogResult resultado = MessageBox.Show(
                $"Deseja realmente excluir o cliente {nome}?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado == DialogResult.Yes)
            {
                string conexaoBanco = "server=localhost;user id=root;password=;database=db_loljaauto";

                using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
                {
                    try
                    {
                        conexao.Open();
                        string sql = "DELETE FROM tb_clientes WHERE CPF = @cpf";

                        using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                        {
                            comando.Parameters.AddWithValue("@cpf", cpf);
                            int linhasAfetadas = comando.ExecuteNonQuery();

                            if (linhasAfetadas > 0)
                            {
                                MessageBox.Show("Cliente excluído com sucesso!");
                                ListarClientes(); // Atualiza a tabela
                            }
                            else
                            {
                                MessageBox.Show("Não foi possível excluir o cliente.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir cliente: " + ex.Message);
                    }
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cad_Fonecedo cad_Fonecedo = new Cad_Fonecedo();
            cad_Fonecedo.Show();

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}