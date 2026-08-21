# PROMPT 16 — CORRECCIÓN DEL PANEL DE ACTIVIDADES + MEJORAS DE EXPERIENCIA EDUCATIVA

Proyecto: AstroBit — Unity

---

# CONTEXTO ACTUAL

AstroBit ya cuenta con una base funcional de jugabilidad educativa implementada en los prompts anteriores.

Actualmente existen y funcionan:

- ALU
- Cache L1
- Cache L2
- Cache L3
- Registros
- Unidad de Control
- RAM1
- RAM2

Cada componente utiliza:

- Collider
- EducationalInteractable
- Label flotante
- interacción mediante [E]
- GameHUD
- panel educativo
- actividad
- respuesta
- recompensa
- ObjectiveSystem
- progresión educativa

También existen actualmente:

- GameHUD
- ObjectiveSystem
- PlayerInteraction
- EducationalInteractable
- LocationZone
- FinalActivity

Y existen dos zonas físicas:

- Zone_CPU
- Zone_RAM

La arquitectura actual funciona correctamente y NO debe ser reemplazada.

---

# PROBLEMA ACTUAL DETECTADO

Durante las pruebas reales del juego se detectó un problema visual en las actividades de tipo Choice.

IMPORTANTE:

El problema NO es necesariamente que las preguntas estén mal planteadas.

El problema principal es que:

**EL PANEL DE LA ACTIVIDAD ES DEMASIADO PEQUEÑO Y/O ESTÁ DEMASIADO ABAJO.**

Como consecuencia, cuando una pregunta tiene suficiente longitud, el texto queda recortado.

Ejemplo observado actualmente en Cache L2:

La actividad muestra algo parecido a:

"LA CACHE L1 NO TIENE EL DATO QUE LA..."

y el resto de la pregunta no aparece.

Esto hace que una pregunta que conceptualmente sí puede tener sentido parezca incompleta o incoherente.

La captura proporcionada en la conversación debe utilizarse como referencia visual del problema.

---

# OBJETIVO PRINCIPAL

Corregir EXCLUSIVAMENTE el tamaño y posición del panel correspondiente a la actividad de preguntas Choice.

NO modificar innecesariamente:

- panel de información
- panel de recompensa
- panel inicial
- sistema de interacción
- labels
- cámara
- jugador
- ALU
- lógica de detección
- lógica de recompensas

El objetivo inmediato es:

1. Mover el panel de actividad un poco más arriba.
2. Reducir/ajustar su tamaño visual si es necesario.
3. Hacer que la pregunta completa sea visible.
4. Mantener visibles todas las opciones.
5. Mantener visible el botón Cerrar.
6. Mantener el panel centrado y visualmente equilibrado.
7. Aplicar la corrección a TODAS las actividades Choice.

---

# MUY IMPORTANTE — NO QUIERO AGRANDAR EL PANEL

No solucionar el problema simplemente haciendo el panel gigante.

La intención visual es exactamente la contraria:

El panel de actividad debe quedar:

- un poco más arriba
- más compacto
- mejor aprovechado verticalmente
- suficientemente pequeño para no tapar demasiado la escena
- pero suficientemente grande para mostrar la pregunta completa

Debe verse como un panel educativo limpio y compacto.

La referencia visual actual muestra que el panel puede ocupar menos espacio vertical y colocarse ligeramente más arriba.

---

# COMPORTAMIENTO ESPERADO

Actualmente:

┌───────────────────────────────┐
│ DIAGNÓSTICO DE CACHE L2       │
│                               │
│ La Cache L1 no tiene el dato  │
│ que la...                     │  ← TEXTO RECORTADO
│                               │
│ CACHE L2                      │
│ RAM                           │
│ CACHE L1                      │
│                               │
│ Cerrar                        │
└───────────────────────────────┘

Debe quedar aproximadamente:

┌───────────────────────────────┐
│ DIAGNÓSTICO DE CACHE L2       │
│                               │
│ La Cache L1 no tiene el dato  │
│ que la CPU necesita. ¿Cuál es │
│ el siguiente nivel de caché   │
│ que debería consultar?        │
│                               │
│ CACHE L2                      │
│ RAM                           │
│ CACHE L1                      │
│                               │
│ Cerrar                        │
└───────────────────────────────┘

La cantidad exacta de líneas dependerá del ancho disponible.

NO cortar frases.

NO truncar texto con "...".

NO reducir el texto de la pregunta solamente para solucionar el problema.

Primero debe solucionarse el layout.

---

# REGLA IMPORTANTE SOBRE EL TAMAÑO

El tamaño debe adaptarse a la cantidad de texto.

Si la pregunta ocupa una línea:

→ panel compacto.

Si ocupa dos líneas:

→ panel ligeramente más alto.

Si ocupa tres líneas:

→ panel debe crecer lo necesario.

Pero siempre:

- mantenerlo centrado
- mantenerlo ligeramente elevado respecto al centro actual
- dejar espacio suficiente para las opciones
- dejar espacio suficiente para Cerrar
- evitar que salga de los límites de la pantalla

No crear diferentes prefabs para cada pregunta.

La solución debe estar en el sistema general de GameHUD.

---

# IMPLEMENTACIÓN DEL PANEL

Antes de modificar:

Inspeccionar exactamente cómo GameHUD construye actualmente:

- Choice panel
- título
- pregunta
- botones de opciones
- botón Cerrar
- RectTransform
- Text / TMP
- tamaños
- anchors
- offsets
- posiciones verticales

Determinar cuál elemento está causando el recorte.

Preferir una solución mínima.

Si el sistema utiliza RectTransform y TextMeshPro:

aprovechar correctamente:

- preferredHeight
- preferredWidth
- wrapping
- overflow
- anchors
- layout
- tamaño dinámico

No inventar un sistema de UI paralelo.

---

# POSICIÓN DEL PANEL

El panel Choice debe quedar ligeramente más arriba que actualmente.

No debe quedar pegado al borde superior.

Debe existir margen suficiente entre:

- título
- pregunta
- opciones
- botón Cerrar

El resultado debe ser visualmente equilibrado.

IMPORTANTE:

No cambiar la posición de los demás modos del GameHUD si no es necesario.

La corrección solicitada es principalmente para:

**Choice / Activity Panel**

---

# PRUEBA OBLIGATORIA DEL PANEL

Probar preguntas de diferente longitud.

Como mínimo:

1. Pregunta corta.
2. Pregunta de dos líneas.
3. Pregunta larga de tres líneas o más.

Confirmar que:

- ninguna pregunta se corta
- ninguna pregunta desaparece
- ninguna opción queda fuera
- Cerrar sigue visible
- el panel no sale de la pantalla
- el panel no ocupa una cantidad excesiva de pantalla

---

# PARTE 2 — REVISAR LAS ACTIVIDADES EDUCATIVAS

Una vez solucionado el problema visual del panel:

revisar las preguntas actuales.

IMPORTANTE:

No cambiar preguntas solamente porque inicialmente parecen extrañas.

Primero comprobar cómo se ven completas después de solucionar el tamaño del panel.

La captura demuestra que parte del problema de coherencia aparente viene de que el texto está siendo cortado.

Después de corregir el layout:

revisar cada actividad para comprobar que:

- la pregunta sea clara
- la pregunta tenga relación con el componente
- la respuesta correcta no sea simplemente obvia porque el componente tiene el mismo nombre
- la actividad enseñe realmente algo
- las opciones incorrectas sean plausibles
- la actividad ayude a comprender la arquitectura

---

# OBJETIVO EDUCATIVO DE LAS ACTIVIDADES

Las actividades NO deben ser simples preguntas de:

"¿Qué componente es este?"

Ejemplo que NO quiero:

Estás en Unidad de Control.

¿Qué componente coordina la CPU?

[ ALU ]
[ UNIDAD DE CONTROL ]
[ RAM ]

Esto es demasiado obvio porque el jugador está literalmente examinando la Unidad de Control.

Las actividades deben plantear una situación.

La pregunta debe hacer que el jugador piense:

"¿Qué ocurriría en este escenario?"

y utilizar el componente que está estudiando.

---

# ACTIVIDADES DESEADAS

## ALU

Concepto:

La ALU realiza operaciones y transformaciones sobre datos.

Ejemplo:

"La CPU necesita comparar dos valores para determinar cuál es mayor. ¿Qué componente realiza esta operación?"

Opciones:

- ALU
- RAM
- Unidad de Control

Correcta:

ALU

Esto enseña qué hace la ALU, en lugar de simplemente preguntar dónde está.

---

# REGISTROS

Concepto:

Los registros almacenan temporalmente datos que la CPU necesita utilizar rápidamente.

Ejemplo:

"Durante la ejecución de una instrucción, la CPU necesita conservar temporalmente un valor que está utilizando en ese momento. ¿Qué opción representa mejor este almacenamiento?"

Opciones:

- Registro
- RAM
- Disco

Correcta:

Registro

---

# UNIDAD DE CONTROL

La actividad debe representar una situación de coordinación.

Ejemplo:

"Una instrucción acaba de llegar a la CPU y debe coordinarse qué componentes participarán en su ejecución. ¿Qué parte de la CPU dirige esa coordinación?"

Opciones:

- Unidad de Control
- ALU
- Cache

Correcta:

Unidad de Control

Esto es mucho más coherente que preguntar simplemente:

"¿Qué componente coordina?"

---

# CACHE L1

Concepto:

L1 es una caché muy rápida y cercana al procesador.

Ejemplo:

"La CPU acaba de utilizar un dato y vuelve a necesitarlo inmediatamente. ¿Qué nivel de memoria conviene consultar primero?"

Opciones:

- Cache L1
- RAM
- Disco

Correcta:

Cache L1

---

# CACHE L2

Concepto:

L2 es un nivel posterior a L1 y normalmente más grande, aunque con mayor latencia que L1.

Ejemplo:

"La CPU necesita un dato, pero la búsqueda en Cache L1 no lo encontró. ¿Qué nivel de caché puede consultar a continuación?"

Opciones:

- Cache L2
- RAM
- Disco

Correcta:

Cache L2

Esta pregunta sí tiene relación directa con el aprendizaje del funcionamiento de la jerarquía.

---

# CACHE L3

Concepto:

L3 es otro nivel de caché que se encuentra antes de llegar a RAM en la representación educativa del juego.

Ejemplo:

"La CPU no encontró el dato en L1 ni en L2. Antes de acudir a la RAM, ¿qué nivel de caché puede consultar?"

Opciones:

- Cache L3
- RAM
- Disco

Correcta:

Cache L3

---

# RAM

Concepto:

La RAM mantiene temporalmente los programas y datos que están siendo utilizados.

Ejemplo:

"Un programa está ejecutándose y necesita mantener sus datos disponibles mientras trabaja. ¿Dónde se mantienen temporalmente esos datos?"

Opciones:

- RAM
- Cache L1
- Disco

Correcta:

RAM

---

# RAM2

RAM2 debe servir para reforzar el concepto de memoria y módulos de RAM.

No hacer una pregunta artificial como:

"¿Qué se puede agregar para tener más memoria?"

si no aporta realmente al aprendizaje.

Buscar una pregunta coherente con el concepto de:

- capacidad
- almacenamiento temporal
- módulos de memoria
- datos en uso

Mantenerla sencilla.

---

# REGLA PARA TODAS LAS ACTIVIDADES

Cada actividad debe responder a:

"¿Qué estoy aprendiendo de este componente?"

y no simplemente:

"¿Cuál es el nombre de este componente?"

La respuesta debe poder explicarse fácilmente durante una presentación.

---

# PARTE 3 — FEEDBACK EDUCATIVO

Agregar/mejorar feedback cuando el jugador responde.

Si responde incorrectamente:

NO limitarse a:

"Respuesta incorrecta."

Mostrar una pequeña explicación educativa.

Ejemplo:

"Incorrecto. La L2 se consulta después de la L1 cuando el dato no está disponible allí."

Debe ser:

- corto
- claro
- educativo

Máximo aproximadamente 1-2 líneas cuando sea posible.

No revelar directamente la respuesta antes de que el jugador tenga oportunidad de intentarlo.

---

# PARTE 4 — RECOMPENSAS EDUCATIVAS

Mantener el sistema actual de recompensas.

Pero mejorar ligeramente el texto para reforzar lo aprendido.

Ejemplos:

✓ ALU COMPRENDIDA

"Realiza operaciones sobre los datos."

✓ REGISTROS COMPRENDIDOS

"Guardan temporalmente datos que la CPU necesita."

✓ UNIDAD DE CONTROL COMPRENDIDA

"Coordina la ejecución de las instrucciones."

✓ CACHE COMPRENDIDA

"Permite acceder rápidamente a datos utilizados con frecuencia."

✓ RAM COMPRENDIDA

"Mantiene temporalmente programas y datos en uso."

No hacer paneles nuevos.

Utilizar el sistema actual de GameHUD.

---

# PARTE 5 — INDICADOR DE PROGRESO

Implementar la mejora recomendada:

mostrar de manera discreta cuánto ha avanzado el jugador.

Ejemplo:

"3/8 COMPONENTES"

o:

"PROGRESO: 3/8"

Debe ser pequeño y no competir visualmente con:

- objetivo
- pista
- ubicación

Puede colocarse cerca del objetivo actual.

IMPORTANTE:

La actividad final NO debe contarse como un componente adicional.

El progreso representa los 8 componentes educativos:

1. ALU
2. Registros
3. Unidad de Control
4. Cache L1
5. Cache L2
6. Cache L3
7. RAM1
8. RAM2

Cuando todos estén comprendidos:

8/8

y posteriormente aparece la actividad final.

---

# PARTE 6 — MENSAJE DE PRIMERA ENTRADA A UNA ZONA

Mejorar ligeramente el sistema de ubicación ya existente.

Cuando el jugador entra por primera vez a una zona:

CPU

mostrar durante unos segundos:

"CPU"

"Centro de procesamiento"

Para RAM:

"RAM"

"Memoria temporal de programas y datos"

Debe ser un mensaje corto.

No mostrarlo constantemente.

Después de unos segundos:

solo queda:

CPU

o:

RAM

como ya funciona actualmente.

No crear otro HUD.

Utilizar GameHUD existente.

---

# PARTE 7 — UBICACIÓN RAM

Actualmente Zone_RAM funciona correctamente, pero su radio/zona resulta demasiado ajustado.

El jugador tiene que estar prácticamente al lado de la RAM para que aparezca:

RAM

Aumentar ligeramente el tamaño de Zone_RAM.

IMPORTANTE:

No exagerar.

Debe cubrir cómodamente la sala de RAM y permitir que el jugador sepa que está en la zona.

Debe ser comparable a la experiencia de Zone_CPU.

No modificar la interacción de RAM.

No modificar proximityRadius.

No modificar interactionDistance.

Modificar solamente la zona de ubicación.

---

# PARTE 8 — ZONA CPU

Zone_CPU actualmente funciona bien.

La zona cubre aproximadamente el 80-90% de la sala.

Si al inspeccionarla se considera seguro:

ampliarla ligeramente para que cubra mejor la sala.

NO hacerla gigantesca.

NO hacer que invada la zona de RAM.

NO crear zonas nuevas artificiales.

Mantener la separación física real existente.

---

# PARTE 9 — POSIBLE MEJORA ADICIONAL

Durante la implementación:

analiza si existe alguna mejora pequeña que realmente aumente la experiencia educativa sin complicar el juego.

Por ejemplo:

- un pequeño mensaje al completar un componente
- una frase que conecte el componente anterior con el siguiente
- una indicación breve de por qué ahora se estudia el siguiente componente
- una pequeña explicación al completar la progresión
- un mensaje de transición entre CPU y RAM

IMPORTANTE:

No implementar ideas por implementar.

Si detectas una mejora que realmente aporte:

1. Explicarla.
2. Justificarla.
3. Implementarla solamente si es pequeña, estable y coherente con el sistema existente.

Máximo 2-3 mejoras adicionales.

NO convertir AstroBit en un RPG.

---

# PARTE 10 — FLUJO EDUCATIVO FINAL

El flujo debería sentirse aproximadamente así:

ENTRAR AL JUEGO

↓

UBICACIÓN

CPU

↓

OBJETIVO

Conoce la ALU.

↓

PISTA

Busca el componente marcado como ALU.

↓

EXPLORACIÓN

↓

ALU

↓

[E] EXAMINAR

↓

EXPLICACIÓN

"La ALU realiza operaciones sobre los datos."

↓

ACTIVIDAD

Situación relacionada con la ALU.

↓

RESPUESTA

↓

FEEDBACK

↓

RECOMPENSA

✓ ALU COMPRENDIDA

↓

PROGRESO

1/8

↓

NUEVO OBJETIVO

Conoce los Registros.

↓

...

↓

CACHE L1

↓

CACHE L2

↓

CACHE L3

↓

RAM

↓

8/8

↓

ACTIVIDAD FINAL

↓

RECORRIDO COMPLETADO

---

# PARTE 11 — RESTRICCIONES ABSOLUTAS

NO crear:

- otro Canvas
- otro GameHUD
- otro sistema de actividades
- otro sistema de interacción
- otro sistema de objetivos
- otro sistema de recompensas
- otro sistema de ubicación

Reutilizar:

- GameHUD
- ObjectiveSystem
- EducationalInteractable
- LocationZone
- FinalActivity
- PlayerInteraction

NO modificar innecesariamente:

- PlayerInteraction.cs
- interactionDistance
- proximityRadius
- Cinemachine
- cámara
- jugador
- MovementInput
- CharacterSkinController
- Input System

NO crear componentes físicos inexistentes:

- Storage
- Disco
- ROM
- Bus
- CPU física
- otros componentes no existentes

NO inventar nuevas salas.

NO crear nuevos objetos educativos del mapa.

NO hacer una refactorización general.

---

# PARTE 12 — PROTEGER LO QUE YA FUNCIONA

Antes de modificar:

comprobar el estado actual.

Después de cada cambio importante:

probar.

La ALU sigue siendo la referencia principal.

Debe continuar funcionando:

ALU_Label

↓

[E]

↓

Panel

↓

Actividad Choice

↓

Respuesta

↓

Recompensa

↓

Objetivo siguiente

↓

Progreso

No aceptar una implementación que arregle el panel pero rompa la ALU.

---

# PRUEBAS OBLIGATORIAS

## PRUEBA 1 — PANEL

Abrir actividades de:

- ALU
- Registros
- Unidad de Control
- Cache L1
- Cache L2
- Cache L3
- RAM1
- RAM2

Confirmar que todas muestran la pregunta completa.

---

## PRUEBA 2 — PREGUNTA LARGA

Utilizar una pregunta de varias líneas.

Confirmar:

- texto completo
- sin clipping
- sin overflow visual
- opciones visibles
- Cerrar visible

---

## PRUEBA 3 — PREGUNTA CORTA

Confirmar que el panel no queda innecesariamente grande.

---

## PRUEBA 4 — ALU

Confirmar:

Label

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

Objetivo siguiente

↓

Progreso 1/8

---

## PRUEBA 5 — REGISTROS

Confirmar actividad conceptual y progreso.

---

## PRUEBA 6 — UNIDAD DE CONTROL

Confirmar que la pregunta tiene sentido y no es simplemente:

"¿Cuál es la Unidad de Control?"

Debe representar una situación.

---

## PRUEBA 7 — CACHE

Probar como mínimo:

L1

L2

L3

Confirmar que las preguntas enseñan la jerarquía.

---

## PRUEBA 8 — RAM

Confirmar:

- zona RAM
- indicador RAM
- actividad
- recompensa
- progreso

---

## PRUEBA 9 — UBICACIÓN

Caminar:

CPU → RAM

y:

RAM → CPU

Confirmar que el indicador cambia correctamente.

---

## PRUEBA 10 — PROGRESO

Completar los 8 componentes.

Confirmar:

1/8
2/8
3/8
...
8/8

y posteriormente:

Actividad final.

---

## PRUEBA 11 — EXPLORACIÓN LIBRE

Confirmar que el jugador puede seguir explorando libremente.

No bloquear físicamente el movimiento.

---

## PRUEBA 12 — ACTIVIDAD FINAL

Confirmar que continúa funcionando después de los cambios.

---

# CRITERIO VISUAL DE ÉXITO

El panel de actividad debe verse:

- compacto
- ligeramente más arriba
- centrado
- limpio
- completamente legible
- sin tapar innecesariamente la escena

La pregunta debe ser completamente visible.

Las opciones deben estar separadas.

Cerrar debe ser claramente visible.

No debe parecer un panel gigante.

---

# CRITERIO EDUCATIVO DE ÉXITO

Cada actividad debe permitir que un estudiante explique:

"Estoy aprendiendo esto porque..."

ALU:

"Aprendo que realiza operaciones sobre datos."

Registros:

"Aprendo que almacenan temporalmente datos que la CPU necesita."

Unidad de Control:

"Aprendo que coordina la ejecución de instrucciones."

Cache:

"Aprendo que permite acceder rápidamente a datos utilizados."

RAM:

"Aprendo que mantiene temporalmente programas y datos en uso."

La actividad debe reforzar ese aprendizaje.

---

# INFORME FINAL OBLIGATORIO

Al terminar, entregar un informe con:

1. Problema exacto encontrado en el panel Choice.
2. Qué elemento causaba el recorte de la pregunta.
3. Qué cambio hiciste en GameHUD.
4. Cómo quedó ajustada la posición del panel.
5. Cómo se calcula/adapta el tamaño del panel.
6. Confirmación de que las preguntas completas son visibles.
7. Qué preguntas conceptuales quedaron configuradas.
8. Respuesta correcta de cada actividad.
9. Qué feedback educativo se agregó.
10. Qué recompensas educativas se configuraron.
11. Cómo quedó el indicador de progreso.
12. Cómo se actualiza el progreso.
13. Cómo quedó Zone_RAM.
14. Cómo quedó Zone_CPU.
15. Cómo funciona el mensaje de primera entrada a cada zona.
16. Qué mejoras adicionales implementaste y por qué.
17. Qué scripts modificaste.
18. Qué cambios hiciste en Inspector.
19. Qué GameObjects nuevos creaste, si alguno.
20. Confirmación de que no se creó otro Canvas/HUD/sistema paralelo.
21. Confirmación de que ALU sigue funcionando.
22. Confirmación de que Registros sigue funcionando.
23. Confirmación de que Unidad de Control sigue funcionando.
24. Confirmación de que Cache L1/L2/L3 siguen funcionando.
25. Confirmación de que RAM1/RAM2 siguen funcionando.
26. Confirmación de que la progresión sigue funcionando.
27. Confirmación de que la actividad final sigue funcionando.
28. Errores nuevos.
29. Warnings nuevos.
30. Qué quedó pendiente.

---

# REGLA FINAL

NO continuar con funcionalidades nuevas después de completar este prompt.

Primero dejar completamente estable:

1. Panel Choice corregido.
2. Preguntas completas y legibles.
3. Actividades conceptuales coherentes.
4. Feedback educativo.
5. Recompensas educativas.
6. Progreso 0/8 → 8/8.
7. Ubicación CPU/RAM.
8. Guía.
9. Actividad final.
10. Todas las pruebas.

Si durante la implementación encuentras una mejora adicional importante:

NO la implementes automáticamente si puede cambiar demasiado la arquitectura.

Primero indícala en el informe como propuesta.

La prioridad absoluta es:

NO ROMPER LO QUE YA FUNCIONA.

Y convertir AstroBit en una experiencia educativa sencilla, jugable, coherente y fácil de demostrar.