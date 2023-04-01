using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATMSim
{
    public class EntidadYaRegistradaException : Exception
    {
        public EntidadYaRegistradaException() { }
        public EntidadYaRegistradaException(string mensaje) : base(mensaje) { }
    }
    public class EntidadAunNoRegistradaException : Exception
    {
        public EntidadAunNoRegistradaException() { }
        public EntidadAunNoRegistradaException(string mensaje) : base(mensaje) { }
    }





    public enum TipoTransaccion
    {
        Retiro,
        Consulta
    }

    public struct Ruta
    {
        string bin;
        public string Bin
        {
            get { return bin; }
            set
            {
                if (Regex.Match(value, @"[0-9]{1,18}").Success)
                    bin = value;
                else
                    throw new ArgumentException("Debe contener solo numeros");
            }
        }

        public string Destino { get; set; }
    }

    internal class ATMSwitch
    {
        private HSM hsm;
        public Dictionary<string, byte[]> ATMKeys { get; set; } = new Dictionary<string, byte[]>();

        public Dictionary<string, Autorizador> Autorizadores { get; set; } = new Dictionary<string, Autorizador>();

        public List<Ruta> tablaRuteo = new List<Ruta>();

        public ATMSwitch()
        {
            hsm = new HSM();
        }

        public void RegistrarATM(ATM atm)
        {
            if (ATMKeys.ContainsKey(atm.Nombre))
                throw new EntidadYaRegistradaException($"El ATM {atm.Nombre} ya se encuentra registrado");

            byte[] llave = hsm.GenerarLlave();

            ATMKeys[atm.Nombre] = llave;
            atm.InstalarLlave(llave);
            atm.Host = this;
        }

        public void EliminarATM(ATM atm)
        {
            if (!ATMKeys.ContainsKey(atm.Nombre))
                throw new EntidadAunNoRegistradaException($"El ATM {atm.Nombre} no se encuentra registrado");

            atm.Reestablecer();
            ATMKeys.Remove(atm.Nombre);
        }

        public void Autorizar(TipoTransaccion tipoTransaccion, Tarjeta tarjeta, int monto, byte[] CryptogramaPin)
        {

        }

        public void RegistrarAutorizador(Autorizador autorizador)
        {

        }

        public void EliminarAutorizador(Autorizador autorizador)
        {

        }

        public string? ObtenerDestino(string tarjeta)
        {
            try
            {
                return tablaRuteo.Where(x => tarjeta.StartsWith(x.Bin))
                                 .OrderByDescending(x => x.Bin.Length)
                                 .ThenBy(x => x.Destino)
                                 .First()
                                 .Destino;
            }
            catch (InvalidOperationException) // si no se encuentra ninguna
            {
                return null;
            }
        }

        public void AgregarRuta(string bin, string nombreAutorizador)
        {
            if (!Autorizadores.ContainsKey(nombreAutorizador))
                throw new EntidadAunNoRegistradaException($"El Autorizador {nombreAutorizador} no se encuentra registrado");

            // Si existe una ruta con el mismo bin, reemplazar destino
            var rutaExistentes = tablaRuteo.Where(x => x.Bin == bin);
            if (rutaExistentes.Any())
            {
                Ruta rutaExistente = rutaExistentes.Single();
                rutaExistente.Destino = "nombreAutorizador";
            }
            else
            // Si no existe el bin en la tabla de bines, agregarlo
                tablaRuteo.Add(new Ruta { Bin = bin , Destino = nombreAutorizador});

        }

    }
}
