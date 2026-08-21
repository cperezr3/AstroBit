# PROMPT 11 — CORRECCIÓN DEL PANEL EDUCATIVO INVISIBLE

**Proyecto:** AstroBit — Unity

---

# CONTEXTO

El diagnóstico del Prompt 10 encontró y confirmó la causa raíz del problema de interacción con `[E]`.

La tecla `E` funciona correctamente.

`PlayerInteraction` funciona correctamente.

`currentTarget` funciona correctamente.

`EducationalInteractable.Interact()` funciona correctamente.

`GameHUD.ShowEducationalPanel()` se ejecuta correctamente.

El problema es exclusivamente visual:

```text
E
↓
currentTarget = EducationalInteractable
↓
EducationalInteractable.Interact()
↓
GameHUD.ShowEducationalPanel()
↓
EducationPanel.SetActive(true)
↓
PERO EL PANEL NO SE RENDERIZA
```

Durante el diagnóstico se confirmó:

```text
panelRoot.activeSelf = true
```

pero el panel no aparece visualmente en Game View.

La causa raíz confirmada es:

```text
EducationPanel
```

se crea como:

```text
new GameObject("EducationPanel")
```

y por tanto tiene un:

```text
Transform
```

normal, no un:

```text
RectTransform
```

Sus elementos hijos de UI sí utilizan `RectTransform`, pero el contenedor padre no forma correctamente la cadena de `RectTransform` necesaria para el sistema UI de Unity.

Se comprobó que:

```text
Backdrop
```

tiene:

```text
rect = (0,0,0,0)
```

y por eso el panel está activo pero tiene tamaño efectivo cero/no se renderiza correctamente.

---

# OBJETIVO

Corregir únicamente la estructura del `EducationPanel` para que el panel educativo existente se renderice correctamente.

El resultado esperado es:

```text
Jugador cerca del ALU
↓
[E] Examinar
↓
Presionar E
↓
Panel educativo visible
↓
ALU
Unidad Aritmético-Lógica
↓
Resolver actividad
↓
12 + 7
↓
19
↓
Recompensa existente
```

---

# 1. DIAGNÓSTICO PREVIO

Antes de modificar, revisa:

```text
Assets/Scripts/UI/GameHUD.cs
```

especialmente:

```text
BuildPanel()
```

y determina exactamente cómo se crea:

```text
EducationPanel
```

Confirma que actualmente se utiliza algo equivalente a:

```text
new GameObject("EducationPanel")
```

sin un `RectTransform`.

**No hagas ningún cambio todavía hasta confirmar esto.**

---

# 2. CAMBIO MÍNIMO

Corrige únicamente la creación de:

```text
EducationPanel
```

para que sea un GameObject de UI con:

```text
RectTransform
```

La solución debe utilizar la API correcta de Unity para crear un elemento UI.

Por ejemplo, si la arquitectura actual lo permite:

```csharp
new GameObject("EducationPanel", typeof(RectTransform))
```

o una solución equivalente.

No copies esta solución ciegamente si existe una forma más adecuada dentro del código actual.

La prioridad es que:

```text
EducationPanel
```

tenga realmente:

```text
RectTransform
```

en lugar de un `Transform` normal.

---

# 3. CONSERVAR LA ESTRUCTURA EXISTENTE

**NO reconstruyas todo `GameHUD`.**

**NO rehagas el Canvas.**

**NO crees otro Canvas.**

**NO crees otro panel.**

**NO crees otro HUD.**

**NO dupliques `EducationPanel`.**

Mantén los elementos existentes:

```text
EducationPanel
├── Backdrop
├── PanelBox
├── ...
```

según la estructura real encontrada en `GameHUD.cs`.

La única modificación necesaria debería ser la creación correcta del contenedor padre.

---

# 4. IMPORTANTE SOBRE EL CANVAS

**NO modificar:**

* `HUDCanvas`
* `Canvas`
* `CanvasScaler`
* `GraphicRaycaster`
* `EventSystem`
* cámara
* Cinemachine

El Canvas actual funciona correctamente porque ya muestra:

```text
ObjectiveText
```

y:

```text
[E] Examinar
```

Por lo tanto, el problema **NO está en el Canvas**.

---

# 5. IMPORTANTE SOBRE LOS HIJOS

Después de convertir `EducationPanel` a `RectTransform`, verifica que:

```text
Backdrop
PanelBox
```

y los demás hijos continúen siendo hijos de:

```text
EducationPanel
```

No cambies innecesariamente:

* tamaños;
* posiciones;
* anchors;
* pivots;
* fuentes;
* colores;
* imágenes;
* textos.

La intención es que el panel aparezca como estaba diseñado originalmente.

---

# 6. NO TOCAR EL FLUJO DE INTERACCIÓN

**NO modificar:**

```text
PlayerInteraction.cs
```

La detección de `E` ya está confirmada como correcta.

**NO modificar:**

```text
interactionDistance = 4
```

**NO modificar:**

```text
proximityRadius = 10
```

**NO modificar:**

```text
currentTarget
```

**NO modificar:**

```text
UpdateLookTarget()
```

No cambiar el sistema de interacción.

---

# 7. NO TOCAR EDUCATIONALINTERACTABLE

**NO modificar** la lógica de:

```text
EducationalInteractable.Interact()
```

Actualmente funciona:

```text
Interact()
↓
state = PanelOpen
↓
GameHUD.ShowEducationalPanel(...)
```

Esto está correcto.

El problema es únicamente que el panel activado no se dibuja.

---

# 8. NO CREAR UNA NUEVA ACTIVIDAD

La actividad educativa ya existe.

Debe seguir siendo:

```text
12 + 7
```

**NO crear:**

* otra actividad;
* otro panel de preguntas;
* otro botón;
* otro sistema educativo.

El botón existente:

```text
Resolver actividad
```

debe seguir llamando a:

```text
OpenActivity()
```

---

# 9. PLAY MODE — PRUEBA OBLIGATORIA

Después de realizar el cambio, compila y entra en Play Mode.

Haz clic dentro de Game View.

Acércate al ALU.

Debe aparecer:

```text
ALU_Label
```

y:

```text
[E] Examinar
```

Presiona:

```text
E
```

---

# 10. RESULTADO ESPERADO DEL PANEL

Al presionar `E` ahora debe aparecer **VISUALMENTE** el panel educativo existente.

Debe poder verse algo equivalente a:

```text
ALU

Unidad Aritmético-Lógica

[ descripción existente ]

[Cerrar]       [Resolver actividad]
```

No importa si el texto exacto varía según el contenido actual del proyecto.

Lo importante es que el panel sea visible y correctamente renderizado.

---

# 11. COMPROBAR INTERACCIÓN CON EL PANEL

No basta con comprobar:

```text
panelRoot.activeSelf = true
```

Esta vez debe comprobarse **VISUALMENTE en Game View**.

El usuario debe poder ver:

```text
EducationPanel
```

y sus elementos.

También debe poder interactuar con los botones.

---

# 12. RESOLVER ACTIVIDAD

Presiona:

```text
Resolver actividad
```

Debe abrirse el panel de actividad existente.

Debe mostrar:

```text
12 + 7
```

No modificar la pregunta.

---

# 13. RESPUESTA INCORRECTA

Introduce una respuesta incorrecta, por ejemplo:

```text
5
```

Debe mostrar el resultado incorrecto existente.

El panel no debe romperse.

No modificar la lógica educativa.

---

# 14. RESPUESTA CORRECTA

Introduce:

```text
19
```

Debe aparecer la recompensa existente:

```text
✓ ALU ANALIZADA
```

---

# 15. CONTINUAR

Presiona:

```text
Continuar
```

Debe cerrarse el panel.

Debe ejecutarse la recompensa existente.

Debe continuar funcionando:

```text
ObjectiveSystem.CompleteObjective(...)
```

y después del delay correspondiente debe aparecer el objetivo siguiente.

---

# 16. COMPROBAR QUE EL ALU NO QUEDA BLOQUEADO

Esta prueba es **MUY IMPORTANTE** porque antes el panel invisible dejaba:

```text
state = PanelOpen
```

permanentemente.

Después de completar correctamente la actividad:

1. Aléjate del ALU.
2. Vuelve a acercarte.
3. Comprueba que:

```text
ALU_Label
```

vuelva a aparecer cuando corresponda.

Y:

```text
[E] Examinar
```

vuelva a aparecer cuando el jugador esté dentro de:

```text
interactionDistance = 4
```

---

# 17. PRUEBA DE CERRAR

También prueba el botón:

```text
Cerrar
```

del panel informativo inicial.

Resultado esperado:

```text
Panel cerrado
↓
state vuelve a Idle
↓
al alejarse/acercarse
↓
ALU vuelve a ser interactuable
```

No debe quedar bloqueado.

---

# 18. COMPROBAR QUE NO SE DUPLICÓ NADA

Después de la corrección confirma que existe:

```text
1 Canvas
1 EducationPanel
1 GameHUD
1 EducationalInteractable en el ALU
```

No debe aparecer ningún Canvas adicional.

No debe aparecer ningún panel duplicado.

---

# 19. ERRORES Y WARNINGS

Después de compilar y probar:

comprueba la Console.

Debe quedar sin:

```text
Error
Exception
```

y sin warnings nuevos relacionados con la corrección.

Si existe algún error, **no lo ocultes**.

Indica exactamente cuál es.

---

# 20. ARCHIVO QUE PREFERENTEMENTE DEBE CAMBIAR

La causa está en:

```text
Assets/Scripts/UI/GameHUD.cs
```

Por lo tanto, intenta que la corrección quede limitada a:

```text
Assets/Scripts/UI/GameHUD.cs
```

Si necesitas modificar otro archivo, **NO lo hagas sin antes explicar por qué es estrictamente necesario.**

No modificar:

```text
PlayerInteraction.cs
```

ni:

```text
EducationalInteractable.cs
```

---

# 21. NO HACER REFACTORIZACIÓN

No aproveches esta corrección para:

* limpiar código;
* renombrar variables;
* reorganizar `GameHUD`;
* cambiar arquitectura;
* mejorar estilos;
* cambiar Canvas;
* cambiar interacción;
* cambiar Input System;
* cambiar Cinemachine.

El objetivo es exclusivamente:

```text
EducationPanel
Transform
↓
RectTransform
↓
UI vuelve a renderizarse
```

---

# 22. INFORME FINAL

Al terminar, informa:

1. Causa raíz confirmada.
2. Archivo modificado.
3. Cambio realizado.
4. Componente que tenía `EducationPanel` antes.
5. Componente que tiene `EducationPanel` después.
6. Si `Backdrop` ahora tiene un tamaño válido.
7. Si el panel educativo aparece **VISUALMENTE** en Game View.
8. Si `[E] Examinar` sigue funcionando.
9. Si al presionar `E` aparece el panel.
10. Si `Resolver actividad` funciona.
11. Si aparece `12 + 7`.
12. Si una respuesta incorrecta funciona.
13. Si `19` funciona.
14. Si aparece `✓ ALU ANALIZADA`.
15. Si `Continuar` funciona.
16. Si el ALU vuelve a ser interactuable después de completar.
17. Si el botón `Cerrar` funciona.
18. Archivos modificados.
19. Errores nuevos en Console.
20. Confirmar que **NO** se modificaron Cinemachine, cámara, jugador, `PlayerInteraction` ni `EducationalInteractable`.

---

# RESTRICCIONES ABSOLUTAS

**NO modificar:**

* Cinemachine
* `CM FreeLook1`
* `CinemachineBrain`
* `CameraTarget`
* cámara
* jugador
* `MovementInput`
* `CharacterSkinController`
* `PlayerInteraction.cs`
* `EducationalInteractable.cs`
* `interactionDistance`
* `proximityRadius`
* `ALU_Label`
* Billboard
* Input System
* ProjectSettings
* sistema de recompensas
* ObjectiveSystem
* actividad educativa

salvo que aparezca un error inesperado directamente causado por la corrección del `RectTransform`.

**NO crear sistemas nuevos.**

**NO crear otro Canvas.**

**NO crear otro HUD.**

**NO crear otra actividad.**

**NO crear otro panel.**

**NO crear otro sistema de interacción.**

**NO hacer refactorización general.**

---

# OBJETIVO FINAL

Quiero que finalmente funcione exactamente así:

```text
Jugador cerca del ALU
↓
ALU_Label
↓
[E] Examinar
↓
Presionar E
↓
Aparece VISUALMENTE:

ALU
Unidad Aritmético-Lógica

[ Cerrar ]
[ Resolver actividad ]

↓
Presionar:
Resolver actividad

↓
12 + 7

↓
Responder:
19

↓
✓ ALU ANALIZADA

↓
Continuar

↓
Recompensa existente

↓
El ALU vuelve a quedar correctamente interactuable según su estado.
```

**Esta es una corrección puntual del UI. No tocar nada más del proyecto.**
