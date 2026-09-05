# Prompt 04 — Implementación Fase 2, Bloque 1: Arquitectura base

## Contexto
Confirmado el alcance de `prompts/output/03_plan.md`:
- A.4 (ScriptableObjects): POSPUESTO. No lo implementes en este bloque ni en
  los siguientes salvo que yo lo pida explícitamente en un futuro prompt.
- D (variedad de interacción): adelante con las 3 mecánicas propuestas
  (pines en RAM, overclock por temporización en CPU, terminal de filtrado en
  Almacenamiento). La cuarta mecánica para "Disco Duro" queda DESCARTADA —
  esa sala no existe como entidad separada, está fusionada en Almacenamiento.

Este prompt cubre SOLO el primer bloque del plan (el que dijiste que todo lo
demás engancha ahí): descomposición de GameHUD.cs + máquina de estados
formal. No avances a los siguientes bloques del plan sin que yo confirme
este primero.

## Antes de tocar código
- Confirma que el árbol de trabajo de git está limpio (sin cambios sin
  commitear) antes de empezar. Si hay cambios pendientes, dímelo y detente.
- Crea una rama nueva para este bloque de trabajo (ej. `feature/arquitectura-hud-estados`)
  en vez de trabajar directo sobre la rama principal.

## Trabajo a realizar
1. Implementa el `GameStateManager` (o el nombre que hayas propuesto en el
   plan) que centraliza el estado menú/juego/pausa/cierre, siguiendo
   exactamente la especificación que dejaste en `03_plan.md` para este punto.
2. Migra las 6+ clases que actualmente manejan estado de forma repartida para
   que consulten/notifiquen al GameStateManager en vez de mantener su propio
   estado local. Ve clase por clase — no lo intentes todo en un solo commit.
3. Descompón `GameHUD.cs` en los componentes de responsabilidad única que
   propusiste (HUDProgressDisplay, HUDPausePanel, HUDVolumeControls, etc.),
   conectados por eventos, no por referencias cruzadas directas.
4. En cada paso, verifica que el juego siga compilando y siendo jugable en el
   Editor antes de seguir al siguiente paso. Si algo rompe el flujo actual
   (guardado JSON, música, pausa, feedback visual existente), detente y
   dime qué pasó — no intentes "arreglarlo sobre la marcha" sin avisar.

## Commits
- Un commit por paso lógico (ej. "feat: agrega GameStateManager",
  "refactor: MenuController usa GameStateManager", "refactor: extrae
  HUDPausePanel de GameHUD", etc.). Nada de un commit gigante al final.

## Al terminar este bloque
Resume en el chat:
- Qué clases se crearon/eliminaron/modificaron.
- Cómo probar manualmente que el flujo completo (menú → CPU → RAM →
  Almacenamiento → Actividad Final → cierre) sigue funcionando igual que antes.
- Cualquier decisión de diseño que tomaste sobre la marcha y que no estaba
  100% especificada en el plan.
- Actualiza `prompts/output/03_plan.md` marcando este bloque como completado.

No avances al Bloque 2 (Input System unificado) todavía — espera mi
confirmación de que este bloque quedó bien.