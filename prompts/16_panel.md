# PROMPT 15 — AJUSTE DE ACTIVIDADES, GUÍA Y MEJORAS DE JUGABILIDAD

Proyecto: AstroBit — Unity

---

# CONTEXTO

AstroBit actualmente tiene funcionando:

- ALU
- Cache L1
- Cache L2
- Cache L3
- Registros
- Unidad de Control
- RAM1
- RAM2

El sistema actual funciona correctamente:

Jugador
↓
Etiqueta
↓
[E] Examinar
↓
Panel educativo
↓
Actividad conceptual
↓
Respuesta
↓
Recompensa
↓
Progresión

También existen actualmente:

- GameHUD
- ObjectiveSystem
- EducationalInteractable
- PlayerInteraction
- LocationZone
- FinalActivity

La progresión educativa y el sistema de ubicación implementados en el Prompt 14 funcionan correctamente.

NO rehacer estos sistemas.

---

# OBJETIVO PRINCIPAL

Corregir el diseño visual del panel de actividades conceptuales.

Actualmente las preguntas de las actividades pueden aparecer cortadas porque el espacio destinado al texto de la pregunta es demasiado pequeño.

La captura proporcionada como referencia muestra exactamente el problema:

La pregunta aparece dentro del panel, pero el texto se corta antes de mostrar todo su contenido.

Por ejemplo, se puede alcanzar a leer algo como:

"La Cache L1 no tiene el dato que la..."

pero la pregunta completa no aparece.

Esto hace que algunas actividades parezcan incoherentes o sin sentido cuando en realidad el problema es que la interfaz está recortando el texto.

---

# CAMBIO 1 — CORREGIR EL TAMAÑO DE LA PREGUNTA

Modificar EXCLUSIVAMENTE el área visual correspondiente al texto de la pregunta dentro del panel de actividad.

NO cambiar innecesariamente:

- el tamaño general del panel
- los botones de respuesta
- el botón Cerrar
- el título
- el subtítulo
- el sistema de interacción
- la lógica de actividades
- la progresión
- las recompensas

La prioridad es que la pregunta completa sea visible.

## Ajuste deseado

Quiero que el texto de la pregunta:

1. Suba ligeramente dentro del panel.
2. Tenga un tamaño de fuente ligeramente menor.
3. Disponga de suficiente espacio vertical para mostrar preguntas completas.
4. No quede pegado a los botones.
5. No se superponga con ninguna otra parte del panel.
6. Mantenga una lectura cómoda.

NO hacer la fuente excesivamente pequeña.

El objetivo NO es simplemente "hacerla caber" reduciéndola muchísimo.

La idea es:

- subir un poco la pregunta
- reducir moderadamente el tamaño
- darle algo más de espacio
- mantener una apariencia visual limpia

---

# IMPORTANTE — USAR WRAP

Antes de reducir demasiado el tamaño de fuente, comprobar que el texto de la pregunta tenga correctamente configurado:

- Word Wrapping
- Overflow adecuado
- alineación
- altura del RectTransform

La pregunta debe poder ocupar varias líneas.

Por ejemplo:

En lugar de intentar mostrar:

"La Cache L1 no tiene el dato que la CPU necesita"

en una sola línea, permitir:

"La Cache L1 no tiene el dato que la CPU necesita"

en 2 o 3 líneas si es necesario.

La pregunta completa debe permanecer visible.

---

# NO CAMBIAR EL CONTENIDO DE LAS PREGUNTAS EN ESTE PASO

Primero corregir la interfaz.

No reemplazar las preguntas actuales simplemente porque se ven incompletas.

Quiero comprobar primero cómo quedan una vez que todo el texto sea visible.

Si después de ver la pregunta completa detectas que alguna pregunta realmente no tiene coherencia educativa, puedes señalarlo en el informe final y proponer una mejora.

Pero NO cambiar contenido educativo innecesariamente durante este ajuste visual.

---

# APLICAR A TODAS LAS ACTIVIDADES

El cambio debe hacerse de forma general.

NO corregir solamente Cache L2.

Todas las actividades conceptuales deben utilizar el mismo layout corregido:

- ALU
- Registros
- Unidad de Control
- Cache L1
- Cache L2
- Cache L3
- RAM1
- RAM2
- Actividad final, si utiliza el mismo panel de preguntas

La solución debe estar en GameHUD o en el sistema común que construye el panel.

NO crear un panel diferente para cada componente.

---

# REFERENCIA VISUAL

La captura proporcionada muestra cómo está actualmente la actividad.

Úsala solamente como referencia visual del problema.

La solución buscada es aproximadamente:

┌──────────────────────────────────────────┐
│          DIAGNÓSTICO DE CACHE L2         │
│                                          │
│  La Cache L1 no tiene el dato que la     │
│  CPU necesita. ¿Dónde debería buscar?    │
│                                          │
│              CACHE L2                    │
│              RAM                         │
│              CACHE L1                    │
│                                          │
│       Cerrar                            │
└──────────────────────────────────────────┘

No tiene que quedar exactamente así.

Lo importante es:

PREGUNTA COMPLETA
↓
OPCIONES COMPLETAS
↓
BOTONES SIN SUPERPOSICIÓN

---

# CAMBIO 2 — ESPACIADO DEL OBJETIVO Y PISTA

También corregir el pequeño problema visual existente en la esquina superior izquierda.

Actualmente aparecen:

OBJETIVO ACTUAL
Conoce la ALU.

y debajo:

PISTA: Busca el componente marcado como ALU dentro de la CPU.

El texto de la pista queda demasiado cerca del objetivo y puede parecer que se superpone.

Aumentar ligeramente el espacio vertical entre:

- objetivo
- pista

Mantener ambos dentro del mismo GameHUD.

NO crear otro Canvas.

NO moverlos a otra interfaz.

La estructura debe seguir siendo:

OBJETIVO ACTUAL

Conoce la ALU.

PISTA

Busca el componente marcado como ALU dentro de la CPU.

Con una separación visual clara.

---

# CAMBIO 3 — UBICACIÓN DE RAM

El sistema de ubicación ya funciona correctamente.

Actualmente:

CPU
→ funciona correctamente.

RAM
→ funciona, pero la zona de detección resulta demasiado ajustada.

El jugador prácticamente tiene que estar al lado de los módulos RAM para que aparezca:

RAM

Quiero ampliar ligeramente Zone_RAM.

NO hacerla gigantesca.

NO cubrir habitaciones que no correspondan.

Simplemente aumentar su área para que la ubicación "RAM" aparezca de manera natural cuando el jugador entre a la sala de RAM.

Mantener:

- Zone_RAM
- LocationZone
- el mismo sistema actual
- una sola ubicación activa

Ajustar únicamente el tamaño/posición del BoxCollider de Zone_RAM si es suficiente.

NO modificar el código de detección si no es necesario.

---

# CAMBIO 4 — ZONA CPU

La zona CPU actualmente funciona muy bien.

La CPU cubre aproximadamente el 80–90 % de la habitación y eso está bien.

Si al inspeccionar la geometría real resulta seguro ampliar ligeramente Zone_CPU para cubrir un poco más de la habitación, hacerlo de manera moderada.

NO hacer que invada zonas que claramente pertenecen a RAM u otras áreas.

La prioridad es conservar el comportamiento actual.

Si Zone_CPU ya está suficientemente bien, NO modificarla.

---

# CAMBIO 5 — MEJORA DE LAS ACTIVIDADES

Después de corregir visualmente el panel, revisar las preguntas conceptuales actuales.

Ahora que se puede leer correctamente el texto completo, verificar si cada actividad tiene sentido desde el punto de vista educativo.

El objetivo de cada actividad debe ser:

"Estoy inspeccionando este componente y la actividad me ayuda a entender qué hace y cómo se relaciona con los demás."

Evitar preguntas cuya respuesta sea simplemente el nombre del objeto que estoy mirando sin enseñar nada.

Por ejemplo, una pregunta como:

"La CPU necesita ejecutar una instrucción. ¿Qué componente es?"

con:

ALU
UNIDAD DE CONTROL
RAM

puede resultar demasiado obvia si estoy parado frente a la Unidad de Control.

No quiero ese tipo de pregunta trivial.

---

# CRITERIO PARA LAS ACTIVIDADES

Cada actividad debe tener:

1. Una situación sencilla.
2. Una pequeña decisión del jugador.
3. Una relación con el funcionamiento del componente.
4. Una respuesta correcta clara.
5. Opciones incorrectas razonables.
6. Una explicación breve después de responder.

Ejemplo:

## UNIDAD DE CONTROL

Situación:

"La CPU recibe una instrucción y necesita coordinar qué debe ocurrir primero."

Pregunta:

"¿Qué componente se encarga de coordinar esta ejecución?"

Opciones:

- Unidad de Control
- Cache L1
- RAM

Pero también puedes diseñar algo mejor si encuentras una interacción más interesante.

La respuesta NO debe ser obvia solamente porque el jugador está parado frente al componente.

---

# PROPONER ACTIVIDADES MÁS JUGABLES

No limitarse obligatoriamente a preguntas de selección múltiple si existe una alternativa sencilla que sea mejor.

Puedes proponer actividades como:

- elegir entre opciones
- ordenar 2–4 pasos
- seleccionar el componente adecuado para una situación
- identificar qué ocurre primero
- decidir dónde buscar un dato
- relacionar dos componentes
- seleccionar qué componente interviene en una situación

Pero siempre mantenerlas simples.

NO crear:

- simuladores complejos
- CPU real
- emuladores
- sistemas de instrucciones reales
- sistemas de memoria reales
- mecánicas complicadas

Debe sentirse como un pequeño juego educativo.

---

# EJEMPLO DE EXPERIENCIA DESEADA

El jugador llega a la Unidad de Control.

Ve:

[E] Examinar

Abre:

UNIDAD DE CONTROL

"La Unidad de Control coordina la ejecución de las instrucciones."

Luego:

ACTIVIDAD

"La CPU debe ejecutar una instrucción. ¿Qué debería hacer primero?"

Opciones razonables.

El jugador toma una decisión.

Después:

✓ UNIDAD DE CONTROL COMPRENDIDA

"Ahora sabes que la Unidad de Control coordina los pasos necesarios para ejecutar una instrucción."

Esto debe enseñar algo.

---

# CAMBIO 6 — CONECTAR LOS COMPONENTES

Quiero que las actividades ayuden progresivamente a comprender la arquitectura completa.

No quiero que sean 8 preguntas independientes.

La progresión educativa debería construir una idea.

Por ejemplo:

ALU
→ qué hace con los datos

REGISTROS
→ dónde mantiene temporalmente datos inmediatos

UNIDAD DE CONTROL
→ cómo coordina la ejecución

CACHE
→ cómo ayuda a obtener datos rápidamente

RAM
→ dónde se mantienen programas y datos en uso

Y al final:

¿Cómo se relacionan?

---

# ACTIVIDAD FINAL

Revisar la actividad final actual.

Debe funcionar como una pequeña reconstrucción del flujo aprendido.

No debe sentirse como un examen.

Debe ser una actividad corta y entretenida que conecte los conceptos.

Por ejemplo, presentar una situación:

"Un programa necesita ejecutar una operación."

Y hacer que el jugador identifique de forma sencilla:

- quién coordina
- dónde puede estar un dato rápidamente
- dónde se mantienen los datos en uso
- quién realiza la operación

La actividad final debe reforzar:

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
MEMORIA

Mantenerlo como una representación educativa simplificada, no como una implementación real de CPU.

---

# PROPUESTA DE MEJORA

Además de los cambios solicitados, inspecciona el estado actual del juego y piensa si existe UNA mejora adicional pequeña que aumente la experiencia educativa.

Puede ser, por ejemplo:

- una pequeña explicación contextual al completar un componente
- una transición más clara entre objetivos
- un mensaje que conecte el componente actual con el siguiente
- un contador sencillo de progreso como "3/8 componentes"
- una pequeña guía contextual
- una indicación de "lo aprendido"
- una interacción sencilla entre dos componentes

NO implementes automáticamente una mejora grande.

Primero determina cuál tendría más impacto con menor riesgo.

Si existe una mejora pequeña y claramente beneficiosa, puedes implementarla.

Si no es necesario, simplemente propónla en el informe final.

La prioridad siempre es:

ESTABILIDAD > CALIDAD EDUCATIVA > NUEVAS FUNCIONES.

---

# RESTRICCIONES

NO modificar innecesariamente:

- PlayerInteraction.cs
- EducationalInteractable.cs
- ObjectiveSystem.cs
- LocationZone.cs
- FinalActivity.cs
- Cinemachine
- cámara
- jugador
- MovementInput
- CharacterSkinController
- Input System

Si alguna modificación de código es estrictamente necesaria para mejorar GameHUD o las actividades:

primero analizarla y modificar solamente lo necesario.

NO crear:

- otro Canvas
- otro GameHUD
- otro sistema de actividades
- otro sistema de ubicación
- otro sistema de objetivos
- otro sistema de interacción

Reutilizar la arquitectura existente.

---

# MUY IMPORTANTE — NO ROMPER LO ACTUAL

Antes de modificar:

verificar que el proyecto compila y que el flujo actual funciona.

Después de modificar:

volver a comprobar:

ALU
Cache L1
Cache L2
Cache L3
Registros
Unidad de Control
RAM1
RAM2

Comprobar especialmente:

- Label
- [E]
- Panel
- pregunta completa
- opciones
- respuesta correcta
- respuesta incorrecta
- recompensa
- progresión
- ubicación

---

# PLAN DE TRABAJO

## FASE 1 — AUDITORÍA

Inspeccionar:

- GameHUD.cs
- EducationalInteractable.cs
- ObjectiveSystem.cs
- LocationZone.cs
- FinalActivity.cs
- configuración actual de los 8 interactuables
- Zone_CPU
- Zone_RAM

Determinar exactamente dónde se construye el texto de la pregunta.

NO modificar hasta localizar el origen del problema.

---

## FASE 2 — CORRECCIÓN VISUAL

Corregir el layout de la pregunta:

- subir ligeramente
- reducir ligeramente la fuente
- permitir wrapping
- aumentar el espacio disponible
- comprobar varias líneas

Aplicarlo al sistema común para todas las actividades.

---

## FASE 3 — CORRECCIÓN DEL HUD

Separar visualmente:

OBJETIVO ACTUAL

de:

PISTA

Sin crear otra interfaz.

---

## FASE 4 — UBICACIÓN

Ajustar ligeramente Zone_RAM.

Solo modificar Zone_CPU si realmente necesita una pequeña ampliación.

---

## FASE 5 — REVISIÓN EDUCATIVA

Con el texto completo visible, revisar las preguntas.

Identificar preguntas:

- demasiado obvias
- poco coherentes
- que no enseñan el concepto
- que no relacionan el componente con la arquitectura

Mejorarlas manteniendo actividades sencillas.

---

## FASE 6 — MEJORA ADICIONAL

Determinar si existe una pequeña mejora de alto valor para la experiencia.

Implementarla solamente si es segura y coherente.

---

## FASE 7 — PRUEBAS

Probar como mínimo:

### ALU

Pregunta completa.

### Cache L1

Pregunta completa.

### Cache L2

Pregunta completa.

### Cache L3

Pregunta completa.

### Registros

Pregunta completa.

### Unidad de Control

Pregunta completa.

### RAM

Pregunta completa.

### Actividad final

Preguntas completas.

Además:

- comprobar respuestas correctas
- comprobar respuestas incorrectas
- comprobar botones
- comprobar recompensa
- comprobar progresión
- comprobar CPU
- comprobar RAM
- comprobar exploración

---

# CRITERIO DE ÉXITO

Al abrir cualquier actividad, debe ser posible leer la pregunta COMPLETA sin que el jugador tenga que adivinar qué dice.

Debe verse aproximadamente:

TÍTULO

↓

PREGUNTA COMPLETA

↓

OPCIONES

↓

CERRAR

Sin superposición.

La pregunta debe seguir siendo legible y no parecer comprimida.

El HUD superior izquierdo debe verse claramente separado:

OBJETIVO ACTUAL

Conoce la Cache L1.

PISTA

Busca la memoria caché más cercana al procesador.

Y la ubicación debe funcionar:

CPU

↓

RAM

↓

CPU

sin tener que colocarse prácticamente encima de los módulos RAM.

---

# INFORME FINAL OBLIGATORIO

Al terminar informar:

1. Qué causaba el recorte de las preguntas.
2. Qué cambios hiciste en el layout.
3. Qué posición/tamaño final tiene el texto de pregunta.
4. Si se utilizó Word Wrapping.
5. Si se modificó el tamaño general del panel.
6. Qué actividades fueron revisadas.
7. Qué preguntas fueron modificadas y por qué.
8. Qué respuestas correctas tiene cada actividad.
9. Qué mejora adicional propusiste o implementaste.
10. Qué cambio se hizo en el espaciado de Objetivo/Pista.
11. Qué cambio se hizo en Zone_RAM.
12. Si se modificó Zone_CPU.
13. Qué archivos fueron modificados.
14. Qué cambios se hicieron en Inspector.
15. Si la ALU sigue funcionando.
16. Si Cache L1/L2/L3 siguen funcionando.
17. Si Registros sigue funcionando.
18. Si Unidad de Control sigue funcionando.
19. Si RAM1/RAM2 siguen funcionando.
20. Si la progresión sigue funcionando.
21. Si la ubicación CPU/RAM sigue funcionando.
22. Errores nuevos.
23. Warnings nuevos.
24. Qué quedó pendiente.

NO continuar con funcionalidades nuevas después de completar este prompt.

Primero dejar esta mejora completamente estable.

La prioridad es:

1. Que todas las preguntas se vean completas.
2. Que las actividades tengan sentido educativo.
3. Que sean sencillas y entretenidas.
4. Que la guía sea clara.
5. Que la ubicación funcione naturalmente.
6. Mantener intacto todo lo que ya funciona.