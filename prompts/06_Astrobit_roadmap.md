# PROMPT — CREAR ROADMAP PERSISTENTE DE ASTROBIT

El commit del bloque actual ya está realizado.

Ahora quiero crear un documento persistente dentro del repositorio para que cualquier futura sesión de Claude Code pueda recuperar rápidamente el contexto y continuar el desarrollo sin depender del historial de este chat.

Crea:

```text
ASTROBIT_ROADMAP.md
```

en la raíz del proyecto.

## OBJETIVO

Este archivo será la memoria técnica y hoja de ruta principal del proyecto AstroBit.

Debe permitir que, incluso después de cerrar completamente Claude Code, Unity y este chat, una nueva sesión pueda ejecutar:

> "Lee ASTROBIT_ROADMAP.md y continúa con el siguiente gran bloque."

y entender qué hacer.

---

# CONTENIDO

Organiza el documento aproximadamente así:

## 1. VISIÓN DEL PROYECTO

Explica brevemente:

* qué es AstroBit;
* que es un videojuego educativo sobre el funcionamiento interno de una computadora;
* objetivo de experiencia:

  * explorar;
  * descubrir;
  * interactuar;
  * manipular;
  * comprender;
  * resolver;
  * progresar;
* identidad:

  * tecnológica;
  * futurista;
  * limpia;
  * inmersiva;
  * visualmente atractiva.

---

## 2. ESTADO ACTUAL

Documenta qué está funcionando actualmente.

Incluye:

* Unity 6 URP;
* escenas;
* menú;
* movimiento;
* cámara;
* interacción;
* HUD;
* progresión;
* sistema educativo;
* Storage;
* RAM;
* CPU;
* FinalActivity;
* Pause;
* Settings;
* Save/Load;
* MusicManager;
* audio ambiental;
* feedback visual;
* MissionBeacon;
* Data Flow;
* post-processing.

No inventes sistemas.

Inspecciona el proyecto para confirmar los nombres reales.

---

## 3. ARQUITECTURA ACTUAL

Documenta los sistemas y scripts importantes existentes.

Por ejemplo:

* `PlayerInteraction`
* `IInteractable`
* `GameHUD`
* `ObjectiveSystem`
* `EducationalInteractable`
* `StorageMission`
* `MissionStepPoint`
* `MissionNavigation`
* `LocationZone`
* `MinimapController`
* `MusicManager`
* `SaveManager`
* `SettingsManager`
* `FinalActivity`
* `MissionBeacon`
* `EmissiveToggle`

Para cada uno explica brevemente su responsabilidad.

No hagas una reestructuración.

Solo documenta.

---

## 4. FLUJO EDUCATIVO ACTUAL

Documenta el flujo real del juego.

Especialmente:

**STORAGE → RAM → CACHE → REGISTROS → ALU → CPU**

y cómo se relaciona con la progresión actual.

No inventes una nueva progresión.

---

## 5. SISTEMAS YA TERMINADOS

Crea una lista de bloques terminados.

Por ejemplo:

### Bloque 1 — Auditoría/Fundamentos

Estado: COMPLETADO

### Bloque 2 — UI/Settings/Save

Estado: COMPLETADO

### Bloque 3 — Game Feel / Feedback

Estado: COMPLETADO

### Bloque 4 — FinalActivity

Estado: COMPLETADO

### Bloque 5 — Polish Visual + Decoración

Estado: COMPLETADO

### Bloque 6 — Data Flow + Computer Activity

Estado: COMPLETADO

Corrige los nombres si el historial real del proyecto permite identificarlos mejor.

---

## 6. DECISIONES IMPORTANTES QUE NO DEBEN REVERTIRSE

Documenta explícitamente:

### TopRig

El comportamiento de Transform de `TopRig` después de Play Mode fue investigado.

Es comportamiento interno/irrelevante de Cinemachine.

La cámara real funciona correctamente.

Por tanto:

> NO tocar TopRig para intentar corregir ese cambio de Transform.

### Mini-quizzes

No reintroducir los mini-quizzes antiguos.

La progresión Storage → RAM → CPU es intencional.

### Cosmic_Retro_Computer_1_FREE

El pack existe, pero no encaja naturalmente con la escala/estética actual.

No utilizarlo simplemente por tenerlo disponible.

### StorageMission ghosts

Los objetos singleton persistentes creados accidentalmente mediante pruebas de Editor sí son un problema real.

Antes de guardar escenas:

* comprobar que no existan duplicados;
* evitar invocar `.Instance` de forma insegura durante pruebas;
* comprobar el estado de la escena.

---

## 7. ESTADO DE GIT

Registra:

* branch actual;
* commit actual;
* mensaje del último commit;
* si el working tree está limpio.

Obtén estos datos directamente de Git.

No inventes el hash.

---

## 8. PRÓXIMOS GRANDES BLOQUES

No quiero una lista infinita de tareas pequeñas.

Define una hoja de ruta de bloques grandes.

El primer siguiente bloque debe ser el de mayor impacto real.

Una posible dirección es:

### SIGUIENTE BLOQUE — GAMEPLAY / INTERACTIVIDAD AVANZADA

Investigar y posteriormente implementar interacciones que hagan que el jugador participe activamente en el aprendizaje.

Posibles sistemas:

* terminal interactiva;
* manipulación de componentes;
* conectar/desconectar;
* configurar;
* activar/desactivar;
* reparar;
* observar procesos;
* resolver problemas;
* interacción contextual con componentes.

IMPORTANTE:

No implementar todo automáticamente ahora.

Este documento debe registrar el bloque como:

**PLANIFICADO / NO IMPLEMENTADO**

La decisión final deberá hacerse después de auditar el estado actual del proyecto.

Después pueden existir bloques como:

* audio polish;
* accessibility;
* encyclopedia/collectibles;
* visual polish adicional;
* optimization;
* QA/final pass.

Pero ordénalos por impacto real y no por cantidad de features.

---

## 9. REGLAS PARA FUTURAS SESIONES

Documenta estas reglas:

1. Primero leer `ASTROBIT_ROADMAP.md`.
2. Inspeccionar el estado actual antes de modificar.
3. Preferir bloques grandes y coherentes.
4. No hacer microcambios sin impacto.
5. No reemplazar sistemas funcionales sin necesidad.
6. Reutilizar arquitectura existente.
7. No crear sistemas duplicados.
8. No introducir dependencias externas innecesarias.
9. No descargar assets externos salvo decisión explícita.
10. Todo contenido nuevo debe estar en español.
11. Validar en Play Mode.
12. Revisar Console.
13. Revisar Git diff.
14. Comprobar StorageMission ghosts antes de guardar escenas.
15. No tocar TopRig por el comportamiento conocido.
16. Priorizar gameplay + visual + educación.
17. No optimizar a ciegas: medir primero cuando sea relevante.

---

## 10. CRITERIO DE CALIDAD

Documenta que AstroBit no debe considerarse terminado simplemente porque:

> "compila."

Debe sentirse como:

> **un videojuego educativo terminado.**

Cada bloque importante debe mejorar al menos una de estas dimensiones:

* Gameplay.
* Educación.
* Visual.
* Inmersión.
* UX.
* Feedback.
* Rendimiento.
* Robustez.

Idealmente varias simultáneamente.

---

## 11. INSTRUCCIÓN PARA FUTURAS SESIONES

Al final del archivo añade una sección claramente visible:

```text
# CÓMO CONTINUAR EL PROYECTO

Antes de comenzar cualquier trabajo:

1. Leer este archivo completo.
2. Revisar el estado actual de Git.
3. Auditar el sistema relacionado con el siguiente bloque.
4. No asumir que el estado actual sigue exactamente igual.
5. Implementar el siguiente GRAN BLOQUE planificado.
6. Trabajar de forma autónoma.
7. Validar.
8. Actualizar este roadmap.
9. Crear un commit del bloque terminado.
10. No hacer push salvo que se solicite explícitamente.
```

Y añade:

```text
## SIGUIENTE ACCIÓN

Leer la sección "Próximos grandes bloques" y ejecutar el primer bloque marcado como PLANIFICADO, después de realizar una auditoría breve del estado actual.
```

---

# IMPORTANTE

No modifiques gameplay ni escena durante esta tarea.

Solo:

1. inspecciona;
2. documenta;
3. crea `ASTROBIT_ROADMAP.md`;
4. revisa el archivo;
5. haz un commit exclusivamente de este documento.

Commit:

```text
docs: add AstroBit development roadmap
```

NO hagas push.

Al finalizar informa:

* archivo creado;
* resumen de su contenido;
* siguiente bloque registrado;
* hash del commit;
* estado de Git.
