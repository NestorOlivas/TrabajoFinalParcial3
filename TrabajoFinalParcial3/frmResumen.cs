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

namespace TrabajoFinalParcial3
{
    public partial class frmResumen : Form
    {
        public frmResumen()
        {
            InitializeComponent();
        }

        private void frmResumen_Load(object sender, EventArgs e)
        {
            cargardg();
        }

        void cargardg()
        {
            SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561");
            string query = $"select Ven_Folio as 'Folio', Ven_Importe as 'Importe', Ven_Fecha as 'Fecha', Ven_IdUsuario as 'Empleado' from Venta";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            dgVentas.DataSource = dt;
            con.Close();
        }
    }
}
