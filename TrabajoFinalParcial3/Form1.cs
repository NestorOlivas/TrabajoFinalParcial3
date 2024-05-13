using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class Form1 : Form
    {
        public static string Key = "";
        public static string Resultado = "";
        public readonly Encoding Encoder = Encoding.UTF8;
        public string pass = "";
        public Form1()
        {
            InitializeComponent();
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

        void EncriptarRSA()
        {
            MessageBox.Show("Selecionar llave publica");
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = "Llave publica en XML (*.xml)|*xml";
            if (opd.ShowDialog() == DialogResult.OK)
            {
                Stream tempstream;
                if ((tempstream = opd.OpenFile()) != null)
                {
                    string xmlfile = new StreamReader(tempstream).ReadToEnd();
                    byte[] datoenc = cRSA.encriptar(txtPass.Text, xmlfile);
                    pass = Convert.ToBase64String(datoenc);
                }
            }
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

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            Usuario usuario = new Usuario();
            if(txtUsuario.Text.Length > 0 && txtPass.Text.Length > 0)
            {
                if (usuario.BuscarUsuario(ref dataTable, txtUsuario.Text))
                {
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontró al usuario");
                    }
                    else
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            usuario.Us_Id = Convert.ToInt32(dr[0]);
                            usuario.Us_Nombre = dr[1].ToString();
                            usuario.Us_Password = dr[2].ToString();
                            usuario.Us_Rol = Convert.ToInt32(dr[3]);

                        }
                        //-------------------------Generar llave de TripleDes--------------------------------
                        string id = usuario.Us_Id.ToString();
                        string nom = usuario.Us_Nombre.Substring(0, 2);
                        string rol = usuario.Us_Rol.ToString();
                        Key = id + nom + rol;
                        //-------------------------Generar llave de TripleDes--------------------------------

                        //-----------------------Desencriptar-------------------------
                        Resultado = desencriptar(usuario.Us_Password);
                        DesencriptarRSA();
                        //-----------------------Desencriptar-------------------------

                        if (Resultado == txtPass.Text)
                        {
                            //MessageBox.Show("Se encontró al usuario: " + usuario.Us_Nombre);
                            frmVenta x = new frmVenta(usuario);
                            this.Hide();
                            x.Show(this);
                        }
                        else
                        {
                            MessageBox.Show("Contraseña incorrecta");
                        }
                        //MessageBox.Show(Key);

                    }
                }
            }
            else
            {
                MessageBox.Show("Favor de ingresar los datos necesarios");
            }
        }
    }
}
