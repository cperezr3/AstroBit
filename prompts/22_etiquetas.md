# PROMPT 22 — Limpieza de etiquetas y etiquetas dinámicas de RAM3/RAM4

## PROYECTO

AstroBit — Unity

---

# CONTEXTO

El Prompt 21 fue implementado y probado personalmente.

Todo el flujo actual funciona correctamente:

CPU → RAM → almacenamiento → archivo → TV → CPU → RAM insuficiente → bodega → recoger RAM → instalar RAM3/RAM4 → ejecutar programa → FinalActivity.

NO quiero modificar esa jugabilidad.

Este prompt es EXCLUSIVAMENTE para corregir/eliminar etiquetas visuales y el contador de progreso.

No agregar nuevas mecánicas.

No modificar la lógica del recorrido.

No modificar FinalActivity.

No modificar ObjectiveSystem salvo lo estrictamente necesario para ocultar el contador al llegar a 8/8.

---

# CAMBIOS SOLICITADOS

Son únicamente estos cuatro grupos:

1. Eliminar la etiqueta/texto "Ejecución El programa en memoria puede..."
2. Ocultar `X/8` cuando llegue a `8/8`.
3. Mostrar etiquetas en RAM3 y RAM4 solamente cuando sean activadas.
4. Eliminar la etiqueta de `RAM1_Bodega` y hacer que las RAM recogidas de la bodega pierdan su etiqueta al recogerse.

---

# 1. ELIMINAR "EJECUCIÓN"

Existe actualmente una etiqueta/mensaje relacionado con la ejecución que muestra algo similar a:

```text
Ejecución

El programa en memoria puede...
El texto además aparece cortado/incompleto.
NO quiero ese texto.
Eliminar/desactivar únicamente esa etiqueta específica.
La interacción de ejecución debe seguir funcionando exactamente igual.
Es decir:
RAM3 instalada
+
RAM4 instalada
↓
Ejecutar programa
↓
FinalActivity
NO tocar esa lógica.
NO eliminar el punto de interacción que permite ejecutar el programa.
NO modificar el texto de otros componentes.
NO modificar la actividad final.
________________________________________
2. OCULTAR EL CONTADOR X/8 AL COMPLETAR LA CPU/RAM
Actualmente el HUD muestra:
0/8
1/8
2/8
...
8/8
El contador funciona correctamente.
Quiero conservarlo durante la fase de aprendizaje.
Pero cuando llegue a:
8/8
debe desaparecer completamente.
Es decir:
7/8
→ visible.
Después de completar el octavo componente:
8/8
→ NO debe permanecer visible.
Simplemente ocultar el ProgressText.
________________________________________
Comportamiento esperado
Al comenzar:
0/8
visible.
Mientras el jugador aprende:
1/8
2/8
3/8
...
7/8
visible.
Al completar el último componente:
8/8
el texto debe ocultarse.
NO quiero que se quede mostrando:
8/8
en la esquina.
________________________________________
Importante
No eliminar el sistema:
CompletedSteps
TotalSteps
OnObjectiveCompleted
ni romper la lógica que utiliza StorageMission para detectar que se completaron los componentes.
El contador solamente debe dejar de mostrarse visualmente.
Si actualmente GameHUD actualiza ProgressText desde ObjectiveSystem, hacer el cambio de forma mínima.
________________________________________
3. ETIQUETA DINÁMICA PARA RAM3 Y RAM4
En la Room RAM existen actualmente:
RAM3
RAM4
Ambas empiezan desactivadas.
Cuando el jugador las instala mediante:
Computer_Ram_Slot_3
Computr_Ram_Slot_4
se activan correctamente.
Ahora quiero que, además, al activarse aparezca su etiqueta educativa correspondiente.
________________________________________
RAM3
Cuando:
RAM3 = inactive
NO debe aparecer ninguna etiqueta.
Cuando el jugador instala RAM3:
Computer_Ram_Slot_3
[E]
↓
RAM3.SetActive(true)
debe aparecer una etiqueta encima de RAM3 con exactamente:
RAM3
Módulo de memoria RAM
o, si el sistema actual utiliza una sola línea de título y subtítulo:
RAM3
Módulo de memoria RAM
La idea es que visualmente quede:
RAM3
Módulo de memoria RAM
________________________________________
RAM4
Mismo comportamiento.
Antes de instalar:
RAM4 = inactive
No debe existir etiqueta visible.
Después de:
Computr_Ram_Slot_4
[E]
↓
RAM4.SetActive(true)
debe aparecer:
RAM4
Módulo de memoria RAM
________________________________________
IMPORTANTE: NO CREAR OTRA RAM
No crear:
•	otra RAM3; 
•	otra RAM4; 
•	copias; 
•	prefabs nuevos; 
•	instancias. 
Utilizar exactamente los GameObjects existentes:
RAM3
RAM4
que ya están correctamente colocados en los slots.
________________________________________
POSICIÓN DE LAS ETIQUETAS
Las etiquetas deben estar asociadas a sus respectivas RAM:
RAM3
  ↑
 etiqueta RAM3
y:
RAM4
  ↑
 etiqueta RAM4
No quiero etiquetas flotando en posiciones arbitrarias.
Utilizar el mismo mecanismo de WorldLabel que ya existe en el proyecto si es apropiado.
No modificar la posición de RAM3 ni RAM4.
No modificar:
•	posición; 
•	rotación; 
•	escala; 
•	mesh; 
•	materiales. 
Solamente trabajar con la etiqueta.
________________________________________
4. RAM1_BODEGA
En la bodega existen varias RAM.
Una de ellas específicamente:
RAM1_Bodega
es la RAM que está inclinada.
Quiero quitarle su etiqueta.
Debe quedar:
RAM1_Bodega
sin ningún texto encima.
NO eliminar el objeto.
NO desactivarlo.
NO quitarle su capacidad de interacción.
Debe seguir funcionando exactamente como antes:
RAM1_Bodega
↓
[E]
↓
recoger RAM
↓
inventario +1
↓
objeto desaparece
Solamente eliminar su WorldLabel o desactivar su generación/visualización.
________________________________________
5. ETIQUETAS DE LAS RAM RECOGIDAS
Actualmente las RAM de la bodega pueden tener etiquetas.
Quiero que, cuando el jugador recoja una RAM, la etiqueta correspondiente desaparezca junto con el objeto.
Por ejemplo:
RAM_Bodega
Etiqueta RAM_Bodega
Al pulsar [E]:
RAM_Bodega → SetActive(false)
Etiqueta → desaparece
Inventario → RAM +1
NO quiero que quede una etiqueta flotando en el lugar donde estaba la RAM.
________________________________________
IMPORTANTE
Esto debe aplicarse a las RAM que el jugador realmente recoge.
No debe afectar a:
RAM3
RAM4
porque esas tienen otro comportamiento:
RAM3/RAM4
inactive
↓
instalación
↓
active
↓
aparece su etiqueta educativa
________________________________________
COMPORTAMIENTO FINAL ESPERADO
Inicio
CPU/RAM:
ALU
Registros
Unidad de Control
Cache L1
Cache L2
Cache L3
RAM1
RAM2
El contador:
0/8
está visible.
RAM3/RAM4:
invisibles
sin etiqueta
Bodega:
RAM1_Bodega
sin etiqueta.
No existe:
Ejecución
El programa en memoria puede...
________________________________________
DURANTE EL APRENDIZAJE
Contador:
1/8
2/8
3/8
...
7/8
visible.
Las etiquetas educativas normales siguen funcionando.
NO modificar las etiquetas de los componentes existentes.
________________________________________
AL COMPLETAR EL OCTAVO COMPONENTE
El contador llega internamente a:
8/8
pero visualmente debe:
DESAPARECER
La progresión interna debe continuar funcionando.
Esto es solamente un cambio visual.
________________________________________
DESPUÉS DE INSTALAR RAM3
Debe aparecer:
RAM3
Módulo de memoria RAM
asociado visualmente a RAM3.
________________________________________
DESPUÉS DE INSTALAR RAM4
Debe aparecer:
RAM3
Módulo de memoria RAM

RAM4
Módulo de memoria RAM
cada una encima de su respectivo módulo.
________________________________________
NO TOCAR
NO modificar:
•	movimiento del jugador; 
•	cámara; 
•	PlayerInteraction; 
•	Input System; 
•	ObjectiveSystem salvo ocultar visualmente ProgressText; 
•	StorageMission salvo que sea estrictamente necesario; 
•	Inventory; 
•	CollectibleRam salvo para corregir la desaparición de su etiqueta si realmente es necesario; 
•	InstallRamSlot salvo para activar la etiqueta de RAM3/RAM4; 
•	FileShelf; 
•	StorageServer; 
•	TV; 
•	MissionStepPoint; 
•	CPU; 
•	ALU; 
•	Registros; 
•	Unidad de Control; 
•	Cache L1; 
•	Cache L2; 
•	Cache L3; 
•	RAM1; 
•	RAM2; 
•	FinalActivity; 
•	preguntas de FinalActivity; 
•	lógica de ejecución; 
•	lógica de instalación de RAM; 
•	lógica de inventario. 
No cambiar nombres de objetos existentes.
No mover objetos.
No crear nuevos sistemas.
________________________________________
INSPECCIÓN OBLIGATORIA
Antes de modificar:
revisar cómo están implementados actualmente:
WorldLabel.cs
GameHUD.cs
ObjectiveSystem.cs
CollectibleRam.cs
InstallRamSlot.cs
StorageMission.cs
y revisar en:
SampleScene.unity
los objetos:
RAM1_Bodega
RAM3
RAM4
Computer_Ram_Slot_3
Computr_Ram_Slot_4
ProgressText
Identificar exactamente qué componente/script está generando cada etiqueta antes de modificarlo.
NO asumir que todas las etiquetas utilizan exactamente la misma implementación.
________________________________________
REGLA IMPORTANTE SOBRE WORLDLABEL
No hacer una eliminación global de WorldLabel.
El proyecto ya tiene etiquetas correctas en muchos componentes.
Solamente modificar las etiquetas indicadas en este prompt:
❌ Ejecución / El programa en memoria puede...
❌ RAM1_Bodega
❌ etiquetas de las RAM de bodega después de recogerlas
❌ cualquier etiqueta RAM SLOT que todavía exista específicamente en estos puntos
Y agregar:
✅ RAM3 — Módulo de memoria RAM
✅ RAM4 — Módulo de memoria RAM
después de su activación.
________________________________________
PRUEBAS OBLIGATORIAS
Realizar Play Mode real.
PRUEBA 1 — Contador
Completar progresivamente los componentes.
Confirmar:
7/8
visible.
Completar el octavo.
Confirmar:
8/8
NO queda visible.
________________________________________
PRUEBA 2 — RAM1_Bodega
Antes de recogerla:
RAM1_Bodega
NO tiene etiqueta.
Pero sigue pudiendo interactuarse con [E].
________________________________________
PRUEBA 3 — RAM recogida
Recoger una RAM de la bodega.
Confirmar:
objeto desaparece
etiqueta desaparece
inventario aumenta
No queda ningún texto flotando en su posición anterior.
________________________________________
PRUEBA 4 — RAM3
Antes:
RAM3 = inactive
sin etiqueta
Interactuar con:
Computer_Ram_Slot_3
Después:
RAM3 = active
RAM3 visible
y aparece:
RAM3
Módulo de memoria RAM
________________________________________
PRUEBA 5 — RAM4
Antes:
RAM4 = inactive
sin etiqueta
Interactuar con:
Computr_Ram_Slot_4
Después:
RAM4 = active
RAM4 visible
y aparece:
RAM4
Módulo de memoria RAM
________________________________________
PRUEBA 6 — Ejecución
Confirmar que eliminar la etiqueta:
Ejecución
El programa en memoria puede...
NO rompe la interacción de ejecución.
El jugador todavía debe poder:
RAM instalada
↓
Ejecutar programa
↓
FinalActivity
________________________________________
CRITERIO DE ÉXITO
El prompt está terminado cuando:
1.	La etiqueta "Ejecución — El programa en memoria puede..." ya no aparece. 
2.	La interacción de ejecución sigue funcionando. 
3.	ProgressText funciona de 0/8 a 7/8. 
4.	Al completar el octavo componente, el contador desaparece. 
5.	RAM1_Bodega no tiene etiqueta. 
6.	Al recoger cualquier RAM de la bodega, su etiqueta desaparece junto con el objeto. 
7.	RAM3 empieza invisible y sin etiqueta. 
8.	RAM4 empieza invisible y sin etiqueta. 
9.	Al activar RAM3 aparece:
RAM3 — Módulo de memoria RAM. 
10.	Al activar RAM4 aparece:
RAM4 — Módulo de memoria RAM. 
11.	RAM3 y RAM4 conservan exactamente sus posiciones actuales. 
12.	No se crean copias de RAM. 
13.	No se altera el inventario. 
14.	No se altera el gating de instalación. 
15.	No se altera FinalActivity. 
16.	No se altera ninguna etiqueta educativa existente que no esté mencionada en este prompt. 
17.	No aparecen errores ni warnings nuevos. 
________________________________________
REGLA FINAL
Este es un prompt de limpieza y corrección visual puntual.
NO aprovechar este prompt para refactorizar sistemas.
NO agregar nuevas mecánicas.
NO mejorar otras etiquetas.
NO cambiar textos educativos existentes.
NO cambiar posiciones de objetos.
NO modificar la jugabilidad.
Hacer únicamente los cambios especificados arriba y comprobarlos en Play Mode antes de terminar.

