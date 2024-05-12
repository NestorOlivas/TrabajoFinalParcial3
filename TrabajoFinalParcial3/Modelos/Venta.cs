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
    public class Venta
    {
        public int Ven_Folio { get; set; }
        public string Ven_Importe { get; set; }
        public string Ven_Fecha { get; set; }
        public string Ven_IdUsuario {  get; set; }
        public int Ven_Estatus { get; set; }


        public Boolean ObetenerIdMax(ref DataTable tabla)
        {
            Boolean bAllok = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = localhost\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561; Persist security Info = true"))
                {
                    connection.Open();
                    string Query = $"select max(Ven_Folio)+1 from Venta";
                    SqlCommand command = new SqlCommand(Query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(tabla);
                    bAllok = true;
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                string x = e.Message;
                MessageBox.Show("Error: " + x);
            }
            return bAllok;

        }

        public bool Guardar(int Folio, string Importe, string Fecha, string IdUsuario, int Estatus)
        {
            Boolean b = false;
            try
            {
                using (SqlConnection connection = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561"))
                {
                    // Abrir la conexión
                    connection.Open();
                    // Crear el comando SQL
                    string consulta = @"INSERT INTO Venta (Ven_Folio, Ven_Importe, Ven_Fecha, Ven_IdUsuario, Ven_Estatus)
                            VALUES (@Folio, @Importe, @Fecha, @IdUsuario, @Estatus)";
                    SqlCommand comando = new SqlCommand(consulta, connection);

                    // Agregar los parámetros
                    comando.Parameters.AddWithValue("@Folio", Folio);
                    comando.Parameters.AddWithValue("@Importe", Importe);
                    comando.Parameters.AddWithValue("@Fecha", Fecha);
                    comando.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    comando.Parameters.AddWithValue("@Estatus", Estatus);

                    // Ejecutar el comando
                    comando.ExecuteNonQuery();
                    b = true;
                    connection.Close();
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
