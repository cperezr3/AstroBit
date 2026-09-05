# Prompt 03 — Plan de mejora (Fase 1) + búsqueda de assets gratuitos

## Contexto
Confirmo el diagnóstico de `prompts/output/02_diagnostico.md`. El estado real
del proyecto es: recorrido educativo lineal completo (Main Menu → CPU → RAM →
Almacenamiento → Actividad Final → cierre), con guardado JSON, música, pausa
y feedback visual ya implementados. No es un prototipo temprano.

Los huecos confirmados a resolver son:
1. Input mixto (Input Manager legado + Input System) sin soporte de gamepad
   ni remapeo de controles.
2. Cero SFX de interacción/UI (sliders de volumen sin nada que reproducir).
3. Sin alto contraste, sin escalado de UI, sin ScriptableObjects en el proyecto.
4. Interacción repetitiva: "acercarse + [E] + leer panel" en casi todo.
5. GameHUD.cs es un God Object (~720 líneas).
6. No hay máquina de estados formal (estado repartido en 6+ clases).
7. 12+ singletons perezosos.
8. Rendimiento: 2196 GameObjects / 1982 MeshRenderers sin static batching ni
   lightmapping (no urgente hoy, pero bloqueante si se agregan más salas).

Esta es la prioridad real. No re-diagnostiques ni asumas que el proyecto está
en un estado más temprano de lo que es.

---

## FASE 1 — Plan de mejora priorizado

Escribe el plan en `prompts/output/03_plan.md`. Para cada punto: qué harás,
por qué, y esfuerzo vs beneficio.

**A. Arquitectura (resolver primero, todo lo demás depende de esto)**
- Diseña una máquina de estados formal (enum + clase GameStateManager o
  similar) que centralice menú/juego/pausa/cierre, reemplazando el estado
  repartido en las 6+ clases actuales.
- Descompón GameHUD.cs en componentes con responsabilidad única (ej.
  HUDProgressDisplay, HUDPausePanel, HUDVolumeControls...), conectados por
  eventos, no por referencias directas cruzadas.
- Evalúa los 12+ singletons: ¿cuáles son necesarios como singleton real
  (ej. AudioManager, SaveManager) y cuáles deberían ser instancias normales
  inyectadas? Propón una lista concreta de qué queda singleton y qué no.
- Introduce ScriptableObjects para al menos: parámetros de dificultad/tiempos
  por sala, definiciones de items/módulos instalables, configuración de audio.

**B. Input unificado y accesibilidad**
- Migra todo el movimiento al Input System (elimina el Input Manager legado).
- Añade soporte de gamepad.
- Añade pantalla de remapeo de controles (Rebinding UI del Input System).
- Modo alto contraste / amigable con daltonismo para elementos interactivos.
- Escalado de tamaño de UI/texto en el menú de configuración.

**C. Audio**
- Implementa SFX reales para: interacción con objetos, apertura/cierre de
  paneles, confirmaciones, errores (ej. "RAM insuficiente"), transición de
  sala, logro/actividad completada. Conéctalos a los sliders de volumen ya
  existentes.

**D. Variedad de interacción (el punto que sigue pendiente de prompts previos)**
- Propón 3-4 formas nuevas de interactuar que no sean "acercarse + [E] + leer
  panel": ej. mini-drag&drop de archivos en Almacenamiento, un mini-juego de
  temporización en CPU (relacionado al "overclock" si ya existe o se agrega),
  un puzzle de conexión de "pines" en RAM, control de un cursor/láser para
  seleccionar sectores en Disco Duro. Deben encajar con lo ya construido, no
  reemplazarlo — evalúa cuál sala se beneficia más de cuál mecánica.

**E. Rendimiento (preventivo)**
- Recomendaciones concretas de static batching / combinación de meshes /
  lightmapping para cuando se agreguen más salas, sin necesidad de tocar
  nada ahora si no es crítico.

Espera mi confirmación antes de pasar a la Fase 2 (implementación).

---

## FASE 1.5 — Búsqueda de assets gratuitos (recomendaciones, no descarga automática)

Quiero mejorar el entorno visual/sonoro con assets externos gratuitos que yo
mismo revisaré e importaré. Haz lo siguiente:

1. Si tienes una herramienta de búsqueda web disponible en esta sesión,
   úsala para buscar assets gratuitos reales y vigentes (no de memoria/no
   inventados) que encajen con la estética actual del proyecto (revisa
   primero el estilo visual ya usado en las escenas: paleta de colores,
   estilo low-poly/flat/pixel/realista, etc., antes de recomendar nada).
2. Si NO tienes acceso a búsqueda web en esta sesión, dime explícitamente que
   no puedes verificar links en vivo, y en su lugar dame una lista de
   búsquedas concretas y las fuentes más confiables para que yo las revise:
   - Unity Asset Store (sección gratuita), filtrando por el estilo detectado.
   - Kenney.nl (assets gratuitos, muy usados para juegos educativos/estilizados).
   - OpenGameArt.org (sprites, texturas, SFX con licencias claras CC0/CC-BY).
   - Freesound.org (SFX de interacción/UI, licencia CC0 filtrable).
   - itch.io (sección de asset packs gratuitos).
   - Sketchfab (modelos 3D con licencia CC0, si el proyecto es 3D).
3. Organiza las recomendaciones (encontradas o buscadas) en
   `prompts/output/03_assets_recomendados.md`, agrupadas por necesidad:
   - SFX de interacción/UI (para resolver el punto C del plan).
   - Modelos/texturas/props para ambientar CPU, RAM y Almacenamiento de forma
     más rica visualmente (chips, cables, placas, contenedores de datos, etc.).
   - Iconografía para el HUD/menú de configuración.
   - Música ambiental adicional si aplica.
4. Para cada recomendación indica: nombre, dónde buscarlo/encontrarlo,
   licencia (debe ser compatible con uso libre, evita licencias que exijan
   atribución compleja o prohíban uso comercial si el juego pudiera venderse
   a futuro), y por qué encaja con el estilo del proyecto.
5. NO descargues ni importes nada automáticamente al proyecto — solo
   investiga y recomienda. Yo reviso y decido qué importar.

---

Empieza por la FASE 1 (plan) y, en paralelo o al final, la FASE 1.5
(recomendación de assets). Guarda ambos resultados en `prompts/output/`.