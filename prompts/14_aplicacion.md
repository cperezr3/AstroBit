PROMPT 14 — SISTEMA DE JUGABILIDAD EDUCATIVA, PROGRESIÓN, GUÍA Y UBICACIÓN
Proyecto: AstroBit — Unity
________________________________________
1. CONTEXTO ACTUAL
AstroBit ya tiene funcionando correctamente un sistema de interacción educativa.
Actualmente existen estos componentes educativos en la escena:
•	ALU 
•	Cache L1 
•	Cache L2 
•	Cache L3 
•	Registros 
•	Unidad de Control 
•	RAM1 
•	RAM2 
Cada componente ya tiene:
•	Collider 
•	EducationalInteractable 
•	etiqueta flotante 
•	interacción mediante [E] 
•	panel educativo 
•	actividad 
•	respuesta 
•	recompensa 
•	integración con ObjectiveSystem 
El sistema actual fue probado y funciona correctamente.
La cadena actual es:
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
ESTE SISTEMA ES LA BASE DEL PROYECTO Y NO DEBE SER DESTRUIDO.
________________________________________
2. OBJETIVO DE ESTE PROMPT
Ahora AstroBit debe evolucionar desde una colección de interacciones independientes hacia una experiencia educativa guiada.
Actualmente el jugador puede completar los componentes de manera independiente.
Quiero que ahora exista una experiencia con:
EXPLORACIÓN
    ↓
UBICACIÓN
    ↓
OBJETIVO ACTUAL
    ↓
GUÍA
    ↓
COMPONENTE
    ↓
EXPLICACIÓN
    ↓
ACTIVIDAD EDUCATIVA
    ↓
RECOMPENSA
    ↓
SIGUIENTE OBJETIVO
El jugador debe sentir que está aprendiendo progresivamente cómo funciona una computadora mientras recorre físicamente su arquitectura.
No quiero un juego complejo.
Quiero una experiencia:
•	sencilla 
•	intuitiva 
•	educativa 
•	guiada 
•	visualmente clara 
•	fácil de explicar 
•	fácil de demostrar 
•	basada en exploración 
•	basada en interacciones pequeñas 
________________________________________
3. REGLA FUNDAMENTAL
Antes de modificar cualquier cosa:
LEE Y ANALIZA prompts/013_estadoactual.md.
Ese archivo contiene la auditoría del estado actual del proyecto.
No asumas que la información anterior sigue siendo cierta sin comprobar el código y la escena.
Debes verificar especialmente:
Assets/Scripts/Interaction/PlayerInteraction.cs
Assets/Scripts/Interaction/EducationalInteractable.cs
Assets/Scripts/UI/GameHUD.cs
Assets/Scripts/Gameplay/ObjectiveSystem.cs
y cualquier otro script directamente relacionado con:
•	interacción 
•	objetivos 
•	actividades 
•	recompensas 
•	HUD 
•	movimiento 
•	ubicación 
________________________________________
4. REGLA DE SEGURIDAD: NO ROMPER LO EXISTENTE
Antes de implementar nuevas funcionalidades, comprueba que actualmente funcionan:
ALU
ALU_Label
    ↓
[E]
    ↓
Panel educativo
    ↓
Actividad
    ↓
Respuesta
    ↓
Recompensa
La ALU es la referencia funcional del sistema.
No modificar innecesariamente:
•	PlayerInteraction.cs 
•	EducationalInteractable.cs 
•	GameHUD.cs 
•	ObjectiveSystem.cs 
•	Cinemachine 
•	cámara 
•	jugador 
•	MovementInput 
•	CharacterSkinController 
•	Input System 
•	interactionDistance 
•	proximityRadius 
•	ALU 
•	ALU_Label 
Si alguna modificación es estrictamente necesaria, debe ser:
1.	identificada, 
2.	justificada, 
3.	mínima, 
4.	compatible con el sistema existente. 
NO hacer refactorizaciones generales.
________________________________________
5. NO CREAR SISTEMAS PARALELOS
Ya existen:
•	GameHUD 
•	ObjectiveSystem 
•	PlayerInteraction 
•	EducationalInteractable 
Reutilizarlos.
NO crear:
•	otro HUD 
•	otro Canvas 
•	otro sistema de interacción 
•	otro sistema de objetivos 
•	otro sistema de recompensas 
•	otro sistema paralelo de actividades 
Si se necesita ampliar alguna de estas clases, hacerlo de manera mínima.
________________________________________
6. NUEVA EXPERIENCIA DE JUEGO
La experiencia debe evolucionar hacia algo parecido a:
INICIO
  ↓
INTRODUCCIÓN
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
ACTIVIDAD FINAL
  ↓
COMPRENSIÓN DEL FLUJO
Esta es una propuesta inicial, no una orden rígida.
Analiza las relaciones educativas entre los componentes existentes y determina si existe una secuencia ligeramente mejor.
Pero:
NO inventes componentes que no existen.
Actualmente no existen:
•	Storage 
•	Disco 
•	ROM 
•	Bus 
•	otros componentes adicionales 
No crearlos en este prompt.
________________________________________
7. EXPLORACIÓN LIBRE
El jugador debe poder recorrer el mapa libremente.
NO quiero un juego donde el jugador esté físicamente bloqueado hasta completar una tarea.
El jugador puede visitar RAM mientras está aprendiendo ALU.
El jugador puede volver a CPU.
El jugador puede explorar.
Sin embargo:
la progresión educativa sí debe seguir un orden.
Por ejemplo:
Objetivo actual:
Conoce la ALU.
Si el jugador visita RAM:
UBICACIÓN: RAM
pero:
OBJETIVO:
Conoce la ALU.
sigue siendo el objetivo actual.
Si interactúa con RAM antes de tiempo, analiza la mejor solución:
Preferencia
Permitir la exploración, pero evitar que esa interacción complete el objetivo actual.
Puede mostrarse un mensaje sencillo:
"Esta sección la estudiaremos más adelante."
o permitir consultar el componente sin avanzar la progresión.
No bloquear físicamente al jugador.
________________________________________
8. SISTEMA DE PROGRESIÓN REAL
Actualmente ObjectiveSystem funciona principalmente como texto informativo.
Quiero convertirlo en una progresión educativa real.
La progresión debe avanzar únicamente cuando el jugador:
1.	llega al componente correspondiente, 
2.	abre su panel, 
3.	completa correctamente la actividad. 
No debe avanzar simplemente por:
•	acercarse, 
•	ver la etiqueta, 
•	presionar E, 
•	abrir el panel. 
Debe completarse la actividad correctamente.
________________________________________
9. PROPUESTA DE PROGRESIÓN
Utiliza los componentes realmente existentes.
Una posible secuencia es:
1. Introducción
       ↓
2. Conoce la CPU
       ↓
3. ALU
       ↓
4. Registros
       ↓
5. Unidad de Control
       ↓
6. Cache L1
       ↓
7. Cache L2
       ↓
8. Cache L3
       ↓
9. RAM
       ↓
10. Comprende cómo trabajan juntas
Puedes reorganizar ligeramente la secuencia si la arquitectura del mapa o la lógica educativa lo justifican.
No crear componentes nuevos para completar la secuencia.
________________________________________
10. OBJETIVO ACTUAL
El jugador debe saber siempre:
¿Qué tengo que hacer ahora?
Por ejemplo:
OBJETIVO ACTUAL

Conoce la ALU.

PISTA

Busca la etiqueta ALU dentro de la CPU.
Después:
OBJETIVO COMPLETADO ✓

Has aprendido que la ALU realiza operaciones
sobre los datos.
Después:
NUEVO OBJETIVO

Conoce los Registros.

PISTA

Busca dónde la CPU mantiene temporalmente
los datos que necesita rápidamente.
La guía debe ser corta.
No mostrar párrafos enormes durante la exploración.
________________________________________
11. SISTEMA DE GUÍA
Quiero una guía educativa sencilla.
Debe responder:
¿Qué debo hacer?
¿Dónde debo ir?
¿Qué estoy aprendiendo?
No quiero un marcador gigante que haga que el jugador simplemente siga una flecha.
Preferencia:
OBJETIVO
Conoce la ALU.

PISTA
Busca el componente marcado como ALU.
La exploración debe seguir siendo parte del juego.
________________________________________
12. SISTEMA DE UBICACIÓN
Actualmente no existe un sistema de habitaciones.
Crear un sistema sencillo y robusto de zonas.
Preferiblemente utilizando:
•	Collider con Is Trigger 
•	un componente sencillo como LocationZone 
•	o una solución equivalente 
El sistema debe determinar dónde está el jugador.
Debe existir una única ubicación actual.
________________________________________
13. UBICACIONES
No inventar habitaciones que no existan físicamente.
Primero inspeccionar la escena.
Determinar qué zonas físicas pueden representar:
•	CPU 
•	RAM 
•	Cache 
•	otras zonas realmente existentes 
Si CPU y Cache están dentro de una misma habitación física, no inventar una separación artificial.
La ubicación debe representar la estructura real del mapa.
________________________________________
14. INDICADOR SUPERIOR DE UBICACIÓN
En la parte superior del HUD debe aparecer algo como:
CPU
Cuando el jugador entra a la zona RAM:
RAM
Cuando cambia de zona:
CACHE
etc.
Debe existir una única ubicación activa.
No deben aparecer múltiples nombres simultáneamente.
________________________________________
15. MENSAJE CONTEXTUAL
Opcionalmente, debajo del nombre puede aparecer una descripción corta.
Ejemplo:
CPU
Centro de procesamiento
o:
RAM
Memoria principal
No hacerlo excesivamente grande.
Debe integrarse visualmente con GameHUD.
NO crear otro Canvas.
________________________________________
16. TRANSICIÓN DE UBICACIÓN
Cuando el jugador entre a una zona nueva puede aparecer brevemente:
CPU
Centro de procesamiento
y después permanecer solamente:
CPU
No es obligatorio crear una animación compleja.
La prioridad es:
claridad > efectos visuales.
________________________________________
17. ACTIVIDADES: ELIMINAR EL ENFOQUE DE SUMAS Y RESTAS
Las operaciones:
12 + 7
8 + 5
14 - 6
20 - 9
fueron únicamente pruebas técnicas.
Ya no deben ser la actividad principal.
Quiero actividades conceptuales relacionadas directamente con cada componente.
NO necesitamos simulaciones reales de CPU.
NO necesitamos emulación.
NO necesitamos ejecutar instrucciones reales.
Necesitamos representaciones educativas sencillas.
________________________________________
18. NUEVO TIPO DE ACTIVIDADES
El sistema debería poder representar preguntas como:
Pregunta

¿Dónde se realiza una operación sobre datos?

[ ALU ]
[ RAM ]
[ CACHE ]

Respuesta:
ALU
o:
Pregunta

La CPU necesita guardar temporalmente
un dato que está utilizando.

¿Dónde puede almacenarlo rápidamente?

[ REGISTRO ]
[ DISCO ]
[ MONITOR ]

Respuesta:
REGISTRO
o:
Pregunta

La CPU necesita ejecutar una instrucción.

¿Qué componente coordina lo que debe hacer?

[ ALU ]
[ UNIDAD DE CONTROL ]
[ RAM ]

Respuesta:
UNIDAD DE CONTROL
________________________________________
19. ACTIVIDADES POR COMPONENTE
Diseñar actividades sencillas y conceptualmente correctas.
ALU
Enseñar:
•	realiza operaciones 
•	trabaja sobre datos 
•	puede realizar operaciones aritméticas y lógicas 
Ejemplo:
La CPU necesita realizar una operación
sobre dos datos.

¿Qué componente realiza esta operación?

[ ALU ]
[ RAM ]
[ CACHE ]
________________________________________
REGISTROS
Enseñar:
•	almacenamiento temporal 
•	datos utilizados rápidamente por la CPU 
•	están dentro de la CPU 
Ejemplo:
La CPU necesita guardar temporalmente
un dato que está utilizando.

¿Dónde puede hacerlo?

[ REGISTROS ]
[ RAM ]
[ DISCO ]
________________________________________
UNIDAD DE CONTROL
Enseñar:
•	coordina la ejecución 
•	dirige el funcionamiento de los componentes 
Ejemplo:
La CPU debe ejecutar una instrucción.

¿Qué componente coordina el proceso?

[ UNIDAD DE CONTROL ]
[ ALU ]
[ RAM ]
________________________________________
CACHE L1
Enseñar:
•	memoria muy rápida 
•	cercana al procesamiento 
•	almacena datos utilizados frecuentemente 
________________________________________
CACHE L2
Enseñar:
•	segundo nivel de caché 
•	mayor capacidad que L1 
•	normalmente algo más lenta que L1 
________________________________________
CACHE L3
Enseñar:
•	nivel de caché de mayor capacidad 
•	ayuda a mantener datos disponibles para el procesador 
________________________________________
RAM
Enseñar:
•	memoria principal 
•	mantiene temporalmente programas y datos en uso 
________________________________________
20. EXACTITUD EDUCATIVA
Mantener los conceptos básicos correctos.
Como mínimo:
•	ALU realiza operaciones. 
•	Registros almacenan datos temporalmente dentro de la CPU. 
•	Unidad de Control coordina la ejecución de instrucciones. 
•	Cache proporciona almacenamiento rápido cercano al procesador. 
•	L1, L2 y L3 representan diferentes niveles de caché. 
•	RAM mantiene temporalmente programas y datos en uso. 
Simplificar está permitido.
Inventar conceptos incorrectos no.
Si una mecánica del juego es solamente una representación educativa, dejarlo claro en el texto.
________________________________________
21. COMPATIBILIDAD CON EL SISTEMA ACTUAL
Antes de cambiar las actividades:
Inspeccionar exactamente cómo funciona actualmente:
EducationalInteractable
GameHUD.ShowEducationalPanel()
GameHUD.ShowActivityPanel()
SubmitAnswer()
ShowReward()
ObjectiveSystem
Determinar si el sistema actual está acoplado exclusivamente a:
operandA
operandB
Operation
Si es así, implementar la modificación mínima necesaria para permitir preguntas conceptuales.
Preferir una arquitectura que pueda soportar:
Actividad
├── Pregunta
├── Opciones
├── Respuesta correcta
├── Feedback
└── Recompensa
sin crear un segundo sistema paralelo.
________________________________________
22. COMPATIBILIDAD CON ACTIVIDADES EXISTENTES
No eliminar las actividades matemáticas hasta que el nuevo sistema conceptual esté funcionando.
Mantener compatibilidad durante la transición.
Una vez comprobado que el sistema conceptual funciona correctamente, configurar los 8 componentes para utilizar actividades educativas conceptuales.
La ALU debe continuar funcionando.
________________________________________
23. RECOMPENSAS
Mantener el sistema de recompensas existente.
Las recompensas pueden evolucionar de:
✓ ALU ANALIZADA
a:
✓ ALU COMPRENDIDA
pero esto no es prioritario.
Primero garantizar:
Actividad correcta
        ↓
Recompensa
        ↓
Objetivo completado
        ↓
Siguiente objetivo
________________________________________
24. ACTIVIDAD FINAL
Después de estudiar los componentes principales, crear una actividad final sencilla.
Debe comprobar si el jugador entiende la relación general.
Ejemplo conceptual:
Una instrucción necesita ejecutarse.

¿Qué componente coordina su ejecución?

[ Unidad de Control ]
Después:
¿Dónde puede mantenerse rápidamente
un dato utilizado frecuentemente?

[ Cache ]
Después:
¿Dónde se realizan operaciones sobre datos?

[ ALU ]
Después:
¿Dónde se mantienen temporalmente los datos
y programas que están en uso?

[ RAM ]
No convertirlo en un examen largo.
Debe servir como conclusión educativa.
________________________________________
25. CONCEPTO FINAL QUE DEBE ENTENDER EL JUGADOR
La experiencia debe terminar dejando una idea sencilla:
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
Esto es una representación educativa simplificada, no una simulación literal de una CPU real.
________________________________________
26. IMPLEMENTACIÓN POR FASES
No implementar todo de manera descontrolada.
Trabajar en este orden:
FASE 1 — AUDITORÍA
Inspeccionar:
•	GameHUD.cs 
•	EducationalInteractable.cs 
•	PlayerInteraction.cs 
•	ObjectiveSystem.cs 
•	escena actual 
•	scripts relacionados 
No modificar nada todavía.
________________________________________
FASE 2 — DISEÑO
Determinar:
•	secuencia educativa 
•	objetivos 
•	componentes asociados 
•	conceptos 
•	actividades 
•	ubicación física de las zonas 
Si existe alguna decisión importante, utilizar la estructura actual del proyecto para tomarla.
No inventar elementos inexistentes.
________________________________________
FASE 3 — SISTEMA DE PROGRESIÓN
Implementar la progresión real.
Debe existir:
Objetivo actual
      ↓
Componente esperado
      ↓
Actividad completada
      ↓
Siguiente objetivo
Explorar libremente no debe alterar el progreso.
________________________________________
FASE 4 — ACTIVIDADES CONCEPTUALES
Implementar el soporte necesario para preguntas conceptuales.
No crear otro sistema de actividades.
________________________________________
FASE 5 — UBICACIÓN
Implementar zonas sencillas.
Verificar:
CPU → RAM → CPU
y comprobar que el HUD cambia correctamente.
________________________________________
FASE 6 — GUÍA
Integrar:
•	objetivo actual 
•	pista 
•	ubicación 
en GameHUD.
No crear otro HUD.
________________________________________
FASE 7 — PRUEBA E2E
Probar el flujo completo.
________________________________________
27. PRUEBAS OBLIGATORIAS
PRUEBA 1 — ALU
Verificar:
Label
↓
[E]
↓
Panel
↓
Actividad conceptual
↓
Respuesta correcta
↓
Recompensa
↓
Siguiente objetivo
________________________________________
PRUEBA 2 — REGISTROS
Verificar el mismo flujo.
________________________________________
PRUEBA 3 — UNIDAD DE CONTROL
Verificar el mismo flujo.
________________________________________
PRUEBA 4 — CACHE
Verificar al menos un nivel.
Después comprobar que la progresión continúa correctamente por los niveles restantes.
________________________________________
PRUEBA 5 — RAM
Verificar interacción y actividad.
________________________________________
PRUEBA 6 — UBICACIÓN
Recorrer físicamente las zonas existentes.
Comprobar:
CPU
↓
RAM
↓
CPU
y cualquier otra zona que realmente exista.
________________________________________
PRUEBA 7 — PROGRESIÓN
Completar los objetivos en orden.
Verificar que cada actividad completada produce el siguiente objetivo.
________________________________________
PRUEBA 8 — EXPLORACIÓN LIBRE
Moverse por el mapa sin completar objetivos.
Confirmar que:
•	el jugador puede explorar 
•	la ubicación se actualiza 
•	el objetivo no cambia accidentalmente 
•	ningún componente se completa por accidente 
________________________________________
PRUEBA 9 — REGRESIÓN DE ALU
Al finalizar todos los cambios:
volver a probar la ALU completa.
La ALU debe continuar funcionando.
________________________________________
28. NO CREAR COMPONENTES INEXISTENTES
No crear:
•	Storage 
•	Disco 
•	ROM 
•	Bus 
•	CPU física adicional 
•	nuevos módulos de RAM 
•	nuevos componentes del mapa 
salvo que exista una necesidad técnica estrictamente relacionada con el sistema lógico.
Este prompt trabaja únicamente con los componentes actualmente existentes.
________________________________________
29. NO HACER
NO:
•	rehacer la cámara 
•	rehacer Cinemachine 
•	rehacer el jugador 
•	rehacer PlayerInteraction 
•	crear otro HUD 
•	crear otro Canvas 
•	crear otro sistema de objetivos 
•	crear otro sistema de interacción 
•	crear otro sistema de recompensas 
•	hacer una refactorización general 
•	modificar código que no sea necesario 
•	eliminar sistemas funcionales 
•	crear componentes físicos inexistentes 
•	implementar una CPU real 
•	implementar un emulador 
•	crear simulaciones técnicas innecesarias 
________________________________________
30. PRIORIDADES
Orden obligatorio de prioridad:
1.	No romper lo existente. 
2.	Crear progresión educativa real. 
3.	Crear guía clara. 
4.	Reemplazar las actividades matemáticas por actividades conceptuales sencillas. 
5.	Crear indicador de ubicación. 
6.	Crear actividad final. 
7.	Mantener la experiencia sencilla. 
8.	Mantener el código y arquitectura lo más simples posible. 
________________________________________
31. CRITERIO DE ÉXITO
Un jugador nuevo debe poder entrar al juego y entender progresivamente:
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
Respuesta correcta
      ↓
ALU COMPRENDIDA
      ↓
¿Qué sigue?
      ↓
REGISTROS
y continuar hasta RAM.
La experiencia debe sentirse como:
explorar → descubrir → aprender → comprender → avanzar.
No como una lista de ejercicios matemáticos.
________________________________________
32. INFORME FINAL OBLIGATORIO
Al terminar, entregar un informe claro indicando:
1.	Estado inicial encontrado. 
2.	Arquitectura existente reutilizada. 
3.	Scripts modificados. 
4.	Componentes lógicos nuevos creados. 
5.	Cambios realizados en Inspector. 
6.	Secuencia educativa final. 
7.	Objetivos configurados. 
8.	Actividad de cada componente. 
9.	Respuesta correcta de cada actividad. 
10.	Cómo funciona la progresión. 
11.	Cómo se evita completar objetivos fuera de orden. 
12.	Cómo funciona la exploración libre. 
13.	Cómo funciona el sistema de ubicación. 
14.	Qué zonas fueron creadas. 
15.	Cómo cambia el indicador superior. 
16.	Cómo funciona la guía. 
17.	Cómo funciona la actividad final. 
18.	Qué ocurre al completar toda la progresión. 
19.	Resultado de la prueba de ALU. 
20.	Resultado de la prueba de Registros. 
21.	Resultado de la prueba de Unidad de Control. 
22.	Resultado de la prueba de Cache. 
23.	Resultado de la prueba de RAM. 
24.	Errores nuevos. 
25.	Warnings nuevos. 
26.	Qué quedó pendiente. 
________________________________________
33. REGLA FINAL
NO continúes con nuevas funcionalidades después de completar este prompt.
Primero deja esta nueva base estable y probada.
El objetivo de este prompt NO es hacer un juego complejo.
El objetivo es transformar AstroBit en una experiencia educativa coherente donde el jugador:
EXPLORA
   ↓
SABE DÓNDE ESTÁ
   ↓
RECIBE UN OBJETIVO
   ↓
RECIBE UNA PISTA
   ↓
ENCUENTRA UN COMPONENTE
   ↓
LO EXAMINA
   ↓
APRENDE QUÉ HACE
   ↓
RESUELVE UNA ACTIVIDAD SENCILLA
   ↓
COMPRENDE EL CONCEPTO
   ↓
RECIBE UNA RECOMPENSA
   ↓
RECIBE EL SIGUIENTE OBJETIVO
manteniendo intacta la base funcional que ya fue probada.
No asumir. Inspeccionar primero. Modificar lo mínimo. Probar después de cada fase.

