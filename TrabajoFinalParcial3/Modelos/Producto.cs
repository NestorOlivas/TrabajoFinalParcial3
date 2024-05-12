using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoFinalParcial3.Modelos
{
    public class Producto
    {
        public int Prod_Id { get; set; }
        public string Prod_Nom {  get; set; }
        public decimal Prod_Costo { get; set; }

        public Boolean BuscarProducto(ref DataTable tabla, int id)
        {
            Boolean b = false;
            try
            {
                using (SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561"))
                {
                    con.Open();
                    string Query = $"Select * from Producto where Prod_Id = '{id}'";
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(tabla);
                    b = true;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return b;
        }
    }
}
