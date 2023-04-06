# Proyecto Final IDS326-L (ATM Sim)

Este proyecto consiste en varias fases, en las cuales ustedes estarían trabajando en las pruebas y mantenimiento de 
un "Simulador de ATM".

**Valor total**: 15 puntos
- Fase 2 - Unit Testing (6 pts)
- Fase 3 - Refactoring (3 pts)
- Fase 4 - Features (6 pts)

**Fecha de Entrega**: 22 Abril 2023 11:59:59 pm


**Pautas Generales**:

- Lo primero que harán es un Fork al proyecto en GitHub para que tengan su propia versión para colaborar.

- Cada miembro del equipo debe aportar en cada fase; si un miembro se ve con muy pocos aportes en al repositorio, corre el riesgo
de perder puntos de forma individual.

- Pueden usar la estrategia de branching que más les convenga, incluyendo trabajando directo en el main/master branch 
(trunc-based strategy), mientras yo pueda ver que todos los integrantes colaboren.

- La idea es que vayan Fase por Fase. No trabajen en las demás fases si no han finalizado la anterior.

Ver los detalles de cada fase a continuación:


## Fase 1 - Experimentación (Entender el proceso y el código)

Normalmente, los casos de pruebas unitarias deberían ser escritos por el programador que escribió el código que éstos prueban.
Como ustedes no escribieron esta aplicación, se les complicará mucho el diseñar casos de pruebas de calidad, si antes que nada
no se familiarizan con el código y el funcionamiento.

Por lo tanto, el primer paso es que lean el código, y que experimenten, modificando el Program.cs para hacer diferentes
operaciones, ver cómo funcionan las transacciones, qué tipo de declinaciones, etc.

Antes de pasar a escribir el primer caso de prueba deben conocer a fondo cada entidad y miembro, como si lo hubiesen escrito
ustedes; si no logran entender, favor de consultarme, en lugar de empezar a trabajar ciegamente en los casos de pruebas.

Una vez conozcan en detalle cómo funciona todo y se sientan cómodos como usuario del API, procedan con la siguiente fase.


## Fase 2 - Unit Testing (Creación de Casos de Pruebas Unitarias)

Esta Fase consiste en crear casos de pruebas unitarias para los distintos comportamientos que se considere relevante probar.

Todos los primeros commit que realicen deben estar orientados a esto; simultáneamente todos los miembros del equipo
deben escribir casos de pruebas. Por lo tanto, cualquier cambio que hagan al Program.cs durate la Fase 1, traten de no 
incluirlo en commit al master.

**Deberán al menos tener 12 diferentes casos de pruebas entre todos**, pero lo importante es que cubran los escenarios importantes
de las siguientes entidades:
- ATM
- ATMSwitch
- Autorizador
- Tarjeta
- (Para el HSM no es requerido pero pueden hacerlo si quieren)

Favor de no perder el tiempo con casos de pruebas triviales (probando cosas a muy bajo nivel, o detalles de implementación,
o probando el "lenguaje de programación" o el framework, etc.), recuerden que lo importante es probar lógica de negocio.

Cuando entiendan que ya cubrieron suficiente del código y que están protegidos, pueden pasar a la próxima parte.

### Ejemplos Incluidos

Si se fijan en el proyecto ATMSimTests, ya hay varias cosas que les estoy proporcionando, para probar cada una de las entidades:

- **AtmTests**: casos de ejemplo que prueban desde el atm realizando la transacción y la validación de lo que el atm realizó al
recibir la respuesta. Al ejecutar este flujo también se está ejecutando el comportamiento de ATMSwitch, Autorizador y HSM. 
Para evaluar el resultado de lo que hizo el ATM se utiliza un Stub llamado FakeConsoleWriter

- **AtmSwitchTests**: casos que prueban el flujo desde el ATMSwitch hacia el HSM y el Autorizador. En este caso, el ATM se sustituye
por un Stub llamado FakeATM. Por lo tanto, puse un helper method llamado Encriptar que permite encriptar el pin a partir de
una llave en claro, como lo hace el ATM (ya que el ATMSwitch espera un criptograma de un PIN, y no el PIN en claro)

- **AuthorizerTests**: casos que prueban directamente el flujo del Autorizador (con el HSM). En este caso, no se prueba ni el ATM ni
el ATMSwitch.

- **FakeATM**: este stub simplemente se utiliza en los casos de pruebas ATMSwitchTests ya que el ATMSwitch requiere una instancia de IATM
sólo para obtener el nombre.

- **FakeConsoleWriter**: este stub es un Wrapper para la clase Console, para evitar que el ATM y el ATMSwitch utilicen el 
WriteLine/Write/ForegroundColor/BackgroundColor/ResetColor; este stub recibe las llamadas a esos métodos sin hacer nada más que almacenar el 
input recibido y permite luego consultar todo lo que escribieron como si lo estuviesen escribiendo en la consola. Con esto, se puede
evaluar el resultado de una transacción en un ATM, usando Asserts, lo cual sería complicado si se escribiese en la terminal.

- **FakeThreadSleeper**: este stub no hace nada. Sólo evita que el ATM esté llamando a Thread.Sleep() para evitar que los casos de
pruebas tarden más de la cuenta por eso.


## Fase 3 - Refactoring (Aplicar cambios al código sin cambiar el comportamiento)

Ya protegidos con los casos de pruebas, procederán a aplicar técnicas de refactoring, con la limitante de que cada técnica que apliquen debe 
estar identificada en el commit por el nombre que tiene en el catálogo de [Refactoring Guru](https://refactoring.guru/refactoring/techniques), o de lo contrario
no la tendré en cuenta.

**Se requiere al menos 10 refactorings entre todos**, aún mejor si realizan refactorings que mitiguen code smells reales que hayan encontrado, 
pero incluso si no encuentran muchos code smells, pueden inventarse razones para hacer refactorings, mientras apliquen una de las técnicas del 
catálogo y la identifiquen en el commit;

Pueden hacer múltiples refactorings por commit, pero deben poner cada uno identificado en el comentario del commit. Por ejemplo:

> Change Value to Reference  
Replace Magic Number with Symbolic Constant

Mejor aún sin 

Recuerden, en esta fase no deberían cambiar el comportamiento ni el significado del código, sólo aplicarle transformaciones que mantengan
la escencia del mismo.

Igual, traten de participar todos en esta fase;


## Fase 4 - Features (Implementar nuevas funcionalidades y mejoras)

Consiste en implementar nuevas funcionalidades y mejoras al programa, según los requerimientos proporcionados debajo, pero aún más importante: 
escribir nuevos casos de pruebas para probar los nuevos cambios implementados por ustedes.

**Deberán implementar al menos 3 de estos requerimientos**; una excepción es que si encuentran un error en el código y lo corrigen, y me
notifican, lo contaré como uno de los 3 requerimientos, pero igual tendrían que implementar 2 más, sin importar cuantos otros errores encuentren.

Algo importante es que deben identificar los commit con el número del ticket que estén trabajando. Si usan branches, también deberían identificarlos 
con el ticket (recomiendo usar branches para esto, pues facilita la experimentación). 

Nota: Esta fase es posiblemente la más complicada, así que por favor no proceder en esta parte hasta que se sientan totalmente cómodos con el código 
y hayan trabajado en las dos anteriores. A diferencia de las demás Fases, para esta no requeriré necesariamente la colaboración de todos los integrantes,
pero igual se recomienda.


### Requerimientos:

#### Easy:

- **RQ01-Montos Decimales**: Permitir retiros de hasta 2 dígitos decimales. Realizar los ajustes necesarios al código para que se acepten 
montos y balances en decimales, manteniendo hasta dos dígitos

- **RQ02-Relación de Tarjetas y Cuentas**: Actualmente, las tarjetas tienen el número de cuenta almacenado como un atributo. En el mundo real, 
es el autorizador que guarda la relación entre Tarjetas y Cuentas, por lo que se debe implementar un cambio para que el autorizador tenga una 
estructura de datos con la relación Tarjeta->Cuenta, y al recibir una transacción, consulte en esta estructura para determinar la cuenta.

#### Medium:

- **RQ03-Limite de Sobregiro Cuenta Corriente**: Actualmente, las cuentas corrientes se permiten sobregirar por cualquier cantidad. Se 
requiere implementar un parámetro que se configure en la creación de la cuenta, y que espcifique el límite de sobregiro, y si se excede este límite 
se declinaría con fondos insuficientes.

- **RQ04-Bloqueo de Tarjetas**: Implementar una estructura de "estado" de las tarjetas en el Autorizador, que permita bloquear la tarjeta 
y que las transacciones declinen si la tarjeta está bloqueada. Esta declinación deberá mostrar un error en la pantalla del ATM indicando esto.

- **RQ05-Límite de Retiro**: Implementar un límite para las transacciones de retiro, configurable por Autorizador. El límite será por transacción,
lo que quiere decir que no se requiere un "acumulador" o mantener algún conteo entre transacción y transacción. Cuando se exceda el límite de retiro, 
deberá mostrar una pantalla de error indicando ésto.

#### Hard:

- **RQ06-Dispensaciones parciales**: Implementar un contador del efectivo para el ATM; al crear el ATM se le especifica el balance del ATM
y se descuenta por cada retiro. Al realizar un retiro, si el balance del ATM no es suficiente para completar el monto del retiro, el ATM dispensará
"lo que le queda", y enviará el TransactionRequest sólo por el monto que puede dispensar. Al final de la transacción indicará al cliente que sólo pudo
dispensar X cantidad.

- **RQ07-Nueva Transacción: Depósitos**: Implementar una nueva transacción de Depósitos (con su propia combinación de teclas), la cual será lo 
contrario del retiro, permitiendo agregarle dinero a una cuenta al realizar uno. Al realizar el depósito el ATM debe mostrar "> Efectivo Depositado: XXXXX", 
por lo que se requiere un nuevo tipo de Comando de respuesta

- **RQ08-Nueva Transacción: Cambio de PIN**: Implementar un nuevo tipo de transacción de "cambio de PIN" (con su propia combinación de teclas) 
que requerirá el pin anterior y el pin nuevo. Si el pin anterior está incorrecto, se declina, indicando la razón en pantalla, y no se aplica el cambio 
de PIN. Si el pin anterior se ingresa correcto, deberá realizarse el "cambio de PIN", y debe indicar que fue satisfactorio en la pantalla. Las futuras 
transacciones con esa misma tarjeta sólo autorizarían con el nuevo PIN. Sólo se deben soportar pines de 4 dígitos numéricos

#### Challenge:

- **RQ09-Cargo de Retiro Internacional**: [*very hard*] Implementar un "cargo de retiro con tarjeta Internacional" que se configure por bin de la tarjeta 
(bin=primeros 6 dígitos de la tarjeta). El ATMswitch deberá mantener una estructura con los bines que aplican para este cargo, y el monto del cargo para 
cada uno. Al recibir un TransactionRequest del ATM para una tarjeta que aplica para este cargo, el ATMSwitch deberá enviarle un(os) Comando(s) 
(requiere la creación de un nuevo comando que permita responder sí o no) al ATM para mostrar "Para este retiro se le aplicará un cargo de RD$XXX", y 
cuando el usuario acepte, el ATM enviará la transacción nuevamente con el monto del cargo sumado al monto del retiro. Entonces el ATMSwitch procederá 
a autorizar con el Autorizador normalmente, pero si se autoriza el Retiro, cuando el Switch envíe los comandos, en el ComandoDispensarEfectivo deberá 
mostrarse el monto sin el cargo (porque se supone que el cargo se cobra al cliente, pero no se le dispensa en efectivo). Esto requiere muchas modificaciones 
y varios casos de pruebas nuevos, así que sólo intentar este si tienen tiempo de sobra


# Buena suerte!