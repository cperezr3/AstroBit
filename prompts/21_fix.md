# PROMPT 21 — ACTIVACIÓN DE RAM3/RAM4 EN SLOTS EXISTENTES

## PROYECTO

AstroBit — Unity

---

# CONTEXTO

El Prompt 20 ya fue implementado y probado personalmente.

La jugabilidad actual funciona correctamente:

CPU → RAM → almacenamiento → buscar archivo → procesarlo → detectar RAM insuficiente → ir a bodega → recoger módulos RAM → inventario → volver a RAM → instalar RAM → ejecutar programa → FinalActivity.

NO quiero modificar ni rehacer esa jugabilidad.

Este prompt es exclusivamente para corregir la representación visual/interacción de la instalación de las RAM adicionales.

---

# OBJETIVO

En la Room RAM ya existen físicamente:

- 2 slots nuevos para RAM.
- Los slots están representados mediante cubos/objetos ya colocados en la escena.
- Ya añadí físicamente dos módulos de RAM nuevos:
  - `RAM3`
  - `RAM4`

Estos módulos `RAM3` y `RAM4` ya están colocados exactamente donde quiero que aparezcan.

Actualmente están desactivados.

Quiero aprovechar esos objetos existentes.

NO quiero utilizar visualmente las pequeñas RAM que están en la bodega para representar la RAM instalada.

Las RAM de la bodega deben seguir funcionando únicamente como objetos que el jugador recoge y que incrementan el inventario.

Cuando el jugador instale las RAM adicionales:

```text
RAM recogida de la bodega
↓
inventario
↓
volver a Room RAM
↓
interactuar con Computer_Ram_Slot_3
↓
activar RAM3
y:
RAM recogida de la bodega
↓
inventario
↓
volver a Room RAM
↓
interactuar con Computer_Ram_Slot_4
↓
activar RAM4
________________________________________
REGLA PRINCIPAL
NO MOVER RAM3 NI RAM4
Los objetos:
RAM3
RAM4
ya están correctamente ubicados en la escena.
NO cambiar:
•	posición; 
•	rotación; 
•	escala; 
•	modelo; 
•	mesh; 
•	materiales. 
Simplemente deben permanecer desactivados hasta que corresponda activarlos.
Cuando se instalen:
RAM3.SetActive(true)
RAM4.SetActive(true)
o utilizar el mecanismo equivalente si la arquitectura actual lo requiere.
________________________________________
NUEVAS INTERACCIONES
Ya coloqué dos computadoras específicamente para realizar esta acción:
Computer_Ram_Slot_3
Computer_Ram_Slot_4
Estas son las interacciones que deben utilizarse.
________________________________________
COMPUTER_RAM_SLOT_3
Cuando el jugador tenga al menos una RAM disponible en el inventario y la misión haya llegado al punto de instalación:
mostrar:
[E] Instalar RAM
o el texto equivalente que ya utilice el sistema actual.
Al presionar [E]:
1.	comprobar que el jugador tenga una RAM disponible; 
2.	consumir 1 RAM del inventario; 
3.	activar RAM3; 
4.	marcar el slot 3 como instalado; 
5.	ocultar/deshabilitar la interacción de instalación de ese slot; 
6.	mantener RAM3 visible permanentemente durante la sesión. 
NO mover ninguna RAM.
NO instanciar la RAM de la bodega.
NO reutilizar físicamente el GameObject de la RAM recogida.
La RAM recogida solamente representa el componente que el jugador obtuvo y que ahora está siendo instalado.
La representación visual instalada será RAM3.
________________________________________
COMPUTER_RAM_SLOT_4
Mismo comportamiento.
Cuando el jugador tenga una RAM disponible:
[E] Instalar RAM
Al interactuar:
1.	comprobar inventario; 
2.	consumir 1 RAM; 
3.	activar RAM4; 
4.	marcar el slot 4 como instalado; 
5.	deshabilitar la interacción de instalación; 
6.	mantener RAM4 visible durante la sesión. 
NO mover RAM4.
NO mover la RAM de la bodega.
NO instanciar copias.
________________________________________
ESTADO INICIAL
Al comenzar la sesión:
RAM3 = inactive
RAM4 = inactive
Los objetos ya existen en la escena.
NO crear nuevos modelos.
NO crear nuevos slots.
NO crear nuevos cubos.
NO crear nuevos puntos de instalación.
________________________________________
ESTADO DESPUÉS DE INSTALAR RAM3
Debe quedar:
RAM3 = active
RAM4 = inactive
Y el inventario debe haber disminuido:
RAM x2
↓
RAM x1
________________________________________
ESTADO DESPUÉS DE INSTALAR RAM4
Debe quedar:
RAM3 = active
RAM4 = active
Inventario:
RAM x0
Y el sistema actual debe reconocer que las dos RAM adicionales fueron instaladas.
________________________________________
MUY IMPORTANTE: CONSERVAR LA LÓGICA DEL PROMPT 20
No romper el gating existente.
Actualmente la ejecución del programa depende de que los módulos necesarios estén instalados.
Mantener esa lógica.
Después de:
RAM3 = active
RAM4 = active
la condición equivalente a:
RamModulesFullyInstalled
debe continuar siendo verdadera y debe desbloquear la ejecución del programa exactamente como funciona actualmente.
NO cambiar la lógica de:
•	StorageMission; 
•	FinalActivity; 
•	ObjectiveSystem; 
salvo que sea estrictamente necesario para adaptar la representación de los slots.
________________________________________
ELIMINAR ETIQUETAS NO DESEADAS
NO quiero etiquetas:
RAM SLOT 1
RAM SLOT 2
RAM SLOT 3
RAM SLOT 4
en estos nuevos puntos de instalación.
Especialmente:
RAM Slot 1
RAM Slot 2
no deben aparecer.
Los dos slots que ya existen físicamente mediante cubos tampoco necesitan etiquetas.
________________________________________
COMPUTER_RAM_SLOT_3 Y 4
Las computadoras:
Computer_Ram_Slot_3
Computer_Ram_Slot_4
son los puntos donde el jugador interactúa.
Si actualmente tienen o generan WorldLabel, revisar si el label está causando el texto de:
RAM Slot 3
RAM Slot 4
No quiero esos textos.
Si es necesario:
•	eliminar el componente de etiqueta; 
•	desactivar su generación; 
•	cambiar su configuración. 
Pero no modificar innecesariamente otros labels del mapa.
________________________________________
ELIMINAR "CARGA EN MEMORIA"
Tampoco quiero la etiqueta o mensaje permanente:
Carga en Memoria
asociado a estos nuevos slots.
No crear ningún nuevo label con ese texto.
Si ya existe un objeto/script que lo genera específicamente para esta interacción, eliminarlo o desactivarlo.
NO modificar otros textos educativos que ya funcionan correctamente.
________________________________________
IMPORTANTE SOBRE LAS RAM DE LA BODEGA
Las RAM pequeñas de la bodega:
RAM1_Bodega
RAM2_Bodega
RAM3_Bodega
o los nombres equivalentes que existan actualmente:
DEBEN MANTENERSE.
Siguen siendo los objetos que el jugador recoge.
NO reemplazarlos.
NO escalarlos.
NO moverlos.
NO utilizarlos como RAM visual instalada.
Su función es:
BODEGA
↓
recoger RAM
↓
inventario RAM +1
Nada más.
________________________________________
REPRESENTACIÓN FINAL
La lógica visual debe quedar así:
Antes de recoger
Bodega:
RAM de repuesto
RAM de repuesto
RAM de repuesto
Room RAM:
RAM1       RAM2

[slot]     [slot]

RAM3       RAM4
OFF        OFF
________________________________________
Después de recoger 2 RAM
Inventario:
RAM x2
Room RAM:
RAM1       RAM2

[slot]     [slot]

RAM3       RAM4
OFF        OFF
________________________________________
Instalar primera RAM
Interacción:
Computer_Ram_Slot_3
[E] Instalar RAM
Resultado:
RAM3 = ON
RAM4 = OFF
Inventario:
RAM x1
________________________________________
Instalar segunda RAM
Interacción:
Computer_Ram_Slot_4
[E] Instalar RAM
Resultado:
RAM3 = ON
RAM4 = ON
Inventario:
RAM x0
________________________________________
ETIQUETAS
Quiero que las etiquetas de los componentes principales existentes sigan funcionando normalmente.
NO hacer una limpieza global de WorldLabel.
NO eliminar etiquetas de:
•	ALU; 
•	Registros; 
•	Unidad de Control; 
•	Cache; 
•	RAM1; 
•	RAM2; 
•	Shelves; 
•	Server; 
•	TV; 
•	otros componentes que ya funcionan. 
Solamente eliminar/desactivar las etiquetas relacionadas con:
RAM SLOT 1
RAM SLOT 2
RAM SLOT 3
RAM SLOT 4
Carga en Memoria
si existen específicamente para esta mecánica.
________________________________________
INSPECCIÓN OBLIGATORIA
Antes de modificar código:
inspeccionar:
•	StorageMission.cs 
•	Inventory.cs 
•	CollectibleRam.cs 
•	InstallRamSlot.cs 
•	GameHUD.cs 
•	WorldLabel.cs 
•	SampleScene.unity 
Y buscar específicamente en la escena:
RAM3
RAM4
Computer_Ram_Slot_3
Computer_Ram_Slot_4
Confirmar sus posiciones, rotaciones y componentes actuales.
NO asumir que los nombres de componentes son exactamente los esperados.
________________________________________
IMPORTANTE SOBRE INSTALLRAMSLOT
Ya existe:
InstallRamSlot.cs
creado en el Prompt 20.
No crear otro sistema de instalación si el actual puede adaptarse limpiamente.
La prioridad es modificarlo para que:
Computer_Ram_Slot_3
active:
RAM3
y:
Computer_Ram_Slot_4
active:
RAM4
Si la arquitectura actual de InstallRamSlot permite hacerlo mediante referencias [SerializeField], utilizar ese mecanismo.
Por ejemplo, conceptualmente:
InstallRamSlot
├── requiredItem = RAM
├── visualObject = RAM3
└── slotId = 3
y:
InstallRamSlot
├── requiredItem = RAM
├── visualObject = RAM4
└── slotId = 4
Pero no es obligatorio utilizar exactamente esos campos.
________________________________________
NO TOCAR
No modificar:
•	preguntas de FinalActivity; 
•	funcionamiento de FinalActivity; 
•	CPU; 
•	ALU; 
•	Registros; 
•	Unidad de Control; 
•	Cache L1; 
•	Cache L2; 
•	Cache L3; 
•	RAM1; 
•	RAM2; 
•	Shelves; 
•	Server; 
•	TV 32 Inch; 
•	búsqueda del archivo; 
•	procesamiento del archivo; 
•	diagnóstico de RAM insuficiente; 
•	inventario salvo lo estrictamente necesario para consumir la RAM; 
•	cámara; 
•	movimiento; 
•	Input System; 
•	mapa; 
•	iluminación; 
•	materiales. 
________________________________________
PRUEBAS OBLIGATORIAS
Después de realizar los cambios, probar en Play Mode.
PRUEBA 1 — Estado inicial
Confirmar:
RAM3 invisible
RAM4 invisible
Y:
RAM SLOT 1
RAM SLOT 2
RAM SLOT 3
RAM SLOT 4
Carga en Memoria
NO deben aparecer como etiquetas.
________________________________________
PRUEBA 2 — Recoger RAM
Confirmar que las RAM de la bodega siguen funcionando.
RAM x1
RAM x2
________________________________________
PRUEBA 3 — Instalar RAM3
Ir a:
Computer_Ram_Slot_3
Interactuar con [E].
Confirmar:
RAM3 visible
RAM4 invisible
Inventario RAM x1
RAM3 debe aparecer exactamente en la posición donde ya fue colocada.
________________________________________
PRUEBA 4 — Instalar RAM4
Ir a:
Computer_Ram_Slot_4
Interactuar con [E].
Confirmar:
RAM3 visible
RAM4 visible
Inventario RAM x0
________________________________________
PRUEBA 5 — Posiciones
Confirmar visualmente:
•	RAM3 no se movió; 
•	RAM4 no se movió; 
•	no se cambiaron sus escalas; 
•	no se cambiaron sus rotaciones; 
•	no atraviesan los slots; 
•	aparecen exactamente donde fueron colocadas manualmente. 
________________________________________
PRUEBA 6 — Gating
Confirmar:
Sin RAM en inventario
→ Computer_Ram_Slot_3 no permite instalar.

Con 1 RAM
→ Slot 3 permite instalar.

Después de instalar RAM3
→ Slot 3 deja de ofrecer instalación.

Con 1 RAM restante
→ Slot 4 permite instalar.

Después de instalar RAM4
→ Slot 4 deja de ofrecer instalación.
________________________________________
PRUEBA 7 — Ejecución
Después de activar:
RAM3
RAM4
confirmar que el flujo existente continúa:
RAM instalada 2/2
↓
ejecución desbloqueada
↓
programa ejecutado
↓
FinalActivity
No modificar FinalActivity.
________________________________________
CRITERIO DE ÉXITO
El prompt está terminado cuando:
1.	Las RAM pequeñas de la bodega siguen siendo recogibles. 
2.	El inventario sigue funcionando. 
3.	Computer_Ram_Slot_3 instala/activa RAM3. 
4.	Computer_Ram_Slot_4 instala/activa RAM4. 
5.	RAM3 y RAM4 conservan exactamente sus posiciones actuales. 
6.	No se instancian ni mueven las RAM de la bodega. 
7.	No aparecen etiquetas RAM SLOT 1/2/3/4. 
8.	No aparece la etiqueta Carga en Memoria. 
9.	Las dos RAM activadas cuentan correctamente para RamModulesFullyInstalled. 
10.	El programa vuelve a poder ejecutarse después de instalar ambas. 
11.	No se rompe ninguna parte del flujo del Prompt 20. 
12.	No aparecen errores ni warnings nuevos. 
________________________________________
REGLA FINAL
Este es un prompt de corrección puntual.
No agregar nuevas mecánicas.
No agregar nuevas salas.
No agregar nuevas interfaces.
No agregar nuevas etiquetas.
No agregar nuevas animaciones.
No cambiar la jugabilidad que ya fue probada y aprobada.
La única modificación importante es:
RAM recogida de bodega
        ↓
     inventario
        ↓
Computer_Ram_Slot_3 → activa RAM3
Computer_Ram_Slot_4 → activa RAM4
Las RAM3 y RAM4 ya existen y ya están correctamente colocadas en la escena.
El trabajo consiste en hacer que se activen mediante [E] en sus respectivas computadoras y eliminar las etiquetas visuales que no quiero.

