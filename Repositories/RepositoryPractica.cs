using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ExamenNetCore1.Models;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace ExamenNetCore1.Repositories
{
    public class RepositoryPractica
    {

        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;


        public RepositoryPractica()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=""NETCORE EXAMEN 1"";Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> GetClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> clientes = new List<string>();
            while (this.reader.Read())
            {
                clientes.Add(this.reader["Empresa"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public Cliente findCliente(string nombreEmpresa)

        {
            SqlParameter pamNombreEmpresa = new SqlParameter("@nombreEmpresa", nombreEmpresa);
            this.com.Parameters.Add(pamNombreEmpresa);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_FIND_CLIENTE";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();

            string CodigoCliente = this.reader["CodigoCliente"].ToString();
            string Empresa = this.reader["Empresa"].ToString();
            string Contacto = this.reader["Contacto"].ToString();
            string Cargo = this.reader["Cargo"].ToString();
            string Ciudad = this.reader["Ciudad"].ToString();
            int Telefono = int.Parse(this.reader["Telefono"].ToString());

            Cliente cliente = new Cliente();
            cliente.CodigoCliente = CodigoCliente;
            cliente.Empresa = Empresa;
            cliente.Contacto = Contacto;
            cliente.Cargo = Cargo;
            cliente.Ciudad = Ciudad;
            cliente.Telefono = Telefono;


            this.reader.Close();
            this.com.Parameters.Clear();
            this.cn.Close();
            return cliente;

        }


        public List<Pedido> findPedidos(string codigoCliente)
        {

            SqlParameter pamCodigoCliente = new SqlParameter("@codigoCliente", codigoCliente);
            this.com.Parameters.Add(pamCodigoCliente);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_FIND_PEDIDOS_CLIENTE";



            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<Pedido> pedidos = new List<Pedido>();
            while (this.reader.Read())
            {
                string CodigoPedido = this.reader["CodigoPedido"].ToString();
                string CodigoCliente = this.reader["CodigoCliente"].ToString();
                string FechaEntrega = this.reader["FechaEntrega"].ToString();
                string FormaEnvio = this.reader["FormaEnvio"].ToString();
                int Importe = int.Parse(this.reader["Importe"].ToString());

                Pedido pedido = new Pedido();
                pedido.CodigoPedido = CodigoPedido;
                pedido.CodigoCliente = CodigoCliente;
                pedido.FechaEntrega = FechaEntrega;
                pedido.FormaEnvio = FormaEnvio;
                pedido.Importe = Importe;


                pedidos.Add(pedido);

            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;


        }

        public Pedido findPedido(string codigoPedido)
        {
            string sql = " SELECT* FROM pedidos WHERE CodigoPedido=@CodigoPedido";
            SqlParameter pamCodigoPedido = new SqlParameter("@CodigoPedido",codigoPedido);

            this.com.Parameters.Add(pamCodigoPedido);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();

            string CodigoPedido = this.reader["CodigoPedido"].ToString();
            string CodigoCliente = this.reader["CodigoCliente"].ToString();
            string FechaEntrega = this.reader["FechaEntrega"].ToString();
            string FormaEnvio = this.reader["FormaEnvio"].ToString();
            int Importe = int.Parse(this.reader["Importe"].ToString());

            Pedido pedido = new Pedido();
            pedido.CodigoPedido = CodigoPedido;
            pedido.CodigoCliente = CodigoCliente;
            pedido.FechaEntrega = FechaEntrega;
            pedido.FormaEnvio = FormaEnvio;
            pedido.Importe = Importe;

            this.reader.Close();
            this.com.Parameters.Clear();
            this.cn.Close();
            return pedido;

        }

        public int InsertarPedido (string codigopedido, string codigocliente, DateTime fechaentrega, string formaenvio, int importe)
        {
            string sql = "insert into pedidos values(@codigopedido,@codigocliente,@fechaentrega,@formaenvio,@importe)";

            SqlParameter pamCodigoPedido = new SqlParameter("@codigopedido", codigopedido);
            SqlParameter pamCodigoCliente = new SqlParameter("@codigocliente", codigocliente);
            SqlParameter pamFechaEntrega = new SqlParameter("@fechaentrega", fechaentrega);
            SqlParameter pamFormaEnvio = new SqlParameter("@formaenvio", formaenvio);
            SqlParameter pamImporte = new SqlParameter("@importe", importe); ;

            this.com.Parameters.Add(pamCodigoPedido);
            this.com.Parameters.Add(pamCodigoCliente);
            this.com.Parameters.Add(pamFechaEntrega);
            this.com.Parameters.Add(pamFormaEnvio);
            this.com.Parameters.Add(pamImporte);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();

            int results = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return results;


        }

        public int EliminarPedido (string codigopedido)
        {

            string sql = "delete from pedidos where CodigoPedido=@codigopedido";
            SqlParameter pamCodigoPedido = new SqlParameter("@codigopedido", codigopedido);
            this.com.Parameters.Add(pamCodigoPedido);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int results = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return results;



        }





    }
}
