# PROMPT 20 — PROGRESIÓN POR ROOMS, RAM INSUFICIENTE, BODEGA, INVENTARIO E INSTALACIÓN FÍSICA

## PROYECTO

AstroBit — Unity

---

# CONTEXTO

AstroBit es un juego educativo/exploratorio sobre el funcionamiento básico de un computador.

Actualmente existen varias zonas:

* Room CPU
* Room RAM
* Room de almacenamiento/disco duro
* Bodega/área disponible para ampliar el escenario

El Prompt 19 implementó correctamente el flujo:

```text
Almacenamiento
↓
Encontrar archivo
↓
Server / almacenamiento
↓
TV 32 Inch
↓
CPU
↓
Unidad de Control
↓
RAM1
↓
RAM2
↓
FinalActivity
```

El flujo fue probado completamente y funciona.

También se confirmó que:

* `ObjectiveSystem` funciona correctamente.
* `GameHUD` funciona correctamente.
* `PlayerInteraction` funciona correctamente.
* `IInteractable` funciona correctamente.
* `FinalActivity` funciona correctamente.
* `StorageMission` controla la progresión del archivo.
* Las 8 piezas CPU/RAM siguen funcionando.
* Las etiquetas permanecen después de completar.
* `[E]` desaparece cuando una interacción ya fue completada.

---

# NUEVO OBJETIVO DE DISEÑO

Ahora quiero que AstroBit tenga una **progresión general ordenada**.

El jugador no debería poder simplemente ir directamente al disco duro y comenzar la misión.

Quiero que el juego tenga una estructura narrativa/técnica:

```text
1. APRENDER CPU
       ↓
2. APRENDER RAM
       ↓
3. APRENDER ALMACENAMIENTO
       ↓
4. RECIBIR MISIÓN
       ↓
5. BUSCAR ARCHIVO
       ↓
6. INTENTAR CARGARLO
       ↓
7. RAM INSUFICIENTE
       ↓
8. IR A BODEGA
       ↓
9. RECOGER 2 MÓDULOS RAM
       ↓
10. INVENTARIO
       ↓
11. VOLVER A ROOM RAM
       ↓
12. INSTALAR RAM
       ↓
13. MEMORIA SUFICIENTE
       ↓
14. EJECUTAR PROGRAMA
       ↓
15. ACTIVIDAD FINAL
       ↓
16. RECORRIDO COMPLETADO
```

Esta será la nueva progresión principal.

---

# REGLA PRINCIPAL

## NO REHACER EL SISTEMA ACTUAL

El Prompt 19 ya funciona.

No reemplazarlo innecesariamente.

Extenderlo.

Reutilizar:

* `ObjectiveSystem`
* `GameHUD`
* `PlayerInteraction`
* `IInteractable`
* `StorageMission`
* `FinalActivity`
* `EducationalInteractable`
* `WorldLabel`
* sistemas actuales de interacción.

Crear nuevos scripts únicamente cuando sea realmente necesario.

---

# FASE 1 — PROGRESIÓN CPU → RAM → ALMACENAMIENTO

Actualmente las 8 piezas CPU/RAM llevan a `FinalActivity`.

Quiero cambiar el momento en el que se habilitan las siguientes zonas.

---

## ETAPA 1 — CPU

El jugador debe completar primero los componentes educativos de la CPU.

Los componentes existentes son:

1. ALU
2. Registros
3. Unidad de Control
4. Cache L1
5. Cache L2
6. Cache L3
7. RAM1
8. RAM2

IMPORTANTE:

Estos componentes ya funcionan.

No volver a introducir preguntas individuales.

El flujo continúa siendo:

```text
[E]
↓
Información
↓
"Entendido"
↓
Recompensa
↓
Completado
```

---

# ETAPA 2 — COMPLETAR CPU

Cuando se hayan completado los componentes correspondientes a CPU, el jugador debe recibir un nuevo objetivo.

Por ejemplo:

```text
OBJETIVO

Has conocido los principales componentes del procesador.

Ahora aprende cómo funciona la memoria RAM.
```

El texto exacto puede adaptarse al estilo actual.

---

# IMPORTANTE SOBRE LAS 8 PIEZAS

Actualmente existe una progresión de 8 componentes.

Antes de modificarla:

**inspecciona `ObjectiveSystem` y determina exactamente qué significa actualmente `CompletedSteps` y `TotalSteps`.**

No asumir que los 8 componentes son todos CPU.

El proyecto actualmente considera:

* ALU
* Registros
* Unidad de Control
* Cache L1
* Cache L2
* Cache L3
* RAM1
* RAM2

Por lo tanto, determina correctamente cómo separar conceptualmente:

### CPU

* ALU
* Registros
* Unidad de Control
* Cache L1
* Cache L2
* Cache L3

### RAM

* RAM1
* RAM2

No rompas la progresión actual.

---

# FASE 2 — ROOM RAM EDUCATIVA

Después de completar la fase CPU, el jugador debe poder entrar en la etapa de aprendizaje de RAM.

La room RAM debe conservar su función educativa.

El jugador debe poder examinar:

* RAM1
* RAM2
* y, si existen objetos apropiados, los slots o elementos relacionados.

No agregar preguntas.

El objetivo es:

```text
Explorar
↓
Examinar RAM
↓
Aprender
↓
Completar
```

---

# ETAPA DE COMPLETAR RAM

Cuando el jugador haya completado los componentes educativos de RAM:

Mostrar:

```text
OBJETIVO

Ya conoces la memoria RAM.

Ahora explora el sistema de almacenamiento.
```

Después se desbloquea la siguiente etapa.

---

# FASE 3 — ROOM DE ALMACENAMIENTO

La room de almacenamiento debe conservar la implementación del Prompt 19.

El jugador debe:

1. entrar;
2. recibir el objetivo;
3. buscar el archivo;
4. encontrarlo;
5. recuperarlo;
6. ir a la TV 32 Inch;
7. abrir el archivo.

NO eliminar la mecánica actual.

---

# FASE 4 — CAMBIO IMPORTANTE EN LA EJECUCIÓN DEL ARCHIVO

Actualmente, después de abrir el archivo desde la TV:

```text
TV
↓
CPU
↓
RAM
↓
FinalActivity
```

Ahora quiero introducir un problema:

> **La RAM disponible no es suficiente para cargar el programa.**

Por lo tanto:

Después de que el jugador llegue al punto de carga de RAM y trate de cargar el programa:

```text
RAM INSUFICIENTE
```

Debe bloquearse la ejecución.

---

# FASE 5 — DIAGNÓSTICO DE MEMORIA

Cuando el jugador intenta cargar el programa:

Mostrar feedback educativo.

Ejemplo:

```text
MEMORIA INSUFICIENTE

El programa necesita más memoria
de la disponible actualmente.

Busca módulos de RAM adicionales.
```

No tiene que ser exactamente este texto.

Debe ser claro y coherente.

---

# OPCIONAL — DATOS SIMPLIFICADOS

Si resulta sencillo integrarlo sin crear complejidad innecesaria, mostrar:

```text
Memoria disponible: 2 GB
Memoria requerida: 4 GB
```

o valores equivalentes.

Estos números son una **representación educativa**, no una simulación real del hardware.

No crear un sistema de gestión real de memoria.

---

# FASE 6 — NUEVO OBJETIVO: BODEGA

Después de detectar RAM insuficiente:

```text
OBJETIVO

La memoria disponible no es suficiente.

Busca módulos de RAM de repuesto en la bodega.
```

El jugador debe desplazarse físicamente hasta la bodega.

---

# BODEGA

La bodega debe contener **2 nuevos módulos de RAM**.

Estos módulos deben estar visualmente separados de los módulos que ya están instalados en la Room RAM.

Pueden estar:

* sobre una estantería;
* sobre una mesa;
* cerca de cajas;
* parcialmente desordenados.

No es necesario crear una bodega extremadamente compleja.

La intención es que se sienta como un lugar donde se guardan componentes de repuesto.

---

# FASE 7 — MÓDULOS RAM DE REPUESTO

Crear o reutilizar dos objetos que representen los módulos de RAM.

Por ejemplo:

```text
RAM_Replacement_01
RAM_Replacement_02
```

Los nombres reales deben adaptarse a la escena.

Cada uno debe poder interactuarse.

Antes de poder recogerlos:

```text
[E] Recoger RAM
```

---

# REGLA

Los módulos de repuesto **NO deben poder recogerse antes de que la misión de RAM insuficiente esté activa**.

Antes de activar la misión:

* no deben ofrecer la interacción;
* no deben interferir con la progresión.

Después de detectar RAM insuficiente:

* pueden recogerse.

---

# FASE 8 — INVENTARIO MINIMALISTA

Quiero introducir un inventario muy simple.

NO crear un sistema RPG.

NO crear:

* peso;
* slots complejos;
* estadísticas;
* crafting;
* equipamiento;
* objetos inútiles.

Solo un inventario contextual.

---

# DISEÑO DEL INVENTARIO

Debe verse limpio y pequeño.

Ejemplo conceptual:

```text
┌─────────────────────────┐
│       INVENTARIO        │
├─────────────────────────┤
│                         │
│       [ IMAGEN RAM ]    │
│                         │
│          RAM            │
│          ×2             │
│                         │
└─────────────────────────┘
```

El diseño visual debe adaptarse al estilo actual del HUD.

No utilizar una ventana enorme que cubra la pantalla.

---

# COMPORTAMIENTO

Al recoger la primera RAM:

```text
RAM ×1
```

Al recoger la segunda:

```text
RAM ×2
```

Después:

```text
OBJETIVO

Has encontrado los módulos de RAM.

Instálalos en la computadora.
```

---

# FASE 9 — INVENTARIO PERSISTENTE

El inventario debe conservarse si el jugador cambia de room.

Por ejemplo:

```text
Bodega
↓
RAM ×2
↓
volver a Room RAM
↓
RAM ×2
```

No perder los objetos al cambiar de zona.

No es necesario implementar un sistema de guardado en disco.

Basta con que el estado persista durante la sesión actual.

---

# FASE 10 — REGRESAR A ROOM RAM

El jugador debe volver físicamente a la Room RAM.

Debe existir un punto donde pueda instalar los módulos.

Idealmente cerca de los slots existentes.

Por ejemplo:

```text
RAM SLOT 1
┌──────────────┐
│              │
└──────────────┘

RAM SLOT 2
┌──────────────┐
│              │
└──────────────┘
```

---

# FASE 11 — INSTALACIÓN FÍSICA

Esta parte es MUY IMPORTANTE.

Quiero que los módulos de RAM aparezcan físicamente instalados en los slots.

Si los objetos de RAM ya existen en la escena pero están destinados a aparecer posteriormente:

pueden mantenerse:

```text
SetActive(false)
```

hasta que sean instalados.

---

# COMPORTAMIENTO

## Slot 1

Si el jugador tiene RAM en el inventario:

```text
[E] Instalar RAM
```

Al interactuar:

* consumir una RAM del inventario;
* activar/mostrar visualmente el módulo;
* colocarlo en el slot correcto;
* marcar Slot 1 como instalado.

Después:

```text
RAM instalada: 1/2
```

---

## Slot 2

Mismo comportamiento:

```text
[E] Instalar RAM
```

Después:

```text
RAM instalada: 2/2
```

---

# MUY IMPORTANTE

No crear una animación compleja de instalación si no es necesaria.

La prioridad es:

```text
Inventario
↓
[E] Instalar
↓
RAM aparece en slot
```

La sensación de que el jugador realmente colocó el componente es suficiente.

---

# FASE 12 — VALIDACIÓN DE LOS SLOTS

El sistema debe saber:

```text
RAM Slot 1 = Installed
RAM Slot 2 = Installed
```

Solo cuando:

```text
1/2
↓
2/2
```

se considera completada la ampliación de memoria.

---

# FASE 13 — MEMORIA SUFICIENTE

Cuando ambos módulos estén instalados:

Mostrar feedback:

```text
MEMORIA AMPLIADA

Los módulos de RAM fueron instalados correctamente.

El sistema dispone ahora de memoria suficiente.
```

El texto puede adaptarse.

Después:

```text
OBJETIVO

La memoria ya es suficiente.

Ejecuta el programa.
```

---

# FASE 14 — DESBLOQUEAR EJECUCIÓN

Ahora sí debe habilitarse la interacción que anteriormente estaba bloqueada.

Por ejemplo:

```text
RAM2
[E] Ejecutar programa
```

o el punto de ejecución que ya utiliza `StorageMission`.

NO crear un segundo sistema de ejecución.

Modificar la condición existente para que dependa de:

```text
RAM instalada 2/2
```

---

# REGLAS DE GATING

El jugador NO debe poder:

### Antes de encontrar archivo

```text
recoger RAM ❌
abrir TV ❌
```

### Antes de abrir archivo

```text
cargar RAM ❌
```

### Antes de detectar RAM insuficiente

```text
recoger módulos ❌
```

### Antes de tener RAM en inventario

```text
instalar RAM ❌
```

### Con solo una RAM

```text
ejecutar programa ❌
```

### Con ambas RAM instaladas

```text
ejecutar programa ✅
```

### Antes de ejecutar

```text
FinalActivity ❌
```

### Después de ejecutar

```text
FinalActivity ✅
```

---

# FASE 15 — ETIQUETAS

Mantener el comportamiento establecido anteriormente.

Antes de completar una interacción:

```text
RAM
[E] Interactuar
```

Después:

```text
RAM
```

La etiqueta permanece.

`[E]` desaparece.

Aplicar esto a:

* módulos de repuesto;
* slots;
* TV;
* puntos de misión.

---

# FASE 16 — RAM DE REPUESTO VISUALMENTE OCULTA

Si los módulos que deben aparecer en los slots ya existen en la escena:

mantenerlos desactivados/ocultos hasta la instalación.

Ejemplo conceptual:

```text
RAM instalada 1
SetActive(true)

RAM instalada 2
SetActive(true)
```

Pero NO asumir esta estructura.

Primero inspeccionar la escena.

Si es mejor crear instancias de un prefab existente, utilizar el sistema que sea más limpio.

---

# FASE 17 — DIAGNÓSTICO DEL SISTEMA

Quiero introducir una pequeña mejora educativa.

Cuando el programa no puede cargarse:

mostrar algo parecido a:

```text
DIAGNÓSTICO

CPU ................. OK
ALMACENAMIENTO ...... OK
RAM ................. INSUFICIENTE

Problema detectado:
memoria insuficiente.
```

Esto debe ser simple.

No crear un sistema de diagnóstico completo.

Puede ser un mensaje del HUD o panel existente.

---

# FASE 18 — ORDEN GENERAL DE LA CAMPAÑA

La progresión final debería ser:

```text
INICIO
  ↓
ROOM CPU
  ↓
Completar componentes CPU
  ↓
ROOM RAM
  ↓
Completar componentes RAM
  ↓
ROOM ALMACENAMIENTO
  ↓
Aprender almacenamiento
  ↓
Buscar archivo
  ↓
Encontrar archivo
  ↓
Recuperar archivo
  ↓
TV 32 Inch
  ↓
Abrir archivo
  ↓
CPU
  ↓
Procesar archivo
  ↓
RAM
  ↓
Intentar cargar
  ↓
RAM INSUFICIENTE
  ↓
BODEGA
  ↓
Recoger RAM ×2
  ↓
INVENTARIO
  ↓
ROOM RAM
  ↓
Instalar RAM ×2
  ↓
MEMORIA SUFICIENTE
  ↓
Ejecutar programa
  ↓
ACTIVIDAD FINAL
  ↓
RECORRIDO COMPLETADO
```

---

# FASE 19 — IMPORTANTE: NO BLOQUEAR LA EXPLORACIÓN FÍSICA INNECESARIAMENTE

Aunque la progresión sea ordenada, no quiero convertir el mapa en una serie de puertas cerradas.

Si actualmente las rooms están físicamente conectadas, el jugador puede desplazarse por ellas.

Lo que debe estar bloqueado es la **progresión de las interacciones/objetivos**, no necesariamente el movimiento.

Por ejemplo:

El jugador podría entrar antes a la bodega, pero:

```text
RAM de repuesto
[E] ❌
```

hasta que la misión correspondiente esté activa.

Esto mantiene la sensación de mundo abierto.

---

# FASE 20 — SISTEMA DE INVENTARIO

Antes de crear un sistema nuevo:

inspeccionar si ya existe algún sistema de inventario en el proyecto.

Si no existe:

crear el sistema mínimo necesario.

Debe poder representar al menos:

```text
RAM ×0
RAM ×1
RAM ×2
```

Funciones mínimas conceptuales:

```text
AddItem()
RemoveItem()
HasItem()
GetItemCount()
```

Los nombres reales deben adaptarse a la arquitectura.

No crear un sistema genérico enorme.

---

# FASE 21 — ESTADOS DE LA MISIÓN

Ampliar `StorageMission` si es la opción más limpia.

Estados conceptuales:

```text
LearningCpu
LearningRam
LearningStorage

SearchingFile
FileFound
FileRetrieved

ComputerOpened
CpuProcessing

RamInsufficient

SearchingReplacementRam
RamCollected

InstallingRam
RamExpanded

ProgramReady
ProgramExecuted

FinalActivity
Completed
```

NO es obligatorio utilizar exactamente estos estados.

Si el sistema actual puede manejarlo de manera más sencilla mediante flags/objetivos, hacerlo así.

La prioridad es evitar complejidad innecesaria.

---

# FASE 22 — OBJETIVOS DEL HUD

Los objetivos deben reflejar el progreso.

Ejemplos:

### CPU

```text
Conoce los componentes principales del procesador.
```

### RAM

```text
Aprende cómo funciona la memoria RAM.
```

### Almacenamiento

```text
Explora el sistema de almacenamiento.
```

### Archivo

```text
Busca el archivo que necesitamos abrir.
```

### TV

```text
Abre el archivo en la computadora.
```

### CPU

```text
El archivo llegó al procesador.
Inicia su procesamiento.
```

### RAM insuficiente

```text
No hay suficiente memoria.
Busca módulos de RAM de repuesto.
```

### Bodega

```text
Recoge los módulos de RAM necesarios.
```

### Instalación

```text
Instala los módulos de RAM en la computadora.
```

### Ejecución

```text
La memoria ya es suficiente.
Ejecuta el programa.
```

### Final

```text
Demuestra que comprendiste el recorrido del programa.
```

Adaptar los textos al estilo existente.

---

# FASE 23 — FINAL ACTIVITY

NO modificar las preguntas existentes en este prompt.

`FinalActivity` debe seguir siendo el sistema actual.

Solo cambiar:

**cuándo se dispara.**

Ahora debe dispararse después de:

```text
Archivo encontrado
↓
Archivo recuperado
↓
TV
↓
CPU
↓
RAM insuficiente
↓
Bodega
↓
RAM ×2
↓
Instalación ×2
↓
Programa ejecutado
```

Después:

```text
FinalActivity
```

---

# FASE 24 — INSPECCIÓN OBLIGATORIA

Antes de modificar:

inspeccionar:

* `StorageMission.cs`
* `ObjectiveSystem.cs`
* `GameHUD.cs`
* `FinalActivity.cs`
* `PlayerInteraction.cs`
* `IInteractable`
* `FileShelf.cs`
* `StorageServer.cs`
* `MissionStepPoint.cs`
* `WorldLabel.cs`
* escena actual
* Room CPU
* Room RAM
* Room almacenamiento
* bodega existente
* objetos RAM existentes
* slots existentes
* posibles prefabs de RAM.

No inventar nombres de GameObjects.

No asumir que la bodega está estructurada de determinada forma.

---

# FASE 25 — POSIBLE MEJORA DE LA BODEGA

Si la escena ya tiene elementos que pueden reutilizarse:

utilizarlos.

Por ejemplo:

* shelves;
* cajas;
* mesas;
* racks;
* componentes existentes.

No crear una nueva habitación completa si ya existe un espacio adecuado.

La bodega debe sentirse como un área de repuestos.

---

# FASE 26 — CALIDAD VISUAL DEL INVENTARIO

El inventario debe ser:

* pequeño;
* limpio;
* legible;
* coherente con el HUD actual;
* no invasivo.

Debe mostrar una representación visual de la RAM.

Si existe un modelo/imagen apropiado dentro del proyecto, reutilizarlo.

NO descargar assets externos.

NO crear un inventario tipo RPG.

---

# FASE 27 — CALIDAD DE LA INSTALACIÓN

Los módulos instalados deben:

* aparecer en la posición correcta;
* tener orientación correcta;
* no atravesar otros objetos;
* no quedar flotando;
* no interferir con el jugador;
* ser claramente visibles.

Antes de implementarlo, medir/inspeccionar los slots reales.

No asumir posiciones.

---

# FASE 28 — PERSISTENCIA DURANTE LA SESIÓN

El estado debe mantenerse si el jugador:

```text
Bodega
↓
Room RAM
↓
Room CPU
↓
Room RAM
```

No debe perder:

* RAM recogidas;
* RAM instaladas;
* archivo encontrado;
* progreso de la misión.

No es necesario implementar guardado permanente todavía.

---

# FASE 29 — REGRESIÓN OBLIGATORIA

Después de implementar todo:

volver a probar desde una sesión limpia:

### CPU

Completar todos los componentes.

### RAM

Completar RAM educativa.

### Almacenamiento

Encontrar archivo.

### TV

Abrir archivo.

### CPU

Procesar.

### RAM

Intentar cargar.

### Bodega

Recoger RAM ×2.

### Inventario

Confirmar RAM ×2.

### RAM

Instalar RAM 1.

### RAM

Instalar RAM 2.

### Ejecución

Ejecutar programa.

### FinalActivity

Responderla.

### Final

Confirmar:

```text
Recorrido completado.
```

---

# FASE 30 — PRUEBAS DE BLOQUEO

Probar explícitamente:

* intentar recoger RAM antes de tiempo;
* intentar instalar sin RAM;
* intentar instalar una segunda RAM sin tenerla;
* intentar ejecutar con 0/2 RAM;
* intentar ejecutar con 1/2 RAM;
* intentar abrir TV sin archivo;
* intentar procesar antes de abrir;
* intentar cargar RAM antes de procesar;
* intentar activar FinalActivity antes de ejecutar.

Todos deben fallar silenciosamente o mostrar feedback apropiado.

NO deben producir errores.

---

# FASE 31 — NO ROMPER CPU/RAM ACTUAL

Después de todo el cambio:

comprobar nuevamente los 8 componentes originales.

Debe seguir funcionando:

```text
ALU
Registros
Unidad de Control
Cache L1
Cache L2
Cache L3
RAM1
RAM2
```

No introducir nuevamente las actividades individuales.

---

# FASE 32 — NO MODIFICAR VISUALMENTE SIN NECESIDAD

NO modificar:

* cámara;
* movimiento;
* iluminación;
* materiales;
* modelos existentes;
* geometría;
* mapa;
* controles;
* Input System.

Solo realizar cambios visuales relacionados directamente con:

* inventario;
* módulos RAM;
* instalación;
* feedback de misión.

---

# FASE 33 — INFORME FINAL

Al terminar, entregar un informe detallado indicando:

## 1. Progresión

Cómo se implementó:

```text
CPU → RAM → almacenamiento → misión
```

## 2. RAM insuficiente

Cómo se detecta y qué condiciones utiliza.

## 3. Bodega

Qué objetos existentes se reutilizaron.

## 4. Inventario

Qué sistema se creó/reutilizó y cómo funciona.

## 5. Módulos RAM

Cómo se recogen.

## 6. Instalación

Cómo aparecen físicamente en los slots.

## 7. Gating

Qué condiciones bloquean/desbloquean cada etapa.

## 8. FinalActivity

Cómo se mantiene y cuándo se dispara.

## 9. Archivos modificados

Lista exacta.

## 10. Archivos nuevos

Lista exacta.

## 11. Pruebas

Resultado de las pruebas normales y de bloqueo.

## 12. Errores/warnings

Indicar cualquier error o warning nuevo.

---

# CRITERIO DE ÉXITO

El Prompt 20 está terminado correctamente cuando:

### PROGRESIÓN

* El jugador aprende primero CPU.
* Después aprende RAM.
* Después puede avanzar al almacenamiento.
* La misión del archivo se mantiene.

### ARCHIVO

* El archivo se encuentra.
* Se recupera.
* Se abre desde la TV.
* Llega a CPU.
* Se procesa.
* Llega a RAM.

### PROBLEMA

* RAM detecta que no hay suficiente memoria.
* La ejecución queda bloqueada.
* Se genera el objetivo de buscar RAM.

### BODEGA

* El jugador puede ir a la bodega.
* Puede recoger exactamente los módulos necesarios.
* Las RAM pasan al inventario.
* El inventario muestra correctamente la cantidad.

### INSTALACIÓN

* El jugador vuelve a RAM.
* Puede instalar RAM 1.
* Puede instalar RAM 2.
* Los módulos aparecen físicamente en los slots.
* El sistema reconoce 2/2.

### EJECUCIÓN

* Se desbloquea la ejecución.
* El programa puede ejecutarse.
* `FinalActivity` aparece únicamente después de completar el recorrido.

### FINAL

* La actividad final funciona como antes.
* Se puede completar.
* Aparece:

```text
Recorrido completado.
```

---

# REGLA FINAL

**No implementes antes de inspeccionar la arquitectura y la escena actuales.**

El objetivo de este prompt no es crear una simulación realista de hardware.

El objetivo es crear una experiencia educativa jugable donde el jugador:

> **aprende cómo funciona el computador → encuentra un problema → diagnostica falta de memoria → busca módulos de RAM → los recoge → los lleva consigo → los instala físicamente → amplía la memoria → vuelve a ejecutar el programa → completa la actividad final.**

Debe sentirse como una pequeña misión técnica dentro de AstroBit, no como una lista de tareas artificiales.

Si durante la implementación encuentras una solución técnica más limpia o una pequeña mejora de jugabilidad que encaje directamente con este flujo, puedes aplicarla, pero **no agregues sistemas grandes ni mecánicas fuera del alcance de este prompt**.
