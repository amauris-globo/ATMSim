using ATMSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ATMSimTests.Fakes
{
    public class FakeHSM : IHSM
    {
        public List<byte[]> criptogramasPines = new List<byte[]>();
        public List<byte[]> criptogramasLlaves = new List<byte[]>();

        public HSM hsm;
        public FakeHSM() 
        {
            HSM hsm = new HSM();
        }

        public byte[] EncriptarPinConLlaveMaestra(string pin)
        {
            byte[] criptogramaPin = EncriptarPinConLlaveMaestra(pin);
            criptogramasPines.Add(criptogramaPin);
            return criptogramaPin;
        }

        public byte[] GenerarLlave()
        {
            byte[] criptogramaLlave = GenerarLlave();
            criptogramasLlaves.Add(criptogramaLlave);
            return criptogramaLlave;
        }

        public byte[] TraducirPin(byte[] criptograma, byte[] criptogramaLlaveOrigen, byte[] criptogramaLlaveDestino)
            => hsm.TraducirPin(criptograma, criptogramaLlaveOrigen, criptogramaLlaveDestino);

        public bool ValidarPin(byte[] criptogramaPinAValidar, byte[] criptogramaLlave, byte[] criptogramaPinCorrecto)
            => hsm.ValidarPin(criptogramaPinAValidar, criptogramaLlave, criptogramaPinCorrecto);
    }
}
