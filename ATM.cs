using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ATMSim
{
    public class ATMNoEstaRegistradoException : Exception { }

    internal class ATM
    {
        private const int TAMANO_LLAVE = 32; // bytes


        byte[]? tpk;
        public ATMSwitch? Host { get; set; } 
        public string Nombre { get; set; }
        public ATM(string nombre)
        {
            Nombre = nombre;
        }

        
        public int Retirar(Tarjeta tarjeta, int pin, int monto)
        {
            byte[] criptogramaPin = Encriptar(pin.ToString());
            int codigoRespuesta = host.Autorizar(TipoTransaccion.Retiro, tarjeta, monto, criptogramaPin);
            if (codigoRespuesta == 0)
                return monto;
            else
                throw new Exception("Declinado");
        }

        public decimal ConsultarBalance(Tarjeta tarjeta, int Pin)
        {
            byte[] criptogramaPin = Encriptar(pin.ToString());
            int codigoRespuesta = host.Autorizar(TipoTransaccion.Retiro, tarjeta, criptogramaPin);
            if (codigoRespuesta == 0)
                return 0;
            else
                throw new Exception("Declinado");
        }

        public void InstalarLlave(byte[] llave)
        {
            tpk = llave;
        }

        public void Reestablecer()
        {
            tpk = null;
            Host = null;
        }


        public byte[] Encriptar(string textoPlano)
        {
            byte[] llave = tpk.Skip(0).Take(TAMANO_LLAVE).ToArray();
            byte[] iv = tpk.Skip(TAMANO_LLAVE).ToArray();
            using (Aes llaveAes = Aes.Create())
            {
                llaveAes.Key = llave;
                llaveAes.IV = iv;

                ICryptoTransform encriptador = llaveAes.CreateEncryptor();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encriptador, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(textoPlano);
                        }
                        return ms.ToArray();
                    }
                }

            }


        }

    }
}
