using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrabajoFinalParcial3.Clases
{
    internal class cRSA
    {
        public RSACryptoServiceProvider servicioRSA { get; set; }

        public cRSA()
        {
            this.servicioRSA = new RSACryptoServiceProvider();
        }

        public byte[] llavepublica()
        {
            string xmlpublico = this.servicioRSA.ToXmlString(false);
            return Encoding.ASCII.GetBytes(xmlpublico);
        }
        public byte[] llaveprivada()
        {
            string xmlprivada = this.servicioRSA.ToXmlString(true);
            return Encoding.ASCII.GetBytes(xmlprivada);
        }

        public static byte[] encriptar(string texto, string xmlpublica)
        {
            RSACryptoServiceProvider rsae = new RSACryptoServiceProvider(1024);
            rsae.FromXmlString(xmlpublica);
            byte[] DatosEncriptados = rsae.Encrypt(Encoding.ASCII.GetBytes(texto), false);
            return DatosEncriptados;
        }

        public static byte[] dencriptar(string textoencriptado, string xmlprivada)
        {
            RSACryptoServiceProvider rsad = new RSACryptoServiceProvider(1024);
            rsad.FromXmlString(xmlprivada);
            byte[] DatosDesencriptados = rsad.Decrypt(Convert.FromBase64String(textoencriptado), false);
            return DatosDesencriptados;
        }
    }
}
