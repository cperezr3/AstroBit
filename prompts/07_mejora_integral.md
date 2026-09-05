# Prompt 02 — Mejora integral de Astrobit (Unity)

## Contexto
Estoy en la raíz del proyecto de Unity de "Astrobit" (carpeta Assets al mismo
nivel). El juego trata de un robot que se mueve dentro de una computadora.
Actualmente hay 3 salas jugables: CPU, RAM y Disco Duro.

Antes de empezar, revisa la carpeta `prompts/` en la raíz del proyecto y lee
los archivos anteriores (01_*.md, etc.) para entender qué se te ha pedido
antes y qué contexto/decisiones ya existen. No repitas trabajo ya hecho ni
contradigas decisiones previas sin avisarme primero.

Quiero una mejora INTEGRAL y significativa: calidad de código, jugabilidad,
accesibilidad, configuración y contenido. El objetivo es que se sienta como
un juego completo y pulido, no un prototipo.

---

## FASE 0 — Auditoría (obligatoria antes de programar)

1. Explora la estructura completa del proyecto: Assets, Scenes, Scripts,
   Prefabs, y el Package Manifest (versión de Unity, Input System, URP/HDRP,
   Cinemachine, TextMeshPro, Addressables, etc.).
2. Abre y analiza las 3 escenas existentes: mecánicas implementadas, scripts
   de movimiento/cámara/colisiones, manejo de estados de juego, UI actual,
   manejo de input.
3. Evalúa la calidad del código: uso de ScriptableObjects, eventos, object
   pooling, separación lógica/presentación, valores hardcodeados, tamaño y
   responsabilidad de los MonoBehaviours.
4. Escribe tu diagnóstico en un archivo nuevo `prompts/output/02_diagnostico.md`
   (créalo si no existe la carpeta) con:
   - Qué está hecho de verdad (mecánicas, arte, audio, UI) vs qué es placeholder.
   - Problemas de arquitectura.
   - Problemas de jugabilidad/game feel.
   - Estado de accesibilidad y configuración.
   - Rendimiento (draw calls, uso de URP, physics innecesarios).
5. Detente aquí. No implementes nada de la Fase 1 hasta que yo confirme el
   plan basado en tu diagnóstico.

---

## FASE 1 — Plan de mejora y expansión

Con base en el diagnóstico, escribe el plan en `prompts/output/02_plan.md`,
dividido en las siguientes categorías. Para cada punto indica: qué cambiarás,
por qué mejora el juego, y esfuerzo vs beneficio (alto/medio/bajo).

**Arquitectura de código (Unity-idiomático)**
- Input System (si aún usan el legado, migrar).
- ScriptableObjects para datos de configuración de gameplay.
- Máquina de estados clara (menú, sala activa, pausa, transición, game
  over/victoria).
- Object pooling para elementos repetidos.
- Separación de lógica y presentación (eventos/UnityEvents u Observer simple).

**Jugabilidad y game feel**
- Movimiento del robot: aceleración/desaceleración, coyote time, buffer de
  input, animaciones responsivas.
- Feedback de acciones: partículas, screen shake sutil, sonido de impacto,
  flash de daño, squash & stretch donde aplique.
- Identidad de mecánica por sala, ligada al tema real del componente:
  * CPU: temporización/velocidad, "overclock" como mecánica de riesgo-beneficio.
  * RAM: gestión de espacio/memoria, plataformas que aparecen/desaparecen
    (lectura/escritura), límite de "buffer".
  * Disco Duro: plataformas giratorias (plato del disco), fragmentación como
    obstáculo, sectores dañados como trampas.
- Propón 2-3 salas/mecánicas nuevas coherentes con el tema (candidatos:
  GPU como sala de renderizado/luz, Placa Madre como hub central, Fuente de
  Poder con energía limitada, Tarjeta de Red con velocidad/paquetes de datos,
  un "Virus" como antagonista recurrente).
- Progresión: habilidades del robot que se desbloquean (ej. "optimizar" =
  más velocidad en CPU, "comprimir" = pasar por espacios pequeños en Disco).
- Curva de dificultad coherente y un hub/menú de niveles.

**Accesibilidad**
- Controles remapeables (Input System + Rebinding UI).
- Modo alto contraste / amigable con daltonismo.
- Escalado de tamaño de UI/texto.
- Indicadores visuales para sonidos importantes.
- Opción de reducir efectos de pantalla (shake, flashes).
- Soporte de gamepad además de teclado.

**Configuración**
- Resolución, pantalla completa/ventana, VSync, límite de FPS.
- Audio Mixer con canales de música/SFX independientes.
- Sensibilidad de cámara/input.
- Guardado de progreso y preferencias (PlayerPrefs o JSON según alcance).

**Arte, UI/UX y audio**
- Paleta de colores distintiva por sala dentro de una identidad visual unificada.
- HUD claro: vida/energía, objetivo actual, progreso.
- Transiciones de escena pulidas.
- Música ambiental por sala + SFX de interacción.

**Rendimiento**
- Configuración de URP, calidad de post-procesado, batching, uso correcto
  de Rigidbody/colliders.

Si hay decisiones de diseño ambiguas, dame 2-3 opciones concretas con tu
recomendación — no asumas por mí. Espera mi confirmación antes de pasar a
la Fase 2.

---

## FASE 2 — Implementación incremental

- Trabaja en bloques pequeños, con commits descriptivos (feat:, fix:,
  refactor:, perf:, content:) — un cambio conceptual por commit.
- Después de cada bloque, resume en el chat: qué cambiaste, cómo probarlo en
  el Editor de Unity, y qué escenas/prefabs se vieron afectados.
- Mantén el proyecto siempre compilable y abrible en Unity sin errores.
- Si necesitas assets (sprites, sonidos, modelos) que no puedes generar,
  dímelo explícitamente — no dejes placeholders sin avisar.
- Documenta sistemas nuevos importantes (progresión, ScriptableObjects de
  configuración, etc.) con comentarios claros y, si es un sistema grande,
  un archivo `prompts/output/02_sistema_<nombre>.md` explicándolo.
- Al terminar cada fase de implementación, actualiza `prompts/output/02_plan.md`
  marcando qué se completó, para mantener trazabilidad entre sesiones.

---

## Criterio de éxito
El juego debe, al final de este proceso: verse y sentirse pulido, tener
controles configurables y accesibles, un menú de opciones funcional, mecánicas
diferenciadas por sala con feedback claro, y una identidad temática coherente
como "robot explorando el interior de una computadora".

Empieza por la FASE 0 ahora.