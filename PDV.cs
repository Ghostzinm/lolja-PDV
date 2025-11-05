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

            // Vincular eventos dos botões
            this.Shown += PDV_Shown;
            this.Load += PDV_Load;

            toolStripButtonProx.Click += toolStripButtonProx_Click;
            toolStripButtonant.Click += toolStripButtonAnt_Click;
            toolStripButtonSalvar.Click += toolStripButtonSalvar_Click;
            toolStripButtonNovo.Click += toolStripButton10_Click; // Botão Novo Produto
            toolStripButtonExcluir.Click += toolStripButtonExcluir_Click; // Botão Excluir Produto
            toolStripButtonCancelar.Click += toolStripButtonCancelar_Click; // Botão Cancelar
        }

        private void PDV_Load(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        private void PDV_Shown(object sender, EventArgs e)
        {
            
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
                        toolStripButtonLimpar_Click(null, null);

                    // Desabilita o botão Cancelar quando a lista de produtos for carregada
                    toolStripButtonCancelar.Enabled = false;
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

            AtualizarBotoes();
        }

        private void AtualizarBotoes()
        {
            toolStripButtonProx.Enabled = indiceAtual < produtos.Rows.Count - 1;
            toolStripButtonant.Enabled = indiceAtual > 0;
            toolStripButtonExcluir.Enabled = indiceAtual >= 0 && produtos.Rows.Count > 0;
        }

        // ========================= NOVO PRODUTO =========================
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            toolStripButtonLimpar_Click(null, null); // Limpa todos os campos
            toolStripButtonCancelar.Enabled = true; // Habilita o botão Cancelar
            toolStripButtonSalvar.Enabled = true; // Habilita o botão Salvar
            indiceAtual = -1; // Define que é um novo produto
            AtualizarBotoes();
        }

        private void toolStripButtonCancelar_Click(object sender, EventArgs e)
        {
            // Cancela a operação de novo produto
            CarregarProdutos(); // Volta à lista de produtos carregada
            toolStripButtonCancelar.Enabled = false; // Desabilita o botão Cancelar
            toolStripButtonSalvar.Enabled = false; // Desabilita o botão Salvar
            AtualizarBotoes(); // Atualiza os botões de navegação
        }

        // ========================= EXCLUIR PRODUTO =========================
        private void toolStripButtonExcluir_Click(object sender, EventArgs e)
        {
            if (indiceAtual < 0 || produtos.Rows.Count == 0)
            {
                MessageBox.Show("Nenhum produto selecionado para excluir.");
                return;
            }

            DataRow row = produtos.Rows[indiceAtual];

            if (!int.TryParse(row["id"].ToString(), out int id))
            {
                MessageBox.Show("ID do produto inválido.");
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"Deseja realmente excluir o produto '{row["descricao"]}'?",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado != DialogResult.Yes)
                return;

            using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
            {
                try
                {
                    conexao.Open();
                    string sqlDelete = "DELETE FROM tb_produtos WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(sqlDelete, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produto excluído com sucesso!");
                    produtos.Rows.RemoveAt(indiceAtual);

                    if (produtos.Rows.Count > 0)
                    {
                        MostrarProduto(Math.Min(indiceAtual, produtos.Rows.Count - 1)); // Exibe o próximo ou anterior produto
                    }
                    else
                    {
                        toolStripButtonLimpar_Click(null, null); // Limpa os campos se não houver mais produtos
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir produto: " + ex.Message);
                }
            }
        }

        // ========================= SALVAR (INSERT OU UPDATE) =========================
        private void toolStripButtonSalvar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conexao = new MySqlConnection(conexaoBanco))
            {
                try
                {
                    conexao.Open();

                    if (indiceAtual == -1) // Novo produto
                    {
                        string sqlInsert = @"INSERT INTO tb_produtos
                            (codigo, descricao, unidade, preco_custo, lucro_percent, preco_venda, preco_atacado, qtde_atacado, est_minimo, grupo, subgrupo, referencia, validade, marca, comissao)
                            VALUES
                            (@codigo, @descricao, @unidade, @preco_custo, @lucro_percent, @preco_venda, @preco_atacado, @qtde_atacado, @est_minimo, @grupo, @subgrupo, @referencia, @validade, @marca, @comissao)";

                        using (MySqlCommand cmd = new MySqlCommand(sqlInsert, conexao))
                        {
                            cmd.Parameters.AddWithValue("@codigo", textBoxCod.Text);
                            cmd.Parameters.AddWithValue("@descricao", textBoxDescricao.Text);
                            cmd.Parameters.AddWithValue("@unidade", comboBoxUnidade.SelectedItem?.ToString() ?? "");
                            cmd.Parameters.AddWithValue("@preco_custo", Convert.ToDecimal(textBoxPrecoCusto.Text));
                            cmd.Parameters.AddWithValue("@lucro_percent", Convert.ToDecimal(textBoxLucroPercent.Text));
                            cmd.Parameters.AddWithValue("@preco_venda", Convert.ToDecimal(textBoxPrecoVenda.Text));
                            cmd.Parameters.AddWithValue("@preco_atacado", Convert.ToDecimal(textBoxPrecoAtacado.Text));
                            cmd.Parameters.AddWithValue("@qtde_atacado", Convert.ToInt32(textBoxQtdeAtacado.Text));
                            cmd.Parameters.AddWithValue("@est_minimo", Convert.ToInt32(textBoxEstMinimo.Text));
                            cmd.Parameters.AddWithValue("@grupo", textBoxGrupo.Text);
                            cmd.Parameters.AddWithValue("@subgrupo", textBoxSubGrupo.Text);
                            cmd.Parameters.AddWithValue("@referencia", textBoxReferencia.Text);
                            cmd.Parameters.AddWithValue("@validade", dateTimePickerValidade.Value);
                            cmd.Parameters.AddWithValue("@marca", textBoxMarca.Text);
                            cmd.Parameters.AddWithValue("@comissao", Convert.ToDecimal(textBoxComissao.Text));

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Produto criado com sucesso!");
                        }
                    }
                    else // Atualizar produto existente
                    {
                        DataRow row = produtos.Rows[indiceAtual];
                        if (!int.TryParse(row["id"].ToString(), out int id))
                        {
                            MessageBox.Show("ID do produto inválido.");
                            return;
                        }

                        string sqlUpdate = @"UPDATE tb_produtos SET
                            codigo = @codigo,
                            descricao = @descricao,
                            unidade = @unidade,
                            preco_custo = @preco_custo,
                            lucro_percent = @lucro_percent,
                            preco_venda = @preco_venda,
                            preco_atacado = @preco_atacado,
                            qtde_atacado = @qtde_atacado,
                            est_minimo = @est_minimo,
                            grupo = @grupo,
                            subgrupo = @subgrupo,
                            referencia = @referencia,
                            validade = @validade,
                            marca = @marca,
                            comissao = @comissao
                            WHERE id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(sqlUpdate, conexao))
                        {
                            cmd.Parameters.AddWithValue("@codigo", textBoxCod.Text);
                            cmd.Parameters.AddWithValue("@descricao", textBoxDescricao.Text);
                            cmd.Parameters.AddWithValue("@unidade", comboBoxUnidade.SelectedItem?.ToString() ?? "");
                            cmd.Parameters.AddWithValue("@preco_custo", Convert.ToDecimal(textBoxPrecoCusto.Text));
                            cmd.Parameters.AddWithValue("@lucro_percent", Convert.ToDecimal(textBoxLucroPercent.Text));
                            cmd.Parameters.AddWithValue("@preco_venda", Convert.ToDecimal(textBoxPrecoVenda.Text));
                            cmd.Parameters.AddWithValue("@preco_atacado", Convert.ToDecimal(textBoxPrecoAtacado.Text));
                            cmd.Parameters.AddWithValue("@qtde_atacado", Convert.ToInt32(textBoxQtdeAtacado.Text));
                            cmd.Parameters.AddWithValue("@est_minimo", Convert.ToInt32(textBoxEstMinimo.Text));
                            cmd.Parameters.AddWithValue("@grupo", textBoxGrupo.Text);
                            cmd.Parameters.AddWithValue("@subgrupo", textBoxSubGrupo.Text);
                            cmd.Parameters.AddWithValue("@referencia", textBoxReferencia.Text);
                            cmd.Parameters.AddWithValue("@validade", dateTimePickerValidade.Value);
                            cmd.Parameters.AddWithValue("@marca", textBoxMarca.Text);
                            cmd.Parameters.AddWithValue("@comissao", Convert.ToDecimal(textBoxComissao.Text));
                            cmd.Parameters.AddWithValue("@id", id);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Produto atualizado com sucesso!");
                        }
                    }

                    CarregarProdutos(); // Atualiza a lista de produtos após salvar
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar produto: " + ex.Message);
                }
            }
        }

        // ========================= NAVIGAÇÃO =========================
        private void toolStripButtonProx_Click(object sender, EventArgs e)
        {
            if (indiceAtual < produtos.Rows.Count - 1)
            {
                MostrarProduto(indiceAtual + 1);
            }
        }

        private void toolStripButtonAnt_Click(object sender, EventArgs e)
        {
            if (indiceAtual > 0)
            {
                MostrarProduto(indiceAtual - 1);
            }
        }



        private void toolStripButtonLimpar_Click(object sender, EventArgs e)
        {
            textBoxCod.Clear();
            textBoxDescricao.Clear();
            comboBoxUnidade.SelectedItem = null;
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
            AtualizarBotoes();
        }
    }
}
