# PROMPT 09 — CORRECCIÓN DE ACTIVACIÓN DE ACTIVIDAD EDUCATIVA AL PULSAR [E]

**Proyecto:** AstroBit — Unity

---

# CONTEXTO ACTUAL

El Prompt 08 ya fue ejecutado y la detección de interacción mediante `[E]` fue corregida.

El problema anterior era que `PlayerInteraction` utilizaba un raycast desde la cámara para detectar el objeto interactuable.

Ese problema ya está **RESUELTO**.

Actualmente:

* `PlayerInteraction` detecta correctamente el ALU mediante proximidad del jugador.
* `interactionDistance = 4`.
* La detección ya NO depende de la distancia de Main Camera.
* Se utiliza `Physics.OverlapSphere` alrededor del jugador.
* Se selecciona el `IInteractable` válido más cercano.
* Se conserva un chequeo de línea de visión para evitar atravesar paredes/obstáculos.
* `currentTarget` funciona correctamente.
* `[E] Examinar` aparece correctamente cuando el jugador está dentro del rango.
* `ALU_Label` aparece correctamente mediante `proximityRadius = 10`.
* La cámara de tercera persona funciona correctamente.
* Cinemachine funciona correctamente.
* El jugador funciona correctamente.
* El ALU no debe moverse ni cambiar de tamaño.
* El sistema de proximidad de `EducationalInteractable` funciona correctamente.

El único archivo modificado por el Prompt 08 fue:

```text
Assets/Scripts/Interaction/PlayerInteraction.cs
```

---

# PROBLEMA ACTUAL

Ahora ocurre lo siguiente:

```text
Jugador se acerca al ALU
        ↓
ALU_Label aparece              ✅
        ↓
[E] Examinar aparece           ✅
        ↓
Jugador presiona E
        ↓
[E] desaparece                  ❌
ALU_Label desaparece            ❌
        ↓
Actividad educativa            ❌
NO aparece
```

Es decir:

**La detección del interactuable funciona, pero la activación de la interacción no está llegando correctamente a la actividad educativa.**

El problema actual ya NO es detectar al ALU.

El problema actual es:

```text
currentTarget
     ↓
Interact()
     ↓
EducationalInteractable
     ↓
actividad educativa
```

La cadena se rompe en algún punto después de presionar `E`.

---

# OBJETIVO PRINCIPAL

Conseguir que:

```text
JUGADOR CERCA DEL ALU
        ↓
ALU_Label visible
        ↓
[E] Examinar visible
        ↓
PULSAR E
        ↓
EducationalInteractable.Interact()
        ↓
SE ABRE LA ACTIVIDAD EDUCATIVA EXISTENTE
        ↓
"12 + 7"
        ↓
Respuesta 19
        ↓
Pantalla de recompensa existente
        ↓
Continuar
        ↓
ObjectiveSystem existente
```

**NO crear una actividad nueva.**

**NO crear otro panel.**

**NO crear otro Canvas.**

**NO crear otro HUD.**

**NO crear otro `EducationalInteractable`.**

**NO crear otro sistema de interacción.**

**NO duplicar la recompensa.**

Utilizar exclusivamente los sistemas que ya existen.

---

# 1. DIAGNÓSTICO OBLIGATORIO

**ANTES DE MODIFICAR CUALQUIER ARCHIVO**, inspecciona el flujo completo que ocurre al presionar `E`.

Como mínimo, revisa:

```text
Assets/Scripts/Interaction/PlayerInteraction.cs
```

y todos los scripts directamente relacionados con:

* `IInteractable`
* `EducationalInteractable`
* `GameHUD`
* input de la tecla `E`
* `Interact()`
* actividad educativa
* panel educativo
* pregunta `12 + 7`
* recompensa
* `ObjectiveSystem`

Busca específicamente:

```text
Interact()
```

```text
currentTarget
```

```text
Input.GetKeyDown(KeyCode.E)
```

o el equivalente utilizado por el proyecto.

Determina exactamente:

1. Qué método se ejecuta cuando el jugador presiona `E`.
2. Qué objeto contiene actualmente `currentTarget`.
3. Qué implementación concreta de `IInteractable` tiene el ALU.
4. Qué método `Interact()` se ejecuta realmente.
5. Qué hace exactamente ese `Interact()`.
6. Si `EducationalInteractable.Interact()` existe.
7. Si `EducationalInteractable.Interact()` llama correctamente al sistema que abre la actividad.
8. Qué método abre el panel educativo.
9. Qué objeto controla actualmente ese panel.
10. Si el panel educativo está activo/inactivo antes de presionar `E`.
11. Si el panel se abre y se cierra inmediatamente.
12. Si alguna lógica limpia `currentTarget` al presionar `E`.
13. Si alguna lógica desactiva `ALU_Label` al comenzar la interacción.
14. Si existe una condición que impida abrir la actividad.
15. Si existe alguna excepción en consola al presionar `E`.
16. Si el ALU tiene correctamente asignada la referencia a la actividad educativa.
17. Si la actividad `12 + 7` sigue siendo la misma actividad existente.
18. Si el sistema de recompensa sigue conectado al flujo educativo.

**NO hagas cambios durante esta fase.**

Primero determina exactamente dónde se rompe la cadena.

---

# 2. IMPORTANTE: NO CONFIAR ÚNICAMENTE EN EL INFORME ANTERIOR

El informe del Prompt 08 afirma que se verificó toda la cadena:

```text
[E]
 ↓
Actividad
 ↓
12 + 7
 ↓
19
 ↓
Recompensa
```

Sin embargo, el comportamiento observado actualmente es:

```text
[E]
 ↓
desaparece [E]
 ↓
desaparece etiqueta
 ↓
NO aparece actividad
```

Por lo tanto:

**NO asumas que la actividad realmente se abre correctamente.**

Verifica el comportamiento real en Play Mode.

El objetivo es encontrar la causa real del comportamiento observado.

---

# 3. NO TOCAR LA DETECCIÓN YA CORREGIDA

El Prompt 08 ya corrigió correctamente:

```text
PlayerInteraction.UpdateLookTarget()
```

La detección ahora funciona mediante:

```text
Physics.OverlapSphere
```

alrededor del jugador.

`interactionDistance` actualmente es:

```text
4
```

**NO cambiarlo salvo que el diagnóstico demuestre que es estrictamente necesario.**

No volver al raycast desde la cámara.

No volver a utilizar:

```text
playerCamera.transform.position
```

como origen de detección.

No volver a utilizar:

```text
playerCamera.transform.forward
```

para encontrar el interactuable.

---

# 4. NO MODIFICAR PROXIMITYRADIUS

La etiqueta ya funciona correctamente.

Actualmente:

```text
proximityRadius = 10
```

Debe permanecer:

```text
proximityRadius = 10
```

No modificar:

* `proximityRadius`
* `ALU_Label`
* Billboard
* `labelHeight`
* posición de la etiqueta
* escala de la etiqueta
* `Quaternion.LookRotation(...)`
* Canvas
* TextMeshPro
* orientación hacia Main Camera

El hecho de que la etiqueta desaparezca al presionar `E` puede ser una consecuencia de que el interactuable cambie de estado.

**No asumir que la solución consiste en modificar la etiqueta.**

---

# 5. INVESTIGAR QUÉ OCURRE EXACTAMENTE AL PRESIONAR E

Necesitamos determinar si ocurre:

```text
E
 ↓
PlayerInteraction
 ↓
currentTarget.Interact()
```

o si ocurre algo diferente.

Comprueba mediante código y/o logs temporales si es necesario:

```text
currentTarget != null
```

justo antes de llamar a `Interact()`.

Después comprueba:

```text
currentTarget.Interact()
```

y determina qué método concreto termina ejecutándose.

Si utilizas logs temporales para diagnosticar:

* hazlos mínimos;
* úsalos únicamente durante la investigación;
* elimínalos o déjalos limpios al terminar si ya no son necesarios.

No llenes la consola de logs permanentes.

---

# 6. INVESTIGAR EDUCATIONALINTERACTABLE

Inspecciona cuidadosamente:

```text
EducationalInteractable
```

Determina:

1. Qué hace `Interact()`.
2. Qué referencias utiliza.
3. Qué método llama para abrir la actividad.
4. Si necesita una referencia a `GameHUD`.
5. Si necesita una referencia a algún controlador de actividad.
6. Si necesita una referencia a un panel.
7. Si la referencia está asignada en el Inspector.
8. Si la actividad educativa está asignada correctamente.
9. Si existe una condición que impida abrirla.
10. Si el objeto ALU tiene realmente el componente correcto.

No reemplazar `EducationalInteractable`.

No crear otro componente.

Si la referencia existente está rota o vacía, corrige únicamente esa referencia o el enlace estrictamente necesario.

---

# 7. INVESTIGAR GAMEHUD

Inspecciona `GameHUD`.

Determina:

* cómo muestra `[E]`;
* cómo oculta `[E]`;
* qué sucede cuando se ejecuta `HidePrompt`;
* si `HidePrompt()` también afecta al panel educativo;
* si al presionar `E` se desactiva accidentalmente algún objeto que contiene la actividad;
* si el HUD tiene referencias compartidas con la actividad.

Es importante diferenciar:

```text
[E] Examinar
```

de:

```text
Panel de actividad educativa
```

Ocultar `[E]` después de interactuar puede ser correcto.

Lo incorrecto es que también desaparezca la actividad o que nunca llegue a abrirse.

---

# 8. INVESTIGAR EL PANEL EDUCATIVO

Encuentra exactamente qué GameObject/Canvas/Panel se utiliza para la actividad educativa existente.

Determina:

```text
¿Está activo inicialmente?
```

```text
¿Quién lo activa?
```

```text
¿Quién lo desactiva?
```

```text
¿Quién configura la pregunta?
```

```text
¿Quién muestra "12 + 7"?
```

```text
¿Quién procesa la respuesta?
```

No crear un panel nuevo.

No crear un Canvas nuevo.

Utilizar el panel que ya existe.

---

# 9. POSIBLE CASO: EL PANEL SE ABRE Y SE CIERRA INMEDIATAMENTE

Investiga específicamente si sucede esto:

```text
Presionar E
     ↓
Actividad se abre
     ↓
alguna lógica de actualización detecta
currentTarget = null
     ↓
la actividad se cierra
```

o:

```text
Presionar E
     ↓
GameHUD.HidePrompt()
     ↓
alguna lógica asociada desactiva accidentalmente
el panel educativo
```

o:

```text
Presionar E
     ↓
EducationalInteractable.Interact()
     ↓
panel.SetActive(true)
     ↓
otra lógica ejecuta SetActive(false)
```

Si encuentras este comportamiento, corrige únicamente la causa.

---

# 10. POSIBLE CASO: INTERACT() NO SE EJECUTA

Si descubres que:

```text
[E]
```

aparece correctamente pero:

```text
currentTarget.Interact()
```

no se ejecuta al pulsar E:

determina por qué.

Revisa:

* input;
* condición de la tecla;
* `currentTarget`;
* estado del jugador;
* estado de la actividad;
* referencias;
* condiciones de bloqueo.

Corrige únicamente lo necesario.

**NO crear un nuevo sistema de input.**

---

# 11. POSIBLE CASO: INTERACT() SE EJECUTA PERO NO ABRE LA ACTIVIDAD

Si:

```text
Interact()
```

sí se ejecuta pero la actividad no aparece:

investiga el método inmediatamente siguiente.

Ejemplo:

```text
PlayerInteraction
      ↓
Interact()
      ↓
EducationalInteractable
      ↓
OpenActivity()
      ↓
???
```

Encuentra exactamente dónde se rompe la cadena.

No inventes una nueva función si ya existe una función equivalente.

Si existe:

```text
OpenActivity()
```

utiliza esa.

Si existe:

```text
ShowEducationalPanel()
```

utiliza esa.

Si existe otro método equivalente, utiliza el existente.

---

# 12. POSIBLE CASO: REFERENCIA NULL

Comprueba la consola de Unity inmediatamente después de presionar `E`.

Busca errores como:

```text
NullReferenceException
MissingReferenceException
```

o cualquier excepción relacionada con:

* `EducationalInteractable`
* `GameHUD`
* actividad
* panel
* recompensa
* `ObjectiveSystem`

Si existe una excepción:

1. identifica el archivo;
2. identifica la línea;
3. determina por qué la referencia es `null` o inválida;
4. corrige únicamente esa referencia o lógica.

No ocultes la excepción.

No añadas `try/catch` simplemente para esconder el error.

---

# 13. NO CAMBIAR LA ACTIVIDAD EDUCATIVA

La actividad educativa existente ya tiene la pregunta:

```text
12 + 7
```

y el flujo de respuesta.

**NO rehacerla.**

**NO cambiar la pregunta.**

**NO crear otra actividad.**

**NO crear otro panel de preguntas.**

**NO crear otro sistema de respuestas.**

La actividad solo necesita volver a ser accesible mediante `E`.

---

# 14. NO CAMBIAR LA RECOMPENSA

La recompensa existente debe permanecer intacta.

El flujo esperado después de solucionar el problema es:

```text
12 + 7
     ↓
19
     ↓
✓ ALU ANALIZADA
     ↓
Continuar
     ↓
ObjectiveSystem.CompleteObjective(...)
```

No duplicar:

* recompensas;
* objetivos;
* `ObjectiveSystem`;
* paneles;
* lógica de finalización.

---

# 15. PRUEBA OBLIGATORIA DESPUÉS DEL CAMBIO

Después de realizar la modificación mínima, ejecutar Play Mode.

## PRUEBA 1 — JUGADOR LEJOS

Colocar al jugador aproximadamente a:

```text
13 unidades
```

o claramente fuera del rango.

Resultado esperado:

```text
ALU_Label = oculto
[E] = oculto
```

---

## PRUEBA 2 — DENTRO DEL RADIO DE ETIQUETA PERO FUERA DE INTERACCIÓN

Colocar al jugador aproximadamente a:

```text
8 unidades
```

Resultado esperado:

```text
ALU_Label = visible
[E] = oculto
Actividad = cerrada
```

Esto confirma que:

```text
proximityRadius = 10
```

sigue separado de:

```text
interactionDistance = 4
```

---

## PRUEBA 3 — DENTRO DEL RANGO DE INTERACCIÓN

Colocar al jugador aproximadamente a:

```text
4 unidades o menos
```

Resultado esperado:

```text
ALU_Label = visible
[E] Examinar = visible
```

---

## PRUEBA 4 — PRESIONAR E

Con `[E] Examinar` visible:

```text
Presionar E
```

Resultado esperado:

```text
[E] puede desaparecer
```

Esto puede ser correcto.

PERO:

```text
Actividad educativa = visible
```

debe aparecer inmediatamente.

---

# 16. PRUEBA 5 — ACTIVIDAD

Comprobar que aparece exactamente la actividad educativa existente.

Debe aparecer:

```text
12 + 7
```

No debe aparecer un panel nuevo.

No debe aparecer una actividad diferente.

---

# 17. PRUEBA 6 — RESPUESTA INCORRECTA

Responder:

```text
5
```

Resultado esperado:

```text
✗ Resultado incorrecto...
```

La actividad debe permanecer abierta.

---

# 18. PRUEBA 7 — RESPUESTA CORRECTA

Responder:

```text
19
```

Resultado esperado:

```text
✓ ALU ANALIZADA
```

utilizando la pantalla de recompensa existente.

---

# 19. PRUEBA 8 — CONTINUAR

Presionar:

```text
Continuar
```

Resultado esperado:

```text
Panel educativo se cierra
        ↓
ObjectiveSystem.CompleteObjective(...)
        ↓
objetivo actualizado
```

No duplicar el objetivo.

No ejecutar la recompensa dos veces.

---

# 20. PRUEBA 9 — VOLVER A ACERCARSE

Después de completar la actividad:

alejarse y volver a acercarse al ALU.

Verificar que el sistema mantiene el comportamiento correcto.

Si el ALU ya debe considerarse completado y existe una lógica existente que cambia su interacción después de completarlo, respetarla.

No inventar una nueva lógica.

---

# 21. PRUEBA 10 — DIFERENTES ÁNGULOS

Verificar nuevamente:

* frente;
* izquierda;
* derecha;
* diagonal;
* atrás.

La cámara debe permanecer en tercera persona normal.

No mover la cámara manualmente para conseguir la interacción.

---

# 22. RESTRICCIÓN CRÍTICA SOBRE PLAYERINTERACTION

El Prompt 08 ya solucionó:

```text
PlayerInteraction.UpdateLookTarget()
```

**NO volver a modificarlo salvo que el diagnóstico demuestre inequívocamente que el problema actual se encuentra allí.**

No cambiar nuevamente:

```text
Physics.OverlapSphere
```

No volver a:

```text
Physics.Raycast desde Main Camera
```

No cambiar innecesariamente:

```text
interactionDistance = 4
```

No modificar la detección simplemente porque `[E]` desaparece después de presionar E.

Que `[E]` desaparezca después de presionar E puede ser comportamiento normal.

El objetivo es que la actividad se abra.

---

# 23. RESTRICCIONES SOBRE LA ETIQUETA

**NO modificar:**

* `EducationalInteractable` relacionado con `proximityRadius`, salvo causa estrictamente necesaria;
* `proximityRadius = 10`;
* `ALU_Label`;
* Billboard;
* Cinemachine;
* cámara;
* `GameHUD` para solucionar problemas que realmente pertenecen a la actividad.

La desaparición de la etiqueta después de interactuar **no debe considerarse automáticamente un bug**.

Primero determina si forma parte del flujo normal de interacción.

---

# 24. CAMBIO MÍNIMO

La modificación final debe ser lo más pequeña posible.

Preferencia:

```text
1 archivo
```

si es suficiente.

Si se necesitan varios archivos, informa exactamente por qué.

No hacer:

* refactorización general;
* reorganización de carpetas;
* nuevos managers;
* nuevos sistemas;
* nuevos Canvas;
* nuevos prefabs;
* nuevos scripts innecesarios;
* cambios de cámara;
* cambios de movimiento;
* cambios de Input System global.

---

# 25. VERIFICACIÓN FINAL OBLIGATORIA

Al terminar, proporciona un informe con:

1. **Causa exacta del problema.**
2. **Archivo responsable.**
3. **Método donde se rompía la cadena.**
4. **Qué ocurre actualmente al presionar E.**
5. **Qué método `Interact()` se ejecuta.**
6. **Qué método abre la actividad educativa.**
7. **Qué referencia estaba faltando o qué condición impedía abrirla**, si existía.
8. **Modificación mínima realizada.**
9. **Archivos modificados.**
10. **Valor final de `interactionDistance`.**
11. **Confirmación de que `proximityRadius` sigue en `10`.**
12. **Confirmación de que `ALU_Label` sigue funcionando.**
13. **Confirmación de que `[E] Examinar` aparece.**
14. **Confirmación de que al presionar E aparece la actividad.**
15. **Confirmación de que aparece `12 + 7`.**
16. **Confirmación de que una respuesta incorrecta funciona.**
17. **Confirmación de que `19` funciona.**
18. **Confirmación de que aparece la recompensa existente.**
19. **Confirmación de que `ObjectiveSystem.CompleteObjective(...)` sigue funcionando.**
20. **Confirmación de que no se duplicaron actividades, paneles, recompensas ni sistemas.**
21. **Errores nuevos de consola, si existen.**
22. **Warnings nuevos de consola, si existen.**

---

# RESTRICCIONES FINALES

## NO MODIFICAR

* Cinemachine
* `CM FreeLook1`
* `CinemachineBrain`
* `CameraTarget`
* `MovementInput`
* `CharacterSkinController`
* cámara de tercera persona
* posición del jugador
* posición del ALU
* escala del ALU
* collider del ALU
* `proximityRadius`
* `ALU_Label`
* Billboard
* actividad educativa existente
* sistema de recompensas existente
* `ObjectiveSystem`
* Input System global
* ProjectSettings

## NO CREAR

* otra actividad educativa;
* otro panel;
* otro Canvas;
* otro HUD;
* otro `EducationalInteractable`;
* otro sistema de interacción;
* otro sistema de input;
* otro manager;
* otra recompensa;
* otro sistema de objetivos.

## NO HACER

* refactorización general;
* rehacer la cámara;
* rehacer Cinemachine;
* rehacer el HUD;
* rehacer la actividad;
* rehacer las recompensas;
* cambiar `interactionDistance` sin motivo;
* volver al raycast desde la cámara;
* modificar `proximityRadius`.

---

# PRIORIDAD ABSOLUTA

El resultado final debe ser exactamente:

```text
JUGADOR CERCA DEL ALU
        ↓
ALU_Label visible
        ↓
[E] Examinar visible
        ↓
PULSAR E
        ↓
[E] puede desaparecer
        ↓
ACTIVIDAD EDUCATIVA EXISTENTE SE ABRE
        ↓
12 + 7
        ↓
RESPUESTA INCORRECTA → permanece en actividad
        ↓
RESPUESTA 19
        ↓
✓ ALU ANALIZADA
        ↓
Continuar
        ↓
ObjectiveSystem.CompleteObjective(...)
        ↓
RECOMPENSA / OBJETIVO ACTUALIZADO
```

**La prioridad NO es mantener `[E]` visible después de presionar E.**

La prioridad es que **al presionar E se ejecute correctamente la interacción existente y se abra la actividad educativa existente**.

Mantén intacto todo lo que ya funciona.

**NO continúes todavía con Cache, Registros, RAM ni Storage.**
