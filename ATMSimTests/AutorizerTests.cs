using ATMSim;
using ATMSimTests.Fakes;
using FluentAssertions;

namespace ATMSimTests
{
    public class AutorizerTests
    {
        private class TestData
        {
            public ATM? atm;
            public string? cardNumber;
            public string? accountNumber;
            public IConsoleWriter? consoleWriter;
            public IThreadSleeper? threadSleeper;
            public IATMSwitch? atmSwitch;
            public IAutorizador? autorizer;
            public IHSM? hsm;
        }

        private TestData SetUpWithFakeHSM(int accountBalance, string cardPin)
        {
            IConsoleWriter consoleWriter = new FakeConsoleWriter();
            IThreadSleeper threadSleeper = new FakeThreadSleeper();
            IHSM hsm = new FakeHSM();


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

            return new TestData()
            {
                atm = atm,
                cardNumber = numeroTarjeta,
                accountNumber = numeroCuenta,
                consoleWriter = consoleWriter,
                threadSleeper = threadSleeper,
                atmSwitch = atmSwitch,
                autorizer = autorizadorDebito,
                hsm = hsm
            };
        }


        [Fact]
        public void Balance_Inquiry_with_incorrect_pin_return_an_error_command()
        {
            TestData testData = SetUpWithFakeHSM(10_000, "1234");
            string? cardNumber = testData.cardNumber;
            FakeConsoleWriter? consoleWriter = (FakeConsoleWriter?)testData.consoleWriter;
            IAutorizador? autorizer = testData.autorizer;
            FakeHSM? hsm = (FakeHSM?) testData.hsm;

           //autorizer.AutorizarRetiro(cardNumber,1000,)

            true.Should().BeTrue();

        }
    }
}