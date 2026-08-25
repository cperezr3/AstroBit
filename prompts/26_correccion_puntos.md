# PROMPT 24 — Revisión final de etiquetas de ROOM y marcadores amarillos

Proyecto: AstroBit — Unity

---

# CONTEXTO

El proyecto actualmente funciona correctamente y NO quiero cambios de lógica, progresión, misiones, actividades, UI ni sistemas existentes.

Este prompt es exclusivamente para hacer una revisión visual y espacial de:

1. Las etiquetas superiores de las rooms:
   - CPU
   - RAM
   - ALMACENAMIENTO / DISCO DURO

2. Los puntos/marcadores amarillos de navegación de MissionNavigation / WorldObjectiveMarker.

El mapa fue reducido y movido anteriormente, por lo que algunas posiciones antiguas quedaron desfasadas.

---

# REGLA PRINCIPAL

NO MODIFICAR:

- ObjectiveSystem
- StorageMission
- FinalActivity
- GameHUD
- MissionUI
- MinimapController
- PlayerInteraction
- EducationalInteractable
- FileShelf
- StorageServer
- InstallRamSlot
- CollectibleRam
- Inventory
- MissionNavigation (salvo que sea estrictamente necesario para corregir la referencia espacial del objetivo)
- La lógica de progresión
- Las actividades
- Las preguntas
- El flujo CPU → RAM → almacenamiento → archivo → CPU → RAM → Actividad Final
- Los objetos ni geometría del mapa

NO agregar nuevas mecánicas.

NO crear nuevos sistemas.

NO cambiar textos existentes.

El objetivo es únicamente corregir la ubicación, altura, orientación y cobertura espacial de las etiquetas y marcadores.

---

# PARTE 1 — ETIQUETAS SUPERIORES DE LAS ROOMS

Actualmente las etiquetas grandes que identifican las rooms:

- CPU
- RAM
- ALMACENAMIENTO / DISCO DURO

quedaron desfasadas después de que el mapa fue reducido y movido.

Quiero que inspecciones la escena actual y determines exactamente cuáles son los objetos/componentes responsables de esas etiquetas.

No asumas posiciones antiguas.

Usa la geometría REAL actual de cada room.

## CPU

La etiqueta de CPU debe:

- quedar centrada respecto a la room CPU;
- estar suficientemente elevada para no atravesar paredes, techo ni decoración;
- poder verse claramente al entrar en la room;
- abarcar visualmente toda la room como identificador de zona;
- no quedar desplazada hacia un extremo;
- no quedar en medio de los componentes educativos;
- no interferir con las etiquetas de ALU, Registros, Unidad de Control, Cache, etc.

## RAM

Hacer exactamente lo mismo con la etiqueta de RAM:

- centrada respecto a la room RAM;
- elevada correctamente;
- visible al entrar;
- representar claramente toda la room;
- no quedar desplazada por los cambios recientes del mapa.

IMPORTANTE:

La room RAM y sus componentes ya están funcionando correctamente.

NO tocar:

- RAM1
- RAM2
- RAM3
- RAM4
- slots
- instalación
- bodega
- lógica de RAM
- progresión

Solo corregir la etiqueta general de la ROOM RAM.

## ALMACENAMIENTO / DISCO DURO

Corregir igualmente la etiqueta superior de almacenamiento.

Debe:

- quedar centrada respecto a la room de almacenamiento;
- estar elevada correctamente;
- poder verse claramente al entrar;
- representar toda la room;
- no quedar desplazada hacia los shelves, servers o TV;
- no interferir con las etiquetas individuales.

---

# PARTE 2 — NO CONFUNDIR ETIQUETA DE ROOM CON ETIQUETA DE OBJETO

Antes de modificar cualquier cosa, identifica claramente:

- cuál es la etiqueta general de ROOM;
- cuáles son las etiquetas individuales de componentes/objetos.

NO mover las etiquetas individuales de:

- ALU
- REGISTROS
- UNIDAD DE CONTROL
- CACHE
- RAM
- SHELVES
- SERVER
- TV
- RAM3
- RAM4
- etc.

El objetivo es únicamente corregir las etiquetas generales de las rooms.

---

# PARTE 3 — REVISIÓN DE LOS PUNTOS AMARILLOS

Ahora revisar todos los puntos/marcadores amarillos utilizados por el sistema de navegación.

El sistema ya funciona correctamente.

NO quiero cambiar cómo funciona.

Solo quiero corregir los objetivos que hayan quedado mal posicionados después de mover/reducir el mapa.

Especial atención a:

## Cargar el programa en la RAM

El punto amarillo que aparece cuando el objetivo es:

"Cargar programa"

debe estar realmente ubicado en:

- el punto de interacción correspondiente;
- la computadora/slot de RAM correcto;
- la zona donde el jugador realmente debe interactuar.

NO debe aparecer:

- en el centro de la room;
- dentro de una pared;
- lejos del objeto;
- encima de otra RAM;
- en una posición antigua del mapa.

Debe señalar exactamente dónde tiene que ir el jugador.

---

# PARTE 4 — REVISIÓN GENERAL DE TODOS LOS MARCADORES

No revisar solamente "Cargar programa".

Haz una auditoría de todos los objetivos actuales de navegación.

Revisa visualmente y mediante las referencias reales de la escena:

- CPU
- RAM
- almacenamiento
- Shelf correcto
- Server
- TV 32 Inch
- Procesamiento en CPU
- Diagnóstico de RAM
- Bodega
- RAM de bodega
- slots RAM3/RAM4
- Carga del programa
- Ejecución del programa
- cualquier otro MissionStepPoint / objetivo utilizado actualmente.

Para cada uno verifica:

1. ¿El marcador apunta al objeto correcto?
2. ¿Está dentro de la room correcta?
3. ¿Está cerca del punto real de interacción?
4. ¿Es visible desde la zona donde el jugador llega?
5. ¿No está dentro de una pared/objeto?
6. ¿No quedó en una posición antigua del mapa?
7. ¿El minimapa también recibe correctamente esa posición?

Si un marcador ya está correctamente colocado:

**NO TOCARLO.**

Solo corregir los que realmente estén mal.

---

# PARTE 5 — RELACIÓN CON EL SISTEMA ACTUAL

El sistema de navegación ya implementado utiliza:

- MissionNavigation
- WorldObjectiveMarker
- MinimapController
- MissionStepPoint
- referencias a objetos reales de la escena.

Respeta esta arquitectura.

No quiero una segunda solución paralela.

Si MissionNavigation ya obtiene correctamente el Transform de un objetivo, conserva esa lógica.

Si el problema es simplemente que el Transform/punto está mal ubicado en la escena, corrige la escena.

Si existe un MissionStepPoint específicamente creado para una interacción y quedó mal colocado, mueve ese punto.

Solo modifica código si después de inspeccionar la escena confirmas que el problema NO puede solucionarse corrigiendo las posiciones existentes.

---

# PARTE 6 — MÉTODO DE VERIFICACIÓN

Antes de cambiar posiciones:

1. Inspecciona la escena actual.
2. Identifica los GameObjects responsables.
3. Obtén los bounds/posición reales de cada room.
4. Identifica los puntos de interacción reales.
5. Compara la posición actual de etiquetas y marcadores.
6. Corrige solamente los que estén desfasados.

No uses posiciones antiguas del mapa como referencia.

La escena actual es la única fuente de verdad.

---

# PARTE 7 — PRUEBA EN PLAY MODE

Después de corregir:

### ROOM CPU

Entrar a CPU y comprobar:

- etiqueta CPU visible;
- centrada;
- elevada;
- cubre visualmente la room;
- no interfiere con componentes.

### ROOM RAM

Entrar a RAM y comprobar:

- etiqueta RAM visible;
- centrada;
- elevada;
- correcta respecto al tamaño actual de la room.

### ALMACENAMIENTO

Entrar a almacenamiento y comprobar:

- etiqueta correcta;
- centrada;
- elevada;
- representa toda la room.

### NAVEGACIÓN

Probar al menos:

- objetivo de almacenamiento;
- objetivo del Server;
- objetivo de TV;
- procesamiento en CPU;
- diagnóstico RAM;
- bodega;
- instalación RAM3/RAM4;
- carga del programa;
- ejecución del programa.

Especialmente verificar que el marcador de:

"Cargar programa"

apunte al lugar correcto en la RAM.

También comprobar que el punto amarillo del minimapa corresponda a la misma posición real.

---

# RESTRICCIÓN MUY IMPORTANTE

Este prompt NO es para mejorar el sistema.

Es una limpieza/corrección espacial final.

NO:

- rediseñar UI;
- cambiar tamaños de fuentes;
- cambiar colores;
- cambiar misiones;
- cambiar textos;
- cambiar progresión;
- cambiar actividades;
- cambiar mecánicas;
- cambiar objetos;
- agregar scripts innecesarios;
- duplicar sistemas;
- crear nuevos puntos si ya existe uno correcto.

Solo:

**corregir posiciones, alturas, orientación y referencias espaciales de las etiquetas generales de rooms y los marcadores amarillos que estén mal ubicados.**

---

# INFORME FINAL

Al terminar, entrega un informe indicando:

1. Qué etiquetas de ROOM encontraste.
2. Cuáles estaban mal posicionadas.
3. Qué posiciones/ajustes corregiste.
4. Qué marcadores amarillos estaban mal.
5. Cuáles corregiste.
6. Cuáles ya estaban correctos y dejaste intactos.
7. Qué archivos modificaste.
8. Si modificaste código, explicar exactamente por qué.
9. Resultado de las pruebas en Play Mode.
10. Confirmar que no se modificó ninguna lógica de progresión ni actividad.

No hagas commit todavía.