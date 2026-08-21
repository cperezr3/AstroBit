# PROMPT 07 — AUMENTAR RADIO DE VISIBILIDAD DE LA ETIQUETA ALU

Proyecto: AstroBit — Unity

## CONTEXTO ACTUAL

La ALU ya tiene implementado:

* `EducationalInteractable`
* `ALU_Label`
* etiqueta flotante Billboard
* sistema de proximidad
* `PlayerInteraction`
* prompt `[E]`
* actividad educativa
* recompensa
* Cinemachine
* cámara de tercera persona

NO quiero rehacer ninguno de esos sistemas.

La corrección anterior de Cinemachine ya fue realizada y la cámara ahora tiene una configuración de tercera persona más apropiada.

## PROBLEMA ACTUAL

El problema que quiero resolver AHORA es únicamente la distancia a la que aparece la etiqueta de la ALU.

En Play Mode ocurre lo siguiente:

```text
                    ALU
                 ┌───────┐
                 │       │
                 └───────┘
                     ↑
                    🧍
                 jugador
                     \
                      \
                       👁
                    cámara
```

Cuando el jugador está relativamente cerca del ALU, pero la cámara permanece en su posición normal de tercera persona, la etiqueta:

```text
ALU
Unidad Aritmético-Lógica
```

NO aparece.

Sin embargo, si giro/manipulo la cámara manualmente y la acerco hasta que prácticamente está mirando directamente al cubo ALU, entonces la etiqueta aparece.

Esto indica que actualmente el sistema de visibilidad/proximidad de `ALU_Label` está utilizando un radio demasiado pequeño o una condición de proximidad que no funciona bien con la distancia normal de la cámara de tercera persona.

## OBJETIVO

Quiero que la etiqueta aparezca cuando **EL JUGADOR** esté cerca del ALU.

NO quiero que sea necesario acercar físicamente la cámara al ALU.

El comportamiento deseado es:

```text
                    ALU
                 ┌───────┐
                 │       │
                 └───────┘
                     ↑
                  etiqueta
                     ↑
                    🧍
                  jugador
                     \
                      \
                       👁
                    cámara
```

Aunque la cámara esté varios metros detrás del jugador, si el jugador está dentro del radio de proximidad del ALU, la etiqueta debe aparecer sobre el ALU.

---

# 1. NO MODIFICAR CINEMACHINE

NO modificar:

* `CM FreeLook1`
* `CinemachineBrain`
* `CameraTarget`
* órbitas
* FOV
* posición de cámara
* rotación de cámara
* `Heading`
* `XAxis`
* `YAxis`
* recentrado

La cámara ya está configurada.

No volver a investigar ni corregir Cinemachine.

---

# 2. INSPECCIONAR PRIMERO EL RADIO ACTUAL

Lee:

```text
Assets/Scripts/Interaction/EducationalInteractable.cs
```

y cualquier otro archivo que `EducationalInteractable` utilice directamente para determinar:

* cuándo aparece `ALU_Label`;
* cuándo desaparece;
* distancia al jugador;
* distancia a la cámara;
* `proximityRadius`;
* `interactionDistance`;
* cualquier threshold relacionado con visibilidad.

NO hagas cambios todavía.

Primero dime exactamente:

```text
¿Qué condición hace que ALU_Label aparezca?
```

y:

```text
¿La distancia se calcula desde el jugador o desde Main Camera?
```

---

# 3. IMPORTANTE: NO CONFUNDIR PLAYER CON CAMERA

La prioridad es que la visibilidad de la etiqueta dependa de:

```text
PLAYER → ALU
```

y no de:

```text
CAMERA → ALU
```

La cámara de tercera persona está deliberadamente alejada del jugador.

Por lo tanto, si existe una condición como:

```csharp
Vector3.Distance(Camera.main.transform.position, transform.position)
```

para decidir si aparece la etiqueta, eso debe ser revisado.

Si actualmente la etiqueta se activa mediante distancia a la cámara, determina si esa es la causa de que solo aparezca cuando acerco la cámara manualmente.

NO cambies todavía el código hasta confirmar esto.

---

# 4. RADIO DESEADO

Una vez identificado el valor actual, aumenta el radio de visibilidad de la etiqueta de forma moderada.

Como primera prueba utiliza aproximadamente:

```text
10 unidades
```

Si actualmente existe algo como:

```csharp
proximityRadius = 6f;
```

el objetivo sería probar:

```csharp
proximityRadius = 10f;
```

NO utilizar valores exagerados como:

```text
50
100
500
```

No queremos que la etiqueta aparezca desde todo el mapa.

El objetivo es simplemente que sea visible cuando el jugador se encuentre razonablemente cerca del ALU.

---

# 5. IMPORTANTE: NO CAMBIAR EL BILLBOARD

La lógica actual de billboard ya fue comprobada.

NO modificar nuevamente:

```csharp
Quaternion.LookRotation(...)
```

NO invertir otra vez el signo.

NO cambiar:

* rotación
* escala
* posición
* Canvas
* TextMeshPro

El problema actual es la distancia de activación/visibilidad.

---

# 6. PLAYERINTERACTION

NO modificar `PlayerInteraction` todavía.

Sé que anteriormente se encontró:

```text
interactionDistance = 3
```

y que el raycast parte de la cámara.

Eso puede ser un problema independiente para `[E]`.

Por ahora quiero solucionar solamente:

```text
PLAYER cerca del ALU
        ↓
ALU_Label visible
```

Después podremos solucionar por separado:

```text
PLAYER cerca del ALU
        ↓
[E]
        ↓
actividad
```

No mezcles ambos problemas.

---

# 7. PRUEBA OBLIGATORIA

Después de identificar el valor actual y modificar únicamente el radio necesario:

Entrar en Play Mode.

Utilizar la cámara normal de tercera persona.

NO mover manualmente la cámara hacia el ALU.

Acercar solamente al jugador.

Probar aproximadamente:

### Prueba 1

Jugador claramente lejos del ALU.

Resultado esperado:

```text
ALU_Label = oculto
```

### Prueba 2

Jugador acercándose al ALU.

Cuando entre al radio configurado:

```text
ALU_Label = visible
```

Debe aparecer:

```text
ALU
Unidad Aritmético-Lógica
```

sobre el cubo.

### Prueba 3

Mantener la cámara aproximadamente 5–7 unidades detrás del jugador.

El jugador permanece cerca del ALU.

Resultado esperado:

```text
ALU_Label = visible
```

aunque la cámara no esté pegada al cubo.

### Prueba 4

Alejar al jugador nuevamente.

Cuando salga del radio:

```text
ALU_Label = oculto
```

---

# 8. NO CAMBIAR EL TAMAÑO DEL ALU

NO modificar:

* Scale X
* Scale Y
* Scale Z
* Collider
* posición
* rotación

del ALU.

El tamaño actual del cubo es correcto.

El objetivo es solucionar la distancia de activación, no cambiar la geometría del escenario.

---

# 9. SI LA DISTANCIA YA SE CALCULA DESDE EL JUGADOR

Si descubres que `EducationalInteractable` ya utiliza correctamente:

```text
PLAYER → ALU
```

entonces NO cambies el cálculo.

En ese caso simplemente determina por qué el radio actual no alcanza la distancia que estamos utilizando en tercera persona y aumenta únicamente ese threshold.

---

# 10. SI LA ETIQUETA SE ACTIVA PERO NO SE VE

Si después de aumentar el radio:

```text
labelActive = true
```

pero el texto sigue sin aparecer visualmente, entonces NO hagas otro cambio inmediatamente.

Informa:

```text
El sistema sí activa ALU_Label,
pero el Canvas sigue sin renderizarse.
```

y detente.

No vuelvas a modificar Cinemachine ni el billboard.

---

# 11. VERIFICACIÓN FINAL

Al terminar informa:

1. Qué variable controlaba originalmente la aparición de `ALU_Label`.
2. Cuál era su valor original.
3. Si la distancia se calculaba desde jugador o cámara.
4. Qué valor nuevo utilizaste.
5. Si la etiqueta ahora aparece desde la cámara normal de tercera persona.
6. Si aparece mientras el jugador está cerca del ALU sin acercar manualmente la cámara.
7. Si desaparece al alejarse.
8. Si modificaste algún otro archivo.
9. Si hubo errores o warnings nuevos.

## RESTRICCIÓN FINAL

No modificar:

* Cinemachine
* PlayerInteraction
* MovementInput
* CharacterSkinController
* GameHUD
* ObjectiveSystem
* Input System
* ProjectSettings
* ALU Transform
* ALU Collider
* jugador
* cámara

Solo modificar el **radio/condición de visibilidad de `ALU_Label`**, si el diagnóstico confirma que ese es el problema.

No continuar con:

* Cache
* Registros
* RAM
* Storage

La prioridad ahora es:

```text
JUGADOR CERCA DEL ALU
        ↓
ALU_Label aparece
        ↓
visible desde tercera persona normal
```
