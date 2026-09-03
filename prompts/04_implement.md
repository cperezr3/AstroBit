# PROMPT — SIGUIENTE GRAN BLOQUE: DATA FLOW + COMPUTER ACTIVITY

Proyecto: **AstroBit — Unity 6 URP**

## CONTEXTO

El bloque anterior de **Polish Visual + Decoración + Game Feel** terminó correctamente.

La CPU ahora responde visualmente al aprendizaje:

* ALU
* Registros
* Unidad de Control
* Caché L1
* Caché L2
* Caché L3

se activan progresivamente cuando el jugador los comprende.

RAM1/RAM2 utilizan el mismo mecanismo.

RAM y Storage ya tienen identidad visual sólida.

El proyecto mantiene funcionando:

* `StorageMission`
* `MissionStepPoint`
* `MissionBeacon`
* `EducationalInteractable`
* `EmissiveToggle`
* `ObjectiveSystem`
* `GameHUD`
* `FinalActivity`
* Save/Load
* Settings
* Pause
* Music
* AmbientAudio
* Cinemachine
* PlayerInteraction

La compilación está limpia.

No existen `StorageMission` ghosts al finalizar el bloque anterior.

---

# OBJETIVO

Ahora quiero implementar el concepto que todavía falta para que AstroBit realmente comunique visualmente:

> **LOS DATOS SE MUEVEN POR LA COMPUTADORA.**

El objetivo conceptual es:

**STORAGE → RAM → CACHE → REGISTROS → ALU → CPU**

Pero quiero hacerlo con criterio.

NO quiero simplemente dibujar una línea gigante de partículas desde una sala hasta otra.

Quiero una representación visual:

* bonita;
* tecnológica;
* sutil;
* comprensible;
* integrada con el escenario;
* conectada con la progresión real.

Debe sentirse como parte de la computadora.

---

# 1. PRIMERO: AUDITA EL RECORRIDO REAL

Antes de implementar, inspecciona:

* Storage.
* corredor.
* RAM.
* CPU.
* posiciones reales de los componentes.
* cables existentes.
* luces.
* MissionBeacons.
* posibles puntos naturales para representar conexiones.

Determina cuál es la mejor ruta visual.

No asumas que debe ser una línea recta.

Puede ser mejor utilizar:

* cables existentes;
* conduits;
* tiras luminosas;
* puntos de conexión;
* pequeños nodos;
* segmentos por zonas.

La representación debe respetar la arquitectura visual existente.

---

# 2. DISEÑO DEL DATA FLOW

Quiero una representación por etapas.

Conceptualmente:

### ETAPA 1

**STORAGE → RAM**

Representar que los datos salen del almacenamiento y llegan a memoria.

### ETAPA 2

**RAM → CACHE**

Representar que la información pasa hacia una memoria más rápida.

### ETAPA 3

**CACHE → REGISTROS**

Representar el acercamiento de los datos al procesamiento inmediato.

### ETAPA 4

**REGISTROS → ALU**

Representar que los datos llegan a la unidad que los procesa.

### ETAPA 5

**ALU → CPU**

Representar el procesamiento completado.

No necesitas implementar una simulación informática real.

Es una representación educativa.

---

# 3. NO CREAR UNA SEGUNDA PROGRESIÓN

MUY IMPORTANTE:

El Data Flow debe depender de la progresión existente.

NO crear:

* otra misión;
* otro sistema de objetivos;
* otra lista de pasos;
* otro SaveManager;
* otro GameManager;
* otra progresión independiente.

Debe escuchar/reutilizar los sistemas existentes.

Si `StorageMission` ya representa el avance del jugador, úsalo.

Si `ObjectiveSystem` ya representa conceptos aprendidos, úsalo.

Si `MissionStepPoint` ya representa hitos físicos, reutilízalo.

---

# 4. REPRESENTACIÓN VISUAL

Investiga qué representación funciona mejor con los assets actuales.

Opciones válidas:

### A. Pulsos luminosos

Un pequeño pulso recorre una ruta.

Ejemplo:

`●────────────→●`

Pero visualmente integrado al mundo.

### B. Segmentos luminosos

La conexión está apagada inicialmente.

Cuando progresa la misión:

`○──○──○──○`

se convierte progresivamente en:

`●══●══●══●`

### C. Nodos + pulsos

Pequeños nodos tecnológicos conectados por líneas/cables.

### D. Cables existentes

Si ya existen cables verdes entre componentes, estudia si pueden convertirse en parte del sistema.

Esto sería especialmente bueno porque evitaría agregar geometría artificial.

---

# 5. PRIORIDAD: CALIDAD SOBRE CANTIDAD

No quiero cientos de partículas.

No quiero:

* Particle System gigantes;
* efectos excesivos;
* bloom exagerado;
* líneas atravesando paredes;
* partículas flotando sin propósito.

Quiero algo que parezca diseñado por un artista técnico.

Un jugador debería poder verlo y pensar:

> "Ah, los datos están viajando por aquí."

sin que el efecto destruya la escena.

---

# 6. PRUEBA PRIMERO UNA SOLA RUTA

Antes de implementar todo:

Construye primero una única demostración pequeña y controlada.

Preferiblemente:

**Storage → RAM**

o una sección equivalente que sea fácil de verificar.

Comprueba:

* posición;
* escala;
* velocidad;
* visibilidad;
* iluminación;
* integración con escenario;
* rendimiento;
* comportamiento al avanzar/repetir/cargar partida.

Si funciona bien, extiende el mismo patrón al resto.

NO construyas las cinco rutas de golpe y descubras al final que el efecto no se ve bien.

---

# 7. RELACIÓN CON LA PROGRESIÓN

El Data Flow debe tener estados.

Por ejemplo:

### Antes de completar una etapa

La conexión permanece:

* apagada;
* tenue;
* o con pequeños indicadores de espera.

### Cuando la etapa está activa

La conexión muestra:

* actividad;
* pulsos;
* movimiento.

### Cuando la etapa está completada

La conexión puede quedar:

* estable;
* ligeramente iluminada;
* indicando que ese flujo ya fue comprendido.

Esto debe funcionar también después de:

**Save → Load**

No debe volver accidentalmente al estado inicial.

---

# 8. CONECTARLO CON LA CPU

Aprovecha el sistema implementado en el bloque anterior.

La CPU ya se enciende progresivamente:

ALU → Registros → Unidad de Control → Cachés.

Ahora el Data Flow debería complementar ese comportamiento.

Ejemplo conceptual:

**Datos llegan → componente correspondiente se activa → jugador entiende la relación.**

No cambies la lógica educativa.

Haz que el efecto visual explique la lógica existente.

---

# 9. GAME FEEL

Añade pequeños detalles donde tengan sentido:

* pulsos;
* LEDs;
* cambios suaves de emisión;
* pequeños sonidos si existe un asset adecuado;
* transición entre estados;
* feedback de llegada.

Pero mantén la estética:

**futurista + limpia + tecnológica.**

No quiero un espectáculo de VFX.

---

# 10. RENDIMIENTO

Esto es especialmente importante.

Antes de llenar el mapa:

evalúa el coste.

Evita:

* Update por cada partícula;
* cientos de GameObjects animados;
* luces dinámicas innecesarias;
* materiales duplicados masivamente;
* Particle Systems sobredimensionados.

Si puedes representar el flujo mediante:

* pocos nodos;
* pocos segmentos;
* una animación controlada;
* materiales/emisión;

prefiere eso antes que una solución pesada.

---

# 11. TOPRIG

NO TOCAR.

La investigación anterior demostró que el cambio de Transform de `TopRig` es comportamiento interno irrelevante de Cinemachine.

No invertir tiempo en ello.

Si la cámara real funciona correctamente:

**ignorar TopRig.**

---

# 12. STORAGE MISSION GHOSTS

Antes de guardar una escena:

verifica que no existan objetos `StorageMission` duplicados creados accidentalmente durante pruebas.

No ejecutes `.Instance` de singletons mediante pruebas de Editor de forma que puedan quedar persistidos.

No guardes escenas contaminadas.

---

# 13. NO FORZAR ASSETS

No utilices:

`Cosmic_Retro_Computer_1_FREE`

si no encaja naturalmente.

Ya se determinó que no combina bien con la escala/estética actual.

No descargues assets externos.

No agregues dependencias externas.

Trabaja con lo que ya existe.

---

# 14. VALIDACIÓN

Al terminar:

1. Compila.
2. Ejecuta Play Mode.
3. Comprueba Storage → RAM.
4. Comprueba RAM → CPU.
5. Comprueba progresión.
6. Comprueba feedback visual.
7. Comprueba Save/Load.
8. Comprueba volver al menú.
9. Comprueba FinalActivity.
10. Revisa Console.
11. Comprueba ausencia de StorageMission ghosts.
12. Comprueba rendimiento razonable.
13. Verifica visualmente el resultado desde la cámara real del jugador.

Si algo se rompe, corrígelo dentro de este bloque.

---

# 15. CRITERIO DE ÉXITO

No consideres terminado el bloque porque:

> "hay partículas moviéndose."

Debe cumplir:

> **El jugador puede interpretar visualmente que la información está recorriendo el sistema.**

Y además:

* encaja con el escenario;
* no parece pegado artificialmente;
* no molesta;
* no rompe la estética;
* está conectado a la progresión;
* sobrevive correctamente a Save/Load;
* tiene buen rendimiento.

Si después de probar una representación literal de partículas concluyes que otra representación comunica mejor el concepto, usa la mejor solución.

No estás obligado a usar partículas.

---

# 16. INFORME FINAL

Al terminar entrega:

1. Qué auditaste.
2. Qué representación elegiste.
3. Por qué la elegiste.
4. Qué rutas implementaste.
5. Qué scripts modificaste.
6. Qué objetos/escenas modificaste.
7. Cómo se conecta con `StorageMission` / `ObjectiveSystem`.
8. Cómo funciona con Save/Load.
9. Validación visual.
10. Validación de gameplay.
11. Estado de compilación.
12. Estado de Console.
13. Estado de rendimiento.
14. Estado de Git/diff.
15. Qué quedó pendiente.
16. Recomendación del siguiente gran bloque.

Trabaja autónomamente.

No me pidas aprobación para cada decisión.

Primero prueba una ruta pequeña y, si funciona, escala el sistema al resto.
