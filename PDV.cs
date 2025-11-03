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
using static System.Net.Mime.MediaTypeNames;

namespace LOLja
{
    public partial class PDV : Form
    {
        private string cod;
        private string descricao;
        private int qtdeAtacado;
        private decimal precoCusto;
        private decimal precoVenda;
        private string subGrupo;
        private string grupo;
        private string marca;
        public PDV()
        {
            InitializeComponent();
        }

        public void limparCampos()
        {
            textBoxCod.Clear();
            textBoxDescricao.Clear();
            textBoxQtdeAtacado.Clear();
            textBoxPrecoCusto.Clear();
            textBoxPrecoVenda.Clear();
            textBoxSubGrupo.Clear();
            textBoxGrupo.Clear();
            textBoxMarca.Clear();
            textBoxReferencia.Clear();
            textBox16.Clear();

        }

        public void campos()
        {
            string cod = textBoxCod.Text;
            string descricao = textBoxDescricao.Text;
            string qtdeAtacado = textBoxQtdeAtacado.Text;
            string precoCusto = textBoxPrecoCusto.Text;
            string precoVenda = textBoxPrecoVenda.Text;
            string subGrupo = textBoxSubGrupo.Text;
            string grupo = textBoxGrupo.Text;
            string marca = textBoxMarca.Text;
        }

        private void BuscarProdutoPorCodigo()
        {
            string cod = textBoxCod.Text.Trim();
            if (string.IsNullOrEmpty(cod))
                return; // não faz nada se estiver vazio

            string conexaoBanco = "server=localhost;user id=root;password=;database=db_loljaauto";

            using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
            {
                try
                {
                    conexao.Open();
                    string sql = "SELECT descricao, qtde_atacado, preco_custo, preco_venda, sub_grupo, grupo, marca " +
                                 "FROM produtos WHERE cod = @cod LIMIT 1";

                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        comando.Parameters.AddWithValue("@cod", cod);
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read()) // se encontrou o produto
                            {
                                textBoxDescricao.Text = reader["descricao"].ToString();
                                textBoxQtdeAtacado.Text = reader["qtde_atacado"].ToString();
                                textBoxPrecoCusto.Text = reader["preco_custo"].ToString();
                                textBoxPrecoVenda.Text = reader["preco_venda"].ToString();
                                textBoxSubGrupo.Text = reader["sub_grupo"].ToString();
                                textBoxGrupo.Text = reader["grupo"].ToString();
                                textBoxMarca.Text = reader["marca"].ToString();
                            }
                            else
                            {
                                // Limpa os campos se não encontrou
                                limparCampos();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao buscar produto: " + ex.Message);
                }
            }
        }
        private void textBoxCod_Leave(object sender, EventArgs e)
        {
            BuscarProdutoPorCodigo();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {


        }

        private void textBoxLucroPercent_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButtonSalvar_Click(object sender, EventArgs e)
        {
            // Preenche as variáveis da classe com os valores dos textBoxes
            cod = textBoxCod.Text.Trim();
            descricao = textBoxDescricao.Text.Trim();
            subGrupo = textBoxSubGrupo.Text.Trim();
            grupo = textBoxGrupo.Text.Trim();
            marca = textBoxMarca.Text.Trim();

            // Converte os valores numéricos com segurança
            qtdeAtacado = int.TryParse(textBoxQtdeAtacado.Text.Trim(), out int qt) ? qt : 0;
            precoCusto = decimal.TryParse(textBoxPrecoCusto.Text.Trim(), out decimal pc) ? pc : 0;
            precoVenda = decimal.TryParse(textBoxPrecoVenda.Text.Trim(), out decimal pv) ? pv : 0;

            string conexaoBanco = "server=localhost;user id=root;password=;database=db_loljaauto";

            using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
            {
                try
                {
                    conexao.Open();

                    string sql = @"INSERT INTO tb_produtos 
(codigo, descricao, unidade, preco_custo, lucro_percent, preco_venda, preco_atacado, qtde_atacado, estoque, est_minimo, grupo, subgrupo, referencia, validade, marca, comissao, imagem, created_at, updated_at)
VALUES 
(@cod, @descricao, @unidade, @precoCusto, @lucroPercent, @precoVenda, @precoAtacado, @qtdeAtacado, @estoque, @estMinimo, @grupo, @subGrupo, @referencia, @validade, @marca, @comissao, @imagem, NOW(), NOW())";

                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        // Campos que você usa
                        comando.Parameters.AddWithValue("@cod", cod);
                        comando.Parameters.AddWithValue("@descricao", descricao);
                        comando.Parameters.AddWithValue("@precoCusto", precoCusto);
                        comando.Parameters.AddWithValue("@precoVenda", precoVenda);
                        comando.Parameters.AddWithValue("@qtdeAtacado", qtdeAtacado);
                        comando.Parameters.AddWithValue("@grupo", grupo);
                        comando.Parameters.AddWithValue("@subGrupo", subGrupo);
                        comando.Parameters.AddWithValue("@marca", marca);

                        // Campos opcionais ou não preenchidos, com valores default
                        comando.Parameters.AddWithValue("@unidade", DBNull.Value);
                        comando.Parameters.AddWithValue("@lucroPercent", 0);
                        comando.Parameters.AddWithValue("@precoAtacado", 0);
                        comando.Parameters.AddWithValue("@estoque", 0);
                        comando.Parameters.AddWithValue("@estMinimo", 0);
                        comando.Parameters.AddWithValue("@referencia", DBNull.Value);
                        comando.Parameters.AddWithValue("@validade", DBNull.Value);
                        comando.Parameters.AddWithValue("@comissao", 0);
                        comando.Parameters.AddWithValue("@imagem", DBNull.Value);

                        comando.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produto salvo com sucesso!");
                    limparCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar o produto: " + ex.Message);
                }
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja limpar os campos?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            limparCampos();

        }

        private void PDV_Load(object sender, EventArgs e)
        {

        }

        private void textBoxDescricao_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
