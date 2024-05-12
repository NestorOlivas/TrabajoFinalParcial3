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
    public partial class frmVenta : Form
    {
        Usuario user = new Usuario();
        Venta venta = new Venta();
        DataTable dataTable1 = new DataTable();
        string idMax = "";
        int ID = 0;

        public static string Key = "3Ad2";
        public static string Resultado = "";
        public frmVenta()
        {
            InitializeComponent();
        }

        public frmVenta(Usuario usuario)
        {
            InitializeComponent();

            user = usuario;

            GenerarFolio();
        }
        void GenerarFolio()
        {
            if (venta.ObetenerIdMax(ref dataTable1))
            {
                foreach (DataRow row in dataTable1.Rows)
                {
                    idMax = row[0].ToString();
                }
                if (idMax == null | idMax == string.Empty)
                {
                    idMax = "1";
                }
            }
            else
            {
                idMax = "1";
            }
            txtFolio.Text = idMax;
        }
        private void frmVenta_Load(object sender, EventArgs e)
        {
            cargardg();
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            
            DataTable dataTable = new DataTable();
            Producto producto = new Producto();
            if(txtIdProducto.TextLength > 0)
            {
                ID = Convert.ToInt32(txtIdProducto.Text);
                if (producto.BuscarProducto(ref dataTable, ID))
                {
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontró el producto");
                    }
                    else
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            producto.Prod_Id = Convert.ToInt32(dr[0]);
                            producto.Prod_Nom = dr[1].ToString();
                            producto.Prod_Costo = Convert.ToDecimal(dr[2]);

                        }

                        txtProducto.Text = producto.Prod_Nom;
                        txtPrecio.Text = producto.Prod_Costo.ToString();

                    }
                }
            }
            else
            {
                MessageBox.Show("No se encontró el producto");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                VentaDet ventaDet = new VentaDet();
                ventaDet.Guardar(Convert.ToInt32(idMax), Convert.ToInt32(txtIdProducto.Text), Convert.ToDecimal(txtPrecio.Text), Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtSubtotal.Text), Convert.ToInt32(user.Us_Id));

                dgVentaDetalle.DataSource = null;
                dgVentaDetalle.Refresh();
                cargardg();
                limpiar();
            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        void cargardg()
        {
            SqlConnection con = new SqlConnection($"Data Source = localhost\\SQLSERVER2019; Initial Catalog = TrabajoParcial3;User = sa; Password = 21030561");
            string query = $"Select * from VentaDetalles where Folio = {Convert.ToInt32(txtFolio.Text)} and Empleado = {user.Us_Id}";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            dgVentaDetalle.DataSource = dt;
            con.Close();
        }

        void limpiar()
        {
            txtIdProducto.Clear();
            txtProducto.Clear();
            txtCantidad.Clear();
            txtPrecio.Clear();
            txtSubtotal.Clear();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            if (txtCantidad.TextLength > 0 && txtPrecio.TextLength > 0)
            {
                int cantidad = 0;
                decimal subtotal = 0;
                cantidad = Convert.ToInt32(txtCantidad.Text);
                subtotal = cantidad * Convert.ToDecimal(txtPrecio.Text);
                txtSubtotal.Text = subtotal.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int Folio = 0, IdUsuario = 0, Estatus = 1;
            decimal Importe = 0;
            string Fecha = dtpFecha.Value.Year + "-" + dtpFecha.Value.Month + "-" + dtpFecha.Value.Day; ;
            Folio = Convert.ToInt32(txtFolio.Text);
            IdUsuario = user.Us_Id;

            string ImporteEncrip = "", UsuarioEncrip = "";

            try
            {
                foreach (DataGridViewRow row in dgVentaDetalle.Rows)
                {
                    if (row.Cells["SubTotal"].Value != null)
                        Importe += (decimal)row.Cells["SubTotal"].Value;
                }

                //-----------------------------Encriptar datos-----------------------------
                EncriptarRSA(Importe.ToString());
                ImporteEncrip = encriptar(Resultado);

                EncriptarRSA(IdUsuario.ToString());
                UsuarioEncrip = encriptar(Resultado);
                //-----------------------------Encriptar datos-----------------------------

                Venta venta = new Venta();
                venta.Guardar(Folio, ImporteEncrip, Fecha, UsuarioEncrip, Estatus);
                MessageBox.Show("La venta se guardo con exito");
                GenerarFolio();
            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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

        static string encriptar(string textoplano)
        {
            var des = CrearDES(Key);
            var ct = des.CreateEncryptor();
            var entrada = Encoding.UTF8.GetBytes(textoplano);
            var salida = ct.TransformFinalBlock(entrada, 0, entrada.Length);
            return Convert.ToBase64String(salida);
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

        void EncriptarRSA(string dato)
        {
            Resultado = "";
            MessageBox.Show("Selecionar llave publica");
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = "Llave publica en XML (*.xml)|*xml";
            if (opd.ShowDialog() == DialogResult.OK)
            {
                Stream tempstream;
                if ((tempstream = opd.OpenFile()) != null)
                {
                    string xmlfile = new StreamReader(tempstream).ReadToEnd();
                    byte[] datoenc = cRSA.encriptar(dato, xmlfile);
                    Resultado = Convert.ToBase64String(datoenc);
                }
            }
        }

        private void btnResumen_Click(object sender, EventArgs e)
        {
            frmResumen x = new frmResumen(user);
            x.ShowDialog();
        }

        private void txtIdProducto_TextChanged(object sender, EventArgs e)
        {
            //
        }
    }
}
