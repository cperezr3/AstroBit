# PROMPT 25 — Auditoría del menú principal existente antes de integrarlo en AstroBit

## CONTEXTO

Tengo DOS proyectos Unity separados:

### Proyecto actual / juego principal
D:\Unity\Astro

Este es el proyecto principal de AstroBit donde actualmente está implementado todo el gameplay:

- CPU
- RAM
- Almacenamiento / Disco Duro
- Bodega
- Inventario
- Flujo de archivo
- Sistema de misiones
- Navegación
- Minimap
- Actividad Final
- etc.

### Proyecto separado con el menú
D:\Unity\AstroBitMenu\AstroBit1.0

Este proyecto contiene un menú principal que ya desarrollé anteriormente.

Quiero integrar ese menú dentro del proyecto actual:

D:\Unity\Astro

Pero NO quiero copiar archivos todavía ni modificar el proyecto actual.

Primero quiero una AUDITORÍA COMPLETA del proyecto del menú.

---

# OBJETIVO DE ESTE PROMPT

Inspeccionar profundamente:

D:\Unity\AstroBitMenu\AstroBit1.0

y determinar cómo está construido el menú y cómo podría integrarse posteriormente en:

D:\Unity\Astro

La prioridad absoluta es:

**NO MODIFICAR NINGUNO DE LOS DOS PROYECTOS DURANTE ESTA FASE.**

No copiar archivos.

No mover carpetas.

No modificar escenas.

No crear scripts.

No importar assets.

No cambiar Project Settings.

No tocar Astro.

Esta fase es únicamente de análisis.

---

# 1. INSPECCIÓN DEL PROYECTO DEL MENÚ

Inspecciona la estructura completa de:

D:\Unity\AstroBitMenu\AstroBit1.0

Identifica:

- versión de Unity;
- Render Pipeline;
- paquetes instalados;
- Input System;
- Cinemachine;
- TextMeshPro;
- sistema de audio;
- escenas;
- prefabs;
- scripts;
- materiales;
- fuentes;
- imágenes;
- iconos;
- animaciones;
- shaders;
- plugins;
- assets externos;
- configuración del Canvas;
- EventSystem;
- cámaras;
- iluminación si existe;
- cualquier dependencia externa.

No te limites a mirar los nombres de carpetas.

Inspecciona las escenas y scripts realmente utilizados por el menú.

---

# 2. IDENTIFICAR LA ESCENA DEL MENÚ

Determina exactamente:

- nombre de la escena principal del menú;
- ubicación del archivo .unity;
- qué GameObjects contiene;
- Canvas utilizados;
- EventSystem;
- cámara;
- fondo;
- botones;
- paneles;
- textos;
- animaciones;
- audio;
- scripts asociados.

Determina también si existe:

- Main Menu
- Options
- Settings
- Credits
- Controls
- Quit
- New Game
- Continue
- Load Game

o cualquier otro sistema.

---

# 3. ANALIZAR LOS BOTONES

Para cada botón del menú, documenta qué hace realmente.

Por ejemplo:

### INICIAR PARTIDA

Determinar:

- ¿carga una escena?
- ¿usa SceneManager.LoadScene?
- ¿usa una escena por nombre?
- ¿usa Build Index?
- ¿hay una transición?
- ¿hay loading screen?
- ¿hay animación antes de cargar?

### OPCIONES

Determinar:

- qué configuraciones existen;
- resolución;
- volumen;
- gráficos;
- sensibilidad;
- controles;
- accesibilidad;
- fullscreen;
- etc.

### SALIR

Determinar exactamente cómo funciona.

### OTROS

Documentar cualquier botón adicional.

---

# 4. ANALIZAR TODOS LOS SCRIPTS DEL MENÚ

Encontrar los scripts que controlan:

- navegación;
- botones;
- escenas;
- audio;
- animaciones;
- configuración;
- resolución;
- controles;
- persistencia;
- transición entre escenas;
- cursor;
- input;
- etc.

Para cada script indicar:

- nombre;
- ubicación;
- responsabilidad;
- dependencias;
- referencias Inspector;
- si depende de otro script;
- si depende de un prefab;
- si depende de un objeto de escena;
- si puede reutilizarse directamente en Astro.

NO modificar los scripts.

---

# 5. ANALIZAR DEPENDENCIAS

Esta parte es MUY IMPORTANTE.

Determina qué cosas del proyecto del menú NO pueden copiarse simplemente porque dependen de:

- paquetes;
- assets;
- plugins;
- Project Settings;
- Tags;
- Layers;
- Input Actions;
- Sorting Layers;
- Rendering Settings;
- fuentes;
- prefabs;
- materiales;
- shaders;
- escenas;
- recursos externos.

Crear una tabla:

| Elemento | Existe en Menu | Existe en Astro | Conflicto | Acción recomendada |
|---|---|---|---|---|

---

# 6. COMPARAR VERSIONES DE UNITY

Comparar:

Proyecto Menu:
D:\Unity\AstroBitMenu\AstroBit1.0

Proyecto principal:
D:\Unity\Astro

Determinar:

- versión exacta de Unity;
- versión del paquete Input System;
- Render Pipeline;
- TextMeshPro;
- Cinemachine;
- paquetes relevantes.

Indicar si existe riesgo de incompatibilidad.

---

# 7. ANALIZAR LAS ESCENAS

Determinar cómo está organizado actualmente el proyecto del menú.

Idealmente queremos terminar con una arquitectura parecida a:

Scenes/
├── MainMenu.unity
└── SampleScene.unity

o:

Scenes/
├── MainMenu.unity
├── Game.unity
└── ...

Pero NO implementarlo todavía.

Solo determinar cuál sería la mejor arquitectura.

---

# 8. PROPUESTA DE INTEGRACIÓN

Después de estudiar el proyecto, proponer cómo integrarlo en:

D:\Unity\Astro

La idea futura es:

ARRANQUE DEL JUEGO
        ↓
MAIN MENU
        ↓
┌─────────────────────┐
│      ASTROBIT       │
│                     │
│   INICIAR PARTIDA   │
│   OPCIONES          │
│   CRÉDITOS          │
│   SALIR             │
└─────────────────────┘
        ↓
INICIAR PARTIDA
        ↓
ESCENA ACTUAL DE ASTROBIT
        ↓
GAMEPLAY

Pero todavía NO implementarlo.

---

# 9. CONSIDERAR EL ESTADO ACTUAL DE ASTRO

Recuerda que el proyecto:

D:\Unity\Astro

ya tiene un gameplay funcional.

NO se debe romper:

- ObjectiveSystem
- StorageMission
- FinalActivity
- GameHUD
- MissionUI
- MissionNavigation
- MinimapController
- PlayerInteraction
- EducationalInteractable
- FileShelf
- StorageServer
- Inventory
- CollectibleRam
- InstallRamSlot
- MissionStepPoint
- WorldObjectiveMarker
- WorldLabel
- flujo CPU → RAM → almacenamiento → archivo → CPU → RAM → Actividad Final.

La futura integración debe agregar el menú POR ENCIMA de este sistema, no reemplazarlo.

---

# 10. MUY IMPORTANTE: NO ALTERAR ASTRO

Durante esta auditoría:

NO abrir/modificar/guardar escenas de Astro.

NO crear archivos dentro de:

D:\Unity\Astro

NO modificar scripts.

NO importar assets.

NO hacer cambios en Git.

NO hacer commits.

NO modificar ProjectSettings.

NO cambiar Build Settings.

Solo inspeccionar.

---

# 11. REVISAR SI EL MENÚ TIENE SISTEMA DE CONFIGURACIÓN

Aunque actualmente Astro todavía no tiene menú principal ni configuración, quiero saber qué trae ya el proyecto del menú.

Analizar especialmente si ya tiene:

- volumen maestro;
- música;
- efectos;
- resolución;
- fullscreen;
- calidad gráfica;
- sensibilidad;
- controles;
- idioma;
- accesibilidad;
- guardar preferencias mediante PlayerPrefs;
- sistema de restaurar valores;
- navegación con teclado/control.

Si existe algo de esto, explicar cómo funciona y si sería reutilizable.

---

# 12. ACCESIBILIDAD

Como posteriormente quiero implementar accesibilidad en AstroBit, revisar si el menú ya tiene algo relacionado con:

- tamaño de texto;
- contraste;
- escalado UI;
- navegación por teclado;
- navegación por gamepad;
- remapeo de controles;
- volumen independiente;
- modo fullscreen;
- reducción de efectos;
- opciones visuales;
- etc.

No implementar nada todavía.

Solo identificar qué existe.

---

# 13. RESULTADO FINAL

Al terminar NO quiero cambios.

Quiero un informe con esta estructura:

## 1. Estado general del proyecto del menú

## 2. Versión de Unity

## 3. Escenas encontradas

## 4. Estructura del menú

## 5. Botones y funcionamiento

## 6. Scripts encontrados

## 7. Assets y dependencias

## 8. Paquetes necesarios

## 9. Conflictos potenciales con Astro

## 10. Qué elementos pueden copiarse directamente

## 11. Qué elementos necesitan adaptación

## 12. Qué elementos NO deberían copiarse

## 13. Arquitectura recomendada para integrar el menú

## 14. Plan de integración paso a paso

## 15. Riesgos

## 16. Recomendación final

---

# REGLA FINAL

NO IMPLEMENTAR TODAVÍA.

NO COPIAR NADA.

NO MODIFICAR NADA.

Primero quiero entender exactamente cómo está construido el menú existente y cuál es la forma más segura de incorporarlo a AstroBit.

Después de recibir este informe, haremos un segundo prompt exclusivamente para la integración.