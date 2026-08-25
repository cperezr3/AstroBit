# PROMPT 29 — CORRECCIONES DE MENÚ, MISIONES, ETIQUETAS Y NAVEGACIÓN

## CONTEXTO

Ya probé personalmente el estado actual de AstroBit después de la implementación del Main Menu, pausa, navegación y sistema de continuación.

El gameplay funciona correctamente en general.

NO quiero una nueva implementación grande ni una refactorización.

Quiero corregir únicamente las observaciones que encontré durante la prueba.

---

# 1. MAIN MENU — DEBE SER LA PRIMERA ESCENA

## PROBLEMA

Actualmente al ejecutar el proyecto:

→ entra directamente a SampleScene / gameplay.

Esto NO debe ocurrir.

## COMPORTAMIENTO CORRECTO

Al presionar Play desde Unity:

MAIN MENU
↓
[NUEVA PARTIDA]
↓
SampleScene

El Main Menu debe ser SIEMPRE la primera escena que se carga al iniciar el juego.

Revisar:

ProjectSettings/EditorBuildSettings.asset

y cualquier otro mecanismo que esté haciendo que SampleScene se cargue primero.

El orden debe ser:

[0] MainMenu.unity
[1] SampleScene.unity

NO crear otro sistema de inicio innecesario.

NO duplicar MainMenu.

NO modificar el gameplay para solucionar esto.

---

# 2. PANEL DE MISIONES — MEJORAR TAMAÑO Y DISTRIBUCIÓN

Actualmente el panel de misiones funciona, pero visualmente tiene varios problemas.

El panel es un poco pequeño y algunas palabras no entran completas.

Además:

Las misiones empiezan demasiado abajo.

Actualmente queda demasiado espacio vacío en la parte superior del panel.

Esto hace que visualmente parezca que el contenido está "hundido" hacia la parte media/baja.

---

## OBJETIVO VISUAL

Quiero conservar exactamente el estilo actual del MissionUI.

NO rediseñarlo.

Solo mejorarlo.

### Cambios:

### A. Agrandar ligeramente el panel

Aumentar moderadamente:

- ancho
- alto

Lo suficiente para que las palabras y textos de las misiones entren completos.

NO hacerlo exageradamente grande.

Debe seguir siendo un panel compacto de HUD.

---

### B. Aumentar ligeramente la fuente

Aumentar ligeramente el tamaño de la fuente de las misiones.

No exagerar.

La prioridad es:

1. Legibilidad.
2. Que las palabras completas entren.
3. Mantener el diseño compacto.

---

### C. Reposicionar las misiones

Actualmente las misiones comienzan demasiado abajo.

Quiero que la primera misión empiece mucho más cerca de la parte superior interna del panel.

Debe quedar aproximadamente:

┌───────────────────────────────┐
│ MISIÓN                        │
│                               │
│ ✓ Explorar la CPU             │
│ ✓ Conocer los componentes     │
│ ● Explorar la memoria RAM     │
│                               │
│                               │
└───────────────────────────────┘

Y NO:

┌───────────────────────────────┐
│ MISIÓN                        │
│                               │
│                               │
│                               │
│                               │
│ ✓ Explorar la CPU             │
│ ✓ Conocer...                  │
│ ● Explorar...                 │
└───────────────────────────────┘

---

### D. Las misiones completadas deben desplazarse hacia abajo

Mantener el comportamiento conceptual actual:

La misión actual aparece resaltada.

Cuando se completa:

✓ pasa a formar parte del historial.

La siguiente misión aparece debajo/como actual.

Quiero que el listado conserve un orden vertical natural:

1. Primera misión
2. Segunda misión
3. Tercera misión
4. etc.

Las misiones deben comenzar desde arriba del área de contenido y ocupar el espacio hacia abajo.

NO cambiar la lógica de progresión.

NO cambiar ObjectiveSystem.

NO cambiar MissionNavigation.

NO cambiar las misiones.

Solo modificar presentación, tamaño, posiciones y layout.

---

# 3. ETIQUETA "PROCESAMIENTO"

## PROBLEMA

Cuando aparece el paso:

[E] Procesar archivo

y el jugador interactúa:

[E]

la acción funciona.

PERO la etiqueta:

"PROCESAMIENTO"

sigue apareciendo.

## COMPORTAMIENTO DESEADO

Antes de interactuar:

[PROCESAMIENTO]
[Procesar archivo]
[E]

Después de presionar E:

→ se procesa correctamente
→ el objetivo cambia
→ la etiqueta "PROCESAMIENTO" debe desaparecer.

No debe quedar flotando después de completar ese paso.

---

## IMPORTANTE

No eliminar la interacción.

No eliminar el punto de procesamiento.

No cambiar el gating.

No cambiar StorageMission.

No cambiar el flujo:

TV
→ CPU
→ RAM

Simplemente hacer que la etiqueta desaparezca cuando ese paso haya sido completado.

Utilizar el mecanismo existente de etiquetas/interacciones si es posible.

No crear un sistema paralelo.

---

# 4. PUNTO AMARILLO — "CARGAR PROGRAMA"

## PROBLEMA

El punto amarillo que indica:

"Cargar programa"

actualmente aparece en una posición incorrecta.

Quiero que el punto amarillo quede claramente:

> ENCIMA DE LA MEMORIA RAM

No debe aparecer en una posición genérica de la habitación.

---

## OBJETIVO

Cuando el jugador llegue a la fase:

"Cargar programa"

el marcador 3D debe apuntar visualmente hacia la memoria RAM correspondiente.

Idealmente:

- encima de RAM1/RAM2 o del punto de carga que corresponda;
- claramente visible;
- suficientemente separado de la geometría para que no atraviese el modelo;
- coherente con el punto amarillo del minimapa.

---

## IMPORTANTE

NO modificar el minimapa.

NO crear un segundo marcador.

NO modificar MissionNavigation innecesariamente.

El objetivo debe seguir siendo resuelto mediante el sistema actual de navegación.

Simplemente corregir el Transform/objetivo utilizado para esa fase.

---

# 5. CONSISTENCIA ENTRE NAVEGACIÓN Y MINIMAPA

Después de corregir el punto de "Cargar programa":

comprobar que:

MissionNavigation
↓
WorldObjectiveMarker
↓
MinimapController

siguen representando el mismo objetivo.

Debe ocurrir:

MUNDO:
🟡 encima de RAM

MINIMAPA:
🟡 ubicación correspondiente a RAM

No quiero que uno indique RAM y otro indique otra posición.

---

# 6. NO TOCAR

Salvo que sea estrictamente necesario para resolver los problemas anteriores, NO modificar:

- ObjectiveSystem
- StorageMission
- FinalActivity
- PlayerInteraction
- EducationalInteractable
- Inventory
- CollectibleRam
- InstallRamSlot
- FileShelf
- StorageServer
- MissionNavigation
- MinimapController
- GameHUD
- MainMenuController
- PauseMenuController
- lógica de CPU
- lógica de RAM
- lógica de almacenamiento
- flujo del archivo
- preguntas de FinalActivity
- sistema de guardado temporal/continuación

No realizar refactorización general.

---

# 7. PRUEBAS OBLIGATORIAS

Después de realizar los cambios, probar:

## TEST 1 — Inicio

Presionar Play.

Resultado esperado:

MainMenu

NO SampleScene.

---

## TEST 2 — Nueva partida

MainMenu:

[NUEVA PARTIDA]

↓

SampleScene.

Debe funcionar normalmente.

---

## TEST 3 — MissionUI

Comprobar:

- panel ligeramente más grande;
- fuente ligeramente más grande;
- textos completos;
- primera misión comienza cerca de la parte superior;
- no hay espacio vacío excesivo;
- las misiones completadas se mantienen ordenadas hacia abajo.

---

## TEST 4 — Procesamiento

Llegar a:

[E] Procesar archivo

Presionar E.

Confirmar:

- procesamiento funciona;
- etiqueta "PROCESAMIENTO" desaparece;
- siguiente objetivo aparece;
- no queda etiqueta flotante.

---

## TEST 5 — Cargar programa

Llegar al paso:

"Cargar programa"

Confirmar:

- punto amarillo visible en el mundo;
- está encima de la memoria RAM;
- no está dentro de una pared/modelo;
- el minimapa muestra el mismo objetivo.

---

## TEST 6 — Regresión

Confirmar que siguen funcionando:

- pausa con ESC;
- volver al menú;
- continuar partida;
- navegación;
- minimapa;
- HUD;
- CPU;
- RAM;
- almacenamiento;
- flujo del archivo.

No repetir obligatoriamente todo el juego si no es necesario, pero comprobar que estas modificaciones no hayan roto ningún sistema.

---

# 8. REVISIÓN FINAL

Al terminar, entrega:

## Cambios realizados

Lista de archivos modificados.

## Problemas encontrados

Indicar si encontraste algún problema adicional durante la revisión.

## Pruebas

Indicar resultado de cada TEST.

## Errores / warnings

Indicar si apareció alguno nuevo.

NO hacer commit todavía.

---

# 9. PROPUESTA DE ACCESIBILIDAD

IMPORTANTE:

Después de terminar la implementación anterior, quiero que me propongas UNA idea de accesibilidad que tenga sentido para AstroBit.

NO la implementes todavía.

Quiero que analices el proyecto actual y propongas una mejora concreta, por ejemplo:

- modo de alto contraste;
- tamaño de texto ajustable;
- reducción de efectos visuales;
- indicadores visuales + textuales para interacciones;
- modo de navegación simplificado;
- configuración de duración de mensajes;
- colores alternativos para personas con daltonismo;
- subtítulos para sonidos importantes;
- etc.

Pero NO quiero una lista genérica.

Quiero:

### PROPUESTA DE ACCESIBILIDAD

**Nombre:**
[Nombre]

**Problema que soluciona:**
[Explicación]

**Cómo funcionaría en AstroBit:**
[Explicación concreta]

**Dónde se integraría:**
[Menú / HUD / interacción / navegación, etc.]

**Por qué sería útil en este juego:**
[Explicación]

**Dificultad estimada:**
Baja / Media / Alta

Y una recomendación de cuál implementarías primero.

NO implementarla todavía.