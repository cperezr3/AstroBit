# PROMPT 26 — IMPLEMENTACIÓN DEL MENÚ PRINCIPAL EN ASTROBIT

## CONTEXTO

Proyecto actual:

D:\Unity\Astro

Proyecto externo del menú ya auditado:

D:\Unity\AstroBitMenu\AstroBit1.0

El Prompt 25 realizó una auditoría completa del proyecto del menú y determinó que es seguro integrar su diseño en AstroBit, pero NO se debe copiar el proyecto completo.

La implementación debe hacerse directamente sobre:

D:\Unity\Astro

El gameplay actual de AstroBit debe mantenerse intacto.

---

# OBJETIVO PRINCIPAL

Integrar el menú principal del proyecto:

D:\Unity\AstroBitMenu\AstroBit1.0

dentro de AstroBit.

El resultado debe ser:

ARRANQUE DEL JUEGO
        ↓
MENÚ PRINCIPAL
        ↓
[NUEVA PARTIDA]
        ↓
SampleScene.unity
        ↓
GAMEPLAY ASTROBIT ACTUAL

La escena actual:

Assets/Scenes/SampleScene.unity

es la escena REAL del gameplay de AstroBit.

NO debe reemplazarse, copiarse ni modificarse accidentalmente.

---

# REGLA MÁS IMPORTANTE

NO copies el proyecto del menú completo.

NO copies:

- ProjectSettings/
- SampleScene.unity del proyecto AstroBitMenu
- Assets/Settings/
- FreeLowPolyRobot/
- TutorialInfo/
- TextMesh Pro/Examples & Extras/
- escenas de ejemplo de TMP
- scripts de ejemplo innecesarios
- InputSystem_Actions.inputactions del proyecto externo
- configuraciones que ya existan en AstroBit

Solo utiliza los elementos necesarios para reproducir/integrar el menú.

La auditoría confirmó que el menú real está en:

Assets/Scenes/Menu.unity

y que los elementos relevantes son:

- Menu.unity
- Fondo_Menu.jpeg
- MenuSystem.cs
- Canvas
- botones
- textos TMP

---

# OBJETIVO DE LA INTEGRACIÓN

Crear en AstroBit:

Assets/Scenes/MainMenu.unity

Esta será la nueva escena principal del juego.

Debe conservar la apariencia y distribución visual del menú original tanto como sea posible.

No rediseñes completamente el menú.

No agregues funcionalidades que no fueron solicitadas.

Primero integra correctamente lo que ya existe.

---

# 1. CREAR MAINMENU.UNITY

Crear:

Assets/Scenes/MainMenu.unity

Partiendo de la estructura visual de:

D:\Unity\AstroBitMenu\AstroBit1.0\Assets\Scenes\Menu.unity

Pero reconstruyéndola/copiendo solamente los elementos necesarios.

La escena debe contener:

- Canvas
- fondo
- botones
- textos
- EventSystem
- cámara necesaria para el menú
- iluminación solamente si realmente hace falta

NO copiar la Main Camera del gameplay.

NO copiar Cinemachine.

NO modificar la cámara de SampleScene.

---

# 2. FONDO

Copiar únicamente:

Fondo_Menu.jpeg

al proyecto AstroBit, preferiblemente dentro de una carpeta organizada como:

Assets/Art/UI/

o una ubicación equivalente limpia.

Mantener su apariencia original.

Utilizarlo como fondo del menú.

---

# 3. TEXTMESHPRO

El proyecto del menú utiliza TextMeshPro.

AstroBit actualmente no tiene la misma estructura de TMP embebida.

Antes de copiar una carpeta completa de TextMeshPro:

- comprobar si TMP ya está instalado mediante Package Manager.
- si ya existe, reutilizarlo.
- si falta, instalar/importar solamente lo necesario.
- NO copiar Examples & Extras.
- NO copiar las aproximadamente 30 escenas/scripts de ejemplo.

No introducir dependencias innecesarias.

---

# 4. JERARQUÍA DEL MENÚ

Mantener aproximadamente la estructura visual del menú original:

Canvas
├── Fondo
├── Título / textos
├── Nueva Partida
├── Continuar
├── Opciones
├── Créditos
└── Salir

No es obligatorio conservar exactamente los nombres internos si eso genera conflictos.

Lo importante es conservar:

- posición
- tamaño
- textos
- apariencia
- orden
- navegación visual

del menú original.

---

# 5. SCRIPT DEL MENÚ

El proyecto externo tiene:

MenuSystem.cs

La auditoría confirmó que actualmente la clase se llama:

NewMonoBehaviourScript

y únicamente contiene la lógica real de:

- Jugar()
- Salir()

No copiarla ciegamente.

Crear un script limpio para AstroBit, por ejemplo:

Assets/Scripts/UI/MainMenuController.cs

o reutilizar/adaptar el existente si es técnicamente más limpio.

IMPORTANTE:

NO utilizar:

SceneManager.LoadScene(buildIndex + 1)

Cambiarlo por carga explícita:

SceneManager.LoadScene("SampleScene");

De esta forma no dependemos de índices accidentales.

---

# 6. NUEVA PARTIDA

El botón:

"Nueva Partida"

debe cargar:

SampleScene

del proyecto AstroBit.

Debe ser exactamente:

SceneManager.LoadScene("SampleScene");

No cargar el SampleScene del proyecto externo.

No crear una segunda escena de gameplay.

No duplicar el mapa.

---

# 7. BUILD SETTINGS

Configurar las escenas para que el flujo sea:

[0] MainMenu
[1] SampleScene

Pero antes:

- verificar qué escenas existen actualmente.
- no eliminar escenas útiles.
- no cambiar el orden de SampleScene sin necesidad.
- asegurar que MainMenu sea la escena inicial.

La escena de gameplay debe continuar siendo:

Assets/Scenes/SampleScene.unity

---

# 8. MUY IMPORTANTE: SINGLETONS DE ASTROBIT

AstroBit actualmente posee sistemas que pueden inicializarse automáticamente / persistir entre escenas, incluyendo sistemas como:

- GameHUD
- ObjectiveSystem
- StorageMission
- MissionNavigation
- MinimapController
- etc.

La auditoría identificó esto como el principal riesgo de integración.

Al entrar en:

MainMenu

NO deben aparecer:

- HUD de gameplay
- panel de misión
- minimapa
- marcador amarillo
- objetivo/pista
- inventario RAM
- elementos del gameplay

El menú debe verse completamente limpio.

---

# 9. SOLUCIÓN PARA LOS SISTEMAS PERSISTENTES

NO rompas los singletons actuales.

NO elimines DontDestroyOnLoad de los sistemas de AstroBit solamente para solucionar el menú.

NO modifiques innecesariamente:

- ObjectiveSystem
- StorageMission
- MissionNavigation
- MinimapController
- GameHUD
- FinalActivity

Primero inspecciona cómo se inicializan realmente.

Implementa una solución mínima y robusta para que:

MainMenu
    ↓
no muestre sistemas de gameplay

pero:

MainMenu
    ↓
Nueva Partida
    ↓
SampleScene
    ↓
todos los sistemas actuales funcionen normalmente

Si es necesario introducir una pequeña comprobación de escena para evitar la creación visual del HUD en MainMenu, debe hacerse de forma mínima y sin alterar el comportamiento de SampleScene.

NO dupliques sistemas.

NO crees versiones "MenuGameHUD" o similares.

---

# 10. BOTÓN CONTINUAR

Actualmente NO existe sistema de guardado.

Por lo tanto:

NO implementar todavía un sistema de Save/Load.

No inventar datos.

Para esta implementación:

- puede permanecer deshabilitado visualmente
- o puede permanecer sin funcionalidad

Preferencia:

mantenerlo visible pero deshabilitado si eso coincide visualmente con el menú original.

No debe generar errores al hacer clic.

---

# 11. BOTÓN OPCIONES

Actualmente NO existe sistema de configuración.

NO implementar todavía:

- volumen
- resolución
- gráficos
- sensibilidad
- accesibilidad
- idioma

Eso se implementará en otro prompt.

Por ahora:

- mantener el botón visible
- dejarlo deshabilitado

No crear paneles falsos.

---

# 12. BOTÓN CRÉDITOS

No existe sistema de créditos todavía.

Por ahora:

- mantener visible
- dejar deshabilitado

No crear contenido inventado.

---

# 13. BOTÓN SALIR

Mantener la lógica:

Application.Quit();

En Editor debe poder mostrar un Debug.Log indicando que se solicitó salir.

En Build debe cerrar correctamente el juego.

No generar errores.

---

# 14. INPUT

AstroBit ya utiliza Input System.

No copiar:

InputSystem_Actions.inputactions

del proyecto externo.

Reutilizar el sistema de entrada que ya tiene AstroBit.

El menú debe poder navegarse con:

- mouse
- teclado si el EventSystem actual lo permite

No implementar todavía controles complejos.

---

# 15. EVENTSYSTEM

Verificar que MainMenu tenga:

EventSystem

y:

InputSystemUIInputModule

si es el sistema compatible con el proyecto actual.

No duplicar EventSystems.

No importar uno innecesario si ya existe.

---

# 16. ACCESIBILIDAD

NO implementar todavía un menú completo de accesibilidad.

Pero preparar el menú de forma que posteriormente pueda agregarse:

- tamaño de texto
- contraste
- volumen
- modo daltónico
- navegación por teclado
- etc.

No modificar todavía la jugabilidad.

---

# 17. NO TOCAR SAMPLESCENE

Esta regla es CRÍTICA.

La implementación del menú NO debe modificar:

Assets/Scenes/SampleScene.unity

salvo que sea absolutamente imprescindible para una integración técnica.

No mover:

- jugador
- cámara
- rooms
- CPU
- RAM
- almacenamiento
- bodega
- servidores
- shelves
- TV
- minimapa
- marcadores
- etiquetas
- puntos de misión

El gameplay actual ya está probado y funcional.

Debe mantenerse exactamente igual.

---

# 18. NO TOCAR LA PROGRESIÓN

No modificar:

- ObjectiveSystem
- StorageMission
- FinalActivity
- Inventory
- MissionNavigation
- FileShelf
- StorageServer
- InstallRamSlot
- CollectibleRam
- MissionStepPoint

salvo que una modificación mínima sea estrictamente necesaria para evitar que estos sistemas aparezcan en el menú.

La progresión:

CPU
→ RAM
→ almacenamiento
→ archivo
→ TV
→ CPU
→ RAM insuficiente
→ bodega
→ RAM3/RAM4
→ ejecución
→ Actividad Final

debe quedar intacta.

---

# 19. LIMPIEZA

NO copiar basura del proyecto externo.

No copiar:

- TutorialInfo
- escenas de ejemplo
- robots
- scripts de ejemplo
- assets no utilizados
- configuraciones del proyecto externo

El proyecto AstroBit debe quedar limpio.

---

# 20. PRUEBAS OBLIGATORIAS

Después de implementar:

## TEST 1 — Abrir MainMenu

Debe mostrar:

- fondo
- título
- botones
- UI del menú

NO debe mostrar:

- HUD de AstroBit
- minimapa
- misión
- marcador amarillo
- inventario
- elementos de gameplay

---

## TEST 2 — Nueva Partida

Desde:

MainMenu

hacer:

[Nueva Partida]

Debe cargar:

SampleScene

---

## TEST 3 — Gameplay

Una vez dentro de SampleScene:

confirmar que vuelven a aparecer normalmente:

- HUD
- misión
- minimapa
- navegación
- marcador amarillo
- jugador
- gameplay

y que funcionan igual que antes.

---

## TEST 4 — Regresión

Comprobar como mínimo:

CPU
→ RAM
→ almacenamiento

y confirmar que la integración del menú no rompió la progresión.

No es necesario repetir absolutamente todo el recorrido si el tiempo de prueba es excesivo, pero sí verificar que los sistemas principales siguen inicializando correctamente.

---

## TEST 5 — Volver al menú

Si no existe todavía botón "Volver al menú" dentro del gameplay:

NO agregarlo.

Solo comprobar que:

MainMenu → SampleScene

funciona correctamente.

---

# 21. CONTROL DE ARCHIVOS

Antes de terminar:

hacer una lista de:

### Archivos nuevos

### Archivos modificados

### Archivos copiados desde AstroBitMenu

### Archivos que deliberadamente NO fueron copiados

Confirmar que NO se copió accidentalmente:

SampleScene.unity
ProjectSettings
TutorialInfo
FreeLowPolyRobot
TMP Examples

---

# 22. REGLA DE IMPLEMENTACIÓN

No hagas una solución gigantesca.

Quiero una integración limpia, simple y mantenible.

Preferencia:

MainMenu.unity
+
MainMenuController.cs
+
Fondo_Menu.jpeg
+
TMP Essentials si realmente hacen falta

y el mínimo cambio necesario para que los sistemas persistentes de AstroBit no aparezcan sobre el menú.

---

# 23. CRITERIO FINAL DE ÉXITO

La experiencia final debe sentirse así:

ABRIR ASTROBIT

        ↓

┌──────────────────────────────┐
│          ASTROBIT            │
│                              │
│       NUEVA PARTIDA          │
│       CONTINUAR              │
│       OPCIONES               │
│       CRÉDITOS               │
│       SALIR                  │
└──────────────────────────────┘

        ↓

NUEVA PARTIDA

        ↓

SampleScene

        ↓

GAMEPLAY ACTUAL DE ASTROBIT

Sin perder ninguna funcionalidad existente.

---

# 24. AL FINAL DEL TRABAJO

No hagas commit automáticamente.

Primero entrega un informe:

1. Qué archivos nuevos creaste.
2. Qué archivos modificaste.
3. Qué elementos del proyecto externo copiaste.
4. Qué elementos NO copiaste.
5. Cómo resolviste los singletons/DontDestroyOnLoad.
6. Cómo configuraste MainMenu → SampleScene.
7. Resultado de las pruebas.
8. Si hubo errores o warnings.
9. Si el gameplay actual quedó intacto.
10. Qué queda pendiente para un futuro sistema de Opciones/Accesibilidad/Guardado.

IMPORTANTE:

NO implementar todavía:
- guardado
- cargar partida
- opciones
- accesibilidad completa
- créditos funcionales
- audio
- loading screen
- animaciones complejas

Este prompt es EXCLUSIVAMENTE para integrar correctamente el menú principal existente dentro de AstroBit.