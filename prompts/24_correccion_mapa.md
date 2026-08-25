# PROMPT 25 — Corrección de posiciones y etiquetas tras rediseño del mapa

## CONTEXTO

Proyecto: **AstroBit — Unity**

El proyecto ya tiene funcionando correctamente:

- Progresión CPU → RAM → Almacenamiento.
- Las 8 actividades educativas CPU/RAM.
- Bodega con módulos RAM.
- Inventario.
- Instalación de RAM3/RAM4.
- Flujo de archivo:
  - buscar archivo en Shelf;
  - recoger archivo;
  - llevarlo al almacenamiento;
  - abrir/enviar desde TV;
  - procesar en CPU;
  - cargar en RAM;
  - ejecutar programa;
  - Actividad Final.
- MissionNavigation.
- MissionUI.
- WorldObjectiveMarker.
- Minimap.
- Sistema de objetivos y pistas.
- FinalActivity.

TODO ESO YA FUNCIONA.

Recientemente modifiqué manualmente el mapa:

- Lo hice más pequeño.
- Recorté algunas zonas en ancho y largo.
- Moví el conjunto de habitaciones.
- Principalmente cambiaron posiciones en el eje Z.
- Los objetos funcionales siguen existiendo.
- Las nuevas posiciones del mapa son ahora la referencia correcta.

Después de este cambio, algunas etiquetas y puntos de navegación quedaron desubicados.

---

# OBJETIVO

Corregir únicamente:

- posiciones;
- visibilidad;
- ubicación de etiquetas;
- ubicación de puntos amarillos;
- ubicación de puntos de misión;
- duración de un feedback;
- contenido del diagnóstico de RAM;
- ubicación de los puntos de interacción del flujo del archivo.

NO rediseñar el sistema.

NO crear nuevas mecánicas.

NO modificar el flujo de jugabilidad existente.

---

# REGLA PRINCIPAL

## PRIMERO INSPECCIONAR, DESPUÉS MODIFICAR

Antes de cambiar coordenadas:

1. Inspeccionar la escena actual.
2. Comparar la posición actual de las rooms.
3. Localizar físicamente:
   - CPU;
   - RAM;
   - Bodega;
   - Disco Duro / Almacenamiento;
   - ALU;
   - Unidad de Control;
   - RAM3;
   - RAM4;
   - TV 32 Inch;
   - server;
   - puntos MissionStepPoint;
   - puntos de navegación;
   - WorldObjectiveMarker;
   - etiquetas WorldLabel.
4. Identificar cuáles objetos se desplazaron debido al rediseño.
5. Utilizar las posiciones actuales de los componentes reales como referencia.

NO asumir las coordenadas anteriores del mapa.

El mapa actual es la nueva fuente de verdad.

---

# 1. ETIQUETA "PROCESAMIENTO DE ARCHIVO"

Actualmente existe una etiqueta relacionada con:

> PROCESAMIENTO DE ARCHIVO

El problema es doble:

### Visibilidad

NO debe aparecer desde el comienzo.

Debe aparecer únicamente cuando el flujo del archivo haya llegado al punto correspondiente después de enviarlo desde el almacenamiento.

Es decir:

```text
Shelf
 ↓
Archivo encontrado
 ↓
Server
 ↓
TV / enviar archivo
 ↓
CPU
 ↓
APARECE "Procesamiento de archivo"

Antes de que el archivo sea enviado desde el Disco Duro/almacenamiento:

PROCESAMIENTO DE ARCHIVO

debe permanecer completamente oculto.

NO cambiar el gating de la misión.

Utilizar el estado existente de StorageMission para determinar cuándo debe aparecer.

2. POSICIÓN DE "PROCESAMIENTO DE ARCHIVO"

Después de mover el mapa, la etiqueta quedó aproximadamente:

en medio de ALU y Unidad de Control.

Eso no es correcto.

Mover el punto/etiqueta a un lugar que tenga sentido visualmente dentro de la zona CPU.

Debe estar:

cerca de un componente de CPU;
fuera de la geometría;
claramente visible;
sin quedar encima de ALU;
sin quedar en medio del espacio entre ALU y Unidad de Control;
sin bloquear otras etiquetas.

Preferencia:

cerca de la Unidad de Control

porque el flujo actual utiliza la Unidad de Control como representación del procesamiento del archivo.

NO cambiar la lógica.

Solo corregir la posición.

3. "CARGAR ARCHIVO EN LA MEMORIA"

Después de reducir/mover el mapa, la etiqueta/punto relacionado con:

Cargar archivo en la memoria

quedó desubicado.

Encontrar el MissionStepPoint correspondiente a:

FileMission_RamLoad

o el objeto equivalente actualmente utilizado.

Moverlo para que quede:

junto a RAM1

Debe estar claramente asociado a la zona RAM.

No dejarlo:

en el centro de la habitación;
alejado de RAM;
dentro de otro objeto;
flotando en una posición que no tenga relación con la RAM.

Utilizar RAM1 como referencia física.

NO cambiar el gating.

NO cambiar el nombre.

NO cambiar la lógica de StorageMission.

4. "EJECUTAR PROGRAMA"

El último punto de interacción antes de FinalActivity corresponde a:

Ejecutar programa

Actualmente debe estar asociado a RAM.

Quiero que quede específicamente:

junto a RAM3

No RAM1.

No RAM2.

No en el centro de la room.

No crear una RAM nueva.

RAM3 ya existe y se activa durante el flujo.

El punto de interacción debe estar correctamente colocado junto a RAM3.

El flujo debe seguir siendo:

RAM3
 ↓
[E] Ejecutar programa
 ↓
FinalActivity

NO modificar la lógica de FinalActivity.

5. ETIQUETAS SUPERIORES DE LAS ROOMS

Al reducir y mover el mapa, las etiquetas superiores de:

CPU
RAM
Bodega
Disco Duro

quedaron desubicadas.

Quiero únicamente corregir su posición.

Deben permanecer:

arriba de cada habitación correspondiente.

Usar la geometría actual de cada room como referencia.

No cambiar:

texto;
fuente;
estilo;
tamaño;
comportamiento.

Solo mover su posición.

El cambio de mapa fue principalmente en Z, así que verificar especialmente la coordenada Z.

No asumir que solamente hay que cambiar Z si al inspeccionar se encuentra que otra coordenada también necesita corrección.

6. PUNTO AMARILLO DEL DISCO DURO

El marcador amarillo de navegación correspondiente al:

Disco Duro / Almacenamiento

quedó desubicado debido al nuevo tamaño/posición del mapa.

Mover el objetivo real utilizado por:

WorldObjectiveMarker

para que apunte correctamente a la room del Disco Duro.

Debe quedar:

dentro de la room correcta;
visualmente centrado respecto al destino;
no fuera del mapa;
no dentro de una pared;
no encima de un objeto que bloquee su visualización.

IMPORTANTE:

No crear un segundo marcador.

Corregir el Transform/objetivo que ya utiliza el sistema de navegación.

El minimapa debe actualizarse automáticamente porque utiliza el mismo objetivo.

7. DIAGNÓSTICO DE MEMORIA

Actualmente, cuando aparece el diagnóstico de memoria, muestra información relacionada con:

RAM;
CPU;
Disco Duro.

Eso NO es lo que quiero.

Quiero que el diagnóstico sea exclusivamente sobre:

RAM

Debe comunicar únicamente el problema de memoria RAM.

Por ejemplo, mantener el concepto actual:

DIAGNÓSTICO

Memoria RAM insuficiente.

Disponible: 2 GB
Necesaria: 4 GB

O el formato equivalente que ya exista en el proyecto.

NO mencionar:

CPU;
procesador;
Disco Duro;
almacenamiento;
otros componentes.

La intención narrativa es:

El archivo necesita más memoria RAM para poder cargarse.

NO cambiar la lógica que detecta RamInsufficientDetected.

Solo corregir el contenido textual mostrado al jugador.

8. DURACIÓN DEL DIAGNÓSTICO

El diagnóstico actual desaparece demasiado rápido.

Aumentar su duración en pantalla en:

+2 segundos respecto a la duración actual.

IMPORTANTE:

No cambiar globalmente la duración de todos los feedbacks del juego.

Solo aumentar la duración del diagnóstico de RAM.

Si actualmente utiliza un método de feedback existente con duración configurable:

mantener el comportamiento general;
modificar únicamente la duración de este diagnóstico.

No afectar:

mensajes de shelves;
mensajes de objetivos;
pistas;
otros feedbacks;
FinalActivity.
9. NO TOCAR LA ROOM RAM

Aunque se van a mover puntos relacionados con RAM:

NO modificar:

RAM1;
RAM2;
RAM3;
RAM4;
Computer_Ram_Slot_3;
Computr_Ram_Slot_4;
slots;
modelos;
escalas;
rotaciones;
colliders.

RAM3 debe mantenerse exactamente donde está.

Solo colocar el punto de interacción/etiqueta correspondiente a su alrededor.

10. NO CAMBIAR LA LÓGICA EXISTENTE

No modificar innecesariamente:

ObjectiveSystem.cs
StorageMission.cs
FinalActivity.cs
PlayerInteraction.cs
EducationalInteractable.cs
InstallRamSlot.cs
CollectibleRam.cs
Inventory.cs
MissionNavigation.cs
MissionUI.cs
MinimapController.cs

Si alguno de estos scripts necesita un cambio mínimo para corregir específicamente la visibilidad o duración solicitada, primero inspeccionar si puede resolverse desde la escena/Inspector.

Preferir:

cambios de escena / Inspector

sobre cambios de código.

11. FLUJO QUE DEBE SEGUIR FUNCIONANDO

Después de las correcciones, comprobar este flujo:

CPU
 ↓
RAM
 ↓
Almacenamiento
 ↓
Buscar archivo
 ↓
Shelf correcto
 ↓
Server
 ↓
TV
 ↓
Enviar archivo
 ↓
CPU
 ↓
Procesamiento de archivo
 ↓
RAM
 ↓
Diagnóstico RAM insuficiente
 ↓
Bodega
 ↓
Recoger RAM
 ↓
Instalar RAM3/RAM4
 ↓
RAM3
 ↓
Ejecutar programa
 ↓
FinalActivity

La única diferencia debe ser que ahora los puntos/etiquetas están correctamente ubicados y el diagnóstico es más claro.

12. PRUEBAS OBLIGATORIAS

Probar en Play Mode.

Prueba 1

Comenzar una partida nueva.

Confirmar:

"Procesamiento de archivo" NO aparece.
Prueba 2

Completar CPU/RAM y llegar al almacenamiento.

Confirmar:

marcador amarillo del Disco Duro está correctamente ubicado;
etiquetas superiores están correctamente ubicadas.
Prueba 3

Buscar archivo.

Confirmar:

no aparece procesamiento antes de enviarlo.
Prueba 4

Enviar archivo desde la TV.

Confirmar:

ahora sí aparece/desbloquea el punto de procesamiento;
está junto a la Unidad de Control o ubicación CPU seleccionada.
Prueba 5

Llegar a RAM.

Confirmar:

"Cargar archivo en la memoria" está correctamente ubicado junto a RAM1.
Prueba 6

Provocar el diagnóstico.

Confirmar:

solo habla de RAM;
NO menciona CPU;
NO menciona Disco Duro;
permanece visible 2 segundos más que antes.
Prueba 7

Instalar RAM3/RAM4.

Confirmar:

no se movieron;
aparecen correctamente.
Prueba 8

Ejecutar programa.

Confirmar:

el punto [E] Ejecutar programa está junto a RAM3;
la interacción sigue funcionando;
FinalActivity aparece normalmente.
Prueba 9 — Regresión

Comprobar:

minimapa;
MissionUI;
marcador amarillo;
objetivos;
pistas;
CPU;
RAM;
bodega;
almacenamiento;
FinalActivity.

Todo debe seguir funcionando como antes.

13. REGLAS DE SEGURIDAD DEL CAMBIO

NO:

rehacer el mapa;
mover las habitaciones nuevamente;
modificar la escala de rooms;
modificar la room RAM;
crear nuevos sistemas de navegación;
crear nuevos marcadores;
crear nuevas etiquetas;
cambiar la progresión;
cambiar las actividades;
cambiar preguntas;
cambiar respuestas;
cambiar el inventario;
cambiar el sistema de misión.

Este prompt es exclusivamente de:

CORRECCIÓN DE POSICIONES + VISIBILIDAD + TEXTO DEL DIAGNÓSTICO + DURACIÓN DEL DIAGNÓSTICO.

INFORME FINAL

Al terminar, entregar:

1. Posiciones corregidas

Indicar qué objetos/puntos fueron movidos:

CPU label;
RAM label;
Bodega label;
Disco Duro label;
marcador amarillo Disco Duro;
Procesamiento de archivo;
Cargar archivo en memoria;
Ejecutar programa.
2. Diagnóstico

Indicar:

texto final;
duración anterior;
duración nueva.
3. Archivos modificados

Lista exacta.

4. Cambios de código

Si hubo cambios de código, explicar exactamente por qué fueron necesarios.

5. Pruebas

Confirmar que el flujo completo fue probado.

6. Errores

Indicar cualquier error o warning nuevo.

7. NO HACER COMMIT

No hacer commit.

Dejar los cambios listos para revisión.