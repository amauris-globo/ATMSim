using ATMSim;
using System.Net.NetworkInformation;
using System.Threading;

IConsoleWriter consoleWriter = new ConsoleWriter();
IThreadSleeper threadSleeper = new ThreadSleeper();


IHSM hsm = new HSM();

ComponentesLlave llaveAJP001 = hsm.GenerarLlave();


ATM atm = new ATM("AJP001", consoleWriter, threadSleeper);
atm.InstalarLlave(llaveAJP001.LlaveEnClaro);

ComponentesLlave llaveAutorizadorDebito = hsm.GenerarLlave();

IAutorizador autorizadorDebito = new Autorizador("AutDB", hsm);
string numeroCuenta = autorizadorDebito.CrearCuenta(TipoCuenta.Corriente, 20_000);
string numeroTarjeta = autorizadorDebito.CrearTarjeta("459413", numeroCuenta);
autorizadorDebito.AsignarPin(numeroTarjeta, "1234");
autorizadorDebito.InstalarLlave(llaveAutorizadorDebito.LlaveEncriptada);



IATMSwitch atmSwitch = new ATMSwitch(hsm, consoleWriter);
atmSwitch.AgregarConfiguracionOpKey(new ConfiguracionOpKey() { 
    Teclas = "AAA", 
    TipoTransaccion = TipoTransaccion.Retiro, 
    Recibo = true });
atmSwitch.AgregarConfiguracionOpKey(new ConfiguracionOpKey()
{
    Teclas = "AAC",
    TipoTransaccion = TipoTransaccion.Retiro,
    Recibo = false
});
atmSwitch.AgregarConfiguracionOpKey(new ConfiguracionOpKey() { 
    Teclas = "B", 
    TipoTransaccion = TipoTransaccion.Consulta, 
    Recibo = false });

atmSwitch.RegistrarATM(atm, llaveAJP001.LlaveEncriptada);
atmSwitch.RegistrarAutorizador(autorizadorDebito, llaveAutorizadorDebito.LlaveEncriptada);
atmSwitch.AgregarRuta("459413", autorizadorDebito.Nombre);

EsperarTeclaEnter("Presione ENTER para realizar una consulta de balance");
atm.EnviarTransactionRequest("B", numeroTarjeta, "1234");

EsperarTeclaEnter("Presione ENTER para realizar un retiro de 12,000 sin impresión de recibo");
atm.EnviarTransactionRequest("AAC",numeroTarjeta, "1234", 12_000);

EsperarTeclaEnter("Presione ENTER para realizar un intento retiro de 6,000 pero con pin incorrecto");
atm.EnviarTransactionRequest("AAA", numeroTarjeta, "9999", 6_000);

EsperarTeclaEnter("Presione ENTER para realizar una consulta de balance");
atm.EnviarTransactionRequest("B", numeroTarjeta, "1234");

EsperarTeclaEnter("Presione ENTER para realizar un retiro de 6,500 con recibo");
atm.EnviarTransactionRequest("AAA", numeroTarjeta, "1234", 6_500);

EsperarTeclaEnter("Presione ENTER para realizar un intento de retiro de 4_000 que declinará por fondos insuficientes");
atm.EnviarTransactionRequest("AAA", numeroTarjeta, "1234", 4_000);

EsperarTeclaEnter("Presione ENTER para realizar un retiro de 12,000 sin impresión de recibo");
atm.EnviarTransactionRequest("B", numeroTarjeta, "1234");

EsperarTeclaEnter("Presione ENTER para finalizar");


static void EsperarTeclaEnter(string mensaje)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(mensaje);
    Console.ReadKey();
    Console.ResetColor();
}








