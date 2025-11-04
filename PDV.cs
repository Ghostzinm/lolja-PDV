using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace LOLja
{
    public partial class PDV : Form
    {
        private DataTable produtos = new DataTable();
        private int indiceAtual = 0;
        private string conexaoBanco = "server=localhost;user id=root;password=;database=db_loljaauto";

        public PDV()
        {
            InitializeComponent();

            // Evento disparado quando o Form é exibido
            this.Shown += PDV_Shown;

            // Vincular eventos dos botões
            toolStripButtonProx.Click += toolStripButtonProx_Click;
        }

        private void PDV_Shown(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
            {
                try
                {
                    conexao.Open();
                    string sql = @"SELECT id, codigo, descricao, unidade, preco_custo, lucro_percent, preco_venda, 
                                   preco_atacado, qtde_atacado, est_minimo, grupo, subgrupo, referencia, validade, 
                                   marca, comissao
                                   FROM tb_produtos ORDER BY codigo ASC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conexao))
                    {
                        produtos.Clear();
                        adapter.Fill(produtos);
                    }

                    if (produtos.Rows.Count > 0)
                        MostrarProduto(0);
                    else
                        MessageBox.Show("Nenhum produto encontrado!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
                }
            }
        }

        private void MostrarProduto(int indice)
        {
            if (indice < 0 || indice >= produtos.Rows.Count)
                return;

            indiceAtual = indice;
            DataRow row = produtos.Rows[indiceAtual];

            textBoxCod.Text = row["codigo"].ToString();
            textBoxDescricao.Text = row["descricao"].ToString();
            comboBoxUnidade.SelectedItem = row["unidade"].ToString();
            textBoxPrecoCusto.Text = row["preco_custo"].ToString();
            textBoxLucroPercent.Text = row["lucro_percent"].ToString();
            textBoxPrecoVenda.Text = row["preco_venda"].ToString();
            textBoxPrecoAtacado.Text = row["preco_atacado"].ToString();
            textBoxQtdeAtacado.Text = row["qtde_atacado"].ToString();
            textBoxEstMinimo.Text = row["est_minimo"].ToString();
            textBoxGrupo.Text = row["grupo"].ToString();
            textBoxSubGrupo.Text = row["subgrupo"].ToString();
            textBoxReferencia.Text = row["referencia"].ToString();

            if (DateTime.TryParse(row["validade"].ToString(), out DateTime validade))
                dateTimePickerValidade.Value = validade;
            else
                dateTimePickerValidade.Value = DateTime.Today;

            textBoxMarca.Text = row["marca"].ToString();
            textBoxComissao.Text = row["comissao"].ToString();

            // Atualizar estado dos botões
            AtualizarBotoes();
        }

        private void AtualizarBotoes()
        {
            toolStripButtonProx.Enabled = indiceAtual < produtos.Rows.Count - 1;
        }

        private void toolStripButtonProx_Click(object sender, EventArgs e)
        {
            if (indiceAtual < produtos.Rows.Count - 1)
                MostrarProduto(indiceAtual + 1);
        }

        private void toolStripButtonAnt_Click(object sender, EventArgs e)
        {
            if (indiceAtual > 0)
                MostrarProduto(indiceAtual - 1);
        }

        private void toolStripButtonLimpar_Click(object sender, EventArgs e)
        {
            textBoxCod.Clear();
            textBoxDescricao.Clear();
            comboBoxUnidade.SelectedIndex = -1;
            textBoxPrecoCusto.Clear();
            textBoxLucroPercent.Clear();
            textBoxPrecoVenda.Clear();
            textBoxPrecoAtacado.Clear();
            textBoxQtdeAtacado.Clear();
            textBoxEstMinimo.Clear();
            textBoxGrupo.Clear();
            textBoxSubGrupo.Clear();
            textBoxReferencia.Clear();
            dateTimePickerValidade.Value = DateTime.Today;
            textBoxMarca.Clear();
            textBoxComissao.Clear();

            // Desabilitar botões pois não há produto selecionado
            toolStripButtonProx.Enabled = false;
        }

        private void toolStripButtonant_Click_1(object sender, EventArgs e)
        {

        }
    }
}
