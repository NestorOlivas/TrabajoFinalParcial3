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
    public class Usuario
    {
        public int Us_Id { get; set; }
        public string Us_Nombre { get; set; }
        public string Us_Password { get; set; }
        public int Us_Rol { get; set; }


        public Boolean BuscarUsuario(ref DataTable tabla, string User)
        {
            Boolean b = false;
            try
            {
                using (SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561"))
                {
                    con.Open();
                    //string Query = $"Select * from Usuario where Us_Nombre = '{User}' and Us_Password = '{Pass}'";
                    string Query = $"Select * from Usuario where Us_Nombre = '{User}'";
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

        public Boolean BuscarUsuarioID(ref DataTable tabla, int id)
        {
            Boolean b = false;
            try
            {
                using (SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561"))
                {
                    con.Open();
                    string Query = $"Select * from Usuario where Us_Id = '{id}'";
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
