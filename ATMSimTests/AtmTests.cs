using ATMSim;
using ATMSimTests.Fakes;
using FluentAssertions;

namespace ATMSimTests
{
    public class AtmTests
    {
        private class TestData
        {
            public ATM atm;
            public string cardNumber;
            public string accountNumber;
            public IConsoleWriter consoleWriter;
            public IThreadSleeper threadSleeper;
        }

        private TestData PrepareAtmHappyPath(int accountBalance, string cardPin)
        {
            IConsoleWriter consoleWriter = new FakeConsoleWriter();
            IThreadSleeper threadSleeper = new FakeThreadSleeper();


            IHSM hsm = new HSM();


            ATM atm = new ATM("AJP001", consoleWriter, threadSleeper);


            IAutorizador autorizadorDebito = new Autorizador("AutDB", hsm);
            string numeroCuenta = autorizadorDebito.CrearCuenta(TipoCuenta.Corriente, accountBalance);
            string numeroTarjeta = autorizadorDebito.CrearTarjeta("459413", numeroCuenta);
            autorizadorDebito.AsignarPin(numeroTarjeta, cardPin);

            IATMSwitch atmSwitch = new ATMSwitch(hsm, consoleWriter);
            atmSwitch.AgregarConfiguracionOpKey(new ConfiguracionOpKey()
            {
                Teclas = "AAA",
                TipoTransaccion = TipoTransaccion.Retiro,
                Recibo = true
            });
            atmSwitch.AgregarConfiguracionOpKey(new ConfiguracionOpKey()
            {
                Teclas = "AAC",
                TipoTransaccion = TipoTransaccion.Retiro,
                Recibo = false
            });
            atmSwitch.AgregarConfiguracionOpKey(new ConfiguracionOpKey()
            {
                Teclas = "B",
                TipoTransaccion = TipoTransaccion.Consulta,
                Recibo = false
            });

            atmSwitch.RegistrarATM(atm);
            atmSwitch.RegistrarAutorizador(autorizadorDebito);
            atmSwitch.AgregarRuta("459413", autorizadorDebito.Nombre);

            return new TestData() { atm = atm, cardNumber = numeroTarjeta, accountNumber = numeroCuenta, 
                consoleWriter = consoleWriter, threadSleeper = threadSleeper };
        }


        [Fact]
        public void Withdrawal_with_balance_on_account_is_successful()
        {
            TestData testData = PrepareAtmHappyPath(10_000, "1234");
            ATM atm = testData.atm;
            string cardNumber = testData.cardNumber;
            FakeConsoleWriter consoleWriter = (FakeConsoleWriter) testData.consoleWriter;
            FakeThreadSleeper threadSleeper = (FakeThreadSleeper) testData.threadSleeper;

            atm.EnviarTransactionRequest("AAA", cardNumber, "1234", 100);

            consoleWriter.consoleText.Should().Contain("> Efectivo dispensado: 100");

        }
    }
}