using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSim
{
    public interface IThreadSleeper
    {
        public void Sleep(int miliseconds);
    }

    public class ThreadSleeper : IThreadSleeper
    {
        public void Sleep(int miliseconds)
        {
            Thread.Sleep(miliseconds);
        }
    }
}
