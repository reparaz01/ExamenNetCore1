using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ExamenNetCore1.Models;
using ExamenNetCore1.Repositories;

namespace ExamenNetCore1
{


    #region PROCEDIMIENTOS ALMACENADOS

    /*
     * 
    -- PROCEDIMIENTO 1

    create procedure SP_CLIENTES
    AS
        select * from clientes
    GO

     -- PROCEDIMIENTO 2

    create procedure SP_FIND_CLIENTE
    @nombreEmpresa NVARCHAR(100)
    AS

    SELECT *
    FROM clientes
    WHERE Empresa = @nombreEmpresa;
    GO

     -- PROCEDIMIENTO 3

    create procedure SP_FIND_PEDIDOS_CLIENTE
        @codigoCliente NVARCHAR(100)
    AS
	SELECT * FROM pedidos
	WHERE CodigoCliente = @codigoCliente;
    GO




    *
    */


    #endregion



    public partial class FormPractica : Form
    {

        RepositoryPractica repo;

        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryPractica();
            loadClientes();
        }

        public void loadClientes()
        {
            List<string> clientes = this.repo.GetClientes();
            foreach (string data in clientes)

            {
                this.cmbclientes.Items.Add(data);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreEmpresa = this.cmbclientes.SelectedItem.ToString();


            int index = this.cmbclientes.SelectedIndex;
            if (index != -1)
            {
                Cliente cliente = new Cliente();
                cliente = this.repo.findCliente(nombreEmpresa);

                this.txtempresa.Text = cliente.Empresa;
                this.txtcontacto.Text = cliente.Contacto;
                this.txtcargo.Text = cliente.Cargo;
                this.txtciudad.Text = cliente.Ciudad;
                this.txttelefono.Text = cliente.Telefono.ToString();

                string codigoCliente = cliente.CodigoCliente;
                List<Pedido> pedidos = this.repo.findPedidos(codigoCliente);


                this.lstpedidos.Items.Clear();
                foreach (Pedido p in pedidos)
                {
                    this.lstpedidos.Items.Add(p.CodigoPedido);
                }

            }


        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lstpedidos.SelectedIndex;
            if (index != -1)
            {

                string nombreEmpresa = this.cmbclientes.SelectedItem.ToString();
                Cliente cliente = new Cliente();
                cliente = this.repo.findCliente(nombreEmpresa);


                string codigoCliente = cliente.CodigoCliente;
                List<Pedido> pedidos = this.repo.findPedidos(codigoCliente);

                string codigoPedido = this.lstpedidos.SelectedItem.ToString();
                Pedido pedido = this.repo.findPedido(codigoPedido);

                this.txtcodigopedido.Text = pedido.CodigoPedido;
                this.txtfechaentrega.Text = pedido.FechaEntrega;
                this.txtformaenvio.Text = pedido.FormaEnvio;
                this.txtimporte.Text = pedido.Importe.ToString();


            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            string codigopedido = this.txtcodigopedido.Text;
            string codigocliente = this.cmbclientes.SelectedItem.ToString();
            DateTime fechaentrega = DateTime.Parse(this.txtfechaentrega.Text);
            string formaenvio = this.txtformaenvio.Text;
            int importe = int.Parse(this.txtimporte.Text);

            int insertados = this.repo.InsertarPedido(codigopedido, codigocliente, fechaentrega, formaenvio, importe);


            List<Pedido> pedidos = this.repo.findPedidos(codigocliente);


            this.lstpedidos.Items.Clear();
            foreach (Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodigoPedido);
            }


            MessageBox.Show("Pedidos Insertados: " + insertados);
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            int index = this.lstpedidos.SelectedIndex;

            if (index != 1)
            {

                Pedido pedido = new Pedido();
                string codigopedido = this.txtcodigopedido.Text;

                int eliminados = this.repo.EliminarPedido(codigopedido);

                string codigocliente = this.cmbclientes.SelectedItem.ToString();    

                List<Pedido> pedidos = this.repo.findPedidos(codigocliente);


                this.lstpedidos.Items.Clear();
                foreach (Pedido p in pedidos)
                {
                    this.lstpedidos.Items.Add(p.CodigoPedido);
                }

                MessageBox.Show("Pedido Eliminado");


            }
        }
    }
}
