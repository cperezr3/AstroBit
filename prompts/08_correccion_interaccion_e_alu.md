# PROMPT 08 — CORRECCIÓN DE INTERACCIÓN [E] Y ACTIVIDAD DE LA ALU

**Proyecto:** AstroBit — Unity

---

# CONTEXTO ACTUAL

La ALU ya funciona correctamente en cuanto a proximidad visual.

El problema anterior de la etiqueta flotante ya está **RESUELTO**.

Actualmente:

* `EducationalInteractable` funciona.
* `ALU_Label` funciona.
* La etiqueta aparece cuando el **JUGADOR** entra en el radio de proximidad.
* `proximityRadius = 10`.
* La etiqueta ya **NO depende de la distancia de Main Camera** para aparecer.
* El Billboard funciona correctamente.
* La cámara de tercera persona funciona.
* Cinemachine funciona.
* El jugador funciona.
* La actividad educativa existente funciona.
* El ALU no debe moverse ni cambiar de tamaño.

La corrección anterior modificó únicamente la lógica de proximidad de `EducationalInteractable` para que la distancia se calcule desde el jugador y no desde la cámara.

El comportamiento actual de la etiqueta ya fue probado y funciona correctamente.

**NO volver a modificar esa parte.**

---

# PROBLEMA ACTUAL

El problema pendiente es la interacción mediante `[E]`.

Cuando el jugador está suficientemente cerca del ALU:

```text
Jugador

   ↓

   ↓ cerca

   ↓

 ┌───────┐
 │  ALU  │
 └───────┘
```

la etiqueta aparece correctamente:

```text
ALU
Unidad Aritmético-Lógica
```

PERO:

```text
[E]
```

no aparece.

Tampoco se puede activar la interacción mediante la tecla `E`.

Actualmente el flujo es:

```text
Jugador cerca
      ↓
Etiqueta aparece       ✅
      ↓
[E] aparece            ❌
      ↓
Presionar E            ❌
      ↓
Actividad 12 + 7       ❌
      ↓
Recompensa             ❌
```

Quiero corregir **únicamente esta cadena de interacción**.

---

# OBJETIVO PRINCIPAL

Cuando el **JUGADOR** esté suficientemente cerca del ALU:

```text
Jugador cerca
      ↓
ALU detectado
      ↓
[E] aparece
      ↓
Jugador pulsa E
      ↓
Se abre la actividad educativa existente
      ↓
12 + 7
      ↓
Respuesta correcta
      ↓
Recompensa existente
```

**NO crear una actividad nueva.**

**NO crear otro sistema de interacción.**

**NO crear otro HUD.**

**NO crear otro Canvas.**

**NO duplicar la recompensa.**

**NO crear otro `EducationalInteractable`.**

Utilizar los sistemas que ya existen.

---

# 1. DIAGNÓSTICO OBLIGATORIO

**ANTES DE MODIFICAR CUALQUIER COSA**, inspecciona:

```text
Assets/Scripts/Interaction/PlayerInteraction.cs
```

y todos los scripts directamente relacionados con:

* `IInteractable`
* `EducationalInteractable`
* `GameHUD`
* prompt `[E]`
* actividad educativa
* recompensa
* input de interacción

Determina exactamente:

1. Cómo `PlayerInteraction` encuentra actualmente el objeto interactuable.
2. Desde dónde se realiza actualmente el raycast, si existe.
3. Qué distancia utiliza.
4. Qué variable controla `interactionDistance`.
5. Cómo se asigna `currentTarget`.
6. Qué condición utiliza `GameHUD` para mostrar `[E]`.
7. Qué método se ejecuta cuando el jugador pulsa `E`.
8. Qué método abre la actividad educativa.
9. Si la actividad de `12 + 7` sigue conectada al `EducationalInteractable` del ALU.
10. Si existe alguna condición adicional que impida mostrar `[E]`.

**NO hagas cambios durante esta fase.**

Primero analiza el código y determina la causa real.

Después informa brevemente qué encontraste **antes de realizar el cambio**.

---

# 2. CAUSA SOSPECHADA

En una investigación anterior se encontró:

```text
PlayerInteraction.interactionDistance = 3
```

y que el sistema de interacción utiliza la cámara para realizar la detección.

La cámara de tercera persona permanece aproximadamente:

```text
5.7 — 7 unidades
```

respecto al jugador/ALU dependiendo de la posición.

Esto puede producir:

```text
             ALU
              ↑
              │
           jugador
              │
              │
              │
           cámara
```

El jugador puede estar suficientemente cerca del ALU, pero la cámara puede estar a más de 3 unidades.

Por eso sospechamos que:

```text
currentTarget = null
```

aunque el jugador esté prácticamente al lado del ALU.

**PERO NO ASUMAS QUE ESTA ES LA ÚNICA CAUSA.**

Confírmalo leyendo el código y probándolo en Unity.

---

# 3. COMPORTAMIENTO DESEADO

La interacción debe depender de la posición del **JUGADOR**, no de exigir que la cámara esté físicamente cerca del ALU.

Queremos conceptualmente:

```text
PLAYER → ALU
```

y no:

```text
CAMERA → ALU
```

La cámara puede estar detrás del jugador debido a la perspectiva de tercera persona.

Eso **NO debe impedir** que el jugador interactúe con un objeto que está suficientemente cerca.

Por ejemplo:

```text
                  ALU
               ┌───────┐
               │       │
               └───────┘
                   ↑
                   │
                jugador
                   🧍
                   │
                   │
                   │
                 cámara
                   👁
```

Si el jugador está dentro del rango de interacción, debe poder aparecer `[E]` aunque la cámara esté varios metros detrás.

---

# 4. CAMBIO MÍNIMO

Si el diagnóstico confirma que `PlayerInteraction` utiliza la distancia o posición de la cámara para determinar si existe un objetivo:

modifica **únicamente lo necesario** para que la interacción se determine respecto al jugador.

**NO hagas una reestructuración completa del sistema.**

**NO reemplaces todo `PlayerInteraction` si no es necesario.**

**NO crees un sistema paralelo.**

**NO crees un nuevo manager.**

Conserva las protecciones existentes contra paredes, obstáculos y objetos no interactuables si ya existen.

La solución debe integrarse con la arquitectura actual.

---

# 5. IMPORTANTE SOBRE EL RAYCAST

**NO elimines automáticamente el raycast actual.**

Primero determina para qué se utiliza.

Si el raycast se usa únicamente para determinar el objeto que está en el centro de la cámara, comprueba si esa lógica es la responsable de que `currentTarget` sea `null`.

Si es necesario modificar la detección, utiliza la posición del jugador para establecer el rango de interacción.

Sin embargo, conserva cualquier protección razonable que ya exista para evitar interactuar a través de paredes u obstáculos.

La prioridad es:

```text
Jugador cerca del ALU
      ↓
ALU detectado
      ↓
currentTarget != null
      ↓
[E]
```

sin exigir:

```text
Cámara < 3 unidades del ALU
```

---

# 6. INTERACTION DISTANCE

**NO aumentes simplemente:**

```text
interactionDistance = 3
```

a:

```text
10
```

```text
20
```

```text
50
```

sin investigar.

El rango de interacción debe ser razonable para un juego de tercera persona.

Como referencia inicial, considera aproximadamente:

```text
3 — 5 unidades
```

y determina mediante Play Mode cuál funciona correctamente.

**IMPORTANTE:**

`proximityRadius = 10` controla la **VISIBILIDAD** de la etiqueta.

`interactionDistance` controla la **INTERACCIÓN**.

Son conceptos diferentes:

```text
proximityRadius
      ↓
muestra ALU_Label
```

y:

```text
interactionDistance
      ↓
permite [E]
```

**NO mezclar ambos valores.**

**NO modificar `proximityRadius`.**

---

# 7. NO MODIFICAR LA ETIQUETA

La etiqueta ya está funcionando correctamente.

**NO modificar:**

* `proximityRadius`
* `ALU_Label`
* Billboard
* `Quaternion.LookRotation(...)`
* posición de la etiqueta
* escala de la etiqueta
* Canvas
* TextMeshPro
* `labelHeight`
* lógica de orientación hacia Main Camera

El sistema de etiqueta ya fue corregido y probado.

**No volver a investigar ese problema.**

---

# 8. NO MODIFICAR CINEMACHINE

**NO modificar:**

* `CM FreeLook1`
* `CinemachineBrain`
* `CameraTarget`
* órbitas
* `Heading`
* `XAxis`
* `YAxis`
* recentrado
* FOV
* posición de cámara
* rotación de cámara

La cámara ya está funcionando correctamente.

El problema actual **NO debe resolverse moviendo la cámara**.

---

# 9. NO MODIFICAR EL JUGADOR

**NO modificar:**

* `MovementInput`
* `CharacterSkinController`
* posición inicial del jugador
* velocidad de movimiento
* collider del jugador
* CharacterController
* Input System

El jugador ya funciona correctamente.

---

# 10. GAMEHUD Y PROMPT [E]

Una vez que `currentTarget` sea correctamente asignado:

comprueba si `GameHUD` ya muestra automáticamente:

```text
[E]
```

Si funciona correctamente cuando:

```text
currentTarget != null
```

**NO modificar `GameHUD`.**

**NO crear otro Canvas.**

**NO crear otro texto.**

**NO crear otro HUD.**

Si `GameHUD` tiene una condición incorrecta independiente, corrige únicamente esa condición y nada más.

---

# 11. TECLA E

Cuando `[E]` sea visible:

presionar `E` debe utilizar el sistema existente.

Debe abrir la actividad educativa que ya está conectada al ALU.

**NO crear una actividad nueva.**

**NO crear un nuevo sistema de input.**

**NO modificar el Input System global.**

**NO crear otro controlador de teclado.**

---

# 12. ACTIVIDAD EDUCATIVA

La actividad existente debe seguir siendo la que ya está asociada a la ALU.

Debe abrirse:

```text
12 + 7
```

No crear otra actividad.

No duplicar el panel.

No crear otro sistema de preguntas.

No cambiar la lógica educativa si ya funciona.

Solo garantizar que la interacción existente pueda llegar hasta ella.

---

# 13. RECOMPENSA

Después de resolver correctamente:

```text
12 + 7 = 19
```

debe utilizarse la recompensa existente.

**NO crear otra recompensa.**

**NO duplicar la lógica de recompensa.**

**NO modificar el sistema de recompensa salvo que el diagnóstico demuestre que la cadena de interacción ya llega correctamente hasta allí y existe un error real en ese punto.**

---

# 14. PRUEBA OBLIGATORIA EN PLAY MODE

Después de realizar el cambio:

## PRUEBA 1 — LEJOS

Colocar al jugador lejos del ALU.

Resultado esperado:

```text
ALU_Label = oculto
[E] = oculto
```

---

## PRUEBA 2 — ACERCARSE

Acercar al jugador al ALU utilizando la cámara normal de tercera persona.

**NO acercar manualmente la cámara.**

Resultado esperado:

```text
ALU_Label = visible
[E] = visible
```

---

## PRUEBA 3 — PULSAR E

Con:

```text
[E]
```

visible:

presionar:

```text
E
```

Resultado esperado:

```text
Actividad educativa existente
```

---

## PRUEBA 4 — ACTIVIDAD

Comprobar:

```text
12 + 7 = 19
```

La actividad debe funcionar normalmente.

---

## PRUEBA 5 — RECOMPENSA

Comprobar que se entrega la recompensa existente.

---

## PRUEBA 6 — ALEJARSE

Alejar al jugador del ALU.

Resultado esperado:

```text
ALU_Label → desaparece cuando corresponde
[E] → desaparece
```

No debe quedar `[E]` permanentemente visible.

---

# 15. PRUEBA DESDE DISTINTOS ÁNGULOS

Probar la interacción desde:

* frente
* izquierda
* derecha
* diagonal
* atrás

La cámara debe permanecer en tercera persona normal.

Si el jugador está dentro del rango de interacción:

```text
[E]
```

debe aparecer.

No debe ser necesario colocar manualmente la cámara frente al ALU.

---

# 16. VERIFICAR QUE NO SE INTERACTÚE A DISTANCIA EXCESIVA

Después de hacer funcionar `[E]`, verifica que no aparezca desde una distancia absurda.

Por ejemplo, un jugador claramente lejos del ALU no debe poder interactuar.

El rango debe sentirse razonable para una interacción de tercera persona.

---

# 17. SI HAY MÁS DE UNA CAUSA

Si encuentras dos problemas diferentes, sepáralos.

Por ejemplo:

```text
Problema A:
```

`PlayerInteraction` no encuentra el ALU.

```text
Problema B:
```

`GameHUD` no muestra `[E]` aunque `currentTarget` exista.

Corrige únicamente lo necesario para solucionar cada uno.

No hagas una reestructuración completa.

---

# 18. SI EL PROBLEMA NO ES PLAYERINTERACTION

Si después del diagnóstico descubres que `PlayerInteraction` sí encuentra correctamente al ALU, pero `[E]` no aparece:

**NO modifiques `PlayerInteraction` innecesariamente.**

Investiga específicamente:

```text
currentTarget
GameHUD
Input E
EducationalInteractable
```

y determina dónde se rompe la cadena.

---

# 19. SI [E] FUNCIONA PERO LA ACTIVIDAD NO SE ABRE

No rehagas la actividad.

Determina qué método existente debería ejecutarse cuando se pulsa `E`.

Comprueba por qué no llega al método de activación.

Corrige únicamente el enlace necesario.

---

# 20. VERIFICACIÓN FINAL OBLIGATORIA

Al terminar informa:

1. Qué causaba exactamente que `[E]` no apareciera.
2. Qué archivo era responsable.
3. Cómo se detectaba anteriormente `currentTarget`.
4. Si se utilizaba la cámara para detectar la interacción.
5. Qué modificación mínima realizaste.
6. Qué valor final tiene `interactionDistance`.
7. Si `[E]` aparece con la cámara normal de tercera persona.
8. Si `[E]` funciona desde frente.
9. Si `[E]` funciona desde izquierda.
10. Si `[E]` funciona desde derecha.
11. Si `[E]` funciona desde atrás.
12. Si al pulsar E se abre la actividad existente.
13. Si `12 + 7` funciona.
14. Si la respuesta correcta funciona.
15. Si la recompensa existente sigue funcionando.
16. Si la etiqueta sigue funcionando.
17. Qué archivos fueron modificados.
18. Si hubo errores o warnings nuevos en consola.

---

# RESTRICCIONES FINALES

**NO modificar:**

* Cinemachine
* `CM FreeLook1`
* `CinemachineBrain`
* `CameraTarget`
* `MovementInput`
* `CharacterSkinController`
* `EducationalInteractable`, salvo que el diagnóstico demuestre que es estrictamente necesario
* `ALU_Label`
* Billboard
* `proximityRadius`
* `GameHUD`, salvo que el diagnóstico demuestre que es estrictamente necesario
* Input System global
* ProjectSettings
* posición del jugador
* posición del ALU
* escala del ALU
* collider del ALU
* actividad educativa existente
* sistema de recompensas existente

**No hacer refactorización general.**

**No crear sistemas nuevos.**

**No crear managers nuevos.**

**No rehacer la cámara.**

**No rehacer el HUD.**

**No rehacer la actividad.**

**No rehacer el sistema de recompensas.**

**No continuar todavía con:**

* Cache
* Registros
* RAM
* Storage

La prioridad absoluta de este prompt es conseguir:

```text
JUGADOR CERCA DEL ALU
        ↓
ALU_Label visible
        ↓
[E] visible
        ↓
PULSAR E
        ↓
ACTIVIDAD 12 + 7
        ↓
RESPUESTA 19
        ↓
RECOMPENSA
```

manteniendo intacto todo lo que ya funciona.
