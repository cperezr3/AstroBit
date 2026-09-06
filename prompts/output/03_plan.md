# AstroBit — Plan de mejora priorizado (Fase 1, Prompt 08 "plan y assets")

Basado en `prompts/output/02_diagnostico.md`, confirmado por el usuario. No se
re-diagnostica nada aquí — este documento solo prioriza y detalla el "cómo" de
cada hueco ya confirmado. Ningún punto de este plan se implementa todavía
(Fase 2 pendiente de tu confirmación).

## Resumen ejecutivo (orden de ataque recomendado)

| # | Bloque | Esfuerzo | Beneficio | Por qué este orden |
|---|---|---|---|---|
| 1 | A.2 Descomponer `GameHUD` | Medio | Medio-Alto | ✅ **COMPLETADO** (Prompt 09, Bloque 1, rama `feature/arquitectura-hud-estados`) — ver detalle abajo |
| 2 | A.1 Máquina de estados | Medio-Alto | Alto | ✅ **COMPLETADO** (Prompt 09, Bloque 1, misma rama) — ver detalle abajo |
| 3 | B.1 Migrar movimiento a Input System | Medio | Alto | ✅ **COMPLETADO** (Prompt 10, Bloque 2, rama `feature/input-system-unificado`) — ver detalle abajo |
| 4 | C. Audio (SFX) | Medio | Alto | Gancho más barato y más citado como "se siente vacío"; los canales de volumen ya existen sin nada que reproducir |
| 5 | B.2 + B.3 Gamepad + Remapeo | Medio-Alto | Alto | ✅ **COMPLETADO** (Prompt 10 Bloque 2 para B.2; Prompt 06 Bloque 3 para B.3, rama `feature/ui-remapeo-controles`) — ver detalle abajo |
| 6 | D. Variedad de interacción | Medio (x cada una) | Alto | Alto impacto en percepción de calidad, pero requiere que el HUD (1) y el input (3) ya estén estables |
| 7 | A.3 Auditoría de singletons | Medio | Medio | Limpieza estructural, no bloquea nada externo — se hace en paralelo o después de (1)/(2) |
| 8 | B.4 + B.5 Alto contraste + escalado UI | Medio | Medio | Depende de (1) (HUD descompuesto) para no tocar 720 líneas de una vez |
| 9 | A.4 ScriptableObjects | Medio-Alto | Alto a largo plazo, bajo si no hay sala nueva | Solo si Fase 2/3 va a añadir una sala nueva (GPU/Placa Madre/etc.) — si no, se pospone explícitamente para no refactorizar sin necesidad |
| 10 | E. Rendimiento | Bajo (ahora) | Preventivo | Sin acción urgente; checklist para cuando se agregue contenido nuevo |

---

## A. Arquitectura

### A.1 Máquina de estados formal

> ✅ **COMPLETADO (Prompt 09, Bloque 1).** Implementado como se describe abajo,
> con dos ajustes reales sobre el diseño original (ver el resumen del bloque al
> final de este documento para el detalle completo): el enum quedó
> `{ MainMenu, Playing, Paused }` (sin `Boot`/`Cutscene`/`Complete`, que no
> tenían un uso real todavía), y `CameraSensitivityController`/
> `WorldObjectiveMarker` se dejaron sin tocar tras verificar que no mantenían
> estado de menú/pausa propio.

**Qué haré:** una clase `GameStateManager` (singleton real, ver A.3) con un
`enum GameState { Boot, MainMenu, Playing, Paused, Cutscene, Complete }`, un
`UnityEvent<GameState> OnStateChanged`, y métodos explícitos
(`EnterMainMenu()`, `StartNewGame()`, `Pause()`/`Resume()`, `MarkComplete()`)
que reemplazan:
- `GameSession.HasActiveGame` (bool estático) → pasa a ser parte del estado.
- `PauseMenuController.isPaused` + `Time.timeScale` manual → lo centraliza
  `GameStateManager.Pause()/Resume()`, `PauseMenuController` solo dibuja UI y
  llama a esos métodos.
- Las comprobaciones repetidas `SceneManager.GetActiveScene().name != "MainMenu"`
  en `GameHUD`, `MinimapController`, `CameraSensitivityController`, `MissionUI`,
  `WorldObjectiveMarker`, `PauseMenuController` → se suscriben a
  `OnStateChanged` una sola vez en su `Awake()`.

**Por qué:** hoy ese estado vive repartido en 6+ clases con el mismo patrón
copiado (documentado como fuente de al menos 2 bugs ya corregidos en el
historial — el indicador de ubicación que no se reseteaba, el marcador de
mundo que se destruía al cambiar de escena). Centralizarlo lo hace imposible
por construcción en vez de por convención, y es la base que necesita todo lo
demás (SFX de transición, rebind, nuevas mecánicas) para enganchar sin repetir
el mismo patrón una vez más.

**Esfuerzo:** medio-alto (toca ~8 archivos, pero de forma mecánica: mover un
`if` a un listener, no cambiar lógica de negocio). **Beneficio:** alto.

### A.2 Descomponer `GameHUD.cs`

> ✅ **COMPLETADO (Prompt 09, Bloque 1).** Implementado tal cual se describe
> abajo, más un quinto archivo no planeado (`HUDText.cs`, helper estático
> compartido) — ver el resumen del bloque al final de este documento.

**Qué haré:** dividir las ~720 líneas actuales en componentes de
responsabilidad única, todos siguiendo el mismo patrón ya usado en el proyecto
(construcción de UI en código + `RuntimeInitializeOnLoadMethod`):
- `HUDObjectiveDisplay` — texto de objetivo/pista/ubicación/progreso.
- `HUDPrompt` — el badge `[E] Acción` animado.
- `HUDFeedbackBanner` — mensajes flotantes temporales.
- `HUDModalPanel` — el sistema de panel de 4 modos (Info/Activity/Choice/
  Reward), que es el que más se reutiliza (`EducationalInteractable`,
  `FinalActivity`, `StorageMission`).
- `GameHUD` queda como fachada delgada: compone los 4 componentes de arriba y
  conserva exactamente la misma API pública (`ShowPrompt`, `ShowFeedback`,
  `ShowEducationalPanel`, etc.) para que **ningún llamador externo cambie una
  sola línea** (`SimpleInteractable`, `EducationalInteractable`,
  `FinalActivity`, `StorageMission`, `PlayerInteraction`, `LocationZone`).

**Por qué:** es el mayor God Object del proyecto; cualquier cambio de panel
hoy obliga a leer 720 líneas para no romper algo. B.4/B.5 (alto contraste,
escalado de UI) y C (SFX en paneles) necesitan tocar exactamente estas zonas —
mejor dividir antes de añadir más responsabilidades encima.

**Esfuerzo:** medio (extracción mecánica, riesgo bajo si se preserva la
fachada). **Beneficio:** medio-alto, y es prerequisito práctico de A.1, B.4,
B.5 y C.

### A.3 Auditoría de singletons — qué queda singleton y qué no

> ⏳ **PENDIENTE — no incluido en el Bloque 1.** `MinimapController`/
> `MissionUI`/`GameHUD` ahora escuchan `GameStateManager.OnStateChanged` para
> su visibilidad (en vez de comparar el nombre de escena), pero siguen siendo
> `DontDestroyOnLoad` para siempre, tal como estaban. La recomendación de
> abajo (que `GameStateManager` los cree/destruya en vez de que se oculten
> solos) no se tocó en este bloque — es un cambio de ciclo de vida más
> arriesgado que la sola migración de visibilidad, y no era el foco de este
> prompt. Queda para un bloque futuro si se confirma.

**Quedan como singleton real (`DontDestroyOnLoad`, estado de progreso o
servicio global que debe sobrevivir a cualquier cambio de escena):**
`SettingsManager`, `SaveManager`, `MusicManager`, `GameStateManager` (nuevo,
A.1), `ObjectiveSystem`, `StorageMission`, `Inventory`, `FinalActivity`. Todos
guardan estado que `SaveManager` necesita leer/escribir o son servicios
verdaderamente globales (audio, configuración).

**Dejan de auto-persistir para siempre y pasan a ser creados/destruidos por
`GameStateManager` al entrar/salir del estado `Playing`:**
`MinimapController`, `WorldObjectiveMarker`, `MissionUI`, `MissionNavigation`.
Hoy ya se ocultan solos en `MainMenu` (chequeando el nombre de la escena) —
con la máquina de estados no necesitan existir fuera de `Playing`, así que no
tiene sentido que sean `DontDestroyOnLoad` eternos.

**Se pliegan dentro de otro sistema en vez de ser su propio singleton:**
`CameraSensitivityController` → su lógica (buscar el `CinemachineFreeLook` y
aplicar sensibilidad/inversión) se mueve al pipeline de `Apply()` que ya tiene
`SettingsManager`, evitando un singleton solo para reenviar un evento.

**Quedan singleton porque solo tiene sentido una instancia, pero pasan a ser
propiedad de `GameStateManager` en vez de autoarrancar por
`RuntimeInitializeOnLoadMethod`:** `GameHUD` (ya descompuesto por A.2),
`SettingsUI`, `PauseMenuController`, `GameCompleteScreen`. Esto elimina los
comentarios de "orden de bootstrap frágil" repetidos en el código actual.

**Esfuerzo:** medio. **Beneficio:** medio — no es visible para el jugador,
pero reduce el grafo de estado ambiente y hace imposibles por construcción los
bugs de orden de inicialización que el código ya documenta haber sufrido.

### A.4 ScriptableObjects

**Qué haré (si se confirma que Fase 2/3 añadirá contenido nuevo — ver nota):**
- `RoomProgressionStepSO` — reemplaza el array hardcodeado
  `ObjectiveSystem.Sequence` (key/objetivo/pista por paso).
- `InstallableModuleSO` — generaliza el módulo de RAM de `StorageMission`
  (hoy: constantes `RamItemId`/`RequiredRamModules`) para poder añadir otros
  tipos de módulo instalable sin tocar código.
- `SfxLibrarySO` — registro de clips por categoría, consumido por el nuevo
  `AudioManager` de C.
- `FinalQuestionSetSO` — las 4 preguntas de `FinalActivity`.

**Nota importante:** el formato de guardado (`SaveData`) ya usa claves de
texto (`achievedKeys: List<string>`), así que migrar a SO es compatible con
las partidas guardadas existentes si los IDs de los SO coinciden con las
claves actuales ("ALU", "RAM1", etc.) — no rompe saves.

**Por qué esperar:** el propio proyecto tiene como regla explícita "no
reemplaces sistemas funcionales sin necesidad" (`06_Astrobit_roadmap.md`,
regla 5). Migrar `ObjectiveSystem`/`StorageMission` a datos es la inversión
correcta **solo si** se va a añadir una sala nueva pronto — si el alcance de
Fase 2 es únicamente pulir lo existente (B, C, D, E de este plan), este punto
es refactor especulativo y se pospone. Recomiendo confirmar el alcance antes
de tocarlo. El único sub-punto que sí vale la pena adelantar sin condición es
`SfxLibrarySO`, porque C lo necesita de todas formas.

**Esfuerzo:** medio-alto si se hace completo; bajo si solo se hace
`SfxLibrarySO`. **Beneficio:** alto a largo plazo, especulativo si no hay sala
nueva confirmada.

---

## B. Input unificado y accesibilidad

### B.1 Migrar movimiento al Input System

> ✅ **COMPLETADO (Prompt 10, Bloque 2).** Implementado tal como se describe
> abajo, con el script llamado `PlayerMovementController` (no
> `PlayerInputController`) y una decisión adicional no especificada aquí:
> `MovementInput` no se quitó de `Jammo_Player`, se dejó deshabilitado (ver
> el resumen del bloque al final de este documento).

**Restricción real a resolver primero:** `MovementInput.cs`
(`Assets/Jammo-Character/`) es un asset vendored de solo lectura por
convención del proyecto, y sus campos `InputX`/`InputZ` se sobrescriben cada
`Update()` con `Input.GetAxis(...)` — no se pueden alimentar desde afuera sin
tocar ese archivo. **Propuesta:** no editar el vendor; crear un script nuevo
`PlayerInputController` (o similar) que replique exactamente la misma
matemática de movimiento (lectura de ejes, rotación hacia la dirección de
movimiento, `CharacterController.Move`, el mismo `Animator.SetFloat("Blend",
...)`) pero leyendo de un `InputActionAsset` en vez de `Input.GetAxis`, y
sustituirlo en `Jammo_Player` en lugar de `MovementInput`
(`CharacterSkinController` no se toca). Es un reemplazo aditivo, no una
edición del vendor.

**Por qué:** requisito duro de B.2 y B.3, y elimina el warning de consola de
Input Manager deprecado.

**Esfuerzo:** medio (la lógica es simple pero exige verificar que la
sensación de movimiento no cambie — probar en Play Mode antes/después).
**Beneficio:** alto.

### B.2 Soporte de gamepad

> ✅ **COMPLETADO (Prompt 10, Bloque 2).** Implementado, pero no vía
> `CinemachineInputProvider` como sugería este punto — ver el resumen del
> bloque al final de este documento para el mecanismo real usado
> (`CinemachineCore.GetInputAxis` reemplazado directamente).

Una vez unificado el input (B.1): mapear movimiento/cámara a los sticks
(Cinemachine ya soporta un Input Provider del Input System para el
`FreeLook`) y `[E]`/`Esc` a botones de mando (Sur/A para interactuar, Start
para pausa). **Esfuerzo:** bajo-medio una vez hecho B.1. **Beneficio:** alto.

### B.3 Pantalla de remapeo de controles

> ✅ **COMPLETADO (Prompt 06, Bloque 3).** Implementado dentro de una nueva
> pestaña "Controles" en `SettingsUI` (que se reorganizó en pestañas para
> esto) — ver el resumen del bloque al final de este documento para el
> alcance exacto de qué es remapeable y qué no.

UI de rebinding usando las extensiones del propio Input System
(`InputActionRebindingExtensions`, "presiona una tecla para reasignar"),
guardado con `action.SaveBindingOverridesAsJson()` en `PlayerPrefs` — mismo
patrón de persistencia que ya usa `SettingsManager`. Vive como pestaña nueva
dentro de `SettingsUI` (tras A.2, para no seguir creciendo el mismo archivo
sin dividir). **Esfuerzo:** medio-alto (UI nueva + validación de conflictos
de binding). **Beneficio:** alto.

### B.4 Modo alto contraste / amigable con daltonismo

Los colores de UI hoy son constantes `Color` repetidas en ~8 clases
(`AccentCyan`, etc.). Propuesta: un `UIThemeSO` (ScriptableObject, ver A.4)
con 2-3 paletas (Normal / Alto Contraste / Protanopia-friendly) que cada
componente de UI lee en vez de tener el color hardcodeado, seleccionable
desde `SettingsUI`. Los colores de emisión en el mundo 3D
(`EmissiveToggle`/`MissionBeacon`) quedan fuera de este punto (menor
prioridad, más arriesgado tocar por sala). **Esfuerzo:** medio.
**Beneficio:** medio — accesibilidad real, alcance acotado a la UI 2D.

### B.5 Escalado de tamaño de UI/texto

Slider nuevo en Configuración que multiplica el `referenceResolution` (o un
factor de escala) de cada `CanvasScaler` ya existente. Simplificación de
alcance: aplicar el cambio la próxima vez que se construye cada Canvas (no
en caliente) para no complicar cada una de las 8 clases de UI con
recomputación dinámica. **Esfuerzo:** medio. **Beneficio:** medio.

---

## C. Audio

**Qué haré:** un `AudioManager` ligero + `SfxLibrarySO` (clips con nombre:
`UIClick`, `UIConfirm`, `UIError`, `InteractSuccess`, `InteractDeny`,
`ObjectiveComplete`, `RoomTransition`), expuesto como
`AudioManager.PlaySfx(SfxId.X)`, que respeta `SettingsManager.SfxVolume` /
`UiVolume` exactamente con el mismo patrón que ya usan `MusicManager` y
`AmbientAudioSource` (multiplicador propio, sin tocar `AudioListener.volume`
dos veces). Puntos de enganche concretos: `SimpleInteractable.Interact`,
`EducationalInteractable` (abrir/cerrar panel, respuesta correcta/incorrecta),
`FileShelf` (acierto/error), hitos de `StorageMission`,
`ObjectiveSystem.CompleteObjective`, botones de `PauseMenuController`/
`MainMenuController`.

**Por qué:** los canales "Efectos" e "Interfaz" ya existen en
`SettingsManager`/`SettingsUI` y hoy no reproducen absolutamente nada — es el
punto de la lista con más beneficio percibido por menos riesgo estructural.

**Esfuerzo:** medio — la plumbing es simple y calca un patrón que ya existe
tres veces en el proyecto; el costo real es conseguir los clips (ver Fase
1.5, `03_assets_recomendados.md`). **Beneficio:** alto.

---

## D. Variedad de interacción

No se reemplaza ninguna mecánica existente ni se crea una progresión
paralela — cada propuesta sigue llamando a los mismos métodos de
`StorageMission`/`ObjectiveSystem` que ya son la fuente de verdad; solo cambia
la capa de entrada/presentación delante de esa llamada.

1. **RAM — "puzzle de pines" al instalar un módulo.** `InstallRamSlot` deja de
   instalar al primer `[E]`: exige alinear 2-3 indicadores en secuencia
   (mini-QTE simple, reutilizando el panel modal ya existente) antes de
   llamar a `StorageMission.ReportRamModuleInstalled(...)`. Encaja con RAM
   porque el propio roadmap ya la describe como "memoria activa,
   conexiones". **Esfuerzo:** medio.
2. **CPU — mini-mecánica de temporización ("overclock").** Interactuable
   nuevo junto a la Unidad de Control: mantener presionado para cargar un
   gauge radial y soltar dentro de una ventana móvil; recompensa de sabor
   (no bloquea progreso si falla, coherente con el diseño "sin fallo duro"
   actual). Es literalmente la mecánica que el roadmap ya proponía como
   identidad de la sala CPU y nunca se implementó. **Esfuerzo:** medio.
3. **Almacenamiento — terminal de filtrado antes de ir al shelf físico.**
   Se conserva el recorrido físico (`FileShelf`) como mecánica de
   "explorar", pero se añade una pantalla (reutilizando el vidrio del "Tv 32
   Inch"/`EmissiveToggle` ya existente) donde el jugador filtra/ordena una
   lista corta antes de comprometerse a un estante — reutiliza el panel de
   opción múltiple de `GameHUD` (`ShowChoicePanel`), no crea UI nueva desde
   cero. **Esfuerzo:** medio.
4. **Disco Duro — cursor/láser de sectores.** Se deja documentada como
   propuesta para cuando esa sala exista (no implementar contra una sala que
   hoy no está construida — evita trabajo especulativo).

**Esfuerzo total:** medio por cada una (3 reales + 1 diferida).
**Beneficio:** alto — es el punto más repetido sin resolver en los prompts
anteriores y el que más cambia la percepción de "juego" vs "galería de
paneles".

---

## E. Rendimiento (preventivo, no urgente)

- Marcar como `isStatic` toda la geometría no interactiva que nunca se mueve
  (paredes/techos/pisos/módulos de corredor) — habilita batching estático de
  URP automáticamente. Mecánico y de bajo riesgo; recomendable hacerlo con un
  pequeño script de Editor dado el volumen (~5800 objetos), no a mano.
- No combinar mallas ni hacer GPU instancing de los ~40 `server (N)` / ~18
  `Corridor Passthrough Lights` **todavía** — medir primero con el Profiler /
  Frame Debugger una vez que el contenido de B/C/D esté encima, siguiendo la
  regla ya existente del proyecto ("no optimizar a ciegas").
- Postergar el horneado de lightmaps hasta que la paleta de alto contraste
  (B.4) y la vestimenta de sala nueva (assets de Fase 1.5) estén decididas,
  para no hornear dos veces.
- Ninguna acción requerida ahora mismo — esta sección es un checklist para
  cuando Fase 2/3 agregue contenido, no una tarea de esta fase.

---

## Cierre

Este plan no toca código todavía. Quedo a la espera de tu confirmación de
alcance (en particular: ¿A.4 completo o solo `SfxLibrarySO`? ¿las 3 mecánicas
de D o menos?) antes de empezar la Fase 2 (implementación).

---

## Bloque 1 — Arquitectura base (A.1 + A.2): COMPLETADO

Implementado en `prompts/09_implementacion_bloque1.md`, rama
`feature/arquitectura-hud-estados` (commits `e3122c4`, `fe6a47c`, `bd3ebcf`,
`b443982` sobre `main`).

**Clases nuevas:**
- `GameStateManager` (`Assets/Scripts/Gameplay/GameStateManager.cs`) — enum
  `GameState { MainMenu, Playing, Paused }`, `OnStateChanged`,
  `StartNewGame()`/`ContinueGame()`/`Pause()`/`Resume()`/`RestartSection()`/
  `ReturnToMenu()`.
- `HUDPrompt`, `HUDFeedbackBanner`, `HUDObjectiveDisplay`, `HUDModalPanel`
  (`Assets/Scripts/UI/`) — los 4 componentes extraídos de `GameHUD`.
- `HUDText` (`Assets/Scripts/UI/HUDText.cs`) — helper estático no planeado
  originalmente: unifica el `CreateText` que `GameHUD.cs` repetía 7 veces, usado
  por `HUDObjectiveDisplay` y `HUDFeedbackBanner`.

**Clases eliminadas:**
- `GameSession.cs` (reemplazada por `GameStateManager`).

**Clases modificadas (migradas a `GameStateManager`, sin cambiar su
comportamiento observable):**
- `MainMenuController` — `Jugar()`/`Continuar()` delegan en
  `GameStateManager.StartNewGame()/ContinueGame()`.
- `PauseMenuController` — ya no mantiene `isPaused`/`Time.timeScale` propios;
  refleja `GameStateManager.OnStateChanged` en su panel y en
  `PlayerInteraction`/`MovementInput`.
- `GameHUD` — pasó de ~720 líneas a fachada de ~110 líneas.
- `MinimapController` / `MissionUI` — visibilidad ahora deriva de
  `GameStateManager.OnStateChanged` en vez de comparar el nombre de escena
  directamente. `MissionUI` no tenía ningún mecanismo de ocultamiento propio
  antes de este bloque (hueco real encontrado durante la implementación, no
  solo refactor — ver "Decisiones tomadas sobre la marcha" en el resumen que
  se entregó en el chat).
- `FinalActivity`, `Inventory`, `StorageMission` — solo comentarios
  actualizados (referenciaban a `GameSession`, ahora referencian
  `GameStateManager`).

**Verificado en Play Mode vía UnityMCP** (`MainMenu` → Nueva Partida con
guardado previo → diálogo de confirmación → `SampleScene` → interacción real
con ALU vía `EducationalInteractable` → panel/recompensa/feedback/progreso →
Pausa/Reanudar → Volver al Menú → Continuar → Reiniciar): todos los estados,
visibilidades de Canvas y transiciones de escena se comportaron igual que
antes del refactor. Sin errores de compilación ni excepciones nuevas en
consola. Se encontraron 2 problemas preexistentes **no relacionados con este
bloque** (detallados en el resumen del chat): `PlayRandomSound` con un
`AudioClip` sin asignar, y el carácter "✓" faltante en el fallback de TMP.

No se avanzó al Bloque 2 (Input System unificado) — pendiente de
confirmación.

---

## Bloque 2 — Input System unificado (B.1 + B.2): COMPLETADO

Implementado en `prompts/10_implementacion_bloque2.md`, rama
`feature/input-system-unificado` sobre `main` ya actualizado (Bloque 1
mergeado + los 2 fixes de bugs), commits `117c1a5`, `8666b19`, `41a38e7`.

**Clases nuevas:**
- `GameInput` (`Assets/Scripts/Gameplay/GameInput.cs`) — singleton que carga
  `Resources/AstroBitControls.inputactions` y expone `MoveAction`/
  `LookAction`/`InteractAction`/`PauseAction`. También reemplaza
  `Cinemachine.CinemachineCore.GetInputAxis` (el delegate estático que por
  defecto llama a `Input.GetAxis("Mouse X"/"Mouse Y")`) por una función propia
  respaldada en el Input System — esto integra la cámara sin agregar
  `CinemachineInputProvider` ni `InputActionReference` al FreeLook, y sin
  tocar `CameraSensitivityController` (que sigue escalando `m_MaxSpeed`/
  `m_InvertInput` exactamente igual, sin que le importe la fuente del valor).
- `PlayerMovementController` (`Assets/Scripts/Gameplay/`) — reemplaza a
  `MovementInput` como el componente que mueve al jugador.
- `Resources/AstroBitControls.inputactions` — mapa "Player": Move (Vector2:
  WASD/flechas + stick izquierdo), Look (Vector2: stick derecho; el mouse se
  lee directo por separado, ver decisiones abajo), Interact (E + botón sur),
  Pause (Escape + Start).

**Clases modificadas:**
- `PlayerInteraction`, `PauseMenuController` — el chequeo de tecla pasó de
  `Keyboard.current.eKey`/`escapeKey` directo a `GameInput.InteractAction`/
  `PauseAction`, agregando el equivalente de mando sin duplicar lógica.
- `PauseMenuController.SetPlayerControlEnabled` — alterna
  `PlayerMovementController.enabled` en vez de `MovementInput.enabled` (ver
  decisiones abajo, es necesario para no correr movimiento dos veces).

**Escena (`SampleScene.unity`), sobre `Jammo_Player`:**
- `MovementInput` (vendored) queda con `m_Enabled: 0` — no se quita el
  componente.
- `PlayerMovementController` agregado y habilitado, con los mismos valores
  tuneados a mano que ya tenía `MovementInput` (Velocity 10,
  desiredRotationSpeed 0.3).

**Hallazgo no obvio encontrado y resuelto (Prompt 10, sección 6):** la cámara
(`CM FreeLook1`) también dependía del Input Manager legado de forma implícita
— no vía código propio del repo, sino porque Cinemachine, al no tener
`CinemachineInputProvider`, usa por defecto `Input.GetAxis("Mouse X"/"Mouse
Y")` internamente. Esto no aparecía en ningún grep de `Input.GetAxis` sobre
`Assets/Scripts` porque la dependencia vive dentro del propio paquete
Cinemachine, activada solo por el nombre de eje configurado en el Inspector
del FreeLook. Resuelto sin tocar el vcam (ver `GameInput` arriba).

**Hallazgo documentado, NO resuelto en este bloque (Prompt 10, sección 6):**
`CharacterSkinController` (`Assets/Jammo-Character/`, vendored, activo en
`Jammo_Player`) usa `Input.GetKeyDown(KeyCode.Alpha1..4)` para cambiar la
expresión facial del robot — una función de debug/demo del asset original,
no documentada en ningún prompt previo como mecánica real de AstroBit. Sigue
funcionando (no se tocó), pero es la razón por la que **no se puede** cambiar
`Active Input Handling` de "Both" a "Input System Package (New)" todavía: ese
cambio haría que las llamadas `Input.*` de este script lancen una excepción
en cada frame. El warning de consola "Input Manager deprecated" seguirá
apareciendo hasta que se decida qué hacer con esta función (quitarla,
reemplazarla, o aceptar convivir con "Both" indefinidamente) — **pregunto:
¿querés que la quite/reemplace en un futuro bloque, o la dejamos como está?**

**Decisiones tomadas sobre la marcha (no 100% especificadas en el plan):**
1. **`MovementInput` no se elimina, se deshabilita.** Removerlo habría roto
   las ~5 llamadas existentes a `FindFirstObjectByType<MovementInput>()`
   (`WorldLabel`, `EducationalInteractable`, `PlayerInteraction`,
   `MinimapController`, `MissionNavigation`) que solo necesitan el
   `Transform` del jugador — un componente deshabilitado sigue siendo
   encontrado por `FindObjectByType`, solo deja de correr su propio
   `Update()`. Evita tocar esos 5 archivos.
2. **Cámara integrada reemplazando `CinemachineCore.GetInputAxis` en vez de
   usar `CinemachineInputProvider`.** Más simple y con cero cambios de
   escena: no requiere generar un `InputActionReference` como asset ni
   agregar un componente al vcam. Mismo resultado funcional.
3. **El mouse-look se lee directo de `Mouse.current.delta`, no a través de
   una acción del Input System.** Solo el stick derecho del gamepad está
   definido como acción "Look" en el asset. Evita ambigüedad de cómo el
   Input System resolvería dos bindings Vector2 sin componer apuntando a la
   misma acción (mouse delta y stick), y de todos modos remapear "el mouse"
   no es algo que la mayoría de los juegos ofrezcan (la sensibilidad, que sí
   es configurable, ya existe desde antes).
4. **La rampa de aceleración de `Horizontal`/`Vertical` del Input Manager
   legado (gravity/sensitivity 3, ~1/3s para llegar a velocidad plena) no se
   replicó byte a byte.** El Input System no tiene un equivalente directo
   para un composite de botones; se aproximó con
   `Vector2.MoveTowards(smoothedMove, rawMove, 3f * Time.deltaTime)`, que da
   un ritmo de rampa muy similar sin tener que leer botones individuales y
   reimplementar el algoritmo de snap/gravity del Input Manager. Diferencia
   menor, documentada — recomiendo un playtest manual para confirmar que se
   siente igual.
5. **No se cambió `ProjectSettings` → `Active Input Handling`.** Sigue en
   "Both" a propósito, por el hallazgo de `CharacterSkinController` arriba.

**Verificado en Play Mode vía UnityMCP** (simulando dispositivos con
`InputSystem.QueueStateEvent`, no un teclado/mando físico): `GameInput` carga
el asset y expone las 4 acciones correctamente; presionar W mueve
efectivamente al `CharacterController` (posición cambió, sin choque con la
pseudo-gravedad); `CinemachineCore.GetInputAxis("Mouse X"/"Mouse Y")`
devuelve el valor esperado tanto para delta de mouse simulado (con el factor
0.1 aplicado) como para un gamepad virtual conectado (stick derecho, sin
escalar); el mismo gamepad virtual dispara `InteractAction`/`PauseAction`
correctamente. **Limitación de esta verificación, no del código:** no pude
confirmar de forma confiable el flanco de un solo frame
(`WasPressedThisFrame()`) para teclado E/Escape específicamente por una
limitación del propio método de prueba (inyectar eventos desde fuera del
bucle de frames de Unity vía `execute_code` no se alinea de forma
determinística con la ventana de un frame) — el mismo problema de medición
ocurre igual con el código viejo (`Keyboard.current.eKey.wasPressedThisFrame`
tampoco es medible así), así que no es una regresión, pero sí falta un
playtest manual con teclado real para cerrar esa duda al 100%.

No se avanzó al Bloque 3 — pendiente de confirmación.

---

## Bloque 3 — UI de remapeo de controles (B.3): COMPLETADO

Implementado a partir del prompt dado en el chat ("Prompt 06 — Implementación
Fase 2, Bloque 3: UI de remapeo de controles" — no se guardó como archivo en
`prompts/`, a diferencia de los bloques anteriores), rama
`feature/ui-remapeo-controles` sobre `main` ya actualizado (Bloques 1 y 2
mergeados), commits `c53c684`, `f4894b0`, `0716d41`.

**Clases nuevas:**
- `ControlsRebindingPanel` (`Assets/Scripts/UI/`) — construye las filas de
  reasignación dentro de la pestaña "Controles" y traduce clicks a
  `InputActionRebindingExtensions.PerformInteractiveRebinding`.

**Clases modificadas:**
- `GameInput` — `SaveBindingOverrides()`/`ResetBindingOverrides()` (PlayerPrefs,
  no el JSON de `SaveManager` — ver decisión 1 abajo), `LoadBindingOverrides()`
  llamado en `Init()`, evento `OnBindingsChanged`.
- `SettingsUI` — reorganizado en 3 pestañas (Audio/Controles/Gráficos) en vez
  de una sola columna vertical; el comportamiento de Audio y Gráficos no
  cambió, solo dónde vive en el layout.

**Alcance real de qué es remapeable (decisión de diseño, ver más abajo):**
Mover (solo tecla primaria WASD, no las flechas), Interactuar (teclado +
mando), Pausa (teclado + mando). Mirar y el stick del mando para moverse
quedan fijos/informativos — no tiene sentido remapear "cuál stick es cuál" en
un mando estándar, y el mouse-look ni siquiera pasa por una acción del Input
System (ver `GameInput.GetCinemachineInputAxis`, Bloque 2).

**Verificado en Play Mode vía UnityMCP:** abrir Configuración desde el Main
Menu, cambiar entre las 3 pestañas (Audio se muestra por defecto,
Controles/Gráficos ocultan las demás) — confirmado con screenshots reales,
sin overflow. Reasigné "Interactuar (teclado)" de E a F simulando la tecla
física: el binding se actualiza a "F" en la UI, `PlayerPrefs` guarda el JSON
de overrides correcto (`"path":"<Keyboard>/f"`), y confirmé que **presionar E
ya no abre el panel educativo** (la acción realmente dejó de responder a la
tecla vieja). Lo que **no pude confirmar de forma limpia** es que F
efectivamente abra el panel extremo a extremo en esta sesión de pruebas: la
combinación de la limitación de foco de ventana (decisión 5) con un problema
geométrico preexistente y no relacionado con este bloque (el raycast de
línea de visión de `PlayerInteraction.FindNearestInteractable()` puede
autobloquearse contra la propia caja del componente CPU cuando el jugador
queda muy cerca/ligeramente incrustado tras un teletransporte de prueba,
algo que no ocurre caminando normalmente) hizo que mis intentos de acercarme
por código a un componente y confirmar la apertura del panel fueran poco
confiables. La acción en sí (binding, habilitación, ausencia de conflicto)
quedó verificada por otras vías directas. Probé además un conflicto real:
reasignar "Pausa (teclado)" a W (ya usado por "Arriba") — se revirtió solo a
"Esc" sin tocar el binding de Arriba, confirmando la detección de conflictos.
"Restaurar valores por defecto" volvió todo a los bindings originales y borró
la entrada de PlayerPrefs.

**Decisiones tomadas sobre la marcha (no 100% especificadas en el prompt):**
1. **Persistencia via `SettingsManager`/`PlayerPrefs`, no `SaveManager`/JSON
   de progreso.** El prompt decía "revisa cómo `SaveManager` guarda otras
   preferencias" pero `SaveManager` no guarda ninguna preferencia —
   `SettingsManager` sí, vía `PlayerPrefs` (volumen, sensibilidad, invertir Y,
   gráficos). Seguí ese patrón real en vez del nombrado en el prompt: un
   binding remapeado es una preferencia, no progreso de partida, y no debe
   borrarse al presionar "Nueva Partida"/"Reiniciar" (que sí borra
   `astrobit_save.json`). Sigue siendo JSON (`SaveBindingOverridesAsJson()`
   del propio Input System), solo que el contenedor es `PlayerPrefs` en vez
   del archivo de guardado.
2. **`SettingsUI` se reorganizó en pestañas**, no en una sección más apilada
   verticalmente — la UI de remapeo no entraba en los 1010px del panel junto
   a lo que ya había. Esto es un cambio de layout más grande de lo que el
   prompt pedía literalmente ("agrega una sección/pestaña"), pero "pestaña"
   ya sugería esta solución y era la única forma de que todo entrara sin
   scroll ni un panel gigante.
3. **Solo la tecla primaria de movimiento (WASD) es remapeable, no las
   flechas ni el stick del mando.** Cada dirección del composite "Move" tiene
   2 bindings de teclado (ej. W y flecha arriba, ambos disparan "Arriba");
   remapear busca el binding por su path original (`<Keyboard>/w`), deja la
   flecha como alternativa fija siempre disponible. El stick del mando no es
   individualmente remapeable porque no tiene sentido "reasignar cuál stick
   es cuál" en un mando estándar de dos sticks.
4. **Detección de conflictos por comparación directa de
   `InputBinding.effectivePath`/`.id`** entre todos los bindings del mapa
   "Player" tras cada reasignación (revirtiendo con `RemoveBindingOverride`
   si hay choque) — es el patrón que la documentación y las muestras
   oficiales del Input System usan para esto; no existe un método
   `IsDuplicate()` de una sola llamada en el propio Input System.
5. **Hallazgo de entorno, no de código:** confirmé (otra vez, como en el
   Bloque 2) que tanto `RebindingOperation` como la detección de proximidad
   de `PlayerInteraction` dependen de que la ventana del Editor tenga foco de
   sistema operativo real para que Unity procese `Update()`/eventos de input
   con normalidad durante las pruebas automatizadas vía `execute_code` — sin
   foco, ninguno de los dos avanza, y esto no tiene relación con el código
   del juego (un jugador real siempre tiene foco en la ventana). Tuve que
   refocar la Game View varias veces durante la verificación.

No se avanzó al Bloque 4 (audio) — pendiente de que descargues/importes los
packs de Kenney recomendados en `prompts/output/03_assets_recomendados.md`.
