# AstroBit — Diagnóstico (Fase 0, Prompt 07 "mejora integral")

Fecha: 2026-09-05. Unity 6000.4.8f1, URP 17.4.0, Input System 1.19.0 (Active Input
Handling = **Both**, legado + nuevo a la vez), Cinemachine 2.10.7, sin `.asmdef`,
sin Test Framework en uso pese a tener el paquete instalado.

## 0. Nota de contexto importante antes de leer el resto

Este prompt (`07_mejora_integral.md`, internamente "Prompt 02") describe a AstroBit
como si estuviera en una etapa temprana: "3 salas jugables (CPU/RAM/Disco Duro)"
como si fueran niveles independientes, sin mencionar menú, guardado ni pulido.
**Eso ya no es el estado real del proyecto.** Revisando `prompts/01_maestro.md` a
`prompts/06_Astrobit_roadmap.md` y el historial de git (`8287f5a` y anteriores),
AstroBit ya pasó por ~6 bloques grandes de trabajo previos: fundamentos (Input
System para interacción, configuración, guardado), integración de Main Menu/pausa,
sistema de misión de almacenamiento completo, polish visual de CPU/RAM, y feedback
visual de flujo de datos. **No es un prototipo — es un recorrido educativo lineal
ya jugable de principio a fin**, con menú, configuración, guardado/carga y pantalla
final.

Dos huecos de trazabilidad que vale la pena señalar (no bloquean nada, solo para
que no se asuma que existen):

- `ASTROBIT_ROADMAP.md` — el prompt `06_Astrobit_roadmap.md` pedía crear este
  archivo en la raíz del repo. **Nunca se creó/commiteó** (no existe en disco ni en
  `git log --all`). El commit `docs: add AstroBit development roadmap` que ese
  prompt esperaba tampoco existe.
- El commit `feat: add data flow visual feedback and restore states` que pedía
  `05_commit.md` tampoco existe como commit separado; ese trabajo terminó
  mezclado dentro del commit más reciente (`8287f5a`, que agrupa settings + main
  menu + HUD + save + data flow de una sola vez).

No se interpreta esto como una decisión a revertir, solo como contexto: la
"memoria persistente" que las sesiones anteriores intentaron dejar por escrito no
llegó a existir, así que este diagnóstico se construyó leyendo directamente
prompts + código + escena en vez de confiar en un roadmap ya escrito.

**Estado de git al iniciar esta sesión:** rama `main`, limpio salvo:
`Assets/TextMesh Pro/.../LiberationSans SDF - Fallback.asset` modificado
(regeneración típica del Editor), y sin trackear: `Assets/Screenshots/`,
`Assets/_Recovery/` + `.meta` (carpeta de recuperación de Unity tras un cierre
anómalo — revisar/gitignorar en vez de dejarla suelta) y los dos prompts nuevos.
Consola del Editor sin errores; un único warning esperado (Input Manager
deprecado, ver sección 4).

---

## 1. Qué está hecho de verdad

**Flujo jugable completo, en una sola escena (`SampleScene`), lineal:**

Sala CPU (ALU → Registros → Unidad de Control → Caché L1 → L2 → L3) → Sala RAM
(RAM1 → RAM2) → Sala de Almacenamiento (buscar archivo en uno de ~20 `FileShelf`
→ entregarlo en `server` → abrirlo en el "Tv 32 Inch" → procesarlo en la CPU →
intentar cargarlo en RAM → RAM insuficiente → bodega → recoger e instalar 2
módulos → ejecutar) → Actividad Final (4 preguntas de opción múltiple) →
`GameCompleteScreen`. Cada paso está orquestado por `ObjectiveSystem` (los 8
pasos CPU/RAM) + `StorageMission` (el resto del recorrido), ambos con
objetivo/pista visibles vía `GameHUD`, y una capa de navegación derivada
(`MissionNavigation` + `MissionUI` + `WorldObjectiveMarker` + `MinimapController`)
que solo lee ese estado sin duplicarlo.

**Menú y meta-sistemas, reales y funcionando (no placeholders):**
- Main Menu (`MainMenuController`) con Nueva Partida / Continuar / Opciones /
  Créditos / Salir, diálogo de confirmación si ya hay guardado, y créditos con los
  packs de terceros listados.
- Pausa (`PauseMenuController`): Continuar / Configuración / Reiniciar sección /
  Volver al menú, con `Time.timeScale`, guarda antes de salir al menú.
- Configuración (`SettingsUI` + `SettingsManager`): volumen maestro/música/
  efectos/interfaz, sensibilidad de cámara + invertir Y (aplicado de verdad sobre
  el `CinemachineFreeLook` vía `CameraSensitivityController`), resolución/pantalla
  completa/VSync/calidad — todo persistido en `PlayerPrefs` y reaplicado al
  iniciar.
- Guardado (`SaveManager`): JSON en `Application.persistentDataPath`, escritura
  casi-atómica (`.tmp` + `File.Replace`), versión de formato, autoguardado en cada
  hito de objetivo, nunca lanza excepción ante un guardado corrupto/ajeno. Restaura
  correctamente el estado visual (glow de componentes, beacons) porque
  `SaveManager.LoadGame()` refresca explícitamente cada `EducationalInteractable`
  y `MissionStepPoint` tras `SceneManager.LoadScene`.
- Música global (`MusicManager`, `DontDestroyOnLoad`, sin duplicados) + un loop de
  audio ambiental (`AmbientAudioSource`, ventilador del SciFi Warehouse Kit).
- Post-procesado URP ya configurado con criterio: Bloom sutil (0.25), Vignette
  sutil (0.2), Tonemapping ACES, **Motion Blur presente pero desactivado por
  defecto** (bueno para confort/accesibilidad aunque no haya un toggle explícito
  todavía).
- Feedback visual "la computadora está viva" ya implementado, no es un hueco:
  cada pieza de la CPU/RAM enciende su propio material de emisión (color propio,
  no genérico) al comprenderse (`EmissiveToggle`), los puntos de misión sin
  representación física propia pulsan en reposo y se encienden sólido al
  completarse (`MissionBeacon`), y hay flujo de datos representado (vidrio del Tv
  y del "server" que se iluminan en secuencia).

## 2. Qué es placeholder / está pendiente / no existe todavía

- **La interacción sigue siendo "acercarse + [E] + leer panel + Entendido"** en
  casi todos los casos. Esto es exactamente el problema que `01_maestro.md`
  sección 11 ya señaló como prioridad y sigue sin resolverse: las únicas
  variaciones mecánicas reales son buscar-en-shelf-con-color, recoger/instalar RAM
  (inventario), y el quiz final. No hay manipular/conectar/reparar/observar-un-
  proceso como mecánica distinta.
- **Cero SFX de interacción/UI.** `SettingsManager` ya expone canales "Efectos" e
  "Interfaz" y los aplica correctamente, pero no hay ni un solo `AudioClip` de
  click/confirmación/error/objetivo-completado en todo el proyecto — los
  sliders funcionan pero no tienen nada que escalar.
- **Sin soporte de mando ni remapeo de controles.** El movimiento usa
  `Input.GetAxis` (Input Manager legado, ver sección 4) y la interacción usa
  `Keyboard.current` directo (sin `InputActionAsset`, sin rebinding UI). Hay un
  `Assets/InputSystem_Actions.inputactions` en el proyecto pero **no lo usa nada**
  (verificado por grep) — es la plantilla por defecto de Unity, no está conectado.
- **Sin modo alto contraste / daltonismo, sin escalado de tamaño de UI/texto, sin
  indicadores visuales para sonidos importantes, sin toggle de "reducir efectos de
  pantalla".** La sección de Accesibilidad del prompt maestro solo está cubierta
  parcialmente (volumen, sensibilidad, invertir Y).
- **Sin ScriptableObjects en todo el proyecto.** Los datos de la progresión
  (`ObjectiveSystem.Sequence`), las preguntas finales (`FinalActivity.Questions`) y
  las constantes de texto de `StorageMission` están todas hardcodeadas como
  arrays/const dentro de MonoBehaviours. Funciona, pero añadir una sala nueva hoy
  significa editar código, no autorar datos.
- **Sin enciclopedia/coleccionable de conceptos** (propuesta en
  `01_maestro.md` sección 17) — no implementada.
- **`Cosmic_Retro_Computer_1_FREE` fue evaluado y descartado explícitamente**
  (documentado en `03_gran.md`/`04_implement.md`: no encaja en escala/estética).
  No debería revisitarse sin una razón nueva y explícita.
- Sin GPU / Placa Madre / Fuente de Poder / Tarjeta de Red como salas nuevas — solo
  existen como ideas candidatas en los prompts, nada implementado.
- No hay curva de dificultad ni estado de fallo real: un `FileShelf` equivocado
  solo repite un mensaje ("No se encuentra el archivo aquí"), sin penalización.
  Coherente con un diseño "sin fallo" para público educativo, pero vale confirmar
  que es la intención y no un vacío.

## 3. Problemas de arquitectura

- **Input mixto real:** `MovementInput.cs` (asset vendored de Jammo-Character,
  no tocar) usa `Input.GetAxis("Horizontal"/"Vertical")` — Input Manager legado —
  mientras que `PlayerInteraction`/`PauseMenuController` usan
  `Keyboard.current` del Input System nuevo. Funciona porque
  `activeInputHandler: 2` ("Both") en ProjectSettings, y ya genera el warning de
  consola "This project uses Input Manager, which is marked for deprecation".
  Cualquier plan de remapeo/gamepad tiene que lidiar con esta mezcla — no se
  puede resolver solo desde el lado nuevo sin envolver o sustituir el movimiento.
- **Sin máquina de estados formal.** El estado "menú / jugando / pausado" está
  repartido de forma implícita entre `GameSession.HasActiveGame` (bool estático),
  `PauseMenuController.isPaused` (bool privado + `Time.timeScale`), y
  comprobaciones de `SceneManager.GetActiveScene().name != "SampleScene"` /
  `!= "MainMenu"` repetidas en al menos 6 clases distintas (`GameHUD`,
  `MinimapController`, `PauseMenuController`, `CameraSensitivityController`,
  etc.). Funciona hoy con 2 escenas; añadir una tercera escena o una cinemática
  obligaría a tocar cada uno de esos puntos.
- **`GameHUD.cs` es una clase de ~720 líneas que hace de todo:** texto de
  objetivo/pista, badge de tecla animado, banner de feedback, banner de ubicación,
  contador de progreso, texto de inventario, Y el sistema completo de panel modal
  (4 modos: Info/Activity/Choice/Reward) con layout manual en código. Es el mayor
  candidato a God Object del proyecto — funciona y está bien comentado, pero
  cualquier cambio de panel obliga a entender las 700 líneas enteras.
- **12+ singletons perezosos** (`ObjectiveSystem`, `StorageMission`, `Inventory`,
  `SaveManager`, `SettingsManager`, `CameraSensitivityController`,
  `MissionNavigation`, `MinimapController`, `WorldObjectiveMarker`,
  `PauseMenuController`, `SettingsUI`, `GameHUD`, `MissionUI`,
  `GameCompleteScreen`, `FinalActivity`, `MusicManager`). Patrón consistente y
  bien documentado (incluye las trampas de orden de `RuntimeInitializeOnLoadMethod`
  ya resueltas con `Bootstrap()` explícitos), pero es, en conjunto, un grafo grande
  de estado global ambiente — vigilar que no siga creciendo sin criterio.
- **Toda la UI se construye en código en tiempo de ejecución** (sin prefabs, sin
  UI Toolkit) de forma consistente en las 8 clases de `Assets/Scripts/UI/` +
  varias de `Interaction/`. Es una decisión deliberada y documentada (evita
  desincronía prefab/código), pero como consecuencia no hay forma de iterar
  visualmente en el Editor ni de detectar un bug de layout (como el overflow de
  `SettingsUI` que `01_maestro.md` sección 5 tuvo que corregir) sin entrar a Play
  Mode cada vez.
- `EducationalInteractable` todavía carga campos de una "Actividad - Matemática
  (legado/transición)" (`operandA`, `operandB`, `operation`) que el propio
  comentario del código marca como sin uso real (el panel de Prompt 18 ya no los
  invoca). Candidato seguro a limpieza.
- Triple redundancia de navegación hacia el mismo objetivo: `WorldObjectiveMarker`
  (baliza 3D), `MinimapController` (punto 2D) y `MissionUI` (texto), los tres
  leyendo `MissionNavigation.CurrentTarget` de forma independiente. No es un bug,
  pero vale una revisión intencional de si los tres aportan o si sobra alguno en
  ciertos momentos del juego.

## 4. Problemas de jugabilidad / game feel

- El movimiento en sí (asset vendored) no tiene aceleración/desaceleración más
  allá del blend del Animator, no hay coyote time ni buffer de input (no hay
  salto), no hay sprint. No se puede tocar el script directamente
  (`Assets/Jammo-Character/` es de solo lectura por convención del proyecto), así
  que cualquier mejora de sensación de movimiento tendría que ser una capa
  encima, no una edición del vendor.
- Fuera de la sala de almacenamiento (que sí tiene variedad: buscar, entregar,
  recoger, instalar), CPU y RAM son 8 interacciones estructuralmente idénticas
  (panel informativo → "Entendido" → recompensa). El feedback visual (glow
  progresivo) ya ayuda a que no se sienta 100% plano, pero la mecánica en sí no
  varía.
- Sin fallos con consecuencia real en ningún punto del recorrido — ver nota en
  sección 2.

## 5. Accesibilidad y configuración — estado real

| Ítem pedido en el prompt maestro | Estado |
|---|---|
| Controles remapeables | ❌ No existe (ni InputActionAsset conectado ni UI) |
| Alto contraste / daltonismo | ❌ No existe |
| Escalado de tamaño de UI/texto | ❌ No existe |
| Indicadores visuales para sonido importante | ❌ No existe |
| Reducir efectos de pantalla (shake/flash) | ⚠️ Parcial: ya son sutiles por defecto (Motion Blur off, Bloom/Vignette bajos) pero sin toggle explícito |
| Soporte de gamepad | ❌ No existe |
| Volumen por canal | ✅ Real (Master/Música), ⚠️ Efectos/Interfaz aplican correctamente pero no tienen ningún sonido que escalar todavía |
| Sensibilidad de cámara / invertir Y | ✅ Real, aplicado sobre Cinemachine FreeLook |
| Resolución / pantalla completa / VSync / calidad | ✅ Real, persistido |
| Guardado de progreso y preferencias | ✅ Real (JSON + PlayerPrefs) |

## 6. Rendimiento

- Escena `SampleScene` (única, 210k líneas YAML): **2196 GameObjects activos,
  1982 `MeshRenderer`, 9 luces** (todas en modo no-baked, es decir tiempo real),
  0 `ParticleSystem`, 0 `AudioSource` en modo Editor (los reales se crean en
  Play). No se observó ningún objeto marcado `isStatic: true` entre los nodos
  raíz muestreados → sin batching estático ni lightmapping horneado; toda la
  iluminación es dinámica.
- A esta escala (una escena, sin instanciar/destruir nada en bucle salvo
  coroutines puntuales) no hay síntoma actual de problema de rendimiento, pero es
  el primer lugar a mirar con el Profiler si Fase 1/2 añaden más salas o props —
  siguiendo la propia regla ya establecida en el proyecto ("no optimizar a
  ciegas", `06_Astrobit_roadmap.md` sección 9, regla 17).
- No hay pooling de objetos — no hace falta hoy porque nada se instancia/destruye
  repetidamente en runtime; solo sería relevante si una mecánica nueva lo
  requiere.
- `MissionNavigation` recalcula cada 0.2s vía corrutina (no cada frame) — barato.
  Las búsquedas `FindFirstObjectByType`/`FindObjectsByType` presentes en
  `PlayerInteraction`, `WorldLabel`, `EducationalInteractable`,
  `WorldObjectiveMarker`, `MinimapController` están cacheadas tras la primera
  llamada o se ejecutan a intervalos, no por frame sin control.
- Único warning de consola: deprecación de Input Manager (esperado, ver sección
  4). Sin errores, sin excepciones.

---

## Resumen para la Fase 1

AstroBit **no es un prototipo que necesite fundamentos** — ya tiene menú, guardado,
configuración parcial, progresión, feedback visual y una identidad temática
consistente. El trabajo de mayor impacto real, en orden aproximado de lo que más
cambiaría la experiencia:

1. Accesibilidad real (remapeo + gamepad + alto contraste/escala de UI) — hueco
   más grande y más citado en ambos prompts, actualmente en 0%.
2. Romper el patrón "acercarse + E + leer" con 1-2 interacciones mecánicamente
   distintas (no otra ronda de paneles).
3. SFX de interacción/UI — los canales de audio ya existen y no tienen nada que
   reproducir.
4. Reducir el God Object de `GameHUD` (al menos separar el sistema de panel modal
   de el resto del HUD) antes de que crezca más.
5. Evaluar mover la progresión "hardcodeada en código" a ScriptableObjects si se
   piensa añadir una sala nueva (GPU/Placa Madre/etc.) — evitarlo si el alcance de
   Fase 2 no incluye salas nuevas todavía.

No se ha tocado ningún script, escena ni asset durante esta auditoría — Fase 0
es solo lectura. Pendiente tu confirmación del plan antes de pasar a Fase 1.
