# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

**AstroBit** — a Unity 6 (`6000.4.8f1`) URP 3D first-person interaction/objective game built on a sci-fi warehouse/office environment. Scripting is plain C# MonoBehaviours in a single default assembly (no `.asmdef` files, no test framework installed).

Note: `ProjectSettings/ProjectSettings.asset` currently has `productName: Astro`, not `AstroBit` — flag this to the user if it becomes relevant (e.g. builds, window title), rather than changing it unprompted.

## Working with the Unity Editor

This project is normally driven live through the **UnityMCP** MCP server (tools like `manage_scene`, `manage_gameobject`, `manage_script`, `read_console`, `run_tests`, resources like `mcpforunity://editor/state`). Prefer those tools over hand-editing `.unity`/`.asset`/`.prefab` YAML whenever the Editor connection is available — check `mcpforunity://instances` / connect via `/mcp` first.

When the MCP connection is unavailable and a scene/config change must be hand-edited as YAML (e.g. `Assets/Scenes/SampleScene.unity`, `ProjectSettings/TagManager.asset`), **do not trust static reasoning about correctness** — verify with a real headless Unity run before considering the change done:

```
"/c/Program Files/Unity/Hub/Editor/6000.4.8f1/Editor/Unity.exe" -batchmode -projectPath "D:/Unity/Astro" -executeMethod <ClassName>.<Method> -logFile "D:/Unity/Astro/editor_log.txt"
```

- For structural scene-graph changes (new GameObjects, reparenting, reassigning component references such as Cinemachine Follow/LookAt), write a small temporary Editor script under `Assets/Editor/` that performs the change via the real Unity API (`GameObject`, `transform.SetParent`, `SerializedObject`, ...) and calls `EditorSceneManager.SaveScene()`. Hand-authoring parenting/reference fields directly in YAML (e.g. `m_Father: {fileID: ...}`) can look valid but silently fail to apply at runtime.
- For read-only verification (camera position sane, no exceptions on entering Play), a temporary `[InitializeOnLoad]` Editor script driving `EditorApplication.isPlaying = true`, polling via `EditorApplication.update` (persist state in `EditorPrefs`, not static fields — entering Play triggers a domain reload that wipes statics), logging to a file, then `EditorApplication.Exit(0)` works and takes roughly 30-60s per run.
- Always check `editor_log.txt` for `Parser Failure`, `Exception`, `NullReference` before trusting the result.
- Delete temporary diagnostic/editor scripts (and their `.meta` files) from `Assets/Editor/` once verification is done — they are not part of the shipped project.
- Only hand-edit small `ProjectSettings/*.asset` files as a last resort. Unity's YAML parser for these is not a standard YAML parser — e.g. empty `"- "` list entries in `TagManager.asset` require an exact trailing space, or you get a silent-looking `Parser Failure`. Diff whitespace (`cat -A`) against an untouched line of the same kind before trusting a hand edit.
- Cinemachine `FreeLook` binding modes are not freely interchangeable: `m_BindingMode` must stay a mode the 3-rig orbit math supports (e.g. `SimpleFollowWithWorldUp` = 5). Fix recenter/heading issues via `m_Heading.m_Definition` instead of changing `m_BindingMode`.

There is no CLI build/lint/test command for this project — building, entering Play mode, and running tests are all done through the Editor (directly or via UnityMCP's `manage_build` / `run_tests` tools), not a terminal build system.

## Architecture

Gameplay code lives under `Assets/Scripts/`, split by concern, all in the global namespace:

- **`Interaction/`** — first-person "look at + press E" interaction.
  - `IInteractable` is the contract: `PromptText`, `CanInteract`, `Interact()`.
  - `PlayerInteraction` (on the player/camera) raycasts forward from `playerCamera` each frame, finds an `IInteractable` via `GetComponentInParent`, and shows/hides the HUD prompt as the look target changes. Interaction fires on `Keyboard.current.eKey.wasPressedThisFrame` (new Input System).
  - `SimpleInteractable` is the generic scene-object implementation: shows optional feedback text, optionally completes/advances an objective, and fires a `UnityEvent OnInteracted` for scene-specific hookups. Supports one-shot (default) or repeatable interaction.
- **`Gameplay/ObjectiveSystem.cs`** — a lazily-created singleton (`ObjectiveSystem.Instance`) that tracks `CurrentObjective` and broadcasts `OnObjectiveChanged` / `OnObjectiveCompleted` via `UnityEvent<string>`. Not tied to a scene object — accessing `.Instance` creates a `DontDestroyOnLoad` GameObject on demand.
- **`UI/GameHUD.cs`** — a self-bootstrapping singleton (`[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]`) that builds its own `Canvas`/`Text` UI entirely in code (no prefab) and subscribes to `ObjectiveSystem` events to keep the on-screen objective text in sync. Exposes `ShowPrompt`/`HidePrompt`/`ShowFeedback` for other systems to call.

Data flow: `SimpleInteractable.Interact()` → `ObjectiveSystem.Instance.CompleteObjective(...)`/`SetObjective(...)` → `GameHUD` (subscribed to `ObjectiveSystem`'s events) updates the on-screen text. `PlayerInteraction` talks to `GameHUD.Instance` directly for the transient look-prompt, independent of the objective flow. Both singletons self-initialize, so scenes don't need to pre-place them.

Objective/feedback/prompt strings authored in the Inspector are currently in Spanish (e.g. default prompt `"[E] Interactuar"`) — match that convention when adding new interactables unless told otherwise.

## Third-party assets

`Assets/GoldenFrame_Terminal_FREE/`, `Assets/Jammo-Character/`, `Assets/SciFi Warehouse Kit/`, `Assets/ScifiOfficeLite/`, and `Assets/iPoly3D/` are vendored third-party asset packs (models/materials/prefabs), several hundred MB combined. Treat them as read-only vendor content — build gameplay by referencing/composing their prefabs from `Assets/Scripts/`, don't modify files inside those folders.
