using ATMSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ATMSimTests.Fakes
{
    internal class FakeThreadSleeper : IThreadSleeper
    {
        public int sleepedMiliseconds = 0;
        public int numberOfCalls = 0;
        public void Sleep(int miliseconds)
        {
            numberOfCalls++;
            sleepedMiliseconds += miliseconds;
        }
    }
}
