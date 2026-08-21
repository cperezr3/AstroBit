# Sistema de Interacción Educativa — AstroBit

## Objetivo

Diseñar e implementar el primer sistema de interacción educativa de AstroBit utilizando la ALU como prototipo vertical.

Este sistema deberá convertirse posteriormente en la base reutilizable para:

- Cache L1
- Cache L2
- Cache L3
- Registros
- Unidad de Control
- RAM
- Sistema de almacenamiento

La prioridad es conseguir una experiencia sencilla, clara y jugable, no construir una arquitectura excesivamente compleja.

---

# CONTEXTO ACTUAL

AstroBit es un videojuego educativo desarrollado en Unity 6000.4.8f1.

La escena contiene tres zonas principales:

1. CPU Room
2. RAM Room
3. Storage Room

La CPU Room contiene:

- ALU
- Cache L1
- Cache L2
- Cache L3
- Registros
- Unidad de Control

Actualmente existe un sistema básico:

- IInteractable
- PlayerInteraction
- SimpleInteractable
- ObjectiveSystem
- GameHUD

PlayerInteraction utiliza raycast y la tecla E.

SimpleInteractable permite interacción one-shot.

ObjectiveSystem actualmente maneja objetivos de forma básica.

La cámara y el movimiento ya funcionan correctamente.

El jugador es Jammo_Player.

IMPORTANTE:

No modificar scripts de terceros de Jammo-Character.

No modificar MovementInput.

No modificar CharacterSkinController.

No modificar el sistema de Input en esta fase.

---

# OBJETIVO DE EXPERIENCIA

Queremos evolucionar la interacción desde:

Jugador → mira objeto → E → texto

hacia:

Jugador
  ↓
se acerca al objeto
  ↓
aparece información breve sobre el objeto
  ↓
[E] Examinar
  ↓
aparece una interfaz educativa
  ↓
el jugador comprende el concepto
  ↓
realiza una pequeña actividad
  ↓
recibe resultado
  ↓
se completa el objetivo

La experiencia debe sentirse como un videojuego educativo y no como una aplicación de diapositivas.

---

# FASE 1 — INSPECCIÓN

Antes de modificar nada:

Utiliza el MCP de Unity y revisa:

- ALU.
- SimpleInteractable de ALU.
- PlayerInteraction.
- GameHUD.
- ObjectiveSystem.
- Canvas/runtime UI existente.
- Colliders de ALU.
- CameraTarget.
- Main Camera.

También lee los scripts correspondientes.

Determina qué partes pueden reutilizarse y cuáles necesitan ampliarse.

NO MODIFIQUES NADA DURANTE ESTA FASE.

---

# FASE 2 — DISEÑO

Diseña una solución sencilla para el siguiente flujo.

## 1. Proximidad

Cuando el jugador entre en un radio razonable alrededor de ALU:

mostrar una pequeña etiqueta flotante sobre el objeto.

Ejemplo:

ALU
Unidad Aritmético-Lógica

La etiqueta debe desaparecer cuando el jugador se aleje.

No debe bloquear la pantalla.

No debe requerir interacción para aparecer.

---

## 2. Prompt de interacción

Cuando el jugador esté suficientemente cerca y pueda interactuar:

mostrar:

[E] Examinar

o una variante visual equivalente.

Debe reutilizarse el sistema existente siempre que sea posible.

---

## 3. Panel educativo

Al pulsar E:

mostrar un panel de información.

Contenido inicial:

ALU
Unidad Aritmético-Lógica

La ALU es una parte fundamental del procesador encargada de realizar operaciones aritméticas y lógicas.

Puede realizar operaciones como:

- suma
- resta
- comparación
- AND
- OR
- NOT

El panel debe poder cerrarse.

El jugador no debe quedar atrapado permanentemente en la interfaz.

---

# FASE 3 — MICROACTIVIDAD DE ALU

Después de mostrar la información educativa, el jugador deberá realizar una actividad sencilla.

Primera actividad:

Resolver una operación.

Ejemplo:

--------------------------------
DIAGNÓSTICO DE ALU

La ALU necesita procesar:

12 + 7

Resultado:

[           ]

          [ EJECUTAR ]
--------------------------------

El jugador introduce el resultado.

Respuesta correcta:

✓ Operación completada

La ALU procesó correctamente la operación.

Respuesta incorrecta:

✗ Resultado incorrecto

Inténtalo nuevamente.

La actividad debe ser sencilla y no requerir sistemas externos.

---

# FASE 4 — RECOMPENSA

Cuando el jugador complete correctamente la actividad:

- marcar la interacción como completada;
- notificar al ObjectiveSystem;
- mostrar feedback;
- permitir continuar con la exploración.

Ejemplo:

✓ ALU ANALIZADA

Has aprendido cómo la Unidad Aritmético-Lógica procesa operaciones.

El sistema debe evitar completar repetidamente la misma actividad.

---

# FASE 5 — ARQUITECTURA

Diseña el sistema para poder reutilizarlo posteriormente.

No queremos crear:

ALUInteraction.cs
CacheInteraction.cs
RAMInteraction.cs
etc.

si todos hacen esencialmente lo mismo.

Preferimos una arquitectura configurable.

Por ejemplo, una interacción educativa podría tener:

- título;
- descripción;
- información;
- objetivo;
- tipo de actividad;
- datos de actividad;
- recompensa;
- estado de completado.

Sin embargo:

NO introduzcas ScriptableObjects, dependency injection, event buses ni frameworks externos salvo que exista una razón realmente necesaria.

Mantén la arquitectura sencilla y apropiada para un proyecto universitario.

---

# FASE 6 — UI

La UI debe ser:

- limpia;
- legible;
- sencilla;
- coherente con la temática tecnológica de AstroBit;
- no excesivamente grande;
- fácil de ampliar.

No necesitamos todavía una interfaz visual profesional.

Primero queremos que funcione correctamente.

El diseño visual podrá mejorarse posteriormente.

---

# FASE 7 — IMPLEMENTACIÓN

Después de analizar el sistema actual:

IMPLEMENTA únicamente el sistema necesario para que la ALU funcione como prototipo vertical.

Puedes:

- crear scripts propios;
- modificar scripts propios existentes cuando sea necesario;
- crear GameObjects/UI necesarios;
- configurar componentes de ALU;
- utilizar el sistema actual de interacción;
- ampliar ObjectiveSystem si es necesario.

NO puedes:

- modificar scripts de Jammo-Character;
- modificar MovementInput;
- modificar CharacterSkinController;
- migrar Input System;
- modificar ProjectSettings sin necesidad;
- rehacer el sistema de movimiento;
- rehacer Cinemachine;
- modificar otras habitaciones;
- implementar Cache, RAM o Storage todavía.

---

# FASE 8 — PRUEBAS

Utiliza el MCP de Unity para comprobar:

1. Entrar en el radio de ALU.
2. Ver información flotante.
3. Ver prompt [E].
4. Interactuar.
5. Abrir panel educativo.
6. Cerrar panel.
7. Volver a interactuar.
8. Realizar actividad.
9. Introducir resultado correcto.
10. Recibir feedback.
11. Completar objetivo.
12. Confirmar que no se puede completar repetidamente.
13. Comprobar que el jugador puede continuar moviéndose.
14. Confirmar que PlayerInteraction continúa funcionando.
15. Confirmar que la cámara continúa funcionando.
16. Revisar consola.

Si el MCP no puede simular una pulsación real de teclado, realiza todas las comprobaciones posibles mediante el Editor y deja claramente indicado qué parte requiere prueba manual.

---

# REGLAS IMPORTANTES

Trabaja de forma incremental.

No hagas refactors innecesarios.

No construyas todavía las actividades de Cache, RAM o Storage.

No remodeles el mapa.

No añadas assets visuales innecesarios.

No cambies el movimiento.

No cambies Cinemachine.

No cambies Input.

La ALU debe ser el prototipo que utilizaremos para validar toda la arquitectura de interacción educativa.

---

# RESULTADO FINAL

Al terminar informa:

A. Arquitectura encontrada.

B. Cambios realizados.

C. Scripts nuevos.

D. Scripts modificados.

E. GameObjects nuevos.

F. Configuración realizada en ALU.

G. Flujo final de interacción.

H. Pruebas realizadas.

I. Problemas encontrados.

J. Cómo reutilizar el sistema para Cache, Registros, RAM y Storage.

K. Próximo paso recomendado.

IMPORTANTE:

Si durante la implementación encuentras una decisión arquitectónica importante que pueda afectar a todo AstroBit, detente antes de realizarla y explícala.