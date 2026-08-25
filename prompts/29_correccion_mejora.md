# PROMPT 28 — AUDITORÍA GENERAL + MARCADOR 3D + MENÚ DE PAUSA + CONTINUAR PARTIDA

## CONTEXTO

El proyecto actual es:

D:\Unity\Astro

AstroBit ya cuenta con:

- Main Menu funcional.
- SampleScene como escena principal del gameplay.
- CPU/RAM.
- Progresión educativa.
- Almacenamiento / disco duro simulado.
- Bodega de RAM.
- Inventario.
- Instalación de RAM3/RAM4.
- Flujo de archivo:
  Almacenamiento → Server → TV → CPU → RAM → Actividad Final.
- ObjectiveSystem.
- StorageMission.
- FinalActivity.
- GameHUD.
- MissionNavigation.
- MissionUI.
- MinimapController.
- WorldObjectiveMarker.
- Navegación mediante puntos amarillos.
- MainMenuController.
- MainMenu → SampleScene.

El Main Menu acaba de ser integrado y funciona.

AHORA QUIERO HACER UNA REVISIÓN GENERAL DEL PROYECTO Y, DESPUÉS DE LA AUDITORÍA, IMPLEMENTAR TRES COSAS:

1. Encontrar y corregir fallos/regresiones existentes.
2. Arreglar el marcador amarillo 3D que desapareció del mundo aunque sigue apareciendo correctamente en el minimapa.
3. Crear un menú de pausa con ESC.
4. Permitir volver al Main Menu desde pausa.
5. Hacer que "CONTINUAR" del Main Menu permita continuar una partida en progreso.

---

# PARTE 1 — AUDITORÍA GENERAL

ANTES DE MODIFICAR CÓDIGO, inspecciona el proyecto completo.

No quiero una auditoría superficial.

Revisa especialmente:

## Sistemas

- MainMenuController
- GameHUD
- MissionUI
- MissionNavigation
- MinimapController
- WorldObjectiveMarker
- ObjectiveSystem
- StorageMission
- FinalActivity
- PlayerInteraction
- EducationalInteractable
- SimpleInteractable
- Inventory
- CollectibleRam
- InstallRamSlot
- FileShelf
- StorageServer
- MissionStepPoint
- WorldLabel
- LocationZone
- StorageZoneTrigger
- scripts relacionados con navegación
- scripts relacionados con UI

## Escenas

Revisar:

- MainMenu.unity
- SampleScene.unity

## ProjectSettings

Revisar especialmente:

- Build Settings
- escenas
- Input
- configuración relevante de tiempo
- configuración relevante de UI

## Referencias

Buscar:

- Missing Scripts
- Missing References
- GameObjects destruidos pero todavía referenciados
- referencias null
- componentes duplicados
- singletons duplicados
- eventos duplicados
- listeners que nunca se registran
- listeners registrados varias veces
- objetos DontDestroyOnLoad innecesarios
- sistemas que se inicializan incorrectamente al cambiar de escena
- Canvas duplicados
- cámaras duplicadas
- minimapas duplicados
- marcadores duplicados

## Código

Buscar:

- errores potenciales de NullReferenceException
- FindFirstObjectByType obsoleto
- métodos que nunca se utilizan
- lógica duplicada
- estados imposibles
- referencias a objetos por nombre que ya no existen
- nombres de GameObjects incorrectos
- condiciones de gating inconsistentes
- problemas de inicialización entre MainMenu y SampleScene
- problemas al cambiar de escena
- problemas al volver al Main Menu
- problemas al volver a SampleScene
- problemas de Time.timeScale
- problemas con Canvas y SceneManager

IMPORTANTE:

No hagas una refactorización masiva.

No cambies arquitectura que ya funciona simplemente porque puede hacerse "más limpia".

Primero identifica problemas reales.

---

# PARTE 2 — BUG DEL PUNTO AMARILLO 3D

## PROBLEMA ACTUAL

Actualmente ocurre esto:

- El objetivo aparece correctamente dentro del minimapa.
- El objetivo amarillo NO aparece en el mundo 3D.
- Antes sí funcionaba.
- El problema apareció después de integrar el Main Menu / cambios recientes.

Quiero recuperar exactamente el comportamiento anterior.

El marcador 3D debe:

- aparecer en el mundo sobre el objetivo actual;
- ser visible cuando existe una misión activa;
- actualizarse automáticamente al cambiar de objetivo;
- desaparecer cuando no existe objetivo;
- seguir funcionando aunque se cambie de escena;
- NO interferir con las etiquetas WorldLabel;
- NO interferir con PlayerInteraction;
- NO convertirse en un nuevo sistema de navegación paralelo.

---

# INVESTIGACIÓN OBLIGATORIA DEL MARCADOR

Inspecciona:

WorldObjectiveMarker.cs

MissionNavigation.cs

ObjectiveSystem.cs

SceneManager

Inicialización mediante RuntimeInitializeOnLoadMethod

Canvas / CanvasGroup / World Space UI

Cámara principal

MainMenu

SampleScene

Y especialmente:

MissionNavigation.CurrentTarget

Determina primero:

> ¿MissionNavigation está encontrando correctamente el objetivo?

Si CurrentTarget es correcto pero el marcador no aparece:

revisar el problema visual/renderizado.

Si CurrentTarget es null incorrectamente:

revisar la lógica de resolución del objetivo.

No asumir la causa.

Quiero que encuentres la causa real.

---

# COMPORTAMIENTO ESPERADO

Ejemplo:

Objetivo:

"Explora la CPU"

↓

En el mundo:

Punto/flecha amarilla sobre la CPU.

↓

En minimapa:

Punto amarillo correspondiente.

---

Después:

"Conoce los componentes de la CPU"

↓

Marcador sobre el componente correspondiente.

---

Después:

"Explora la RAM"

↓

Marcador en la RAM.

---

Después:

"Explora el almacenamiento"

↓

Marcador en Zone_Storage.

---

Después:

buscar archivo

↓

marcador sobre el Shelf correspondiente o zona relevante.

---

Etc.

No quiero un marcador estático.

Debe seguir utilizando el objetivo dinámico actual.

---

# PARTE 3 — MENÚ DE PAUSA CON ESC

Crear un sistema de pausa.

Cuando el jugador esté dentro de:

SampleScene

y presione:

ESC

debe aparecer un menú de pausa.

Ejemplo:

┌─────────────────────────────┐
│                             │
│          PAUSA              │
│                             │
│       CONTINUAR             │
│       REINICIAR             │
│       VOLVER AL MENÚ        │
│                             │
└─────────────────────────────┘

El diseño debe utilizar el mismo lenguaje visual del Main Menu.

No hacer un menú completamente diferente.

---

# BOTONES

## CONTINUAR

Debe:

- cerrar el menú;
- restaurar el gameplay;
- restaurar Time.timeScale;
- devolver el control al jugador.

---

## REINICIAR

Por ahora puede reiniciar la partida desde el inicio.

Debe:

- volver al estado inicial de SampleScene;
- limpiar progreso de la partida actual;
- cargar nuevamente SampleScene;
- garantizar que Time.timeScale vuelva a 1.

Si implementarlo ahora introduce riesgo innecesario, puedes dejar el botón preparado pero deshabilitado.

PERO NO inventes un sistema complejo de guardado.

---

## VOLVER AL MENÚ

Debe:

1. pausar correctamente;
2. limpiar el estado temporal de la partida;
3. restaurar Time.timeScale = 1;
4. cargar:

MainMenu

El jugador debe volver al Main Menu.

---

# PARTE 4 — IMPORTANTE: PAUSA Y TIME.TIMESCALE

Cuando la pausa esté activa:

Time.timeScale = 0

Pero la UI debe continuar funcionando.

Por lo tanto:

- los botones deben funcionar;
- el menú debe poder cerrarse;
- SceneManager debe funcionar;
- animaciones de UI que necesiten funcionar deben utilizar unscaled time cuando corresponda.

Al salir de pausa:

Time.timeScale = 1

No dejar nunca el proyecto accidentalmente congelado.

---

# PARTE 5 — INPUT DE ESC

Utilizar el sistema de input que ya utiliza el proyecto.

NO importar otro sistema de input.

Si actualmente se utiliza:

Input.GetKeyDown(KeyCode.Escape)

y funciona con la configuración actual:

puede utilizarse.

Si el proyecto utiliza Input System para esta acción:

integrarlo correctamente.

NO mezclar sistemas innecesariamente.

---

# PARTE 6 — ESTADO DE PAUSA

Crear un controlador específico, por ejemplo:

Assets/Scripts/UI/PauseMenuController.cs

o un nombre equivalente.

Debe ser responsable únicamente de:

- abrir pausa;
- cerrar pausa;
- volver al menú;
- reiniciar;
- controlar Time.timeScale;
- controlar visibilidad del Canvas.

No meter lógica de misiones dentro de PauseMenuController.

---

# PARTE 7 — CONTINUAR DESDE MAIN MENU

El botón:

CONTINUAR

actualmente está deshabilitado.

Quiero que ahora tenga comportamiento.

Pero hay que diferenciar:

### CASO A — Hay una partida en progreso

Ejemplo:

Jugador está en SampleScene:

CPU completada
RAM parcialmente completada
Storage todavía pendiente

Presiona ESC.

Luego:

VOLVER AL MENÚ

↓

Main Menu

↓

CONTINUAR

↓

Debe volver a la partida que estaba en progreso.

---

### CASO B — No existe partida en progreso

Al abrir AstroBit por primera vez:

CONTINUAR

debe estar:

DESHABILITADO.

No debe intentar cargar SampleScene como si hubiera una partida guardada.

---

# PARTE 8 — NO NECESITO TODAVÍA GUARDADO EN DISCO

MUY IMPORTANTE:

Por ahora NO quiero implementar un sistema completo de save files.

No crear:

- JSON
- archivos binarios
- PlayerPrefs para todo el progreso
- serialización compleja
- slots de guardado

Quiero inicialmente una solución de:

> "partida en progreso durante la ejecución actual"

Por ejemplo:

GameSession / SessionState / GameState.

Puede mantener:

- que existe una partida;
- escena actual;
- progreso necesario para continuar.

Si volver al Main Menu NO destruye el estado de la partida, entonces CONTINUAR debe poder regresar a SampleScene manteniendo el progreso actual.

---

# PARTE 9 — SINGLETON / PERSISTENCIA

Aquí debes tener especial cuidado.

Actualmente existen sistemas que se inicializan automáticamente.

NO convertir indiscriminadamente todos los sistemas en DontDestroyOnLoad.

No quiero:

- GameHUD duplicado
- Minimap duplicado
- MissionUI duplicado
- ObjectiveSystem duplicado
- StorageMission duplicado
- WorldObjectiveMarker duplicado

La solución debe ser controlada.

Si necesitas un sistema persistente entre MainMenu y SampleScene:

crear únicamente el controlador mínimo necesario.

---

# PARTE 10 — CONTINUAR DEBE MANTENER EL PROGRESO

Ejemplo:

Jugador completa:

ALU
REGISTROS
UNIDAD DE CONTROL
CACHE L1
CACHE L2
CACHE L3

Luego vuelve al menú.

Presiona:

CONTINUAR

Debe regresar con:

6/8

No:

0/8.

---

Otro ejemplo:

Jugador ya recogió:

RAM1
RAM2

Y tiene:

RAM x2

Al volver a continuar:

ese estado debe mantenerse.

---

Otro ejemplo:

Jugador ya encontró el archivo.

Debe seguir encontrándolo como parte del estado actual.

---

NO duplicar objetos ni provocar estados inconsistentes.

---

# PARTE 11 — MAIN MENU

El Main Menu debe comportarse así:

### Primera ejecución

NUEVA PARTIDA     ACTIVO
CONTINUAR         DESHABILITADO
OPCIONES          DESHABILITADO
CRÉDITOS          DESHABILITADO
SALIR             ACTIVO

---

### Después de iniciar una partida

NUEVA PARTIDA     ACTIVO
CONTINUAR         ACTIVO
OPCIONES          DESHABILITADO
CRÉDITOS          DESHABILITADO
SALIR             ACTIVO

---

Si el jugador vuelve al menú mediante:

ESC → VOLVER AL MENÚ

la partida debe seguir disponible para CONTINUAR.

---

# PARTE 12 — NUEVA PARTIDA

"NUEVA PARTIDA" debe iniciar una partida completamente nueva.

Debe limpiar el estado temporal anterior.

Ejemplo:

Partida anterior:

6/8

↓

NUEVA PARTIDA

↓

0/8

---

# PARTE 13 — NO ROMPER EL FLUJO EXISTENTE

El flujo actual debe continuar funcionando:

CPU
↓
RAM
↓
Almacenamiento
↓
Buscar archivo
↓
Server
↓
TV
↓
CPU
↓
Diagnóstico RAM insuficiente
↓
Bodega
↓
RAM3/RAM4
↓
Instalación
↓
RAM
↓
Ejecutar
↓
FinalActivity

No modificar las preguntas de FinalActivity.

No modificar la lógica educativa existente salvo que la auditoría encuentre un bug real.

---

# PARTE 14 — AUDITORÍA DE UI

Revisar también:

- Canvas duplicados
- referencias null
- botones sin eventos
- botones invisibles
- textos cortados
- elementos fuera de pantalla
- anchors incorrectos
- CanvasScaler
- resolución 1920x1080
- escalado
- UI del Main Menu
- UI del HUD
- MissionUI
- minimapa
- marcador 3D

No cambiar diseños que ya funcionan correctamente.

---

# PARTE 15 — AUDITORÍA DE NAVEGACIÓN

Verificar:

- objetivo actual
- MissionNavigation.CurrentTarget
- MissionUI
- minimapa
- WorldObjectiveMarker

Los tres deben representar el mismo objetivo.

No debe ocurrir:

MINIMAPA → objetivo correcto

pero

MUNDO → objetivo inexistente

o

MISIÓN → objetivo diferente.

Debe existir una única fuente de verdad:

MissionNavigation.

---

# PARTE 16 — PRUEBAS OBLIGATORIAS

Realizar pruebas reales.

## TEST 1

Abrir proyecto.

Entrar a MainMenu.

Verificar:

- menú correcto;
- HUD oculto;
- minimapa oculto;
- marcador 3D oculto.

---

## TEST 2

NUEVA PARTIDA.

Entrar a SampleScene.

Verificar:

- HUD;
- misión;
- minimapa;
- marcador amarillo 3D.

---

## TEST 3

Moverse hacia el objetivo inicial.

Confirmar que:

- marcador 3D aparece;
- minimapa muestra objetivo;
- MissionUI muestra el mismo objetivo.

---

## TEST 4

Completar un objetivo.

Confirmar que:

- cambia MissionNavigation.CurrentTarget;
- cambia MissionUI;
- cambia minimapa;
- cambia marcador 3D.

---

## TEST 5

Presionar ESC.

Debe aparecer:

PAUSA

El gameplay debe detenerse.

---

## TEST 6

CONTINUAR.

Debe:

- cerrar pausa;
- Time.timeScale = 1;
- gameplay continuar.

---

## TEST 7

ESC → VOLVER AL MENÚ.

Debe:

- cargar MainMenu;
- no mostrar HUD;
- no mostrar minimapa;
- no mostrar marcador.

---

## TEST 8

Desde MainMenu:

CONTINUAR.

Debe:

- volver a SampleScene;
- mantener progreso;
- mantener estado de partida.

---

## TEST 9

NUEVA PARTIDA.

Debe:

- limpiar progreso;
- comenzar desde 0;
- estado inicial correcto.

---

## TEST 10

Repetir el flujo suficiente para comprobar que:

- StorageMission no se duplica;
- ObjectiveSystem no se duplica;
- GameHUD no se duplica;
- MinimapController no se duplica;
- WorldObjectiveMarker no se duplica.

---

# PARTE 17 — ERRORES

Durante la auditoría:

Si encuentras errores reales:

CORRÍGELOS.

Pero distingue entre:

### ERROR REAL

Algo que rompe funcionalidad.

### WARNING PREEXISTENTE

No introducir cambios innecesarios solo para eliminarlo.

Especial atención a:

FindFirstObjectByType obsoleto.

Si aparece únicamente como warning preexistente y no rompe nada:

no refactorizar todo el proyecto por eso.

---

# PARTE 18 — REGLA DE SEGURIDAD

NO hacer modificaciones masivas.

NO reemplazar sistemas completos.

NO reescribir ObjectiveSystem.

NO reescribir StorageMission.

NO reescribir MissionNavigation.

NO reescribir FinalActivity.

NO reconstruir el Main Menu desde cero.

NO eliminar scripts simplemente porque parezcan poco utilizados.

Antes de eliminar algo:

confirmar referencias reales.

---

# PARTE 19 — RESULTADO FINAL ESPERADO

El juego debe tener este flujo:

MAIN MENU
   │
   ├── NUEVA PARTIDA
   │        ↓
   │     SAMPLESCENE
   │        ↓
   │     GAMEPLAY
   │
   └── CONTINUAR
            ↓
       PARTIDA ACTUAL
            ↓
         SAMPLESCENE


Durante el gameplay:

ESC
 ↓
┌───────────────────────┐
│         PAUSA         │
│                       │
│     CONTINUAR         │
│     REINICIAR         │
│     VOLVER AL MENÚ    │
└───────────────────────┘


Y la navegación:

MissionNavigation
       │
       ├── MissionUI
       ├── Minimap
       └── WorldObjectiveMarker
                ↓
          MISMO OBJETIVO

---

# PARTE 20 — INFORME FINAL

Al terminar NO hagas commit todavía.

Entrega un informe:

## 1. Auditoría

Qué encontraste.

## 2. Bugs corregidos

Especialmente:

- marcador amarillo 3D.

## 3. Archivos creados

Lista.

## 4. Archivos modificados

Lista.

## 5. Sistema de pausa

Cómo funciona.

## 6. Sistema de continuación

Cómo se conserva la partida durante la sesión.

## 7. Main Menu

Cómo interactúa con CONTINUAR / NUEVA PARTIDA.

## 8. Time.timeScale

Cómo se controla.

## 9. Pruebas

Indicar cada TEST y resultado.

## 10. Errores/warnings

Indicar cuáles quedan y si son preexistentes.

## 11. Regresión

Confirmar explícitamente que:

CPU → RAM → almacenamiento → archivo → CPU → RAM → FinalActivity

sigue funcionando.

NO hacer commit.

Quiero probar personalmente todo antes de confirmar.