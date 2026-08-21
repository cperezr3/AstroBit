# PROMPT 18 — SIMPLIFICACIÓN DE CPU/RAM Y NUEVA JUGABILIDAD PARA LA ROOM DE DISCO DURO

## PROYECTO

AstroBit — Unity

---

# CONTEXTO GENERAL

AstroBit es un juego educativo/exploratorio sobre componentes de un computador.

Actualmente existe una room principal donde se encuentran los componentes relacionados con la CPU y RAM.

La interacción actual de los componentes funciona mediante:

`[E] Examinar`

Al examinar un componente aparece información educativa mediante paneles.

Anteriormente existía además una actividad/pregunta asociada a cada componente.

Después de probar personalmente el resultado del Prompt 17, se confirmó que:

* La interacción funciona.
* Los paneles funcionan.
* La información se muestra correctamente.
* Las actividades/preguntas funcionan técnicamente.
* La progresión funciona.
* No existen problemas importantes de funcionamiento en el flujo actual.

Sin embargo, se ha tomado una decisión de diseño:

**Las actividades/preguntas individuales de CPU y RAM ya no deben formar parte de la experiencia.**

La intención ahora es hacer que AstroBit se sienta más como un juego educativo de exploración e interacción y menos como una sucesión de preguntas obvias.

---

# OBJETIVOS DE ESTE PROMPT

Este prompt tiene DOS objetivos principales:

## OBJETIVO A — SIMPLIFICAR LA ROOM CPU/RAM

Eliminar de la experiencia las actividades/preguntas individuales de cada componente.

Mantener la información educativa de los componentes.

Mantener las etiquetas de los componentes.

Mantener la interacción `[E] Examinar` mientras el componente todavía no ha sido examinado/completado.

Una vez examinado/completado un componente:

* La etiqueta del componente debe permanecer visible.
* `[E] Examinar` debe desaparecer.
* El componente debe quedar marcado como completado.
* La información ya estudiada no debe volver a generar una actividad/pregunta.

---

# OBJETIVO B — AUMENTAR LA JUGABILIDAD EN LA ROOM DEL DISCO DURO

Existe otra room del mapa correspondiente al almacenamiento/disco duro.

En esta room existen, entre otros elementos:

* Servers
* Shelf with Crates (19)

Esta room actualmente necesita mayor jugabilidad e interacción.

Quiero implementar una experiencia jugable equivalente en calidad a la de la room CPU/RAM, pero con una mecánica diferente.

La idea propuesta es una misión de búsqueda:

> El jugador necesita encontrar un archivo/dato específico almacenado en alguno de los elementos del sistema de almacenamiento.

Por ejemplo:

1. El jugador entra a la room del disco duro.
2. Recibe información sobre qué debe buscar.
3. Debe inspeccionar diferentes `Shelf with Crates`.
4. No sabe inicialmente cuál contiene el archivo correcto.
5. Debe explorar/interactuar con los distintos elementos.
6. Encuentra el archivo correcto.
7. Después debe llevarlo al lugar correspondiente.
8. Interactúa con el servidor/equipo indicado.
9. Ejecuta o entrega el archivo.
10. El objetivo se completa.

**Esta es una propuesta inicial.**

Antes de implementar, debes inspeccionar la escena y determinar cómo adaptar esta idea a los objetos reales existentes.

---

# FASE 1 — AUDITORÍA OBLIGATORIA ANTES DE MODIFICAR

Antes de escribir o modificar código, inspecciona el estado real del proyecto.

NO asumas nombres de GameObjects, componentes, scripts o jerarquías.

Investiga:

### Sistema actual

* `PlayerInteraction`
* `IInteractable`
* `SimpleInteractable`
* `EducationalInteractable`
* `ObjectiveSystem`
* `GameHUD`
* scripts relacionados con etiquetas/proximidad
* cualquier sistema actualmente utilizado para objetivos
* cualquier sistema actualmente utilizado para mostrar `[E]`

### Room CPU/RAM

Identifica:

* ALU
* Registros
* Unidad de Control
* Cache L1
* Cache L2
* Cache L3
* RAM1
* RAM2
* etiquetas correspondientes
* objetos interactuables
* cómo se determina que un componente fue completado.

### Room de almacenamiento

Inspecciona específicamente:

* Servers
* todos los `Shelf with Crates (19)`
* jerarquía de los objetos
* colliders
* scripts existentes
* posibles objetos interactuables ya disponibles
* posiciones relativas
* elementos visuales que puedan utilizarse para la nueva mecánica.

IMPORTANTE:

**No modificar assets ni scripts de terceros sin necesidad.**

Priorizar los scripts propios de:

`Assets/Scripts/`

o la estructura equivalente existente en el proyecto.

---

# FASE 2 — ELIMINAR LAS ACTIVIDADES INDIVIDUALES

## CPU/RAM

Las preguntas/actividades individuales de:

* ALU
* Registros
* Unidad de Control
* Cache L1
* Cache L2
* Cache L3
* RAM1
* RAM2

deben dejar de aparecer.

Ya no debe existir el flujo:

```text
[E]
↓
Información
↓
Pregunta
↓
Respuesta
↓
Completado
```

Debe convertirse en:

```text
[E]
↓
Información
↓
Componente estudiado/completado
```

La experiencia debe ser más directa.

---

# IMPORTANTE — NO ELIMINAR LA INFORMACIÓN

Eliminar la actividad NO significa eliminar los paneles informativos.

Debe mantenerse la información educativa.

El jugador seguirá pudiendo examinar cada componente y aprender sobre él.

La diferencia es que después de la información:

**NO debe aparecer una pregunta.**

---

# PANEL 1 Y PANEL 3

Los paneles informativos existentes deben conservarse según su función actual.

No crear una actividad sustituta.

No introducir preguntas nuevas.

No reemplazar la información educativa por texto arbitrario.

Si actualmente el flujo utiliza Panel 1 y Panel 3 para mostrar información complementaria, mantener la información útil y coherente.

El objetivo es que la experiencia educativa sobreviva aunque desaparezca el cuestionario.

---

# ETIQUETAS DE LOS COMPONENTES

ESTO ES MUY IMPORTANTE.

Actualmente cada componente tiene una etiqueta que permite identificarlo.

Esa etiqueta **NO debe desaparecer después de completar/examinar el componente**.

El comportamiento deseado es:

## Antes de examinar

```text
ALU
[E] Examinar
```

## Después de examinar

```text
ALU
```

Es decir:

* La etiqueta `ALU` permanece.
* La etiqueta `REGISTROS` permanece.
* La etiqueta `UNIDAD DE CONTROL` permanece.
* La etiqueta `CACHE L1` permanece.
* La etiqueta `CACHE L2` permanece.
* La etiqueta `CACHE L3` permanece.
* La etiqueta `RAM` permanece.
* etc.

Pero:

`[E] Examinar`

debe desaparecer una vez que ese componente ya fue completado.

---

# REGLA DE ESTADO

El sistema debe distinguir claramente entre:

### Componente no examinado

```text
[Etiqueta]
[E] Examinar
```

### Componente ya examinado

```text
[Etiqueta]
```

La etiqueta debe seguir siendo visible.

No debe volver a aparecer `[E] Examinar` después de completar el componente.

No eliminar el objeto.

No ocultar la etiqueta.

No desactivar innecesariamente el GameObject completo.

La intención es conservar la identidad visual del componente en el escenario.

---

# OBJETIVO DE LA ROOM CPU/RAM

La room CPU/RAM debe quedar como una zona de:

**Exploración + descubrimiento + aprendizaje.**

El jugador recorre la room y examina:

* ALU
* Registros
* Unidad de Control
* Cache L1
* Cache L2
* Cache L3
* RAM1
* RAM2

Cada componente aporta información.

No se debe obligar al jugador a responder una pregunta obvia del tipo:

> "¿Qué componente estás viendo?"

porque el propio escenario ya indica claramente qué componente está examinando.

---

# FASE 3 — NUEVA JUGABILIDAD PARA LA ROOM DE DISCO DURO

Ahora se debe ampliar la jugabilidad del área de almacenamiento.

La room contiene:

* Servers
* Shelf with Crates (19)

No quiero simplemente copiar las preguntas de CPU/RAM.

Quiero una mecánica de exploración.

---

# MECÁNICA PROPUESTA — BÚSQUEDA DE ARCHIVO

Utiliza esta idea como base:

## MISIÓN

El jugador recibe un objetivo relacionado con un archivo almacenado.

Ejemplo conceptual:

> "Necesitamos recuperar el archivo de configuración perdido. Debe encontrarse en uno de los módulos de almacenamiento."

El jugador deberá buscarlo.

---

# EXPLORACIÓN

Los diferentes `Shelf with Crates (19)` pueden funcionar como puntos de búsqueda.

Por ejemplo:

```text
Shelf 01 → no contiene el archivo
Shelf 02 → no contiene el archivo
Shelf 03 → no contiene el archivo
...
Shelf X → archivo encontrado
```

No es obligatorio utilizar exactamente esta estructura.

Primero inspecciona la escena y determina cómo utilizar los objetos existentes de forma natural.

---

# INTERACCIÓN CON LOS SHELF WITH CRATES

Cada Shelf con Crates puede convertirse en un punto de inspección.

Al acercarse:

```text
[E] Buscar
```

o un texto equivalente que sea coherente con el sistema actual.

Al interactuar:

### Caso incorrecto

Mostrar una pequeña respuesta contextual, por ejemplo:

> "No hay nada aquí."

o

> "Este módulo no contiene el archivo solicitado."

Después:

* el Shelf queda marcado como revisado;
* `[E]` desaparece;
* el jugador continúa buscando.

### Caso correcto

Mostrar algo como:

> "¡Archivo encontrado!"

El objetivo cambia.

Por ejemplo:

```text
Archivo encontrado.
Llévalo al servidor para ejecutarlo.
```

---

# IMPORTANTE — EVITAR ALEATORIEDAD INNECESARIA

No hace falta crear una simulación compleja de almacenamiento real.

La mecánica debe sentirse jugable, pero ser estable y controlable para la demo.

Puede existir un Shelf específico que contenga el archivo.

La ubicación debe quedar claramente determinada por el sistema.

Si es posible, hacer que el sistema permita configurar fácilmente cuál Shelf contiene el objetivo desde Inspector o mediante una referencia.

---

# SEGUNDA FASE DE LA MISIÓN

Una vez encontrado el archivo:

```text
OBJETIVO:
Archivo encontrado.
Llévalo al servidor.
```

Ahora debe existir interacción con un `Server`.

El jugador se dirige al servidor correspondiente.

Al acercarse:

```text
[E] Ejecutar archivo
```

Al interactuar:

* se muestra feedback;
* el archivo se considera ejecutado/procesado;
* el objetivo se completa.

---

# RESULTADO FINAL

Después de ejecutar el archivo:

Mostrar un resultado contextual, por ejemplo:

> "Archivo ejecutado correctamente."

o equivalente coherente con la narrativa educativa.

El objetivo de la room queda completado.

---

# FASE 4 — EDUCACIÓN SIN PREGUNTAS OBVIAS

La room del disco duro también debe enseñar.

Sin embargo, NO quiero reemplazar las preguntas de CPU/RAM por preguntas igualmente obvias.

La información debe aparecer integrada en la interacción.

Ejemplo conceptual:

### Al inspeccionar un Shelf

> "Los datos almacenados en un dispositivo de almacenamiento se organizan para poder ser localizados y recuperados cuando el sistema los necesita."

### Al interactuar con un Server

> "Los servidores pueden proporcionar servicios y almacenar o procesar información para otros equipos."

No es necesario utilizar exactamente estos textos.

**Primero analiza el propósito educativo de AstroBit y adapta la información a la mecánica.**

---

# FASE 5 — HUD Y OBJETIVOS

La nueva room debe utilizar el sistema de objetivos existente.

NO crear un segundo sistema paralelo de objetivos si `ObjectiveSystem` ya puede manejarlo.

Debe integrarse con:

* `ObjectiveSystem`
* `GameHUD`
* `PlayerInteraction`
* sistema actual de interacción
* sistema actual de etiquetas.

El HUD debe mostrar claramente el objetivo actual.

Ejemplo:

```text
OBJETIVO

Busca el archivo perdido en los módulos de almacenamiento.
```

Después:

```text
OBJETIVO

Lleva el archivo al servidor.
```

Después:

```text
OBJETIVO

Ejecuta el archivo.
```

Finalmente:

```text
Recorrido completado.
```

---

# PROGRESIÓN

La progresión debe ser secuencial.

Ejemplo:

```text
1. Buscar archivo
        ↓
2. Inspeccionar Shelf
        ↓
3. Encontrar archivo
        ↓
4. Llevar archivo al Server
        ↓
5. Ejecutar archivo
        ↓
6. Completar room
```

El jugador no debería poder ejecutar el archivo antes de haberlo encontrado.

---

# AUMENTAR JUGABILIDAD GENERAL

Quiero que este prompt no solamente agregue una interacción aislada.

La intención es que AstroBit empiece a sentirse como una experiencia jugable.

Por eso, durante la implementación, busca oportunidades **dentro de los objetos y sistemas que ya existen** para aumentar la interacción.

Pero respeta una regla:

> **No añadir mecánicas complejas innecesarias solamente por añadirlas.**

Priorizar:

* exploración;
* objetivos;
* interacción;
* feedback;
* progresión;
* descubrimiento;
* pequeñas recompensas visuales;
* sensación de avance.

---

# NO CREAR UNA COPIA DE LA ROOM CPU/RAM

La room de disco duro debe compartir la misma infraestructura técnica, pero no necesariamente la misma mecánica.

CPU/RAM:

```text
Explorar
→ Examinar componente
→ Leer información
→ Completar componente
```

Disco duro:

```text
Explorar
→ Buscar
→ Encontrar
→ Transportar/entregar
→ Ejecutar
→ Completar misión
```

Esto hace que ambas rooms se sientan diferentes.

---

# ETIQUETAS EN LA ROOM DE DISCO DURO

Aplicar el mismo principio de interacción utilizado en CPU/RAM.

Antes de interactuar:

```text
[Nombre del objeto]
[E] Interactuar
```

Después de completar:

```text
[Nombre del objeto]
```

La etiqueta debe permanecer.

La acción `[E]` debe desaparecer únicamente cuando la interacción correspondiente ya fue completada.

No ocultar permanentemente el objeto ni su etiqueta.

---

# POSIBLE ESTRUCTURA DE OBJETIVOS

Utilizar una estructura similar a:

```text
Objetivo 1:
Busca el archivo perdido.

Objetivo 2:
Encuentra el archivo en uno de los módulos de almacenamiento.

Objetivo 3:
Lleva el archivo al servidor.

Objetivo 4:
Ejecuta el archivo.

Objetivo 5:
Recorrido completado.
```

Sin embargo, simplifica los objetivos si el sistema actual funciona mejor con menos estados.

---

# REUTILIZACIÓN DE SISTEMAS EXISTENTES

Antes de crear nuevas clases:

Investiga si los sistemas existentes pueden reutilizarse.

Especialmente:

* `IInteractable`
* `PlayerInteraction`
* `EducationalInteractable`
* `SimpleInteractable`
* `ObjectiveSystem`
* `GameHUD`

Si `EducationalInteractable` puede adaptarse limpiamente para la nueva mecánica, reutilizarlo.

Si no es apropiado, crear un componente pequeño y específico.

No duplicar sistemas que ya existen.

---

# RESTRICCIONES IMPORTANTES

NO modificar:

* cámara;
* movimiento del jugador;
* controles;
* Input System;
* mapa existente;
* geometría de la escena;
* assets de terceros;
* materiales;
* iluminación;
* modelos;
* decoración;
* room CPU/RAM más allá de la eliminación de las actividades y el comportamiento de etiquetas;
* funcionamiento de los sistemas que ya están funcionando correctamente.

NO romper:

* `[E]`;
* `PlayerInteraction`;
* `ObjectiveSystem`;
* `GameHUD`;
* etiquetas;
* progresión;
* interacción existente.

---

# ADVERTENCIA SOBRE EL PANEL 2

Como las actividades individuales ya no se utilizarán:

El flujo de actividad/pregunta puede dejar de invocarse para CPU/RAM.

Pero:

**NO borrar código del Panel 2 innecesariamente si ese sistema puede ser reutilizado por otra parte del juego.**

Primero determina dónde se utiliza.

Si `FinalActivity` u otro sistema todavía depende de él, no eliminarlo globalmente.

La prioridad es eliminar la aparición de las actividades individuales, no destruir infraestructura que pueda seguir siendo necesaria.

---

# VERIFICACIÓN CPU/RAM

Probar manualmente:

### ALU

```text
[E] Examinar
→ información
→ completar
→ etiqueta permanece
→ [E] desaparece
```

### Registros

Mismo comportamiento.

### Unidad de Control

Mismo comportamiento.

### Cache L1

Mismo comportamiento.

### Cache L2

Mismo comportamiento.

### Cache L3

Mismo comportamiento.

### RAM1

Mismo comportamiento.

### RAM2

Mismo comportamiento.

Confirmar que:

* nunca aparece la pregunta;
* nunca aparece una opción de respuesta;
* no se dispara la actividad;
* la etiqueta permanece;
* `[E]` desaparece después de completar;
* el componente queda registrado como completado.

---

# VERIFICACIÓN DISCO DURO

Probar:

1. Entrar en la room.
2. Ver el objetivo inicial.
3. Explorar los Shelf with Crates.
4. Interactuar con varios.
5. Confirmar feedback.
6. Encontrar el Shelf correcto.
7. Confirmar que el objetivo cambia.
8. Ir al Server.
9. Confirmar que la interacción correcta está disponible.
10. Ejecutar/entregar el archivo.
11. Confirmar finalización.
12. Confirmar que las etiquetas permanecen.
13. Confirmar que las interacciones completadas pierden `[E]`.
14. Confirmar que no aparecen errores.

---

# CASOS DE ERROR

Probar también:

* intentar ejecutar antes de encontrar el archivo;
* revisar un Shelf incorrecto;
* volver a interactuar con un Shelf ya completado;
* intentar completar un objetivo fuera de orden.

El sistema debe responder de forma coherente sin romper la progresión.

---

# INFORME FINAL OBLIGATORIO

Al terminar, entrega un informe indicando:

## 1. Room CPU/RAM

* Qué se modificó.
* Cómo se eliminaron las actividades individuales.
* Cómo se mantiene la información educativa.
* Cómo se mantiene la etiqueta.
* Cómo desaparece `[E]` después de completar.

## 2. Room Disco Duro

* Qué objetos existentes fueron utilizados.
* Cómo se implementó la búsqueda.
* Cómo se selecciona el Shelf correcto.
* Cómo se implementó el Server.
* Cómo funciona la progresión.

## 3. Archivos modificados

Lista exacta de archivos.

## 4. Sistemas reutilizados

Indicar si se reutilizaron:

* `PlayerInteraction`
* `IInteractable`
* `EducationalInteractable`
* `SimpleInteractable`
* `ObjectiveSystem`
* `GameHUD`

## 5. Pruebas

Indicar qué se probó y resultado.

## 6. Errores

Indicar:

* errores nuevos;
* warnings nuevos;
* problemas pendientes.

---

# CRITERIO DE ÉXITO

El Prompt 18 está terminado correctamente cuando:

### CPU/RAM

* Las actividades/preguntas individuales desaparecieron.
* La información educativa permanece.
* Las etiquetas permanecen visibles.
* `[E] Examinar` desaparece después de completar.
* La progresión funciona.
* No se rompe el sistema existente.

### Disco Duro

* La room tiene una misión jugable.
* El jugador debe explorar.
* Puede interactuar con los Shelf with Crates.
* Existe un objetivo relacionado con encontrar un archivo.
* Encontrar el archivo cambia la progresión.
* Existe interacción con un Server.
* El archivo puede ser ejecutado/entregado.
* La misión tiene final.
* El HUD refleja el objetivo actual.
* Las etiquetas permanecen después de completar las interacciones.

### General

* No aparecen errores nuevos.
* No se rompen las interacciones existentes.
* No se modifican assets de terceros innecesariamente.
* La nueva jugabilidad se siente integrada con AstroBit.
* La experiencia deja de depender de preguntas obvias y comienza a tener más exploración e interacción.

---

# REGLA FINAL

**No implementes cambios antes de inspeccionar la arquitectura y la escena actuales.**

Primero determina qué sistemas ya existen y reutilízalos.

No inventes nombres de objetos o scripts.

No reemplaces sistemas funcionales sin necesidad.

No hagas cambios visuales o de gameplay fuera de los objetivos de este prompt.

La prioridad es:

**preservar lo que ya funciona + eliminar las actividades obvias + mantener las etiquetas + añadir una segunda room con una mecánica de exploración/búsqueda que aumente la jugabilidad general de AstroBit.**
