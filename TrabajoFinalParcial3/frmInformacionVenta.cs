using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrabajoFinalParcial3.Clases;
using TrabajoFinalParcial3.Modelos;

namespace TrabajoFinalParcial3
{
    public partial class frmInformacionVenta : Form
    {
        public static string Key = "";
        public static string Resultado = "";
        int IdUsuario = 0;

        Usuario user = new Usuario();
        Venta ventaEncriptada = new Venta();
        public frmInformacionVenta()
        {
            InitializeComponent();
        }

        public frmInformacionVenta(Usuario usuario, Venta venta)
        {
            InitializeComponent();
            this.user = usuario;
            ventaEncriptada = venta;

            lblFecha.Text = ventaEncriptada.Ven_Fecha.ToString();
            txtFolio.Text = ventaEncriptada.Ven_Folio.ToString();
            DesencriptarInformación();
            cargardg();
        }

        private void frmInformacionVenta_Load(object sender, EventArgs e)
        {
            
        }

        void cargardg()
        {
            SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561");
            string query = $"Select * from VentaDetalles where Folio = {Convert.ToInt32(txtFolio.Text)} and Empleado = {IdUsuario}";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            dgVenta.DataSource = dt;
            con.Close();

        }

        void DesencriptarInformación()
        {
            //-------------------------Generar llave de TripleDes--------------------------------
            string id = user.Us_Id.ToString();
            string nom = user.Us_Nombre.Substring(0, 2);
            string rol = user.Us_Rol.ToString();
            Key = id + nom + rol;
            //-------------------------Generar llave de TripleDes--------------------------------

            //-----------------------Desencriptar-------------------------
            Resultado = desencriptar(ventaEncriptada.Ven_Importe);
            DesencriptarRSA();
            txtTotal.Text = Resultado;

            Resultado = desencriptar(ventaEncriptada.Ven_IdUsuario);
            DesencriptarRSA();
            IdUsuario = Convert.ToInt32(Resultado);
            //-----------------------Desencriptar-------------------------


            DataTable dataTable = new DataTable();
            Usuario usuario2 = new Usuario();
            if (usuario2.BuscarUsuarioID(ref dataTable, IdUsuario))
            {
                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró al usuario");
                }
                else
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        usuario2.Us_Id = Convert.ToInt32(dr[0]);
                        usuario2.Us_Nombre = dr[1].ToString();
                        usuario2.Us_Password = dr[2].ToString();
                        usuario2.Us_Rol = Convert.ToInt32(dr[3]);

                    }
                }
            }

            txtEmpleado.Text = usuario2.Us_Nombre;

        }

        void DesencriptarRSA()
        {
            MessageBox.Show("Selecionar llave privada");
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = "Llave privada en XML (*.xml)|*xml";
            if (opd.ShowDialog() == DialogResult.OK)
            {
                Stream tempstream;
                if ((tempstream = opd.OpenFile()) != null)
                {
                    string xmlfile = new StreamReader(tempstream).ReadToEnd();
                    byte[] datoenc = cRSA.dencriptar(Resultado, xmlfile);
                    Resultado = Encoding.ASCII.GetString(datoenc);
                }
            }
        }

        //------------------------------------Metodos TripleDes-----------------------------------------
        static TripleDES CrearDES(string Key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }

        static string desencriptar(string textocifrado)
        {
            var des = CrearDES(Key);
            var ct = des.CreateDecryptor();
            var entrada = Convert.FromBase64String(textocifrado);
            var salida = ct.TransformFinalBlock(entrada, 0, entrada.Length);
            return Encoding.UTF8.GetString(salida);
        }
        //------------------------------------Metodos TripleDes-----------------------------------------
    }
}
