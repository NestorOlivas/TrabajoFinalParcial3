using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrabajoFinalParcial3.Modelos;

namespace TrabajoFinalParcial3
{
    public partial class frmResumen : Form
    {
        string fecha;
        Usuario user = new Usuario();
        Venta venta = new Venta();

        public frmResumen()
        {
            InitializeComponent();
        }

        public frmResumen(Usuario usuario)
        {
            InitializeComponent();

            this.user = usuario;
        }

        private void frmResumen_Load(object sender, EventArgs e)
        {
            cargardg();
        }

        void cargardg()
        {
            try
            {
                fecha = dtpFecha.Value.Year + "-" + dtpFecha.Value.Month + "-" + dtpFecha.Value.Day;
                SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561");
                string query = $"select Ven_Folio as 'Folio', Ven_Importe as 'Importe', Ven_Fecha as 'Fecha', Ven_IdUsuario as 'Empleado' from Venta where Ven_Fecha = '{fecha}'";
                DataTable dt = new DataTable();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
                dgVentas.DataSource = dt;
                con.Close();
            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            cargardg();
        }

        private void dgVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(user.Us_Rol == 2)
            {
                DataGridViewRow dgr = dgVentas.Rows[e.RowIndex];
                venta.Ven_Folio = Convert.ToInt32(dgr.Cells[0].Value);
                venta.Ven_Importe = Convert.ToString(dgr.Cells[1].Value);
                venta.Ven_Fecha = Convert.ToString(dgr.Cells[2].Value);
                venta.Ven_IdUsuario = Convert.ToString(dgr.Cells[3].Value);
                //venta.Ven_Estatus = Convert.ToInt32(dgr.Cells[4].Value);

                frmInformacionVenta x = new frmInformacionVenta(user, venta);
                x.ShowDialog();
            }
        }
    }
}
