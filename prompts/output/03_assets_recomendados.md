# AstroBit — Assets gratuitos recomendados (Fase 1.5, Prompt 08)

Tuve búsqueda web disponible en esta sesión y la usé — todo lo listado abajo
son resultados reales de la búsqueda (septiembre 2026), no de memoria. Aun así,
**verifica tú mismo la licencia exacta y la vigencia del link antes de
importar**: los packs de Kenney.nl son consistentemente CC0 y confiables, pero
los uploads individuales de Freesound/Sketchfab/itch.io varían de autor a
autor y a veces cambian de licencia con el tiempo.

## Estilo detectado en el proyecto (para calibrar qué encaja)

Revisé las capturas en `Assets/Screenshots/` y el código de UI antes de buscar:
- **Escena 3D:** semi-realista, no cartoon — texturas PBR grises/metálicas
  industriales (SciFi Warehouse Kit), bloques de PCB de colores sólidos como
  componentes de CPU (rojo vino, púrpura, teal), conduits/cables verdes
  brillantes en el piso, iluminación oscura y moody con acentos cian.
- **Mascota:** robot rojo/cromado con cabeza-ventilador (renders promocionales,
  no está en la escena jugable en sí).
- **UI:** fondo negro semitransparente, acento cian/teal (`#5AF3FF`
  aproximado), TextMeshPro, sin iconografía todavía (todo es texto).
- **Audio actual:** música ambiental suave + un loop de ventilador; nada más.

Esto descarta paquetes "cartoon/low-poly flat color" y favorece paquetes
sci-fi **realistas o semi-realistas**, con acentos cian/verde, y SFX
electrónicos/synth en vez de 8-bit muy retro (salvo el logo, que sí es
pixel-art, pero eso es solo el título del menú, no la estética 3D).

---

## 1. SFX de interacción/UI (resuelve el punto C del plan)

| Recomendación | Dónde encontrarlo | Licencia | Por qué encaja |
|---|---|---|---|
| **UI Pack: Sci-Fi (Sounds)** | kenney.nl/assets/ui-pack-sci-fi | CC0 | Es el pack más directamente alineado: sonidos de UI con estética sci-fi, 130 assets, gratis para uso comercial sin atribución. Primera opción. |
| **Sci-fi Sounds** (Kenney) | kenney.nl/assets/sci-fi-sounds | CC0 | Efectos sci-fi generales (más allá de UI) — útil para "objetivo completado" / "sistema activado" con más cuerpo que un simple beep. |
| **Interface Sounds** (Kenney) | kenney.nl/assets/interface-sounds | CC0 | 100 sonidos de interfaz genéricos — respaldo si el pack sci-fi no cubre algún caso puntual (ej. un "cerrar panel" muy neutro). |
| **UI Audio** (Kenney) | kenney.nl/assets/ui-audio | CC0 | Alternativa/complemento a Interface Sounds, mismo autor y licencia. |
| Clips sueltos de confirmación/error si hace falta variar | freesound.org — buscar "Confirm Beeps" (SilverIllusionist), "Sci-fi Warning Beep" (JapanYoshiTheGamer), "Sci Fi button beep" (peepholecircus) | CC0 / CC-BY según el clip — **revisar cada uno individualmente en la página del sonido antes de usar** | Freesound mezcla licencias por usuario; estos tres aparecieron marcados como uso libre en la búsqueda pero confirma el badge de licencia en la propia página antes de descargar. |
| **Electric Sound Effects Library** (LittleRobotSoundFactory) | freesound.org/people/LittleRobotSoundFactory/packs/16881/ | Verificar en la página (LittleRobotSoundFactory suele licenciar CC-BY 3.0, que exige atribución simple) | Útil para el feedback de "sistema activado"/glow encendiéndose (chispazo eléctrico corto) en CPU/RAM — coherente con el `EmissiveToggle` que ya existe. Si la atribución es un problema, usar solo como referencia y buscar equivalente CC0 en Kenney. |
| Servos/robot (opcional, si se anima al robot jugador) | freesound.org — packs "servo and motor sounds" (Artninja), "Robot sounds" (dotY21) | Verificar por pack | Solo relevante si en algún momento se anima el movimiento del robot con sonido propio; no es prioritario para el plan actual (el jugador no tiene SFX de movimiento hoy). |

**Recomendación concreta de prioridad:** importar primero **UI Pack: Sci-Fi**
y **Sci-fi Sounds** de Kenney (ambos CC0 sin condiciones, sin dudas de
licencia) — con esos dos probablemente se cubre el 100% de los ganchos que
pide el punto C del plan (`UIClick`, `UIConfirm`, `UIError`,
`InteractSuccess`, `InteractDeny`, `ObjectiveComplete`, `RoomTransition`) sin
tener que mezclar autores de Freesound.

## 2. Modelos/texturas/props para ambientar CPU, RAM y Almacenamiento

| Recomendación | Dónde encontrarlo | Licencia | Por qué encaja |
|---|---|---|---|
| **LowPoly Modular Sci-Fi Environments** | opengameart.org/content/lowpoly-modular-sci-fi-environments | CC0 (uso personal y comercial libre, según la ficha) | Set modular grande de interiores sci-fi (FBX/OBJ/Blend) — buen complemento genérico para vestir corredores/paneles sin comprometerse a un solo prop puntual. |
| **2 Tiling Circuit Textures Pack** | opengameart.org/content/2-tilling-circuit-textures-pack | CC0 | Texturas de circuito tileables — encajan directamente con los bloques de PCB de colores sólidos que ya son la identidad visual de la sala CPU; podrían aplicarse como detalle adicional sin cambiar la geometría existente. |
| **Sci-Fi Interface Textures** | opengameart.org/content/sci-fi-interface-textures | CC0 | Texturas de pantalla/interfaz en 2K — útiles para las pantallas del "Tv 32 Inch" / terminal de Almacenamiento (punto D.3 del plan) en vez de un material liso. |
| **SciFi Cable Scene (con texturas animadas)** | opengameart.org/content/scifi-cable-scene-with-animated-textures | CC0 | Cables con textura animada (incluye "cable cortado") — coherente con los conduits verdes ya existentes en el piso de CPU/RAM; podría usarse para variar el flujo de datos visual sin geometría nueva. |
| **Printed Circuit Board Texture** | opengameart.org/content/printed-circuit-board-texture | CC0 | Textura de PCB realista simple — alternativa más "de foto" a la textura de circuito estilizada, para paneles/pisos de detalle. |
| **Low Poly Server Racks With Modules Included** | sketchfab.com — buscar "Low Poly Server Racks With Modules Included" (autor councilboar) | Revisar en la página (Sketchfab Store vs. Download gratis — confirmar que el botón sea "Download" y no "Buy") | Server racks modulares en 4 tamaños — directamente relevante para reforzar la sala de Almacenamiento (hoy son ~40 props "server (N)" repetidos del pack iPoly3D; esto daría variedad sin más). |
| **MotherBoard + Components** (Daniel Cardona) | sketchfab.com/3d-models/motherboard-components-3bc94057328243d4b341a55f59160f8a | Revisar licencia en la página (Sketchfab permite al autor elegir CC0/CC-BY/Standard — no asumir CC0 sin confirmar) | Si en algún momento se explora una sala de Placa Madre (candidata mencionada en prompts previos), esta es una referencia visual realista de motherboard completa con componentes. |
| Cable PC low poly | sketchfab.com/3d-models/low-poly-pc-cable-464d4da5b46842ed8cb37b7c50168a60 | Revisar licencia en la página | Prop pequeño y genérico, útil como relleno de detalle en cualquier sala sin comprometer mucho tiempo de integración. |

**Nota de licencia importante para todo Sketchfab:** a diferencia de Kenney
(siempre CC0) y OpenGameArt (licencia visible en la ficha), en Sketchfab cada
autor elige su propia licencia y el buscador no distingue "gratis para
descargar" de "CC0 real" — **antes de importar cualquier modelo de Sketchfab,
confirma en la página del modelo que dice explícitamente "CC Attribution" o
"CC0" y no una licencia "Standard" (que suele prohibir redistribución/reventa)**.

## 3. Iconografía para el HUD / menú de configuración

| Recomendación | Dónde encontrarlo | Licencia | Por qué encaja |
|---|---|---|---|
| **Input Prompts** (Kenney) | kenney.nl/assets/input-prompts | CC0 | 1500 iconos de teclas/botones de teclado y mando (Xbox, PlayStation, Switch, Steam Deck, genérico) en PNG y SVG. **Esto es exactamente lo que necesita B.3 (pantalla de remapeo) y B.2 (gamepad)** del plan — hoy el HUD dibuja el prompt `[E]` como texto plano; con este pack podría mostrar el icono real de la tecla/botón. Prioridad alta. |
| **Input Prompts Pixel** / **Pixel 1-Bit** (Kenney) | kenney.nl/assets/input-prompts-pixel · kenney.nl/assets/input-prompts-pixel-16 | CC0 | Alternativa pixel-art de lo mismo — solo si se decide alinear el HUD con la estética pixel del logo del menú principal en vez del estilo limpio/realista actual del HUD in-game. No recomendado mezclar ambos estilos en la misma pantalla. |
| **UI Pack: Sci-Fi** (Kenney, iconos incluidos) | kenney.nl/assets/ui-pack-sci-fi | CC0 | Además de sonidos, la ficha de Kenney para UI Sci-Fi normalmente incluye también sprites de botones/paneles/cursores — revisar el contenido exacto al descargar, puede cubrir both audio e iconografía de una sola vez. |
| **Game Icons** (Kenney) | kenney.nl/assets/game-icons | CC0 | Set de iconos genéricos (engranaje de configuración, altavoz de audio, gamepad, etc.) — útil para los encabezados de sección de `SettingsUI` (AUDIO/CONTROLES/GRÁFICOS) en vez de solo texto. |

## 4. Música ambiental adicional

| Recomendación | Dónde encontrarlo | Licencia | Por qué encaja |
|---|---|---|---|
| **[Music Assets] FREE Music Loop Bundle** (Tallbeard Studios) | tallbeard.itch.io/music-loop-bundle | CC0 (más de 200 loops, según la ficha) | Bundle grande y CC0 explícito — más seguro que buscar pistas sueltas de autores distintos. Filtrar por las pistas de ambiente/tensión baja para no chocar con la música de menú ya existente. |
| **Free Sci-Fi Music Pack Vol. 2** | itch.io — buscar "Free Sci-Fi Music Pack Vol. 2" | Declarado "public domain / royalty free / CC0" en la ficha — **confirmar el archivo de licencia incluido en el ZIP antes de usar** | Directamente temático (sci-fi) — candidato para música específica de la sala de Almacenamiento si se quiere variar del loop único actual de `MusicManager`. |
| Colección "CC0 Music" (itch.io) | itch.io/c/7822176/cc0-music | Depende de cada entrada — la colección agrupa ítems marcados CC0, pero confirmar cada uno | Punto de partida para explorar más variedad si las dos opciones de arriba no alcanzan. |

**Nota:** el proyecto ya tiene un `MusicManager` funcional y singular (una
sola pista global, según el comentario del código: "no decide qué suena ni
cuándo"). Si se agrega música por sala, eso es un cambio de diseño (¿una
pista por sala vs. una pista global?) que vale la pena decidir explícitamente
antes de importar más de un track — no es solo un tema de conseguir el asset.

---

## Resumen de prioridad de importación

1. **Kenney "UI Pack: Sci-Fi" + "Sci-fi Sounds"** — cubre casi todo el punto C
   del plan, CC0 sin ambigüedad, cero riesgo de licencia.
2. **Kenney "Input Prompts"** — necesario para B.2/B.3 (gamepad + remapeo),
   CC0 sin ambigüedad.
3. **Kenney "Game Icons"** — mejora `SettingsUI` con iconos de sección, CC0.
4. Texturas de OpenGameArt (circuit/interface/cable) — vestimenta adicional de
   salas existentes, todas CC0 confirmado en la ficha.
5. Modelos de Sketchfab (server racks, motherboard) — **solo tras verificar
   licencia caso por caso**, menor prioridad que 1-4 porque requieren más
   trabajo de integración (retopología/escala/materiales para que combinen
   con SciFi Warehouse Kit/iPoly3D ya en uso) y la licencia no es uniforme.
6. Música adicional (itch.io) — solo si se decide explícitamente variar la
   música por sala; no es un hueco urgente hoy.

No se descargó ni importó nada — todo queda pendiente de tu revisión.

Sources:
- [UI Pack - Sci-Fi · Kenney](https://kenney.nl/assets/ui-pack-sci-fi)
- [Sci-fi Sounds · Kenney](https://kenney.nl/assets/sci-fi-sounds)
- [Interface Sounds · Kenney](https://kenney.nl/assets/interface-sounds)
- [UI Audio · Kenney](https://kenney.nl/assets/ui-audio)
- [Input Prompts · Kenney](https://kenney.nl/assets/input-prompts)
- [Input Prompts Pixel · Kenney](https://kenney.nl/assets/input-prompts-pixel)
- [Input Prompts Pixel 1-Bit · Kenney](https://kenney.nl/assets/input-prompts-pixel-1-bit)
- [Game Icons · Kenney](https://kenney.nl/assets/game-icons)
- [Freesound - Confirm Beeps.wav by SilverIllusionist](https://freesound.org/people/SilverIllusionist/sounds/664261/)
- [Freesound - Sci-fi Warning Beep by JapanYoshiTheGamer](https://freesound.org/people/JapanYoshiTheGamer/sounds/361247/)
- [Freesound - Sci Fi button beep by peepholecircus](https://freesound.org/people/peepholecircus/sounds/196979/)
- [Freesound - Electric Sound Effects Library by LittleRobotSoundFactory](https://freesound.org/people/LittleRobotSoundFactory/packs/16881/)
- [Freesound - servo and motor sounds by Artninja](https://freesound.org/people/Artninja/packs/41391/)
- [Freesound - Robot sounds by dotY21](https://freesound.org/people/dotY21/packs/17582/)
- [LowPoly Modular Sci-Fi Environments | OpenGameArt.org](https://opengameart.org/content/lowpoly-modular-sci-fi-environments)
- [2 Tilling Circuit Textures Pack | OpenGameArt.org](https://opengameart.org/content/2-tilling-circuit-textures-pack)
- [Sci-Fi interface textures | OpenGameArt.org](https://opengameart.org/content/sci-fi-interface-textures)
- [SciFi cable scene (with animated textures) | OpenGameArt.org](https://opengameart.org/content/scifi-cable-scene-with-animated-textures)
- [Printed Circuit Board Texture | OpenGameArt.org](https://opengameart.org/content/printed-circuit-board-texture)
- [Low Poly Server Racks With Modules Included - Sketchfab](https://sketchfab.com/3d-models/low-poly-server-racks-with-modules-included-14435ab46d2e407799e51ef9242179a7)
- [Low poly PC Cable - Sketchfab](https://sketchfab.com/3d-models/low-poly-pc-cable-464d4da5b46842ed8cb37b7c50168a60)
- [MotherBoard + Components - Sketchfab](https://sketchfab.com/3d-models/motherboard-components-3bc94057328243d4b341a55f59160f8a)
- [Cc0 3D models - Sketchfab](https://sketchfab.com/tags/cc0)
- [[Music Assets] FREE Music Loop Bundle by Tallbeard Studios](https://tallbeard.itch.io/music-loop-bundle)
- [Top free albums & soundtracks tagged Sci-fi - itch.io](https://itch.io/soundtracks/free/tag-science-fiction)
- [CC0 Music - Collection - itch.io](https://itch.io/c/7822176/cc0-music)
