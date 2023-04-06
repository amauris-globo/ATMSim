using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ATMSim
{
    public interface IHSM
    {
        public byte[] GenerarLlave();
        public byte[] TraducirPin(byte[] criptograma, byte[] criptogramaLlaveOrigen, byte[] criptogramaLlaveDestino);

        public byte[] EncriptarPinConLlaveMaestra(string pin);

        public bool ValidarPin(byte[] criptogramaPinAValidar, byte[] criptogramaLlave, byte[] criptogramaPinCorrecto);
    }
    public class HSM: IHSM
    {
        private const int TAMANO_LLAVE = 32; // bytes

        private Aes lmk; // llave maestra (Local Master Key)
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

        public byte[] TraducirPin(byte[] criptograma, byte[] criptogramaLlaveOrigen, byte[] criptogramaLlaveDestino)
        {
            byte[] llaveOrigenIv = DesencriptarLlave(criptogramaLlaveOrigen);

            byte[] llaveDestinoIv = DesencriptarLlave(criptogramaLlaveDestino);

            string textoPlano = DesencriptarDato(criptograma, llaveOrigenIv);
            byte[] nuevoCriptograma = EncriptarDato(textoPlano, llaveDestinoIv);

            return nuevoCriptograma;
        }

        public byte[] EncriptarPinConLlaveMaestra(string pin)
        {
            return EncriptarDato(pin, this.lmk);
        }

        public bool ValidarPin(byte[] criptogramaPinAValidar, byte[] criptogramaLlaveAutorizador, byte[] criptogramaPinCorrecto)
        {
            byte[] llaveAutorizador = DesencriptarLlave(criptogramaLlaveAutorizador);

            string pinCorrecto = DesencriptarDato(criptogramaPinCorrecto, this.lmk);
            string pinAValidar = DesencriptarDato(criptogramaPinAValidar, llaveAutorizador);

            return pinCorrecto == pinAValidar;
        }

        private byte[] CombinarLlaveConIV(byte[] llave, byte[] iv) => llave.Concat(iv).ToArray();
        private byte[] ExtraerLlave(byte[] llaveIv) => llaveIv.Skip(0).Take(TAMANO_LLAVE).ToArray();
        private byte[] ExtraerIV(byte[] llaveIv) => llaveIv.Skip(TAMANO_LLAVE).ToArray();


        private byte[] EncriptarLlave(byte[] llaveIv)
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


        private byte[] DesencriptarLlave(byte[] criptogramaLlaveIv)
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

        private byte[] EncriptarDato(string textoPlano, byte[] llaveIv)
        {
            byte[] llave = ExtraerLlave(llaveIv);
            byte[] iv = ExtraerIV(llaveIv);
            return EncriptarDato(textoPlano, llave, iv);
        }

        private byte[] EncriptarDato(string textoPlano, byte[] llave, byte[] iv)
        {
            using (Aes llaveAes = Aes.Create())
            {
                llaveAes.Key = llave;
                llaveAes.IV = iv;
                return EncriptarDato(textoPlano, llaveAes);
            }
        }

        private byte[] EncriptarDato(string textoPlano, Aes llave)
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

        private string DesencriptarDato(byte[] criptograma, byte[] llaveIv)
        {
            byte[] llave = ExtraerLlave(llaveIv);
            byte[] iv = ExtraerIV(llaveIv);
            return DesencriptarDato(criptograma, llave, iv);
        }

        private string DesencriptarDato(byte[] criptograma, byte[] llave, byte[] iv)
        {
            using (Aes llaveAes = Aes.Create())
            {
                llaveAes.Key = llave;
                llaveAes.IV = iv;
                return DesencriptarDato(criptograma, llaveAes);
            }
        }

        private string DesencriptarDato(byte[] criptograma, Aes llave)
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
