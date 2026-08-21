# PROMPT 06 — CORREGIR ORIENTACIÓN REAL DE ALU_LABEL DESDE TERCERA PERSONA

Proyecto: AstroBit — Unity

## SITUACIÓN REAL ACTUAL

La corrección anterior NO resolvió el problema.

No asumir que la verificación anterior fue correcta.

El comportamiento que observo realmente es:

* El ALU es visible.
* El jugador puede acercarse al ALU.
* `ALU_Label` existe.
* La etiqueta puede llegar a verse.
* Pero la etiqueta NO se comporta correctamente desde la cámara normal de tercera persona.
* Cuando la cámara está en la posición normal de tercera persona, mirando hacia el ALU, la etiqueta desaparece visualmente.
* Cuando la cámara queda en una orientación opuesta, por ejemplo cuando el jugador está de espaldas y la cámara queda del otro lado, la etiqueta sí puede verse.

El comportamiento deseado es exactamente el contrario:

> La etiqueta debe verse normalmente desde la cámara de tercera persona mientras el jugador mira hacia el ALU.

## IMPORTANTE

NO cambies el tamaño del ALU.

NO cambies la posición del ALU.

NO cambies la posición del jugador.

NO cambies Cinemachine.

NO cambies PlayerInteraction.

NO cambies MovementInput.

NO cambies GameHUD.

NO cambies ObjectiveSystem.

NO cambies ProjectSettings.

NO crees nuevos sistemas.

El problema a investigar es exclusivamente:

```text
EducationalInteractable
        ↓
ALU_Label
        ↓
Canvas World Space
        ↓
orientación de la cara frontal
        ↓
Main Camera
```

---

# 1. INSPECCIONAR EL CANVAS REAL

Lee:

```text
Assets/Scripts/Interaction/EducationalInteractable.cs
```

Inspecciona cómo `BuildLabel()` crea:

```text
ALU_Label
    Canvas
        Title
        Subtitle
```

Determina específicamente:

* Render Mode
* RectTransform
* localRotation
* localScale
* sizeDelta
* posición
* eje forward
* eje up
* orientación inicial
* configuración de TextMeshPro

Necesito saber cuál es la **cara frontal real del Canvas**.

No asumir que el Canvas mira hacia `+Z`.

---

# 2. INSPECCIONAR EL BILLBOARD ACTUAL

Actualmente `UpdateLabel()` utiliza una lógica equivalente a:

```csharp
Vector3 direction = labelPos - cam.transform.position;

labelRoot.transform.rotation =
    Quaternion.LookRotation(direction, Vector3.up);
```

Analiza matemáticamente qué está haciendo.

Quiero que determines:

```text
Main Camera
     ↓
vector cámara → etiqueta
     ↓
dirección utilizada
     ↓
forward del Canvas
```

y compares eso con la cara frontal real del Canvas.

---

# 3. REPRODUCIR EXACTAMENTE EL BUG

No hagas una prueba abstracta.

Usando Unity MCP:

### PRUEBA A — TERCERA PERSONA NORMAL

Coloca al jugador en la situación habitual:

```text
             ALU
              ▲
              │
              │
             🧍
              \
               \
                👁
              CÁMARA
```

La cámara debe estar detrás del jugador mirando hacia el ALU.

Comprueba:

* ALU visible
* ALU_Label existente
* ALU_Label activo
* Canvas activo
* Title activo
* Subtitle activo
* texto visible o invisible

Guarda los valores de:

* cámara position
* cámara rotation
* label position
* label rotation
* label forward
* label up
* vector cámara → label

---

# 4. PRUEBA B — ORIENTACIÓN OPUESTA

Ahora reproduce la situación donde actualmente la etiqueta sí aparece.

No cambies ningún otro sistema.

Registra los mismos valores:

* cámara position
* cámara rotation
* label position
* label rotation
* label forward
* label up
* vector cámara → label

Compara A contra B.

---

# 5. DETERMINAR SI ES LA CARA DEL CANVAS

Quiero una respuesta concreta:

¿La etiqueta está desapareciendo porque estamos viendo el **backface** del Canvas?

Comprueba si:

```text
Canvas forward
```

apunta hacia:

```text
Main Camera
```

o en dirección contraria.

Si el Canvas tiene su cara visible hacia `-Z`, por ejemplo, entonces un:

```csharp
Quaternion.LookRotation(direction, Vector3.up)
```

puede estar orientando el eje incorrecto.

En ese caso la solución puede requerir invertir el forward, por ejemplo:

```csharp
Quaternion.LookRotation(-direction, Vector3.up)
```

PERO NO cambies esto simplemente por intuición.

Primero confirma cuál es la cara frontal real del Canvas.

---

# 6. MUY IMPORTANTE — PROBAR LA SOLUCIÓN CORRECTA

Una vez determinado el eje frontal real:

Haz SOLO la modificación mínima dentro de:

```text
Assets/Scripts/Interaction/EducationalInteractable.cs
```

La etiqueta debe:

* permanecer encima del ALU
* mirar a Main Camera
* mantenerse vertical
* no inclinarse con el pitch de la cámara
* no invertirse
* no cambiar escala
* no cambiar posición del ALU

El cálculo debe ser equivalente a:

```csharp
Vector3 direction =
    cam.transform.position - labelPos;

direction.y = 0f;
```

o su equivalente inverso según la cara frontal real del Canvas.

IMPORTANTE:

No queremos copiar directamente:

```csharp
labelRoot.transform.rotation = cam.transform.rotation;
```

porque eso hace que el Canvas copie también la inclinación de la cámara.

Queremos un billboard horizontal:

```text
              CÁMARA
                 👁
                 │
                 │
          ┌─────────────┐
          │     ALU     │
          └─────────────┘
                 ▲
              etiqueta
```

La etiqueta debe rotar alrededor del eje Y, manteniéndose vertical.

---

# 7. COMPROBACIÓN DE INTERACCIÓN

Después de corregir exclusivamente la orientación de la etiqueta:

En Play Mode:

1. Coloca la cámara en tercera persona normal.
2. Acércate al ALU.
3. Comprueba si aparece:

```text
ALU
Unidad Aritmético-Lógica
[E]
```

4. Si `[E]` NO aparece, NO inventes una solución.
5. Determina si `PlayerInteraction` está detectando al ALU.

Pero NO modifiques `PlayerInteraction` todavía.

Queremos separar:

```text
PROBLEMA 1
Etiqueta invisible

PROBLEMA 2
[E]/interacción no aparece
```

No asumas que ambos problemas tienen la misma causa.

---

# 8. VERIFICACIÓN FINAL

La prueba obligatoria debe ser desde la cámara normal de tercera persona.

Debe verificarse:

### Frente

```text
👁
 \
  🧍
   \
   [ALU]
```

Etiqueta visible.

### Lado izquierdo

```text
👁 → [ALU]
```

Etiqueta visible.

### Lado derecho

```text
[ALU] ← 👁
```

Etiqueta visible.

### Atrás

Etiqueta visible.

### Diagonal

Etiqueta visible.

La etiqueta debe permanecer:

```text
ALU
Unidad Aritmético-Lógica
```

legible.

---

# 9. INFORME FINAL

Al terminar informa:

1. Cuál era realmente la cara frontal del Canvas.
2. Qué dirección estaba usando el billboard anterior.
3. Por qué la etiqueta aparecía desde una orientación pero desaparecía desde la cámara normal.
4. Qué cambio mínimo realizaste.
5. Si ahora la etiqueta aparece desde la tercera persona normal.
6. Si permanece vertical.
7. Si mantiene posición.
8. Si mantiene escala.
9. Si `[E]` aparece.
10. Si la actividad educativa vuelve a activarse.
11. Si hubo errores o warnings nuevos.

NO continúes con:

* Cache
* Registros
* RAM
* Storage

hasta que la ALU esté completamente funcional.
