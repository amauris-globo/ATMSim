using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSim
{
    public enum Denominacion
    {
        DOP200,
        DOP500,
        DOP1000,
        DOP2000,
    }
    public class Billete
    {
        public Denominacion Denominacion { get; set; }


    }
}
