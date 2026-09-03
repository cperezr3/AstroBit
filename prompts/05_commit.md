# PROMPT — COMMIT DEL BLOQUE DATA FLOW + COMPUTER ACTIVITY

El bloque **DATA FLOW + COMPUTER ACTIVITY** está terminado y validado.

Quiero que ahora hagas **ÚNICAMENTE el cierre y commit de este bloque**.

## 1. REVISAR ESTADO DE GIT

Ejecuta:

```bash
git status
git diff --stat
git diff
```

Revisa cuidadosamente todos los cambios.

Confirma que pertenecen únicamente al trabajo realizado en este bloque y a las correcciones necesarias derivadas de él.

Los cambios esperados son principalmente:

* `Assets/Scripts/Interaction/MissionBeacon.cs`
* `Assets/Scripts/Interaction/MissionStepPoint.cs`
* `Assets/Scripts/Interaction/EducationalInteractable.cs`
* `Assets/Scripts/Gameplay/SaveManager.cs`
* `Assets/Scenes/SampleScene.unity`

Si aparecen otros archivos modificados:

* NO los borres automáticamente.
* Determina si pertenecen al trabajo realizado.
* Si son cambios accidentales/no relacionados, no los incluyas en el commit.
* No sobrescribas ni descartes trabajo legítimo previo.

## 2. REVISIÓN ESPECIAL

Confirma antes del commit:

* No existen `StorageMission` ghosts.
* No se modificó intencionalmente `TopRig`.
* No hay archivos temporales de Unity.
* No hay backups o artefactos accidentales.
* No hay cambios ajenos al proyecto.
* No se está incluyendo ningún asset externo innecesario.

## 3. VALIDACIÓN FINAL

Antes de hacer commit confirma:

* compilación limpia;
* sin errores nuevos;
* sin warnings nuevos;
* Data Flow visual funcionando;
* CPU feedback funcionando;
* Save/Load funcionando;
* `RefreshVisualState()` ejecutándose después de restaurar el estado;
* escena limpia.

No hagas nuevas mejoras.

No refactorices.

No cambies gameplay.

No abras otro bloque de trabajo.

Este paso es únicamente:

> **revisar → validar → commit.**

## 4. COMMIT

Si todo está correcto, crea el commit.

Usa este mensaje:

```text
feat: add data flow visual feedback and restore states
```

Después ejecuta:

```bash
git status
git log -1 --oneline
```

## 5. IMPORTANTE

NO hagas `git push`.

Solo crea el commit local.

Al terminar, dame un informe breve con:

1. Archivos incluidos.
2. Archivos excluidos por no pertenecer al bloque, si hubo alguno.
3. Hash del commit.
4. Mensaje del commit.
5. Estado final de `git status`.
