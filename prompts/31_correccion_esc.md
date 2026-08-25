# PROMPT 31 — CORRECCIÓN UI: FOCO DEL MENÚ DE PAUSA + TAMAÑO DEL MENÚ PRINCIPAL

## CONTEXTO

El juego actualmente funciona correctamente.

Quiero hacer únicamente dos correcciones visuales/UX:

1. Corregir el borde celeste que queda seleccionado en el menú de pausa después de usar "Continuar".
2. Reducir moderadamente el tamaño general del Main Menu, porque actualmente aparece demasiado grande y algunos elementos no se visualizan correctamente.

NO quiero cambios de gameplay ni de progresión.

---

# PARTE 1 — MENÚ DE PAUSA

## PROBLEMA

Cuando entro al juego y presiono `ESC` por primera vez:

- El menú de pausa aparece correctamente.
- Ningún botón queda marcado visualmente.

Si pulso:

`CONTINUAR`

la partida continúa correctamente.

Pero posteriormente vuelvo a presionar `ESC`:

- El menú vuelve a aparecer.
- El botón "Continuar" queda con un borde celeste.
- Parece que Unity está conservando el último `Selectable` seleccionado.

Quiero que cada nueva apertura del menú de pausa empiece visualmente limpia.

---

## COMPORTAMIENTO ESPERADO

Cada vez que se abra el menú con `ESC`:

```text
PAUSA

CONTINUAR
VOLVER AL MENÚ PRINCIPAL

Ningún botón debe estar seleccionado automáticamente.

No debe aparecer el borde celeste de Selected.

Sin embargo:

El hover del mouse debe seguir funcionando.
Highlighted debe seguir funcionando.
Los botones deben seguir siendo completamente interactuables.
La navegación existente mediante teclado/controlador, si existe, no debe romperse.

La solución debe limpiar únicamente la selección persistente del EventSystem.

Investiga primero PauseMenuController.cs y determina cómo se está conservando el objeto seleccionado.

Si corresponde, utilizar una solución equivalente a:

EventSystem.current.SetSelectedGameObject(null);

pero solamente después de comprobar que es compatible con el flujo actual.

También evalúa si es necesario limpiar la selección al pulsar "Continuar".

NO desactives los componentes Selectable.

NO elimines los estados Normal, Highlighted, Pressed o Selected.

PARTE 2 — REDUCIR MODERADAMENTE EL MAIN MENU
PROBLEMA

El Main Menu actualmente está demasiado grande.

Los elementos del menú ocupan demasiado espacio y algunos elementos no aparecen correctamente dentro de la pantalla.

Quiero reducirlo moderadamente, no rehacer el diseño.

El diseño actual me gusta.

NO quiero cambiar:

Fondo.
Tipografía.
Colores.
Estilo visual.
Textos.
Orden de botones.
Lógica de botones.
MainMenuController.
Navegación entre escenas.

Solamente quiero ajustar la escala/tamaño del conjunto visual para que quede correctamente contenido dentro de la pantalla.

OBJETIVO VISUAL

Reducir proporcionalmente el tamaño del contenido principal del menú.

Por ejemplo, si actualmente está diseñado para ocupar prácticamente toda la pantalla:

┌─────────────────────────────────────────┐
│                                         │
│              ASTROBIT                   │
│                                         │
│        [ NUEVA PARTIDA ]                │
│        [ CONTINUAR ]                    │
│        [ OPCIONES ]                     │
│        [ CRÉDITOS ]                     │
│        [ SALIR ]                         │
│                                         │
└─────────────────────────────────────────┘

quiero que quede con más margen alrededor:

┌─────────────────────────────────────────┐
│                                         │
│             ASTROBIT                    │
│                                         │
│         [ NUEVA PARTIDA ]               │
│         [ CONTINUAR ]                   │
│         [ OPCIONES ]                    │
│         [ CRÉDITOS ]                    │
│         [ SALIR ]                       │
│                                         │
└─────────────────────────────────────────┘

La diferencia debe ser MODERADA.

No reducirlo hasta hacerlo pequeño.

INVESTIGACIÓN ANTES DE MODIFICAR

Antes de tocar la escena:

Inspecciona MainMenu.unity.
Identifica el Canvas principal.
Identifica el contenedor que agrupa el título y botones.
Comprueba el CanvasScaler.
Comprueba si el problema viene de:
escala del contenedor,
tamaño de botones,
posiciones,
anchors,
CanvasScaler,
o una combinación.
Determina cuál es el cambio mínimo que permita que el menú entre correctamente en pantalla.

IMPORTANTE:

No cambies arbitrariamente posiciones individuales si el problema puede resolverse escalando correctamente el contenedor principal.

RESTRICCIONES

NO modificar:

ObjectiveSystem
StorageMission
MissionNavigation
MinimapController
GameHUD
WorldObjectiveMarker
PlayerInteraction
EducationalInteractable
FinalActivity
Inventory
CollectibleRam
InstallRamSlot
FileShelf
StorageServer

No modificar:

SampleScene.unity salvo que sea absolutamente necesario.
Progresión del juego.
Sistema de misiones.
Sistema de pausa.
Sistema de guardado.
Flujo CPU → RAM → almacenamiento.
Minimap.
Marcadores amarillos.

Para el Main Menu, modificar únicamente los elementos visuales necesarios de:

MainMenu.unity

Para el problema del foco, modificar únicamente:

PauseMenuController.cs

o el componente estrictamente necesario relacionado con el EventSystem.

PRUEBAS OBLIGATORIAS
TEST 1 — MAIN MENU

Presionar Play.

Verificar:

Main Menu aparece correctamente.
Todo el contenido cabe dentro de la pantalla.
Ningún botón queda cortado.
El diseño mantiene su apariencia original.
El tamaño se redujo moderadamente, no excesivamente.
TEST 2 — NUEVA PARTIDA

Desde Main Menu:

NUEVA PARTIDA

Verificar:

Carga SampleScene.
HUD funciona.
Minimap funciona.
Misiones funcionan.
No se modificó gameplay.
TEST 3 — PAUSA

Dentro del juego:

ESC

Verificar:

Pause Menu aparece.
Ningún botón tiene selección celeste automática.
TEST 4 — CONTINUAR

Pulsar:

CONTINUAR

Después:

ESC

Verificar:

Pause Menu vuelve a aparecer.
"Continuar" NO queda marcado con borde celeste.
Ningún otro botón queda seleccionado automáticamente.
TEST 5 — HOVER

Mover el mouse sobre:

CONTINUAR

Verificar:

El estado Highlighted aparece normalmente.
Al retirar el mouse, desaparece.
No se rompió la interacción normal de los botones.
TEST 6 — VOLVER AL MENÚ

Desde Pause Menu:

VOLVER AL MENÚ PRINCIPAL

Verificar:

Regresa correctamente a Main Menu.
El Main Menu mantiene el nuevo tamaño reducido.
No queda ningún elemento del Pause Menu visible.
No aparecen duplicados.
CRITERIO DE ÉXITO

El trabajo está terminado solamente si:

Pause Menu
No queda ningún botón seleccionado automáticamente al abrirlo.
Desaparece el borde celeste persistente.
Hover/Highlighted continúa funcionando.
Continuar sigue funcionando.
Volver al menú sigue funcionando.
Main Menu
El menú se ve moderadamente más pequeño.
Todo cabe correctamente en pantalla.
No hay botones/textos cortados.
Mantiene el mismo diseño visual.
No se alteró la lógica del menú.
Proyecto
No hay errores nuevos.
No hay warnings nuevos relacionados con estos cambios.
No se modificó ninguna mecánica de gameplay.
No se modificaron sistemas que no sean necesarios para estas dos correcciones.
INFORME FINAL

Al terminar, indica:

Qué causaba el borde celeste persistente.
Qué archivo se modificó para solucionarlo.
Qué cambio puntual se hizo.
Qué elemento del Main Menu se estaba sobredimensionando.
Qué ajuste se hizo para reducirlo.
Qué pruebas se ejecutaron.
Si aparecieron errores/warnings.
Confirmar que no se modificó la lógica de gameplay.