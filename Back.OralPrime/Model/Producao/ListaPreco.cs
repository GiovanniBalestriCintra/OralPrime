using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Producao
{
    internal class ListaPreco
    {

        // Variáveis
        private int _id;
        private string _descricao;


        public int Id { get => _id; set => _id = value; }
        public string Descricao { get => _descricao; set => _descricao = value; }


        // Método construtor sem ID.
        public ListaPreco(string descricao)
        {
            _descricao = descricao;
        }

        // Método construtor com ID.
        public ListaPreco(int id, string descricao)
        {
            _id = id;
            _descricao = descricao;
        }


        // Inserir
        public static int Inserir(ListaPreco listaPreco)
        {
            string sql = "CALL sp_InserirListaPreco(@Descricao)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Descricao", listaPreco.Descricao);

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir


        // Atualizar
        public static int Atualizar(ListaPreco listaPreco)
        {
            string sql = "CALL sp_AtualizarListaPreco(@Id, @Descricao)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", listaPreco.Id);
                cmd.Parameters.AddWithValue("@Descricao", listaPreco.Descricao);

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Atualizar


        // Deletar
        // Os itens (tb_ListaPrecoItem) são apagados junto, por causa do ON DELETE CASCADE.
        // A procedure bloqueia se existir pedido usando esta lista.
        public static int Deletar(ListaPreco listaPreco)
        {
            string sql = "CALL sp_DeletarListaPreco(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", listaPreco.Id);

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Deletar


        // ValidarCampo string
        private static string ValidarCampoString(object campo)
        {
            if (campo == DBNull.Value || campo == null)
            {
                return "";
            }
            else
            {
                return campo.ToString();
            }
        } // Fim ValidarCampo


        // Listar todos
        public static List<ListaPreco> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosListaPreco()";

            List<ListaPreco> listaPrecos = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string descricao = ValidarCampoString(reader["descricao"]);

                        listaPrecos.Add(new ListaPreco(id, descricao));
                    }
                }

                return listaPrecos;
            }
        } // Fim ListarTodos


        // Listar por ID
        public static ListaPreco? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdListaPreco(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                ListaPreco? listaPreco = null;

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int idListado = reader.GetInt32("id");
                        string descricao = ValidarCampoString(reader["descricao"]);

                        listaPreco = new ListaPreco(idListado, descricao);
                    }
                }

                return listaPreco;
            }
        } // Fim ListarPorId


    }// Classe

}// NameSpace