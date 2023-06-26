using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsandoBanco.BancoDados;

namespace UsandoBanco
{
    internal class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }

        public Produto() 
        {
            Console.Write("Informe o nome do produto: ");
            Nome = Console.ReadLine();

            Console.Write("Informe o código do produto: ");
            Codigo = Console.ReadLine();

            Console.Write("Informe o valor do produto: ");
            decimal valor = 0;
            while (!decimal.TryParse(Console.ReadLine(),out valor))
            {
                Console.Write("Valor inválido!");
                Thread.Sleep(2000);

                Console.Write("Informe o valor do produto: ");
            }

            Valor = valor;

            Console.Write("Informe a descrição do produto: ");
            Descricao = Console.ReadLine();
        }

        public Produto(int id, string nome, string codigo, decimal valor, string descricao)
        {
            Id = id;
            Nome = nome;
            Codigo = codigo;
            Valor = valor;
            Descricao = descricao;
        }

        public Produto(long id)
        {
            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT ID, Nome, Codigo, Valor, Descriçao FROM PRODUTO WHERE ID = @ID";
                cmd.Parameters.AddWithValue("id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Id = reader.GetInt32(0);
                        Nome = reader.GetString(1);
                        Codigo = reader.GetString(2);
                        Valor = reader.GetDecimal(3);
                        Descricao = reader.GetString(4);
                    }
                }
            }
        }

        public bool IsValid()
        {
            return Id > 0;
        }

        public void Save()
        {
            using(var conn = new SqlConnection(DBInfo.DBConnection)) 
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = "INSERT INTO PRODUTO (Nome, Codigo, Valor, Descriçao) VALUES (@Nome, @Codigo, @Valor, @Descricao)";
                cmd.Parameters.AddWithValue("Nome", Nome);
                cmd.Parameters.AddWithValue("Codigo", Codigo);
                cmd.Parameters.AddWithValue("Valor", Valor);
                cmd.Parameters.AddWithValue("Descricao", Descricao);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpDate()
        {
            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = "UPDATE PRODUTO SET NOME = @Nome, CODIGO = @Codigo, VALOR = @Valor, DESCRICAO = @Descriçao WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Id);
                cmd.Parameters.AddWithValue("@Nome", Nome);
                cmd.Parameters.AddWithValue("@Codigo", Codigo);
                cmd.Parameters.AddWithValue("@Valor", Valor);
                cmd.Parameters.AddWithValue("@Descricao", Descricao);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            using(var conn = new SqlConnection( DBInfo.DBConnection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = "DELETE FROM PRODUTO WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public static List<Produto> GetAll()
        {
            var result = new List<Produto>();

            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT ID, Nome, Codigo, Valor, Descriçao FROM PRODUTO";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var produto = new Produto(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDecimal(3), reader.GetString(4));
                        result.Add(produto);
                    }
                }
            }

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Nome: {Nome}");
            sb.AppendLine($"Código: {Codigo}");
            sb.AppendLine($"Valor: {Valor}");
            sb.AppendLine($"Descrição: {Descricao}");

            return sb.ToString();
        }
    }

}
