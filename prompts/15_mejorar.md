# PROMPT 15 — PULIDO DE JUGABILIDAD, ACTIVIDADES EDUCATIVAS Y EXPERIENCIA DE ASTROBIT

Proyecto: AstroBit — Unity

---

# CONTEXTO

AstroBit ya tiene implementado y funcionando el sistema desarrollado en los prompts anteriores.

El estado actual confirmado es:

- ALU
- Cache L1
- Cache L2
- Cache L3
- Registros
- Unidad de Control
- RAM1
- RAM2

Todos tienen:

- Collider
- EducationalInteractable
- etiqueta flotante
- interacción mediante [E]
- panel educativo
- actividad
- recompensa
- integración con ObjectiveSystem

También existe actualmente:

- GameHUD
- ObjectiveSystem
- sistema de progresión educativa
- sistema de guía mediante Objetivo + Pista
- sistema de ubicación por zonas
- Zone_CPU
- Zone_RAM
- FinalActivity
- actividades conceptuales mediante selección de opciones

La progresión actual funciona y ya fue probada.

NO rehacer estos sistemas.

Este prompt es una fase de MEJORA Y PULIDO sobre la base existente.

---

# OBJETIVO PRINCIPAL

Quiero mejorar la experiencia para que AstroBit deje de sentirse como:

"camina hasta un objeto → responde una pregunta → siguiente objeto"

y empiece a sentirse más como:

"explora → descubre → comprende qué hace el componente → realiza una pequeña acción relacionada → aprende → continúa".

La experiencia debe seguir siendo:

- sencilla
- educativa
- intuitiva
- entretenida
- fácil de explicar
- fácil de demostrar
- coherente con la arquitectura básica de una computadora
- sin convertirse en un simulador de CPU real

No quiero complejidad innecesaria.

---

# REGLA MÁS IMPORTANTE

NO modificar algo simplemente porque "se puede mejorar".

Antes de tocar una parte del sistema:

1. Inspecciona cómo funciona actualmente.
2. Identifica exactamente qué está causando el problema.
3. Propón el cambio mínimo necesario.
4. Implementa.
5. Prueba.
6. Comprueba que no se haya roto lo anterior.

La ALU y el sistema actual de interacción son la referencia funcional.

---

# PARTE 1 — CORREGIR EL HUD DE OBJETIVO Y PISTA

Actualmente el HUD superior izquierdo presenta un problema visual.

El texto de:

OBJETIVO

y:

PISTA

está demasiado junto verticalmente y puede llegar a superponerse.

Ejemplo actual:

Conoce la ALU.

Pista: Busca un componente marcado como ALU dentro de la CPU.

El segundo texto termina apareciendo demasiado cerca o debajo del primero, haciendo que la lectura sea incómoda.

## SOLICITUD

Ajustar el layout del HUD existente para que:

OBJETIVO

Conoce la ALU.

Pista:

Busca el componente marcado como ALU dentro de la CPU.

tenga una separación visual clara.

La pista debe quedar un poco más abajo del objetivo.

NO crear otro Canvas.

NO crear otro HUD.

NO crear otro sistema de texto.

Utilizar el GameHUD existente.

El resultado debe ser visualmente limpio.

---

# PARTE 2 — REVISAR TODAS LAS ACTIVIDADES

Esta es la parte MÁS IMPORTANTE del prompt.

Actualmente las actividades conceptuales funcionan técnicamente, pero algunas preguntas no tienen suficiente sentido pedagógico.

Ejemplo actual de Unidad de Control:

"La CPU necesita ejecutar una..."

[ ALU ]

[ UNIDAD DE CONTROL ]

[ RAM ]

Esta pregunta no es suficientemente buena.

¿Por qué?

Porque el jugador está parado frente a la Unidad de Control y prácticamente la respuesta está revelada por el propio contexto.

No quiero preguntas de este estilo.

---

# PRINCIPIO PARA TODAS LAS ACTIVIDADES

La actividad NO debe preguntar:

"¿Qué componente eres?"

Debe plantear una pequeña situación.

El jugador debe pensar:

"¿Qué está intentando hacer la computadora?"

↓

"¿Qué componente participa en ese proceso?"

↓

"¿Qué debería ocurrir?"

La respuesta correcta debe depender de haber entendido el concepto, no simplemente de mirar el objeto que está delante.

---

# OBJETIVO EDUCATIVO

Cada actividad debe enseñar algo sobre:

1. Qué hace el componente.
2. Cuándo participa.
3. Cómo se relaciona con otros componentes.
4. Qué ocurre antes o después.
5. Por qué es útil dentro de la computadora.

NO hace falta enseñar todos los detalles técnicos.

La explicación debe ser simple y correcta.

---

# ALU

## Concepto

La ALU realiza operaciones sobre datos.

No quiero volver a usar:

12 + 7

8 + 5

etc.

## Tipo de actividad recomendado

Presentar una situación.

Ejemplo:

"La CPU necesita comparar dos valores para saber si son iguales."

¿Qué componente realiza esta operación?

[ ALU ]

[ RAM ]

[ UNIDAD DE CONTROL ]

Respuesta:

ALU.

Otra posibilidad:

"La CPU necesita realizar una operación sobre dos datos que ya tiene disponibles."

¿Qué componente realiza el cálculo?

[ ALU ]

[ CACHE ]

[ RAM ]

Respuesta:

ALU.

La actividad debe enseñar que:

La ALU procesa y transforma datos mediante operaciones.

---

# REGISTROS

## Concepto

Los registros son pequeñas ubicaciones de almacenamiento muy rápidas dentro de la CPU.

Se utilizan para mantener temporalmente datos o información que la CPU necesita durante la ejecución.

## Actividad

Ejemplo:

"La CPU está trabajando con un valor que necesita utilizar inmediatamente."

¿Dónde puede mantenerlo temporalmente para trabajar con él rápidamente?

[ REGISTRO ]

[ DISCO ]

[ MONITOR ]

Respuesta:

REGISTRO.

Otra posibilidad:

"Un dato ya está siendo utilizado por la CPU y necesita mantenerse disponible durante el procesamiento."

¿Qué opción tiene más sentido?

[ REGISTRO ]

[ DISCO ]

[ TECLADO ]

Respuesta:

REGISTRO.

La pregunta NO debe ser simplemente:

"¿Qué componente almacena datos?"

porque eso también podría generar confusión con RAM o cache.

---

# UNIDAD DE CONTROL

Esta actividad debe ser revisada especialmente.

## Concepto

La Unidad de Control coordina las acciones necesarias para ejecutar instrucciones.

No realiza los cálculos de la ALU.

No es una memoria.

Su función principal dentro de esta representación educativa es coordinar qué debe hacerse y en qué momento.

## Actividad recomendada

Ejemplo:

"La CPU tiene una instrucción que debe ejecutar y necesita coordinar qué componentes deben participar."

¿Qué componente se encarga de coordinar este proceso?

[ UNIDAD DE CONTROL ]

[ ALU ]

[ RAM ]

Respuesta:

UNIDAD DE CONTROL.

Otra posibilidad:

"Una instrucción indica que se debe realizar una operación con unos datos. Antes de que la operación pueda ejecutarse, alguien debe coordinar los pasos necesarios."

¿Qué componente cumple esa función?

[ UNIDAD DE CONTROL ]

[ CACHE L1 ]

[ ALU ]

Respuesta:

UNIDAD DE CONTROL.

IMPORTANTE:

No preguntar:

"¿Qué componente coordina la CPU?"

simplemente porque el jugador está frente a la Unidad de Control.

Debe existir una situación que explique por qué se necesita.

---

# CACHE L1

## Concepto

La caché permite mantener datos/instrucciones de acceso frecuente cerca del procesador para reducir el tiempo de acceso.

L1 es el nivel más cercano y normalmente el más rápido entre los niveles de caché mostrados en AstroBit.

## Actividad

Ejemplo:

"La CPU necesita nuevamente un dato que utilizó hace poco."

¿Cuál sería el primer lugar lógico que debería consultar entre estos niveles?

[ CACHE L1 ]

[ RAM ]

[ DISCO ]

Respuesta:

CACHE L1.

La actividad debe enseñar el concepto de reutilización y cercanía.

---

# CACHE L2

## Concepto

L2 es otro nivel de caché, normalmente con mayor capacidad que L1 pero con mayor latencia.

## Actividad

Ejemplo:

"La CPU necesita un dato, pero no está disponible en L1."

¿Qué nivel de caché puede consultar después?

[ CACHE L2 ]

[ RAM ]

[ DISCO ]

Respuesta:

CACHE L2.

Esto además debe conectar conceptualmente L1 → L2.

---

# CACHE L3

## Concepto

L3 es otro nivel de caché que puede proporcionar almacenamiento rápido antes de recurrir a la RAM.

## Actividad

Ejemplo:

"El dato no se encontró en L1 ni en L2."

¿Qué opción representa el siguiente nivel de caché antes de consultar la RAM?

[ CACHE L3 ]

[ DISCO ]

[ MONITOR ]

Respuesta:

CACHE L3.

Esto enseña una secuencia.

---

# RAM

RAM1 y RAM2 representan módulos de memoria RAM.

No quiero que ambas actividades sean exactamente iguales.

## RAM1

Concepto:

La RAM mantiene temporalmente programas y datos que están siendo utilizados.

Actividad:

"Un programa está ejecutándose y necesita mantener sus datos disponibles mientras trabaja."

¿Dónde se almacenan temporalmente esos datos?

[ RAM ]

[ DISCO ]

[ MONITOR ]

Respuesta:

RAM.

---

# RAM2

Quiero que RAM2 introduzca el concepto de capacidad/módulos.

Ejemplo:

"El sistema necesita disponer de más memoria RAM."

¿Qué solución representa mejor esta situación?

[ AGREGAR OTRO MÓDULO DE RAM ]

[ CAMBIAR LA ALU ]

[ ELIMINAR LA CACHE ]

Respuesta:

AGREGAR OTRO MÓDULO DE RAM.

Esto debe ayudar a diferenciar RAM1 y RAM2 educativamente.

---

# IMPORTANTE SOBRE LAS PREGUNTAS

NO copiar literalmente todos los ejemplos anteriores si al inspeccionar el sistema existe una forma mejor de implementarlos.

Primero analiza cada actividad.

Para cada componente determina:

- concepto principal
- concepto secundario
- situación
- pregunta
- opciones
- respuesta
- explicación posterior

Las preguntas deben evitar:

- respuestas obvias por el nombre del objeto
- preguntas ambiguas
- conceptos técnicamente incorrectos
- respuestas que puedan considerarse correctas en más de una opción
- preguntas que simplemente repitan el título del componente

---

# PARTE 3 — HACER LAS ACTIVIDADES MÁS "JUGABLES"

Actualmente todas las actividades utilizan selección de respuesta.

NO quiero crear un sistema gigantesco.

Pero quiero evaluar si podemos introducir pequeñas variaciones.

Por ejemplo:

## Tipo A — Elegir

¿Qué componente debería participar?

[ ALU ]
[ RAM ]
[ CACHE ]

## Tipo B — Elegir el siguiente paso

El dato no está en L1.

¿Qué ocurre después?

[ Buscar en L2 ]
[ Ir directamente al disco ]
[ Ejecutar la ALU ]

## Tipo C — Identificar una función

¿Qué ocurre principalmente en este punto del proceso?

[ Coordinar ]
[ Procesar datos ]
[ Almacenar temporalmente ]

## Tipo D — Ordenar un pequeño flujo

Por ejemplo:

1. Buscar un dato.
2. Encontrarlo en caché.
3. Utilizarlo en la operación.

Si implementar ordenar elementos requiere demasiado cambio en la arquitectura actual:

NO implementarlo todavía.

Primero determina si una variación simple puede reutilizar el sistema actual de Choice.

La prioridad es estabilidad.

---

# PARTE 4 — CONSTRUIR UNA NARRATIVA EDUCATIVA

Actualmente el jugador tiene objetivos.

Quiero que esos objetivos tengan un poco más de sentido como recorrido.

La experiencia debería sentirse como:

"Voy a conocer cómo funciona una computadora."

No simplemente:

"Ahora ve al siguiente objeto."

Revisar los textos de:

- objetivos
- pistas
- subtítulos
- explicaciones
- recompensas

para que formen una narrativa sencilla.

Ejemplo:

OBJETIVO:

Conoce la ALU.

PISTA:

Busca el componente marcado como ALU dentro de la CPU.

Al completar:

✓ ALU COMPRENDIDA

"La ALU se encarga de realizar operaciones sobre los datos."

Siguiente:

"Ahora descubre dónde la CPU mantiene temporalmente los datos que necesita."

OBJETIVO:

Conoce los Registros.

Esto debe continuar progresivamente.

---

# PARTE 5 — CONECTAR LOS COMPONENTES

Quiero que las actividades empiecen a enseñar relaciones.

No solo:

ALU = hace operaciones.

Sino:

"Para que la ALU pueda trabajar, necesita datos."

"Los registros pueden mantener datos que la CPU está utilizando."

"La Unidad de Control coordina la ejecución."

"La caché mantiene datos/instrucciones de acceso frecuente cerca del procesador."

"La RAM mantiene temporalmente programas y datos en uso."

De esta manera el jugador va construyendo un modelo mental.

---

# PARTE 6 — REVISAR EL ORDEN PEDAGÓGICO

Actualmente la secuencia es:

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

↓

RAM1

↓

RAM2

Analiza si este orden sigue siendo el mejor para enseñar los conceptos.

NO cambiarlo automáticamente.

Si consideras que existe un orden pedagógicamente mejor, indícalo primero.

Si el cambio es pequeño y claramente mejora la experiencia, puedes implementarlo.

La secuencia debe poder explicarse fácilmente durante una presentación.

---

# PARTE 7 — MEJORAR EL INDICADOR DE UBICACIÓN

Actualmente:

CPU funciona muy bien.

La zona CPU cubre aproximadamente el 80–90% de la habitación y el indicador aparece correctamente.

NO reducir esa zona.

Si es posible, ampliarla ligeramente para cubrir mejor la habitación sin invadir otras zonas.

---

# RAM

Actualmente la zona RAM es demasiado pequeña.

El jugador prácticamente debe estar al lado de RAM1/RAM2 para que aparezca:

RAM

Quiero ampliar ligeramente Zone_RAM.

Objetivo:

Que el indicador RAM aparezca cuando el jugador entre claramente a la sala de RAM, no únicamente cuando esté cerca físicamente de los módulos.

No exagerar.

No hacer que RAM aparezca mientras el jugador todavía está en el pasillo o en otra sala.

Ajustar el BoxCollider de Zone_RAM utilizando la geometría real de la habitación.

---

# IMPORTANTE SOBRE ZONAS

No crear zonas artificiales nuevas si no existen físicamente.

Actualmente solo existen:

Zone_CPU

Zone_RAM

Mantenerlas como base.

Si al inspeccionar la escena existe otra separación física evidente que pueda mejorar la experiencia, puedes proponerla.

Pero:

NO crear Storage.

NO crear Disco.

NO crear ROM.

NO crear Bus.

NO inventar habitaciones inexistentes.

---

# PARTE 8 — PROPONER MEJORAS

Quiero que además pienses como diseñador de juego educativo.

Después de inspeccionar el proyecto, determina si falta alguna pequeña cosa que pueda mejorar mucho la experiencia.

Ejemplos de cosas que podrías considerar:

- indicador de progreso "3/8"
- mensaje de transición entre componentes
- pequeño mensaje de "nuevo conocimiento adquirido"
- mejor feedback después de una respuesta incorrecta
- pistas progresivas
- una actividad final más interactiva
- conexión visual entre conceptos
- una pequeña introducción al comenzar
- una pantalla final del recorrido
- algún elemento que motive a continuar explorando

PERO:

NO implementes automáticamente una nueva funcionalidad importante solo porque la encuentres interesante.

Primero clasifica las propuestas:

### IMPRESCINDIBLE
Necesario para que la experiencia funcione correctamente.

### RECOMENDADO
Mejoraría bastante la experiencia y es sencillo de implementar.

### OPCIONAL
Podría implementarse en un prompt posterior.

Implementa solamente lo que corresponda al alcance de este prompt.

---

# PARTE 9 — ACTIVIDAD FINAL

Revisar la actividad final existente.

Actualmente resume:

UNIDAD DE CONTROL

↓

REGISTROS / CACHE

↓

ALU

↓

RESULTADO

↓

RAM

La actividad final debe tener coherencia con lo aprendido.

No debe convertirse en un examen difícil.

Debe comprobar que el jugador entendió las relaciones principales.

Por ejemplo:

Pregunta 1:

"Una instrucción necesita ser coordinada. ¿Qué componente participa principalmente en esa función?"

Pregunta 2:

"La CPU necesita utilizar rápidamente un dato que está disponible para ella. ¿Qué opción representa mejor el almacenamiento cercano y rápido?"

Pregunta 3:

"La CPU necesita realizar una operación sobre datos. ¿Qué componente realiza esa operación?"

Pregunta 4:

"¿Dónde se mantienen temporalmente programas y datos que están siendo utilizados?"

Las preguntas deben relacionarse entre sí y terminar formando una pequeña explicación del flujo.

---

# PARTE 10 — COMPATIBILIDAD

NO romper:

- PlayerInteraction.cs
- EducationalInteractable.cs
- ObjectiveSystem.cs
- GameHUD.cs
- FinalActivity.cs
- LocationZone.cs
- sistema de interacción
- sistema de recompensas
- sistema de progresión
- Cinemachine
- cámara
- jugador
- MovementInput
- CharacterSkinController
- Input System

Esto NO significa que esté prohibido modificar GameHUD o EducationalInteractable.

Si una modificación es necesaria:

- modificar lo mínimo
- conservar la funcionalidad existente
- no crear sistemas paralelos
- probar inmediatamente

---

# PARTE 11 — NO ELIMINAR LAS ACTIVIDADES ANTERIORES HASTA TENER LAS NUEVAS

Las actividades de suma/resta ya no se utilizan en los componentes finales.

Sin embargo:

NO eliminar inmediatamente el soporte Math.

Primero:

1. Implementar/ajustar las actividades conceptuales.
2. Configurar los 8 componentes.
3. Probarlos.
4. Confirmar que Choice funciona correctamente.
5. Confirmar que la progresión funciona.
6. Confirmar que la ALU sigue funcionando.
7. Solo entonces evaluar si el código Math puede eliminarse.

Si no es necesario eliminarlo:

déjalo como compatibilidad interna.

---

# PARTE 12 — PRUEBAS OBLIGATORIAS

## PRUEBA 1 — HUD

Comprobar que:

Objetivo

y

Pista

no se superponen.

---

## PRUEBA 2 — ALU

Comprobar:

Label

↓

[E]

↓

Panel

↓

Nueva actividad conceptual

↓

Respuesta correcta

↓

Recompensa

↓

Siguiente objetivo

---

## PRUEBA 3 — REGISTROS

Mismo flujo.

La actividad debe tener sentido respecto al concepto.

---

## PRUEBA 4 — UNIDAD DE CONTROL

La actividad debe ser contextual.

No debe ser una pregunta cuya respuesta sea simplemente el nombre del objeto.

---

## PRUEBA 5 — CACHE

Comprobar L1 → L2 → L3.

Las actividades deben enseñar la relación entre niveles.

---

## PRUEBA 6 — RAM

Comprobar RAM1 y RAM2.

---

## PRUEBA 7 — UBICACIÓN CPU

Moverse por la habitación.

Confirmar que:

CPU

aparece de manera estable.

---

## PRUEBA 8 — UBICACIÓN RAM

Entrar a la habitación RAM desde una distancia razonable de los módulos.

Confirmar que:

RAM

aparece antes de estar pegado físicamente a los módulos.

Confirmar que no invade incorrectamente la zona CPU/pasillo.

---

## PRUEBA 9 — PROGRESIÓN

Completar:

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

↓

RAM1

↓

RAM2

y confirmar que el flujo sigue funcionando.

---

## PRUEBA 10 — EXPLORACIÓN LIBRE

El jugador debe poder caminar libremente.

No debe quedar atrapado ni bloqueado.

---

# REGLAS ABSOLUTAS

NO crear:

- Storage
- Disco
- ROM
- Bus
- CPU física nueva
- habitaciones nuevas
- sistemas de interacción paralelos
- HUD paralelo
- Canvas paralelo
- sistema de actividades paralelo
- sistema de objetivos paralelo

NO rehacer:

- cámara
- Cinemachine
- jugador
- movimiento
- interacción
- recompensas

NO hacer una refactorización general.

NO cambiar componentes físicos existentes.

NO mover objetos del mapa salvo que sea estrictamente necesario y esté justificado.

NO inventar arquitectura computacional real.

---

# CRITERIO EDUCATIVO

Cada actividad debe poder responderse después de haber leído la explicación del componente.

Pero la respuesta NO debe ser obvia simplemente porque el jugador está parado frente al componente.

La actividad debe hacer que el jugador piense brevemente:

"Ah, entonces en esta situación participa este componente."

Ese es el nivel de dificultad deseado.

---

# CRITERIO DE JUGABILIDAD

Cada interacción debería sentirse así:

EXPLORAR

↓

ENCONTRAR

↓

DESCUBRIR

↓

ENTENDER

↓

TOMAR UNA DECISIÓN

↓

RECIBIR FEEDBACK

↓

APRENDER

↓

CONTINUAR

No:

CAMINAR

↓

LEER

↓

CLIC

↓

SIGUIENTE

---

# PRIORIDAD

Orden de prioridad:

1. Mantener todo lo que funciona.
2. Corregir el HUD de objetivo/pista.
3. Mejorar las actividades para que tengan coherencia educativa.
4. Hacer que las actividades enseñen relaciones entre componentes.
5. Mejorar ligeramente Zone_RAM.
6. Ajustar Zone_CPU solamente si es necesario.
7. Revisar y mejorar la actividad final.
8. Proponer mejoras adicionales de experiencia.

---

# RESTRICCIÓN DE ALCANCE

No continuar desarrollando nuevas funcionalidades después de completar este prompt.

Si durante la implementación encuentras una idea interesante que excede este alcance:

NO la implementes.

Inclúyela en:

"Propuestas para un próximo prompt".

---

# INFORME FINAL OBLIGATORIO

Al terminar, entregar un informe con:

1. Problema encontrado en el HUD.
2. Cómo se corrigió el espacio entre objetivo y pista.
3. Scripts modificados.
4. Cambios realizados en GameHUD.
5. Cambios realizados en las actividades.
6. Actividad final de cada componente.
7. Respuesta correcta de cada actividad.
8. Concepto educativo que enseña cada actividad.
9. Cómo cada actividad evita ser una pregunta obvia.
10. Qué relación entre componentes enseña cada actividad.
11. Orden final de progresión.
12. Cambios realizados en Zone_RAM.
13. Dimensiones finales de Zone_RAM.
14. Si se modificó Zone_CPU.
15. Cómo funciona actualmente el indicador de ubicación.
16. Qué se probó.
17. Resultado de la prueba de ALU.
18. Resultado de la prueba de Registros.
19. Resultado de la prueba de Unidad de Control.
20. Resultado de la prueba de Cache.
21. Resultado de la prueba de RAM.
22. Resultado de la prueba de progresión.
23. Resultado de la prueba de exploración.
24. Errores nuevos.
25. Warnings nuevos.
26. Archivos modificados.
27. Cambios realizados en Inspector.
28. Qué propuestas adicionales detectaste.
29. Clasifica esas propuestas como:
   - Imprescindible
   - Recomendado
   - Opcional
30. Qué quedó pendiente.

---

# RESULTADO FINAL ESPERADO

AstroBit debe sentirse como un pequeño recorrido educativo.

El jugador debe poder:

ENTRAR

↓

SABER DÓNDE ESTÁ

↓

VER EL OBJETIVO

↓

LEER UNA PISTA CORTA

↓

EXPLORAR

↓

ENCONTRAR UN COMPONENTE

↓

EXAMINARLO

↓

ENTENDER QUÉ HACE

↓

REALIZAR UNA ACTIVIDAD RELACIONADA CON SU FUNCIÓN

↓

RECIBIR FEEDBACK

↓

COMPRENDER SU RELACIÓN CON OTROS COMPONENTES

↓

RECIBIR UN NUEVO OBJETIVO

↓

CONTINUAR EXPLORANDO

La meta NO es crear una CPU real.

La meta es que al terminar el recorrido un estudiante pueda explicar, con palabras sencillas:

- qué hace la Unidad de Control
- qué hacen los Registros
- qué hace la ALU
- para qué sirve la Cache
- qué diferencia existe entre L1, L2 y L3
- para qué sirve la RAM
- cómo se relacionan estos componentes dentro de una computadora

Y que además pueda demostrarlo jugando.

NO continuar con nuevas funcionalidades después de este prompt.