# PROMPT 12 — EXTENSIÓN DEL SISTEMA EDUCATIVO A TODOS LOS COMPONENTES

Proyecto: AstroBit — Unity

---

# CONTEXTO

La interacción educativa de la ALU ya está completamente terminada y probada.

La ALU funciona correctamente de punta a punta:

```text
Jugador cerca del ALU
        ↓
ALU_Label visible
        ↓
[E] Examinar visible
        ↓
Presionar E
        ↓
Panel educativo visible
        ↓
Resolver actividad
        ↓
Actividad 12 + 7
        ↓
Respuesta correcta: 19
        ↓
Recompensa existente
        ↓
Continuar
        ↓
ObjectiveSystem avanza

La implementación fue corregida y probada visualmente en Game View.
Estado actual de la ALU
La ALU debe considerarse ahora como:
REFERENCIA FUNCIONAL DEL SISTEMA EDUCATIVO
No debe modificarse salvo que aparezca un error directamente causado por una modificación necesaria para reutilizar el sistema.
________________________________________
DIAGNÓSTICO FINAL DE LA ALU
Durante los prompts anteriores se corrigieron varios problemas:
Proximidad
EducationalInteractable utiliza correctamente:
proximityRadius = 10
para mostrar:
ALU_Label
La proximidad se calcula respecto al jugador.
No respecto a la cámara.
________________________________________
Interacción
PlayerInteraction utiliza:
interactionDistance = 4
y detecta el interactuable mediante proximidad al jugador.
La detección mediante la cámara fue eliminada porque no funcionaba correctamente con la cámara de tercera persona.
La protección contra obstáculos mediante línea de visión se mantiene.
________________________________________
Tecla E
La tecla:
E
funciona correctamente mediante:
Keyboard.current.eKey.wasPressedThisFrame
No existe otro sistema de interacción paralelo.
________________________________________
Panel educativo
El panel educativo existente ahora funciona correctamente.
Se descubrieron y corrigieron dos problemas en GameHUD.cs:
1.	EducationPanel se creaba con Transform en lugar de RectTransform. 
2.	hudCanvasTransform se capturaba antes de añadir el componente Canvas, provocando una referencia destruida/fake null. 
Actualmente:
EducationPanel
es un elemento UI correctamente parentado bajo:
HUDCanvas
y se renderiza correctamente.
________________________________________
Archivos modificados para corregir la ALU
Actualmente la implementación estable incluye:
Assets/Scripts/Interaction/PlayerInteraction.cs
Assets/Scripts/UI/GameHUD.cs
Además existe:
Assets/Scripts/Interaction/EducationalInteractable.cs
que ya funciona correctamente y NO debe ser reescrito innecesariamente.
________________________________________
OBJETIVO PRINCIPAL DEL PROMPT 12
Ahora quiero extender el mismo sistema educativo que funciona en la ALU a los demás componentes educativos del proyecto.
Como mínimo:
ALU
Cache
Registros
RAM
Unidad de Control
y cualquier otro componente educativo que ya exista en el proyecto y que esté destinado a explicar partes del computador.
________________________________________
COMPONENTES OBJETIVO
Como mínimo debes investigar y trabajar con:
1. ALU
2. Cache
3. Registros
4. RAM
5. Unidad de Control
También busca si existen otros componentes relacionados con:
CPU
Procesador
Memoria
Buses
Unidad de Control
Registros
ALU
Cache
RAM
ROM
Storage
Disco
No agregues componentes nuevos si no existen.
Primero identifica cuáles ya existen realmente en la escena/proyecto.
________________________________________
REGLA MÁS IMPORTANTE
NO MODIFICAR LA ALU
La ALU ya está funcionando.
No modificar:
•	su posición; 
•	escala; 
•	collider; 
•	EducationalInteractable; 
•	ALU_Label; 
•	proximityRadius; 
•	actividad; 
•	recompensa; 
•	textos; 
•	configuración; 
•	comportamiento; 
•	interacción. 
La ALU únicamente se utilizará como referencia para replicar el sistema.
________________________________________
FASE 1 — AUDITORÍA OBLIGATORIA
ANTES DE MODIFICAR CUALQUIER COSA, inspecciona el proyecto.
Busca:
•	GameObjects de Cache; 
•	GameObjects de Registros; 
•	GameObjects de RAM; 
•	GameObjects de Unidad de Control; 
•	otros componentes educativos; 
•	scripts asociados; 
•	Canvas; 
•	labels; 
•	colliders; 
•	EducationalInteractable; 
•	actividades existentes; 
•	referencias a GameHUD; 
•	referencias a ObjectiveSystem. 
No realices cambios durante esta fase.
Primero genera una tabla de diagnóstico similar a:
Componente	GameObject	Script	Collider	EducationalInteractable	Label	Actividad existente	Estado
ALU	...	...	...	Sí	Sí	12 + 7	FUNCIONANDO
Cache	...	...	...	...	...	...	...
Registros	...	...	...	...	...	...	...
RAM	...	...	...	...	...	...	...
Unidad de Control	...	...	...	...	...	...	...
La información debe salir del proyecto real.
NO inventes nombres.
________________________________________
FASE 2 — IDENTIFICAR EL PATRÓN DE LA ALU
Analiza exactamente cómo está configurada la ALU actualmente.
Determina:
1.	GameObject utilizado. 
2.	Collider utilizado. 
3.	EducationalInteractable. 
4.	valores de proximidad; 
5.	título; 
6.	subtítulo; 
7.	descripción; 
8.	texto del label; 
9.	actividad; 
10.	operandos; 
11.	operación; 
12.	respuesta correcta; 
13.	recompensa; 
14.	conexión con ObjectiveSystem. 
No cambies nada.
La finalidad es construir un patrón conceptual:
COMPONENTE EDUCATIVO
        ↓
EducationalInteractable
        ↓
Label de proximidad
        ↓
[E] Examinar
        ↓
GameHUD
        ↓
Actividad educativa
        ↓
Respuesta
        ↓
Recompensa
________________________________________
FASE 3 — NO DUPLICAR SISTEMAS
Todos los componentes deben utilizar el sistema existente.
NO crear:
•	otro PlayerInteraction; 
•	otro EducationalInteractable personalizado; 
•	otro GameHUD; 
•	otro Canvas; 
•	otro sistema de prompts; 
•	otro sistema de actividades; 
•	otro sistema de recompensas; 
•	otro sistema de input; 
•	otro ObjectiveSystem. 
La arquitectura debe continuar siendo:
PlayerInteraction
        ↓
IInteractable
        ↓
EducationalInteractable
        ↓
GameHUD
        ↓
actividad educativa
        ↓
recompensa
________________________________________
FASE 4 — CONFIGURAR CACHE
Si existe un objeto de Cache en la escena:
configúralo utilizando el sistema existente.
Debe funcionar:
Jugador se acerca a Cache
        ↓
Cache_Label visible
        ↓
[E] Examinar
        ↓
E
        ↓
Panel educativo de Cache
La actividad debe tratar sobre Cache.
________________________________________
ACTIVIDAD DE CACHE
Antes de crear una pregunta nueva:
BUSCA si ya existe una actividad configurada para Cache.
Si existe:
REUTILIZARLA
Si NO existe, crea una actividad sencilla utilizando el mismo sistema de preguntas existente.
La pregunta debe ser apropiada para un estudiante que está aprendiendo arquitectura de computadores.
Por ejemplo, conceptualmente:
¿Qué función cumple la memoria caché?
Pero NO copies esta pregunta obligatoriamente.
Primero verifica qué sistema de actividad utiliza el proyecto y qué contenido educativo ya existe.
La respuesta correcta debe estar claramente definida.
________________________________________
RECOMPENSA DE CACHE
Utilizar el sistema de recompensa existente.
El texto puede ser equivalente a:
✓ CACHE ANALIZADA
si el sistema actual permite personalizar el mensaje.
No crear un nuevo sistema de recompensa.
________________________________________
FASE 5 — CONFIGURAR REGISTROS
Si existe un objeto de Registros:
configúralo utilizando el mismo sistema.
Flujo esperado:
Jugador cerca
        ↓
Registros_Label
        ↓
[E] Examinar
        ↓
E
        ↓
Panel educativo
        ↓
Actividad sobre Registros
        ↓
Respuesta correcta
        ↓
Recompensa
No reutilices obligatoriamente la pregunta de ALU.
La actividad debe ser coherente con Registros.
________________________________________
ACTIVIDAD DE REGISTROS
Busca primero si ya existe.
Si no existe, crea una actividad sencilla y educativa relacionada con:
•	función de los registros; 
•	almacenamiento temporal; 
•	datos utilizados por la CPU; 
•	relación con la ejecución de instrucciones. 
No hagas preguntas excesivamente complejas.
________________________________________
RECOMPENSA DE REGISTROS
Utilizar el sistema existente.
Puede utilizar un mensaje equivalente a:
✓ REGISTROS ANALIZADOS
si el sistema actual permite personalizarlo.
________________________________________
FASE 6 — CONFIGURAR RAM
Si existe un objeto de RAM:
configúralo con el mismo sistema.
Flujo esperado:
Jugador cerca de RAM
        ↓
RAM_Label
        ↓
[E] Examinar
        ↓
E
        ↓
Panel educativo
        ↓
Actividad RAM
        ↓
Respuesta correcta
        ↓
Recompensa
La actividad debe explicar de forma sencilla el propósito de la RAM.
________________________________________
ACTIVIDAD DE RAM
Buscar primero si existe una actividad.
Si no existe, crear una actividad sencilla sobre:
•	memoria RAM; 
•	almacenamiento temporal; 
•	relación con programas en ejecución; 
•	diferencia básica entre RAM y almacenamiento permanente. 
No hacer una actividad excesivamente compleja.
________________________________________
RECOMPENSA DE RAM
Utilizar el sistema existente.
Mensaje equivalente:
✓ RAM ANALIZADA
si es compatible con el sistema actual.
________________________________________
FASE 7 — CONFIGURAR UNIDAD DE CONTROL
Si existe un objeto de Unidad de Control:
configurarlo utilizando el sistema existente.
Flujo esperado:
Jugador cerca
        ↓
UnidadControl_Label
        ↓
[E] Examinar
        ↓
E
        ↓
Panel educativo
        ↓
Actividad sobre Unidad de Control
        ↓
Respuesta correcta
        ↓
Recompensa
La actividad debe explicar de forma sencilla la función de la Unidad de Control.
________________________________________
ACTIVIDAD DE UNIDAD DE CONTROL
Buscar primero si ya existe.
Si no existe, crear una actividad sencilla relacionada con:
•	coordinación de instrucciones; 
•	señales de control; 
•	coordinación de componentes; 
•	ciclo de ejecución. 
Mantenerla apropiada para el nivel educativo del proyecto.
________________________________________
RECOMPENSA DE UNIDAD DE CONTROL
Utilizar el sistema existente.
Mensaje equivalente:
✓ UNIDAD DE CONTROL ANALIZADA
si el sistema actual permite personalizarlo.
________________________________________
FASE 8 — OTROS COMPONENTES
Después de:
Cache
Registros
RAM
Unidad de Control
revisa qué otros componentes educativos ya existen.
Por ejemplo:
Buses
Storage
Disco
ROM
Memoria
CPU
etc.
NO implementar automáticamente todos sin revisar primero.
Para cada uno determina:
¿Existe?
¿Es educativo?
¿Tiene collider?
¿Tiene interactuable?
¿Tiene actividad?
¿Debe ser interactuable?
Si claramente forma parte del recorrido educativo, intégralo.
________________________________________
FASE 9 — LABELS
Cada componente debe tener su propio label.
Ejemplos:
CACHE
Memoria Caché
REGISTROS
Registros
RAM
Memoria RAM
UNIDAD DE CONTROL
Unidad de Control
Pero utiliza el sistema de label existente.
NO crear otro sistema de Billboard.
NO crear otro Canvas mundial.
NO modificar el Billboard que ya funciona para ALU.
________________________________________
MUY IMPORTANTE SOBRE PROXIMIDAD
El sistema debe conservar la separación conceptual:
proximityRadius
para:
Label visible
y:
interactionDistance
para:
[E] visible
No mezclar ambos.
La ALU utiliza:
proximityRadius = 10
interactionDistance = 4
Puedes utilizar valores equivalentes para los demás componentes, salvo que por la geometría del objeto sea necesario ajustar algo.
Si ajustas un valor, justifica por qué.
No establecer arbitrariamente:
20
50
100
________________________________________
FASE 10 — COLLIDERS
Cada componente interactuable debe tener una forma razonable de detección.
Comprueba:
Cache
Registros
RAM
Unidad de Control
y los demás componentes.
Si ya tienen Collider:
REUTILIZARLO
No reemplazarlo sin necesidad.
Si no tienen Collider y es estrictamente necesario para que EducationalInteractable pueda ser detectado:
agregar el Collider mínimo necesario.
No cambiar la geometría visual del objeto.
No modificar su escala.
________________________________________
FASE 11 — ACTIVIDADES
Todas las actividades deben utilizar:
GameHUD
existente.
No crear nuevos paneles.
No crear nuevos Canvas.
No crear otro sistema de preguntas.
El flujo debe seguir siendo:
EducationalInteractable
        ↓
GameHUD.ShowEducationalPanel()
        ↓
Resolver actividad
        ↓
GameHUD.ShowActivityPanel()
________________________________________
FASE 12 — RECOMPENSAS
Todas las recompensas deben utilizar el sistema existente.
No crear:
RewardManager2
ni otro sistema paralelo.
Mantener:
ObjectiveSystem
existente.
________________________________________
FASE 13 — NO ROMPER LA ALU
Después de configurar los nuevos componentes:
volver a probar la ALU.
Debe seguir funcionando:
ALU
↓
[E]
↓
Panel
↓
12 + 7
↓
19
↓
Recompensa
Si algo nuevo rompe la ALU:
DETENTE.
No continuar agregando componentes.
Primero solucionar la regresión.
________________________________________
FASE 14 — PRUEBA INDIVIDUAL OBLIGATORIA
Cada componente debe probarse individualmente.
ALU
Label
[E]
Panel
Actividad
Respuesta
Recompensa
Cache
Label
[E]
Panel
Actividad
Respuesta
Recompensa
Registros
Label
[E]
Panel
Actividad
Respuesta
Recompensa
RAM
Label
[E]
Panel
Actividad
Respuesta
Recompensa
Unidad de Control
Label
[E]
Panel
Actividad
Respuesta
Recompensa
________________________________________
FASE 15 — PRUEBA DE ÁNGULOS
Para cada componente nuevo:
probar interacción desde:
•	frente; 
•	izquierda; 
•	derecha; 
•	diagonal; 
•	atrás. 
La cámara debe permanecer en tercera persona normal.
NO mover la cámara manualmente para facilitar la interacción.
________________________________________
FASE 16 — PRUEBA DE DISTANCIA
Para cada componente:
Lejos
Esperado:
Label oculto
[E] oculto
Dentro de proximidad
Esperado:
Label visible
[E] posiblemente oculto
Dentro de interacción
Esperado:
Label visible
[E] visible
Esto confirma que:
proximityRadius
y:
interactionDistance
siguen separados.
________________________________________
FASE 17 — PRUEBA DE ACTIVIDAD
Para cada actividad:
1.	Abrir panel. 
2.	Verificar título. 
3.	Verificar descripción. 
4.	Presionar Resolver actividad. 
5.	Verificar pregunta. 
6.	Introducir respuesta incorrecta. 
7.	Confirmar mensaje incorrecto. 
8.	Introducir respuesta correcta. 
9.	Confirmar recompensa. 
10.	Presionar Continuar. 
No asumir que porque la ALU funciona, los demás funcionan automáticamente.
Cada uno debe ser probado.
________________________________________
FASE 18 — PRUEBA DE CIERRE
Para cada componente:
[E]
↓
Panel
↓
Cerrar
Después:
Alejarse
↓
Acercarse
Debe volver a aparecer:
Label
[E]
si el componente no fue completado.
________________________________________
FASE 19 — COMPONENTES COMPLETADOS
Cuando una actividad se completa correctamente, verifica el comportamiento existente del sistema.
Si el diseño actual establece que un componente completado:
CanInteract = false
entonces mantener ese comportamiento.
No modificarlo simplemente para que pueda repetirse.
La ALU ya demuestra el comportamiento esperado:
ALU completada
↓
No interactuable
Mantener la misma lógica salvo que exista una razón explícita en el proyecto para hacerlo diferente.
________________________________________
FASE 20 — NO CREAR CONTENIDO INNECESARIO
No inventar una gran cantidad de contenido educativo.
La prioridad es:
FUNCIONALIDAD
antes que:
CANTIDAD
Primero conseguir que todos los componentes funcionen con el mismo flujo.
________________________________________
FASE 21 — SI FALTA INFORMACIÓN
Si un componente no tiene suficiente información para crear correctamente su actividad:
NO inventes arbitrariamente.
Indica:
Componente:
Qué falta:
Qué sería necesario:
y continúa con los componentes que sí puedan implementarse correctamente.
________________________________________
FASE 22 — SI ENCUENTRAS UN PROBLEMA DE UI
Si alguno de los nuevos paneles no aparece:
NO crear otro Canvas.
NO crear otro HUD.
NO modificar Cinemachine.
NO modificar PlayerInteraction.
Comparar su estructura con la ALU y con el EducationPanel corregido.
Comprobar:
RectTransform
Parent
Canvas
anchors
sizeDelta
activeSelf
La ALU debe utilizarse como referencia.
________________________________________
FASE 23 — SI ENCUENTRAS UN PROBLEMA DE INTERACCIÓN
Si [E] no aparece para algún componente:
NO aumentar arbitrariamente interactionDistance.
Investigar:
Collider
IInteractable
EducationalInteractable
CanInteract
currentTarget
y determinar la causa.
________________________________________
FASE 24 — ARCHIVOS
Antes de modificar archivos, identifica cuáles son necesarios.
Preferir:
Configuración existente
sobre:
Nuevo código
Si es posible configurar los componentes desde el Inspector:
hacerlo.
No crear scripts nuevos innecesariamente.
________________________________________
FASE 25 — RESTRICCIONES ABSOLUTAS
NO modificar innecesariamente:
•	PlayerInteraction.cs 
•	GameHUD.cs 
•	EducationalInteractable.cs 
•	Cinemachine 
•	cámara 
•	jugador 
•	Input System 
•	ProjectSettings 
•	ObjectiveSystem 
•	sistema de recompensas 
•	ALU 
•	ALU_Label 
•	Billboard 
•	proximityRadius 
•	interactionDistance 
Si necesitas modificar uno de estos archivos para soportar correctamente los nuevos componentes:
primero diagnostica y explica por qué es necesario.
No hagas cambios arbitrarios.
________________________________________
NO CREAR
NO crear:
•	otro Canvas; 
•	otro HUD; 
•	otro GameHUD; 
•	otro PlayerInteraction; 
•	otro sistema de interacción; 
•	otro Input System; 
•	otro sistema de actividades; 
•	otro sistema de recompensas; 
•	otro ObjectiveSystem; 
•	managers nuevos; 
•	sistemas paralelos. 
________________________________________
FASE 26 — COMPILACIÓN
Después de realizar los cambios:
compilar el proyecto.
Verificar:
0 errores
y revisar warnings nuevos.
Si aparece un error:
detenerse y corregirlo antes de continuar.
________________________________________
FASE 27 — INFORME FINAL
Al terminar, entrega un informe completo.
1. Auditoría
Tabla:
Componente	GameObject	Collider	EducationalInteractable	Label	Actividad	Estado
________________________________________
2. Componentes implementados
Indicar:
ALU
Cache
Registros
RAM
Unidad de Control
Otros
________________________________________
3. Actividades
Para cada componente:
Componente:
Pregunta:
Respuesta correcta:
________________________________________
4. Recompensas
Para cada componente:
Componente:
Recompensa:
________________________________________
5. Pruebas
Indicar:
ALU              ✓
Cache            ✓
Registros        ✓
RAM              ✓
Unidad de Control ✓
y explicar cualquier fallo.
________________________________________
6. Pruebas de distancia
Indicar que se comprobó:
Lejos
↓
Label oculto
[E] oculto
Proximidad
↓
Label visible
Interacción
↓
[E] visible
________________________________________
7. Pruebas de ángulos
Indicar para cada componente:
Frente      ✓/✗
Izquierda   ✓/✗
Derecha     ✓/✗
Diagonal    ✓/✗
Atrás       ✓/✗
________________________________________
8. Prueba de actividades
Para cada componente:
Panel visible       ✓/✗
Resolver actividad  ✓/✗
Pregunta visible    ✓/✗
Incorrecta          ✓/✗
Correcta            ✓/✗
Recompensa          ✓/✗
Continuar           ✓/✗
________________________________________
9. Regresión de ALU
Confirmar explícitamente:
ALU sigue funcionando correctamente: SÍ/NO
________________________________________
10. Archivos modificados
Lista exacta:
Assets/...
Assets/...
No decir simplemente "varios archivos".
________________________________________
11. Errores
Indicar:
Errores nuevos:
Warnings nuevos:
________________________________________
OBJETIVO FINAL
Al finalizar quiero que AstroBit tenga un sistema educativo consistente para los componentes principales:
                  ASTROBIT
                     │
        ┌────────────┼────────────┐
        │            │            │
       CPU          RAM         STORAGE
        │
   ┌────┼────┬──────────┐
   │    │    │          │
  ALU Cache Registros Unidad
                   de Control
Y que cada componente educativo siga el mismo patrón:
Jugador se acerca
        ↓
Label aparece
        ↓
[E] Examinar
        ↓
Presionar E
        ↓
Panel educativo
        ↓
Resolver actividad
        ↓
Pregunta
        ↓
Respuesta
        ↓
Recompensa
        ↓
Objetivo actualizado
La ALU ya es la implementación de referencia.
No rehacer la ALU.
No crear sistemas paralelos.
Extender el sistema existente de forma consistente y mínima.
No continuar todavía con contenido adicional que no pertenezca a estos componentes.
La prioridad es que:
ALU
Cache
Registros
RAM
Unidad de Control
queden funcionales, visualmente correctos y probados de punta a punta.

