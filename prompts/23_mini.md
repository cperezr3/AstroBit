# PROMPT 23 — SISTEMA DE MISIONES, GUÍA VISUAL Y MINIMAPA

## PROYECTO

AstroBit — Unity

---

# CONTEXTO

El estado actual de AstroBit está funcionando correctamente.

El recorrido educativo y de almacenamiento ya fue implementado y probado:

CPU → RAM → Almacenamiento → búsqueda de archivo → Server → TV → CPU → RAM → instalación de RAM3/RAM4 → ejecución → FinalActivity.

También existen actualmente:

- ObjectiveSystem
- GameHUD
- StorageMission
- FinalActivity
- PlayerInteraction
- LocationZone / LocationZoneTrigger
- MissionStepPoint
- Inventory
- CollectibleRam
- InstallRamSlot
- WorldLabel

NO quiero modificar la lógica de estas mecánicas.

Este prompt tiene como objetivo agregar una nueva capa de **navegación y orientación para el jugador**.

---

# OBJETIVO GENERAL

Quiero que el jugador nunca se pierda dentro del mapa.

Actualmente el jugador tiene objetivos y pistas, pero quiero convertirlos en un sistema visual más parecido al de un videojuego.

La idea es:

```text
MISIÓN ACTUAL
      ↓
objetivo prioritario
      ↓
marcador amarillo en el mundo
      ↓
dirección / camino hacia el objetivo
      ↓
minimapa
El jugador debe saber inmediatamente:
"¿Qué tengo que hacer ahora y hacia dónde tengo que ir?"
________________________________________
PARTE 1 — MENÚ / PANEL DE MISIONES
Crear un pequeño panel de misiones en el HUD.
Debe ser discreto y limpio.
No quiero un menú gigante que tape la pantalla.
Puede ubicarse en una zona superior izquierda o lateral izquierda del HUD, siempre que no interfiera con:
•	movimiento; 
•	interacción; 
•	objetivo/pista existente; 
•	inventario; 
•	otros elementos actuales. 
________________________________________
DISEÑO
Visualmente quiero algo parecido a:
┌───────────────────────────────┐
│ MISIÓN                        │
│                               │
│ ● EXPLORAR LA CPU             │
│                               │
│   Conoce los componentes      │
│   principales del procesador. │
└───────────────────────────────┘
La misión prioritaria debe estar claramente resaltada.
Puede utilizar:
•	un punto amarillo; 
•	una pequeña flecha; 
•	brillo sutil; 
•	borde; 
•	icono. 
No exagerar.
________________________________________
PRIMERA MISIÓN
Al comenzar el juego, la misión prioritaria debe ser:
Explorar la CPU
Con una descripción breve como:
Conoce los principales componentes del procesador.
Debe ser la misión visualmente resaltada.
________________________________________
PROGRESIÓN DE MISIONES
No crear una lista completamente independiente de objetivos.
Utilizar la progresión que ya existe en ObjectiveSystem y StorageMission.
La interfaz debe reflejar el estado real del juego.
Conceptualmente:
MISIÓN PRINCIPAL

● Explorar la CPU
  0/6 componentes conocidos
Cuando termina CPU:
✓ Explorar la CPU

● Explorar la memoria RAM
  0/2 módulos conocidos
Después:
✓ Explorar la CPU
✓ Explorar la RAM

● Explorar el almacenamiento
Después:
✓ Explorar la CPU
✓ Explorar la RAM
✓ Explorar el almacenamiento

● Recuperar el archivo
Después:
✓ Explorar la CPU
✓ Explorar la RAM
✓ Explorar el almacenamiento

● Procesar el archivo
Y así sucesivamente según el flujo existente.
________________________________________
IMPORTANTE
No inventar una nueva progresión incompatible.
Antes de programar:
inspeccionar cómo ObjectiveSystem y StorageMission determinan actualmente:
•	CPU completada; 
•	RAM completada; 
•	almacenamiento desbloqueado; 
•	archivo encontrado; 
•	archivo recuperado; 
•	TV; 
•	CPU; 
•	RAM insuficiente; 
•	RAM instalada; 
•	ejecución. 
Utilizar esos estados existentes.
Si es necesario crear una pequeña capa visual:
MissionUI
puede hacerlo.
Pero el sistema de misiones visual NO debe convertirse en el sistema que controla la lógica real.
Debe ser una representación de los objetivos existentes.
________________________________________
PARTE 2 — MARCADOR AMARILLO DE OBJETIVO
Quiero un marcador visual en el mundo.
La idea es similar a los sistemas de navegación de videojuegos:
              ↓
           🟡
           ALU
o:
       → → → 🟡
El marcador debe mostrar:
"Aquí está tu siguiente objetivo."
________________________________________
CARACTERÍSTICAS
Debe:
•	tener color amarillo; 
•	ser claramente visible; 
•	ser amigable; 
•	no bloquear la vista; 
•	mantenerse asociado al objetivo; 
•	actualizarse cuando cambia la misión. 
Puede utilizar:
•	un punto; 
•	una flecha; 
•	un pequeño icono; 
•	un indicador flotante. 
Preferiblemente una combinación sencilla:
       ↓
      🟡
________________________________________
EJEMPLO
Si la misión actual es:
Explorar la CPU
y el siguiente componente que debe estudiar es ALU:
Jugador
   ↓
   ↓
   ↓
  🟡
  ALU
Cuando ALU se completa y el siguiente objetivo es Registros:
Jugador
   ↓
   ↓
  🟡
Registros
Debe actualizarse automáticamente.
________________________________________
OBJETIVOS DE NAVEGACIÓN
No utilizar únicamente posiciones hardcodeadas si el sistema existente ya tiene referencias a los GameObjects.
Preferir referencias directas a los objetos reales de la escena.
Por ejemplo:
ALU
Registros
Unidad de Control
Cache L1
Cache L2
Cache L3
RAM1
RAM2
y posteriormente:
Shelf
Server
TV
Computer_Ram_Slot_3
Computer_Ram_Slot_4
según corresponda.
________________________________________
PARTE 3 — CAMINO AMARILLO
Quiero además explorar la posibilidad de un pequeño camino visual amarillo que indique la dirección.
NO necesito todavía un sistema complejo de navegación.
No crear un NavMesh si no es necesario.
No crear un sistema de pathfinding complejo.
Primero implementar una versión sencilla.
Por ejemplo:
Jugador
  ↓
  •
  •
  •
  •
  🟡 Objetivo
Puede ser mediante:
•	pequeños puntos; 
•	flechas; 
•	segmentos; 
•	una línea visual; 
•	indicadores flotantes. 
El efecto debe ser:
"Sigue este camino."
________________________________________
IMPORTANTE SOBRE EL CAMINO
El camino debe ser visual y amigable, no una línea enorme que atraviese todo el mapa de manera fea.
No quiero que:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
cruce paredes o estructuras.
Si un sistema de camino real resulta demasiado complejo para la arquitectura actual, implementar primero solamente:
flechas / puntos direccionales
orientados hacia el siguiente objetivo.
La prioridad es que el jugador sepa hacia dónde ir.
________________________________________
PARTE 4 — MINIMAPA
Agregar un minimapa en la esquina superior derecha.
Debe ser pequeño y discreto.
Conceptualmente:
                         ┌───────────────┐
                         │       🟡      │
                         │               │
                         │   ▲           │
                         │  PLAYER       │
                         │               │
                         └───────────────┘
________________________________________
ESTILO
Quiero un minimapa inspirado conceptualmente en los minimapas de juegos como Genshin Impact:
•	circular o cuadrado con esquinas redondeadas; 
•	limpio; 
•	pequeño; 
•	semitransparente; 
•	fácil de interpretar; 
•	jugador representado mediante un punto o flecha; 
•	objetivo representado en amarillo. 
NO copiar elementos visuales protegidos.
Solo utilizarlo como referencia de UX.
________________________________________
JUGADOR EN EL MINIMAPA
El jugador debe aparecer como:
▲
o un pequeño indicador equivalente.
Preferiblemente una flecha que indique hacia dónde está mirando/moviéndose.
Debe actualizarse en tiempo real.
________________________________________
OBJETIVO EN EL MINIMAPA
El objetivo actual debe aparecer como:
🟡
o un marcador equivalente.
Por ejemplo:
┌───────────────────┐
│                   │
│        🟡         │
│                   │
│     ▲             │
│                   │
└───────────────────┘
Así el jugador puede saber:
Estoy aquí ▲
Tengo que ir allí 🟡
________________________________________
MINIMAPA Y ROOMS
El mapa actual tiene diferentes rooms.
Quiero que el minimapa represente la estructura espacial real del escenario de forma sencilla.
No hace falta mostrar todos los objetos individuales.
Prioridad:
CPU
RAM
Almacenamiento
y las conexiones/corredores principales.
Si existe una forma sencilla de utilizar una cámara secundaria ortográfica sobre el escenario:
puede evaluarse.
Pero NO sacrificar rendimiento ni introducir un sistema excesivamente complejo.
________________________________________
RESTRICCIÓN IMPORTANTE DEL MINIMAPA
NO quiero que el minimapa muestre:
•	etiquetas de todos los objetos; 
•	nombres de componentes; 
•	textos educativos; 
•	colliders; 
•	gizmos; 
•	información técnica innecesaria. 
Debe ser solamente navegación.
________________________________________
PARTE 5 — ACTUALIZACIÓN DINÁMICA
El sistema debe actualizar automáticamente el objetivo cuando el objetivo actual se completa.
Ejemplo:
ALU
↓
completada
↓
objetivo = Registros
Entonces:
MissionUI
actualiza:
● Explorar CPU
  Siguiente: Registros
El marcador amarillo cambia a Registros.
El minimapa cambia el marcador amarillo.
El indicador direccional cambia.
Todo debe representar el mismo objetivo.
________________________________________
ARQUITECTURA
Antes de programar, inspeccionar:
ObjectiveSystem.cs
GameHUD.cs
StorageMission.cs
MissionStepPoint.cs
LocationZone.cs
LocationZoneTrigger.cs
PlayerInteraction.cs
WorldLabel.cs
SampleScene.unity
Determinar cuál es actualmente la fuente real de verdad del objetivo.
Preferencia:
ObjectiveSystem
       ↓
MissionNavigation
       ↓
MissionUI
       ↓
WorldMarker
       ↓
Minimap
No crear varios sistemas que mantengan objetivos independientes.
Debe existir una sola fuente de verdad.
________________________________________
POSIBLE ESTRUCTURA
Si la arquitectura actual lo permite, puede crearse:
MissionUI.cs
MissionNavigation.cs
WorldObjectiveMarker.cs
MinimapController.cs
Pero no crear archivos innecesarios.
Si algún componente existente puede reutilizarse limpiamente, hacerlo.
________________________________________
NO MODIFICAR LA JUGABILIDAD
Este prompt NO debe modificar:
•	las 8 actividades CPU/RAM; 
•	preguntas; 
•	respuestas; 
•	FinalActivity; 
•	almacenamiento; 
•	búsqueda del archivo; 
•	TV; 
•	procesamiento; 
•	RAM insuficiente; 
•	bodega; 
•	inventario; 
•	RAM3; 
•	RAM4; 
•	instalación; 
•	ejecución. 
La única excepción es conectar visualmente sus estados actuales al sistema de navegación.
________________________________________
NO MODIFICAR LAS ETIQUETAS EXISTENTES
No hacer una limpieza global de WorldLabel.
No cambiar las etiquetas educativas actuales.
El sistema de navegación es independiente.
________________________________________
PRIMERA FASE DE IMPLEMENTACIÓN
Antes de hacer el camino amarillo y el minimapa completo, comprobar que el sistema puede representar correctamente:
Misión actual
↓
Objetivo actual
↓
posición del objetivo
Si esto funciona correctamente:
continuar con:
World Marker
↓
Minimapa
↓
Dirección
________________________________________
PRUEBAS OBLIGATORIAS
Realizar pruebas reales en Play Mode.
PRUEBA 1 — Inicio
Al comenzar:
MISIÓN

● Explorar la CPU
El objetivo visual debe apuntar hacia la CPU.
El minimapa debe mostrar:
Jugador ▲
Objetivo 🟡
________________________________________
PRUEBA 2 — ALU
Al entrar en el recorrido:
el marcador debe orientar al jugador hacia ALU.
Al completar ALU:
el objetivo debe cambiar al siguiente componente real.
________________________________________
PRUEBA 3 — Progresión CPU
Comprobar al menos:
ALU
↓
Registros
↓
Unidad de Control
↓
Cache L1
↓
Cache L2
↓
Cache L3
El marcador y minimapa deben actualizarse.
________________________________________
PRUEBA 4 — RAM
Después de CPU:
● Explorar la RAM
El marcador debe dirigir al jugador hacia RAM.
________________________________________
PRUEBA 5 — Almacenamiento
Después de completar CPU + RAM:
● Explorar el almacenamiento
El objetivo debe apuntar hacia la room de almacenamiento.
________________________________________
PRUEBA 6 — Flujo del archivo
Durante:
buscar archivo
↓
server
↓
TV
↓
CPU
↓
RAM
el objetivo debe cambiar según el estado real de la misión.
________________________________________
CRITERIO DE ÉXITO
El prompt está terminado cuando:
1.	Existe un panel de misión pequeño y claro. 
2.	La misión prioritaria está resaltada. 
3.	Al comenzar muestra "Explorar la CPU". 
4.	El objetivo visual amarillo indica dónde ir. 
5.	El objetivo se actualiza al completar cada etapa. 
6.	El jugador aparece claramente en el minimapa. 
7.	El objetivo aparece claramente en el minimapa. 
8.	El minimapa funciona en tiempo real. 
9.	El minimapa no muestra información innecesaria. 
10.	El sistema utiliza el estado real de ObjectiveSystem/StorageMission. 
11.	No existe una segunda lógica paralela de objetivos. 
12.	CPU → RAM → almacenamiento → archivo continúa funcionando exactamente igual. 
13.	FinalActivity continúa funcionando exactamente igual. 
14.	No se modifican las preguntas existentes. 
15.	No se modifican las interacciones existentes. 
16.	No aparecen errores ni warnings nuevos. 
________________________________________
REGLA FINAL
Este prompt agrega navegación y orientación, no nuevas mecánicas educativas.
NO implementar todavía:
•	actividades nuevas para ALU; 
•	operaciones matemáticas; 
•	transferencia entre Registros; 
•	simulación de buses; 
•	animaciones de datos; 
•	puzzles nuevos; 
•	sistema de combate; 
•	inventario avanzado; 
•	NPCs. 
Eso se dejará para una futura etapa.
La prioridad ahora es que AstroBit tenga una navegación clara, amigable y propia de un videojuego:
             MISIÓN
                ↓
        "¿Qué debo hacer?"
                ↓
          🟡 OBJETIVO
                ↓
        camino / dirección
                ↓
           MINIMAPA
                ↓
        "¿Dónde tengo que ir?"
Implementar únicamente esto y probarlo completamente en Play Mode antes de terminar.

