# AstroBit — Hallazgos pendientes (no arreglar todavía)

Registro de problemas reales encontrados durante la verificación de bloques,
que quedan **documentados pero sin corregir** a propósito (fuera del alcance
del bloque donde se encontraron). No borrar entradas al "solucionarlas" —
marcarlas como resueltas con la fecha y el commit que las arregló.

---

## 1. Auto-bloqueo del raycast de línea de visión en objetos anchos y bajos

**Dónde:** `PlayerInteraction.FindNearestInteractable()`
(`Assets/Scripts/Interaction/PlayerInteraction.cs`).

**Qué pasa:** el chequeo de línea de visión hace
`Physics.Raycast(origin, toPoint.normalized, distToPoint - 0.05f)` desde
`playerTransform.position + Vector3.up` (altura de ojos, ~1 unidad sobre el
suelo) hacia `collider.ClosestPoint(playerTransform.position)`. Para
colliders **anchos/profundos pero de poca altura** — exactamente la forma de
los componentes de CPU (ALU, Cache L1/L2/L3, Unidad de Control: todos con
escala Y entre 0.76 y 1.28, pero X/Z de 5 a 9 unidades) — el punto más
cercano que devuelve `ClosestPoint` puede estar en una cara distinta a la que
el rayo golpea primero si el jugador está muy cerca y el origen (a la altura
de "ojos") queda por encima de la altura del objeto. El rayo entonces
atraviesa/roza el propio collider del objeto antes de llegar al punto
objetivo, y `Physics.Raycast` lo reporta como "bloqueado por el propio
objeto" — el objeto se auto-bloquea a sí mismo.

**Cómo se encontró:** durante la verificación automatizada del Bloque 3
(rebinding), al teletransportar al jugador por código muy cerca de ALU/
CacheL1/CacheL2 para simular una interacción, `FindNearestInteractable()`
devolvía `null` pese a que el objeto estaba en rango y `CanInteract` era
`true`. Confirmado con un chequeo directo: `Physics.Raycast` reportaba
`blocked=True, hit=<el mismo objeto>` en varias posiciones de prueba
distintas alrededor de ALU y CacheL1/CacheL2, a distintas distancias (1.5,
1.8, 2.3, 2.8 unidades), todas dentro del rango de interacción normal.

**Por qué no se arregló ahora:** es un problema preexistente de
`PlayerInteraction` (Bloque 0, muy anterior a los Bloques 1-3), no relacionado
con el Input System ni con el remapeo de controles. Cambiarlo a mitad de la
verificación del Bloque 3 habría mezclado un fix no pedido con el trabajo de
ese bloque.

**¿Afecta a un jugador real?** Probablemente poco o nada en la práctica: un
jugador camina hacia el objeto con el controlador de físicas real
(`CharacterController`), que lo detiene naturalmente contra la superficie del
collider a una distancia y ángulo consistentes con cómo choca la cápsula del
personaje — no con un teletransporte instantáneo a una coordenada exacta como
hice yo para las pruebas. No hay reporte del usuario de que esto pase jugando
normal. Vale la pena confirmarlo con un playtest real acercándose de frente a
la ALU/las Cachés antes de asumir que nunca ocurre.

**Posible arreglo futuro (no implementado):** en vez de raycastear hacia
`collider.ClosestPoint(...)`, apuntar hacia el centro del collider (o hacia
un punto ligeramente más alto, a la misma altura que el origen) evita que el
rayo tenga que descender y rozar la cara superior del propio objeto. Otra
opción: ignorar explícitamente el propio collider del interactuable candidato
al chequear obstrucciones (ya que "bloqueado por el objeto que se quiere
alcanzar" nunca debería contar como bloqueo real).

**Estado:** ⏳ Pendiente. Sin fecha objetivo.

---

## 2. Advertencia de limpieza de `GameInput` al salir de Play Mode

**Dónde:** consola del Editor, al presionar Stop en Play Mode (observado
después de mergear el Bloque 3, pero puede no estar relacionado con ese
bloque específicamente).

**Qué pasa:** al salir de Play Mode aparece una vez:

```
Some objects were not cleaned up when closing the scene. (Did you spawn new
GameObjects from OnDestroy?)
The following scene GameObjects were found:
GameInput
```

**Por qué no se investigó a fondo:** no corrompe la escena guardada
(confirmado: `git status`/`git diff` sobre `SampleScene.unity` no mostró
ningún cambio después de que apareciera este mensaje) y no impidió que el
siguiente Play Mode ni la siguiente compilación funcionaran con normalidad.
Es un mensaje que Unity emite en el Editor para objetos `DontDestroyOnLoad`
creados perezosamente (patrón que usa `GameInput` y el resto de los
singletons del proyecto) cuando el orden exacto de destrucción al salir de
Play Mode no coincide con lo que Unity espera — no necesariamente indica una
fuga de memoria real ni un bug funcional.

**Por qué queda anotado igual:** no lo había visto en las verificaciones de
los Bloques 1 y 2 con los otros singletons (`GameStateManager`, `SaveManager`,
etc.), así que podría ser específico de `GameInput` (por ejemplo, por su
`OnDestroy()`, que limpia `CinemachineCore.GetInputAxis`) y no simplemente
"como le pasa a cualquier singleton perezoso del proyecto". Vale la pena
confirmar si se repite de forma consistente antes de decidir si hace falta
tocar algo.

**Estado:** ⏳ Pendiente de confirmar si es reproducible / si amerita acción.
Sin fecha objetivo.
