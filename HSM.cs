using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ATMSim
{
    internal class HSM
    {
        private const int TAMANO_LLAVE = 32; // bytes

        Aes lmk;
        public HSM()
        {
            this.lmk = Aes.Create();
        }

        public byte[] GenerarLlave()
        {
            Aes llaveAes = Aes.Create();
            byte[] llaveIv = CombinarLlaveConIV(llaveAes.Key, llaveAes.IV);

            return EncriptarLlave(llaveIv);
        }

        public byte[] CombinarLlaveConIV(byte[] llave, byte[] iv) => llave.Concat(iv).ToArray();
        public byte[] ExtraerLlave(byte[] llaveIv) => llaveIv.Skip(0).Take(TAMANO_LLAVE).ToArray();
        public byte[] ExtraerIV(byte[] llaveIv) => llaveIv.Skip(TAMANO_LLAVE).ToArray();


        public byte[] Traducir(byte[] criptograma, byte[] criptogramaLlaveOrigen, byte[] criptogramaLlaveDestino)
        {
            byte[] llaveOrigenIv = DesencriptarLlave(criptogramaLlaveOrigen);

            byte[] llaveDestinoIv = DesencriptarLlave(criptogramaLlaveDestino);

            string textoPlano = Desencriptar(criptograma, llaveOrigenIv);
            byte[] nuevoCriptograma = Encriptar(textoPlano, llaveDestinoIv);

            return nuevoCriptograma;
        }


        public byte[] EncriptarLlave(byte[] llaveIv)
        {    

            ICryptoTransform encriptador = lmk.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encriptador, CryptoStreamMode.Write))
                {
                    cs.Write(llaveIv);
                    return ms.ToArray();
                }
            }
        }





        public byte[] DesencriptarLlave(byte[] criptogramaLlaveIv)
        {
            ICryptoTransform desencriptador = lmk.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(criptogramaLlaveIv))
            {
                using (CryptoStream cs = new CryptoStream(ms, desencriptador, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return ms.ToArray();
                    }
                }
            }
        }

        public byte[] Encriptar(string textoPlano, byte[] llaveIv)
        {
            byte[] llave = ExtraerLlave(llaveIv);
            byte[] iv = ExtraerIV(llaveIv);
            return Encriptar(textoPlano, llave, iv);
        }

        public byte[] Encriptar(string textoPlano, byte[] llave, byte[] iv)
        {
            using (Aes llaveAes = Aes.Create())
            {
                llaveAes.Key = llave;
                llaveAes.IV = iv;
                return Encriptar(textoPlano, llaveAes);
            }
        }

        public byte[] Encriptar(string textoPlano, Aes llave)
        {


            ICryptoTransform encriptador = llave.CreateEncryptor();

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

        public string Desencriptar(byte[] criptograma, byte[] llaveIv)
        {
            byte[] llave = ExtraerLlave(llaveIv);
            byte[] iv = ExtraerIV(llaveIv);
            return Desencriptar(criptograma, llave, iv);
        }

        public string Desencriptar(byte[] criptograma, byte[] llave, byte[] iv)
        {
            using (Aes llaveAes = Aes.Create())
            {
                llaveAes.Key = llave;
                llaveAes.IV = iv;
                return Desencriptar(criptograma, llaveAes);
            }
        }

        public string Desencriptar(byte[] criptograma, Aes llave)
        {
            ICryptoTransform desencriptador = llave.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(criptograma))
            {
                using (CryptoStream cs = new CryptoStream(ms, desencriptador, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }

        }
    }
}
