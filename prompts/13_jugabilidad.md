# PROMPT 13 — SISTEMA DE JUGABILIDAD EDUCATIVA, PROGRESIÓN Y GUÍA DE ASTROBIT

Proyecto: AstroBit — Unity

---

# CONTEXTO ACTUAL

AstroBit ya tiene funcionando correctamente un sistema de interacción educativa.

Actualmente existen y funcionan:

- ALU
- Cache L1
- Cache L2
- Cache L3
- Registros
- Unidad de Control
- RAM1
- RAM2

Cada componente tiene:

- Collider
- EducationalInteractable
- etiqueta flotante
- interacción mediante `[E]`
- panel educativo
- actividad
- recompensa
- integración con ObjectiveSystem

El sistema actual funciona correctamente.

La cadena actual es:

```text
Jugador se acerca
        ↓
Etiqueta visible
        ↓
[E] Examinar
        ↓
Panel educativo
        ↓
Actividad
        ↓
Respuesta
        ↓
Recompensa
        ↓
Objetivo completado
Este sistema NO debe ser destruido.
La arquitectura actual de interacción, HUD, etiquetas, cámara, jugador y recompensas ya funciona y debe utilizarse como base.
________________________________________
NUEVO OBJETIVO
Ahora AstroBit debe evolucionar desde una colección de actividades independientes hacia una experiencia de juego educativa guiada.
Actualmente el jugador puede completar cada componente de manera separada, pero no existe una verdadera progresión educativa.
Quiero que el juego tenga:
INICIO
  ↓
GUÍA
  ↓
CPU
  ↓
ALU
  ↓
REGISTROS
  ↓
UNIDAD DE CONTROL
  ↓
CACHE
  ↓
RAM
  ↓
COMPRENSIÓN DEL FLUJO
La idea no es hacer un juego complicado.
La idea es crear una experiencia sencilla, clara y fácil de explicar durante una presentación.
El jugador debe sentir que está:
aprendiendo cómo funciona una computadora mientras explora físicamente sus componentes.
________________________________________
FILOSOFÍA DE JUGABILIDAD
NO quiero que AstroBit se convierta en un juego complejo.
La jugabilidad debe ser:
•	sencilla 
•	intuitiva 
•	educativa 
•	guiada 
•	fácil de explicar 
•	fácil de demostrar 
•	visualmente clara 
•	basada en exploración 
•	basada en pequeñas interacciones 
El jugador NO debería necesitar leer grandes cantidades de texto.
Debe poder entender:
Qué estoy viendo
↓
Qué hace
↓
Qué debo hacer
↓
Por qué lo estoy haciendo
________________________________________
CAMBIO IMPORTANTE: NO MÁS SUMAS Y RESTAS
Las actividades actuales utilizan operaciones como:
12 + 7
8 + 5
14 - 6
20 - 9
Esto sirvió como prueba técnica del sistema, pero YA NO quiero utilizar operaciones matemáticas como actividad principal.
Las actividades deben representar de manera sencilla lo que realmente hace cada componente.
Por ejemplo:
ALU
En lugar de:
12 + 7 = ?
hacer algo sencillo como:
La ALU recibe dos valores.

Selecciona qué operación debe realizar:

[ SUMA ]
[ COMPARACIÓN ]
[ RESTA ]
O una interacción equivalente.
La actividad debe enseñar:
La ALU realiza operaciones y cálculos sobre datos.
________________________________________
IMPORTANTE SOBRE LAS ACTIVIDADES
NO quiero simulaciones técnicamente complejas.
No necesitamos implementar una CPU real.
No necesitamos ejecutar instrucciones reales.
No necesitamos crear un emulador.
No necesitamos implementar buses reales.
NO necesitamos una arquitectura computacional funcional.
Necesitamos una:
REPRESENTACIÓN EDUCATIVA SIMPLE.
Cada actividad debe ser una pequeña interacción que permita explicar el concepto.
________________________________________
EJEMPLOS DE ACTIVIDADES
Estos son ejemplos conceptuales.
NO los implementes automáticamente sin antes revisar la arquitectura actual.
________________________________________
ALU
Concepto:
La ALU realiza operaciones sobre datos.
Actividad posible:
La CPU necesita calcular:

5 + 3

¿Qué componente realiza esta operación?

[ ALU ]
[ RAM ]
[ REGISTROS ]
Respuesta:
ALU
Otra posibilidad:
¿Qué operación debe realizar la ALU?

5 + 3

[ SUMAR ]
[ GUARDAR ]
[ CONTROLAR ]
La actividad debe ser sencilla.
________________________________________
REGISTROS
Concepto:
Los registros almacenan temporalmente datos que la CPU necesita rápidamente.
Actividad:
La CPU necesita guardar temporalmente el valor 8.

¿Dónde debe colocarlo?

[ REGISTRO ]
[ RAM ]
[ CACHE ]
Respuesta:
REGISTRO
Otra actividad posible:
Ordena:

CPU necesita un dato inmediatamente.

1. Buscar el dato
2. Guardarlo temporalmente
Pero mantenerlo sencillo.
________________________________________
UNIDAD DE CONTROL
Concepto:
La Unidad de Control coordina qué debe hacer la CPU.
Actividad:
La CPU debe ejecutar una instrucción.

¿Qué componente coordina el proceso?

[ ALU ]
[ UNIDAD DE CONTROL ]
[ RAM ]
Respuesta:
UNIDAD DE CONTROL
Otra posibilidad:
¿Qué debe hacer primero la CPU?

[ Coordinar la instrucción ]
[ Apagar el sistema ]
[ Guardar todo en el disco ]
________________________________________
CACHE L1
Concepto:
Es una memoria muy rápida que mantiene datos que la CPU utiliza frecuentemente.
Actividad:
La CPU necesita nuevamente un dato que acaba de utilizar.

¿Dónde conviene buscar primero?

[ CACHE L1 ]
[ RAM ]
[ DISCO ]
Respuesta:
CACHE L1
________________________________________
CACHE L2
Concepto:
Es una caché más grande pero generalmente algo menos rápida que L1.
Actividad:
El dato no está en L1.

¿Dónde debería buscarse después?

[ L2 ]
[ DISCO ]
[ MONITOR ]
Respuesta:
L2
________________________________________
CACHE L3
Concepto:
Es una caché de mayor capacidad que ayuda a mantener datos disponibles para los núcleos.
Actividad:
El dato no apareció en L1 ni L2.

¿Dónde se puede buscar antes de ir a RAM?

[ L3 ]
[ DISCO ]
[ TECLADO ]
Respuesta:
L3
________________________________________
RAM
Concepto:
La RAM mantiene temporalmente los datos y programas que están siendo utilizados.
Actividad:
Un programa está ejecutándose.

¿Dónde se mantienen temporalmente sus datos?

[ RAM ]
[ DISCO ]
[ MONITOR ]
Respuesta:
RAM
________________________________________
IMPORTANTE
Estas actividades son solamente ejemplos.
Antes de implementarlas:
1.	Inspecciona cómo funciona actualmente EducationalInteractable. 
2.	Inspecciona cómo GameHUD construye y muestra las actividades. 
3.	Determina si el sistema actual soporta únicamente preguntas numéricas. 
4.	Determina cuál sería la modificación mínima necesaria para permitir preguntas conceptuales. 
5.	Mantén compatibilidad con las actividades existentes. 
6.	No destruyas la actividad actual hasta tener la nueva funcionando. 
________________________________________
NUEVA ESTRUCTURA DE PROGRESIÓN
Actualmente los objetivos son independientes.
Quiero convertirlos en una secuencia educativa.
Propuesta:
INTRODUCCIÓN
     ↓
CONOCE LA CPU
     ↓
ALU
     ↓
REGISTROS
     ↓
UNIDAD DE CONTROL
     ↓
CACHE L1
     ↓
CACHE L2
     ↓
CACHE L3
     ↓
RAM
     ↓
COMPRENDER EL FLUJO
No asumir que esta secuencia es perfecta.
Analiza primero los componentes realmente existentes en la escena.
Si alguna relación pedagógica tiene más sentido, puedes proponer una variante.
Pero NO agregues componentes inexistentes.
________________________________________
OBJETIVOS GUIADOS
Quiero que el jugador siempre tenga claro:
¿Qué debo hacer ahora?
Por ejemplo:
OBJETIVO ACTUAL

Explora la CPU y encuentra la ALU.

[Objetivo]
Conoce la ALU.

[Guía]
Busca el componente marcado dentro de la CPU.
Después:
OBJETIVO COMPLETADO ✓

Has aprendido que la ALU realiza operaciones
sobre los datos.

NUEVO OBJETIVO

Busca los Registros.
Después:
NUEVO OBJETIVO

Aprende dónde la CPU almacena
temporalmente los datos que necesita.
________________________________________
SISTEMA DE GUÍA
El juego debe ayudar al jugador sin hacerlo todo automáticamente.
No quiero un marcador gigante que lleve al jugador de la mano.
Quiero una guía sencilla.
Por ejemplo:
OBJETIVO
Conoce la ALU

PISTA
Busca el componente con la etiqueta "ALU".
Cuando el jugador está en el cuarto correcto:
UBICACIÓN
CPU
Y puede aparecer:
Estás en la CPU.

Aquí se procesan las instrucciones
y se coordinan diferentes componentes.
________________________________________
INDICADOR DE UBICACIÓN
Quiero implementar un sistema de ubicación contextual.
Dependiendo del cuarto donde esté el jugador, debe aparecer un mensaje en la parte superior de la pantalla.
Ejemplo:
CPU
Cuando esté en la zona de CPU.
Si entra en RAM:
RAM
Si existe una zona específica para caché:
CACHE
Si entra en otra zona:
REGISTROS
etc.
________________________________________
IMPORTANTE SOBRE LA UBICACIÓN
NO crear un sistema complejo de navegación.
La solución puede utilizar:
•	colliders trigger 
•	zonas 
•	GameObjects existentes 
•	componentes sencillos 
•	un LocationZone o sistema equivalente 
La prioridad es que sea:
simple
estable
fácil de configurar
fácil de explicar
________________________________________
COMPORTAMIENTO DEL INDICADOR
Cuando el jugador entra:
CPU
debe aparecer arriba.
Cuando entra en RAM:
RAM
debe cambiar automáticamente.
Cuando sale de una zona:
debe actualizarse a la zona correspondiente.
Evitar que aparezcan múltiples ubicaciones simultáneamente.
Debe existir una única ubicación actual.
________________________________________
MENSAJE EDUCATIVO DE UBICACIÓN
Además del nombre:
CPU
puede existir opcionalmente un pequeño texto contextual:
Centro de procesamiento
Por ejemplo:
┌───────────────────────────┐
│           CPU             │
│   Centro de procesamiento │
└───────────────────────────┘
No hacerlo excesivamente grande.
Debe ser compatible visualmente con el HUD actual.
________________________________________
NO CREAR OTRO HUD
Ya existe:
GameHUD
Debe reutilizarse.
NO crear:
•	otro Canvas 
•	otro HUD 
•	otro sistema de interfaz 
•	otro sistema de interacción 
Si GameHUD necesita una pequeña ampliación para soportar ubicación y guía:
modificarlo de manera mínima y ordenada.
________________________________________
SISTEMA DE OBJETIVOS
Ya existe:
ObjectiveSystem
Debe reutilizarse.
No crear otro sistema de objetivos.
El nuevo flujo debe integrarse con:
ObjectiveSystem.CompleteObjective(...)
Los objetivos deben tener una secuencia.
Ejemplo:
Objetivo 1:
Encuentra la CPU.

↓

Objetivo 2:
Conoce la ALU.

↓

Objetivo 3:
Conoce los Registros.

↓

Objetivo 4:
Conoce la Unidad de Control.

↓

Objetivo 5:
Explora la Cache.

↓

Objetivo 6:
Comprende la RAM.

↓

Objetivo 7:
Comprende cómo trabajan juntas.
________________________________________
SISTEMA DE PROGRESIÓN
El jugador no debería poder completar libremente cualquier componente si el objetivo actual todavía no corresponde.
Sin embargo, NO quiero bloquear completamente la exploración.
Debe poder caminar libremente.
La diferencia debe ser:
Explorar libremente
        +
seguir una guía educativa
Si el jugador interactúa con un componente fuera del objetivo actual:
puede mostrarse algo como:
Este componente será estudiado más adelante.
O simplemente permitir la interacción si no rompe la progresión.
Analiza cuál opción es mejor para el sistema actual.
NO bloquear innecesariamente el mapa.
________________________________________
RECOMPENSAS
Las recompensas actuales funcionan.
No eliminarlas.
Pero ahora las recompensas pueden representar progreso educativo.
Por ejemplo:
✓ ALU COMPRENDIDA
✓ REGISTROS COMPRENDIDOS
✓ UNIDAD DE CONTROL COMPRENDIDA
✓ CACHE COMPRENDIDA
✓ RAM COMPRENDIDA
No es obligatorio cambiar todos los textos inmediatamente.
Primero garantizar que la progresión funcione.
________________________________________
ACTIVIDAD FINAL
Después de conocer los componentes principales:
ALU
REGISTROS
UNIDAD DE CONTROL
CACHE
RAM
quiero una actividad final muy sencilla que permita demostrar cómo se relacionan.
Por ejemplo:
Una instrucción necesita ejecutarse.

¿Qué componente ayuda a coordinarla?

[Unidad de Control]

¿Dónde puede encontrarse rápidamente un dato utilizado recientemente?

[Cache]

¿Dónde se mantienen temporalmente los datos del programa?

[RAM]

¿Dónde se realizan operaciones?

[ALU]
Puede ser una pequeña secuencia de preguntas.
NO convertirlo en un examen complicado.
El objetivo es que el jugador termine entendiendo:
INSTRUCCIÓN
    ↓
UNIDAD DE CONTROL
    ↓
REGISTROS / CACHE
    ↓
ALU
    ↓
RESULTADO
    ↓
RAM
La representación debe ser conceptualmente sencilla.
________________________________________
IMPORTANTE SOBRE EXACTITUD EDUCATIVA
No inventar comportamientos técnicamente incorrectos solamente para hacer el juego más sencillo.
Si una explicación necesita simplificarse:
hacerlo explícitamente como una representación educativa.
Mantener conceptos básicos correctos:
•	ALU realiza operaciones. 
•	Registros almacenan datos temporalmente dentro de la CPU. 
•	Unidad de Control coordina la ejecución de instrucciones. 
•	Caché proporciona almacenamiento rápido cercano al procesador. 
•	L1, L2 y L3 tienen diferentes niveles/características. 
•	RAM almacena temporalmente programas y datos en uso. 
No implementar detalles internos complejos que no sean necesarios.
________________________________________
DISEÑO DE LA EXPERIENCIA
Quiero que el flujo general se sienta aproximadamente así:
┌───────────────────────────────┐
│ OBJETIVO                      │
│ Conoce la ALU                 │
│                               │
│ PISTA                         │
│ Busca el componente marcado.  │
└───────────────────────────────┘

              ↓

Jugador explora

              ↓

┌───────────────────────────────┐
│ CPU                           │
│ Centro de procesamiento       │
└───────────────────────────────┘

              ↓

Encuentra ALU

              ↓

[E] Examinar

              ↓

┌───────────────────────────────┐
│ ALU                           │
│ Unidad Aritmético-Lógica      │
│                               │
│ La ALU realiza operaciones    │
│ sobre los datos.              │
│                               │
│ [Aprender]       [Cerrar]     │
└───────────────────────────────┘

              ↓

Actividad sencilla

              ↓

Respuesta correcta

              ↓

✓ ALU COMPRENDIDA

              ↓

NUEVO OBJETIVO

Conoce los Registros.
________________________________________
REUTILIZAR LO QUE YA FUNCIONA
NO modificar innecesariamente:
•	PlayerInteraction.cs 
•	EducationalInteractable.cs 
•	sistema actual de interacción 
•	interactionDistance 
•	proximityRadius 
•	ALU 
•	ALU_Label 
•	Cinemachine 
•	cámara 
•	jugador 
•	MovementInput 
•	CharacterSkinController 
•	Input System 
•	sistema de recompensas 
Si alguna modificación es estrictamente necesaria:
primero diagnosticar y justificarla.
________________________________________
MUY IMPORTANTE: NO ROMPER LA ALU
La ALU es actualmente la referencia funcional.
Debe permanecer funcionando:
Jugador
↓
ALU_Label
↓
[E]
↓
Panel
↓
Actividad
↓
Respuesta
↓
Recompensa
↓
Objetivo
Antes de realizar cambios importantes:
comprueba el estado actual.
Después de los cambios:
volver a probar la ALU.
________________________________________
PLAN DE IMPLEMENTACIÓN
NO implementar todo de golpe sin comprobar.
Trabajar por fases.
FASE 1 — AUDITORÍA
Inspeccionar:
GameHUD.cs
EducationalInteractable.cs
PlayerInteraction.cs
ObjectiveSystem
y todos los scripts relacionados con:
•	objetivos 
•	actividades 
•	recompensas 
•	HUD 
•	ubicación 
•	interacción 
Determinar qué puede reutilizarse.
NO modificar nada todavía.
________________________________________
FASE 2 — DISEÑO DE PROGRESIÓN
Antes de programar:
crear una propuesta concreta de:
Objetivo 1
Objetivo 2
Objetivo 3
...
basada únicamente en los componentes existentes.
Mostrar brevemente:
Objetivo
Componente
Concepto educativo
Actividad
Recompensa
Siguiente objetivo
No crear componentes nuevos del mapa.
________________________________________
FASE 3 — ACTIVIDADES CONCEPTUALES
Modificar el sistema actual de actividades solamente si es necesario para permitir preguntas conceptuales.
Preferir una solución que permita:
Pregunta
Respuesta correcta
Opciones
Feedback
Recompensa
sin romper las actividades existentes.
No crear un segundo sistema de actividades.
________________________________________
FASE 4 — PROGRESIÓN DE OBJETIVOS
Integrar la secuencia con:
ObjectiveSystem
Los objetivos deben avanzar después de completar correctamente la actividad correspondiente.
No avanzar solamente por abrir el panel.
Debe requerirse completar correctamente la actividad.
________________________________________
FASE 5 — SISTEMA DE UBICACIÓN
Implementar el indicador contextual:
CPU
RAM
CACHE
etc.
utilizando zonas sencillas.
Probar que cambia correctamente al entrar y salir.
________________________________________
FASE 6 — GUÍA
Agregar:
OBJETIVO ACTUAL
PISTA
al HUD existente.
Debe actualizarse automáticamente según el progreso.
________________________________________
FASE 7 — PRUEBA COMPLETA
Realizar una prueba desde el comienzo:
Inicio
↓
Primer objetivo
↓
Exploración
↓
CPU
↓
ALU
↓
Actividad
↓
Recompensa
↓
Siguiente objetivo
↓
Registros
↓
Unidad de Control
↓
Cache
↓
RAM
↓
Actividad final
________________________________________
PRUEBAS OBLIGATORIAS
PRUEBA 1 — ALU
Confirmar que sigue funcionando.
________________________________________
PRUEBA 2 — REGISTROS
Confirmar:
Label
[E]
Panel
Actividad conceptual
Respuesta
Recompensa
Objetivo siguiente
________________________________________
PRUEBA 3 — UNIDAD DE CONTROL
Mismo flujo.
________________________________________
PRUEBA 4 — CACHE
Comprobar al menos un nivel.
Verificar que la progresión no se rompe.
________________________________________
PRUEBA 5 — RAM
Comprobar interacción y actividad.
________________________________________
PRUEBA 6 — UBICACIÓN
Moverse entre:
CPU
RAM
otros cuartos existentes
y confirmar que el indicador cambia.
________________________________________
PRUEBA 7 — PROGRESIÓN
Completar los objetivos en orden.
Verificar que:
Objetivo actual
cambia correctamente.
________________________________________
PRUEBA 8 — EXPLORACIÓN
Moverse libremente sin completar nada.
Confirmar que el jugador puede explorar sin romper el sistema.
________________________________________
RESTRICCIONES ABSOLUTAS
NO crear:
•	nuevos componentes físicos inexistentes 
•	Storage 
•	Disco 
•	ROM 
•	Bus 
•	otros componentes que no estén en la escena 
NO inventar objetos del mapa.
NO rehacer:
•	cámara 
•	Cinemachine 
•	jugador 
•	sistema de interacción 
•	HUD completo 
•	sistema de recompensas 
NO crear sistemas paralelos.
NO duplicar:
•	GameHUD 
•	Canvas 
•	PlayerInteraction 
•	EducationalInteractable 
•	ObjectiveSystem 
NO hacer refactorización general.
________________________________________
PRIORIDAD
El orden de prioridad es:
1.	Mantener funcionando todo lo actual. 
2.	Crear progresión educativa. 
3.	Crear guía clara. 
4.	Cambiar actividades matemáticas por actividades conceptuales sencillas. 
5.	Implementar indicador de ubicación. 
6.	Crear actividad final que conecte los conceptos. 
7.	Mantener la jugabilidad simple. 
________________________________________
CRITERIO DE ÉXITO
Al terminar, un jugador nuevo debería poder entrar al juego y entender qué hacer sin que alguien tenga que explicárselo constantemente.
La experiencia debería sentirse así:
¿Dónde estoy?

        ↓

CPU

        ↓

¿Qué debo hacer?

        ↓

Conoce la ALU

        ↓

¿Dónde está?

        ↓

Explorar

        ↓

[E] Examinar

        ↓

¿Qué hace?

        ↓

Actividad sencilla

        ↓

Comprendido ✓

        ↓

¿Qué sigue?

        ↓

Registros
Y continuar progresivamente.
________________________________________
RESULTADO FINAL ESPERADO
AstroBit debe pasar de:
Objetivos independientes
        +
Actividades de suma/resta
        +
Interacciones aisladas
a:
        ASTROBIT
           │
           ▼
      EXPLORACIÓN
           │
           ▼
       UBICACIÓN
           │
           ▼
        OBJETIVO
           │
           ▼
         GUÍA
           │
           ▼
      COMPONENTE
           │
           ▼
      [E] EXAMINAR
           │
           ▼
    EXPLICACIÓN BREVE
           │
           ▼
    ACTIVIDAD CONCEPTUAL
           │
           ▼
        RESPUESTA
           │
           ▼
       RECOMPENSA
           │
           ▼
    SIGUIENTE OBJETIVO
           │
           ▼
       NUEVO COMPONENTE
El jugador debe terminar entendiendo de forma sencilla cómo se relacionan:
UNIDAD DE CONTROL
        ↓
REGISTROS / CACHE
        ↓
ALU
        ↓
RAM
sin necesidad de implementar una CPU real.
________________________________________
INFORME FINAL OBLIGATORIO
Al terminar informa:
1.	Qué arquitectura existente reutilizaste. 
2.	Qué scripts modificaste. 
3.	Qué componentes nuevos del sistema lógico agregaste. 
4.	Qué objetivos quedaron configurados. 
5.	Qué actividades conceptuales quedaron configuradas. 
6.	Qué respuestas correctas tiene cada actividad. 
7.	Cómo funciona la progresión. 
8.	Cómo funciona el indicador de ubicación. 
9.	Qué zonas de ubicación existen. 
10.	Cómo se actualiza el texto superior. 
11.	Cómo funciona la guía. 
12.	Qué ocurre si el jugador interactúa fuera del objetivo actual. 
13.	Qué ocurre al completar correctamente una actividad. 
14.	Qué ocurre al completar toda la progresión. 
15.	Si la ALU sigue funcionando. 
16.	Si Cache sigue funcionando. 
17.	Si Registros sigue funcionando. 
18.	Si Unidad de Control sigue funcionando. 
19.	Si RAM sigue funcionando. 
20.	Si existen errores o warnings nuevos. 
21.	Qué cambios se hicieron en Inspector. 
22.	Qué archivos fueron modificados. 
23.	Qué quedó pendiente. 
NO continuar con nuevas funcionalidades después de completar este prompt.
Primero dejar esta nueva base de jugabilidad completamente estable.
La prioridad es convertir AstroBit en un juego educativo sencillo, coherente y fácil de demostrar, manteniendo intacto todo lo que ya funciona.

