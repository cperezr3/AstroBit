# PROMPT 19 — FLUJO COMPLETO DEL ARCHIVO: ALMACENAMIENTO → CPU → RAM → ACTIVIDAD FINAL

## PROYECTO

AstroBit — Unity

---

# CONTEXTO

AstroBit es un juego educativo/exploratorio sobre el funcionamiento básico de un computador.

Actualmente existen varias rooms:

* Room CPU
* Room RAM
* Room de almacenamiento/disco duro

La implementación del Prompt 18 dejó funcionando correctamente:

### CPU/RAM

Los componentes:

* ALU
* Registros
* Unidad de Control
* Cache L1
* Cache L2
* Cache L3
* RAM1
* RAM2

se pueden examinar mediante:

`[E]`

El flujo actual es:

```text
[E]
↓
Información
↓
"Entendido"
↓
Recompensa / información aprendida
↓
Completado
```

Las etiquetas de los componentes permanecen visibles después de completar y `[E]` desaparece.

Las 8 piezas CPU/RAM continúan funcionando y llevan correctamente hacia `FinalActivity`.

---

# ROOM DE ALMACENAMIENTO

En la room de almacenamiento existen actualmente:

* 22 objetos `Shelf with Crates (N)`
* objetos visuales tipo `server`
* una `Tv 32 Inch` ubicada junto a los servidores

IMPORTANTE:

Los objetos llamados `server` **NO deben interpretarse como servidores reales dentro de la lógica educativa**.

Son únicamente elementos visuales utilizados para representar una zona de almacenamiento/disco duro.

La intención es:

> Simular visualmente un disco duro / sistema de almacenamiento utilizando los objetos disponibles en la escena.

No quiero crear una simulación técnicamente exacta de un disco duro físico.

Quiero una representación educativa y jugable.

---

# IMPLEMENTACIÓN ACTUAL DEL PROMPT 18

Actualmente existen:

* `StorageMission.cs`
* `FileShelf.cs`
* `StorageServer.cs`
* `WorldLabel.cs`

La misión actual funciona aproximadamente así:

```text
Entrar a Zone_Storage
↓
Buscar archivo
↓
Revisar Shelves
↓
Encontrar archivo
↓
Ir al Server
↓
Completar misión
```

Un Shelf específico tiene:

```text
containsFile = true
```

Actualmente es `Shelf with Crates (9)`.

Esto funciona correctamente.

---

# OBJETIVO DEL PROMPT 19

Ahora quiero transformar esa misión en una experiencia mucho más completa.

La idea es representar, de manera simplificada y jugable, el recorrido que realiza un archivo/programa cuando el usuario intenta abrirlo:

```text
ALMACENAMIENTO
      ↓
   ARCHIVO
      ↓
   SERVIDOR
      ↓
    TV / PC
      ↓
    CPU
      ↓
 PROCESAMIENTO
      ↓
     RAM
      ↓
PROGRAMA CARGADO
      ↓
ACTIVIDAD FINAL
```

No quiero una simulación técnica perfecta.

Quiero una **representación educativa simplificada**, comprensible y divertida.

---

# PARTE 1 — ELIMINAR ETIQUETAS DE LOS SHELVES

Actualmente los `Shelf with Crates` muestran etiquetas de almacenamiento.

Quiero eliminar **todas las etiquetas visibles de "Almacenamiento" asociadas a los Shelves**.

Esto incluye las etiquetas de:

* Shelf with Crates (1)
* Shelf with Crates (2)
* ...
* Shelf with Crates (22)

y cualquier etiqueta equivalente que identifique visualmente cada Shelf como "Almacenamiento".

## IMPORTANTE

Esto NO significa eliminar:

* los Shelves;
* sus colliders;
* `FileShelf`;
* su interacción;
* su lógica;
* el archivo;
* el sistema de objetivos.

Solo quiero eliminar la **etiqueta visual de almacenamiento** que aparece sobre los Shelves.

Los Shelves siguen siendo interactuables.

Por ejemplo:

```text
Shelf
[E] Buscar
```

puede seguir funcionando.

Simplemente no quiero que sobre cada Shelf aparezca una etiqueta tipo:

```text
ALMACENAMIENTO
```

---

# PARTE 2 — ETIQUETA DEL SERVER

Actualmente la etiqueta del `server` aparece demasiado baja, prácticamente dentro o en medio del objeto.

Quiero elevarla.

Debe quedar claramente por encima del objeto, de manera similar a una etiqueta flotante.

Conceptualmente:

```text
        SERVER
          ↑
      etiqueta
          |
       [OBJETO]
       [OBJETO]
       [OBJETO]
```

NO quiero:

```text
      [SERVER]
       [OBJETO]
```

donde el texto queda metido en la geometría.

## REGLA

Modificar únicamente la posición vertical de la etiqueta del Server.

No modificar:

* modelo;
* escala del Server;
* collider;
* posición del Server;
* lógica de interacción.

La etiqueta debe quedar claramente legible y separada visualmente del objeto.

---

# PARTE 3 — CONCEPTO DEL SERVER

Aunque el GameObject se llame `server`, para la experiencia educativa se debe considerar:

> **Representación visual del almacenamiento/disco duro.**

No implementar conceptos de servidores de red.

No hablar de:

* clientes;
* peticiones de red;
* servidores web;
* protocolos;
* red;
* conexiones cliente-servidor.

La narrativa debe ser sobre:

**almacenamiento → archivo → computadora → CPU → RAM.**

El objeto `server` es simplemente la representación visual que utilizamos para el almacenamiento.

---

# PARTE 4 — ENCONTRAR EL ARCHIVO

Mantener la mecánica actual del Prompt 18.

Al entrar a la room:

```text
OBJETIVO

Busca el archivo que necesitamos abrir.
```

El jugador explora los Shelves.

Al interactuar:

```text
[E] Buscar
```

Los Shelves incorrectos:

```text
No se encuentra el archivo aquí.
```

El Shelf correcto:

```text
¡Archivo encontrado!
```

La misión avanza.

---

# PARTE 5 — LLEVAR EL ARCHIVO AL STORAGE SERVER

Después de encontrar el archivo:

```text
OBJETIVO

Lleva el archivo al almacenamiento.
```

El jugador va hacia el objeto `server`.

La interacción debe ser algo como:

```text
[E] Recoger archivo
```

o:

```text
[E] Transferir archivo
```

Utiliza el texto que mejor encaje con la interacción actual.

Al interactuar correctamente:

```text
Archivo recuperado.
Ahora debemos abrirlo en la computadora.
```

El objetivo cambia.

---

# PARTE 6 — TV 32 INCH

Existe un objeto:

**Tv 32 Inch**

ubicado junto a los Servers.

Quiero utilizarlo como representación de la computadora/interfaz desde la cual el jugador intenta abrir el archivo.

No hace falta convertirlo literalmente en un monitor funcional.

Debe convertirse en un punto de interacción.

---

# INTERACCIÓN CON TV 32 INCH

Después de obtener el archivo:

La TV debe permitir:

```text
[E] Abrir archivo
```

o equivalente.

Antes de encontrar/recoger el archivo:

* no debe permitir iniciar este proceso.

Después de tener el archivo:

* debe permitir iniciar el proceso.

---

# PARTE 7 — INICIAR EL PROCESO DE APERTURA

Cuando el jugador interactúe con la TV:

debe comenzar una secuencia educativa que represente de forma simplificada lo que ocurre cuando una computadora abre un programa.

No quiero una cinemática compleja obligatoriamente.

Puede utilizarse:

* HUD;
* mensajes;
* objetivos;
* feedback;
* cambios de estado;
* desplazamiento del jugador entre rooms.

La prioridad es que el jugador **entienda que el archivo está viajando por diferentes partes del computador**.

---

# FLUJO EDUCATIVO

La secuencia deseada es aproximadamente:

```text
ARCHIVO
↓
ALMACENAMIENTO
↓
CPU
↓
PROCESAMIENTO
↓
RAM
↓
PROGRAMA CARGADO
↓
EJECUCIÓN
```

---

# PARTE 8 — ENVIAR ARCHIVO A LA CPU

Después de interactuar con la TV:

Mostrar algo como:

> "El archivo se está cargando en el procesador."

El objetivo:

```text
OBJETIVO

Ve a la CPU.
El procesador debe comenzar a trabajar con el archivo.
```

El jugador debe desplazarse físicamente hasta la Room CPU.

Esto es importante.

**No quiero que el proceso simplemente termine automáticamente.**

Quiero que el jugador tenga que desplazarse por el mapa y continuar la secuencia.

---

# PARTE 9 — ¿QUÉ COMPONENTE DE LA CPU?

Aquí quiero que inspecciones primero la estructura actual.

Los componentes existentes son:

* Unidad de Control
* Registros
* Cache L1
* Cache L2
* Cache L3
* ALU

No quiero inventar una representación técnicamente incorrecta solo porque necesitamos una interacción.

Determina cuál de estos componentes puede representar mejor el siguiente paso de forma educativa.

Mi propuesta inicial es:

```text
TV
↓
Unidad de Control
↓
Registros / Cache
↓
ALU
```

Pero **NO implementes esto ciegamente**.

Analiza la arquitectura educativa que ya tiene AstroBit y decide cuál sería la secuencia más clara.

Si es mejor representar el proceso mediante:

```text
Unidad de Control → Registros → ALU
```

hazlo.

Si es mejor utilizar:

```text
Unidad de Control → Cache → ALU
```

hazlo.

Si conviene simplificarlo a uno o dos componentes, también está bien.

---

# REQUISITO

Debe existir al menos una interacción significativa dentro de la Room CPU.

Por ejemplo:

```text
OBJETIVO

El archivo llegó a la CPU.
Inicia su procesamiento.
```

El jugador debe interactuar con el componente correspondiente.

Al hacerlo:

```text
Procesando archivo...
```

y después:

```text
El archivo ha sido procesado.
Ahora debe cargarse en la memoria RAM.
```

---

# PARTE 10 — NO HACER QUE EL JUGADOR HAGA 6 INTERACCIONES INNECESARIAS

No quiero convertir esto en:

```text
ALU
↓
Registros
↓
Cache L1
↓
Cache L2
↓
Cache L3
↓
Unidad de Control
```

solo porque los objetos existen.

Eso podría volver el recorrido tedioso.

El objetivo es que el jugador entienda el proceso, no hacerle recorrer todos los objetos.

Puedes utilizar **uno o dos componentes como máximo** para representar esta fase.

Si Claude considera que otro componente representa mejor el proceso, utilizarlo.

---

# PARTE 11 — PASAR A RAM

Una vez finalizado el procesamiento:

```text
OBJETIVO

El programa está listo para cargarse en memoria.
Ve a la RAM.
```

El jugador debe desplazarse físicamente a la Room RAM.

---

# PARTE 12 — CARGAR EL PROGRAMA EN RAM

En la Room RAM debe existir una interacción específica relacionada con el archivo.

Por ejemplo:

```text
[E] Cargar programa
```

Al interactuar:

```text
El programa está ahora cargado en memoria RAM.
```

La información educativa debe explicar brevemente el concepto.

Por ejemplo:

> "La RAM mantiene temporalmente los datos y programas que el sistema necesita utilizar mientras están en ejecución."

No utilizar necesariamente este texto exacto.

Mantener el estilo educativo que ya tiene AstroBit.

---

# PARTE 13 — EJECUTAR EL ARCHIVO

Una vez cargado en RAM:

```text
OBJETIVO

El programa está cargado.
Ejecuta el archivo.
```

El jugador interactúa con el punto correspondiente.

Puede ser:

* RAM1;
* RAM2;
* otro punto apropiado de la Room RAM.

Primero inspecciona la escena y decide cuál encaja mejor.

---

# PARTE 14 — ACTIVIDAD FINAL

Después de que el archivo haya recorrido correctamente el flujo:

```text
Almacenamiento
↓
TV
↓
CPU
↓
RAM
↓
Programa cargado
```

debe abrirse la **Actividad Final existente**.

IMPORTANTE:

No crear una nueva actividad final si `FinalActivity` ya existe y funciona.

Reutilizar el sistema actual.

La actividad final debe representar la conclusión del recorrido completo.

El jugador responderá la actividad final para demostrar que comprendió el proceso.

---

# FLUJO FINAL DESEADO

La experiencia completa debería sentirse aproximadamente así:

```text
┌─────────────────────┐
│ ROOM ALMACENAMIENTO │
└──────────┬──────────┘
           ↓
     Buscar archivo
           ↓
      Encontrarlo
           ↓
        Server
           ↓
     Recuperar archivo
           ↓
       TV 32 Inch
           ↓
      Abrir archivo
           ↓
┌─────────────────────┐
│      ROOM CPU       │
└──────────┬──────────┘
           ↓
    Procesar archivo
           ↓
┌─────────────────────┐
│      ROOM RAM       │
└──────────┬──────────┘
           ↓
     Cargar programa
           ↓
     Ejecutar archivo
           ↓
     ACTIVIDAD FINAL
           ↓
      Recorrido
      completado
```

---

# PARTE 15 — OBJETIVOS DEL HUD

Utilizar `ObjectiveSystem` existente.

No crear un sistema nuevo.

Los objetivos deben cambiar de forma natural.

Ejemplo:

### Objetivo 1

```text
Busca el archivo necesario en el almacenamiento.
```

### Objetivo 2

```text
Lleva el archivo al almacenamiento principal.
```

### Objetivo 3

```text
Utiliza la computadora para abrir el archivo.
```

### Objetivo 4

```text
El archivo llegó a la CPU.
Inicia su procesamiento.
```

### Objetivo 5

```text
El programa está listo.
Cárgalo en la RAM.
```

### Objetivo 6

```text
Ejecuta el programa.
```

### Objetivo 7

```text
Responde la actividad final.
```

### Final

```text
Recorrido completado.
```

Los textos son ejemplos.

Adáptalos al estilo actual del HUD.

---

# PARTE 16 — ESTADO DE LA MISIÓN

Debe existir una progresión controlada.

Ejemplo conceptual:

```text
StorageSearching
        ↓
FileFound
        ↓
FileRetrieved
        ↓
ComputerReady
        ↓
CpuProcessing
        ↓
RamLoading
        ↓
ProgramLoaded
        ↓
FinalActivity
        ↓
Completed
```

No es obligatorio utilizar exactamente estos nombres.

Reutiliza la arquitectura actual si existe una forma mejor.

---

# REGLAS DE PROGRESIÓN

El jugador NO debe poder:

* abrir el archivo antes de encontrarlo;
* iniciar el proceso desde la TV sin haber recuperado el archivo;
* procesarlo antes de activar el flujo;
* cargarlo en RAM antes de completar CPU;
* ejecutar el programa antes de cargarlo;
* abrir `FinalActivity` antes de completar el recorrido.

Cada paso debe depender del anterior.

---

# PARTE 17 — FEEDBACK

Cada transición importante debe tener feedback.

Ejemplos:

```text
Archivo encontrado.
```

```text
Archivo recuperado.
```

```text
Enviando archivo a la CPU...
```

```text
Procesamiento iniciado.
```

```text
Procesamiento completado.
```

```text
Cargando programa en RAM...
```

```text
Programa cargado.
```

```text
El programa está listo para ejecutarse.
```

No es necesario implementar animaciones complejas.

El feedback mediante HUD y paneles es suficiente si encaja mejor con el proyecto actual.

---

# PARTE 18 — MEJORAS DE JUGABILIDAD

Además del flujo obligatorio anterior, antes de implementar debes revisar si existe alguna mejora pequeña que pueda aumentar la sensación de juego.

Por ejemplo:

### A. Pistas progresivas

Si el jugador lleva demasiado tiempo buscando:

```text
Pista:
Revisa cuidadosamente los módulos de almacenamiento.
```

Después:

```text
Pista:
El archivo debería encontrarse en uno de los Shelves.
```

No hacer que el juego sea frustrante.

---

### B. Feedback diferente para Shelves

No todos los Shelves incorrectos tienen que mostrar exactamente el mismo mensaje.

Puede haber pequeños mensajes contextuales.

Por ejemplo:

```text
No se encuentra el archivo aquí.
```

```text
Solo hay datos antiguos en este módulo.
```

```text
Este módulo no contiene el archivo solicitado.
```

No exagerar.

---

### C. Sensación de transferencia

Cuando el archivo pasa de almacenamiento a CPU/RAM, se puede utilizar un mensaje temporal:

```text
TRANSFERENCIA

Almacenamiento → CPU
```

y posteriormente:

```text
CPU → RAM
```

Esto puede ayudar mucho a entender el concepto.

No es obligatorio crear una animación física.

---

### D. Estado visual

Si es fácil de implementar con los sistemas existentes:

* Shelf revisado → deja de mostrar `[E]`.
* Server utilizado → deja de mostrar `[E]`.
* TV utilizada → deja de mostrar `[E]`.
* Componente CPU utilizado → deja de mostrar `[E]`.
* RAM utilizada → deja de mostrar `[E]`.

Pero las etiquetas deben permanecer.

Mantener el mismo comportamiento ya establecido en CPU/RAM.

---

# PARTE 19 — INFORMACIÓN EDUCATIVA

El recorrido debe enseñar sin convertir cada paso en un examen.

La información debe aparecer como parte natural de la interacción.

Por ejemplo:

### Almacenamiento

El archivo se encuentra almacenado.

### CPU

La CPU interpreta/procesa las instrucciones necesarias.

### RAM

El programa necesita estar en memoria para poder ejecutarse.

### Actividad Final

El jugador demuestra que entendió el recorrido.

---

# IMPORTANTE SOBRE PRECISIÓN EDUCATIVA

No afirmar que el archivo físicamente "viaja" literalmente de esta forma dentro de una computadora real.

La secuencia debe presentarse como una:

> **representación simplificada del proceso de abrir y ejecutar un programa.**

La prioridad es que el jugador entienda:

* almacenamiento;
* procesamiento;
* memoria;
* ejecución.

No buscar una simulación arquitectónica perfecta.

---

# PARTE 20 — INSPECCIÓN OBLIGATORIA ANTES DE IMPLEMENTAR

Antes de modificar código:

Inspecciona:

* `StorageMission.cs`
* `FileShelf.cs`
* `StorageServer.cs`
* `WorldLabel.cs`
* `EducationalInteractable.cs`
* `ObjectiveSystem.cs`
* `GameHUD.cs`
* `PlayerInteraction.cs`
* `FinalActivity`
* `LocationZone`
* `Zone_Storage`
* objetos `server`
* `Tv 32 Inch`
* componentes CPU existentes
* componentes RAM existentes.

Determina qué puede reutilizarse.

NO crear sistemas paralelos si los actuales pueden manejar el flujo.

---

# PARTE 21 — NO ROMPER LO QUE YA FUNCIONA

El Prompt 18 fue probado en Play Mode y funciona correctamente.

Por lo tanto:

**NO rehacer la arquitectura actual innecesariamente.**

Mantener:

* `PlayerInteraction`
* `IInteractable`
* `ObjectiveSystem`
* `GameHUD`
* `FinalActivity`
* `WorldLabel`
* `FileShelf`
* `StorageServer`

si siguen siendo adecuados.

Extenderlos solo cuando sea necesario.

---

# PARTE 22 — RESTRICCIONES

NO modificar:

* movimiento;
* cámara;
* Input System;
* controles;
* geometría del mapa;
* modelos;
* materiales;
* iluminación;
* assets de terceros.

NO cambiar el diseño de la room CPU/RAM salvo para integrar la nueva secuencia.

NO eliminar las etiquetas de CPU/RAM.

NO eliminar las etiquetas de RAM.

NO eliminar las etiquetas de componentes CPU.

Solo eliminar las etiquetas de almacenamiento asociadas a los Shelves.

---

# PARTE 23 — NO SOBREIMPLEMENTAR

No convertir este prompt en un sistema gigantesco.

NO implementar:

* inventario completo;
* sistema de archivos real;
* filesystem virtual complejo;
* simulación de buses físicos;
* animaciones complejas;
* networking;
* servidores reales;
* sistemas de procesos reales;
* múltiples archivos;
* árboles de directorios.

El objetivo es:

**Una experiencia educativa jugable y clara.**

---

# PARTE 24 — PRUEBAS OBLIGATORIAS

Realizar Play Mode real.

## Almacenamiento

Verificar:

* las etiquetas de los Shelves desaparecieron;
* los Shelves siguen siendo interactuables;
* el archivo puede encontrarse;
* el Server funciona después de encontrarlo;
* el Server no funciona antes;
* la etiqueta del Server está por encima del objeto.

## TV

Verificar:

* no puede utilizarse antes de tener el archivo;
* puede utilizarse después;
* cambia el objetivo;
* inicia correctamente la fase CPU.

## CPU

Verificar:

* el objetivo llega correctamente;
* existe una interacción significativa;
* la interacción se completa;
* el objetivo cambia a RAM.

## RAM

Verificar:

* el programa puede cargarse;
* el estado cambia correctamente;
* la ejecución queda habilitada.

## Actividad Final

Verificar:

* no aparece antes de tiempo;
* aparece después de completar el recorrido;
* sigue funcionando exactamente como antes;
* sus preguntas y respuestas no fueron modificadas.

---

# PARTE 25 — REGRESIÓN

Después de implementar la nueva mecánica:

Volver a comprobar las 8 piezas CPU/RAM.

Confirmar:

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

siguen funcionando.

No debe romperse el flujo existente de aprendizaje.

---

# INFORME FINAL

Al terminar, entregar un informe con:

## 1. Cambios visuales

* etiquetas eliminadas de Shelves;
* nueva posición de etiqueta del Server.

## 2. Flujo implementado

Describir paso a paso:

```text
Shelf
→ archivo
→ Server
→ TV
→ CPU
→ RAM
→ Actividad Final
```

## 3. Componentes CPU utilizados

Explicar qué componente(s) se utilizaron para representar el procesamiento y por qué.

## 4. Sistemas reutilizados

Indicar qué sistemas existentes fueron reutilizados.

## 5. Archivos modificados

Lista exacta.

## 6. Archivos nuevos

Lista exacta.

## 7. Mejoras adicionales

Si se implementó alguna mejora de jugabilidad adicional, explicarla.

## 8. Pruebas

Indicar qué se probó.

## 9. Errores/warnings

Indicar cualquier error o warning nuevo.

---

# CRITERIO DE ÉXITO

El Prompt 19 está terminado cuando:

* Las etiquetas de almacenamiento de todos los Shelves desaparecieron.
* Los Shelves siguen funcionando.
* La etiqueta del Server está correctamente elevada.
* El archivo puede encontrarse.
* El archivo puede recuperarse.
* La TV 32 Inch puede iniciar el proceso.
* El jugador debe desplazarse a la CPU.
* Existe una interacción CPU significativa.
* El proceso continúa hacia RAM.
* El jugador debe desplazarse a la RAM.
* El programa puede cargarse.
* El programa puede ejecutarse.
* La Actividad Final se abre al final del recorrido.
* La Actividad Final sigue funcionando.
* El flujo es secuencial.
* Los objetivos se actualizan correctamente.
* Las etiquetas permanecen después de completar las interacciones.
* No se rompen CPU/RAM.
* No aparecen errores nuevos.
* No se crean sistemas paralelos innecesarios.

---

# REGLA FINAL

**Primero inspecciona la arquitectura actual y la escena.**

Después diseña la implementación mínima necesaria.

La idea central NO es simular un computador físicamente.

La idea es que el jugador experimente de forma interactiva y educativa:

> **"Encontré un archivo → lo llevé al almacenamiento → lo abrí desde la computadora → llegó a la CPU → fue procesado → se cargó en RAM → lo ejecuté → demostré que entendí el proceso."**

Si durante la inspección encuentras una forma más clara, educativa o divertida de representar este mismo flujo, puedes proponerla e implementarla **siempre que mantenga la intención principal del recorrido y no agregue complejidad innecesaria**.
