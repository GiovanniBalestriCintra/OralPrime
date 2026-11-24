using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Producao
{
    internal class ListaPrecoItem
    {

        // Variáveis
        private int _id;
        private int _listaPrecoId;
        private int _numero;
        private decimal _valor;


        public int Id { get => _id; set => _id = value; }
        public int ListaPrecoId { get => _listaPrecoId; set => _listaPrecoId = value; }
        public int Numero { get => _numero; set => _numero = value; }
        public decimal Valor { get => _valor; set => _valor = value; }


        // Método construtor sem ID.
        public ListaPrecoItem(int listaPrecoId, int numero, decimal valor)
        {
            _listaPrecoId = listaPrecoId;
            _numero = numero;
            _valor = valor;
        }

        // Método construtor com ID.
        public ListaPrecoItem(int id, int listaPrecoId, int numero, decimal valor)
        {
            _id = id;
            _listaPrecoId = listaPrecoId;
            _numero = numero;
            _valor = valor;
        }


        // Inserir
        public static int Inserir(ListaPrecoItem item)
        {
            string sql = "CALL sp_InserirListaPrecoItem(@ListaPrecoId, @Numero, @Valor)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@ListaPrecoId", item.ListaPrecoId);
                cmd.Parameters.AddWithValue("@Numero", item.Numero);
                cmd.Parameters.AddWithValue("@Valor", item.Valor);

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir


        // Atualizar
        // A procedure não troca o item de lista: atualiza só o número e o valor.
        public static int Atualizar(ListaPrecoItem item)
        {
            string sql = "CALL sp_AtualizarListaPrecoItem(@Id, @Numero, @Valor)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Numero", item.Numero);
                cmd.Parameters.AddWithValue("@Valor", item.Valor);

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Atualizar


        // Deletar
        public static int Deletar(ListaPrecoItem item)
        {
            string sql = "CALL sp_DeletarListaPrecoItem(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", item.Id);

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Deletar


        // Lê a linha atual do reader e monta o objeto
        private static ListaPrecoItem MontarItem(MySqlDataReader reader)
        {
            int id = reader.GetInt32("id");
            int listaPrecoId = reader.GetInt32("lista_preco_id");
            int numero = reader.GetInt32("numero");
            decimal valor = reader.GetDecimal("valor");

            return new ListaPrecoItem(id, listaPrecoId, numero, valor);
        }


        // Listar todos
        public static List<ListaPrecoItem> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosListaPrecoItem()";

            List<ListaPrecoItem> itens = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        itens.Add(MontarItem(reader));
                    }
                }

                return itens;
            }
        } // Fim ListarTodos


        // Listar por ID
        public static ListaPrecoItem? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdListaPrecoItem(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                ListaPrecoItem? item = null;

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        item = MontarItem(reader);
                    }
                }

                return item;
            }
        } // Fim ListarPorId


        // Listar os itens de uma lista de preço (ordenados pelo número)
        public static List<ListaPrecoItem> ListarPorListaPreco(int listaPrecoId)
        {
            string sql = "CALL sp_ListarItensPorListaPreco(@ListaPrecoId)";

            List<ListaPrecoItem> itens = new();

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@ListaPrecoId", listaPrecoId);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        itens.Add(MontarItem(reader));
                    }
                }

                return itens;
            }
        } // Fim ListarPorListaPreco


    }// Classe

}// NameSpace