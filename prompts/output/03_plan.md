# AstroBit — Plan de mejora priorizado (Fase 1, Prompt 08 "plan y assets")

Basado en `prompts/output/02_diagnostico.md`, confirmado por el usuario. No se
re-diagnostica nada aquí — este documento solo prioriza y detalla el "cómo" de
cada hueco ya confirmado. Ningún punto de este plan se implementa todavía
(Fase 2 pendiente de tu confirmación).

## Resumen ejecutivo (orden de ataque recomendado)

| # | Bloque | Esfuerzo | Beneficio | Por qué este orden |
|---|---|---|---|---|
| 1 | A.2 Descomponer `GameHUD` | Medio | Medio-Alto | Todo lo demás (accesibilidad, SFX, nuevas mecánicas) engancha a GameHUD; hacerlo primero evita tocar el God Object dos veces |
| 2 | A.1 Máquina de estados | Medio-Alto | Alto | Elimina la clase de bugs "olvidé resetear X al volver al menú" antes de añadir más estado (SFX, rebind, mecánicas nuevas) |
| 3 | B.1 Migrar movimiento a Input System | Medio | Alto | Requisito duro para B.2 (gamepad) y B.3 (remapeo); cuanto antes se haga, menos código nuevo depende del input mixto |
| 4 | C. Audio (SFX) | Medio | Alto | Gancho más barato y más citado como "se siente vacío"; los canales de volumen ya existen sin nada que reproducir |
| 5 | B.2 + B.3 Gamepad + Remapeo | Medio-Alto | Alto | Depende de (3); es el hueco de accesibilidad más grande y más repetido en los prompts |
| 6 | D. Variedad de interacción | Medio (x cada una) | Alto | Alto impacto en percepción de calidad, pero requiere que el HUD (1) y el input (3) ya estén estables |
| 7 | A.3 Auditoría de singletons | Medio | Medio | Limpieza estructural, no bloquea nada externo — se hace en paralelo o después de (1)/(2) |
| 8 | B.4 + B.5 Alto contraste + escalado UI | Medio | Medio | Depende de (1) (HUD descompuesto) para no tocar 720 líneas de una vez |
| 9 | A.4 ScriptableObjects | Medio-Alto | Alto a largo plazo, bajo si no hay sala nueva | Solo si Fase 2/3 va a añadir una sala nueva (GPU/Placa Madre/etc.) — si no, se pospone explícitamente para no refactorizar sin necesidad |
| 10 | E. Rendimiento | Bajo (ahora) | Preventivo | Sin acción urgente; checklist para cuando se agregue contenido nuevo |

---

## A. Arquitectura

### A.1 Máquina de estados formal

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

Una vez unificado el input (B.1): mapear movimiento/cámara a los sticks
(Cinemachine ya soporta un Input Provider del Input System para el
`FreeLook`) y `[E]`/`Esc` a botones de mando (Sur/A para interactuar, Start
para pausa). **Esfuerzo:** bajo-medio una vez hecho B.1. **Beneficio:** alto.

### B.3 Pantalla de remapeo de controles

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
