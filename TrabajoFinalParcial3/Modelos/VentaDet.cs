using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoFinalParcial3.Modelos
{
    public class VentaDet
    {
        public int Vd_IdVenta {  get; set; }
        public int Vd_IdProd {  get; set; }
        public decimal Vd_ProdPrecio { get; set; }
        public decimal Vd_Cantidad { get; set; }
        public decimal Vd_Total { get; set; }


        public bool Guardar(int Id, int IdProd, decimal Precio, decimal Cantidad, decimal Total, int IdUsuario)
        {
            Boolean b = false;
            try
            {
                using (SqlConnection connection = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561"))
                {
                    // Abrir la conexión
                    connection.Open();
                    // Crear el comando SQL
                    string consulta = @"INSERT INTO VentaDetalle (Vd_IdVenta, Vd_IdProd, Vd_ProdPrecio, Vd_Cantidad, Vd_Total, Vd_IdUsuario)
                            VALUES (@Folio, @IdProd, @ProdPrecio, @Cantidad, @Total, @IdUser)";
                    SqlCommand comando = new SqlCommand(consulta, connection);

                    // Agregar los parámetros
                    comando.Parameters.AddWithValue("@Folio", Id);
                    comando.Parameters.AddWithValue("@IdProd", IdProd);
                    comando.Parameters.AddWithValue("@ProdPrecio", Precio);
                    comando.Parameters.AddWithValue("@Cantidad", Cantidad);
                    comando.Parameters.AddWithValue("@Total", Total);
                    comando.Parameters.AddWithValue("@IdUser", IdUsuario);

                    // Ejecutar el comando
                    comando.ExecuteNonQuery();
                    b = true;
                    connection.Close();
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return b;
        }
    }
}
